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
using CMS.CMS.Form;
using CMS.CMS.Form.Element;

namespace CMS.Forms
{
    public class Form_Settings : CMS_Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="listLength">lists length</param>
        /// <param name="unregComments">unregistred can comment</param>
        public Form_Settings(int listLength, bool unregComments)
        {
            _action = new CMS_Action("/backend/settings");

            CMS_Form_Element_Textbox list_length = new CMS_Form_Element_Textbox("list_length");
            list_length.setLabel("Lists length (number of items displayed on one page)");
            list_length.setRequired();
            list_length.setValue(listLength.ToString());

            this.addElement(list_length);

            CMS_Form_Element_Select unregistered_comments = new CMS_Form_Element_Select("unregistered_comments");
            unregistered_comments.setLabel("Can anonymous post comments?");
            unregistered_comments.setRequired();

            unregistered_comments.addOption("true", "Yes");
            unregistered_comments.addOption("false", "No");

            unregistered_comments.setValue(unregComments.ToString().ToLower());

            this.addElement(unregistered_comments);

            CMS_Form_Element_Submit ok = new CMS_Form_Element_Submit("ok");
            ok.setLabel("Save");
            this.addElement(ok);


        }

    }
}
