using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Entities;
using System.Transactions;
using System.ComponentModel.Composition;

namespace VideoStore.Business.Components
{
    public class UserProvider : IUserProvider
    {
        public void CreateUser(VideoStore.Business.Entities.User pUser)
        {
            using(TransactionScope lScope = new TransactionScope())
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                lContainer.Users.AddObject(pUser);
                lContainer.SaveChanges();
                lScope.Complete();
            }
        }


        public User ReadUserById(int pUserId)
        {
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                User lCustomer = lContainer.Users.Include("LoginCredential").Where((pUser) => pUser.Id == pUserId).FirstOrDefault();
                return lCustomer;
            }
        }


        public void UpdateUser(User pUser)
        {
            using(TransactionScope lScope = new TransactionScope())
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                lContainer.Users.Attach(pUser);
                lContainer.ObjectStateManager.ChangeObjectState(pUser, System.Data.EntityState.Modified);
                lContainer.SaveChanges();
                lScope.Complete();
            }
        }


        public void DeleteUser(User pUser)
        {
            using(TransactionScope lScope = new TransactionScope())
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                lContainer.Users.DeleteObject(pUser);
                lContainer.SaveChanges();
            }
        }


        public bool ValidateUserCredentials(string username, string password)
        {
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                
                var lCredentials = from lCredential in lContainer.LoginCredentials
                            where lCredential.UserName == username && lCredential.EncryptedPassword == password
                            select lCredential;
                return lCredentials.Count() > 0;
            }
        }


        public User GetUserByUserNamePassword(string username, string password)
        {
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                
                var lCredentials = from lCredential in lContainer.LoginCredentials
                            where lCredential.UserName == username && lCredential.EncryptedPassword == password
                            select lCredential;

                if (lCredentials.Count() > 0)
                {
                    LoginCredential lCredential = lCredentials.First();
                    var lUsers = from lCustomer in lContainer.Users
                                 where lCustomer.LoginCredential.Id == lCredential.Id
                                 select lCustomer;
                    if (lUsers.Count() > 0)
                    {
                        return lUsers.First();
                    }
                }
                return null;
            }
        }


        public User GetUserByUserName(string username)
        {
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                var lCredentials = from lCredential in lContainer.LoginCredentials
                                   where lCredential.UserName == username
                                   select lCredential;

                if (lCredentials.Count() > 0)
                {
                    LoginCredential lCredential = lCredentials.First();
                    var lUsers = from lCustomer in lContainer.Users
                                 where lCustomer.LoginCredential.Id == lCredential.Id
                                 select lCustomer;
                    if (lUsers.Count() > 0)
                    {

                        var user = lUsers.First();
                        return user;
                    }
                }
                return null;
            }
        }


        public User GetUserByEmail(string email)
        {
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                var users = from user in lContainer.Users.Include("LoginCredential")
                            where user.Email == email
                            select user;

                //only allow 1 user per email in system
                if (users.Count() > 0)
                {
                    User user = users.First();
                    return user;
                }
                return null;
            }
        }
    }
}
