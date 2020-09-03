namespace Ingaia.Challenge.WebApi.Models
{
    public class UserModel
    {
        public UserModel(string username, string password)
        {
            Username = username;
            Password = password;
            Role = "regular";
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
