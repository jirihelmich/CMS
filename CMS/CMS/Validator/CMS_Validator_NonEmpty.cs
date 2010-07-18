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
using CMS.CMS.Form.Element;

namespace CMS.CMS.Validator
{
    public class CMS_Validator_NonEmpty : CMS_Validator
    {
        /// <summary>
        /// Visitor pattern style method - validates the value of the given element
        /// </summary>
        /// <param name="e">Element</param>
        /// <returns>is valid</returns>
        public bool validate(CMS_Form_Element e)
        {
            if (!String.IsNullOrEmpty(e.getValue()))
            {
                return true;
            }
            e.addValidationError("You have to fill in this field.");
            return false;
        }

    }
}
