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
using CMS.CMS.Validator;

namespace CMS.CMS.Form.Element
{
    public class CMS_Form_Element_DateTime : CMS_Form_Element_Textbox
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">name</param>
        public CMS_Form_Element_DateTime(string name)
            : base(name)
        {

            this.addValidator(new CMS_Validator_DateTime());
        
        }

        /// <summary>
        /// Set value override
        /// </summary>
        /// <see cref="CMS_Form_Element"/>
        /// <param name="value">Value</param>
        /// <returns>Floating object</returns>
        public override CMS_Form_Element setValue(string value)
        {
            string newVal = CMS.App.CMS_App.reFormatDate(value);
            return base.setValue(newVal);
        }

    }
}
