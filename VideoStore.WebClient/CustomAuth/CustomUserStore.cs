using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VideoStore.Services.Interfaces;
using VideoStore.Services.MessageTypes;

namespace VideoStore.WebClient.CustomAuth
{
    public class CustomUserStore : 
        IUserStore<User, int>, 
        IUserStore<User>,
        IUserRoleStore<User, int>, 
        IUserRoleStore<User>,
        IUserPasswordStore<User, int>,
        IUserPasswordStore<User>,
        IUserEmailStore<User>,
        IUserPhoneNumberStore<User>,
        IUserTwoFactorStore<User, int>
    {
        private static IUserService UserServiceClient
        {
            get
            {
                return ServiceFactory.Instance.UserService;
            }
        }

        private static IRoleService RoleServiceClient
        {
            get
            {
                return ServiceFactory.Instance.RoleService;
            }
        }

        public System.Threading.Tasks.Task CreateAsync(User user)
        {
            return RunVoidTask( () => UserServiceClient.CreateUser(user));
        }

        public System.Threading.Tasks.Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<User> FindByIdAsync(int userId)
        {
            return RunTask<User>(() => UserServiceClient.ReadUserById(userId));
        }

        public System.Threading.Tasks.Task<User> FindByNameAsync(string userName)
        {
            return RunTask<User>(() => UserServiceClient.GetUserByUserName(userName));
        }

        public System.Threading.Tasks.Task UpdateAsync(User user)
        {
            return RunVoidTask(() => UserServiceClient.UpdateUser(user));
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task AddToRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<IList<string>> GetRolesAsync(User user)
        {
            return RunTask<IList<String>>(() => RoleServiceClient.GetRolesForUser(user).ConvertAll(x => x.Name));
        }

        public System.Threading.Tasks.Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return RunTask<bool>(() => 
                RoleServiceClient.GetRolesForUser(user).ConvertAll(x => x.Name).Contains(roleName)
                );
        }

        public System.Threading.Tasks.Task RemoveFromRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

     
        //Called to fetch correct password from db
        public System.Threading.Tasks.Task<string> GetPasswordHashAsync(User user)
        {
            return RunTask<string>(() => UserServiceClient.ReadUserById(user.Id).LoginCredential.EncryptedPassword);
        }


        public System.Threading.Tasks.Task<bool> HasPasswordAsync(User user)
        {
            return RunTask<bool>(() =>  UserServiceClient.ReadUserById(user.Id) == null);
        }

      
        //Called on client to set password on user parameter before user is sent to db for persistence
        public System.Threading.Tasks.Task SetPasswordHashAsync(User user, string passwordHash)
        {
            //TODO: Add Salting
            return RunVoidTask(() => 
                {
                    user.LoginCredential.EncryptedPassword = passwordHash;

                }
            );
        }


        public Task<User> FindByIdAsync(string userId)
        {
            return RunTask<User>(() => ServiceFactory.Instance.UserService.ReadUserById(Int32.Parse(userId)));
        }

        private Task<T> RunTask<T>(Func<T> p)
        {
            Task<T> task = new Task<T>(p);
            task.Start();
            return task;
        }

        private Task RunVoidTask(Action p)
        {
            Task task = new Task(p);
            task.Start();
            return task;
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return RunTask<User>(() => ServiceFactory.Instance.UserService.GetUserByEmail(email));
        }

        public Task<string> GetEmailAsync(User user)
        {
            return RunTask<string>(()=>user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(User user, string email)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPhoneNumberAsync(User user)
        {
            return RunTask<string>(() => String.Empty);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            return RunTask<bool>(() => true );
        }

        public Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            return RunVoidTask(() => { });
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            return RunVoidTask(() => { });
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return RunTask<bool>(() => false);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            return RunVoidTask(() => { });
        }
    }
}