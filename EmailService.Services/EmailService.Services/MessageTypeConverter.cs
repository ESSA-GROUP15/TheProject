using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Services
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

            AutoMapper.Mapper.CreateMap<global::EmailService.Business.Entities.EmailMessage,
                global::EmailService.MessageTypes.EmailMessage>();
        }

        public void InitializeExternalToInternalMappings()
        {
            AutoMapper.Mapper.CreateMap<global::EmailService.MessageTypes.EmailMessage,
                global::EmailService.Business.Entities.EmailMessage>();
        }

        public Destination Convert<Source, Destination>(Source s)
        {
            var result = AutoMapper.Mapper.Map<Source, Destination>(s);
            return result;
        }
    }
}
