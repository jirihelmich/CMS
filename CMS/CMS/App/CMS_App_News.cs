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

namespace CMS.CMS.App.News
{
    public class CMS_App_News
    {
        /// <summary>
        /// 
        /// </summary>
        protected CMS_App _app;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public CMS_App_News(CMS_App app)
        {
            _app = app;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<NewsListOutputModel> getAll()
        {
            return this.getAll(new LangDataContext());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public List<NewsListOutputModel> getAll(LangDataContext a)
        {
            using (a)
            {
                var data = a.news;

                List<NewsListOutputModel> l = new List<NewsListOutputModel>();

                foreach (news n in data)
                {
                    NewsListOutputModel nom = this.getNOM(n);

                    l.Add(nom);
                }

                return l;
            }
        }

        public NewsListOutputModel get(long id)
        {
            using (LangDataContext dC = new LangDataContext())
            {
                return getNOM(getById(id, dC));
            }
        }

        private NewsListOutputModel getNOM(news p)
        {
            NewsListOutputModel pom = new NewsListOutputModel();
            pom.Id = p.id;

            LangOutputModel title = new LangOutputModel();
            LangOutputModel shortdesc = new LangOutputModel();
            LangOutputModel content = new LangOutputModel();

            foreach (string lang in Helpers.LangHelper.langs)
            { 
                title.setByCulture(lang, p.text1.texts_values.Single(x => x.culture == lang).value);
                shortdesc.setByCulture(lang, p.text2.texts_values.Single(x => x.culture == lang).value);
                content.setByCulture(lang, p.text3.texts_values.Single(x => x.culture == lang).value);
            }

            pom.Title = title;
            pom.Shortdesc = shortdesc;
            pom.Content = content;

            return pom;
        }

        public bool delete(long id)
        {
            using (LangDataContext dataContext = new LangDataContext())
            {
                dataContext.news.DeleteOnSubmit(dataContext.news.Where(x => x.id == id).Single());

                dataContext.SubmitChanges();

                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public news getById(long id)
        {
            return getById(id, new LangDataContext());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public news getById(long id, LangDataContext a)
        {

                try
                {
                    var data = a.news
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
                news p = dC.news.Where(x => x.id == input.request[0].Id).Single();

                foreach (string lang in Helpers.LangHelper.langs)
                {
                    var titleTxt = p.text1.texts_values.SingleOrDefault(x => x.culture == lang);
                    var shortdescTxt = p.text2.texts_values.SingleOrDefault(x => x.culture == lang);
                    var txtTxt = p.text3.texts_values.SingleOrDefault(x => x.culture == lang);

                    if (titleTxt == null)
                    {
                        titleTxt = new texts_value();
                        titleTxt.culture = lang;
                        titleTxt.text = p.text1;
                    }

                    if (txtTxt == null)
                    {
                        txtTxt = new texts_value();
                        txtTxt.culture = lang;
                        txtTxt.text = p.text3;
                    }

                    if (shortdescTxt == null)
                    {
                        shortdescTxt = new texts_value();
                        shortdescTxt.culture = lang;
                        shortdescTxt.text = p.text2;
                    }

                    titleTxt.value = input.request.Single(x => x.lang == lang).data.title;
                    titleTxt.seo_value = this._app.makeAlias(input.request.Single(x => x.lang == lang).data.title);

                    txtTxt.value = input.request.Single(x => x.lang == lang).data.content;
                }

                dC.SubmitChanges();
            }

            return true;
        }

        public long add(NewsInputModel input)
        {
            using (LangDataContext dataContext = new LangDataContext())
            {
                news p = new news();

                text title = new text();
                text shortdesc = new text();
                text content = new text();

                dataContext.news.InsertOnSubmit(p);
                dataContext.texts.InsertOnSubmit(title);
                dataContext.texts.InsertOnSubmit(content);

                p.text1 = title;
                p.text2 = shortdesc;
                p.text3 = content;

                foreach (NewsInputModel.News newPage in input.request)
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

                    texts_value shortdescValue = new texts_value();
                    shortdescValue.culture = newPage.lang;
                    shortdescValue.value = newPage.data.shortdesc;
                    shortdescValue.text = shortdesc;

                    dataContext.texts_values.InsertOnSubmit(titleValue);
                    dataContext.texts_values.InsertOnSubmit(contentValue);
                    dataContext.texts_values.InsertOnSubmit(shortdescValue);
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
                return a.news.Count();
            }
        }
    }
}
