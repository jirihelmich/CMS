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
using System.Text;
using CMS.Models;
using System.Collections.Generic;

namespace CMS.Helpers
{
    public class CommentHelper
    {
        /// <summary>
        /// Renders comments tree
        /// </summary>
        /// <param name="key">current tree key</param>
        /// <param name="tree">whole tree</param>
        /// <param name="add">Render new comment link</param>
        /// <returns>Rendered data</returns>
        public static string Help(long key, Dictionary<long, List<comment>> tree, bool add)
        {
            if (tree.ContainsKey(key))
            {
                foreach (comment c in tree[key])
                {
                    StringBuilder b = new StringBuilder();
                    b.Append("<div class=\"comment\">");
                    b.Append("<div class=\"header\">");
                    b.Append(c.useralias);
                    b.Append(" wrote ");
                    b.Append(c.title);
                    b.Append("</div>");
                    b.Append("<div class=\"text\">");
                    b.Append(c.text);
                    b.Append("</div>");
                    if (add)
                    {
                        b.Append("<div class=\"footer\">");
                        b.Append("<a href=\"/comment/add?id=" + c.id + "\">Reply</a>");
                        b.Append("</div>");
                    }

                    b.Append(CommentHelper.Help(c.id, tree, add));

                    b.Append("</div>");

                    return b.ToString();
                 }
            }

            return String.Empty;
        }

    }
}
