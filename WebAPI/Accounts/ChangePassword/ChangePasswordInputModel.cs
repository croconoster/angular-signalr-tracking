namespace WebAPI.Accounts.ChangePassword
{
    public class ChangePasswordInputModel
    {
        public ChangePasswordInputModel(string userId, string currentPassword, string newPassword)
        {
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
        public string UserId { get; set; }
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
    }
}
