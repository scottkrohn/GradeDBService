using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace WCFServiceWebRole1
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract]
	public interface IService1
	{
		[OperationContract]
		[WebGet(UriTemplate="getStudent?id={studentId}", ResponseFormat=WebMessageFormat.Json)]
		Student getStudent(string studentId);

		[OperationContract]
		[WebGet(UriTemplate="getAllStudents?", ResponseFormat=WebMessageFormat.Json)]
		List<Student> getAllStudents();

		[OperationContract]
		[WebGet(UriTemplate="addStudent?first={firstName}&last={lastName}&id={studentId}&user={username}&pw={hashpw}", ResponseFormat=WebMessageFormat.Json)]
		bool addStudent(string firstName, string lastName, string studentId, string username, string hashPw);

		[OperationContract]
		[WebGet(UriTemplate="getSemester?id={semesterId}", ResponseFormat=WebMessageFormat.Json)]
		Semester getSemester(int semesterId);

		[OperationContract]
		[WebGet(UriTemplate="getAllSemestersForStudent?id={studentId}", ResponseFormat=WebMessageFormat.Json)]
		List<Semester> getAllSemestersForStudent(string studentId);

		[OperationContract]
		[WebGet(UriTemplate="addSemester?studentId={studentId}&termName={termName}&termYear={termYear}", ResponseFormat=WebMessageFormat.Json)]
		bool addSemester(string studentId, string termName, string termYear); 

		[OperationContract]
		[WebGet(UriTemplate="getAllCoursesForSemester?id={semesterId}", ResponseFormat=WebMessageFormat.Json)]
		List<Course> getAllCoursesForSemester(int semesterId);
		
		[OperationContract]
		[WebGet(UriTemplate="addCourse?id={assocSemesterId}&code={courseCode}&num={courseNumber}", ResponseFormat=WebMessageFormat.Json)]
		bool addCourse(int assocSemesterId, string courseCode, string courseNumber);

		[OperationContract]
		[WebGet(UriTemplate="getCourseWorkItems?courseId={courseId}", ResponseFormat=WebMessageFormat.Json)]
		List<WorkItem> getCourseWorkItems(int courseId);

		[OperationContract]
		[WebGet(UriTemplate="addWorkItem?id={assocCourseId}&name={itemName}&cat={category}&poss={pointsPossible}&earned={pointsEarned}", ResponseFormat=WebMessageFormat.Json)]
		bool addWorkItem(int assocCourseId, string itemName, string category, double pointsPossible, double pointsEarned);

	}

	[DataContract]
	public class Semester
	{
		[DataMember]
		public string termName;
		[DataMember]
		public string termYear;
		[DataMember]
		public string semesterId;
		[DataMember]
		public string assocStudentId;
	}

	[DataContract]
	public class Student
	{
		[DataMember]
		public string firstName;
		[DataMember]
		public string lastName;
		[DataMember]
		public string id;
		[DataMember]
		public string username;
	}

	[DataContract]
	public class Course
	{
		[DataMember]
		public int assocSemesterId;
		[DataMember]
		public string courseCode;	// ie. CSE
		[DataMember]
		public string courseNumber;	// ie. 340
		[DataMember]
		public int courseId;
		[DataMember]
		public Dictionary<String, Double> categories;
	}

	[DataContract]
	public class WorkItem
	{
		[DataMember]
		public int assocCourseId;
		[DataMember]
		public string itemName;
		[DataMember]
		public string categoryName;
		[DataMember]
		public double pointsPossible;
		[DataMember]
		public double pointsEarned;
	}

	[DataContract]
	public class CategoryWeight
	{
		[DataMember]
		public string categoryName;
		[DataMember]
		public int categoryWeight;
	}
}
