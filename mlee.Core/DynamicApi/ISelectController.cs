using AutoMapper.Internal;
using mlee.Core.DynamicApi.Attributes;
using mlee.Core.Library.Attributes;
using mlee.Core.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.DynamicApi
{

    public interface ISelectController
    {
        bool IsController(Type type);
    }

    internal class DefaultSelectController : ISelectController
    {
        public bool IsController(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (!typeof(IDynamicApi).IsAssignableFrom(type) ||
                !typeInfo.IsPublic || typeInfo.IsAbstract || typeInfo.IsGenericType)
            {
                return false;
            }


            var attr = Reflection.GetSingleAttributeOrDefaultByFullSearch<DynamicApiAttribute>(typeInfo);

            if (attr == null)
            {
                return false;
            }

            if (Reflection.GetSingleAttributeOrDefaultByFullSearch<NonDynamicApiAttribute>(typeInfo) != null)
            {
                return false;
            }

            return true;
        }
    }
}
