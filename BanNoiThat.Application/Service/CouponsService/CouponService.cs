using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.CouponDtos;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Service.CouponsService
{
    public class CouponService : IServiceCoupon
    {
        private readonly IUnitOfWork _uow;

        public CouponService(IUnitOfWork uow) {
            _uow = uow;
        }

        public async Task<ResultCheckCoupon> CheckCouponInOrder(string couponCode, Cart cart)
        {
            var result = new ResultCheckCoupon()
            {
                IsCanApply = false,
                AmountDiscount = 0,
                NameCoupon = "",
            };

            var entityCoupon = await _uow.CouponsRepository.GetAsync(x => x.CouponCode == couponCode);
            result.NameCoupon = entityCoupon.Description;
            var entityUser = await _uow.UserRepository.GetAsync(x => x.Id == cart.User_Id);
            var listEntityCouponUsage = await _uow.CouponUsageRepository.GetAllAsync(x => x.User_Id == entityUser.Id && x.Coupon_Id == entityCoupon.Id);
            
            if(entityCoupon.StartDate < DateTime.Now && entityCoupon.EndDate > DateTime.Now )
            {
                return result;
            }

            if (entityCoupon.Quantity <= 0) {
                return result;
            }

            if (listEntityCouponUsage.Count() >= entityCoupon.UsageLimit)
            {
                return result;
            }

            var totalPriceCart = CalculateTotalPriceInCart(cart);
            result.CouponCode = couponCode;
            result.Coupon_Id = entityCoupon.Id;
            result.NameCoupon = entityCoupon.Description;
            result.IsCanApply = true;
            result.AmountDiscount = CalculateCoupon(entityCoupon, totalPriceCart);

            return result;
        }

        public async Task CheckApplyTogether(List<string> couponCodes, string currentCoupon)
        {
            Dictionary<string, int> typeCoupons = new Dictionary<string, int>();
            foreach(var  couponCode in couponCodes)
            {
                var entity = await _uow.CouponsRepository.GetAsync(x => x.CouponCode == couponCode);
                int timeAdd = 0;
                if (typeCoupons.TryGetValue(entity.TypeCoupon, out timeAdd))
                {
                    if(timeAdd > 0 && (entity.TypeCoupon == StaticDefine.CouponType_OnlyCouponProduct || entity.TypeCoupon == StaticDefine.CouponType_OnlyCouponShipping))
                    {
                        throw new Exception("Mã này không được áp dụng đồng thời " + currentCoupon);
                    }
                    typeCoupons[entity.TypeCoupon] += ++timeAdd;
                }
                else
                {
                    typeCoupons.Add(entity.TypeCoupon, 1);
                }
            }
        }


        private double CalculateCoupon(Coupon coupon, double totalPriceCart)
        {
            if(coupon.DiscountType == StaticDefine.DiscountType_Percent)
            {
                var result = (coupon.DiscountValue / 100) * totalPriceCart;
                return result > coupon.MaxDiscount ? coupon.MaxDiscount : result ;
            }
            else if(coupon.DiscountType == StaticDefine.DiscountType_FixedAmount)
            {
                return coupon.DiscountValue;
            }
            else
            {
                return 0;
            }
        }

        private double CalculateTotalPriceInCart(Cart cart)
        {
            double totalPrice = 0;
            foreach(var cartItem in cart.CartItems)
            {
                totalPrice += cartItem.Quantity * cartItem.ProductItem.SalePrice;
            }
            return totalPrice;
        }

    }
}
