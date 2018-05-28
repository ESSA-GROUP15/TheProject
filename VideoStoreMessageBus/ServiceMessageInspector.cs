using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace VideoStoreMessageBus
{
    public class ServiceMessageInspector : IDispatchMessageInspector, IEndpointBehavior
    {
        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(this);
        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        #endregion

        #region IDispatchMessageInspector Members

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            if (request != null)
            {
                Console.WriteLine("MESSAGE RECEIVED: (REQUEST RECEIVED)");
                Console.WriteLine("_________________________________________");
                Console.WriteLine(request.ToString());
                Console.WriteLine("_________________________________________");
            }
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (reply != null)
            {
                Console.WriteLine("MESSAGE SENT: (REPLY SENT)");
                Console.WriteLine("_________________________________________");
                Console.WriteLine(reply.ToString());
                Console.WriteLine("_________________________________________");
            }
        }

        #endregion
    }
}
