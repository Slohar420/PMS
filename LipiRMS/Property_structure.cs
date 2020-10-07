using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ProjectMGMTSRV
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class LoginCred
    {
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string password { get; set; }

    }

    [DataContract]
    public class DownloadFile
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public byte[] filebyte{ get; set; }

    }

    [DataContract]
    public class EmpProject
    {
        [DataMember]
        public string Project_Name { get; set; }
        [DataMember]
        public string fk_idlogin { get; set; }
        [DataMember]
        public string project_subtype { get; set; }
        [DataMember]
        public string Project_duedate { get; set; }
        [DataMember]
        public string project_manager { get; set; }
        [DataMember]
        public string[] TeamID { get; set; }
        [DataMember]
        public string Project_Type { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Source { get; set; }

    }
    [DataContract]
    public class EmployeeReq
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string EmployeeCode { get; set; }
        [DataMember]
        public string Desgination { get; set; }
        [DataMember]
        public string JoningDate { get; set; }
        [DataMember]
        public string Skills { get; set; }

        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Salary { get; set; }

        [DataMember]
        public string Role { get; set; }

        [DataMember]
        public string DepartmetnID { get; set; }
        ///
        [DataMember]
        public string Experince { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string UserType { get; set; }

    }
    [DataContract]
   
   
    public class Response
    {
        [DataMember]
        public DataSet DS { get; set; }
        [DataMember]
        public string Error { get; set; }
        [DataMember]
        public bool isValid { get; set; }
        [DataMember]
        public bool FirstTimeLogin { get; set; }
    }
    [DataContract]
    public class User
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string UserPassword { get; set; }
    }

    [DataContract] 

    public class Reply
    {
        [DataMember]
        public bool result { get; set; }
        [DataMember]
        public DataSet DS { get; set; }
        [DataMember]
        public string Error { get; set; }

    }

    public class ReqAddUserInThisProject
    {
        [DataMember]
        public string projectName { get; set; }
        [DataMember]
        public string [] userID { get; set; }

        [DataMember]
        public string type { get; set; }
    }
    public class EmployeeUpdateReq
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string EmployeeCode { get; set; }
        [DataMember]
        public string Desgination { get; set; }
        [DataMember]
        public string JoningDate { get; set; }
        [DataMember]
        public string Skills { get; set; }

        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Salary { get; set; }

        [DataMember]
        public string Role { get; set; }

        [DataMember]
        public string DepartmetnID { get; set; }
        ///
        [DataMember]
        public string Experince { get; set; }
        [DataMember]
        public string Password { get; set; }

    }
    [DataContract]
  
    public class Tasks
    {
        [DataMember]
        public string tid;
        [DataMember]
        public string task_descriptions;
        [DataMember]
        public string assigned_to;
        [DataMember]
        public string task_name;
        [DataMember]
        public string duedate;
        [DataMember]
        public string fk_idlogin;
        [DataMember]
        public string email_status;
        [DataMember]
        public string start_date;
        [DataMember]
        public string task_status;
        [DataMember]
        public string projectid;
        [DataMember]
        public string task_type;
        [DataMember]
        public string task_attachment;
        [DataMember]
        public string task_priority;
        [DataMember]
        public byte[] attachmentBytes;
    }
    [DataContract]
    public class Suspend
    {
        [DataMember]
        public string projectName { get; set; }
        [DataMember]
        public string Reson { get; set; }
        [DataMember]
        public string ExpeseType { get; set; }
        [DataMember]
        public string Amount { get; set; }
        [DataMember]
        public string duedate { get; set; }
        [DataMember]
        public string ExpensesDescription { get; set; }


        [DataMember]
        public string Type { get; set; }
    }
    [DataContract]
    public class ReqTestingSubmitt
    {
        [DataMember]
        public string projectName { get; set; }
        [DataMember]
        public string testingLeader { get; set; }
        [DataMember]
        public string testingDescritption { get; set; }
        [DataMember]
        public string expenseType { get; set; }
        [DataMember]
        public string expenseAmount { get; set; }
        [DataMember]
        public string expenseDescription { get; set; }
        [DataMember]
        public byte[] attachmentBytes;
        [DataMember]
        public string task_attachment;
    }
    [DataContract]
    public class ReqProjectFinalSubmitt
    {
        [DataMember]
        public string projectName { get; set; }
        [DataMember]
        public string ProjectConclusion { get; set; }
        [DataMember]        
        public string expenseType { get; set; }
        [DataMember]
        public string expenseAmount { get; set; }
        [DataMember]
        public string expenseDescription { get; set; }
    }
    public class UserDetailsReq
    {
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Password { get; set; }
    }
}