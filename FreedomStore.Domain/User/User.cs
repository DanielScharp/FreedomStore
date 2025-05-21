using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomStore.Domain.User
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(45)]
        public string? Nickname { get; set; }
        [MaxLength(45)]
        public string? Password { get; set; }
        [MaxLength(200)]
        public string? Name { get; set; }
        [MaxLength(200)]
        public string? Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int UserType { get; set; }
        public bool IsActive { get; set; }
        
        public DateTime RegistrationDate { get; set; }
        [MaxLength(45)]
        public string? RegistrationIPAddress { get; set; }
        public Address? Address { get; set; }
    }

    public class Address
    {
        [MaxLength(200)]
        public string? Street { get; set; }
        [MaxLength(20)]
        public string? Number { get; set; }
        [MaxLength(100)]
        public string? Complement { get; set; }
        [MaxLength(45)]
        public string? Neighborhood { get; set; }
        [MaxLength(100)]
        public string? City { get; set; }
        [MaxLength(50)]
        public string? State { get; set; }
        [MaxLength(20)]
        public string? PostalCode { get; set; }
        [MaxLength(50)]
        public string? Country { get; set; } = "Brasil";
    }
}
