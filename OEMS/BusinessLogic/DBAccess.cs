using OEMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OEMS
{
    public class DBAccess
    {
        // connection string
        private readonly string ConnectoionString = @"Data Source=DESKTOP-TIMJN3H\SQLEXPRESS;Initial Catalog=OEMS;Integrated Security=True";
        //validate login credentials 
        public bool ValidateLogin(string Username, string Password)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "Select * from Accounts Where Username='"+Username+"' and Password = '"+Password+"'";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["Username"].ToString().Equals(Username) && reader["Password"].ToString().Equals(Password))
                    {
                        AccountSession.User.AccountId = Int32.Parse(reader["AccountId"].ToString());
                        AccountSession.User.Type = reader["Type"].ToString().ToCharArray()[0];
                        AccountSession.User.Username = reader["Username"].ToString();
                        AccountSession.User.FName = reader["FName"].ToString();
                        AccountSession.User.LName = reader["LName"].ToString();
                        if(AccountSession.User.Type == 'S')
                        {

                            var student = GetStudent(AccountSession.User.AccountId);
                            AccountSession.User.StudentId = student.StudentId;
                            AccountSession.User.Name = student.Name;
                            AccountSession.User.CourseId = student.CourseId;
                            AccountSession.Course = GetCourse(Int32.Parse(student.CourseId));
                            
                            //AccountSession.AllCourses = GetCourses();
                            //AccountSession.AllModules = GetModules();
                        }

                        return true;
                    }
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return false;
        }


        public bool ValidateUsername(string Username)
        {

            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "Select * from Accounts Where Username='" + Username + "'";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["Username"].ToString().Equals(Username))
                    {
                        return true;
                    }
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return false;
        }


        #region Students

        public UserModel GetStudent(int accId)
        {
            UserModel user = new UserModel();
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "SELECT Accounts.AccountId AS Account_id, Accounts.*, Students.AccountId AS StudentAcc_id, Students.*  FROM Students LEFT JOIN Accounts ON Students.AccountID=Accounts.AccountID WHERE Students.AccountId =" + accId + "";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var NewUser = new UserModel
                    {
                        AccountId = Int32.Parse(reader["Account_id"].ToString()),
                        StudentId = Int32.Parse(reader["StudentId"].ToString()),
                        Type = reader["Type"].ToString().ToCharArray()[0],
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        FName = reader["FName"].ToString(),
                        LName = reader["LName"].ToString(),
                        Name = reader["Name"].ToString(),
                        CourseId = reader["CourseId"].ToString()
                    };

                    user = NewUser;
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return user;

        }

        public List<UserModel> GetStudents()
        {
            var users = new List<UserModel>();

            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "SELECT Accounts.AccountId AS Account_id, Accounts.*, Students.AccountId AS StudentAcc_id, Students.*  FROM Students LEFT JOIN Accounts ON Students.AccountID=Accounts.AccountID";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var NewUser = new UserModel
                    {
                        AccountId = Int32.Parse(reader["Account_id"].ToString()),
                        StudentId = Int32.Parse(reader["StudentId"].ToString()),
                        Type = reader["Type"].ToString().ToCharArray()[0],
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        FName = reader["FName"].ToString(),
                        LName = reader["LName"].ToString(),
                        Name = reader["Name"].ToString(),
                        CourseId = reader["CourseId"].ToString()
                    };

                    users.Add(NewUser);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return users;
            
        }

        public void CreateStudent(UserModel user)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "INSERT INTO Accounts (Username, Password, Type, FName, LName) output INSERTED.AccountId VALUES ('"+user.Username+ "','" + user.Password+ "','" + user.Type+ "','" + user.FName+ "','" + user.LName+ "')";
                SqlCommand command = new SqlCommand(Query, connection);
                int ID = (Int32)command.ExecuteScalar();

                user.Username = ID+"_"+user.FName;
                user.AccountId = ID;

                Query = "UPDATE Accounts SET Username = '"+user.Username+"' WHERE AccountId='"+ID+"'";
                command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

                Query = "INSERT INTO Students (AccountId, Name, CourseId) VALUES ('" + user.AccountId + "','" + user.FName + " " + user.LName + "','" + user.CourseId + "')";
                command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();



            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        internal void UpdateStudent(UserModel student)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "UPDATE Students SET Name='" + student.Name + "' WHERE AccountId = '" + student.AccountId + "'";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

                Query = "UPDATE Accounts SET Password='" + student.Password + "', FName='" + student.FName + "', LName='" + student.LName + "' WHERE AccountId = '" + student.AccountId + "'";
                command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        #region Lecturers

        public List<UserModel> GetLecturers()
        {
            var users = new List<UserModel>();

            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "SELECT Accounts.AccountId AS Account_id, Accounts.*, Lecturers.AccountId AS LecturerAcc_id, Lecturers.*  FROM Lecturers LEFT JOIN Accounts ON Lecturers.AccountID=Accounts.AccountID";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var NewUser = new UserModel
                    {
                        AccountId = Int32.Parse(reader["Account_id"].ToString()),
                        LecturerId = Int32.Parse(reader["LecturerId"].ToString()),
                        Type = reader["Type"].ToString().ToCharArray()[0],
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        FName = reader["FName"].ToString(),
                        LName = reader["LName"].ToString(),
                        Name = reader["Name"].ToString(),
                        Area = reader["Area"].ToString()
                    };

                    users.Add(NewUser);
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return users;
        }

       

        public void CreateLecturer(UserModel user)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "INSERT INTO Accounts (Username, Password, Type, FName, LName) VALUES ('"+user.Username+ "','" + user.Password+ "','" + user.Type+ "','" + user.FName+ "','" + user.LName+ "')";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

                Query = "Select * from Accounts Where Username='" + user.Username + "'";
                command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["Username"].ToString().Equals(user.Username))
                    {
                        user.AccountId = Int32.Parse(reader["AccountId"].ToString());
                    }
                }
                reader.Close();

                Query = "INSERT INTO Lecturers (AccountId, Name, /*Area*/) VALUES ('" + user.AccountId + "','" + user.FName + " " + user.LName + "','" + user.Area + "')";
                command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        internal void UpdateLecturer(UserModel newuser)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "UPDATE Lecturers SET Name='" + newuser.Name + "', Area='" + newuser.Area + "' WHERE AccountId = '" + newuser.AccountId + "'";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

                Query = "UPDATE Accounts SET Password='" + newuser.Password + "', FName='" + newuser.FName + "', LName='" + newuser.LName + "' WHERE AccountId = '" + newuser.AccountId + "'";
                command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        #region Course
        public void CreateCourse(CourseModel course)
        {
 
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();
                string Query = "INSERT INTO Courses (Name) VALUES ('" + course.Name + "')";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public List<CourseModel> GetCourses()
        {

            var courses = new List<CourseModel>();
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "SELECT * FROM Courses";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int courseId = Int32.Parse(reader["CourseId"].ToString());
                    SqlConnection countconn = new SqlConnection(ConnectoionString);
                    countconn.Open();
                    string CountQuery = "SELECT COUNT(*) FROM CourseModules WHERE CourseId="+ courseId + "";
                    SqlCommand countcommand = new SqlCommand(CountQuery, countconn);
                    int count = (Int32)countcommand.ExecuteScalar();
                    countconn.Close();
                    var NewUser = new CourseModel
                    {
                        CourseId = courseId,
                        Name = reader["Name"].ToString(),
           
                    };
                    
                    courses.Add(NewUser);
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return courses;
        }

        public CourseModel GetCourse(int id)
        {
            CourseModel course = new CourseModel();
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "SELECT * FROM Courses WHERE CourseId ="+id+"";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if ((Int32.Parse(reader["CourseId"].ToString())) == id)
                    {
                        var user = new CourseModel
                        {
                            CourseId = id,
                            Name = reader["Name"].ToString()
                        };
                      

                        course = user;
                    }
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return course;
        }

        #endregion

    }
}