using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoStore.Services.MessageTypes;

namespace VideoStore.WebClient.CustomAuth
{
    public class CustomRoleStore : IRoleStore<Role, int>
    {
        public System.Threading.Tasks.Task CreateAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task DeleteAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<Role> FindByIdAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<Role> FindByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}