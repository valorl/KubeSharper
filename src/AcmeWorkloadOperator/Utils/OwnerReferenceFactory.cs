using AcmeWorkloadOperator.Acme;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcmeWorkloadOperator.Utils
{
    public class OwnerReferenceFactory
    {
        public static List<V1OwnerReference> NewList(AcmeService owner)
        {
            return new List<V1OwnerReference>
            {
                new V1OwnerReference
                {
                    Controller = true,
                    ApiVersion = "acme.dev/v1",
                    Kind = "AcmeService",
                    Name = owner.Metadata.Name,
                    Uid = owner.Metadata.Uid,
                    BlockOwnerDeletion = true
                }
            };
        }
    }
}
