using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using VideoStore.Services.Interfaces;

namespace VideoStore.WebClient
{
    /// <summary>
    /// Remote Services accessed using a Channel Factory in this Service Factory:
    /// Forr help, see: https://msdn.microsoft.com/en-us/library/ms734681%28v=vs.110%29.aspx
    /// </summary>
    public class ServiceFactory
    {
        private static ServiceFactory sFactory = new ServiceFactory();

        public static ServiceFactory Instance
        {
            get
            {
                return sFactory;
            }
        }

        public IUserService UserService
        {
            get
            {
                return GetTcpService<IUserService>("net.tcp://localhost:9010/UserService");
            }
        }

        public IOrderService OrderService
        {
            get
            {
                return GetTcpService<IOrderService>("net.tcp://localhost:9010/OrderService");
            }
        }

        public IRoleService RoleService
        {
            get
            {
                return  GetTcpService<IRoleService>("net.tcp://localhost:9010/RoleService");
            }
        }

        public ICatalogueService CatalogueService
        {
            get
            {
                return GetTcpService<ICatalogueService>("net.tcp://localhost:9010/CatalogueService");
            }
        }


        private T GetTcpService<T>(String pAddress)
        {
            NetTcpBinding tcpBinding = new NetTcpBinding();
            EndpointAddress address = new EndpointAddress(pAddress);
            return new ChannelFactory<T>(tcpBinding, pAddress).CreateChannel();
        }
    }
}