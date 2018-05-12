using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Services.Interfaces;
using VideoStore.Business.Components.Interfaces;
using Microsoft.Practices.ServiceLocation;
using VideoStore.Services.MessageTypes;

namespace VideoStore.Services
{
    public class RoleService : IRoleService
    {

        private IRoleProvider RoleProvider
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IRoleProvider>();
            }
        }

        public List<Role> GetRolesForUser(User pUser)
        {
            var internalType = MessageTypeConverter.Instance.Convert<
                VideoStore.Services.MessageTypes.User,
                VideoStore.Business.Entities.User>(
                pUser);

            var internalResult = RoleProvider.GetRolesForUser(internalType);

            var externalResult = MessageTypeConverter.Instance.Convert<
                List<VideoStore.Business.Entities.Role>,
                List<VideoStore.Services.MessageTypes.Role>>(internalResult);

            return externalResult;
        }


        public List<Role> GetRolesForUserName(string pUserName)
        {
            var internalResult = RoleProvider.GetRolesForUserName(pUserName);
            var externalResult = MessageTypeConverter.Instance.Convert<
                    List<VideoStore.Business.Entities.Role>,
                    List<VideoStore.Services.MessageTypes.Role>>(internalResult);
            return externalResult;
        }
    }
}
