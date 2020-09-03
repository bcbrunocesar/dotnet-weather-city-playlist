namespace Ingaia.Challenge.WebApi.Entities
{
    public class UserEntity
    {
        public UserEntity(string username)
        {
            Username = username;
            Role = "regular";
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
