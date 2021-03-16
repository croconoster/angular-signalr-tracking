namespace WebAPI.Accounts.LoginUser
{
    public class LoginSuccessModel
    {
        public LoginSuccessModel(string userId, string token, string firstName, string lastName, string email, string role)            
        {
            this.UserId = userId;
            this.Token = token;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Role = role;
        }

        public string Token { get; }

        public string UserId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public string Role { get; set; }
    }
}
