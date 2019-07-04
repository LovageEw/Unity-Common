using System.Linq;
using System.Text.RegularExpressions;

public static class StringExtensions {

    public static string RemoveString(this string str, params string[] target)
    {
        return target.Aggregate(str, (current, item) => current.Replace(item, ""));
    }

    public static string ToCamelCase(this string str) {
        return Regex.Replace(
            str.Replace("_", " "),
            @"\b[a-z]",
            match => match.Value.ToUpper()).Replace(" ", "");
    }
}
