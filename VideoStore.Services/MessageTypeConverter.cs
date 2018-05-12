using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoStore.Business.Components.Interfaces;

namespace VideoStore.Services
{
    /// <summary>
    /// Based on AutoMapper: https://github.com/AutoMapper/AutoMapper/wiki/Getting-started
    /// See above link if help is needed
    /// </summary>
    public class MessageTypeConverter
    {
        private static MessageTypeConverter sMessageTypeConverter = new MessageTypeConverter();

        public static MessageTypeConverter Instance
        {
            get
            {
                return sMessageTypeConverter;
            }
        }



        public MessageTypeConverter()
        {
            InitializeExternalToInternalMappings();
            InitializeInternalToExternalMappings();
        }

        private void InitializeInternalToExternalMappings()
        {
            AutoMapper.Mapper.CreateMap<VideoStore.Business.Entities.Media,
                VideoStore.Services.MessageTypes.Media>().ForMember(
                    dest => dest.StockCount, opts => opts.MapFrom( src => src.Stocks.Quantity));

            AutoMapper.Mapper.CreateMap<VideoStore.Business.Entities.Order,
                VideoStore.Services.MessageTypes.Order>();

            AutoMapper.Mapper.CreateMap<VideoStore.Business.Entities.OrderItem,
                VideoStore.Services.MessageTypes.OrderItem>();

            AutoMapper.Mapper.CreateMap<VideoStore.Business.Entities.User,
                 VideoStore.Services.MessageTypes.User>();

            AutoMapper.Mapper.CreateMap<VideoStore.Business.Entities.LoginCredential,
                VideoStore.Services.MessageTypes.LoginCredential>();
        }

        public void InitializeExternalToInternalMappings()
        {
            AutoMapper.Mapper.CreateMap<VideoStore.Services.MessageTypes.Media,
                VideoStore.Business.Entities.Media>();

            AutoMapper.Mapper.CreateMap<VideoStore.Services.MessageTypes.Order,
               VideoStore.Business.Entities.Order>();

            AutoMapper.Mapper.CreateMap<VideoStore.Services.MessageTypes.OrderItem,
                VideoStore.Business.Entities.OrderItem>();

            AutoMapper.Mapper.CreateMap<VideoStore.Services.MessageTypes.User,
                 VideoStore.Business.Entities.User>();

            AutoMapper.Mapper.CreateMap<VideoStore.Services.MessageTypes.LoginCredential,
                VideoStore.Business.Entities.LoginCredential>();
        }

        public Destination Convert<Source, Destination>(Source s) where Destination : class
        {
            if(typeof(Source) == typeof(VideoStore.Services.MessageTypes.User))
            {
                return ConvertUserToInternalType(s as MessageTypes.User) as Destination;
            }
            else if(typeof(Source) == typeof(VideoStore.Business.Entities.User))
            {
                return ConvertToExternalType(s as Business.Entities.User) as Destination;
            }
            var result = AutoMapper.Mapper.Map<Source, Destination>(s);
            if(typeof(Source) == typeof(MessageTypes.Order))
            {
                (result as Business.Entities.Order).Customer = ConvertUserToInternalType(
                    (s as MessageTypes.Order).Customer
                );
            }
            return result;
        }

        private MessageTypes.User ConvertToExternalType(Business.Entities.User user)
        {
            MessageTypes.User lExternal = new MessageTypes.User()
            {
                Address = user.Address,
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Revision = user.Revision,
            };

            if(user.LoginCredential != null)
            {
                lExternal.LoginCredential = new MessageTypes.LoginCredential()
                {
                    Id = user.LoginCredential.Id,
                    UserName = user.LoginCredential.UserName,
                    EncryptedPassword = user.LoginCredential.EncryptedPassword
                };
            }

            return lExternal;
        }

        private Business.Entities.User ConvertUserToInternalType(MessageTypes.User user)
        {

            Business.Entities.User lInternal = UserProvider.ReadUserById(user.Id);
            if(lInternal == null)
            {
                lInternal = new Business.Entities.User();
            }
            
            
            lInternal.Address = user.Address;
            lInternal.Email = user.Email;
            lInternal.Id = user.Id;
            lInternal.Name = user.Name;
            lInternal.Revision = user.Revision;
            

            if(user.LoginCredential != null)
            {
                lInternal.LoginCredential = new Business.Entities.LoginCredential()
                {
                    Id = user.LoginCredential.Id,
                    UserName = user.LoginCredential.UserName,
                    EncryptedPassword = user.LoginCredential.EncryptedPassword
                };
            }
            return lInternal;
        }


        private IUserProvider UserProvider
        {
            get
            {
                return ServiceFactory.GetService<IUserProvider>();
            }
        }
    }
}
