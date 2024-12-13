using AutoMapper;
using webshop.Models;
using webshop.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.ShippingProviderName, opt => opt.MapFrom(src => src.ShippingProvider.Name))
            .ReverseMap();
        CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
    }
}
