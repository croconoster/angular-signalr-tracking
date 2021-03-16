namespace WebAPI.Accounts.UpdateUser
{
    public class UpdateUserInputModel
    {
        public UpdateUserInputModel(string userId, string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            UserId = userId;
            Email = email;
        }

        public string UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
    }
}
