using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace VideoStoreMessageBus.Interfaces
{
    [ServiceContract]
    public interface ISubscriptionService
    {
        [OperationContract]
        void Subscribe(String pTopic, String pCallerAddress);

        [OperationContract]
        void Unsubscribe(String pTopic, String pCallerAddress);
    }

}
