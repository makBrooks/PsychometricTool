namespace PsychometricWeb.Models
{
    public class Psychometric
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? ID { get; set; }
        public string? Action { get; set; }
        public string[]? A { get; set; }
        public string[]? B { get; set; }
        public string[]? C { get; set; }
        public string[]? D { get; set; }
        public string[]? Most { get; set; }
        public string[]? Least { get; set; }
        
    }
    public class Psychometriclist
    {
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
        public string? D { get; set; }
        public string? Most { get; set; }
        public string? Least { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

    }
    public class Login
    {
        public string? userName { get; set; }
        public string? Password { get; set; }
    }
    public class UsersDto
    {
        public string UID { get; set; } = "";
        public string? FULLNAME { get; set; } = "";
        public string? UName { get; set; } = "";
        public string? Phone { get; set; } = "";
        public string? UserType { get; set; }
        public string? Password { get; set; }
        public int Role { get; set; }
    }
    public class Registration
    {
        public string UNAME { get; set; } = "";
        public string? FULLNAME { get; set; } = "";
        public string? PASSWORD { get; set; } = "";
        public string? EMAIL { get; set; } = "";
        public string? PHONE { get; set; } = "";
        public string? ADHARNUMBER { get; set; } = "";
        public IFormFile ADHARUPLOAD { get; set; }
        public string? UPLOADPATH { get; set; }
        public int Role { get; set; }
    }
    public class JsonResponse
    {
        public int statuscode { get; set; }
        public string? status { get; set; }
        public string? msg { get; set; }
        public Object? Data { get; set; }
    }
}
