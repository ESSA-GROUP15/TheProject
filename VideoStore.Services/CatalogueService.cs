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
    public class CatalogueService : ICatalogueService
    {

        private ICatalogueProvider CatalogueProvider
        {
            get
            {
                return ServiceFactory.GetService<ICatalogueProvider>();
            }
        }

        public List<Media> GetMediaItems(int pOffset, int pCount)
        {
            var internalResult = CatalogueProvider.GetMediaItems(pOffset, pCount);
            var externalResult = MessageTypeConverter.Instance.Convert<
                List<VideoStore.Business.Entities.Media>,
                List<VideoStore.Services.MessageTypes.Media>>(internalResult);

            return externalResult;
        }


        public Media GetMediaById(int pId)
        {
            var external = MessageTypeConverter.Instance.Convert<
                VideoStore.Business.Entities.Media,
                VideoStore.Services.MessageTypes.Media>(
                CatalogueProvider.GetMediaById(pId));
            return external;
        }
    }
}
