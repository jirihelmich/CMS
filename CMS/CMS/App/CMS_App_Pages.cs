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
using CMS.Models;
using CMS.CMS.Login;
using CMS.CMS.Form;
using CMS.CMS.Form.Element;
using CMS.CMS.Services;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using CMS.CMS.InputModels;
using CMS.CMS.OutputModels;

namespace CMS.CMS.App.Pages
{
    public class CMS_App_Pages
    {
        /// <summary>
        /// 
        /// </summary>
        protected CMS_App _app;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public CMS_App_Pages(CMS_App app)
        {
            _app = app;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getFrontpageArticlesCount()
        {
            List<long> levels = this._app.allowedLevels();

            using (LangDataContext a = new LangDataContext())
            {
                var data = a.pages
                    .Count();
                return data;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PageListOutputModel> getAll()
        {
            return this.getAll(new LangDataContext());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public List<PageListOutputModel> getAll(LangDataContext a)
        {
            using (a)
            {
                var data = a.pages;

                List<PageListOutputModel> l = new List<PageListOutputModel>();

                foreach (page p in data)
                {
                    PageListOutputModel pom = new PageListOutputModel();
                    pom.Id = p.id;

                    LangOutputModel title = new LangOutputModel();
                    title.cz = p.text.texts_values.Where(x => x.culture == "cz").Single().value;
                    title.gb = p.text.texts_values.Where(x => x.culture == "gb").Single().value;
                    title.de = p.text.texts_values.Where(x => x.culture == "de").Single().value;
                    title.ru = p.text.texts_values.Where(x => x.culture == "ru").Single().value;

                    LangOutputModel content = new LangOutputModel();
                    content.cz = p.text1.texts_values.Where(x => x.culture == "cz").Single().value;
                    content.gb = p.text1.texts_values.Where(x => x.culture == "gb").Single().value;
                    content.de = p.text1.texts_values.Where(x => x.culture == "de").Single().value;
                    content.ru = p.text1.texts_values.Where(x => x.culture == "ru").Single().value;

                    pom.Title = title;
                    pom.Content = content;

                    l.Add(pom);
                }

                return l;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public page getById(long id)
        {
            return getById(id, new LangDataContext());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public page getById(long id, LangDataContext a)
        {
            using (a)
            {
                try
                {
                    var data = a.pages
                        .Single(x => x.id == id);
                    return data;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public long add(PageInputModel input)
        {
            using (LangDataContext dataContext = new LangDataContext())
            {
                page p = new page();

                text title = new text();
                text content = new text();

                dataContext.pages.InsertOnSubmit(p);
                dataContext.texts.InsertOnSubmit(title);
                dataContext.texts.InsertOnSubmit(content);

                p.text = title;
                p.text1 = content;

                foreach (PageInputModel.Page newPage in input.request)
                {
                    texts_value titleValue = new texts_value();
                    titleValue.culture = newPage.lang;
                    titleValue.value = newPage.data.title;
                    titleValue.text = title;

                    texts_value contentValue = new texts_value();
                    contentValue.culture = newPage.lang;
                    contentValue.value = newPage.data.content;
                    contentValue.text = content;

                    dataContext.texts_values.InsertOnSubmit(titleValue);
                    dataContext.texts_values.InsertOnSubmit(contentValue);
                }

                dataContext.SubmitChanges();

                return p.id;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getCount()
        {
            using (LangDataContext a = new LangDataContext())
            {
                return a.pages.Count();
            }
        }
    }
}
