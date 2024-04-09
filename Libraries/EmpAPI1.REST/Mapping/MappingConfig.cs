using Menu.Domain.Models;
using Menu.REST.DTO;

#if ProducesConsumes
#endif

namespace Menu.REST.Mapping
{
    public class MappingConfig : Infrastructure.Mapping.MappingConfig
    {
        public MappingConfig() : base()
        {
            CreateMap<CustomerDTO, Customer>().ReverseMap();


            CreateMap<DishDTO, Dish>()
                .ForMember(dest => dest.QuantityAvailable, opt => opt.MapFrom(src => src.Quantity))
                .ReverseMap();

            CreateMap<OrderDTO, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.DishId, opt => opt.MapFrom(src => src.DishId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ReverseMap();
        }
    }
}