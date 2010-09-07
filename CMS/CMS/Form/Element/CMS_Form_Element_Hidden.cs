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
using System.Text;

namespace CMS.CMS.Form.Element
{
    public class CMS_Form_Element_Hidden : CMS_Form_Element
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Element name</param>
        public CMS_Form_Element_Hidden(string name) : base(name)
        {
            
        }

        /// <summary>
        /// Renders HTML markup
        /// </summary>
        /// <returns>HTML markup</returns>
        public override string render()
        {
            if (_name.Trim().Length == 0)
            {
                throw new Form_ElementNameNotSetException();
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<input type=\"hidden\" name=\"" + _name + "\" ");

            if (_class != string.Empty)
            {
                builder.Append(" class=\"" + _class + "\"");
            }

            builder.Append("id=\"" + _name + "\" ");
            if (_value != null && _value.Length > 0)
            {
                builder.Append("value=\"" + _value + "\" ");
            }
            builder.Append(" />");

            return builder.ToString();
        }

    }
}
