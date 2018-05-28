using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using DeliveryCo.MessageTypes;

namespace DeliveryCo.Services.Interfaces
{
    [ServiceContract]
    public interface IDeliveryService
    {
        [OperationContract(IsOneWay = true)]
        void SubmitDelivery(DeliveryInfo pDeliveryInfo);
    }
}
