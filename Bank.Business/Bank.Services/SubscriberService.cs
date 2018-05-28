using Bank.Business.Components.Interfaces;
using Common;
using Common.Model;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank.Services
{
    public class SubscriberService : ISubscriberService
    {

        private ITransferProvider TransferProvider
        {
            get { return ServiceLocator.Current.GetInstance<ITransferProvider>(); }
        }

        public void PublishToSubscriber(Common.Model.Message pMessage)
        {
            if (pMessage is Common.Model.TransferMessage)
            {
                TransferMessage lMessage = pMessage as TransferMessage;
                TransferProvider.Transfer(lMessage.pAmount, lMessage.pFromAcctNumber, lMessage.pToAcctNumber);
            }
        }
    }
}
