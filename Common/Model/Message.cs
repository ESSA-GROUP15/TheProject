using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Model
{

    [DataContract]
    [KnownType(typeof(SendEmailMessage))]//VideoStore send email
    [KnownType(typeof(TransferMessage))]//VideoStore request tranfer
    [KnownType(typeof(TransferResultMessage))]//Bank send transfer result to VideoStore
    [KnownType(typeof(SubmitDeliveryMessage))]//VideoStore submit delivery order to DeliveryCo
    [KnownType(typeof(DeliverProcessedMessage))]//DeliveryCo call VideoStore to email "deliver submitted"
    [KnownType(typeof(DeliverCompleteMessage))]//DeliveryCo call VideoStore to email "deliver complete"

    public abstract class Message : IVisitable
    {
        [DataMember]
        public String Topic { get; set; }


        public void Accept(IVisitor pVisitor)
        {
            pVisitor.Visit(this);
        }
    }

}
