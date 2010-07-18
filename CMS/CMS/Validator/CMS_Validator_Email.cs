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
    public class CMS_Validator_Email : CMS_Validator
    {
        /// <summary>
        /// Visitor pattern style method - validates the value of the given element
        /// </summary>
        /// <param name="e">Element</param>
        /// <returns>is valid</returns>
        public bool validate(CMS_Form_Element e)
        {
            try {
                RegexStringValidator v = new RegexStringValidator("[a-zA-Z.-_]+@[a-zA-Z.-_].[a-zA-Z]{2,4}");
                v.Validate(e.getValue());
            }
            catch(Exception )
            {
                e.addValidationError("The given value is not a valid e-mail address");
                return false;
            }
            return true;
        }

    }
}
