using BanNoiThat.Application.DTOs.User;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServiceUser
    {
        Task<InfoUserResponse> GetInfoUser(string userId);
        Task UpdateInfoUser(string userId, InfoUserRequest modelRequest);
    }
}
