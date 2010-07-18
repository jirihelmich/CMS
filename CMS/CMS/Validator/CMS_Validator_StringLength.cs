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
    public class CMS_Validator_StringLength : CMS_Validator
    {
        /// <summary>
        /// Maximal length of filled value
        /// </summary>
        int? _maxlength;

        /// <summary>
        /// Minimal length of filled value
        /// </summary>
        int? _minlength;

        /// <summary>
        /// Constructor
        /// </summary>
        public CMS_Validator_StringLength()
        { 
        
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="min">minimal length</param>
        /// <param name="max">maximal length</param>
        public CMS_Validator_StringLength(int min, int max)
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException("Maximal length of string cannot be lower than 0!");
            }

            _minlength = min;
            _maxlength = max;
        }

        /// <summary>
        /// Sets the maximal length
        /// </summary>
        /// <param name="max">length</param>
        /// <returns>floating object</returns>
        public CMS_Validator_StringLength setMaxLength(int max)
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException("Maximal length of string cannot be lower than 0!");
            }

            _maxlength = max;
            return this;
        }

        /// <summary>
        /// Sets the minimal length
        /// </summary>
        /// <param name="min">length</param>
        /// <returns>floating object</returns>
        public CMS_Validator_StringLength setMinLength(int min)
        {
            _minlength = min;
            return this;
        }

        /// <summary>
        /// Visitor pattern style method - validates the value of the given element
        /// </summary>
        /// <param name="e">Element</param>
        /// <returns>is valid</returns>
        public bool validate(CMS_Form_Element e)
        {

            if (_maxlength == null && _minlength == null)
            {
                throw new ArgumentNullException("Both minlength and maxlength not set");
            }

            if (_maxlength != null)
            {
                if (e.getValue().Length > _maxlength)
                {
                    e.addValidationError("You have to fill in no more than " + _maxlength + " characters!");
                    return false;
                }
            }

            if (_minlength != null)
            {
                if (e.getValue().Length < _minlength)
                {
                    e.addValidationError("You have to fill in at least "+_minlength+" characters!");
                    return false;
                }
            }

            return true;
        }

    }
}
