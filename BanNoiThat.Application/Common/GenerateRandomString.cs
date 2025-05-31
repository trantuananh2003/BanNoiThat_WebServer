using System.Text;

namespace BanNoiThat.Application.Common
{
    public static class GenerateRandomString
    {
        public static string Generate(int length)
        {
            string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            StringBuilder result = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(allowedChars.Length);
                result.Append(allowedChars[index]);
            }

            return result.ToString();
        }
    }
}
