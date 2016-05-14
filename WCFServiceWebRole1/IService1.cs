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
		[WebGet(UriTemplate="getSemester?id={semesterId}", ResponseFormat=WebMessageFormat.Json)]
		Semester getSemester(string semesterId);

		[OperationContract]
		[WebGet(UriTemplate="getAllSemestersForStudent?id={studentId}", ResponseFormat=WebMessageFormat.Json)]
		List<Semester> getAllSemestersForStudent(string studentId);
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
		public string assocSemesterId;
		[DataMember]
		public string courseCode;	// ie. CSE
		[DataMember]
		public string courseNumber;	// ie. 340
		[DataMember]
		public string courseId;
	}

	[DataContract]
	public class WorkItem
	{
		[DataMember]
		public string assocCourseId;
		[DataMember]
		public string name;
		[DataMember]
		public string categoryName;
		[DataMember]
		public double pointsPossible;
		[DataMember]
		public double pointsEarned;
	}
}
