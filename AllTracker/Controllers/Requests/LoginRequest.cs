using System.ComponentModel.DataAnnotations;

namespace AllTracker.Controllers.Requests
{
    /// <summary>
    /// A request to attempt authenticating an <see cref="User"/>
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Identifier of the <see cref="User"/> to authenticate
        /// </summary>
        /// <remarks>
        /// UserName or Email property of an <see cref="User"/>
        /// </remarks>
        [Required]
        [Display(Name = "Username")]
        public string Identifier { get; set; } = "";

        /// <summary>
        /// The Password to attempt to authenticate the <see cref="User"/> with
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; } = false;
    }
}
