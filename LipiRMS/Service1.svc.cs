using Classes;
using PipeClient;
using ProjectMGMTSRV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace LipiRMS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    //[AspNetCompatibilityRequirements(RequirementsMode =AspNetCompatibilityRequirementsMode.Allowed)]



    public class Service1 : IService1
    {
        public DBConnect dBconnect = null;
        public string DocumentDirectory = ConfigurationManager.AppSettings["DocumentDirectory"].ToString();
        public Service1()
        {
            // Console.WriteLine(MD5Hash("admin@123"));
        }

        public Reply GetDepartmetnId()
        {
            Reply objReply = new Reply();
            string strQuery = "";
            string strError = "";
            DataSet DS = new DataSet();

            if (dBconnect == null)
                dBconnect = new DBConnect();
            try
            {
                if (dBconnect.database_type == DBType.mysql)
                    strQuery = "select * from projectmanagement.departments";
                else
                    strQuery = "select * from [projectmanagement].[departments]";

                if (dBconnect.Select(strQuery, out DS, out strError) && DS != null && DS.Tables[0].Rows.Count != 0)
                {
                    objReply.DS = DS;
                    objReply.result = true;
                    objReply.Error = "";
                }
                else
                {
                    objReply.DS = DS;
                    objReply.result = false;
                    objReply.Error = strError;
                }
            }
            catch (Exception ex)
            {
                objReply.DS = DS;
                objReply.result = false;
                objReply.Error = ex.ToString();
                Log.Write("Exception in GetDepartmentID Error : - " + ex.ToString(), "");
            }
            return objReply;
        }

        public Reply GetProjectUser(string ProjectName)
        {
            Reply objReply = new Reply();
            string strQuery = "";
            string strError = "";
            DataSet DS = new DataSet();

            string projectId = "", teamMemberUniqeId = "";
            if (dBconnect == null)
                dBconnect = new DBConnect();

            try
            {

                strQuery = " select * from projectmanagement.login where idlogin not in (select fkid_member from projectmanagement.team_organizer" +
                           " where unique_key not in (select team_member from projectmanagement.projects where idProjects != '" + ProjectName + "') )";



                if (DS != null)
                    DS = null;
                if (dBconnect.Select(strQuery, out DS, out strError) && DS != null && DS.Tables[0].Rows.Count != 0)
                {
                    objReply.DS = DS;
                    objReply.result = true;
                    objReply.Error = "";
                }
                else
                {
                    objReply.DS = DS;
                    objReply.result = false;
                    objReply.Error = strError;
                }



            }
            catch (Exception ex)
            {
                objReply.DS = DS;
                objReply.result = false;
                objReply.Error = ex.ToString();
                Log.Write("Exception in GetDepartmentID Error : - " + ex.ToString(), "");
            }
            return objReply;
        }

        public Reply checklogin(LoginCred login)
        {
            Reply objReply = new Reply();
            string strQuery = "";
            string strError = "";
            DataSet DS = new DataSet();

            if (dBconnect == null)
                dBconnect = new DBConnect();

            try
            {
                strQuery = " select * from login where mail_id='" + login.username + "' and password='" + login.password + "'";

                if (dBconnect.Select(strQuery, out DS, out strError) && DS != null && DS.Tables[0].Rows.Count != 0)
                {
                    objReply.DS = DS;
                    objReply.result = true;
                    objReply.Error = "";
                }
                else
                {
                    objReply.DS = null;
                    objReply.result = false;
                    objReply.Error = "User does not exist!";
                }



            }
            catch (Exception ex)
            {
                objReply.DS = DS;
                objReply.result = false;
                objReply.Error = ex.ToString();
                Log.Write("Exception in GetDepartmentID Error : - " + ex.ToString(), "");
            }
            return objReply;
        }

        public bool logout(string UserId)
        {
            bool res = false;
            string strQuery = "";
            string strError = "";
            DataSet DS = new DataSet();

            if (dBconnect == null)
                dBconnect = new DBConnect();

            try
            {
                string loginstatusId = "";
                strQuery = " SELECT idlogin_status FROM projectmanagement.login_status where fk_userid = '" + UserId + "' ORDER BY login_time DESC LIMIT 1";
                if (dBconnect.Select(strQuery, out DS, out strError) && DS != null && DS.Tables[0].Rows.Count == 1)
                {
                    loginstatusId = DS.Tables[0].Rows[0]["idlogin_status"].ToString();
                }
                string Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                strQuery = "UPDATE projectmanagement.login_status set logout_time = '" + Date + "',status = 'Logout' where idlogin_status = '" + loginstatusId + "' ";
                if (dBconnect.Insert(strQuery, out strError) && strError == "")
                {
                    res = true;
                }
                else
                    res = false;

            }
            catch (Exception ex)
            {
                Log.Write("Exception in GetDepartmentID Error : - " + ex.ToString(), "");
            }
            return res;
        }

        public Reply GetUserInThisProjectAllreadyExist(string ProjectName)
        {
            Reply objReply = new Reply();
            string strQuery = "";
            string strError = "";
            DataSet DS = new DataSet();
            string projectId = "", teamMemberUniqeId = "";
            if (dBconnect == null)
                dBconnect = new DBConnect();


            try
            {
                strQuery = "select * from login where idlogin in (select fkid_member from team_organizer where unique_key in (select team_member from projects where idProjects='" + ProjectName + "') )";

                if (dBconnect.Select(strQuery, out DS, out strError) && DS != null && DS.Tables[0].Rows.Count != 0)
                {
                    objReply.DS = DS;
                    objReply.result = true;
                    objReply.Error = "";
                }
                else
                {
                    objReply.DS = DS;
                    objReply.result = false;
                    objReply.Error = strError;
                }



            }
            catch (Exception ex)
            {
                objReply.DS = DS;
                objReply.result = false;
                objReply.Error = ex.ToString();
                Log.Write("Exception in GetDepartmentID Error : - " + ex.ToString(), "");
            }
            return objReply;
        }
        public bool addUserInThisProject(ReqAddUserInThisProject objReqAddUserInThisProject)
        {
            bool result = false;
            string projectId = "";
            string teamMemberUniqeId = "";
            try
            {
                // hasDataSaved bool property

                // sql query 
                string strQuery = "";

                DataSet DS = new DataSet();
                string strError = "";
                // Response data
                Response response = new Response();
                string DepartMentId = "";
                if (dBconnect == null)
                    dBconnect = new DBConnect();

                if (objReqAddUserInThisProject != null)
                {
                    if (dBconnect.database_type == DBType.mysql)
                    {
                        if (objReqAddUserInThisProject.type == "Add")
                        {
                            strQuery = "select * from projectmanagement.projects where idProjects = '" + objReqAddUserInThisProject.projectName + "'";

                            if (dBconnect.Select(strQuery, out DS, out strError) && DS != null && DS.Tables[0].Rows.Count == 1)
                            {
                                projectId = DS.Tables[0].Rows[0]["idprojects"].ToString();
                                teamMemberUniqeId = DS.Tables[0].Rows[0]["team_member"].ToString();
                            }

                            strQuery = "";
                            for (int i = 0; i < objReqAddUserInThisProject.userID.Length; i++)
                            {
                                strQuery = "INSERT INTO projectmanagement.team_organizer(fkid_project,fkid_member,unique_key)" +
                                      "VALUES('" + projectId + "','" + objReqAddUserInThisProject.userID[i] + "','" + teamMemberUniqeId + "'); ";

                                if (dBconnect.Insert(strQuery, out strError) && strError == "")
                                {
                                    result = true;
                                }
                                else
                                    result = false;
                            }
                        }
                        else
                        {
                            strQuery = "select * from projectmanagement.projects where idProjects = '" + objReqAddUserInThisProject.projectName + "'";

                            if (dBconnect.Select(strQuery, out DS, out strError) && DS != null && DS.Tables[0].Rows.Count == 1)
                            {
                                projectId = DS.Tables[0].Rows[0]["idprojects"].ToString();
                                teamMemberUniqeId = DS.Tables[0].Rows[0]["team_member"].ToString();
                            }

                            strQuery = "";
                            for (int i = 0; i < objReqAddUserInThisProject.userID.Length; i++)
                            {
                                strQuery = "delete from projectmanagement.team_organizer where fkid_project = '" + projectId + "'and fkid_member = '" + objReqAddUserInThisProject.userID[i] + "' and unique_key ='" + teamMemberUniqeId + "'";

                                if (dBconnect.Delete(strQuery, out strError) && strError == "")
                                {
                                    result = true;
                                }
                                else
                                    result = false;
                            }
                        }
                    }
                }
                else
                {
                    Log.Write("addUserInThisProject Details Is null", "");
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In addUserInThisProject() Error " + ex.ToString(), "");
                result = false;
            }
            return result;
        }

        public bool addEmployee(EmployeeReq objEmployeeReq)
        {
            bool result = false;
            try
            {
                // hasDataSaved bool property

                // sql query 
                string query = "";

                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();
                string DepartMentId = "";
                if (dBconnect == null)
                    dBconnect = new DBConnect();

                if (objEmployeeReq != null)
                {
                    if (dBconnect.database_type == DBType.mysql)
                    {
                        query = "select iddepartments from  projectmanagement.departments where iddepartments = '" + objEmployeeReq.DepartmetnID + "' ";

                        if (dBconnect.Select(query, out DS, out Error) && DS != null && DS.Tables[0].Rows.Count == 1)
                        {
                            DepartMentId = DS.Tables[0].Rows[0]["iddepartments"].ToString();
                        }
                        else
                        {
                            query = ""; Error = "";
                            query = "insert into projectmanagement.departments (department_name) values ('" + objEmployeeReq.DepartmetnID + "') ";

                            if (dBconnect.Insert(query, out Error) && Error == "")
                            {
                                query = "select iddepartments from  projectmanagement.departments ORDER BY iddepartments DESC LIMIT 1";

                                if (dBconnect.Select(query, out DS, out Error) && DS != null && DS.Tables[0].Rows.Count == 1)
                                {
                                    DepartMentId = DS.Tables[0].Rows[0]["iddepartments"].ToString();
                                }
                            }
                        }
                        query = "";
                        query = " INSERT INTO projectmanagement.login (user_name,mail_id,password,employee_id,designation,date_of_joining,experience,skills," +
                                " salary,user_role,department_id,UserType) " +
                                " VALUES('" + objEmployeeReq.FullName + "' , '" + objEmployeeReq.Email + "' , '" + objEmployeeReq.Password + "'," +
                                "'" + objEmployeeReq.EmployeeCode + "','" + objEmployeeReq.Desgination + "','" + objEmployeeReq.JoningDate + "'," +
                                "'" + objEmployeeReq.Experince + "','" + objEmployeeReq.Skills + "','" + objEmployeeReq.Salary + "','" + objEmployeeReq.Role + "','" + DepartMentId + "','" + objEmployeeReq.UserType + "' )";

                        if (dBconnect.Insert(query, out Error) && Error == "")
                            result = true;
                        else
                            result = false;
                    }
                }
                else
                {
                    Log.Write("Employee Details Is null", "");
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In addEmployee() Error " + ex.ToString(), "");
                result = false;
            }
            return result;
        }
        public Response GetAllManager()  // MARK:- It retrives all project managers
        {
            Response res = new Response();
            string strQuery = "";
            string strError = "";
            DataSet ds = new DataSet();
            DBConnect dBConnect = new DBConnect();
            try
            {
                strQuery = "select user_name,idlogin from projectmanagement.login where designation='project manager'";
                if (dBConnect.Select(strQuery, out ds, out strError) && ds != null)
                {
                    res.DS = ds;
                    res.Error = strError;
                    res.isValid = true;
                }
                else
                {
                    res.DS = null;
                    res.Error = strError;
                    res.isValid = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        public Response GetAllMember()  // MARK:- It retrives all  members
        {
            Response res = new Response();
            string strQuery = "";
            string strError = "";
            DataSet ds = new DataSet();
            DBConnect dBConnect = new DBConnect();
            try
            {
                strQuery = "select user_name,idlogin from projectmanagement.login ";
                if (dBConnect.Select(strQuery, out ds, out strError) && ds != null)
                {
                    res.DS = ds;
                    res.Error = strError;
                    res.isValid = true;
                }
                else
                {
                    res.DS = null;
                    res.Error = strError;
                    res.isValid = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        public bool addProject(EmpProject project)
        {
            try
            {
                // hasDataSaved bool property
                bool hasDataSaved;
                // sql query 
                string query = "";

                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();

                if (project != null)
                {
                    Log.Write("Project Details Request Recevid", "");

                    var iUniqueKye = GenerateMemberKey();  // it generates a random key for project team

                    query = " insert into projects(fk_idlogin,project_name,project_type,subtype,due_date,project_manager,team_member,Description,Source,Status,project_createDatetime ) " +
                        "values('" + project.fk_idlogin + "','" + project.Project_Name + "'," +
                    "'" + project.Project_Type + "','" + project.project_subtype + "','" + project.Project_duedate + "'," +
                    "'" + project.project_manager + "','" + iUniqueKye + "','" + project.Description + "','" + project.Source + "','Processing' , '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                    ;
                    if (dBconnect.Insert(query, out Error) && Error == "")
                    {
                        string lastpid = "";
                        DataSet ds = new DataSet();
                        query = "select * from projects where project_name='" + project.Project_Name + "'";


                        dBconnect.Select(query, out ds, out Error);
                        lastpid = ds.Tables[0].Rows[0]["idProjects"].ToString();

                        query = "";

                        for (int i = 0; i < project.TeamID.Length; i++)
                        {
                            query += "insert into team_organizer (fkid_project,fkid_member,unique_key) values(" + lastpid + "," + project.TeamID[i] + ",'" + iUniqueKye + "') ;";
                        }

                        if (dBconnect.Insert(query, out Error))
                        {

                            return true;
                        }
                        else { return false; }

                    }
                    else
                    { return false; }

                }
                else
                {
                    Log.Write("Not Request Foud ", "");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Write("Exception In addProject() Error " + ex.ToString(), "");
                return false;
            }

        }

        private int GenerateMemberKey(string key = "")  // MARK:- It generates a random key 
        {
            int re = 0;
            try
            {
                Random rnd = new Random();
                var memberkey = rnd.Next(10000000, 99999999);
                DataSet ds = new DataSet();
                // hasDataSaved bool property
                bool hasDataSaved;
                // sql query 
                string query = "";

                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();


                Log.Write("Project Details Request Recevid", "");

                query = "select distinct unique_key from team_organizer where unique_key='" + memberkey.ToString() + "'";
                if (dBconnect.Select(query, out ds, out Error))
                {
                    if (ds.Tables[0].Rows.Count == 0 || ds == null)
                    {
                        re = memberkey;
                    }
                    else
                    {
                        GenerateMemberKey(memberkey.ToString());
                    }
                }
                else
                {

                }
            }
            catch
            {

            }
            return re;
        }


        public Response allUsers() //MARK:- It returns  all user (added by shiv)
        {
            try
            {
                Log.Write("allUsers ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();
                query = "select * from login as l left outer join (select * from projects as p left join team_organizer as t on p.team_member = t.unique_key) as newT on l.idlogin = newT.fkid_member  ORDER BY idlogin desc";
                //query = " select * from login";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                    return response;
                }
                else
                {
                    Log.Write("USer Details Not Found In allusers()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
        }

        /// <summary>
        /// It updates user (added by shiv)
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>true if updated else false</returns>
        public bool updateUser(EmployeeUpdateReq employee)
        {
            try
            {
                Log.Write("update user ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();
                query = "update login set user_name='" + employee.FullName + "', mail_id='" + employee.Email + "' , employee_id='" + employee.EmployeeCode + "', designation='" + employee.Desgination + "' , date_of_joining='" + employee.JoningDate + "', experience='" + employee.Experince + "' , skills='" + employee.Skills + "' , salary='" + employee.Salary + "', user_role='" + employee.Role + "' where idlogin=" + employee.id + "";

                if (dBconnect.Update(query, out Error))
                {
                    return true;
                }
                else
                {
                    Log.Write("invalid Details updateUser()", "");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In updateUser() Error " + ex.ToString(), "");
                return false;
            }
        }

        /// <summary>
        /// Returns user accordingly received id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Response with Error and data</returns>
        public Response getUser(string id)
        {
            try
            {
                Log.Write("one user detail id is-" + id + "", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();
                query = "select * from login where idlogin=" + id + "";
                //query = " select * from login";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                    return response;
                }
                else
                {
                    Log.Write("USer Details Not Found In getUser()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getUser() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
        }

        public bool deleteUser(string id)
        {
            bool returnedvalue = false;
            try
            {

                Log.Write("User id is-" + id + "", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();
                query = "delete from login where idlogin=" + id + "";
                //query = " select * from login";


                if (dBconnect.Delete(query, out Error) && Error == "")
                {
                    returnedvalue = true;
                }
                else
                {
                    Log.Write("User Details Not Found In deleteUser()", "");
                    returnedvalue = false;
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In deleteUser() Error " + ex.ToString(), "");
                returnedvalue = false;
            }
            return returnedvalue;
        }
        public Response allProjects(string data) //MARK:- It return all projects list 
        {
            try
            {
                Log.Write("getAllProjectDetails Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();
                if (data == null)
                    query = " select * from projects where status != 'Complete' and status != 'Suspend'";
                if(data== "admintesting")
                    query = " select * from projects where status = 'testing'";
                else
                    query = " select * from projects ";
                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;

                    return response;
                }
                else
                {
                    Log.Write("Project Details Not Found In getAllProjectDetails()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
        }
        public Response GetUserInProjects() //MARK:- It return all projects list 
        {
            try
            {
                Log.Write("GetUserInProjects Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = "  SELECT  project_name, idProjects,  (select Count(fkid_member) from projectmanagement.team_organizer where unique_key = team_member) as 'userCount'" +
                          ",(select sum(worked_minutes) from projectmanagement.tasks where projectid = idProjects) as 'minute' ," +
                          "(select Count(idtasks) from projectmanagement.tasks where projectid = idProjects) as 'task'," +
                          "  case when task_starttime IS NULL THEN project_createDatetime else task_starttime end as 'lastactivity'" +
                          "  FROM projectmanagement.projects as T1                                                " +
                          "  left join  projectmanagement.team_organizer as T2 on T1.idProjects = T2.fkid_project " +
                          "  left JOIN TASKS AS T3 ON T1.idProjects = t3.projectid                                " +
                          "  where status != 'Suspend' and status != 'Complete' group by project_name  ORDER BY idProjects DESC";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                    Log.Write("Project Details Found " + DS.Tables[0].Rows.Count, "");
                    return response;
                }
                else
                {
                    Log.Write("Project Details Not Found In GetUserInProjects()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In GetUserInProjects() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid">Project ID</param>
        /// <returns></returns>
        public string getTaskName(string pid) //MARK:- It returns taskname 
        {
            string returningStr = "";
            try
            {
                Log.Write("getAllProjectDetails Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = " select * from tasks where projectid=" + pid + " order by idtasks desc";

                if (dBconnect.Select(query, out DS, out Error))
                    if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                    {
                        string lastTid = DS.Tables[0].Rows[0][1].ToString().Split('_')[1];
                        lastTid = lastTid.TrimStart(new Char[] { '0' });
                        if (lastTid.Length == 1)
                            returningStr = "Task_00" + ((Convert.ToInt32(lastTid) + 1)).ToString();
                        else if (lastTid.Length == 2)
                            returningStr = "Task_0" + ((Convert.ToInt32(lastTid) + 1)).ToString();
                        else if (lastTid.Length == 3)
                            returningStr = "Task_" + ((Convert.ToInt32(lastTid) + 1)).ToString();
                    }
                    else
                    {
                        returningStr = "Task_001";
                        Log.Write("No task found in  tasks table", "");
                        return returningStr;
                    }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                return returningStr;
            }
            return returningStr;
        }

        public bool addTask(Tasks task)
        {
            // Response data
            Response response = new Response();

            try
            {
                // hasDataSaved bool property
                bool hasDataSaved;
                // sql query 
                string query = "";

                string Error = "";

                DataSet ds = new DataSet();

                if (dBconnect == null)
                    dBconnect = new DBConnect();

                if (task.task_attachment == "" && task.attachmentBytes.Length == 0)
                {
                    task.task_attachment = "N/A";
                }

                if (task != null)
                {
                    Log.Write("Task Details Request Recevid", "");

                    query = " insert into tasks(task_name,task_descriptions,duedate,assigned_to,fk_idlogin,email_status,start_date, task_status,projectid,task_type,task_attachment,task_priority) " +
                        "values('" + task.task_name + "','" + task.task_descriptions + "'," + "'" + task.duedate + "','" + task.assigned_to + "','" + task.fk_idlogin + "'," +
                    "'" + task.email_status + "','" + task.start_date + "','" + task.task_status + "','" + task.projectid + "','" + task.task_type + "','" + task.task_attachment + "','" + task.task_priority + "')";


                    if (dBconnect.Insert(query, out Error) && Error == "")
                    {
                        Log.Write("Task added", "");

                        query = "select project_name from projects where idProjects=" + task.projectid + "";
                        dBconnect.Select(query, out ds, out Error);
                        string projectname = ds.Tables[0].Rows[0][0].ToString();
                        string fullProjectDirectoryPath = DocumentDirectory + "\\" + projectname;
                        //int lind = task.task_attachment.LastIndexOf('.') + 1;
                        //string extension = task.task_attachment.Substring(lind);
                        //string filename = task.task_name + "_" + DateTime.Now.ToString("dd_MM_yyyy") +"."+extension;

                        if (task.task_attachment != "" && task.task_attachment != "N/A")   //when task attachment is not uploaded
                        {
                            if (writeFile(task.task_attachment, fullProjectDirectoryPath, task.attachmentBytes))
                            {
                                string taskid = "";
                                query = "select idtasks from projectmanagement.tasks order by idtasks DESC limit 1";
                                if (dBconnect.Select(query, out ds, out Error) && ds.Tables[0].Rows.Count == 1)
                                {
                                    taskid = ds.Tables[0].Rows[0]["idtasks"].ToString();
                                }
                                string date = DateTime.Now.ToString("dd/MM/yyyy");
                                query = "  INSERT INTO projectmanagement.file_master " +
                                         " (fk_idprojects," +
                                         " fk_taksid," +
                                         " file_attachment," +
                                         " Upload_By,date_time)" +
                                         " VALUES(" +
                                         "'" + task.projectid + "'," +
                                         "'" + taskid + "'," +
                                         "'" + task.task_attachment + "'," +
                                         "'" + task.fk_idlogin + "','" + date + "')";

                                if (dBconnect.Insert(query, out Error) && Error == "")
                                {
                                    Log.Write("File Name Insert In DB", "");
                                    return true;
                                }
                                else
                                {
                                    Log.Write("File Name Insert In DB Failed", "");
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    { return false; }

                }
                else
                {
                    Log.Write("Not Request Foud ", "");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Write("Exception In addProject() Error " + ex.ToString(), "");
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">project id</param>
        /// <returns></returns>
        public Response retriveAllTask(string id)
        {
            try
            {
                Log.Write("All Task Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();

                string[] data = id.Split('#');
                if (dBconnect == null)
                    dBconnect = new DBConnect();

                if (data[1] == "admintesting")
                {
                    query = " select *," +
                       " (select user_name from login as l where l.idlogin = newt.assigned_to) as task_member," +
                       " (select user_name from login as l where l.idlogin = newt.fk_idlogin) as assigned_by" +
                       " from tasks as newt" +
                       " where projectid = '" + data[0] + "'" +
                       " and assigned_to in(select idlogin from login where userType = 'Tester')" +
                       " ORDER BY idtasks DESC";
                }
                else
                {
                    query = " select *,(select user_name from login as l where l.idlogin=newt.assigned_to) as task_member,(select user_name from login as l where l.idlogin=newt.fk_idlogin) as assigned_by from tasks as newt where projectid=" + data[0] + " ORDER BY idtasks DESC";
                }           

               
                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;

                    return response;
                }
                else
                {
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In retriveAllTask() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
        }
        public Response projectTeam(string pid) //MARK:- It returns project team 
        {
            Response resp = new Response();
            try
            {
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = "  select user_name,idlogin from login as l inner join (select fkid_member from team_organizer where unique_key = (select team_member from projects where idprojects = " + pid + ")) as t on l.idlogin = t.fkid_member";

                if (dBconnect.Select(query, out DS, out Error))
                    if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                    {
                        resp.DS = DS;
                        resp.Error = "";
                        resp.isValid = true;
                    }
                    else
                    {
                        resp.DS = null;
                        resp.Error = Error;
                        resp.isValid = false;
                    }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                resp.DS = null;
                resp.Error = "Exception occured!";
                resp.isValid = false;
            }
            return resp;
        }

        public Response getEditProject(string ProjectName) //MARK:- It return  projects list according to select project for edit// 
        {
            try
            {
                Log.Write("getEditProject Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = " select * from projects where idProjects = '" + ProjectName + "'";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;

                    return response;
                }
                else
                {
                    Log.Write("Project Details Not Found In getEditProject()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getEditProject() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
        }

        public bool UpdateProject(EmpProject project) // Update Project //
        {
            try
            {
                // hasDataSaved bool property
                bool hasDataSaved;
                // sql query 
                string query = "";

                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();

                if (project != null)
                {
                    Log.Write("Project Details Request Recevid", "");
                    query = " UPDATE projectmanagement.projects " +
                            " SET" +
                            " project_type = '" + project.Project_Type + "'," +
                            " subtype = '" + project.project_subtype + "'," +
                            " due_date = '" + project.Project_duedate + "'," +
                            " project_manager ='" + project.project_manager + "'," +
                            " Description = '" + project.Description + "'," +
                            " Source = '" + project.Source + "'" +
                            " WHERE idProjects = '" + project.Project_Name + "'";


                    if (dBconnect.Update(query, out Error) && Error == "")
                    {
                        return true;
                    }
                    else
                    { return false; }

                }
                else
                {
                    Log.Write("Not Request Foud ", "");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Write("Exception In UpdateProject() Error " + ex.ToString(), "");
                return false;
            }

        }

        public bool SuspendProject(Suspend reqSuspend) // Suspend Project //
        {
            string projectid = "";
            bool hasDataSaved = false;
            try
            {
                // hasDataSaved bool property

                // sql query 
                string query = "";

                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();

                if (reqSuspend != null)
                {
                    Log.Write("Project Details Request Recevid", "");
                    if (reqSuspend.Type == "Suspend")
                    {
                        query = " UPDATE projectmanagement.projects " +
                            " SET" +
                            " Status = 'Suspend'" +
                            " WHERE idProjects = '" + reqSuspend.projectName + "'";


                        if (dBconnect.Update(query, out Error) && Error == "")
                        {
                            DataSet ds1 = new DataSet();


                            query = "insert into projectmanagement.suspend (fk_projectid , reson , suspend_datetime) values('" + reqSuspend.projectName + "' , '" + reqSuspend.Reson + "','" + DateTime.Now.ToString("yyyy/MM/dd") + "')";
                            if (dBconnect.Insert(query, out Error) && Error == "")
                            {
                                hasDataSaved = true;
                                query = " insert into projectmanagement.expnses (fk_projectid , expense_type,expense_description,amount) " +
                                     " values('" + reqSuspend.projectName + "' , '" + reqSuspend.ExpeseType + "' ,'" + reqSuspend.ExpensesDescription + "','" + reqSuspend.Amount + "')";
                                if (dBconnect.Insert(query, out Error) && Error == "")
                                {
                                    hasDataSaved = true;
                                }
                                else
                                    hasDataSaved = false;
                            }
                            else
                                hasDataSaved = false;


                        }
                        else
                        { hasDataSaved = false; }
                    }
                    else
                    {
                        query = " UPDATE projectmanagement.projects " +
                            " SET" +
                            " Status = 'Processing'" +
                            " WHERE idProjects = '" + reqSuspend.projectName + "'";

                        if (dBconnect.Update(query, out Error) && Error == "")
                        {
                            query = " UPDATE projectmanagement.suspend " +
                            " SET" +
                            " resume_datetime = '" + DateTime.Now.ToString("yyyy/MM/dd") + "'" +
                            " WHERE fk_projectid = '" + reqSuspend.projectName + "'";
                            if (dBconnect.Update(query, out Error) && Error == "")
                            {
                                hasDataSaved = true;
                            }
                            else
                                hasDataSaved = false;
                        }
                    }

                }
                else
                {
                    Log.Write("Not Request Foud ", "");
                    hasDataSaved = false;
                }

            }
            catch (Exception ex)
            {
                Log.Write("Exception In UpdateProject() Error " + ex.ToString(), "");
                hasDataSaved = false;
            }
            return hasDataSaved;
        }

        /// <summary>
        /// it returns true if file writing is successed
        /// </summary>
        /// <param name="filename">File name to save as</param>
        /// <param name="path">file local path</param>
        /// <param name="filebytes">file byte</param>
        /// <returns></returns>
        private bool writeFile(string filename, string path, byte[] filebytes)
        {
            bool argReturn = false;
            try
            {
                if (Directory.Exists(path))
                {
                    File.WriteAllBytes(path + "\\" + filename, filebytes);
                    argReturn = true;
                }
                else if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    File.WriteAllBytes(path + "\\" + filename, filebytes);
                    argReturn = true;
                }
                else
                {
                    Log.Write("File writing failed!", "Unknown");
                    argReturn = false;
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception Occured While File writing!", "Unknown");
                argReturn = false;
                throw;
            }
            return argReturn;
        }

        public string UpdateTask(string taskname_pid)
        {
            Reply objReply = new Reply();
            string strQuery = "";
            string strError = "";
            DataSet DS = new DataSet();
            if (dBconnect == null)
                dBconnect = new DBConnect();
            string returning = "false";

            try
            {
                string[] data = taskname_pid.Split('#');

                string pid = data[1].ToString();
                string taskname = data[0].ToString();

                if (data.Length > 2)
                {
                    if (data[2].ToLower() == "dailysubmission")
                    {
                        strQuery = "select task_starttime,(select salary from login where idlogin=( select assigned_to from tasks where projectid=" + pid + " and task_name='" + taskname + "')) as salary,Expence,worked_minutes from tasks  where projectid=" + pid + " and task_name='" + taskname + "' and task_status='In Process'";

                        if (dBconnect.Select(strQuery, out DS, out strError))
                        {
                            if (DS.Tables[0].Rows.Count > 0)
                            {
                                double prevWorkedMinute = Convert.ToDouble(DS.Tables[0].Rows[0][3].ToString());
                                double taskExpence = Convert.ToDouble(DS.Tables[0].Rows[0][2].ToString());  //Previews Expences
                                double memberSlry = Convert.ToDouble(DS.Tables[0].Rows[0][1].ToString());  // Task worker salary

                                double oneMinuteSlry = ((memberSlry / 30) / (8 * 60));  // Task worker one minutes salary

                                DateTime today = Convert.ToDateTime((DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")));

                                DateTime dateTime = Convert.ToDateTime(DS.Tables[0].Rows[0][0].ToString().Replace("_", " "));

                                double workedHours = today.Hour - dateTime.Hour;  //Total worked houres
                                double workedMin = today.Minute - dateTime.Minute; //Total worked minutes

                                workedMin += prevWorkedMinute + workedHours * 60;   //Total worked minutes including houres

                                taskExpence += Convert.ToDouble(String.Format("{0:0.00}", oneMinuteSlry)) * workedMin;

                                strQuery = "update tasks set worked_minutes='" + workedMin.ToString() + "',Expence=" + taskExpence + ",task_status='Pause'  where projectid=" + pid + " and task_name='" + taskname + "'";

                                if (dBconnect.Update(strQuery, out strError))
                                {
                                    returning = "true#Pause";
                                }
                                else
                                {
                                    returning = "false#";
                                }
                            }
                            else
                            {
                                returning = "false#PLEASE START TASK!";
                            }
                        }
                    }
                    else if (data[2].ToLower() == "finalsubmission")
                    {
                        strQuery = "select task_starttime,(select salary from login where idlogin=( select assigned_to from tasks where projectid=" + pid + " and task_name='" + taskname + "')) as salary,Expence,worked_minutes from tasks  where projectid=" + pid + " and task_name='" + taskname + "'";

                        if (dBconnect.Select(strQuery, out DS, out strError))
                        {

                            if (DS.Tables[0].Rows.Count > 0)
                            {
                                string taskExpDescription = "";

                                if (data.Length > 3 && data[6].ToString() != "")
                                {
                                    taskExpDescription = data[6].ToString();
                                }
                                string finalSubmissionDate = DateTime.Now.ToString("dd/MM/yyyy_HH:mm:ss");
                                double prevWorkedMinute = Convert.ToDouble(DS.Tables[0].Rows[0][3].ToString());
                                double taskExpence = Convert.ToDouble(DS.Tables[0].Rows[0][2].ToString());  //Previews Expences
                                double memberSlry = Convert.ToDouble(DS.Tables[0].Rows[0][1].ToString());  // Task worker salary

                                double oneMinuteSlry = ((memberSlry / 30) / (8 * 60));  // Task worker one minutes salary

                                DateTime today = Convert.ToDateTime((DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")));

                                DateTime dateTime = Convert.ToDateTime(DS.Tables[0].Rows[0][0].ToString().Replace("_", " "));

                                double workedHours = today.Hour - dateTime.Hour;  //Total worked houres
                                double workedMin = today.Minute - dateTime.Minute; //Total worked minutes 

                                workedMin += prevWorkedMinute + workedHours * 60;   //Total worked minutes including houres

                                taskExpence += Convert.ToDouble(String.Format("{0:0.00}", oneMinuteSlry)) * workedMin;

                                //if (data[5] != "")
                                //    taskExpence += Convert.ToDouble(data[5].ToString());

                                strQuery = "update tasks set worked_minutes='" + workedMin.ToString() + "',Expence=" + taskExpence + ",task_status='Complete',submission_date='" + finalSubmissionDate + "',otherexp_description='" + taskExpDescription + "'  where projectid=" + pid + " and task_name='" + taskname + "'";

                                if (dBconnect.Update(strQuery, out strError))
                                {
                                    string taskid = "";
                                    if (DS != null)
                                        DS = null;
                                    strQuery = "select idtasks FROM projectmanagement.tasks where projectid=" + pid + " and task_name='" + taskname + "'";
                                    if (dBconnect.Select(strQuery, out DS, out strError) && DS != null && DS.Tables[0].Rows.Count == 1)
                                    {
                                        taskid = DS.Tables[0].Rows[0]["idtasks"].ToString();
                                    }
                                    strQuery = " insert into projectmanagement.expnses (fk_projectid , expense_type,expense_description,amount,fk_taskid) " +
                                        " values('" + pid + "' , '" + data[7] + "' ,'" + taskExpDescription + "','" + data[5] + "', " + taskid + ")";
                                    if (dBconnect.Insert(strQuery, out strError) && strError == "")
                                    {
                                        returning = "true";
                                    }
                                    else
                                        returning = "false";
                                    strQuery = "select project_name from projects where idProjects=" + pid + "";
                                    dBconnect.Select(strQuery, out DS, out strError);
                                    string projectname = DS.Tables[0].Rows[0][0].ToString();
                                    string fullProjectDirectoryPath = DocumentDirectory + "\\" + projectname;



                                    if (data[3] != "" && data[4] != "")  //when task attachment is not uploaded
                                    {
                                        string extension = data[4].Substring(data[4].LastIndexOf('.') + 1);
                                        string filename = taskname + "_" + DateTime.Now.ToString("dd_MM_yyyy") + "." + extension;

                                        byte[] filebyte = Convert.FromBase64String(data[3]);

                                        if (writeFile(filename, fullProjectDirectoryPath, filebyte))
                                        {
                                            string query = "", idlogin = "";
                                            query = "select assigned_to from tasks where projectid = " + pid + " and task_name = '" + taskname + "'";
                                            if (dBconnect.Select(query, out DS, out strError) && DS.Tables[0].Rows.Count == 1)
                                            {
                                                idlogin = DS.Tables[0].Rows[0]["assigned_to"].ToString();
                                            }
                                            string date = DateTime.Now.ToString("dd/MM/yyyy");
                                            query = "  INSERT INTO projectmanagement.file_master " +
                                                     " (fk_idprojects," +
                                                     " fk_taksid," +
                                                     " file_attachment," +
                                                     " Upload_By,date_time)" +
                                                     " VALUES(" +
                                                     "'" + pid + "'," +
                                                     "'" + taskid + "'," +
                                                     "'" + filename + "'," +
                                                     "'" + idlogin + "','" + date + "')";

                                            if (dBconnect.Insert(query, out strError) && strError == "")
                                            {
                                                Log.Write("File Name Insert In DB", "");
                                                returning = "true";
                                            }
                                            else
                                            {
                                                Log.Write("File Name Insert In DB Failed", "");
                                                returning = "false";
                                            }

                                        }
                                        else
                                        {
                                            returning = "false";
                                        }
                                    }
                                    else
                                    {
                                        returning = "true";
                                    }
                                }
                                else
                                {
                                    returning = "false";
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                    else if (data[2].ToLower() == "suspend")
                    {
                        strQuery = "update tasks set task_status='suspend' where projectid=" + pid + " and task_name='" + taskname + "'";

                        if (dBconnect.Update(strQuery, out strError))
                        {
                            returning = "true";
                        }
                        else
                        {
                            returning = "false";
                        }
                    }
                    else if (data[2].ToLower() == "active")
                    {
                        strQuery = "update tasks set task_status='" + data[2] + "' where projectid=" + pid + " and task_name='" + taskname + "'";

                        if (dBconnect.Update(strQuery, out strError))
                        {
                            returning = "true";
                        }
                        else
                        {
                            returning = "false";
                        }
                    }
                    else if (data[2].ToLower() == "delete")
                    {
                        strQuery = "delete from tasks  where projectid=" + pid + " and task_name='" + taskname + "'";

                        if (dBconnect.Update(strQuery, out strError))
                        {
                            returning = "true";
                        }
                        else
                        {
                            returning = "false";
                        }
                    }
                    else if (data[2].ToLower() == "startstatus")
                    {
                        strQuery = "select task_status from tasks  where projectid=" + pid + " and task_name='" + taskname + "'";

                        if (dBconnect.Select(strQuery, out DS, out strError))
                        {
                            string taskStatus = DS.Tables[0].Rows[0][0].ToString();
                            if (taskStatus.ToLower() == "in process")
                            {
                                returning = "true#STARTED...";
                            }
                            else if (taskStatus.ToLower() == "active")
                            {
                                returning = "true#NOT STARTED YET!";
                            }
                            else if (taskStatus.ToLower() == "pause")
                            {
                                returning = "true#PAUSE";
                            }
                            else
                            {
                                returning = "false#STOPPED";
                            }
                        }
                        else
                        {
                            returning = "false";
                        }
                    }
                }
                else
                {
                    string dtime = DateTime.Now.ToString("dd/MM/yyyy_HH:mm:ss");
                    strQuery = "update tasks set task_starttime='" + dtime + "',task_status='In Process' where projectid=" + pid + " and task_name='" + taskname + "'";

                    if (dBconnect.Select(strQuery, out DS, out strError))
                    {
                        returning = "true#" + dtime;
                    }
                    else
                    {
                        returning = "false";
                    }
                }

            }
            catch (Exception ex)
            {
                returning = "false";
            }

            return returning;
        }

        public Response GetActiveAndSuspendProjectCount() //MARK:- It return  projects list according to select project for edit// 
        {
            try
            {
                Log.Write("GetActiveAndSuspendProjectCount Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = " select count(case when status = 'Processing' or status ='Testing' then idprojects end) as 'active', " +
                        "count(case when status = 'Complete' then idprojects end) as 'complete'," +
                        "count(case when status = 'Suspend' then idprojects end) as 'Suspend'  FROM  projectmanagement.projects ";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;

                    return response;
                }
                else
                {
                    Log.Write("Project Details Not Found In GetActiveAndSuspendProjectCount()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In GetActiveAndSuspendProjectCount() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
        }

        public Response GetSuspendProjectDetails() //MARK:- It return all projects list 
        {
            try
            {
                Log.Write("getAllProjectDetails Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response(); 

                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = "  SELECT  project_name, idProjects,  (select Count(fkid_member) from projectmanagement.team_organizer where unique_key = team_member) as 'userCount'" +
                         ",(select sum(worked_minutes) from projectmanagement.tasks where projectid = idProjects) as 'minute' ," +
                         "(select Count(idtasks) from projectmanagement.tasks where projectid = idProjects) as 'task'," +
                         "  case when task_starttime IS NULL THEN project_createDatetime else task_starttime end as 'lastactivity'" +
                         "  FROM projectmanagement.projects as T1                                                " +
                         "  left join  projectmanagement.team_organizer as T2 on T1.idProjects = T2.fkid_project " +
                         "  left JOIN TASKS AS T3 ON T1.idProjects = t3.projectid                                " +
                         "  where status = 'Suspend' group by project_name  ORDER BY idProjects DESC";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;

                    return response;
                }
                else
                {
                    Log.Write("Project Details Not Found In getAllProjectDetails()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
        }
        public Response GetChartData(string value) //MARK:- It return all projects list 
        {
            Response response = new Response();
            try
            {
                Log.Write("GetChartData Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = " select count(case when task_status = 'In Process' then task_status end) as 'Inprocess'," +
                        " count(case when task_status = 'Active' then task_status end) as 'Active', " +
                        " count(case when task_status = 'Complete' then task_status end) as 'complete'" +
                        " FROM projectmanagement.tasks where task_type = '" + value + "'; ";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("Project Details Not Found In getAllProjectDetails()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }

        public Response GetChartDataAllTask() //MARK:- It return all projects list 
        {
            Response response = new Response();
            try
            {
                Log.Write("GetChartData Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = " select count(case when Status = 'Processing' then Status end) as 'Inprocess', " +
                        " count(case when Status = 'Suspend' then Status end) as 'Suspend', " +
                        " count(case when Status = 'Complete' then Status end) as 'complete'" +
                        " FROM projectmanagement.projects";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("Project Details Not Found In getAllProjectDetails()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }

        public Response GetTodayTask() //MARK:- It return all projects list 
        {
            Response response = new Response();
            try
            {
                Log.Write("GetChartData Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = " SELECT task_name , user_name FROM projectmanagement.tasks as t1 " +
                        " inner join login as t2 on t2.idlogin = t1.assigned_to where start_date = '" + DateTime.Now.ToString("dd-MM-yyyy") + "' ";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("Project Details Not Found In getAllProjectDetails()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }
        public Reply GetProjectDetails(string data)
        {
            Reply objReply = new Reply();
            string strQuery = "";
            string strError = "";
            DataSet DS = new DataSet();

            if (dBconnect == null)
                dBconnect = new DBConnect();

            try
            {
                if (data != null && data != "")
                {
                    string[] value = data.Split('#');
                    if (value[0] == "" && value[1] == "" && value[2] == "")
                    {
                        strQuery = "  SELECT  project_name,status," +
                               "  (select Count(fkid_member) from projectmanagement.team_organizer where unique_key = team_member) as 'UserCount'," +
                               "  (select Count(idtasks) from projectmanagement.tasks where projectid = idProjects) as 'task',  " +
                               "  (select sum(worked_minutes) from projectmanagement.tasks where projectid = idProjects) as 'minute'," +
                               " (select sum(expence) from projectmanagement.tasks where projectid = idProjects) as 'main hour expence'," +
                               "  (select sum(amount) from projectmanagement.expnses where projectid = fk_projectid and expense_type = 'Tour') as 'tour Expense'," +
                               " (select sum(amount) from projectmanagement.expnses where projectid = fk_projectid and expense_type != 'Tour') as 'expsense', " +
                               " case when task_starttime IS NULL THEN project_createDatetime else task_starttime end as 'lastactivity'" +
                               " FROM projectmanagement.projects as T1" +
                               " left join  projectmanagement.team_organizer as T2 on T2.fkid_project = T1.idProjects" +
                               " left JOIN TASKS AS T3 ON   t3.projectid = T1.idProjects left join expnses as t4 on t1.idprojects = t4.fk_projectid" +
                               " group by project_name ORDER BY task_starttime DESC";
                    }
                    else
                    {
                        string condition = "";
                        if (value[0] != "")
                        {
                            condition = "where status = '" + value[0] + "'";
                        }
                        else if (value[0] == "")
                        {
                            condition = "where project_createDatetime between '" + value[1] + "' and '" + value[2] + "'";
                        }
                        else
                        {
                            condition = "where project_createDatetime between '" + value[1] + "' and '" + value[2] + "' and status = '" + value[0] + "'";
                        }
                        strQuery = "  SELECT  project_name,status," +
                               "  (select Count(fkid_member) from projectmanagement.team_organizer where unique_key = team_member) as 'UserCount'," +
                               "  (select Count(idtasks) from projectmanagement.tasks where projectid = idProjects) as 'task',  " +
                               "  (select sum(worked_minutes) from projectmanagement.tasks where projectid = idProjects) as 'minute'," +
                               " (select sum(expence) from projectmanagement.tasks where projectid = idProjects) as 'main hour expence'," +
                               "  (select sum(amount) from projectmanagement.expnses where projectid = fk_projectid and expense_type = 'Tour') as 'tour Expense'," +
                               " (select sum(amount) from projectmanagement.expnses where projectid = fk_projectid and expense_type != 'Tour') as 'expsense', " +
                               " case when task_starttime IS NULL THEN project_createDatetime else task_starttime end as 'lastactivity'" +
                               " FROM projectmanagement.projects as T1" +
                               " left join  projectmanagement.team_organizer as T2 on T2.fkid_project = T1.idProjects" +
                               " left JOIN TASKS AS T3 ON   t3.projectid = T1.idProjects left join expnses as t4 on t1.idprojects = t4.fk_projectid" +
                               " " + condition + " " +
                                " group by project_name ORDER BY task_starttime DESC";

                    }
                }
                else
                {
                    strQuery = "  SELECT  project_name,status," +
                              "  (select Count(fkid_member) from projectmanagement.team_organizer where unique_key = team_member) as 'UserCount'," +
                              "  (select Count(idtasks) from projectmanagement.tasks where projectid = idProjects) as 'task',  " +
                              "  (select sum(worked_minutes) from projectmanagement.tasks where projectid = idProjects) as 'minute'," +
                              " (select sum(expence) from projectmanagement.tasks where projectid = idProjects) as 'main hour expence'," +
                              "  (select sum(amount) from projectmanagement.expnses where projectid = fk_projectid and expense_type = 'Tour') as 'tour Expense'," +
                              " (select sum(amount) from projectmanagement.expnses where projectid = fk_projectid and expense_type != 'Tour') as 'expsense', " +
                              " case when task_starttime IS NULL THEN project_createDatetime else task_starttime end as 'lastactivity'" +
                              " FROM projectmanagement.projects as T1" +
                              " left join  projectmanagement.team_organizer as T2 on T2.fkid_project = T1.idProjects" +
                              " left JOIN TASKS AS T3 ON   t3.projectid = T1.idProjects left join expnses as t4 on t1.idprojects = t4.fk_projectid" +
                              " group by project_name ORDER BY task_starttime DESC";
                }

               

                if (dBconnect.Select(strQuery, out DS, out strError) && DS != null && DS.Tables[0].Rows.Count != 0)
                {
                    objReply.DS = DS;
                    objReply.result = true;
                    objReply.Error = "";

                }
                else
                {
                    objReply.DS = null;
                    objReply.result = false;
                    objReply.Error = "User does not exist!";
                }



            }
            catch (Exception ex)
            {
                objReply.DS = DS;
                objReply.result = false;
                objReply.Error = ex.ToString();
                Log.Write("Exception in GetDepartmentID Error : - " + ex.ToString(), "");
            }
            return objReply;
        }

        public byte[] downloadAttachment(string filename_pname)
        {
            byte[] filebyte = new byte[0];
            string base64 = "";
            try
            {

                Log.Write("Data Request Recevid : " + filename_pname, "");

                string filename = filename_pname.Split('#')[0];
                string projectname = filename_pname.Split('#')[1];


                if (dBconnect == null)
                    dBconnect = new DBConnect();

                if (Directory.Exists(DocumentDirectory + "\\" + projectname))
                {
                    if (File.Exists(DocumentDirectory + "\\" + projectname + "\\" + filename))
                    {
                        filebyte = File.ReadAllBytes(DocumentDirectory + "\\" + projectname + "\\" + filename);
                        base64 = Convert.ToBase64String(filebyte);
                        if (filebyte.Length > 0)
                        {
                            return filebyte;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return filebyte;
                throw ex;
            }
            return filebyte;
        }
        public Response GetRecentActivity() //MARK:- It return all projects list 
        {
            Response response = new Response();
            try
            {
                Log.Write("GetChartData Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();
                string Todate = DateTime.Now.ToString("dd-MM-yyyy_00:00:00");
                string Fdate = DateTime.Now.ToString("dd-MM-yyyy_23:59:59");
                query = " SELECT submission_date, user_name  FROM projectmanagement.tasks as t1" +
                        " inner join login as t2 on t1.assigned_to = t2.idlogin" +
                        " where task_status = 'Complete'" +
                        " and submission_date between '" + Todate + "' and '" + Fdate + "' order by submission_date DESC";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("Project Details Not Found In getAllProjectDetails()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }

        public Response GetRecentActivityTesting() //MARK:- It return all projects list 
        {
            Response response = new Response();
            try
            {
                Log.Write("GetRecentActivityTesting Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();
                string Todate = DateTime.Now.ToString("dd-MM-yyyy_00:00:00");
                string Fdate = DateTime.Now.ToString("dd-MM-yyyy_23:59:59");
                query = " SELECT submission_date, user_name  FROM projectmanagement.tasks as t1" +
                        " inner join login as t2 on t1.assigned_to = t2.idlogin" +
                        " where task_status = 'Complete'" +
                        " and submission_date between '" + Todate + "' and '" + Fdate + "' and task_type = 'Testing' order by submission_date DESC";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("GetRecentActivityTesting Not Found In getAllProjectDetails()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In GetRecentActivityTesting() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }
        public Reply GetTaskDetails(string userid)
        {
            Reply objReply = new Reply();
            string strQuery = "";
            string strError = "";
            DataSet DS = new DataSet();
            string userRole = "" ;
            if (dBconnect == null)
                dBconnect = new DBConnect();

            try
            {
               
                strQuery = "select user_role from projectmanagement.login where idlogin = '" + userid + "'";
                if (dBconnect.Select(strQuery, out DS , out strError) && DS != null && DS.Tables[0].Rows.Count == 1)
                {
                    userRole = DS.Tables[0].Rows[0]["user_role"].ToString();
                }

                if (userRole.ToString().ToLower().Contains("admintesting"))
                {
                    strQuery = " SELECT project_name," +
                         " COUNT(case when task_type = 'Testing' then idtasks end) as 'task' ," +
                         " count(Case when task_type = 'Testing' then(case when task_status = 'Complete' then task_status end)end) as 'complete'," +
                         " count(case when task_type = 'Testing'then(case when task_status = 'Active' then task_status  end)end) as 'active'," +
                         " count(case when task_type = 'Testing'then(case when task_status = 'In Process' then task_status  end)end) as 'inprocess', " +
                         " count(case when task_type = 'Testing'then( case when task_status = 'Pause' then task_status  end)end) as 'pause'," +
                         " count(case when task_type = 'Testing'then(case when task_status = 'Suspend' then task_status  end)end) as 'suspend'," +
                         " sum( case when task_type = 'Testing'then(case  when worked_minutes IS NULL then '0' else (worked_minutes)end)end) as 'minute'" +
                         " FROM projectmanagement.projects as T1 left JOIN TASKS AS T3 ON   t3.projectid = T1.idProjects" +
                         " where Status = 'testing'   group by project_name";
                }
                else
                {
                    strQuery = " SELECT  project_name," +
                             " COUNT(T3.idtasks) as 'task' ," +
                             " count(case when task_status = 'Complete' then task_status end) as 'complete'," +
                             " count(case when task_status = 'Active' then task_status  end) as 'active'," +
                             " count(case when task_status = 'In Process' then task_status  end) as 'inprocess'," +
                             " count(case when task_status = 'Pause' then task_status  end) as 'pause'," +
                             " count(case when task_status = 'Suspend' then task_status  end) as 'suspend'," +
                             " sum(case  when worked_minutes IS NULL then '0' else (worked_minutes)end) as 'minute'" +
                             " FROM projectmanagement.projects as T1" +
                             " left JOIN TASKS AS T3 ON   t3.projectid = T1.idProjects" +
                             " where t3.assigned_to = '" + userid + "'" +
                             " group by project_name ";                   
                }     

               

                if (dBconnect.Select(strQuery, out DS, out strError) && DS != null && DS.Tables[0].Rows.Count != 0)
                {
                        objReply.DS = DS;
                        objReply.result = true;
                        objReply.Error = "";
               
                }
                else
                {
                    objReply.DS = null;
                    objReply.result = false;
                    objReply.Error = "User does not exist!";
                }



            }
            catch (Exception ex)
            {
                objReply.DS = DS;
                objReply.result = false;
                objReply.Error = ex.ToString();
                Log.Write("Exception in GetDepartmentID Error : - " + ex.ToString(), "");
            }
            return objReply;
        }
        public Response GetRecentAssignedTask(string userid) //MARK:- It return all projects list 
        {
            Response response = new Response();
            try
            {
                Log.Write("GetChartData Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();
                string Todate = DateTime.Now.ToString("dd-MM-yyyy_00:00:00");
                string Fdate = DateTime.Now.ToString("dd-MM-yyyy_23:59:59");

                query = " SELECT start_date, user_name  FROM projectmanagement.tasks as t1 " +
                        " inner join login as t2 on t1.fk_idlogin = t2.idlogin " +
                        " where task_status != 'Complete'" +
                        " and start_date between '" + Todate + "'  and '" + Fdate + "'   and assigned_to = '" + userid + "'    order by start_date DESC";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("Project Details Not Found In getAllProjectDetails()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }

        public Response GetTaskStatusUser(string userid) //MARK:- It return all projects list 
        {
            Response response = new Response();
            try
            {
                Log.Write("GetChartData Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                string UserRole = "";


                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = "select user_role from projectmanagement.login where idlogin = '" + userid + "'";
                if (dBconnect.Select(query, out DS, out Error) && DS != null && DS.Tables[0].Rows.Count == 1)
                {
                    UserRole = DS.Tables[0].Rows[0]["user_role"].ToString();
                }

                if (UserRole.ToString().ToLower().Contains("admintesting"))
                {

                    query = " SELECT " +
                        " count(case when task_type = 'Testing' then ( case when task_status = 'Complete' then task_status end)end) as 'complete'," +
                        " count(case when task_type = 'Testing' then ( case when task_status = 'Active' then task_status  end)end) as 'active'," +
                        " count(case when task_type = 'Testing' then ( case when task_status = 'In Process' then task_status  end)end) as 'inprocess'," +
                        " count(case when task_type = 'Testing' then ( case when task_status = 'Pause' then task_status  end)end) as 'pause'," +
                        " count(case when task_type = 'Testing' then ( case when task_status = 'Suspend' then task_status  end)end) as 'suspend'" +
                        " FROM projectmanagement.tasks";
                        
                }
                else
                {
                    query = " SELECT " +
                        " count(case when task_status = 'Complete' then task_status end) as 'complete'," +
                        " count(case when task_status = 'Active' then task_status  end) as 'active'," +
                        " count(case when task_status = 'In Process' then task_status  end) as 'inprocess'," +
                        " count(case when task_status = 'Pause' then task_status  end) as 'pause'," +
                        " count(case when task_status = 'Suspend' then task_status  end) as 'suspend'" +
                        " FROM projectmanagement.tasks" +
                        " where assigned_to = '" + userid + "'";
                }
                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("Project Details Not Found In getAllProjectDetails()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }

        public Response GetWorkOverView(string userid) //MARK:- It return all projects list 
        {
            Response response = new Response();
            try
            {
                Log.Write("GetChartData Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();

                string Todate = DateTime.Now.ToString("dd-MM-yyyy_00:00:00");
                string Fdate = DateTime.Now.ToString("dd-MM-yyyy_23:59:59");

                query = " SELECT task_name , worked_minutes FROM projectmanagement.tasks " +
                        " where task_starttime between '" + Todate + "' and '" + Fdate + "' " +
                        " and assigned_to = '" + userid + "'; ";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("Project Details Not Found In getAllProjectDetails()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }
        public Response GetTesterList() //MARK:- It return all tester list 
        {
            Response response = new Response();
            try
            {
                Log.Write("GetTesterList Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();



                query = " SELECT * FROM projectmanagement.login where userType = 'Tester'";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("GetTesterList Details Not Found In GetTesterList()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In GetTesterList() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }

        public bool SubmittTestingProject(ReqTestingSubmitt objReqTestingSubmitt) //Project Relase for Teseting
        {
            bool response = false;
            string projectid = "";
            try
            {
                Log.Write("GetTesterList Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();


                query = "update projectmanagement.projects set Status = 'Testing' , testing_leader = '" + objReqTestingSubmitt.testingLeader + "' where idprojects = '" + objReqTestingSubmitt.projectName + "'";
                if (dBconnect.Insert(query, out Error) && Error == "")
                {

                    if (objReqTestingSubmitt.expenseType != "")
                    {
                        query = "insert into projectmanagement.expnses (fk_projectid,expense_type,expense_description,amount) values(" + objReqTestingSubmitt.projectName + ",'" + objReqTestingSubmitt.expenseType + "','" + objReqTestingSubmitt.expenseDescription + "','" + objReqTestingSubmitt.expenseAmount + "')";
                        if (dBconnect.Insert(query, out Error) && Error == "")
                        {
                            response = true;
                        }
                        else
                        {
                            response = false;
                        }
                    }
                }
                else
                    response = false;

            }
            catch (Exception ex)
            {
                Log.Write("Exception In SubmittTestingProject() Error " + ex.ToString(), "");
                response = false;
            }
            return response;
        }

        public Response GetAllExpenses(string projectName) //MARK:- It return all tester list 
        {
            Response response = new Response();
            try
            {
                string projectid = "";
                Log.Write("GetAllExpenses Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = "select idProjects FROM projectmanagement.projects where project_name = '" + projectName + "'";
                if (dBconnect.Select(query, out DS, out Error) && DS != null && DS.Tables[0].Rows.Count == 1)
                {
                    projectid = DS.Tables[0].Rows[0]["idProjects"].ToString();
                }

                query = " SELECT sum(worked_minutes) as 'minute'," +
                        " sum(expence) as 'mainHourExpense'," +
                        " (SELECT sum( case when expense_type = 'Hardware' then amount else '0' end)" +
                        " from projectmanagement.expnses where fk_projectid = projectid) as 'HardwareExpense'," +
                        " (SELECT sum( case when expense_type = 'Software' then amount else '0' end)" +
                        " from projectmanagement.expnses where fk_projectid = projectid) as 'SoftwareExpense'," +
                        " (SELECT sum( case when expense_type = 'Tour' then amount else '0' end)" +
                        " from projectmanagement.expnses where fk_projectid = projectid) as 'TourExpense'," +
                        " (SELECT sum( case when expense_type = 'Other' then amount else '0' end)" +
                        " from projectmanagement.expnses where fk_projectid = projectid) as 'OtherExpense'" +
                        " FROM projectmanagement.tasks where projectid = '" + projectid + "'";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("GetAllExpenses Details Not Found In GetTesterList()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In GetAllExpenses() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }

        public bool FinalProjectSubmit(ReqProjectFinalSubmitt objReqProjectFinalSubmitt) //Project Relase For Final Submiting
        {
            bool response = false;
            string projectid = "";
            try
            {
                Log.Write("GetTesterList Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = "update projectmanagement.projects set Status = 'Complete' ,projectConclusion = '" + objReqProjectFinalSubmitt.ProjectConclusion + "'  where idprojects = '" + objReqProjectFinalSubmitt.projectName + "'";
                if (dBconnect.Insert(query, out Error) && Error == "")
                {
                    query = "insert into projectmanagement.expnses (fk_projectid,expense_type,expense_description,amount) values(" + objReqProjectFinalSubmitt.projectName + ",'" + objReqProjectFinalSubmitt.expenseType + "','" + objReqProjectFinalSubmitt.expenseDescription + "','" + objReqProjectFinalSubmitt.expenseAmount + "')";
                    if (dBconnect.Insert(query, out Error) && Error == "")
                    {
                        response = true;
                    }
                    else
                    {
                        response = false;
                    }
                }
                else
                    response = false;

            }
            catch (Exception ex)
            {
                Log.Write("Exception In SubmittTestingProject() Error " + ex.ToString(), "");
                response = false;
            }
            return response;
        }

        public Response GetCompleteProjectDetails() //MARK:- It return all Complete projects list 
        {
            try
            {
                Log.Write("getAllProjectDetails Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data
                Response response = new Response();

                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = "  SELECT  project_name,  idProjects, (select Count(fkid_member) from projectmanagement.team_organizer where unique_key = team_member) as 'userCount'" +
                         ",(select sum(worked_minutes) from projectmanagement.tasks where projectid = idProjects) as 'minute' ," +
                         "(select Count(idtasks) from projectmanagement.tasks where projectid = idProjects) as 'task'," +
                         "  case when task_starttime IS NULL THEN project_createDatetime else task_starttime end as 'lastactivity'" +
                         "  FROM projectmanagement.projects as T1                                                " +
                         "  left join  projectmanagement.team_organizer as T2 on T1.idProjects = T2.fkid_project " +
                         "  left JOIN TASKS AS T3 ON T1.idProjects = t3.projectid                                " +
                         "  where status = 'Complete' group by project_name  ORDER BY idProjects DESC";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;

                    return response;
                }
                else
                {
                    Log.Write("Project Details Not Found In getAllProjectDetails()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getAllProjectDetails() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
        }

        public Response GetUserCount() //MARK:- It return all tester list 
        {
            Response response = new Response();
            try
            {
                string projectid = "";
                Log.Write("GetUserCount Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = "SELECT count(idlogin) as 'user' FROM projectmanagement.login";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("GetUserCount Details Not Found In GetTesterList()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In GetUserCount() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }

        public Response GetProjectDetailsReport(string data)
        {
            Response response = new Response();
            try
            {
                string projectid = "";
                Log.Write("GetProjectDetailsReport Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();

                string[] value = data.Split('#');
                string condition = "";

                if (value[0] == "0" && value[1] == "0" && value[2] == "0")
                {

                }
                else if (value[0] != "0" && value[1] != "0" && value[2] != "0")
                {
                    condition = " where projectid = '" + value[0] + "' and t2.status = '" + value[1] + "' and task_status = '" + value[2] + "'";
                }
                else if (value[0] != "0" && value[1] == "0" && value[2] == "0")
                {
                    condition = " where projectid = '" + value[0] + "'";
                }
                else if (value[0] != "0" && value[1] != "0" && value[2] == "0")
                {
                    condition = " where projectid = '" + value[0] + "' and  t2.status = '" + value[1] + "'";
                }
                else if (value[0] == "0" && value[1] != "0" && value[2] != "0")
                {
                    condition = " where t2.status = '" + value[1] + "' and task_status = '" + value[2] + "'";
                }
                query = " SELECT project_name , t2.status, task_name , task_status , duedate , submission_date , task_starttime ," +
                           " user_name , worked_minutes , expence , task_type" +
                           " FROM projectmanagement.tasks as t1" +
                           " inner join projects as t2 on t1.projectid = t2.idprojects" +
                           " inner join login as t3 on t1.assigned_to = t3.idlogin" +
                           " " + condition + " ";


                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    Log.Write("GetProjectDetailsReport Response True Rows Count  " + DS.Tables[0].Rows.Count, "");
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("GetUserCount Details Not Found In GetTesterList()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In GetUserCount() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }

        public Response GetProjectDetailsUserWise(string data)
        {
            Response response = new Response();
            try
            {
                string projectid = "";
                Log.Write("GetUserCount Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();
                if (data == "" || data == null)
                {
                    query = " select user_name , designation , UserType , project_name , " +
                            " task_name , task_status , worked_minutes , expence , task_starttime , submission_date ,duedate " +
                            " FROM projectmanagement.tasks as t1 " +
                            " inner join projects as t2 on t1.projectid = t2.idprojects " +
                            " inner join login as t3 on t1.assigned_to = t3.idlogin";
                }
                else
                {
                    query = " select user_name , designation , UserType , project_name , " +
                           " task_name , task_status , worked_minutes , expence , task_starttime , submission_date ,duedate " +
                           " FROM projectmanagement.tasks as t1 " +
                           " inner join projects as t2 on t1.projectid = t2.idprojects " +
                           " inner join login as t3 on t1.assigned_to = t3.idlogin " +
                           " where assigned_to = '" + data + "'";

                }

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("GetUserCount Details Not Found In GetTesterList()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In GetUserCount() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }

        public bool UpdatePassword(UserDetailsReq objUserDetailsReq) //MARK:- It return all tester list 
        {
            bool response = false;
            try
            {
                string projectid = "";
                Log.Write("GetUserCount Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = "update projectmanagement.login set password = '" + objUserDetailsReq.Password + "' where idlogin = '" + objUserDetailsReq.UserId + "'";
                if (dBconnect.Update(query, out Error) && Error == "")
                {
                    response = true;
                }
                else
                {
                    response = false;
                }

            }
            catch (Exception ex)
            {
                Log.Write("Exception In GetUserCount() Error " + ex.ToString(), "");

                response = false;
            }
            return response;
        }

        public Response GetExpenseDetailsUserWise(string data)
        {
            Response response = new Response();
            try
            {
                string projectid = "";
                Log.Write("GetExpenseDetailsUserWise ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();
                if (data == "" || data == null)
                {
                    query = " SELECT user_name ," +
                           " (select sum(worked_minutes) from projectmanagement.tasks where assigned_to = idlogin) as 'main hour expence minutes'," +
                           " (select sum(expence) from projectmanagement.tasks where assigned_to = idlogin) as 'main hour expence'," +
                           " (select sum(amount) from projectmanagement.expnses where projectid = fk_projectid and expense_type = 'Tour') as 'tourExp'," +
                           " (select sum(amount) from projectmanagement.expnses where projectid = fk_projectid and expense_type != 'Tour') as 'otherExp'" +
                           " FROM projectmanagement.login as t1" +
                           " left join projectmanagement.tasks as t2 on t1.idlogin = t2.assigned_to " +
                           " group by user_name";
                }
                else
                {
                    string[] value = data.Split('#');
                    string condition = "";
                    if (value.Length == 2)
                    {
                        condition = "where start_date between '" + Convert.ToDateTime(value[0]).ToString("dd-MM-yyyy_00:00:00") + "' and '" + Convert.ToDateTime(value[1]).ToString("dd-MM-yyyy_23:59:59") + "'";
                    }
                    else if (value.Length == 1)
                    {
                        condition = "where idlogin = '" + data + "'";
                    }
                    else
                    {
                        condition = "where start_date between '" + value[1] + "' and '" + value[2] + "' and idlogin = '" + data + "'";
                    }

                    query = " SELECT user_name ," +
                           " (select sum(worked_minutes) from projectmanagement.tasks where assigned_to = idlogin) as 'main hour expence minutes'," +
                           " (select sum(expence) from projectmanagement.tasks where assigned_to = idlogin) as 'main hour expence'," +
                           " (select sum(amount) from projectmanagement.expnses where projectid = fk_projectid and expense_type = 'Tour') as 'tourExp'," +
                           " (select sum(amount) from projectmanagement.expnses where projectid = fk_projectid and expense_type != 'Tour') as 'otherExp'" +
                           " FROM projectmanagement.login as t1" +
                           " left join projectmanagement.tasks as t2 on t1.idlogin = t2.assigned_to " +
                           " " + condition + "" +
                           " group by user_name";

                }

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("GetExpenseDetailsUserWise Details Not Found In GetTesterList()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In GetExpenseDetailsUserWise() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }

        public Response getFileData(string projectId)
        {
            Response response = new Response();
            try
            {

                Log.Write("getFileData Request Recevid ", "");
                // sql query 
                string query = "";
                // Dataset for response data
                DataSet DS = new DataSet();
                string Error = "";
                // Response data


                if (dBconnect == null)
                    dBconnect = new DBConnect();

                query = " SELECT t3.task_name , t1.file_attachment , t4.user_name ,t1.date_time FROM projectmanagement.file_master as t1 " +
                        " inner join  projects as t2 on t1.fk_idprojects = t2.idProjects" +
                        " inner join tasks as t3 on t3.idtasks = t1.fk_taksid" +
                        " inner join login as t4 on t4.idlogin = t1.Upload_By" +
                        " where fk_idprojects = '" + projectId + "'";

                dBconnect.Select(query, out DS, out Error);
                if (DS != null && DS.Tables[0].Rows.Count > 0 && Error == "")
                {
                    response.DS = DS;
                    response.Error = Error;
                    response.isValid = true;
                }
                else
                {
                    Log.Write("getFileData Details Not Found In getFileData()", "");
                    return response = new Response { DS = null, Error = "Opps! Somthing went wrong...", isValid = false };
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception In getFileData() Error " + ex.ToString(), "");
                return new Response { DS = null, Error = "Invalid data provided...", isValid = false };
            }
            return response;
        }
    }


}



