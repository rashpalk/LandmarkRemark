using System;
using System.Collections.Generic;
using TitleLandmark_Models;
using TitleLandmarkLibrary;

namespace TitleLandmark_Interfaces
{
    public interface IUser
    {
        int RegisterUser(UserModel userModel);
        TitleLandmarkLibrary.User GetUsers(int? userId, string userName, string password);
    }
    public interface IUserData
    {
        int SaveUserData(UserDataModel userDataModel);
        List<TitleLandmarkLibrary.UserData> GetUsersData(int? UserId, string userName, string searchString);
    }
}
