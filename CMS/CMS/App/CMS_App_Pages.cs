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
                    PageListOutputModel pom = this.getPOM(p);

                    l.Add(pom);
                }

                return l;
            }
        }

        public PageListOutputModel get(long id)
        {
            using (LangDataContext dC = new LangDataContext())
            {
                return getPOM(getById(id, dC));
            }
        }

        private PageListOutputModel getPOM(page p)
        {
            PageListOutputModel pom = new PageListOutputModel();
            pom.Id = p.id;

            LangOutputModel title = new LangOutputModel();
            LangOutputModel content = new LangOutputModel();

            foreach (string lang in Helpers.LangHelper.langs)
            { 
                title.setByCulture(lang, p.text.texts_values.Single(x => x.culture == lang).value);
                content.setByCulture(lang, p.text1.texts_values.Single(x => x.culture == lang).value);
            }

            pom.Title = title;
            pom.Content = content;

            return pom;
        }

        public bool delete(long id)
        {
            using (LangDataContext dataContext = new LangDataContext())
            {
                dataContext.pages.DeleteOnSubmit(dataContext.pages.Where(x => x.id == id).Single());

                dataContext.SubmitChanges();

                return true;
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

        public bool edit(PageInputModel input)
        {
            using (LangDataContext dC = new LangDataContext())
            {
                page p = dC.pages.Where(x => x.id == input.request[0].Id).Single();

                foreach (string lang in Helpers.LangHelper.langs)
                {
                    var titleTxt = p.text.texts_values.SingleOrDefault(x => x.culture == lang);
                    var txtTxt = p.text1.texts_values.SingleOrDefault(x => x.culture == lang);

                    if (titleTxt == null)
                    {
                        titleTxt = new texts_value();
                        titleTxt.culture = lang;
                        titleTxt.text = p.text;
                    }

                    if (txtTxt == null)
                    {
                        txtTxt = new texts_value();
                        txtTxt.culture = lang;
                        txtTxt.text = p.text1;
                    }

                    titleTxt.value = input.request.Single(x => x.lang == lang).data.title;
                    titleTxt.seo_value = this._app.makeAlias(input.request.Single(x => x.lang == lang).data.title);

                    txtTxt.value = input.request.Single(x => x.lang == lang).data.content;
                }

                dC.SubmitChanges();
            }

            return true;
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
                    titleValue.seo_value = this._app.makeAlias(newPage.data.title);

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
