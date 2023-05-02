using AutoMapper;
using Order.Application.DataContract.Request.Order;
using Order.Application.DataContract.Response.Order;
using Order.Application.Interfaces;
using Order.Domain.Interfaces.Services;
using Order.Domain.Models;
using Order.Domain.Services;
using Order.Domain.Validations.Base;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Application
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderApplication(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }



        public async Task<Response> CreateAsync(CreateOrderRequest order)
        {
            var orderModel = _mapper.Map<OrderModel>(order);

            return await _orderService.CreateAsync(orderModel);
        }
    }
}
