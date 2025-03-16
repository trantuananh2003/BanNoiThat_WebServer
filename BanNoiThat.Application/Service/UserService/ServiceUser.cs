using AutoMapper;
using BanNoiThat.Application.DTOs.User;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.OutService;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Service.UserService
{
    public class ServiceUser  : IServiceUser
    {
        private IBlobService _blobService;
        private IUnitOfWork _uow;
        private IMapper _mapper;

        public ServiceUser(IUnitOfWork uow,IBlobService blobService, IMapper mapper)
        {
            _blobService = blobService;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task UpdateInfoUser(string userId, InfoUserRequest modelRequest)
        {
            var userEntity = await _uow.UserRepository.GetAsync(x => x.Id == userId);
            _uow.UserRepository.AttachEntity(userEntity);

            userEntity.BirthDay = modelRequest.BirthDay;
            userEntity.IsMale = modelRequest.IsMale;
            userEntity.FullName = modelRequest.FullName;

            await _uow.SaveChangeAsync();
        }

        public async Task<InfoUserResponse> GetInfoUser(string userId)
        {
            var userEntity = await _uow.UserRepository.GetAsync(x => x.Id == userId);
            var infoUserResponse = _mapper.Map<InfoUserResponse>(userEntity);

            return infoUserResponse;
        }
    }
}
