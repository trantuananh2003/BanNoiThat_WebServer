﻿using BanNoiThat.Application.Interfaces.IRepository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class CouponsRepository : Repository<Coupon>, ICouponRepository
    {
        public CouponsRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
