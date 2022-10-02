using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Logged_In_Users.Models;


namespace Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;     

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select from 
                            dbo.Users where  user_id = @user_id, user_email = @user_email, user_name = @user_name, user_password = @user_password
                            ";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("UserLoginCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
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
        public JsonResult Post(Login usr)
        {
            string query = @"
                            select from dbo.Users where user_email = @user_email, user_name = @user_name, user_password = @user_password                            
                            ";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("UserLoginCon");
            SqlDataReader myReader;
            var EmailValidation = false;
            var tempMail = usr.user_email;
            var tempName = usr.user_name;
            String passInput;


            //var mail = "@mit.com";
            // Regex rgx = new Regex(@"[a-z0-9._%+-]" + mail);
            Regex rgx = new Regex(@"^*id.tricorglobal.com$");
            Regex alpha = new Regex(@"[a - zA - Z0 - 9_\s] +");


            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                SqlCommand query22 = new SqlCommand("select * from dbo.Users where user_email = '" + tempMail + "'", myCon);
                SqlCommand queryUserName = new SqlCommand("select * from dbo.Users where user_name = '" + tempName + "'", myCon);
                SqlCommand Password = new SqlCommand("select * from dbo.Users where user_name = '" + tempName + "'", myCon);
                SqlDataAdapter sd = new SqlDataAdapter(query22);
                SqlDataAdapter sdUserName = new SqlDataAdapter(queryUserName);
                DataTable dataEmail = new DataTable();
                DataTable dataUserName = new DataTable();
                passInput.WriteLine
                Console.ReadLine();
                //Console.WriteLine
                sd.Fill(dataEmail);
                sdUserName.Fill(dataUserName);
                if (dataEmail.Rows.Count > 0 && dataUserName.Rows.Count > 0)
                {
                    return new JsonResult("You can log in to the application");
                }
                else if (dataEmail.Rows.Count < 0 && dataUserName.Rows.Count < 0)
                {
                    return new JsonResult("The account has not been registered");
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
                            if (Password == passInput)
                            {
                                myCommand.Parameters.AddWithValue("@user_id", usr.user_id);
                                myCommand.Parameters.AddWithValue("@user_email", usr.user_email);
                                myCommand.Parameters.AddWithValue("@user_name", usr.user_name);
                                myCommand.Parameters.AddWithValue("@user_status", "Logged in");
                                myReader = myCommand.ExecuteReader();
                                table.Load(myReader);
                                myReader.Close();
                                myCon.Close();
                                return new JsonResult("User Logged in");
                                
                            }
                            else
                            {
                                return new JsonResult("The password must only contain alphanumeric character");
                            }


                        }


                    }

                }
            }

            if (EmailValidation == true)
            {
                return new JsonResult("Your Email is valid");
            }
            else
            {
                return new JsonResult("Your Email is invalid");
            }
        }

    }
}
