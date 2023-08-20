using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace core7_redis.Entities
{
    [Table("users")]
    public class User {

        [Key]
        [Column("id")]
        public int Id {get; set;}

        [Column("lastname")]
        public string LastName {get; set;}

        [Column("firstname")]
        public string FirstName {get; set;}

        [Column("email")]
        public string Email {get; set;}

        [Column("mobile")]
        public string Mobile {get; set;}

        [Column("username")]
        public string UserName {get; set;}

        [Column("password")]
        public string Password {get; set;}
    }    
}