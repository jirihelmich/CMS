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
using CMS.Models;
using System.Collections.Generic;

namespace CMS.Forms
{
    public class Form_User_Add : CMS_Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Form_User_Add()
        {

            this._action = new CMS_Action("/backend/AddUser");

            CMS_Form_Element_Textbox username = new CMS_Form_Element_Textbox("username");
            username.setLabel("Username");
            username.setRequired();

            this.addElement(username);

            CMS_Form_Element_Password password = new CMS_Form_Element_Password("password");
            password.setLabel("Password");
            password.setRequired();

            this.addElement(password);

            CMS_Form_Element_Textbox email = new CMS_Form_Element_Textbox("email");
            email.setRequired();
            email.setLabel("E-mail");
            email.addValidator(new CMS_Validator_Email());

            this.addElement(email);

            CMS_Form_Element_Select role = new CMS_Form_Element_Select("role");
            role.setLabel("User role");
            role.setRequired();

            this.addElement(role);

            CMS_Form_Element_Submit ok = new CMS_Form_Element_Submit("ok");
            ok.setLabel("Add the user");

            this.addElement(ok);
        
        }

        /// <summary>
        /// set roles for selectbox
        /// </summary>
        /// <param name="roles">roles</param>
        public void setRoles(List<role> roles)
        {
            CMS_Form_Element_Select s = (CMS_Form_Element_Select)this._elements["role"];
            foreach (role r in roles)
            {
                s.addOption(r.id.ToString(), r.name);                
            }
        }

        /// <summary>
        /// Sets edit data for edit mode
        /// </summary>
        /// <param name="u">user</param>
        public void setEditData(user u)
        {
            this._action = new CMS_Action("/backend/EditUser?id=" + u.id.ToString());

            this._elements["username"].setValue(u.username);
            this._elements["role"].setValue(u.rolesid.ToString());
            this._elements["email"].setValue(u.email);

            this._elements["password"].setRequired(false);

            this._elements["ok"].setLabel("Save changes");
        }
    }
}
