﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Model
{
    [DataContract]
    public class SendEmailMessage:Message
    {
        [DataMember]
        public string Message;

        [DataMember]
        public string ToAddresses;

        [DataMember]
        public DateTime Date;
    }
}
