using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Helper
{
    public static class StringExtension
    {
        public static string GenerateFilterName(this string str, string replaceSpace)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            // Chuyển thành chữ thường
            string result = str.ToLower();

            // Loại bỏ dấu tiếng Việt
            result = RemoveDiacritics(result);

            // Thay thế khoảng trắng bằng dấu "_"
            result = Regex.Replace(result, @"\s+", replaceSpace);

            // Loại bỏ các ký tự đặc biệt khác nếu cần
            result = Regex.Replace(result, @"[^a-z0-9_]", "");

            return result;
        }
        private static string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
