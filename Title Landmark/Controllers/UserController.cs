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
    public class UserController : Controller
    {
        private readonly IUser _user;
        

        public UserController(IUser user)
        {
            _user = user;           
        }

        [HttpPost("Register")]
        public async Task<IActionResult> ReisterUser([FromBody]UserModel userModel)
        {
            try
            {
                Task<int> task = new Task<int>(() =>
                {
                    int output = _user.RegisterUser(userModel);
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

        [HttpGet("GetUser/{userId}/{userName}/{password}")]
        public async Task<IActionResult> GetUser(int? userId, string userName, string password)
        {
            try
            {
                Task<UserModel> task = new Task<UserModel>(() =>
                    {
                        UserModel userModel = new UserModel();                      

                        User user = _user.GetUsers(userId, userName, password);
                        if (user != null)
                        {
                            userModel.UserId = user.UserId;
                            userModel.FirstName = user.FirstName;
                            userModel.LastName = user.LastName;
                        }
                        return userModel;
                    });
                task.Start();
                return Ok(await task);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error" + ex.Message);
            }
        }
    }
}
