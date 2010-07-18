using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CMS.CMS.Form;
using CMS.CMS.Form.Element;
using CMS.CMS.Validator;
using CMS.Models;

namespace CMS.Forms
{
    public class Form_Article_New : CMS_Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Form_Article_New()
        {

            _action = new CMS_Action("/backend/NewArticle");

            CMS_Form_Element_Textbox title = new CMS_Form_Element_Textbox("title");
            title.setLabel("Title");
            title.setRequired();
            title.addValidator(new CMS_Validator_StringLength(0, 10));

            this.addElement(title);

            CMS_Form_Element_Textarea perex = new CMS_Form_Element_Textarea("perex");
            perex.setRequired();
            perex.setClass("ckeditor");
            perex.setLabel("Introtext");

            CMS_Validator_StringLength v = new CMS_Validator_StringLength();
            v.setMaxLength(1000);
            perex.addValidator(v);

            this.addElement(perex);

            CMS_Form_Element_Textarea text = new CMS_Form_Element_Textarea("text");
            text.setRequired();
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
            published.setLabel("Publish date");

            this.addElement(published);

            CMS_Form_Element_DateTime pullback = new CMS_Form_Element_DateTime("pullback");
            pullback.setLabel("Pullback date");

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

            this.addElement(published_bool);

            CMS_Form_Element_Submit submit = new CMS_Form_Element_Submit("submit");
            submit.setLabel("Save the article");
            this.addElement(submit);
        
        }

        /// <summary>
        /// Sets data for the selecbox
        /// </summary>
        /// <param name="authors">list of authors</param>
        /// <returns>floating object</returns>
        public Form_Article_New setAuthors(List<author> authors)
        {
            foreach (author a in authors)
            {
                ((CMS_Form_Element_Select)this._elements["authors"]).addOption(a.id.ToString(), a.lastname + " " + a.name);
            }

            return this;
        }

        /// <summary>
        /// Sets data for the selecbox
        /// </summary>
        /// <param name="authors">list of roles</param>
        /// <returns>floating object</returns>
        public Form_Article_New setRoles(List<role> roles)
        {
            foreach (role r in roles)
            {
                ((CMS_Form_Element_Select)this._elements["roles"]).addOption(r.id.ToString(), r.name);
            }

            return this;
        }

        /// <summary>
        /// Sets data for the selecbox
        /// </summary>
        /// <param name="authors">list of categories</param>
        /// <returns>floating object</returns>
        public Form_Article_New setCategories(List<category> categories)
        {
            foreach (category c in categories)
            {
                ((CMS_Form_Element_Select)this._elements["categories"]).addOption(c.id.ToString(), c.name);
            }
            return this;
        }

    }
}
