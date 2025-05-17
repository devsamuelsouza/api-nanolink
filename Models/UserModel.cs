using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nanolink.Models
{
    public class UserModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N")[..6];
        public string User { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? DataCadastro { get; set; } = string.Empty;
        public char? Sex { get; set; }
        public string? Email { get; set; } = string.Empty;
    }
}