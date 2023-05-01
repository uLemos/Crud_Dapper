﻿using AutoMapper;
using Order.Application.DataContract.Request.Client;
using Order.Application.DataContract.Response.Client;
using Order.Domain.Models;

namespace Order.Application.Mapper
{
    public class CoreM : Profile
    {
        public CoreM()
        {
            ClientMap();
        }

        private void ClientMap()
        {
            CreateMap<CreateClientRequest, ClientModel>();
            
            CreateMap<ClientModel, ClientResponse>();
        }
    }
}