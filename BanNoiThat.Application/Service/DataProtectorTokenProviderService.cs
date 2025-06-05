using BanNoiThat.Application.Interfaces.IService;
using Microsoft.AspNetCore.DataProtection;
using System;

public class DataProtectorTokenProviderService
{
    private readonly IServiceUser _serviceUser;
    private readonly IDataProtectionProvider _dataProtectionProvider;
    private readonly TimeSpan _tokenLifetime = TimeSpan.FromHours(1); // Thời gian sống của token

    public DataProtectorTokenProviderService(IServiceUser serviceUser, IDataProtectionProvider dataProtectionProvider)
    {
        _serviceUser = serviceUser;
        _dataProtectionProvider = dataProtectionProvider;
    }

    // Hàm generate token
    public string GenerateToken(string userId, string purpose)
    {
        var protector = _dataProtectionProvider.CreateProtector(purpose);

        var tokenData = new TokenData
        {
            UserId = userId,
            Purpose = purpose,
            CreatedAt = DateTime.UtcNow
        };

        var serializedToken = System.Text.Json.JsonSerializer.Serialize(tokenData);
        return protector.Protect(serializedToken);
    }

    // Hàm validate token
    public bool ValidateToken(string token, string purpose, out string userId)
    {
        userId = null;

        try
        {
            var protector = _dataProtectionProvider.CreateProtector(purpose);
            var unprotectedToken = protector.Unprotect(token);

            var tokenData = System.Text.Json.JsonSerializer.Deserialize<TokenData>(unprotectedToken);
            if (tokenData == null || tokenData.Purpose != purpose)
                return false;

            // Kiểm tra thời gian sống của token
            if (DateTime.UtcNow - tokenData.CreatedAt > _tokenLifetime)
                return false;

            userId = tokenData.UserId;
            return true;
        }
        catch
        {
            return false;
        }
    }

    private class TokenData
    {
        public string UserId { get; set; }
        public string Purpose { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
