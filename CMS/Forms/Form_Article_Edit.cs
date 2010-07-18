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
using CMS.CMS.Form;
using CMS.CMS.Form.Element;
using CMS.CMS.Validator;
using CMS.Models;
using System.Collections.Generic;
using System.Text;

namespace CMS.Forms
{
	public class Form_Article_Edit : CMS.Form.CMS_Form
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="OriginalArticle">Edited article</param>
        public Form_Article_Edit(article OriginalArticle)
        {

            _action = new CMS_Action("/backend/editArticle?id="+OriginalArticle.id.ToString());

            CMS_Form_Element_Textbox title = new CMS_Form_Element_Textbox("title");
            title.setLabel("Title");
            title.setRequired();
            title.setValue(OriginalArticle.title);
            title.addValidator(new CMS_Validator_StringLength(0, 100));

            this.addElement(title);

            CMS_Form_Element_Textarea perex = new CMS_Form_Element_Textarea("perex");
            perex.setRequired();
            perex.setValue(OriginalArticle.introtext);
            perex.setLabel("Introtext");
            perex.setClass("ckeditor");

            CMS_Validator_StringLength v = new CMS_Validator_StringLength();
            v.setMaxLength(1000);
            perex.addValidator(v);

            this.addElement(perex);

            CMS_Form_Element_Textarea text = new CMS_Form_Element_Textarea("text");
            text.setRequired();
            text.setValue(OriginalArticle.fulltext);
            text.setLabel("Text");
            text.setClass("ckeditor");

            this.addElement(text);

            CMS_Form_Element_Select authors = new CMS_Form_Element_Select("authors");
            authors.setIsMultiSelect();
            authors.setLabel("Authors");
            authors.setSize(4);
            authors.setRequired();

            this.addElement(authors);

            CMS_Form_Element_Select categories = new CMS_Form_Element_Select("categories");
            categories.setIsMultiSelect();
            categories.setLabel("Categories");
            categories.setSize(4);
            categories.setRequired();

            this.addElement(categories);

            CMS_Form_Element_DateTime published = new CMS_Form_Element_DateTime("published");
            published.setRequired();
            published.setValue(OriginalArticle.date_published.ToString());
            published.setLabel("Publish date");

            this.addElement(published);

            CMS_Form_Element_DateTime pullback = new CMS_Form_Element_DateTime("pullback");
            pullback.setLabel("Pullback date");
            pullback.setValue(OriginalArticle.date_pullback.ToString());

            this.addElement(pullback);

            CMS_Form_Element_Select roles = new CMS_Form_Element_Select("roles");
            roles.setRequired();
            roles.setLabel("Role with access to the article");
            roles.setValue("1");

            this.addElement(roles);

            CMS_Form_Element_File smallIcon = new CMS_Form_Element_File("smallIcon");
            smallIcon.setRequired();
            smallIcon.setLabel("Small icon");

            this.addElement(smallIcon);

            CMS_Form_Element_File bigIcon = new CMS_Form_Element_File("bigIcon");
            bigIcon.setRequired();
            bigIcon.setLabel("Big icon");

            this.addElement(bigIcon);

            CMS_Form_Element_Textbox tags = new CMS_Form_Element_Textbox("tags");
            tags.setLabel("Tags (whitespace separated)");

            this.addElement(tags);

            CMS_Form_Element_Select published_bool = new CMS_Form_Element_Select("published_bool");
            published_bool.addOption("0", "No");
            published_bool.addOption("1", "Yes");
            published_bool.setLabel("Published state");

            published_bool.setValue(OriginalArticle.published.ToString());

            this.addElement(published_bool);

            CMS_Form_Element_Submit submit = new CMS_Form_Element_Submit("submit");
            submit.setLabel("Save the article");
            this.addElement(submit);
        
        }

        /// <summary>
        /// Sets authors for the selectbox
        /// </summary>
        /// <param name="authors">List of authors</param>
        /// <param name="saved">Currently assigned authors</param>
        /// <returns>floating object</returns>
        public Form_Article_Edit setAuthors(List<author> authors, List<articles_author> saved)
        {
        
            CMS_Form_Element_Select select = ((CMS_Form_Element_Select)this._elements["authors"]);

            foreach (author a in authors)
            {
                select.addOption(a.id.ToString(), a.lastname + " " + a.name);
            }

            select.setValue(saved.Select(x => x.authorsid.ToString()).ToArray());

            return this;
        }

        /// <summary>
        /// Sets roles for the selectbox
        /// </summary>
        /// <param name="roles">List of roles</param>
        /// <param name="saved">Currently assigned roles</param>
        /// <returns>floating object</returns>
        public Form_Article_Edit setRoles(List<role> roles, string saved)
        {
            CMS_Form_Element_Select rolesField = ((CMS_Form_Element_Select)this._elements["roles"]);
            foreach (role r in roles)
            {
                rolesField.addOption(r.id.ToString(), r.name);
            }

            return this;
        }

        /// <summary>
        /// Sets categories for the selectbox
        /// </summary>
        /// <param name="categories">List of categories</param>
        /// <param name="saved">Currently assigned categories</param>
        /// <returns>floating object</returns>
        public Form_Article_Edit setCategories(List<category> categories, List<articles_category> saved)
        {
            CMS_Form_Element_Select categoriesField = ((CMS_Form_Element_Select)this._elements["categories"]);
            foreach (category c in categories)
            {
                //categoriesField.addOption(c.id.ToString(), c.name);
            }

            categoriesField.setValue(saved.Select(x => x.categoriesid.ToString()).ToArray());

            return this;
        }

        /// <summary>
        /// Sets tags for the selectbox
        /// </summary>
        /// <param name="categories">List of tags</param>
        /// <param name="saved">Currently assigned tags</param>
        /// <returns>floating object</returns>
        public Form_Article_Edit setTags(List<tag> tags)
        {
            StringBuilder s = new StringBuilder();

            bool first = true;
            foreach (tag tag in tags)
            {
                if (!first) s.Append(" ");
                s.Append(tag.name);
                first = false;
            }
            this._elements["tags"].setValue(s.ToString());

            return this;
        }
	}
}
