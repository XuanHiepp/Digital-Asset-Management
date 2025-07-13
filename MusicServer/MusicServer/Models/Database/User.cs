using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database
{
    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public int userType { get; set; }
        public bool IsEmailVerified { get; set; }
        public Guid ActivationCode { get; set; }
        public string ConfirmPassword { get; set; }
        public string ResetPasswordCode { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerPrivateKey { get; set; }
    }
}
