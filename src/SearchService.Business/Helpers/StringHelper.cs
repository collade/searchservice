namespace SearchService.Business.Helpers
{
    using System.Globalization;
    using System.Text.RegularExpressions;

    public static class StringHelper
    {
        public static string ToUrlSlug(this string text)
        {
            return Regex.Replace(
                Regex.Replace(
                    Regex.Replace(
                        text.Trim().ToLower()
                            .Replace("ö", "o")
                            .Replace("ç", "c")
                            .Replace("ş", "s")
                            .Replace("ı", "i")
                            .Replace("ğ", "g")
                            .Replace("ü", "u"),
                        @"\s+", " "), // multiple spaces to one space
                    @"\s", "-"), // spaces to hypens
                @"[^a-z0-9\s-]", string.Empty); // removing invalid chars
        }

        public static string ToLowerTR(this string text)
        {
            return text.Trim().ToLower(new CultureInfo("tr-TR"));
        }
    }
}