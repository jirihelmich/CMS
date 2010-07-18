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

namespace CMS.CMS.Form
{
    public class FormActionNotSetException : Exception
    {
        /// <summary>
        /// Message property
        /// </summary>
        public override string Message
        {
            get
            {
                return "The form has no action set!";
            }
        }
    }
}
