namespace core7_redis.Models
{
        public class UserDto {

                public string Id {get; set;}

                public string LastName {get; set;}

                public string FirstName {get; set;}

                public int Email {get; set;}

                public string Mobile {get; set;}

                public string UserName {get; set;}

                public string Password {get; set;}  
                public List<string> Itemz { get; set; }
  
        }
}