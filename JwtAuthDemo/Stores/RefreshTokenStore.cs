using JwtAuthDemo.Models;

namespace JwtAuthDemo.Stores
{
    public static class RefreshTokenStore
    {
        public static List<RefreshToken> Tokens = new();
    }
}
