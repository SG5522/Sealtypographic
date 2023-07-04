using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DJImageAuthorizationServer.Models
{
    public class ErrorViewModel
    {
        [Display(Name = "Error")]
        public string Error { get; set; }

        [Display(Name = "Description")]
        public string ErrorDescription { get; set; }
    }
}