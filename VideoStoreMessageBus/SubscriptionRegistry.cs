using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStoreMessageBus
{
    class SubscriptionRegistry
    {
        private static SubscriptionRegistry sRegistry;
        private const String cRegistryFile = "Registry.txt";

        public static SubscriptionRegistry Instance
        {
            get
            {
                //not thread safe!
                if (sRegistry == null)
                {
                    sRegistry = new SubscriptionRegistry();
                }
                return sRegistry;
            }
        }

        private static Dictionary<String, List<String>> sTopicSubscriptions =
            new Dictionary<string, List<String>>();

        public SubscriptionRegistry()
        {
            if (File.Exists(cRegistryFile))
            {
                using (Stream stream = new FileStream(cRegistryFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    sTopicSubscriptions = (Dictionary<String, List<String>>)formatter.Deserialize(stream);
                }
            }
        }

        public void AddSubscription(String pTopic, String pHandlerAddress)
        {
            if (!sTopicSubscriptions.ContainsKey(pTopic))
            {
                sTopicSubscriptions.Add(pTopic, new List<String>());
            }
            if (!sTopicSubscriptions[pTopic].Contains(pHandlerAddress))
            {
                sTopicSubscriptions[pTopic].Add(pHandlerAddress);
            }

            UpdatePersistedRegistry();
        }



        public void RemoveSubscription(String pTopic, String pHandlerAddress)
        {
            if (sTopicSubscriptions.ContainsKey(pTopic))
            {
                if (sTopicSubscriptions[pTopic].Contains(pHandlerAddress))
                {
                    sTopicSubscriptions[pTopic].Remove(pHandlerAddress);
                }
            }
            UpdatePersistedRegistry();
        }

        public List<String> GetTopicSubscribers(String pTopic)
        {
            return sTopicSubscriptions.ContainsKey(pTopic) ? sTopicSubscriptions[pTopic] : new List<String>();
        }

        private void UpdatePersistedRegistry()
        {
            using (Stream stream = new FileStream(cRegistryFile, FileMode.Create, FileAccess.Write, FileShare.Write))
            {

                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(stream, sTopicSubscriptions);
            }
        }
    }
}
