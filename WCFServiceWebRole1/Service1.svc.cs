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
		 * Return a Semester object given a semester ID.
		************************************************************/
		public Semester getSemester(string semesterId)
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
	}

}
