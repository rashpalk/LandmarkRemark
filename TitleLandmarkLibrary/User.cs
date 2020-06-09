using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace TitleLandmarkLibrary
{
    /// <summary>
    /// Users class deals with user table and responsible for inserting , updating and finding a user
    /// </summary>
    public class User
    {
        
        public int? UserId { get; set; }        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string UserName { get; set; }
        public string Password { get; set; }

        public string ConnectionString { get; set; }

        public User()
        {
         
        }
        public User(IDataReader reader)
        {
            UserId =Convert.ToInt32(reader["UserId"]);
            FirstName = reader["FirstName"].ToString();
            LastName = reader["LastName"].ToString();
        }


        public  int RegisterUser()
        {
            int Output = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertUpdateUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = LastName;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UserName;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;                   

                    SqlParameter outPutParameter = new SqlParameter();
                    outPutParameter.ParameterName = "@Output";
                    outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                    outPutParameter.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(outPutParameter);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    Output =Convert.ToInt16(outPutParameter.Value);                    
                }
            }
            return Output;
        }

        public  IEnumerable<User> GetUser()
        {
           List<User> response = new List<User>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UserName;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;                    
                    con.Open();                   

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Add(
                                new User(reader)
                            );
                        }
                    }
                }
            }
            return response;
        }
    }

    /// <summary>
    /// UserData class deals with userdata table and responsible for inserting , updating and finding a user data i.e. notes corresponsing to its location
    /// </summary>
    public class UserData
    {

        public int? UserDataId { get; set; }
        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public string Label { get; set; }
        public bool IsPublic { get; set; }

        public int? UserId { get; set; }

        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public UserData()
        {

        }
        public UserData(IDataReader reader)
        {
            UserDataId = Convert.ToInt32(reader["UserDataId"]);
            Longitude = reader["Longitude"].ToString();
            Latitude = reader["Latitude"].ToString();
            Label = reader["Name"].ToString() + ":" + reader["Label"].ToString();
            Name = reader["Name"].ToString();
            UserId = Convert.ToInt32(reader["UserId"]);
        }


        public int SaveUserData()
        {
            int Output = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertUpdateUserData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserDataId", SqlDbType.Int).Value = UserDataId;
                    cmd.Parameters.Add("@Longitude", SqlDbType.VarChar).Value = Longitude;
                    cmd.Parameters.Add("@Latitude", SqlDbType.VarChar).Value = Latitude;
                    cmd.Parameters.Add("@Label", SqlDbType.VarChar).Value = Label;
                    cmd.Parameters.Add("@IsPublic", SqlDbType.VarChar).Value = IsPublic;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;

                    SqlParameter outPutParameter = new SqlParameter();
                    outPutParameter.ParameterName = "@Output";
                    outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                    outPutParameter.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(outPutParameter);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    Output = Convert.ToInt16(outPutParameter.Value);
                }
            }
            return Output;
        }

        public List<UserData> GetUserData(int? UserId, string userName, string searchString)
        {
            List<UserData> response = new List<UserData>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetUserData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userName;
                    cmd.Parameters.Add("@SearchString", SqlDbType.VarChar).Value = searchString;
                    con.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Add(
                                new UserData(reader)
                            );
                        }
                    }
                }
            }
            return response;
        }
    }
}
