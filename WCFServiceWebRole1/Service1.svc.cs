using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace WCFServiceWebRole1
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
	public class Service1 : IService1
	{

		/************************************************************
		 * Return a single Student from the database given a specific
		 * student ID. If the student doesn't exist it returns null.
		*************************************************************/
		public Student getStudent(string studentId)
		{
			Student student = new Student();
			try
			{
				DataTable data = DatabaseQuery.selectQuery(String.Format("SELECT * FROM skrohn_gradetracker.students WHERE student_id={0}", studentId));
				if(data.Rows.Count > 0) 
				{
					student.firstName = data.Rows[0][1].ToString();
					student.lastName = data.Rows[0][1].ToString();
					student.id = data.Rows[0][2].ToString();
					student.username = data.Rows[0][3].ToString();
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				return null;
			}
			return student;
		}

		/************************************************************
		 * Returns a List<Student> for every student found in the
		 * database.
		************************************************************/
		public List<Student> getAllStudents()
		{
			List<Student> allStudents = new List<Student>();
			try
			{
				DataTable data = DatabaseQuery.selectQuery(String.Format("SELECT * FROM skrohn_gradetracker.students"));
				for (int i = 0; i < data.Rows.Count; i++)
				{
					Student foundStudent = new Student();
					foundStudent.firstName = data.Rows[i][0].ToString();
					foundStudent.lastName= data.Rows[i][1].ToString();
					foundStudent.id = data.Rows[i][2].ToString();
					foundStudent.username = data.Rows[i][3].ToString();
					allStudents.Add(foundStudent);
				}
			}
			catch (Exception ex)
			{
				return null;
			}
			return allStudents;
		}

		/************************************************************
		 * Add a new student to the database.
		************************************************************/
		public bool addStudent(string firstName, string lastName, string studentId, string username, string hashPw)
		{
			string queryString = String.Format("INSERT INTO skrohn_gradetracker.students (first_name, last_name, student_id, username, hash_pw) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\")", firstName, lastName, studentId, username, hashPw);

			return DatabaseQuery.executeNonQuery(queryString);
		}

		/************************************************************
		 * Return a Semester object given a semester ID.
		************************************************************/
		public Semester getSemester(int semesterId)
		{
			Semester semester = new Semester();
			try
			{
				DataTable data = DatabaseQuery.selectQuery(String.Format("SELECT * FROM skrohn_gradetracker.semesters WHERE semester_id={0}", semesterId));
				if (data.Rows.Count > 0)
				{
					semester.assocStudentId = data.Rows[0][0].ToString();
					semester.termName = data.Rows[0][1].ToString();
					semester.termYear = data.Rows[0][2].ToString();
					semester.semesterId = data.Rows[0][3].ToString();
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				semester.termName = ex.Message;
			}
			return semester;
		}

		/************************************************************
		 * Return all semesters associated with a specific student ID.
		************************************************************/
		public List<Semester> getAllSemestersForStudent(string studentId)
		{
			List<Semester> allSemesters = new List<Semester>();
			try
			{
				DataTable data = DatabaseQuery.selectQuery(String.Format("SELECT * FROM skrohn_gradetracker.semesters WHERE assoc_student_id={0}", studentId));
				for (int i = 0; i < data.Rows.Count; i++)
				{
					Semester foundSemester = new Semester();
					foundSemester.assocStudentId = data.Rows[i][0].ToString();
					foundSemester.termName = data.Rows[i][1].ToString();
					foundSemester.termYear = data.Rows[i][2].ToString();
					foundSemester.semesterId= data.Rows[i][3].ToString();
					allSemesters.Add(foundSemester);
				}
			}
			catch (Exception ex)
			{
				return null;
			}
			return allSemesters;
		}

		/************************************************************
		 * Add a new semester and associate it with a particular
		 * student ID.
		************************************************************/
		public bool addSemester(string studentId, string termName, string termYear)
		{
			// Don't dheck if this Semester already exists, allow user to enter duplicate semesters.
			string query = String.Format("INSERT INTO skrohn_gradetracker.semesters (assoc_student_id, term_name, term_year) VALUES (\"{0}\", \"{1}\", \"{2}\")", studentId, termName, termYear);
			return DatabaseQuery.executeNonQuery(query);
		}

		/************************************************************
		 * Return all Courses associated with a particular semester. 
		************************************************************/
		public List<Course> getAllCoursesForSemester(int semesterId)
		{
			List<Course> courses = new List<Course>();
			try 
			{
				DataTable data = DatabaseQuery.selectQuery(String.Format("SELECT * FROM skrohn_gradetracker.courses WHERE assoc_semester_id={0}", semesterId));
				for(int i = 0; i < data.Rows.Count; i++)
				{
					Course foundCourse = new Course();
					foundCourse.courseId = Convert.ToInt32(data.Rows[i][0]);
					foundCourse.assocSemesterId = Convert.ToInt32(data.Rows[i][1]);
					foundCourse.courseCode = data.Rows[i][2].ToString();
					foundCourse.courseNumber = data.Rows[i][3].ToString();
					DataTable categories = DatabaseQuery.selectQuery(String.Format("SELECT * FROM skrohn_gradetracker.weights WHERE assoc_course_id={0}", foundCourse.courseId));
					
					// Insert categories/weights into Course objects dictionary of categories
					foundCourse.categories = new Dictionary<string,double>();
					foreach (DataRow row in categories.Rows)
					{
						foundCourse.categories[row[1].ToString()] = Convert.ToDouble(row[2]);
					}

					courses.Add(foundCourse);
				}
			}
			catch(Exception ex) 
			{
				return null;
			}


			return courses;
		}

		/************************************************************
		 * Add a Course to a Specified Semester.
		************************************************************/
		public bool addCourse(int assocSemesterId, string courseCode, string courseNumber)
		{
			// Don't check if course already exists, allow user to add duplicate courses.
			string query = String.Format("INSERT INTO skrohn_gradetracker.courses (assoc_semester_id, course_code, course_number) VALUES ({0}, \"{1}\", \"{2}\")", assocSemesterId, courseCode, courseNumber);
			return DatabaseQuery.executeNonQuery(query);
		}

		/************************************************************
		 * Get all WorkItems for a given Course.
		************************************************************/
		public List<WorkItem> getCourseWorkItems(int courseId)
		{
			List<WorkItem> workItems = new List<WorkItem>();
			try
			{
				DataTable data = DatabaseQuery.selectQuery(String.Format("SELECT * FROM skrohn_gradetracker.work_items WHERE assoc_course_id={0}", courseId));
				for (int i = 0; i < data.Rows.Count; i++)
				{
					WorkItem item = new WorkItem();
					item.assocCourseId = Convert.ToInt32(data.Rows[i][0]);
					item.itemName = data.Rows[i][1].ToString();
					item.categoryName = data.Rows[i][2].ToString();
					item.pointsPossible = Convert.ToDouble(data.Rows[i][3]);
					item.pointsEarned = Convert.ToDouble(data.Rows[i][4]);
					workItems.Add(item);
				}
			}
			catch (Exception ex) 
			{
				return null;
			}
			return workItems;
		}

		/************************************************************
		 * Add a work item to a specified Course.
		************************************************************/
		public bool addWorkItem(int assocCourseId, string itemName, string category, double pointsPossible, double pointsEarned)
		{
			// Replace '+' char with spaces
			itemName.Replace("+", " ");
			category.Replace("+", " ");
			string query = String.Format("INSERT INTO skrohn_gradetracker.work_items (assoc_course_id, item_name, category_name, points_possible, points_earned) VALUES ({0}, \"{1}\", \"{2}\", {3}, {4})", assocCourseId, itemName, category, pointsPossible, pointsEarned);
			return DatabaseQuery.executeNonQuery(query);
		}

		/************************************************************
		 * Delete a single specific WorkItem
		************************************************************/
		public bool deleteWorkItem(int assocCourseId, string itemName, string category)
		{
			// Replace '+' char with spaces
			itemName.Replace("+", " ");
			category.Replace("+", " ");
			string query = String.Format("DELETE FROM skrohn_gradetracker.work_items WHERE assoc_course_id={0} AND item_name=\"{1}\" AND category_name=\"{2}\"", assocCourseId, itemName, category);
			return DatabaseQuery.executeNonQuery(query);
		}
		/************************************************************
		 * Delete all WorkItems associated with a specific Course.
		************************************************************/
		public bool deleteCourseWorkItems(int courseId)
		{
			string query = String.Format("DELETE FROM skrohn_gradetracker.work_items WHERE assoc_course_id={0}", courseId);
			return DatabaseQuery.executeNonQuery(query);
		}
	}
}
