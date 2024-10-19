using League.Data.Entities;
using League.Helpers;
using League.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace League.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly IBlobHelper _blobHelper;

        public AccountController(
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IConfiguration configuration,
            UserManager<User> userManager,
            IBlobHelper blobHelper)
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _configuration = configuration;
            _userManager = userManager;
            _blobHelper = blobHelper;
        }


        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Username);

                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        //TODO: FAZER A VIEW DO DASHBOARD E ALTERAR O CAMINHO 
                        return this.RedirectToAction("DashBoard", "Admin");
                    }

                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }


                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to Login");
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterGuestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    Guid imageId = Guid.Empty;

                    if(model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    }

                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.Username
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    await _userHelper.AddUserToRoleAsync(user, "Guest");

                    var isInRole = await _userHelper.IsUserInRoleAsync(user, "Guest");
                    if (!isInRole)
                    {
                        await _userHelper.AddUserToRoleAsync(user, "Guest");
                    }

                    string myToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    //TODO : FAZER ENVIO DO EMAIL PERSONALIZADO
                    Response response = _mailHelper.SendEmail(model.Username, "Email Confirmation",
                        $"<h1>Email Confirmation</h1>To confirm your email, click this link: <a href=\"{tokenLink}\">Confirm Email</a>");

                    if (response.IsSuccess)
                    {
                        ViewBag.Message = "The instructions to allow your user has been sent to email.";
                        return View(model);
                    }

                    ModelState.AddModelError(string.Empty, "The user couldn't be logged");
                }
            }

            return View(model);

        }


        public IActionResult RegisterNewUser()
        {
            ViewBag.Roles = new SelectList(new List<string>
            {
                "Admin",
                "FederationEmpolyee",
                "ClubRepresentant",
            });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewUser(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    }

                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.Username,
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    await _userHelper.AddUserToRoleAsync(user, model.SelectedRole);

                    string myToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    //TODO : FAZER ENVIO DO EMAIL PERSONALIZADO
                    Response response = _mailHelper.SendEmail(model.Username, "Email Confirmation",
                        $"<h1>Email Confirmation</h1>To confirm your email, click this link and reset the password: <a href=\"{tokenLink}\">Confirm Email</a>");

                    if (response.IsSuccess)
                    {
                        ViewBag.Message = "The instructions to allow the user has been sent to email.";
                        return View(model);
                    }

                    ModelState.AddModelError(string.Empty, "The user couldn't be logged");
                }
            }

            return View(model);
        }


        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.PhoneNumber = user.PhoneNumber;
                model.ImageId = user.ImageId;
            }


            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;

                    if(model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                        user.ImageId = imageId;
                    }

                    var response = await _userHelper.UpdateUserAsync(user);
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(model);
        }


        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(model);
        }


        public async Task<IActionResult> ConfirmEmail(string userid, string token)
        {
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userid);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            var resetPasswordToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

            return RedirectToAction("ResetPassword", new {userId = user.Id, token = resetPasswordToken});
        }


        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }


        public async Task <IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        public IActionResult RecoverPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email is not registered.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                var tokenLink = Url.Action("ResetPassword", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "Password Recover",
                    $"<h1>Password Reset</h1>To reset the password click in this link: <a href=\"{tokenLink}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    ViewBag.Message = "The instructions to recover your password has been sent to email.";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "The email couldn't be sent.");
            }

            return View(model);

        }



        public IActionResult ResetPassword(string token)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User Not Found!");
                    return View(model);
                }

                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Password reset successful.";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "The password couldn't be reset.");
            }

            return View(model);
        }
    }
}
