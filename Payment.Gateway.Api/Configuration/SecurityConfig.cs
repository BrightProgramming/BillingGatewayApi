namespace Payment.Gateway.Api.Configuration
{
    /// <summary>
    /// Config for validation incoming api call
    /// </summary>
    public class SecurityConfig
    {
        /// <summary>
        /// Valid user name to use
        /// </summary>
        public string ValidUsername { get; set; }

        /// <summary>
        /// Valid password to use
        /// </summary>
        public string ValidPassword { get; set; }
    }
}
