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
        public static string ChuanHoa(this string str, string replaceSpace)
        {
            
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            // Loại bỏ dấu tiếng Việt
            str = RemoveDiacritics(str);

            // Chuyển thành chữ thường
            string result = str.ToLower();

            // Thay thế khoảng trắng bằng dấu "_"
            result = Regex.Replace(result, @"\s+", replaceSpace);

            // Loại bỏ các ký tự đặc biệt khác nếu cần
            result = Regex.Replace(result, @"[^a-z0-9_]", "");

            return result;
        }
        private static string RemoveDiacritics(string str)
        {
            string[] VietnameseSigns = {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
                };
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
    }
}
