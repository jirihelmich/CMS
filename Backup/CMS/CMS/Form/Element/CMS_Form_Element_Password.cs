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
    public class CMS_Form_Element_Password : CMS_Form_Element
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Element name</param>
        public CMS_Form_Element_Password(string name) : base(name)
        {
            
        }


        /// <summary>
        /// Returns HTML for the element
        /// </summary>
        /// <returns>HTML markup</returns>
        public override string render()
        {
            if (_name.Trim().Length == 0)
            {
                throw new Form_ElementNameNotSetException();
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<input type=\"password\" name=\"" + _name + "\" ");
            builder.Append("id=\"" + _name + "\" ");

            if (_class != string.Empty)
            {
                builder.Append(" class=\"" + _class + "\"");
            }

            if (_value != null && _value.Length > 0)
            {
                builder.Append("value=\"" + _value + "\" ");
            }
            builder.Append(" />");

            return builder.ToString();
        }

    }
}
