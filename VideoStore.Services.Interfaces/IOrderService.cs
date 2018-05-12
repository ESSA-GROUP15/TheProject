using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using VideoStore.Services.MessageTypes;

namespace VideoStore.Services.Interfaces
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        [FaultContract(typeof(InsufficientStockFault))]
        void SubmitOrder(Order pOrder);
    }
}
