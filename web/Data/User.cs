using System.ComponentModel.DataAnnotations;


namespace Szakdolgozat.Data
{
    public enum Role { Admin, User, Guest, Worker }
    public class User
    {
        [Key]
        public int Id { get; set; }
        public  string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Role UserRole { get; set; }
        
        public void SetPassword(string password)
        {
            this.Password = password;
        }
    }
}

