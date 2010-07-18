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
using CMS.Models;
using CMS.CMS.Validator;

namespace CMS.Forms
{
    public class Form_Author_Add : CMS_Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">user id</param>
        public Form_Author_Add(long id)
        {
            _action = new CMS_Action("/backend/PromoteUser?id=" + id.ToString());

            CMS_Form_Element_Textbox name = new CMS_Form_Element_Textbox("name");
            name.setLabel("First name");
            name.setRequired();

            this.addElement(name);

            CMS_Form_Element_Textbox lastname = new CMS_Form_Element_Textbox("lastname");
            lastname.setRequired();
            lastname.setLabel("Lastname");

            this.addElement(lastname);

            CMS_Form_Element_Textarea description = new CMS_Form_Element_Textarea("description");
            description.setLabel("Description");
            description.setRequired();
            description.setClass("ckeditor");

            this.addElement(description);

            CMS_Form_Element_Submit ok = new CMS_Form_Element_Submit("ok");
            ok.setLabel("Promote to author");

            this.addElement(ok);
        }

        /// <summary>
        /// Sets data for edit mode
        /// </summary>
        /// <param name="edited">edited entity</param>
        public void setEditData(author edited)
        {
            _action = new CMS_Action("/backend/EditAuthor?uid=" + edited.usersid.ToString());

            _elements["lastname"].setValue(edited.lastname);
            _elements["name"].setValue(edited.name);
            _elements["description"].setValue(edited.description);

            _elements["ok"].setLabel("Save changes");
        }
    }
}
