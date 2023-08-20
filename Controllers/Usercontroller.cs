using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;  
using core7_redis.Models;
using core7_redis.Entities;
using core7_redis.Cache;
using core7_redis.Services;

namespace core7_redis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase {

        private readonly IDistributedCache _distributedCache;
        private readonly IUserService _userService;
        // private readonly ICacheService _cacheService;

        public UserController(
            IUserService userService,
            // ICacheService cacheService,
            IDistributedCache distributedCache            
            )
        {
            _userService = userService;
            _distributedCache = distributedCache;
            // _cacheService = cacheService;
        }

        [HttpPost("/user")]
        public async Task<IActionResult> PostUser([FromBody] UserModel userModel) {
            var userdata = _userService.addUser(userModel);
            if (userdata is true) {

                var expiration = DateTime.Now.Date.AddDays(5);
                var totalSeconds = expiration.Subtract(DateTime.Now).TotalSeconds;

                var distributedCacheEntryOptions = new DistributedCacheEntryOptions();
                distributedCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(totalSeconds);
                distributedCacheEntryOptions.SlidingExpiration = null;

                var jsonData = JsonConvert.SerializeObject(userModel);
                await _distributedCache.SetStringAsync("User", jsonData, distributedCacheEntryOptions);

                return Ok(new {statuscode = 200, message="New User successfully added."});
            } else {
                return BadRequest(new {statuscode = 500, message="Unable to add new user."});
            }
        }


        // private IQueryable<UserDto> GetUsrs()
        // {
        //     var newdata = from u in _userService.GetAll()
        //         select new UserDto()
        //         {
        //             Id = u.Id,
        //             FirstName = u.FirstName
        //         };
        //     return newdata;
        // }

        [HttpGet("/getusers")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetUsers() {
        // public IActionResult GetUsers() {
            // return Ok(users);
            // _distributedCache.Remove("User");
            // return null;

            // var jsonSettings = new JsonSerializerSettings()
            // {
            //     MissingMemberHandling = MissingMemberHandling.Error
            // };

            var jsonData = await _distributedCache.GetStringAsync("User");
            if (jsonData is not null) {
                try {
                    var dashboardData = JsonConvert.DeserializeObject<User>(jsonData);
                    return Ok(dashboardData);
                } catch(Exception) {}
            }
            Console.WriteLine("empty....");

            var users =  _userService.GetAll();   
            

            var expiration = DateTime.Now.Date.AddDays(5);
            var totalSeconds = expiration.Subtract(DateTime.Now).TotalSeconds;

            var distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(totalSeconds);
            distributedCacheEntryOptions.SlidingExpiration = null;

    
            // return Ok(System.Text.Json.JsonSerializer.Serialize(xdata));
            // string jsonData2 = "";
            List<User> newdata = new List<User>();
            foreach (var item in users)
            {
                var data = new User
                {
                    Id=item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    Mobile = item.Mobile,
                    UserName = item.UserName,
                    Password = item.Password
                };
                newdata.Add(data);
            };
            string jsonData2 = JsonConvert.SerializeObject(newdata);
            await _distributedCache.SetStringAsync("User", jsonData2);

            return Ok(users);
            // var jsonData2 = Newtonsoft.Json.JsonConvert.SerializeObject(newdata);
            // // var jsonData2 = JsonConvert.SerializeObject(newdata);                                
            // await _distributedCache.SetStringAsync("User", jsonData2, distributedCacheEntryOptions);
            // var dashboardData2 = JsonConvert.DeserializeObject<List<UserDto>>(jsonData2);
            // var dashboardData2 = JsonConvert.DeserializeObject<User>(jsonData2);
            // return Ok(dashboardData2);
        }

        [HttpGet("/resetcache")]
        public IActionResult ResetCache() {
            _distributedCache.Remove("User");
            return Ok(new {statuscode = 200, message ="Distribued Cache has been reset."});
        }

        // [HttpGet("/getuserbyid/{id}")]
        // public User Get(int id) {
        //     User filteredData;
        //     var cacheData = _cacheService.GetData < IEnumerable < User >> ("user");
        //     if (cacheData != null) {
        //         filteredData = cacheData.Where(x => x.Id == id).FirstOrDefault();
        //         return filteredData;
        //     }
        //     filteredData = _userService.GetById(id);
        //     return filteredData;
        // }

        // [HttpPut("updateuser/{id}")]
        // public IActionResult PutUser([FromBody]UserModel user, int id) {
        //     try {
        //         _userService.UpdateUser(user, id);
        //         _cacheService.RemoveData("user");
        //         return Ok(new {statuscode = 200, message = "User updated successfully."});
        //     } catch(Exception) {
        //         return Ok(new {statuscode = 404, message = "User not found."});
        //     }
        // }

        // [HttpDelete("deleteuser/{id}")]
        // public IActionResult Delete_User(int id) {
        //     try {
        //         _userService.DeleteUser(id);
        //         _cacheService.RemoveData("user");
        //         return Ok(new {statuscode = 200, message = "User has deleted successfully."});
        //     } catch(Exception) {
        //         return NotFound(new {statuscode = 404, message = "User not found."});
        //     }
        // }

    }    
}