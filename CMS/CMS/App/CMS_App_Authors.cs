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
using CMS.Models;

namespace CMS.CMS.App
{
    public class CMS_App_Authors
    {

        /// <summary>
        /// Returns all authors
        /// </summary>
        /// <returns></returns>
        public List<author> getAll()
        {
            using (LangDataContext a = new LangDataContext())
            {
                var data = a.authors.ToList();
                return data;
            }
        }

    }
}
