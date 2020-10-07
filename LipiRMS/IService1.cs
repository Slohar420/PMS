using ProjectMGMTSRV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using static LipiRMS.Service1;

namespace LipiRMS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "addEmployee")]
        bool addEmployee(EmployeeReq objEmployeeReq);

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "GetDepartmetnId")]
        Reply GetDepartmetnId();
        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "addProject")]
        bool addProject(EmpProject project);



        [OperationContract]
        [WebInvoke(Method = "POST",
         ResponseFormat = WebMessageFormat.Json,
         RequestFormat = WebMessageFormat.Json,
         UriTemplate = "GetAllMember")]
        Response GetAllMember();

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "GetAllManager")]
        Response GetAllManager();

        [OperationContract]
        [WebInvoke(Method = "POST",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           UriTemplate = "GetProjectUser")]
        Reply GetProjectUser(string ProjectName);

        [OperationContract]
        [WebInvoke(Method = "POST",
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json,
          UriTemplate = "checklogin")]
        Reply checklogin(LoginCred login);

        [OperationContract]
        [WebInvoke(Method = "POST",
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json,
          UriTemplate = "GetUserInThisProjectAllreadyExist")]
        Reply GetUserInThisProjectAllreadyExist(string ProjectName);

        [OperationContract]
        [WebInvoke(Method = "POST",
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json,
          UriTemplate = "addUserInThisProject")]
        bool addUserInThisProject(ReqAddUserInThisProject objReqAddUserInThisProject);

        [OperationContract]
        [WebInvoke(Method = "POST",
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json,
          UriTemplate = "allUsers")]
        Response allUsers();

        [OperationContract]
        [WebInvoke(Method = "POST",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           UriTemplate = "updateUser")]
        bool updateUser(EmployeeUpdateReq employee);


        [OperationContract]
        [WebInvoke(Method = "POST",
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json,
          UriTemplate = "getUser")]
        Response getUser(string id);

        [OperationContract]
        [WebInvoke(Method = "POST",
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json,
          UriTemplate = "deleteUser")]
        bool deleteUser(string id);


        [OperationContract]
        [WebInvoke(Method = "POST",
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json,
          UriTemplate = "allProjects")]
        Response allProjects(string data);


        [OperationContract]
        [WebInvoke(Method = "POST",
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json,
          UriTemplate = "getTaskName")]
        string getTaskName(string pid);

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "addTask")]
        bool addTask(Tasks task);

        [OperationContract]
        [WebInvoke(Method = "POST",
      ResponseFormat = WebMessageFormat.Json,
      RequestFormat = WebMessageFormat.Json,
      UriTemplate = "retriveAllTask")]
        Response retriveAllTask(string id);

        [OperationContract]
        [WebInvoke(Method = "POST",
      ResponseFormat = WebMessageFormat.Json,
      RequestFormat = WebMessageFormat.Json,
      UriTemplate = "projectTeam")]
        Response projectTeam(string pid);

        [OperationContract]
        [WebInvoke(Method = "POST",
    ResponseFormat = WebMessageFormat.Json,
    RequestFormat = WebMessageFormat.Json,
    UriTemplate = "GetUserInProjects")]
        Response GetUserInProjects();

        [OperationContract]
        [WebInvoke(Method = "POST",
  ResponseFormat = WebMessageFormat.Json,
  RequestFormat = WebMessageFormat.Json,
  UriTemplate = "getEditProject")]
        Response getEditProject(string ProjectName);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "UpdateProject")]
        bool UpdateProject(EmpProject project);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "SuspendProject")]
        bool SuspendProject(Suspend reqSuspend);


        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "UpdateTask")]
        string UpdateTask(string taskname_pid);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "downloadAttachment")]
        byte[] downloadAttachment(string filename_pname);


        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetActiveAndSuspendProjectCount")]
        Response GetActiveAndSuspendProjectCount();

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetSuspendProjectDetails")]
        Response GetSuspendProjectDetails();

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetChartData")]
        Response GetChartData(string value);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetChartDataAllTask")]
        Response GetChartDataAllTask();

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetTodayTask")]
        Response GetTodayTask();

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetProjectDetails")]
        Reply GetProjectDetails(string data);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "logout")]
        bool logout(string UserId);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetRecentActivity")]
        Response GetRecentActivity();

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetRecentActivityTesting")]
        Response GetRecentActivityTesting();

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetTaskDetails")]
        Reply GetTaskDetails(string userid);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetRecentAssignedTask")]
        Response GetRecentAssignedTask(string userid);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetTaskStatusUser")]
        Response GetTaskStatusUser(string userid);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetWorkOverView")]
        Response GetWorkOverView(string userid);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetTesterList")]
        Response GetTesterList();

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "SubmittTestingProject")]

        bool SubmittTestingProject(ReqTestingSubmitt objReqTestingSubmitt);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetAllExpenses")]
        Response GetAllExpenses(string projectName);
        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "FinalProjectSubmit")]
        bool FinalProjectSubmit(ReqProjectFinalSubmitt objReqProjectFinalSubmitt);
       
        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetCompleteProjectDetails")]
        Response GetCompleteProjectDetails();

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetUserCount")]
        Response GetUserCount();

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetProjectDetailsReport")]
        Response GetProjectDetailsReport(string data);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetProjectDetailsUserWise")]
        Response GetProjectDetailsUserWise(string data);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "UpdatePassword")]
        bool UpdatePassword(UserDetailsReq objUserDetailsReq);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "GetExpenseDetailsUserWise")]
        Response GetExpenseDetailsUserWise(string data);

        [OperationContract]
        [WebInvoke(Method = "POST",
ResponseFormat = WebMessageFormat.Json,
RequestFormat = WebMessageFormat.Json,
UriTemplate = "getFileData")]
        Response getFileData(string projectId);
    }
}

