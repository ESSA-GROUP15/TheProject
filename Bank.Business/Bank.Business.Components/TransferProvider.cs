using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bank.Business.Components.Interfaces;
using Bank.Business.Entities;
using System.Transactions;
using Bank.Services.Interfaces;
using Bank.Business.Components.PublisherService1;

namespace Bank.Business.Components
{
    public class TransferProvider : ITransferProvider
    {


        public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber, Guid pExternalOrderId)
        {
            using (TransactionScope lScope = new TransactionScope())
            using (BankEntityModelContainer lContainer = new BankEntityModelContainer())
            {

                try
                {
                    Account lFromAcct = GetAccountFromNumber(pFromAcctNumber);
                    Account lToAcct = GetAccountFromNumber(pToAcctNumber);
                    lFromAcct.Withdraw(pAmount);
                    lToAcct.Deposit(pAmount);
                    lContainer.Attach(lFromAcct);
                    lContainer.Attach(lToAcct);
                    lContainer.ObjectStateManager.ChangeObjectState(lFromAcct, System.Data.EntityState.Modified);
                    lContainer.ObjectStateManager.ChangeObjectState(lToAcct, System.Data.EntityState.Modified);
                    lContainer.SaveChanges();
                    

                    
                    Common.Model.TransferResultMessage transferResultMessage = new Common.Model.TransferResultMessage()
                    {
                        Topic="VideoStore",
                        Success = true,
                        OrderNumber=pExternalOrderId,
                        Message="Transfer success"
                    };
                    PublisherServiceClient lClient = new PublisherServiceClient();
                    lClient.Publish(transferResultMessage);
                 
                }
                catch (Exception lException)
                {                 
                    Common.Model.TransferResultMessage transferResultMessage = new Common.Model.TransferResultMessage()
                    {
                        Topic = "VideoStore",
                        Success = false,
                        OrderNumber = pExternalOrderId,
                        Message = "Error occured while transferring money:  " + lException.Message
                    };
                    //Console.WriteLine("Error occured while transferring money:  " + lException.Message);
                    //throw;
                    PublisherServiceClient lClient = new PublisherServiceClient();
                    lClient.Publish(transferResultMessage);
                }
                lScope.Complete();
            }
        }

        private Account GetAccountFromNumber(int pToAcctNumber)
        {
            using (BankEntityModelContainer lContainer = new BankEntityModelContainer())
            {
                return lContainer.Accounts.Where((pAcct) => (pAcct.AccountNumber == pToAcctNumber)).FirstOrDefault();
            }
        }
    }
}
