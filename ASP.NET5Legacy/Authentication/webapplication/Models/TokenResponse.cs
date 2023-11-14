namespace RestWithASPNETUdemy.Models
{
    public class TokenResponse
    {
        public TokenResponse(bool autenticated, string created, string expiration, string accessToken, string refreshToken)
        {
            Autenticated = autenticated;
            Created = created;
            Expiration = expiration;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public bool Autenticated { get; set; }
        public string Created { get; set; }
        public string Expiration { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
