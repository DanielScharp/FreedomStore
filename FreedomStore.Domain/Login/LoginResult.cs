using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomStore.Domain.Login
{
    public class LoginResult
    {
        public int Id { get; set; }
        public string? Nickname { get; set; }
        public string? IpOrigem { get; set; }
        public int? UserType { get; set; }
        public string? Token { get; set; }
    }
}
