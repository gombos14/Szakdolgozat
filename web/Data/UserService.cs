using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;


namespace Szakdolgozat.Data
{
    public class UserService
    {
        #region Property
        private readonly AppDBContext _appDbContext;
        #endregion

        public UserService(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public User CreateUser(string firstName, string lastName, string email, string password, string phoneNumber)
        {
            // Create a new user instance
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            // Hash the password and set it on the user instance
            var hashedPassword = HashPassword(password);
            user.SetPassword(hashedPassword);
            return user;
        }

        public async Task<bool> InsertUserAsync(User user)
        {
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            // Retrieve the user from the database or another storage location
            var user = GetUserByEmail(email);

            // Verify the password using the PasswordHashingService
            var isPasswordValid = VerifyPassword(password, user.Password);

            if (isPasswordValid)
            {
                return user;
            }
            else
            {
                throw new Exception("Login failed");
            }
        }
        
        public string HashPassword(string password)
        {
            const int SaltWorkFactor = 10;
            
            // Generate a salt for the password
            var salt = BCryptNet.GenerateSalt(SaltWorkFactor);

            // Hash the password using the generated salt
            var hashedPassword = BCryptNet.HashPassword(password, salt);

            return hashedPassword;
        }
        
        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Verify that the provided password matches the hashed password
            var isPasswordValid = BCryptNet.Verify(password, hashedPassword);

            return isPasswordValid;
        }

        public User GetUserByEmail(string email)
        {
            return _appDbContext.Users.FirstOrDefault(user => user.Email.Equals(email));
        }
    }
}

