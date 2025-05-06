using AutoMapper;
using Azure.Storage.Blobs.Models;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.DTOs.User;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.OutService;
using BanNoiThat.Domain.Entities;
using System.Reflection;

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

            userEntity.Birthday = modelRequest.BirthDay;
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

        public async Task<PagedList<UnitUserMangeReponse>> GetAllUser(string stringSearch, int pageCurrent,int pageSize)
        {
            var listEntity = await _uow.UserRepository.GetAllAsync();

            listEntity = listEntity.Skip((pageCurrent-1) * pageSize).Take(pageSize);

            var listResult = _mapper.Map<List<UnitUserMangeReponse>>(listEntity);

            var listPagedEntity = new PagedList<UnitUserMangeReponse>(listResult, pageCurrent, pageSize, listEntity.Count());

            return listPagedEntity;
        }

        public async Task UpdateFieldUser(string userId, string field, string valueField)
        {
            var entity = await _uow.UserRepository.GetAsync(x => x.Id == userId);
            _uow.UserRepository.AttachEntity(entity);

            var propertyInfo = entity.GetType().GetProperty(field);
            
            if(field == "Role")
            {
                entity.Role = valueField;
            }
            
            // Lưu thay đổi
            await _uow.SaveChangeAsync();
        }

        public async Task UpdateUserBlock(string userId, Boolean isBlock)
        {
            var userEntity = await _uow.UserRepository.GetAsync(x => x.Id == userId);
            _uow.UserRepository.AttachEntity(userEntity);
            userEntity.IsBlocked = isBlock;

            await _uow.SaveChangeAsync();
        }
    }
}
