﻿namespace BanNoiThat.Application.DTOs.User
{
    public class UnitUserMangeReponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string IsMale { get; set; }
        public DateTime Birthday { get; set; }
        public string Role_Id { get; set; }
        public string RoleName { get; set; }
        public bool IsBlocked { get; set; }
    }
}
