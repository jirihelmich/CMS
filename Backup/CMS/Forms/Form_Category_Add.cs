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
    public class Form_Category_Add : CMS_Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">id of parent category</param>
        public Form_Category_Add(long parent)
        {
            _action = new CMS_Action("/backend/AddCategory?parent="+parent.ToString());

            CMS_Form_Element_Textbox name = new CMS_Form_Element_Textbox("name");
            name.setRequired();
            name.setLabel("Category title");

            this.addElement(name);

            CMS_Form_Element_Submit ok = new CMS_Form_Element_Submit("ok");
            ok.setLabel("Add the category");

            this.addElement(ok);
        }

        /// <summary>
        /// Sets data for edit mode
        /// </summary>
        /// <param name="edited">edited entity</param>
        public void setEditData(category c)
        {
            this._action = new CMS_Action("/backend/editCategory?id=" + c.id);
            this._elements["name"].setValue(c.name);
            this._elements["ok"].setLabel("Save changes");
        }
    }
}
