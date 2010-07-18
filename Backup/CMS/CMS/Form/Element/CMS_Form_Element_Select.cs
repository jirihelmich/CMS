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
using System.Collections.Generic;
using System.Text;

namespace CMS.CMS.Form.Element
{
    public class CMS_Form_Element_Select : CMS_Form_Element
    {

        /// <summary>
        /// Determines if the element has multiple values
        /// </summary>
        protected Boolean _isMultiSelect = false;

        /// <summary>
        /// HTML size attr
        /// </summary>
        protected int _size = 1;

        /// <summary>
        /// 
        /// </summary>
        protected List<string> _values = new List<string>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Element name</param>
        public CMS_Form_Element_Select(string name) : base(name)
        {
            
        }

        /// <summary>
        /// Options storage
        /// </summary>
        private Dictionary<string, string> _options = new Dictionary<string, string>();

        /// <summary>
        /// Adds a new option into the list of options
        /// </summary>
        /// <param name="key">posted value</param>
        /// <param name="value">displayed label</param>
        /// <returns>floating object</returns>
        public CMS_Form_Element_Select addOption(string key, string value)
        {
            if (_options.ContainsKey(key))
            {
                throw new ArgumentException("The given key " + key + " already used!");
            }

            _options.Add(key, value);

            return this;
        }

        /// <summary>
        /// Changes multiselect setting
        /// </summary>
        /// <returns>floating object</returns>
        public CMS_Form_Element_Select setIsMultiSelect()
        {
            return this.setIsMultiSelect(true);
        }

        /// <summary>
        /// Changes multiselect setting
        /// </summary>
        /// <param name="flag">flag</param>
        /// <returns>floating object</returns>
        public CMS_Form_Element_Select setIsMultiSelect (bool flag)
        {
            _isMultiSelect = flag;
            return this;
        }

        /// <summary>
        /// Sets the size of the element
        /// </summary>
        /// <param name="size">Size</param>
        /// <returns>floating object</returns>
        public CMS_Form_Element_Select setSize(int size)
        {

            if (size < 1)
            {
                throw new ArgumentException("Size cannot be lower than 1!");
            }

            _size = size;

            return this;
        }

        /// <summary>
        /// Renders HTML markup
        /// </summary>
        /// <returns>HTML markup</returns>
        public override string render()
        {
            if (_name.Trim().Length == 0)
            {
                throw new Form_ElementNameNotSetException();
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<select name=\"" + _name + "\" ");

            if (_isMultiSelect)
            {
                builder.Append("multiple=\"multiple\" ");
            }


            if (_class != string.Empty)
            {
                builder.Append(" class=\"" + _class + "\"");
            }


            builder.Append("size=\"" + _size + "\" ");

            builder.Append(" id=\"" + _name + "\">");

            foreach (string key in _options.Keys)
            {
                builder.Append("<option value=\"" + key + "\"");

                if (_values.Contains(key)) builder.Append(" selected=\"selected\"");

                builder.Append(">" + _options[key] + "</option>");
            }

            builder.Append("</select>");

            return builder.ToString();
        }

        /// <summary>
        /// HTML value attribute value getter
        /// </summary>
        /// <param name="value">HTML value/text attr. value</param>
        /// <returns></returns>
        public override CMS_Form_Element setValue(string value)
        {
            string[] values = value.Split(',');

            _values = values.ToList();

            return this;
        }

        /// <summary>
        /// Sets values
        /// </summary>
        /// <param name="value">values sepatared by comma</param>
        /// <returns>floating object</returns>
        public CMS_Form_Element setValue(string[] value)
        {
            _values = value.ToList();

            return this;
        }

        /// <summary>
        /// Returns the only value if there is no more values
        /// </summary>
        /// <returns>value</returns>
        public override string getValue()
        {
            if (_values.Count == 0) throw new InvalidOperationException("No stored values.");

            return _values.First().ToString();
        }

        /// <summary>
        /// Returns a list of values
        /// </summary>
        /// <returns>List of values</returns>
        public List<string> getValues()
        {
            return _values;
        }

    }
}
