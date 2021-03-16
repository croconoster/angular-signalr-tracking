namespace WebAPI.Accounts
{
    public abstract class UserInputModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}