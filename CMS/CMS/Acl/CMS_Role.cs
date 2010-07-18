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
    public class CMS_Role : IEquatable<CMS_Role>
    {
        /// <summary>
        /// Role name
        /// </summary>
        protected String _name;

        /// <summary>
        /// parental role
        /// </summary>
        protected CMS_Role _parent;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Role name</param>
        public CMS_Role(String name)
        {
            this._name = name.ToLower();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Role name</param>
        /// <param name="parent">Parentar role</param>
        public CMS_Role(String name, CMS_Role parent)
        {
            this._name = name.ToLower();
            this._parent = parent;
        }

        /// <summary>
        /// Return name of the role
        /// </summary>
        /// <returns>name</returns>
        public String getName()
        {
            return this._name;
        }

        /// <summary>
        /// IEquatable method
        /// </summary>
        /// <param name="r">role</param>
        /// <returns>Equality</returns>
        public bool Equals(CMS_Role r)
        {
            return (r.getName() == _name);
        }

        /// <summary>
        /// Parental role
        /// </summary>
        /// <returns>parental role</returns>
        public CMS_Role getParent()
        {
            return _parent;
        }

    }
}
