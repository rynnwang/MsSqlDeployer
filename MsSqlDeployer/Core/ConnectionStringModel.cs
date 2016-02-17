using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Beyova.MicrosoftSqlDeployTool.Core
{
    /// <summary>
    /// Class for saving SQL connection information, which can be used to generate a connection string.
    /// </summary>
    public class ConnectionStringModel : ICloneable
    {
        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        public string Server
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data base.
        /// </summary>
        /// <value>
        /// The data base.
        /// </value>
        public string Database
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// To the connection string.
        /// </summary>
        /// <returns></returns>
        public string ToConnectionString()
        {
            SqlConnectionStringBuilder connectionBuilder = new SqlConnectionStringBuilder();
            connectionBuilder.DataSource = this.Server;
            connectionBuilder.IntegratedSecurity = string.IsNullOrWhiteSpace(this.UserName);
            connectionBuilder.InitialCatalog = this.Database;
            if (!connectionBuilder.IntegratedSecurity)
            {
                connectionBuilder.UserID = this.UserName;
                connectionBuilder.Password = this.Password;
            }

            return connectionBuilder.ToString();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is windows authentication.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is windows authentication; otherwise, <c>false</c>.
        /// </value>
        public bool IsWindowsAuthentication
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.UserName);
            }
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public XElement ToXml()
        {
            XElement xElement = XElement.Parse("<ConnectionString />");
            xElement.SetAttributeValue("Server", this.Server);
            xElement.SetAttributeValue("Database", this.Database);
            //xElement.SetAttributeValue("Port", this.Port.ToString());
            xElement.SetAttributeValue("UserName", this.UserName);
            xElement.SetAttributeValue("Password", this.Password);

            return xElement;
        }

        /// <summary>
        /// To the base64.
        /// </summary>
        /// <returns></returns>
        public string ToBase64()
        {
            try
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(this.ToConnectionString());
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Loads the load connection string from base64.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static ConnectionStringModel LoadConnectionStringFromBase64(string connectionString)
        {
            ConnectionStringModel model = null;

            try
            {
                byte[] bytes = Convert.FromBase64String(connectionString);
                model = ConnectionStringModel.LoadConnectionString(System.Text.Encoding.UTF8.GetString(bytes));
            }
            catch
            {
            }

            return model;
        }

        /// <summary>
        /// Loads the connection by string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static ConnectionStringModel LoadConnectionString(string connectionString)
        {
            ConnectionStringModel result = new ConnectionStringModel();

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                try
                {
                    SqlConnectionStringBuilder connectionBuilder = new SqlConnectionStringBuilder(connectionString);
                    result.Server = connectionBuilder.DataSource;
                    result.UserName = connectionBuilder.UserID;
                    result.Password = connectionBuilder.Password;
                    result.Database = connectionBuilder.InitialCatalog;
                }
                catch (Exception ex) { }

            }

            return result;
        }

        /// <summary>
        /// Loads the connection string.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static ConnectionStringModel LoadConnectionString(XElement node)
        {
            ConnectionStringModel result = null;

            if (node != null && node.Name.LocalName == "ConnectionString")
            {
                result = new ConnectionStringModel();
                result.UserName = GetAttributeValue(node, "UserName");
                result.Password = GetAttributeValue(node, "Password");
                result.Server = GetAttributeValue(node, "Server");
                //result.Port = ConvertStringToPort(GetAttributeValue(node, "Port"));
                result.Database = GetAttributeValue(node, "Database");
            }

            return result;
        }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        protected static string GetAttributeValue(XElement xElement, string attributeName)
        {
            string result = string.Empty;

            if (xElement != null && !string.IsNullOrWhiteSpace(attributeName))
            {
                var attribute = xElement.Attribute(attributeName.Trim());
                if (attribute != null)
                {
                    result = attribute.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the string to port.
        /// </summary>
        /// <param name="portString">The port string.</param>
        /// <returns></returns>
        protected static int ConvertStringToPort(string portString)
        {
            int port = 0;

            int.TryParse(portString, out port);

            if (port <= 0)
            {
                port = 1433;
            }

            return port;
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
