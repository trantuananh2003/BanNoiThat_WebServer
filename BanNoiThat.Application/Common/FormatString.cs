using System.Text.RegularExpressions;

namespace BanNoiThat.Application.Common
{
    public static class FormatString
    {
        public static string GenerateSlug(this string str)
        {
            str = str.Trim();
            str = str.ToLower();
            str = str.RemoveSign();
            str = Regex.Replace(str, " ", "-");
            return str;
        }

        public static string RemoveSign(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            str = Regex.Replace(str, "(à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ)", "a");
            str = Regex.Replace(str, "(è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ)", "e");
            str = Regex.Replace(str, "(ì|í|ị|ỉ|ĩ)", "i");
            str = Regex.Replace(str, "(ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ)", "o");
            str = Regex.Replace(str, "(ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ)", "u");
            str = Regex.Replace(str, "(ỳ|ý|ỵ|ỷ|ỹ)", "y");
            str = Regex.Replace(str, "(đ)", "d");
            str = Regex.Replace(str, "(À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ)", "A");
            str = Regex.Replace(str, "(È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ)", "E");
            str = Regex.Replace(str, "(Ì|Í|Ị|Ỉ|Ĩ)", "I");
            str = Regex.Replace(str, "(Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ)", "O");
            str = Regex.Replace(str, "(Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ)", "U");
            str = Regex.Replace(str, "(Ỳ|Ý|Ỵ|Ỷ|Ỹ)", "Y");
            str = Regex.Replace(str, "(Đ)", "D");

            return str;
        }
    }
}
