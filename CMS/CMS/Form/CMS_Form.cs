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
using CMS.CMS.Form.Method;
using CMS.CMS.Form.Element;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CMS.CMS.Form
{
    public class CMS_Form
    {
        /// <summary>
        /// URL where we want to send form data on submit
        /// </summary>
        protected CMS_Action _action;

        /// <summary>
        /// HTTP METHOD
        /// </summary>
        protected CMS_Method _method = new CMS_Method_Post();

        /// <summary>
        /// Form elements
        /// </summary>
        protected Dictionary<string, CMS_Form_Element> _elements = new Dictionary<string,CMS_Form_Element>();

        /// <summary>
        /// Determines if render should append multipart/form-data enctype
        /// </summary>
        protected Boolean _multipart = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public CMS_Form()
        {
            
        }

        /// <summary>
        /// Indexer over stored elements
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CMS_Form_Element this[string index]
        {
            get
            {
                return _elements[index];
            }
            set
            {
                addElement(value);    
            }
        }

        /// <summary>
        /// Constructor with specifiing method for the form
        /// </summary>
        /// <param name="method">HTTP method</param>
        public CMS_Form(CMS_Method method)
        {
            _method = method;
        }


        /// <summary>
        /// Constructor with specifiing action for the form
        /// </summary>
        /// <param name="method">Action URL</param>
        public CMS_Form(CMS_Action action)
        {
            _action = action;
        }


        /// <summary>
        /// Constructor with specifiing method and action for the form
        /// </summary>
        /// <param name="method">HTTP method</param>
        /// <param name="action">Action URL</param>
        public CMS_Form(CMS_Action action, CMS_Method method)
        {
            _action = action;
            _method = method;
        }

        /// <summary>
        /// Adds new element to the form
        /// </summary>
        /// <param name="e">form element</param>
        /// <returns>floating object</returns>
        public CMS_Form addElement(CMS_Form_Element e)
        {
            if (_elements.ContainsValue(e))
            {
                throw new FormElementAlreadyAddedException();
            }

            if (e.GetType().ToString() == "CMS.CMS.Form.Element.CMS_Form_Element_File")
            {
                _multipart = true;
            }

            _elements.Add(e.getName(), e);
            return this;
        }

        /// <summary>
        /// Removes element
        /// </summary>
        /// <param name="e">element</param>
        /// <returns>floating object</returns>
        public CMS_Form removeElement(CMS_Form_Element e)
        {
            _elements.Remove(e.getName());
            return this;
        }

        /// <summary>
        /// Renders markup for the form
        /// </summary>
        /// <returns>HTML</returns>
        /// TODO: decorators would be great
        public string render()
        {
            if (_action == null)
            {
                throw new FormActionNotSetException();
            }

            StringBuilder builder = new StringBuilder();

            builder.Append("<form action=\"" + _action.ToString() + "\" method=\"" + _method.ToString() + "\"");

            if (_multipart)
            {
                builder.Append(" enctype=\"multipart/form-data\"");
            }

            builder.Append(">");

            builder.Append("<dl>");

            foreach (CMS_Form_Element e in _elements.Values)
            {

                if (e.Labeled)
                {
                    builder.Append("<dt><label for=\"" + e.getName() + "\">" + e.getLabel() + "</label></dt>");
                }
                else {
                    builder.Append("<dt>&nbsp;</dt>");
                }
                builder.Append("<dd>"+e.render());

                if (e.hasErrors())
                {
                    builder.Append("<ul>");
                    foreach (string err in e.getValidationErrors())
                    {
                        builder.Append("<li>"+err+"</li>");
                    }
                    builder.Append("</ul>");
                }

                builder.Append("</dd>");
            }

            builder.Append("</dl>");
            builder.Append("</form>");

            return builder.ToString();
        }

        /// <summary>
        /// Sets action for the form
        /// </summary>
        /// <param name="a">Action</param>
        /// <returns>floating object</returns>
        public CMS_Form setAction(CMS_Action a)
        {
            _action = a;
            return this;
        }

        /// <summary>
        /// Action getter
        /// </summary>
        /// <returns>Action</returns>
        public CMS_Action getAction()
        {
            return _action;
        }

        /// <summary>
        /// Method getter
        /// </summary>
        /// <returns>HTTP method</returns>
        public CMS_Method getMethod()
        {
            return _method;
        }

        /// <summary>
        /// Runs the validation of the form
        /// </summary>
        /// <param name="form">Form data</param>
        /// <returns></returns>
        public bool isValid(NameValueCollection form)
        {
            bool ret = true;
            foreach (string key in form.AllKeys)
            {
                if (_elements.Keys.Contains<string>(key))
                {

                    _elements[key].setValue(form[key]);

                    if (!_elements[key].isValid())
                    {
                        ret = false;
                    }
                }
                else {
                    ret = false;
                }
            }
            return ret;
        }

        /// <summary>
        /// Gets all form elements in list
        /// </summary>
        /// <returns>List of elements</returns>
        public List<CMS_Form_Element> getElements()
        {
            return _elements.Values.ToList();
        }

    }
}
