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
using CMS.CMS.Validator;

namespace CMS.Forms
{
    public class Form_Comment_New : CMS_Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="ident">ident (art or parent)</param>
        public Form_Comment_New(long id,string ident)
        {
            _action = new CMS_Action("/comment/add?"+ident+"="+id.ToString());

            CMS_Form_Element_Textbox name = new CMS_Form_Element_Textbox("name");
            name.setRequired();
            name.setLabel("Nickname");

            this.addElement(name);

            CMS_Form_Element_Textbox email = new CMS_Form_Element_Textbox("email");
            email.setRequired();
            email.setLabel("E-mail");
            email.addValidator(new CMS_Validator_Email());

            this.addElement(email);

            CMS_Form_Element_Textbox title = new CMS_Form_Element_Textbox("title");
            title.setLabel("Title");
            title.setRequired();

            this.addElement(title);

            CMS_Form_Element_Textarea text = new CMS_Form_Element_Textarea("text");
            text.setRequired();
            text.setLabel("Comment");

            this.addElement(text);

            CMS_Form_Element_Submit ok = new CMS_Form_Element_Submit("ok");
            ok.setLabel("Comment!");

            this.addElement(ok);
        }

    }
}
