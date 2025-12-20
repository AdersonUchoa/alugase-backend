using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string Value(this Enum e)
        {
            var enumMember =
                e.GetType()
                 .GetMember(e.ToString())[0]
                 .GetCustomAttribute<EnumMemberAttribute>()?
                 .Value;

            if (string.IsNullOrWhiteSpace(enumMember))
                return e.ToString();

            if (enumMember.EndsWith(".html"))
                return enumMember;

            var cleaned = enumMember.Contains("|")
                ? enumMember.Split('|')[0]
                : enumMember;

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cleaned.ToLower());
        }
    }
}
