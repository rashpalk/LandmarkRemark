using System;
using TitleLandmarkLibrary;
using TitleLandmark_Models;
using TitleLandmark_Interfaces;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;

namespace TitleLandmark_Repo
{
    /// <summary>
    /// repo class for userlibrary
    /// </summary>
    public class UsersRepo:IUser
    {
       
        public int RegisterUser(UserModel userModel)
        {
            User user = new User();
            user.UserId = userModel.UserId;
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.UserName = userModel.UserName;
            user.Password = userModel.Password;
            user.ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
            int output = user.RegisterUser();
            return output;
        }

        public User GetUsers(int? userId, string userName, string password)
        {
            User user = new User();
            user.UserId = userId;
            user.UserName = userName;
            user.Password = password;            
            user.ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;            
            return user.GetUser().FirstOrDefault();
        }
    }

    /// <summary>
    /// repo class for userdatalibrary
    /// </summary>
    public class UsersDataRepo : IUserData
    {
        public int SaveUserData(UserDataModel userDataModel)
        {
            UserData usersData = new UserData();
            usersData.UserDataId = userDataModel.UserDataId;
            usersData.Longitude = userDataModel.Longitude;
            usersData.Latitude = userDataModel.Latitude;
            usersData.Label = userDataModel.Label;
            usersData.IsPublic = userDataModel.IsPublic;
            usersData.UserId = userDataModel.UserId;
            usersData.ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
            int output = usersData.SaveUserData();
            return output;
        }

        public List<UserData> GetUsersData(int? UserId, string userName, string searchString)
        {
            UserData usersData = new UserData();
            usersData.ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;            
            return usersData.GetUserData(UserId, userName,searchString);
        }
    }
}
