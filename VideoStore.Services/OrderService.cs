using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Services.Interfaces;
using VideoStore.Business.Components.Interfaces;
using Microsoft.Practices.ServiceLocation;
using VideoStore.Services.MessageTypes;

using System.ServiceModel;

namespace VideoStore.Services
{
    public class OrderService : IOrderService
    {

        private IOrderProvider OrderProvider
        {
            get
            {
                return ServiceFactory.GetService<IOrderProvider>();
            }
        }

        public void SubmitOrder(Order pOrder)
        {
            try
            {
                OrderProvider.SubmitOrder(
                    MessageTypeConverter.Instance.Convert<
                    VideoStore.Services.MessageTypes.Order,
                    VideoStore.Business.Entities.Order>(pOrder)
                );
            }
            catch(VideoStore.Business.Entities.InsufficientStockException ise)
            {
                throw new FaultException<InsufficientStockFault>(
                    new InsufficientStockFault() { ItemName = ise.ItemName });
            }
        }
    }
}
