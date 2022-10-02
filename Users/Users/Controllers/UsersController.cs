using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Users.Models;
using System.Text.RegularExpressions;

namespace Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select user_id, user_email, user_name, user_password from 
                            dbo.Users
                            ";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("UserLoginCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public string JsonStringBody([FromBody] string PassConfirmation)
        {
            return PassConfirmation;
        }
        public JsonResult Post(Userss usr)
        {
            string query = @"
                            insert into dbo.Users (user_email, user_name, user_password)
                            values (@user_email, @user_name, @user_password)
                            ";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("UserLoginCon");
            SqlDataReader myReader;
            var EmailValidation = false;
            var tempMail = usr.user_email;
            var tempName = usr.user_name;                        


                //var mail = "@mit.com";
                // Regex rgx = new Regex(@"[a-z0-9._%+-]" + mail);
            Regex rgx = new Regex(@"^*id.tricorglobal.com$");
                Regex alpha = new Regex(@"[a - zA - Z0 - 9_\s] +");


                using (SqlConnection myCon = new SqlConnection(sqlDatasource))
                {
                    myCon.Open();
                SqlCommand query22 = new SqlCommand("select * from dbo.Users where user_email = '" + tempMail + "'", myCon);
                SqlCommand queryUserName = new SqlCommand("select * from dbo.Users where user_name = '" + tempName + "'", myCon);
                SqlDataAdapter sd = new SqlDataAdapter(query22);
                SqlDataAdapter sdUserName = new SqlDataAdapter(queryUserName);
                DataTable dataEmail = new DataTable();
                DataTable dataUserName = new DataTable();
                Console.ReadLine();
                //Console.WriteLine
                sd.Fill(dataEmail);
                sdUserName.Fill(dataUserName);
                if (dataEmail.Rows.Count > 0)
                {
                    return new JsonResult("The Email already registered ");
                }
                else if (dataUserName.Rows.Count > 0)
                {
                    return new JsonResult("The Username already registered");
                }
                else
                {
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        //var x = rgx.IsMatch(usr.user_email.ToString());
                        if (!rgx.IsMatch(usr.user_email.ToString()))
                        {
                            EmailValidation = true;
                            return new JsonResult("Your Email is invalid ");

                        }
                        else if (rgx.IsMatch(usr.user_email.ToString()))
                        {
                            if (alpha.IsMatch(usr.user_password.ToString()))
                            {                               
                                    myCommand.Parameters.AddWithValue("@user_email", usr.user_email);
                                    myCommand.Parameters.AddWithValue("@user_name", usr.user_name);
                                    myCommand.Parameters.AddWithValue("@user_password", usr.user_password);
                                    EmailValidation = false;
                                    myReader = myCommand.ExecuteReader();
                                    table.Load(myReader);
                                    myReader.Close();
                                    myCon.Close();
                                    return new JsonResult("Your Email is valid");
                              
                            }
                            else
                            {
                                return new JsonResult("The password must only contain alphanumeric character");
                            }
                            

                        }
                       

                    }
                   
                }
            }

            if(EmailValidation == true)
            {
                return new JsonResult("Your Email is valid");
            }
            else
            {
                return new JsonResult("Your Email is invalid");
            }
        }

        [HttpPut]
        public JsonResult Put(Userss usr)
        {
            string query = @"
                            update dbo.Users
                            set user_email = @user_email, user_name = @user_name, user_password = @user_password
                            where user_id = @user_id
                            ";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("UserLoginCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@user_id", usr.user_id);
                    myCommand.Parameters.AddWithValue("@user_email", usr.user_email);
                    myCommand.Parameters.AddWithValue("@user_name", usr.user_name);
                    myCommand.Parameters.AddWithValue("@user_password", usr.user_password);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.Users
                            where  user_id = @user_id
                            ";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("UserLoginCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@user_id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
