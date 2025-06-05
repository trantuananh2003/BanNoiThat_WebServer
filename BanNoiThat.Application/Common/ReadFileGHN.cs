using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanNoiThat.Application.Common
{
    public static class ReadFileGHN
    {
        public static async Task<(string DistrictID, string WardCode)> GetIDsAsync(string filePath, string districtName, string wardName)
        {
            // Đọc file vào bộ nhớ trước khi xử lý
            await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(1); // Lấy sheet đầu tiên
                var rows = worksheet.RowsUsed();

                foreach (var row in rows.Skip(1)) // Bỏ qua dòng đầu tiên (header)
                {
                    var currentDistrictName = row.Cell(4).GetString().Trim(); // Cột DistrictName
                    var currentWardName = row.Cell(6).GetString().Trim();    // Cột WardName

                    if (currentDistrictName.Equals(districtName, StringComparison.OrdinalIgnoreCase) &&
                        currentWardName.Equals(wardName, StringComparison.OrdinalIgnoreCase))
                    {
                        var districtID = row.Cell(3).GetString(); // Cột DistrictID
                        var wardCode = row.Cell(5).GetString();   // Cột WardCode
                        return (districtID, wardCode);
                    }
                }
            }

            return (null, null); // Không tìm thấy kết quả
        }
    }
}
