using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace VideoStoreMessageBus.Interfaces
{
    [ServiceContract]
    public interface IPublisherSynService
    {
        [OperationContract]
        void PublishSyn(Message pMessage);
    }

}
