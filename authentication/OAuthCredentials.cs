using System;
namespace googleassistantcsharpdemo.authentication
{
    [Serializable]
    public class OAuthCredentials
    {
        public string accessToken { get; set; }

        public int expiresIn { get; set; }

        public string tokenType { get; set; }

        public string refreshToken { get; set; }

        public string idToken { get; set; }

        public long expirationTime { get; set; }
    }
}
