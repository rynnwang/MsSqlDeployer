using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ifunction.MicrosoftSqlDeployTool.Core
{
    /// <summary>
    /// Class for solution
    /// </summary>
    public class Solution : ICloneable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the connection setting.
        /// </summary>
        /// <value>
        /// The connection setting.
        /// </value>
        public ConnectionStringModel ConnectionSetting
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the full XML.
        /// </summary>
        /// <value>
        /// The full XML.
        /// </value>
        public XElement FullXml
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the incremental XML.
        /// </summary>
        /// <value>
        /// The incremental XML.
        /// </value>
        public IEnumerable<XElement> IncrementalXml
        {
            get;
            set;
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public XElement ToXml()
        {
            var element = Constants.Solution.CreateNodeByName();
            element.SetAttributeValue(Constants.Name, this.Name.SafeToString());

            if (this.ConnectionSetting != null)
            {
                element.Add(this.ConnectionSetting.ToXml());
            }

            if (this.FullXml != null)
            {
                element.Add(this.FullXml);
            }

            if (this.IncrementalXml != null)
            {
                foreach (var one in this.IncrementalXml)
                {
                    element.Add(one);
                }
            }

            return element;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
