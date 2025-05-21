using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomStore.Domain.Login
{
    public class Login
    {
        public string? Nickname { get; set; }
        public string? Password { get; set; }
        public string? IpOrigem { get; set; }
        public string? Email { get; set; }
    }
}
