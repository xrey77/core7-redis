using core7_redis.Entities;
using core7_redis.Models;

namespace core7_redis.Services
{
    public interface IUserService {
        bool addUser(UserModel userModel);
        IEnumerable<User> GetAll();
        User GetById(int id);        
        void UpdateUser(UserModel usermodel, int id);
        void DeleteUser(int id);

    }
    
    public class UserService : IUserService {

        private DbContextClass _dbContext;

        public UserService(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }

        public bool addUser(UserModel userModel) {
            try {
                User userdata = new User();
                userdata.FirstName = userModel.FirstName;
                userdata.LastName = userModel.LastName;
                userdata.Email = userModel.Email;
                userdata.Mobile = userModel.Mobile;
                userdata.UserName = userModel.UserName;
                userdata.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
                _dbContext.Users.Add(userdata);
                _dbContext.SaveChanges();
                return true;
            } catch(Exception) {
                return false;
            }
        }

        public IEnumerable<User> GetAll()
        {
            var users = _dbContext.Users.ToList();
            return users;
        }

        public User GetById(int id)
        {
                var user = _dbContext.Users.Find(id);
                if (user is null) {
                    throw new Exception("User does'not exists....");
                }
                return user;
        }

        public void UpdateUser(UserModel userParam, int id)
        {
                var user = _dbContext.Users.Find(id);
                if (user is null)
                    throw new Exception("User not found");

                if (!string.IsNullOrWhiteSpace(userParam.FirstName)) {
                    user.FirstName = userParam.FirstName;
                }

                if (!string.IsNullOrWhiteSpace(userParam.LastName)) {
                    user.LastName = userParam.LastName;
                }

                if (!string.IsNullOrWhiteSpace(userParam.Mobile)) {
                    user.Mobile = userParam.Mobile;
                }

                if (!string.IsNullOrWhiteSpace(userParam.Password))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(userParam.Password);

                }
                // user.Date_updated = DateTime.Now;
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();            
        }

        public void DeleteUser(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user is not null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
            else {
               throw new Exception("User not found");
            }   
        }


    }
}