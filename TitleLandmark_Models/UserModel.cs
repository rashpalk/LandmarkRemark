using System;

namespace TitleLandmark_Models
{

    public class UserModel
    {
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserDataModel
    {
        public int? UserDataId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Label { get; set; }
        public bool IsPublic { get; set; }
        public int? UserId { get; set; }
      //  public string Name { get; set; }

    }
}
