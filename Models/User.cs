namespace demo.Models
{
    public class User
    {
        public int user_id { get; set; }
        public string username { get; set; }
        public string password_hash { get; set; }
        public int role_id { get; set; }
    }
}