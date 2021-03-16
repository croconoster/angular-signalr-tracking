using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Accounts;
using WebAPI.Accounts.ChangePassword;
using WebAPI.Accounts.CreateUser;
using WebAPI.Accounts.LoginUser;
using WebAPI.Accounts.UpdateUser;
using WebAPI.Common;
using WebAPI.Data.Models;

namespace WebAPI.Infrastructure
{
    public interface IUser
    {

    }

    public interface IIdentity
    {
        Task<Result<LoginSuccessModel>> Login(UserInputModel userInput);

        Task<Result<IUser>> Create(CreateUserInputModel createUserInput);

        Task<Result> Update(UpdateUserInputModel updateUserInput);

        Task<Result> ChangePassword(ChangePasswordInputModel changePasswordInput);

        Task<Result<IEnumerable<UserOutputModel>>> GetAllUsersAsync(string currentUserId);

        Task<Result<UserOutputModel>> GetUsersByIdAsync(string userId);
    }


    public class IdentityService : IIdentity
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public IdentityService(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleManager = roleManager;
        }

        public async Task<Result> ChangePassword(ChangePasswordInputModel changePasswordInput)
        {
            User user = await _userManager.FindByIdAsync(changePasswordInput.UserId);

            if (!await _userManager.CheckPasswordAsync(user, changePasswordInput.CurrentPassword))
            {
                return "Could not change your password because your current password is inccorect";
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, changePasswordInput.CurrentPassword, changePasswordInput.NewPassword);

            if (!result.Succeeded)
            {
                return result.Errors.Select(x => $"{x.Code} - {x.Description}").ToList();
            }

            return true;
        }

        public async Task<Result<LoginSuccessModel>> Login(UserInputModel userInput)
        {
            User user = await _userManager.FindByNameAsync(userInput.Email);

            if(user == null)
            {
                return "Could not find a user with the given email address";
            }

            if (!await _userManager.CheckPasswordAsync(user, userInput.Password))
            {
                return "Could not log in becuase your password is incorrect";
            }

            IList<string> roles = await _userManager.GetRolesAsync(user) ?? new List<string>();

            string token = _jwtTokenGenerator.GenerateToken(user, roles);

            return new LoginSuccessModel(user.Id, token, user.FirstName, user.LastName, user.Email, roles.FirstOrDefault());
        }

        public async Task<Result<IUser>> Create(CreateUserInputModel createUserInput)
        {
            User user = await _userManager.FindByNameAsync(createUserInput.Email);

            if (user != null)
            {
                return "User with this email address already exists";
            }

            user = new User(createUserInput.FirstName, createUserInput.LastName, createUserInput.Email);

            IdentityResult result = await _userManager.CreateAsync(user, createUserInput.Password);

            if (!result.Succeeded)
            {
                return result.Errors.Select(x => $"{x.Code} - {x.Description}").ToList();
            }

            // We need to introduce domain events so we can fire off a role creation
            if (await _userManager.Users.CountAsync() == 1)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            if (!result.Succeeded)
            {
                return result.Errors.Select(x => $"{x.Code} - {x.Description}").ToList();
            }

            return user;
        }

        public async Task<Result> Update(UpdateUserInputModel updateUserInput)
        {
            string userid = updateUserInput.UserId;

            User user = await _userManager.FindByIdAsync(userid);

            if (user == null)
            {
                return "Coult not find user to update details for";
            }

            Result result = await UpdateEmail(user, updateUserInput.Email);
            if (!result.Succeeded)
            {
                return result;
            }

            if (!string.IsNullOrWhiteSpace(updateUserInput.FirstName))
            {
                user.FirstName = updateUserInput.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(updateUserInput.LastName))
            {
                user.LastName = updateUserInput.LastName;
            }

            await _userManager.UpdateAsync(user);

            return true;
        }

        private async Task<Result> UpdateEmail(User user, string email)
        {
            if (!string.IsNullOrWhiteSpace(email) && user.UserName != email)
            {
                User checkUserEmail = await _userManager.FindByEmailAsync(email);

                if (checkUserEmail != null && checkUserEmail.Id != user.Id)
                {
                    return "The provided email is already taken";
                }

                User checkUserUsername = await _userManager.FindByNameAsync(email);
                if (checkUserUsername != null && checkUserUsername.Id != user.Id)
                {
                    return "The provided email is already taken";
                }

                user.Email = email;
                user.UserName = email;
            }

            return true;
        }
    
        public async Task<Result<IEnumerable<UserOutputModel>>> GetAllUsersAsync(string currentUserId)
        {
            User user = await _userManager.FindByIdAsync(currentUserId);

            if(user == null)
            {
                return "Provided user with id does not exist";
            }


            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return "Only admin users are allowed to see all users";
            }

            IList<User> users = await _userManager.Users.ToListAsync();

            return users.Select(x => new UserOutputModel(x.Id, x.FirstName, x.LastName, x.Email, GetRole(x).Result)).ToList();
        }

        private async Task<string> GetRole(User x)
        {
            return (await _userManager.GetRolesAsync(x)).FirstOrDefault();
        }

        public async Task<Result<UserOutputModel>> GetUsersByIdAsync(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return "Provided user with id does not exist";
            }

            return new UserOutputModel(user.Id, user.FirstName, user.LastName, user.Email, await GetRole(user));
        }
    }
}
