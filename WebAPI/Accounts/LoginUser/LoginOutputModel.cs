namespace WebAPI.Accounts.LoginUser
{
    public class LoginOutputModel : UserOutputModel
    {
        public LoginOutputModel(string token, string id, string firstName, string lastName, string email, string role)
            :base(id,firstName,lastName,email,role)
        {
            Token = token;
        }

        public string Token { get; }
    }
}