using AutoMapper;
using Order.Application.DataContract.Request.Client;
using Order.Application.DataContract.Request.Order;
using Order.Application.DataContract.Request.Product;
using Order.Application.DataContract.Request.User;
using Order.Application.DataContract.Response.Client;
using Order.Application.DataContract.Response.Order;
using Order.Application.DataContract.Response.Product;
using Order.Application.DataContract.Response.User;
using Order.Domain.Models;

namespace Order.Application.Mapper
{
    public class CoreM : Profile
    {
        public CoreM()
        {
            ClientMap();
            OrderMap();
            ProductMap();
            UserMap();
        }

        private void ClientMap()
        {
            CreateMap<CreateClientRequest, ClientModel>();
            
            CreateMap<ClientModel, ClientResponse>();
        }

        private void OrderMap()
        {
            CreateMap<CreateOrderRequest, OrderModel>();
            
            CreateMap<OrderModel, OrderResponse>();
        }

        private void ProductMap()
        {
            CreateMap<CreateProductRequest, ProductModel>();

            CreateMap<ProductModel, ProductResponse>();
        }

        private void UserMap()
        {
            CreateMap<CreateUserRequest, UserModel>();

            CreateMap<UserModel, UserResponse>();
        }

    }
}
