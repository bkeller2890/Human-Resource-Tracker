using System.Text;

namespace HRTracker.Utils;

public static class StringExtensions
{
    public static string ToMd5Hash(this string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        using var md5 = System.Security.Cryptography.MD5.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = md5.ComputeHash(bytes);
        var sb = new StringBuilder();
        foreach (var b in hash) sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}
