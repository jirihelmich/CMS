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
    public class Form_Resource_Add : CMS.Form.CMS_Form
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public Form_Resource_Add()
        {
            this._action = new CMS_Action("/backend/AddResource");

            CMS_Form_Element_Textbox name = new CMS_Form_Element_Textbox("name");
            name.setRequired();
            name.setLabel("Name of the resource");

            this.addElement(name);

            CMS_Form_Element_Textbox controller = new CMS_Form_Element_Textbox("controller");
            controller.setRequired();
            controller.setLabel("Name of the controller");

            this.addElement(controller);

            CMS_Form_Element_Textbox action = new CMS_Form_Element_Textbox("action");
            action.setLabel("Name of the action");
            action.setRequired();

            this.addElement(action);

            CMS_Form_Element_Submit submit = new CMS_Form_Element_Submit("ok");
            submit.setLabel("Add the resource");

            this.addElement(submit);
        }

        /// <summary>
        /// Sets data for edit mode
        /// </summary>
        /// <param name="r">resource</param>
        /// <param name="roles">List of roles</param>
        /// <param name="allowed">List of allowed roles</param>
        public void setEditData(resource r, List<role> roles, List<role> allowed)
        {
            _action = new CMS_Action("/backend/EditResource?id=" + r.id);

            this._elements["name"].setValue(r.name);
            this._elements["controller"].setValue(r.controller);
            this._elements["action"].setValue(r.action);

            CMS_Form_Element submit = this._elements["ok"];
            this._elements.Remove("ok");

            List<long> allowedIds = allowed.Select(x => x.id).ToList();

            foreach (role roleItem in roles)
            {
                CMS_Form_Element_Select s = new CMS_Form_Element_Select("role_"+roleItem.id);
                s.setLabel("Acces for role "+roleItem.name);
                s.addOption("-1","Please choose");
                s.addOption("1","Allow");
                s.addOption("0","Deny");

                if (allowedIds.Contains(roleItem.id))
                {
                    s.setValue("1");
                }
                else
                {
                    s.setValue("0");
                }

                this.addElement(s);
            }
            this.addElement(submit);

            this._elements["ok"].setLabel("Save changes");

        }

    }
}
