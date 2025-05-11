namespace JwtAuthDemo.Models
{
    public class RefreshToken
    {
        public string Token { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public DateTime ExpiryDate { get; set; }
    }
}
