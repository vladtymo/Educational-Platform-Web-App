using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SS_EDUP.Core.Entities;
using SS_EDUP.Core.Interfaces;
using SS_EDUP.Core.ViewModels.User;
using SS_EDUP.Infrastructure.ViewModels.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.Services
{
    public class UserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private IConfiguration _configuration;
        private EmailService _emailService;
        private readonly IMapper _mapper;


        public UserService(EmailService emailService, IConfiguration configuration, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            var mappedUser = _mapper.Map<AppUser, EditUserVM>(user);

            return new ServiceResponse
            {
                Success = true,
                Message = "User logged in successfully.",
                Payload = mappedUser
            };

        }

        public async Task<ServiceResponse> LoginUserAsync(LoginUserVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User or password incorrect."
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return new ServiceResponse
                {
                    Success = true,
                    Message = "User logged in successfully."
                };
            }

            if (result.IsNotAllowed)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Confirm your email please."
                };
            }


            if (result.IsLockedOut)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User is locked. Connect with administrator."
                };
            }

            return new ServiceResponse
            {
                Success = false,
                Message = "User or password incorrect."
            };
        }

        public async Task<ServiceResponse> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Message = "No user associated with email",
                    Success = false
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_configuration["HostSettings:URL"]}/Admin/ResetPassword?email={email}&token={validToken}";
            string emailBody = "<h1>Follow the instructions to reset your password</h1>" + $"<p>To reset your password <a href='{url}'>Click here</a></p>";
            await _emailService.SendEmailAsync(email, "Fogot password", emailBody);

            return new ServiceResponse
            {
                Success = true,
                Message = $"Reset password for {_configuration["HostSettings:URL"]} has been sent to the email successfully!"
            };
        }

        public async Task<ServiceResponse> ResetPasswordAsync(ResetPasswordVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "No user associated with email",
                };
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Password doesn't match its confirmation",
                };
            }

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);
            if (result.Succeeded)
            {
                return new ServiceResponse
                {
                    Message = "Password has been reset successfully!",
                    Success = true,
                };
            }
            return new ServiceResponse
            {
                Message = "Something went wrong",
                Success = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

        public async Task<ServiceResponse> LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
            return new ServiceResponse()
            {
                Success = true,
                Message = "User logged out."
            };
        }

        public async Task<ServiceResponse> RegisterUserAsync(RegisterUserVM model)
        {

            if (model.Password != model.ConfirmPassword)
            {
                return new ServiceResponse
                {
                    Message = "Confirm pssword do not match.",
                    Success = false
                };
            }

            var mappesUser = _mapper.Map<RegisterUserVM, AppUser>(model);
            mappesUser.UserName = model.Email;
            var result = await _userManager.CreateAsync(mappesUser, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(mappesUser, model.Role);
                await SendConfirmationEmailAsync(mappesUser);
                return new ServiceResponse
                {
                    Success = true,
                    Message = "User successfully created."
                };
            }

            List<IdentityError> errorList = result.Errors.ToList();
            string errors = "";

            foreach (var error in errorList)
            {
                errors = errors + error.Description.ToString();
            }

            return new ServiceResponse
            {
                Success = false,
                Message = errors
            };
        }

        public async Task<ServiceResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User not found"
                };

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
                return new ServiceResponse
                {
                    Message = "Email confirmed successfully!",
                    Success = true,
                };

            return new ServiceResponse
            {
                Success = false,
                Message = "Email did not confirm",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task SendConfirmationEmailAsync(AppUser newUser)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            var encodedEmailToken = Encoding.UTF8.GetBytes(token);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            string url = $"{_configuration["HostSettings:URL"]}/Admin/ConfirmEmail?userid={newUser.Id}&token={validEmailToken}";

            string emailBody = $"<h1>Confirm your email</h1> <a href='{url}'>Confirm now</a>";
            await _emailService.SendEmailAsync(newUser.Email, "Email confirmation.", emailBody);
        }

        public async Task<ServiceResponse> GetUserProfileAsync(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User not found.",
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var mappesUser = _mapper.Map<AppUser, UserProfileVM>(user);
            mappesUser.Role = roles[0];
            return new ServiceResponse
            {
                Success = true,
                Message = "User profile loaded.",
                Payload = mappesUser
            };
        }

        public async Task<ServiceResponse> GetUserForSettingsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User not found.",
                };
            }
            var mappedUser = _mapper.Map<AppUser, UpdateProfileVM>(user);
            return new ServiceResponse
            {
                Success = true,
                Message = "User loaded.",
                Payload = mappedUser
            };
        }

        public async Task<ServiceResponse> UpdateProfileAsync(UpdateProfileVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Message = "User not found.",
                    Success = false
                };
            }

            if (model.Password != model.ConfirmPassword)
            {
                return new ServiceResponse
                {
                    Message = "Password do not match.",
                    Success = false
                };
            }


            if (user.Email != model.Email)
            {
                user.EmailConfirmed = false;
            }
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.UserName = model.Email;


            var changePassword = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
            if (changePassword.Succeeded)
            {

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await SendConfirmationEmailAsync(user);
                    await _signInManager.SignOutAsync();
                    return new ServiceResponse
                    {
                        Message = "User successfully updated.",
                        Success = true
                    };
                }
            }

            List<IdentityError> errorList = changePassword.Errors.ToList();
            string errors = "";

            foreach (var error in errorList)
            {
                errors = errors + error.Description.ToString();
            }
            return new ServiceResponse
            {
                Message = errors,
                Success = false
            };
        }

        public async Task<ServiceResponse> EditUserAsync(EditUserVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Message = "User not found.",
                    Success = false
                };
            }

            user.Surname = model.Surname;
            user.Name = model.Name;
            user.PhoneNumber= model.PhoneNumber;
            if(user.Email != model.Email)
            {
                user.EmailConfirmed= false;
                user.Email = model.Email;
                user.UserName = model.Email;
                await SendConfirmationEmailAsync(user);
            }
          
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
               
                return new ServiceResponse
                {
                    Message = "User successfully updated.",
                    Success = true
                };
            }

            List<IdentityError> errorList = result.Errors.ToList();
            string errors = "";

            foreach (var error in errorList)
            {
                errors = errors + error.Description.ToString();
            }
            return new ServiceResponse
            {
                Message = errors,
                Success = false
            };

        }

        public async Task<ServiceResponse> GetAllUsers()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();
            List<AllUsersVM> mappedUsers = users.Select(u => _mapper.Map<AppUser, AllUsersVM>(u)).ToList();

            for (int i = 0; i < users.Count; i++)
            {
                mappedUsers[i].Role = (await _userManager.GetRolesAsync(users[i])).FirstOrDefault();
            }
            return new ServiceResponse
            {
                Success = true,
                Message = "All users loaded",
                Payload = mappedUsers
            };

        }

        //public async Task<ServiceResponse> GetAuthenticationKeyAsync(string userId)
        //{
        //    var user = await _userManager.GetUserIdAsync(userId);
        //    await _userManager.ResetAuthenticatorKeyAsync(user);
        //    var token = await _userManager.GetAuthenticatorKeyAsync(user);
        //    var model = new TwoFactorAuthenticationVM() { Token= token };
        //    return 
        //}        //public async Task<ServiceResponse> GetAuthenticationKeyAsync(string userId)
        //{
        //    var user = await _userManager.GetUserIdAsync(userId);
        //    await _userManager.ResetAuthenticatorKeyAsync(user);
        //    var token = await _userManager.GetAuthenticatorKeyAsync(user);
        //    var model = new TwoFactorAuthenticationVM() { Token= token };
        //    return 
        //}
    }
}
