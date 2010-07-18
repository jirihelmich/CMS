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
using System.Collections.Generic;

namespace CMS.CMS.Form.Element
{
    public abstract class CMS_Form_Element
    {
        /// <summary>
        /// element html attrbute name value
        /// </summary>
        protected string _name;

        /// <summary>
        /// element value attribute value
        /// </summary>
        protected string _value;

        /// <summary>
        /// element label
        /// </summary>
        protected string _label;

        /// <summary>
        /// Does the element have a label (e.g. submit btn not)
        /// </summary>
        protected bool _labeled = true;

        /// <summary>
        /// 
        /// </summary>
        protected string _class = "";

        /// <summary>
        /// A list of attached validators
        /// </summary>
        protected List<CMS_Validator> _validators = new List<CMS_Validator>();

        /// <summary>
        /// A list of validation errors
        /// </summary>
        protected List<string> _validationErrors = new List<string>();

        /// <summary>
        /// NonEmptyValidator
        /// </summary>
        protected CMS_Validator_NonEmpty _req;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">HTML attribute name value</param>
        public CMS_Form_Element(string name)
        {
            _name = name;
        }

        /// <summary>
        /// ToString - returns HTML markup
        /// </summary>
        /// <returns>HTML markup</returns>
        public string toString()
        {
            return this.render();
        }

        /// <summary>
        /// Returns HTML for the element
        /// </summary>
        /// <returns>markup</returns>
        public abstract string render();

        /// <summary>
        /// HTML name attribute value getter
        /// </summary>
        /// <returns></returns>
        public string getName()
        {
            return _name;
        }

        /// <summary>
        /// HTML name attribute value setter
        /// </summary>
        /// <param name="name">HTML name attr. value</param>
        /// <returns></returns>
        public CMS_Form_Element setName(string name)
        {
            _name = _filterName(name);
            return this;
        }

        /// <summary>
        /// HTML value attribute value getter
        /// </summary>
        /// <param name="value">HTML value/text attr. value</param>
        /// <returns></returns>
        public virtual CMS_Form_Element setValue(string value)
        {
            _value = value;
            return this;
        }

        /// <summary>
        /// HTML value attribute value getter
        /// </summary>
        /// <returns></returns>
        public virtual string getValue()
        {
            return _value;
        }

        /// <summary>
        /// filters "bad" chars from the element's name
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>filtered name</returns>
        protected string _filterName(string name)
        {
            //TODO;
            return name;
        }

        /// <summary>
        /// Label setter
        /// </summary>
        /// <param name="label">label</param>
        /// <returns>floating object</returns>
        public CMS_Form_Element setLabel(string label)
        {
            _label = label;
            return this;
        }

        /// <summary>
        /// Label getter
        /// </summary>
        /// <returns>label</returns>
        public string getLabel()
        {
            return _label;
        }
        
        /// <summary>
        /// Does this element type have a label?
        /// </summary>
        public bool Labeled
        {
            get
            {
                return _labeled;
            }
        }

        /// <summary>
        /// Attaches another validator
        /// </summary>
        /// <param name="v">validator</param>
        /// <returns>floating object</returns>
        public CMS_Form_Element addValidator(CMS_Validator v)
        {
            _validators.Add(v);
            return this;
        }

        /// <summary>
        /// Checks if all validators validates the value as correct
        /// </summary>
        /// <returns></returns>
        public bool isValid()
        {
            bool ret = true;
            foreach(CMS_Validator v in _validators)
            {
                if (!v.validate(this))
                {
                    ret = false;
                }
            }

            return ret;
        }

        /// <summary>
        /// Adds NonEmpty validator
        /// </summary>
        /// <returns></returns>
        public CMS_Form_Element setRequired()
        {
            return this.setRequired(true);
        }

        /// <summary>
        /// Adds validation error
        /// </summary>
        /// <param name="e">Error</param>
        public void addValidationError(string e)
        {
            _validationErrors.Add(e);
        }

        /// <summary>
        /// Returns array of validation error messages
        /// </summary>
        /// <returns></returns>
        public string[] getValidationErrors()
        {
            return _validationErrors.ToArray();
        }

        /// <summary>
        /// Checks if there was a validation error
        /// </summary>
        /// <returns></returns>
        public bool hasErrors()
        {
            return (_validationErrors.Count > 0);
        }

        /// <summary>
        /// Sets if the value can be empty
        /// </summary>
        /// <param name="p">boolean</param>
        /// <returns>Floating object</returns>
        public CMS_Form_Element setRequired(bool p)
        {
            if (p == false && _req != null)
            {
                _validators.Remove(_req);
            }
            else if (p == true && _req == null) {
                _req = new CMS_Validator_NonEmpty();
                _validators.Add(_req);
            }

            return this;
        }

        /// <summary>
        /// Sets HTML attr class
        /// </summary>
        /// <param name="c">class</param>
        /// <returns>Floating object</returns>
        public CMS_Form_Element setClass(string c)
        {
            _class = c;
            return this;
        }
    }
}
