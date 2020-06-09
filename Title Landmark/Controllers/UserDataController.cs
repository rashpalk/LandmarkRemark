using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TitleLandmark_Interfaces;
using TitleLandmark_Models;
using TitleLandmarkLibrary;



namespace Title_Landmark.Controllers
{
    [Route("api/[controller]")]
    public class UserDataController : Controller
    {
        private readonly IUserData _userData;

        public UserDataController(IUserData userData)
        {
            _userData = userData;
        }

        [HttpPost("SaveUserData")]
        public async Task<IActionResult> SaveUserData([FromBody]UserDataModel userDataModel)
        {
            try
            {
                Task<int> task = new Task<int>(() =>
                {
                    int output = _userData.SaveUserData(userDataModel);
                    return output;
                });
                task.Start();
                return Ok(await task);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "bad request" + ex.Message);
            }
        }

        [HttpGet("GetUsersData/{UserId}/{userName}/{searchString}")]
        public async Task<IActionResult> GetUsersData(int? userId, string userName, string searchString )
        {
            try
            {
                Task<IEnumerable<UserDataModel>> task = new Task<IEnumerable<UserDataModel>>(() =>
                {

                    List<UserData> userRecords = _userData.GetUsersData(userId, userName, searchString);
                    List<UserDataModel> userRecordsModel = new List<UserDataModel>();

                    foreach (UserData data in userRecords)
                    {
                        UserDataModel userRecordModel = new UserDataModel();
                        userRecordModel.UserDataId = data.UserDataId;
                        userRecordModel.Longitude = data.Longitude;
                        userRecordModel.Latitude = data.Latitude;
                        userRecordModel.Label = data.Label;
                        userRecordModel.IsPublic = data.IsPublic;
                        userRecordsModel.Add(userRecordModel);
                    }                 

                    return userRecordsModel;
                });

                task.Start();
                return Ok(await task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error"+ex.Message);
            }
        }
    }
}
