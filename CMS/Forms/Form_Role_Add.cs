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

namespace CMS.Forms
{
    public class Form_Role_Add : CMS_Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">parentid</param>
        public Form_Role_Add(long parent)
        {
            _action = new CMS_Action("/backend/AddRole?parent="+parent.ToString());

            CMS_Form_Element_Textbox name = new CMS_Form_Element_Textbox("name");
            name.setRequired();
            name.setLabel("Category title");

            this.addElement(name);

            CMS_Form_Element_Submit ok = new CMS_Form_Element_Submit("ok");
            ok.setLabel("Add the role");

            this.addElement(ok);
        }

        /// <summary>
        /// Sets data for edit mode
        /// </summary>
        /// <param name="r">role</param>
        public void setEditData(role r)
        {
            this._action = new CMS_Action("/backend/editRole?id=" + r.id);
            this._elements["name"].setValue(r.name);
            this._elements["ok"].setLabel("Save changes");
        }
    }
}
