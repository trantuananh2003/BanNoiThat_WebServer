using System.Text;

namespace BanNoiThat.Application.Service.RecommendSystem
{
    public static class HandleKeyword
    {
        public static void AddKeyWord(string keywords)
        {
            string filePath = "FileExtensionSupport";
            string fileName = "VocabularyKeyword.txt";
            string currentDirectory = Directory.GetCurrentDirectory();

            // Kiểm tra và tạo thư mục
            string fullDirectoryPath = Path.Combine(currentDirectory, filePath);
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            string filePathRoot = Path.Combine(fullDirectoryPath, fileName);

            // Lưu file
            try
            {
                using (var stream = new FileStream(filePathRoot, FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    var bytes_keywords = Encoding.UTF8.GetBytes(keywords + " ");
                    stream.Write(bytes_keywords, 0, bytes_keywords.Length);
                }
                Console.WriteLine("Thêm từ khóa thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi xảy ra: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        public static async Task<string[]> ReadKeywordFromVocal()
        {
            string filePath = "FileExtensionSupport";
            string fileName = "VocabularyKeyword.txt";
            string currentDirectory = Directory.GetCurrentDirectory();

            // Kiểm tra và tạo thư mục nếu không tồn tại
            string fullDirectoryPath = Path.Combine(currentDirectory, filePath);
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            string filePathRoot = Path.Combine(fullDirectoryPath, fileName);

            // Đọc file
            try
            {
                // Kiểm tra xem file có tồn tại không
                if (!File.Exists(filePathRoot))
                {
                    Console.WriteLine("File không tồn tại.");
                    return null;
                }

                // Đọc nội dung file
                using (var stream = new FileStream(filePathRoot, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var reader = new StreamReader(stream))
                {
                    string content = await reader.ReadToEndAsync();
                    Console.WriteLine("Nội dung file:");
                    Console.WriteLine(content);
                    return content.Split(" ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi xảy ra: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return null;
            }


        }
    }
}
