namespace UserManagementApi.DTOs.User
{
    public class UserDTO
    {
        public Guid? UserId { get; set; }

        public string FullName { get; set; } = null!;

        public string PasscodeHash { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string PasswordSalt { get; set; } = null!;

        public Guid LastActionUserId { get; set; }

        public Guid RoleId { get; set; }
    }
}
