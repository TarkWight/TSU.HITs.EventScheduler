using System.ComponentModel.DataAnnotations;

namespace EventScheduler.API.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
