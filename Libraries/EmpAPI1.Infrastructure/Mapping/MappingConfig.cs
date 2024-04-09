using AutoMapper;
using Menu.Domain.Models;
using Menu.Infrastructure.DTO;

namespace Menu.Infrastructure.Mapping
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            //mapping customer 
            CreateMap<CustomerEF, Customer>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
            .ReverseMap();

            // mapping dish 
            CreateMap<DishEF, Dish>()
                .ForMember(dest => dest.QuantityAvailable, opt=> opt.MapFrom(src => src.QuantityAvailable))                
            .ReverseMap();

            // mapping order
            CreateMap<OrderEF, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ReverseMap();


            //mapping orderItem
            CreateMap<OrderItemEF, OrderItem>()
                .ForMember(dest => dest.DishId, opt => opt.MapFrom(src => src.Dish.Id))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.DishName, opt => opt.MapFrom(src=>src.Dish.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src=> src.Dish.Price * src.Quantity))
                .ReverseMap();
        }
    }
}
