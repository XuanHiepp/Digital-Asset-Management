using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicServer.Models.Database;
using MusicServer.Models.Database.KPI;
using MusicServer.Repositories.Interfaces;
using MusicServer.Services.Interfaces;

namespace MusicServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IPKIEncordeService pkiEncordeService;
        public readonly MusicDBContext dbContext;

        public UserAuthController(IUserRepository userRepository,
            IPKIEncordeService pkiEncordeService,
            MusicDBContext dbContext)
        {
            this.userRepository = userRepository;
            this.pkiEncordeService = pkiEncordeService;
            this.dbContext = dbContext;
        }

        //Registration POST action 
        [HttpPost("Registration")]
        public IActionResult Registration(UserRegis userlogin)
        {
            bool Status = false;
            string message = "";
            //
            // Model Validation 
            if (ModelState.IsValid)
            {
                User user = new User();
                user.EmailID = userlogin.EmailID;
                #region //Email is already Exist 
                var isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                #endregion

                user.FirstName = userlogin.FirstName;
                user.LastName = userlogin.LastName;
                user.DateOfBirth = userlogin.DateOfBirth;
                user.Password = userlogin.Password;
                user.ConfirmPassword = userlogin.ConfirmPassword;
                user.userType = userlogin.userType;

                #region Generate Activation Code 
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region  Password Hashing 
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //
                #endregion
                user.IsEmailVerified = false;

                #region Save to Database
                userRepository.Create(user);

                //Send Email to User
                SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                message = "Registration successfully done. Account activation link " +
                    " has been sent to your email id:" + userlogin.EmailID;
                Status = true;
                #endregion
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                message = "Invalid Request";
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }


        }
        //Verify Account  

        [HttpGet("VerifyAccount")]
        public IActionResult VerifyAccount(string id)
        {
            bool Status = false;
                                                            // Confirm password does not match issue on save changes
            var v = dbContext.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
            if (v != null)
            {
                v.IsEmailVerified = true;
                dbContext.SaveChanges();
                Status = true;
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Invalid Request");
            }
            
        }

        //Login POST
        [HttpPost("Login")]
        public IActionResult Login(UserLogin login)
        {
            string message = "";

            var v = dbContext.Users.Where(a => a.EmailID == login.EmailID).FirstOrDefault();
            if (v != null)
            {
                if (!v.IsEmailVerified)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Please verify your email first");
                }

                if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                {
                    return Ok(new { UserID = v.UserID, UserType = v.userType });
                }
                else
                {
                    return Ok(new { UserID = 0, UserType = 0 });
                }
            }
            else
            {
                //message = "Invalid credential provided";
                //return StatusCode(StatusCodes.Status500InternalServerError, message);
                return Ok(new { UserID = 0, UserType = 0 });
            }
        }

        ////Logout
        //[HttpGet]
        //public ActionResult Logout()
        //{
        //    FormsAuthentication.SignOut();
        //    Session[Logined] = 0;
        //    Session.Remove("UserID");
        //    return RedirectToAction("Login", "User");
        //}


        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            var v = dbContext.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
            return v != null;
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var fromEmail = new MailAddress("kevinmash98@gmail.com", "Dotnet Awesome");
            var toEmail = new MailAddress(emailID);
            //var fromEmailPassword = "********"; // Replace with actual password

            string subject = "";
            string body = "";

            if (emailFor == "VerifyAccount")
            {
                
                var link = "http://localhost:4200/music/user-verify/" + activationCode;

                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                var link = "https://localhost:5001/api/UserAuth/ResetPassword?id=" + activationCode;

                subject = "Reset Password";
                body = "Hi,<br/><br/>We got request for reset your account password." +
                    " Please click on the below link to reset your account" +
                    " <br/><br/><a href=" + link + ">Reset Password Link</a>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.mailtrap.io",
                Port = 2525,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("468108b73e9201", "b91c10bc7669a7")
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(string EmailID)
        {
            string message = "";
            bool status = false;
            var account = dbContext.Users.Where(a => a.EmailID == EmailID).FirstOrDefault();
            if (account != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(account.EmailID, resetCode, "ResetPassword");
                account.ResetPasswordCode = resetCode;

                dbContext.SaveChanges();
                message = "Reset password link has been sent to your email address!";
                return StatusCode(StatusCodes.Status200OK, message);
            }
            else
            {
                message = "Account not found!";
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            
        }

        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword(string id)
        {
            string message = "";
            var account = dbContext.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
            if (account != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return Ok(new { user = model });
            }
            else
            {
                message = "HttpNotFound not found!";
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpPost("ResetPassword")]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            string message = "";
            bool status = false;

            if (ModelState.IsValid)
            {
                var account = dbContext.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                if (account != null)
                {
                    account.Password = Crypto.Hash(model.NewPassword);
                    account.ResetPasswordCode = "";

                    dbContext.SaveChanges();
                    message = "New password updated successfully!";
                    return StatusCode(StatusCodes.Status200OK, message);
                }
                else
                {
                    message = "Account not found!";
                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
            }
            else
            {
                message = "Something Invalid!";
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpGet("GetOwnerInfo/{UserID}")]
        public IActionResult GetOwnerInfo(int UserID)
        {
            // Confirm password does not match issue on save changes
            var v = dbContext.Users.Where(a => a.UserID == UserID).FirstOrDefault();
            if (v.OwnerAddress != null && v.OwnerAddress != "")
            {
                return Ok(new { ownerAddress = v.OwnerAddress, ownerPrivateKey = v.OwnerPrivateKey });
            }
            else
            {
                return Ok(new { ownerAddress = "" });
            }
        }

        [HttpPost("UpdateOwnerAddress")]
        public IActionResult UpdateOwnerAddress(OwnerAddressInfo ownerAddr)
        {
            string message = "";
            var v = dbContext.Users.Where(a => a.UserID == ownerAddr.UserID).FirstOrDefault();
            if (v != null)
            {
                v.OwnerAddress = ownerAddr.OwnerAddress;
                dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, message);
            }
            else
            {
                message = "Error occur!";
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

        }

        [HttpPost("UpdateOwnerPrivateKey")]
        public IActionResult UpdateOwnerPrivateKey(OwnerPrivateKeyInfo ownerPrKey)
        {
            string message = "";
            var v = dbContext.Users.Where(a => a.UserID == ownerPrKey.UserID).FirstOrDefault();
            if (v != null)
            {
                v.OwnerPrivateKey = ownerPrKey.OwnerPrivateKey;
                dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, message);
            }
            else
            {
                message = "Error occur!";
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

        }



        [HttpGet("KPITest")]
        public IActionResult KPITest()
        {
             pkiEncordeService.GenerateGemFileForKPI(commonName: "mail.google.com",
                            organization: "Google Inc.",
                            organizationalUnit: "Information Technology",
                            locality: "San Diego",
                            state: "California",
                            countryIso2Characters: "US",
                            emailAddress: "support@google.com",
                            signatureAlgorithm: SignatureAlgorithm.SHA256,
                            rsaKeyLength: RsaKeyLength.Length2048Bits);

             return StatusCode(StatusCodes.Status200OK);

        }
    }
}