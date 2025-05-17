using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nanolink.Models
{
    public class UrlModel
    {
        public string UrlCurta { get; set; } = Guid.NewGuid().ToString("N")[..6];
        public string IdUser { get; set; } = string.Empty;
        public string NameUrl { get; set; } = string.Empty;
        public string UrlLonga { get; set; } = string.Empty;
    }
}