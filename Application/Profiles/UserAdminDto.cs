namespace Application.Profiles
{
    public class UserAdminDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsBanned { get; set; }
    }
}