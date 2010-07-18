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
    public class CMS_Validator_DateTime : CMS_Validator
    {

        /// <summary>
        /// Visitor pattern style method - validates the value of the given element
        /// </summary>
        /// <param name="e">Element</param>
        /// <returns>is valid</returns>
        public bool validate(CMS_Form_Element e)
        {
            RegexStringValidator r = new RegexStringValidator("[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2}");

            bool result = true;

            try
            {
                r.Validate(e.getValue());

                string[] vals = e.getValue().Split(' ');
                string[] date = vals[0].Split('-');

                int year = int.Parse(date[0]);
                int month = int.Parse(date[1]);
                if (month > 12) result = false;
                int day = int.Parse(date[2]);

                if (month == 2 && (year % 4 == 0) && day > 29) result = false;
                if (month == 2 && (year % 4 != 0) && day > 28) result = false;

                if (month < 8 && (month % 2 == 0) && day > 30) result = false;
                if (month < 8 && (month % 2 == 1) && day > 31) result = false;

                if (month > 7 && (month % 2 == 0) && day > 31) result = false;
                if (month > 7 && (month % 2 == 0) && day > 31) result = false;

                string[] time = vals[1].Split(':');

                int hour = int.Parse(time[0]);
                if (hour > 24) result = false;

                int minute = int.Parse(time[1]);
                if (minute > 60) result = false;

                int second = int.Parse(time[2]);
                if (second > 60) result = false;
            }
            catch (ArgumentException)
            {
                result = false;
            }

            if (result == false)
            {
                e.addValidationError("The given value doesn't match the datetime pattern YYYY-MM-DD HH:mm:ss");
            }

            return result;
            
        }

    }
}
