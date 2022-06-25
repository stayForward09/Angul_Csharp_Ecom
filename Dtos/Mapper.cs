using AutoMapper;
using StackApi.Models;

namespace StackApi.Dtos;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<PartAdd, Part>()
        .ForMember(dest => dest.PartName, opt => opt.MapFrom(src => src.PartName))
        .ForMember(dest => dest.PartDesc, opt => opt.MapFrom(src => src.PartDesc))
        .ForMember(dest => dest.PartPrice, opt => opt.MapFrom(src => src.PartPrice))
        .ForMember(dest => dest.PcId, opt => opt.MapFrom(src => src.PartCategory))
        .ForMember(dest => dest.Pid, opt => opt.Ignore())
        .ForMember(dest => dest.ModifiedOn, opt => opt.Ignore())
        .ReverseMap();

        CreateMap<SearchViewHistoryAdd, SearchViewHistory>()
        .ForMember(dest => dest.searchterm, opt => opt.MapFrom(src => src.searchterm))
        .ForMember(dest => dest.visitedPrd, opt => opt.MapFrom(src => src.visitedPrd))
        .ForMember(dest => dest.SerachedOn, opt => opt.Ignore())
        .ForMember(dest => dest.SvId, opt => opt.Ignore())
        .ForMember(dest => dest.UsID, opt => opt.Ignore())
        .ReverseMap();

        CreateMap<User, TokenUserDetails>()
        .ForMember(dest => dest.usID, opt => opt.MapFrom(src => src.UsID))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Fname} {src.Mname} {src.Lname}".Replace("  ", " ")))
        .ForMember(dest => dest.emailID, opt => opt.MapFrom(src => src.EmailID))
        .ReverseMap();

        CreateMap<Discount, DiscountAdd>()
        .ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.CouponCode))
        .ForMember(dest => dest.CouponName, opt => opt.MapFrom(src => src.CouponName))
        .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
        .ForMember(dest => dest.DType, opt => opt.MapFrom(src => src.DType))
        .ForMember(dest => dest.CId, opt => opt.MapFrom(src => src.CId))
        .ForMember(dest => dest.PrdId, opt => opt.MapFrom(src => src.PrdId))
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
        .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
        .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
        .ReverseMap();

        CreateMap<CartItems, CartItemsAdd>()
        .ForMember(dest => dest.CIPrid, opt => opt.MapFrom(src => src.CIPrid))
        .ForMember(dest => dest.CIQty, opt => opt.MapFrom(src => src.CIQty))
        .ForMember(dest => dest.CITId, opt => opt.MapFrom(src => src.CITId))
        .ReverseMap();

        CreateMap<UserDetailsDtos, UserDetails>()
        .ForMember(des => des.Address, opt => opt.MapFrom(src => src.Address))
        .ForMember(des => des.CompanyCategory, opt => opt.MapFrom(src => src.CompanyCategory))
        .ForMember(des => des.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
        .ForMember(des => des.UsId, opt => opt.Ignore())
        .ReverseMap();
    }
}