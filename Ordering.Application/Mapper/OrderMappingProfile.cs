using AutoMapper;
using Order.Application.Commands;
using Order.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Mapper
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Ordering.Core.Entities.Order, OrderResponse>().ReverseMap();
            CreateMap<Ordering.Core.Entities.Order, CheckoutOrderCommand>().ReverseMap();
        }
    }
}
