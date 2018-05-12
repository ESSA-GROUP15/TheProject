using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCo.Services
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

            AutoMapper.Mapper.CreateMap<global::DeliveryCo.Business.Entities.DeliveryInfo,
                global::DeliveryCo.MessageTypes.DeliveryInfo>();
        }

        public void InitializeExternalToInternalMappings()
        {
            AutoMapper.Mapper.CreateMap<global::DeliveryCo.MessageTypes.DeliveryInfo,
                global::DeliveryCo.Business.Entities.DeliveryInfo>();
        }

        public Destination Convert<Source, Destination>(Source s)
        {
            var result = AutoMapper.Mapper.Map<Source, Destination>(s);
            return result;
        }
    }
}
