namespace MyFinanceFy.Libs.Ext
{
    public static class StringExtension
    {
        public static string CreateMD5(this string? input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes).ToLower(); // .NET 5 +
            }
        }
        public static string PictureProfile(this string? input)
        {
            if (input != null && input != "")
            {
                var hashEmail = input.CreateMD5();
                string pastaBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Profiles");
                if(!Directory.Exists(pastaBase)) Directory.CreateDirectory(pastaBase);
                var profile = Directory.GetFiles(pastaBase, $"*{hashEmail}*").FirstOrDefault();

                if (profile != null) return $"/Uploads/Profiles/{Path.GetFileName(profile)}";
            }
            return "/img/profile.jpg";
        }
    }
}
