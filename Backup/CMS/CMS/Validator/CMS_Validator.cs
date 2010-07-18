using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.CMS.Form.Element;

namespace CMS.CMS.Validator
{
    public interface CMS_Validator
    {
        /// <summary>
        /// Visitor pattern style method - validates the value of the given element
        /// </summary>
        /// <param name="e">Element</param>
        /// <returns>is valid</returns>
        bool validate(CMS_Form_Element e);

    }
}
