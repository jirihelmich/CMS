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
    public class CMS_Form_Element_Submit : CMS_Form_Element
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Element name</param>
        public CMS_Form_Element_Submit(string name) : base(name)
        {
            _labeled = false;
        }


        /// <summary>
        /// Returns HTML for the element
        /// </summary>
        /// <returns></returns>
        public override string render()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<input type=\"submit\" name=\"" + _name + "\" ");



            if (_class != string.Empty)
            {
                builder.Append(" class=\"" + _class + "\"");
            }

            builder.Append("id=\"" + _name + "\" ");
            if (_label != null && _label.Length > 0)
            {
                builder.Append("value=\"" + _label + "\" ");
            }
            builder.Append(" />");

            return builder.ToString();
        }

        /// <summary>
        /// Sets label through input value attrivute
        /// </summary>
        /// <param name="value">Label</param>
        /// <returns>floating object</returns>
        public new CMS_Form_Element setValue(string value)
        {
            _label = value;
            return this;
        }

        /// <summary>
        /// Returns label
        /// </summary>
        /// <returns>label</returns>
        public new string getValue()
        {
            return _label;
        }

        /// <summary>
        /// Labeled flag - there is no need to render dt label for submit button
        /// </summary>
        public new bool Labeled
        {
            get
            {
                return _labeled;
            }
        }

    }
}
