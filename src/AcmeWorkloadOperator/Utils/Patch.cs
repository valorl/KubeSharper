using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcmeWorkloadOperator.Utils
{
    public static class Patch
    {
        public static JsonPatchDocument<T> New<T>() where T : class
        {
            return new JsonPatchDocument<T>()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}
