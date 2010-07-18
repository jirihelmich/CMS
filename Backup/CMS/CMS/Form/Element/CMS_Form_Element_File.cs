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
    public class CMS_Form_Element_File : CMS_Form_Element
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">name</param>
        public CMS_Form_Element_File(string name)
            : base(name)
        { 
            
        }

        /// <summary>
        /// Renders HTML markup
        /// </summary>
        /// <returns>HTML markup</returns>
        public override string render()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<input type=\"file\" name=\"" + _name + "\"");

            if (_class != string.Empty)
            {
                builder.Append(" class=\"" + _class + "\"");
            }

            builder.Append(" />");

            return builder.ToString();
        }

    }
}
