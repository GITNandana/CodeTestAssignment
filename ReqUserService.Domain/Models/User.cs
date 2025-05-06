namespace ReqUserService.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Avatar { get; set; }
    }

    public class JsonData
    {
        public User Data { get; set; }
        public Support Support { get; set; }
    }

    
    public class Support
    {
        public string Url { get; set; }
        public string Text { get; set; }
    }

}
