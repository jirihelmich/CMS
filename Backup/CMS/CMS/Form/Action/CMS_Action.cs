using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CMS.CMS.Form
{
    public class CMS_Action
    {

        /// <summary>
        /// Action URL
        /// </summary>
        protected string _url;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">Action URL</param>
        public CMS_Action(string url)
        {
            _url = url;
        }

        /// <summary>
        /// Returns a string representing the object
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return _url;
        }

    }
}
