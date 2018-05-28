using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Common.Model
{
    [ServiceContract]
    public interface ISubscriberService
    {
        [OperationContract(IsOneWay = true)]
        void PublishToSubscriber(Message pMessage);
    }
}
