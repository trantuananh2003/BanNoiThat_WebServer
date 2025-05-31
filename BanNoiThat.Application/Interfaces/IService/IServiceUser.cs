using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.DTOs.User;

namespace BanNoiThat.Application.Interfaces.IService
{
    public interface IServiceUser
    {
        Task<PagedList<UnitUserMangeReponse>> GetAllUser(string stringSearch, int pageCurrent, int pageSize);
        Task<InfoUserResponse> GetInfoUser(string userId);
        Task UpdateInfoUser(string userId, InfoUserRequest modelRequest);
        Task UpdateUserBlock(string userId, bool isBlock);
    }
}
