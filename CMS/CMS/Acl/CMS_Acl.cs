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
using System.Collections.Generic;

namespace CMS.CMS.Acl
{
    public class CMS_Acl : ICloneable
    {
        /// <summary>
        /// Roles storage
        /// </summary>
        protected Dictionary<String,CMS_Role> _roles = new Dictionary<String,CMS_Role>();

        /// <summary>
        /// Resources storage
        /// </summary>
        protected Dictionary<String,CMS_Resource> _resources = new Dictionary<String,CMS_Resource>();

        /// <summary>
        /// List of allowed couples - role & resource
        /// </summary>
        protected CoupleList _allowed = new CoupleList();

        /// <summary>
        /// List of denied couples - role & resource
        /// </summary>
        protected CoupleList _denied = new CoupleList();

        /// <summary>
        /// Class for comfortable handling paired roles and resources
        /// </summary>
        public class Couple : IEquatable<Couple>
        {
            /// <summary>
            /// Role instance
            /// </summary>
            protected CMS_Role _role;

            /// <summary>
            /// Resource instance
            /// </summary>
            protected CMS_Resource _resource;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="role">Role</param>
            /// <param name="resource">Resource</param>
            public Couple(CMS_Role role, CMS_Resource resource)
            {
                _role = role;
                _resource = resource;
            }

            /// <summary>
            /// IEquatable method for comparing couples
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            public bool Equals(Couple c)
            {
                return (c._role == _role && c._resource == _resource);
            }

        }

        /// <summary>
        /// List of couples (nested class)
        /// </summary>
        public class CoupleList : ICloneable
        {
            /// <summary>
            /// List of couples
            /// </summary>
            protected List<Couple> _couples = new List<Couple>();

            /// <summary>
            /// Constructor
            /// </summary>
            public CoupleList()
            {
            
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="couples">List of couples</param>
            public CoupleList(List<Couple> couples)
            {
                _couples = couples;
            }

            /// <summary>
            /// Add a new couple into the list
            /// </summary>
            /// <param name="c"></param>
            public void Add(Couple c)
            {
                _couples.Add(c);
            }

            /// <summary>
            /// Determines if the given couple is in the list
            /// </summary>
            /// <param name="c">couple</param>
            /// <returns>bool</returns>
            public bool Contains(Couple c)
            {
                return _couples.Contains(c);
            }

            /// <summary>
            /// ICloneable
            /// </summary>
            /// <returns></returns>
            public object Clone()
            {
                List<Couple> newList = new List<Couple>();
                foreach (Couple c in _couples)
                {
                    newList.Add(c);
                }

                return new CoupleList(newList);
            }
        }

        /// <summary>
        /// Add the given role into the storage
        /// </summary>
        /// <param name="role">role instance</param>
        /// <returns>floating object</returns>
        public CMS_Acl addRole(CMS_Role role)
        {
            if (!_roles.ContainsValue(role)) _roles.Add(role.getName(), role);
            return this;
        }

        /// <summary>
        /// Add the give resource into the storage
        /// </summary>
        /// <param name="resource">resource instance</param>
        /// <returns>floating object</returns>
        public CMS_Acl addResource(CMS_Resource resource)
        {
            if (!_resources.ContainsValue(resource)) _resources.Add(resource.getName(), resource);
            return this;
        }

        /// <summary>
        /// Allow access for the given role to the given resource 
        /// </summary>
        /// <param name="role">role name</param>
        /// <param name="resource">resource name</param>
        /// <returns>floating object</returns>
        public CMS_Acl allow(string role, string resource)
        {
            return allow(_getRoleByName(role), _getResourceByName(resource));
        }

        /// <summary>
        /// Allow access for the given role to the given resource 
        /// </summary>
        /// <param name="role">role name</param>
        /// <param name="resource">resource name</param>
        /// <returns>floating object</returns>
        public CMS_Acl allow(CMS_Role role, CMS_Resource resource)
        {
            _allowed.Add(new Couple(role, resource));
            return this;
        }

        /// <summary>
        /// Deny access for the given role to the given resource
        /// </summary>
        /// <param name="role">role instance</param>
        /// <param name="resource">resource instance</param>
        /// <returns></returns>
        public CMS_Acl deny(CMS_Role role, CMS_Resource resource)
        {
            _denied.Add(new Couple(role, resource));
            return this;
        }

        /// <summary>
        /// Determines if the given role has access to the given 
        /// </summary>
        /// <param name="role">role name</param>
        /// <param name="resource">resource name</param>
        /// <returns>bool</returns>
        public Boolean isAllowed(String role, String resource)
        {
            return isAllowed(_getRoleByName(role.ToLower()), _getResourceByName(resource.ToLower()));
        }

        /// <summary>
        /// Determines if the given role has access to the given 
        /// </summary>
        /// <param name="role">role instance</param>
        /// <param name="resource">resource instance</param>
        /// <returns>bool</returns>
        public Boolean isAllowed(CMS_Role role, CMS_Resource resource)
        {
            while (role != null)
            {
                CMS_Resource res = resource;
                while (res != null)
                {
                    Couple c = new Couple(role, res);
                    if (_denied.Contains(c))
                    {
                        return false;
                    }
                    if (_allowed.Contains(c))
                    {
                        return true;
                    }
                    res = res.getParent();
                }
                role = role.getParent();
            }
            return false;
        }

        /// <summary>
        /// Returns a stored instance of resource with the given name
        /// </summary>
        /// <param name="name">resource name</param>
        /// <returns>resource instance</returns>
        protected CMS_Resource _getResourceByName(String name)
        {
            name = name.ToLower();
            if (!_resources.ContainsKey(name)) throw new ACLResourceNotRegisteredException(name);
            return _resources[name];
        }

        /// <summary>
        /// Returns a stored instance of role with the given name
        /// </summary>
        /// <param name="name">role name</param>
        /// <returns>role instance</returns>
        protected CMS_Role _getRoleByName(String name)
        {
            name = name.ToLower();
            if (!_roles.ContainsKey(name)) throw new ACLRoleNotRegisteredException(name);
            return _roles[name];
        }

        /// <summary>
        /// IClonable method
        /// </summary>
        /// <returns>clone of the current instance</returns>
        public object Clone()
        {
            CMS_Acl clone = new CMS_Acl();
            clone._resources = _resources;
            clone._roles = _roles;
            clone._allowed = (CoupleList)_allowed.Clone();
            clone._denied = (CoupleList)_denied.Clone();

            return clone;
        }

    }
}
