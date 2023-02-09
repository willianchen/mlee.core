﻿using System.Reflection;
using System;
using mlee.Core.Library.Helpers;

namespace mlee.Core.Library.Attributes
{

    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class DynamicApiAttribute : Attribute
    {
        /// <summary>
        /// Equivalent to AreaName
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 分组名称列表
        /// </summary>
        public string[] GroupNames { get; set; }

        internal static bool IsExplicitlyEnabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<DynamicApiAttribute>();
            return remoteServiceAttr != null;
        }

        internal static bool IsExplicitlyDisabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<DynamicApiAttribute>();
            return remoteServiceAttr != null;
        }

        internal static bool IsMetadataExplicitlyEnabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<DynamicApiAttribute>();
            return remoteServiceAttr != null;
        }

        internal static bool IsMetadataExplicitlyDisabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<DynamicApiAttribute>();
            return remoteServiceAttr != null;
        }

        internal static bool IsMetadataExplicitlyDisabledFor(MethodInfo method)
        {
            var remoteServiceAttr = method.GetSingleAttributeOrNull<DynamicApiAttribute>();
            return remoteServiceAttr != null;
        }

        internal static bool IsMetadataExplicitlyEnabledFor(MethodInfo method)
        {
            var remoteServiceAttr = method.GetSingleAttributeOrNull<DynamicApiAttribute>();
            return remoteServiceAttr != null;
        }
    }
}
