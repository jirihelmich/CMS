using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace CMS.CMS.Acl
{
    public class CMS_Resource : IEquatable<CMS_Resource>
    {
        /// <summary>
        /// resource name
        /// </summary>
        protected String _name;

        /// <summary>
        /// parental resource
        /// </summary>
        protected CMS_Resource _parent;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Resource name</param>
        public CMS_Resource(String name)
        {
            this._name = name.ToLower();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">resource name</param>
        /// <param name="parent">parental resource</param>
        public CMS_Resource(String name, CMS_Resource parent)
        {
            this._name = name.ToLower();
            this._parent = parent;
        }

        /// <summary>
        /// Get name of the resource
        /// </summary>
        /// <returns>name</returns>
        public String getName()
        {
            return this._name;
        }

        /// <summary>
        /// IEquatable method
        /// </summary>
        /// <param name="r">Equals to</param>
        /// <returns>equality</returns>
        public bool Equals(CMS_Resource r)
        {
            return (r.getName() == _name);
        }

        /// <summary>
        /// Return parental resource
        /// </summary>
        /// <returns>parental resource</returns>
        public CMS_Resource getParent()
        {
            return _parent;
        }

    }
}
