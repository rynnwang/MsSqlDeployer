using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ifunction
{
    /// <summary>
    /// Class Extension.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// check string is null?
        /// </summary>
        /// <param name="inputValue">input value</param>
        /// <param name="defaultValue">default value</param>
        /// <returns>System.String.</returns>
        public static string SafeToString(this string inputValue, string defaultValue = "")
        {
            return string.IsNullOrWhiteSpace(inputValue) ? defaultValue : inputValue;
        }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>System.String.</returns>
        public static string GetAttributeValue(this XElement node, string attributeName)
        {
            XAttribute attribute = (node == null && !string.IsNullOrWhiteSpace(attributeName)) ? null : node.Attribute(attributeName);

            return attribute == null ? string.Empty : attribute.Value;
        }

        /// <summary>
        /// Automatics the short unique identifier.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        public static string ToShortId(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var hash = 0;

            input = input.ToLowerInvariant();

            for (var i = 0; i < input.Length; i++)
            {
                hash = hash * 31 + Convert.ToInt32(input[i]);
            }

            return hash.ToString("x").ToLowerInvariant();
        }

        /// <summary>
        /// Automatics the short unique identifier.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        public static string ToShortId(this Guid? input)
        {
            if (input == null)
            {
                return string.Empty;
            }

            return ToShortId(input.Value.ToString());
        }
    }
}
