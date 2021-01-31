using MediatR;
using Order.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Queries
{
    public class GetOrderByUsernameQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public string Username { get; set; }

        public GetOrderByUsernameQuery(string username)
        {
            Username = username;
        }
    }
}
