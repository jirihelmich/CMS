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
    public class Form_LoginForm : CMS_Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Form_LoginForm(string backUrl)
        {
            _action = new CMS_Action("/user/login");

            CMS_Form_Element_Textbox username = new CMS_Form_Element_Textbox("username");
            username.setLabel("Uživatelské jméno");
            username.setRequired();

            addElement(username);

            CMS_Form_Element_Hidden hidden = new CMS_Form_Element_Hidden("backUrl");
            hidden.setValue(backUrl);

            addElement(hidden);

            CMS_Form_Element_Password pw = new CMS_Form_Element_Password("password");
            pw.setLabel("Heslo");
            pw.setRequired();

            addElement(pw);

            CMS_Form_Element_Submit submit = new CMS_Form_Element_Submit("submit");
            submit.setLabel("Přihlásit se");

            addElement(submit);

        }
    }
}
