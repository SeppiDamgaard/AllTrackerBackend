namespace AllTracker.Requests.Responses
{
    public class LoginResponse
    {
        /// <summary>
        /// Id of the newly logged in <see cref="Employee"/>
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// UserName of the newly logged in <see cref="Employee"/>
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// JWT token attached to the logged in <see cref="Employee"/>
        /// </summary>
        /// <remarks>
        /// Used to authenticate all subsequent requests
        /// </remarks>
        public string Token { get; set; }
    }
}
