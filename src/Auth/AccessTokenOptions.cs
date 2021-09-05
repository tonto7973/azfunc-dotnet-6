namespace test_func_6.Auth
{
    public class AccessTokenOptions
    {
        public string Authority { get; set; }
        public string Audience { get; set; }

        public string NameClaimType { get; set; }
        public string RoleClaimType { get; set; }
    }
}
