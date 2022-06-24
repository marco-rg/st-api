namespace ST.Models
{
    public class LoginRequest
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string TokenAquasym { get; set; }

        public string RefreshTokenAquasym { get; set; }

        public string TokenHealthAdvisor { get; set; }

        public string RefreshTokenHealthAdvisor { get; set; }

        public HALogin Permissions { get; set; }

        public string timeExpiredTokenHA { get; set; }
        public string timeExpiredTokenAquasym { get; set; }

        public string timeExpiredRefreshTokenHA { get; set; }

        public string Empresa { get; set; }

        public string AppVersion { get; set; }
    }
}