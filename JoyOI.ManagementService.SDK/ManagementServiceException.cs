﻿using System;

namespace JoyOI.ManagementService.SDK
{
    public class ManagementServiceException : Exception
    {
        public int Code { get; private set; }

        public ManagementServiceException(string json) : base("Management Service: " + json)
        {
            this.Code = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json).code;
        }
    }
}