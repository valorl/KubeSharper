using k8s;
using KubeSharper.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KubeSharper.EventSources
{
    public partial class Listers
    {
        public Lister<T> GetNamespacedForCustom<T>(IKubernetes operations)
        {
            var crd = CustomResourceDefinition.For<T>();
            async Task<IList<T>> ListerFunc(string @namespace)
            {
                var list = await operations.ListNamespacedCustomObjectAsync(
                    crd.Group, crd.Version, @namespace, crd.Plural);
                var jObj = (JObject)list;
                var jItems = (JArray)jObj["items"];
                return jItems.ToObject<List<T>>();
            }

            return new Lister<T>(ListerFunc);
        }
    }
}
