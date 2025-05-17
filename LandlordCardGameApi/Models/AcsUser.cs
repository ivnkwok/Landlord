namespace LandlordCardGameApi.Models
{
    public enum UserRoles
    {
        Owner,
        Guest
    }

    public class AcsUser
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }

        public UserRoles Role { get; set; }
    }
}
