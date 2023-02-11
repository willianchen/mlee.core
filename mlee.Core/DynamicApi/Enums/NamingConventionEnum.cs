using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.DynamicApi.Enums
{

    public enum NamingConventionEnum
    {
        /// <summary>
        /// camelCase
        /// </summary>
        CamelCase,
        /// <summary>
        /// PascalCase
        /// </summary>
        PascalCase,
        /// <summary>
        /// snake_case
        /// </summary>
        SnakeCase,
        /// <summary>
        /// kebab-case
        /// </summary>
        KebabCase,
        /// <summary>
        /// extension.case
        /// </summary>
        ExtensionCase,
        /// <summary>
        /// Customize with GetRestFulControllerName and GetRestFulActionName method 
        /// </summary>
        Custom
    }
}
