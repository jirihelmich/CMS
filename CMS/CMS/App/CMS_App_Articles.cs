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
using System.Text;

namespace CMS.CMS.App.Articles
{
    public class CMS_App_Articles
    {
        /// <summary>
        /// 
        /// </summary>
        protected CMS_App _app;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public CMS_App_Articles(CMS_App app)
        {
            _app = app;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<article> getFrontpage(int count, int start, string order)
        {
            List<long> allowedLevels = this._app.allowedLevels();
            using (LangDataContext a = new LangDataContext())
            {
                var data = a.articles
                    .Where(x => x.published == 1 && allowedLevels.Contains(x.level) && x.date_published <= DateTime.Now && (x.date_pullback <= x.date_published || x.date_pullback > DateTime.Now));
               
                if (order == "date")
                {
                    return data.OrderByDescending(x => x.date_published)
                        .Skip(start)
                        .Take(count).ToList();
                }else if (order == "id")
                {
                    return data.OrderBy(x => x.id)
                        .Skip(start)
                        .Take(count).ToList();
                }
                else if (order == "hits")
                {
                    return data.OrderByDescending(x => x.hits)
                        .Skip(start)
                        .Take(count).ToList();
                }
                else if (order == "title")
                {
                    return data.OrderBy(x => x.title)
                            .Skip(start)
                            .Take(count).ToList();
                }else{
                    return data.Skip(start).Take(count).ToList();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<article> getFrontpage(int count)
        {
            return this.getFrontpage(count, 0, "date");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<article> getFrontpage(int count, string order)
        {
            return this.getFrontpage(count, 0, order);
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
                var data = a.articles
                    .Where(x => x.published == 1 && levels.Contains(x.level) && x.date_published <= DateTime.Now && (x.date_pullback <= x.date_published || x.date_pullback > DateTime.Now))
                    .Count();
                return data;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Array getAll()
        {
            return this.getAll(new LangDataContext());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Array getAll(LangDataContext a)
        {
            using (a)
            {
                var data = a.articles;
                return data.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public article getById(long id)
        {
            return getById(id, new LangDataContext());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public article getById(long id, LangDataContext a)
        {
            using (a)
            {
                try
                {
                    var data = a.articles
                        .Single(x => x.id == id);

                    data.hits++;

                    try
                    {
                        a.SubmitChanges();
                    }
                    catch (Exception)
                    {
                    }

                    return data;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool newArticle(CMS_Form form, HttpRequestBase Request)
        {
            DateTime published = this._app.dateFromString(form["published"].getValue());
            DateTime pullback = this._app.dateFromString(form["pullback"].getValue());

            article newArticle = new article();
            newArticle.date_published = published;
            newArticle.date_lastmod = newArticle.date = DateTime.Now;
            newArticle.alias = this._app.makeAlias(form["title"].getValue());
            newArticle.date_pullback = pullback;
            newArticle.fulltext = form["text"].getValue();
            newArticle.hits = 0;
            newArticle.modifications_count = 1;
            newArticle.title = form["title"].getValue();
            newArticle.introtext = form["perex"].getValue();
            newArticle.level = int.Parse(form["roles"].getValue());
            newArticle.published = int.Parse(form["published_bool"].getValue());

            long id;

            using (LangDataContext artDataCntx = new LangDataContext())
            {
                artDataCntx.articles.InsertOnSubmit(newArticle);

                try
                {
                    artDataCntx.SubmitChanges();
                }
                catch (Exception)
                {
                    return false;
                }

                id = newArticle.id;

                foreach (string articleAuthorIdString in ((CMS_Form_Element_Select)form["authors"]).getValues())
                {
                    articles_author authorArticles = new articles_author();
                    authorArticles.articlesid = id;
                    authorArticles.authorsid = long.Parse(articleAuthorIdString);
                    authorArticles.date = DateTime.Now;
                    newArticle.articles_authors.Add(authorArticles);
                }

                string[] tags = form["tags"].getValue().Split(' ');

                foreach (string t in tags)
                {
                    long tagId = 0;
                    try
                    {
                        var tag = artDataCntx.tags.Where(x => x.name == t).Single();
                        tagId = tag.id;
                    }
                    catch (InvalidOperationException)
                    {
                        tag newTag = new tag();
                        newTag.name = t;
                        newTag.date = DateTime.Now;

                        artDataCntx.tags.InsertOnSubmit(newTag);

                        try
                        {
                            artDataCntx.SubmitChanges();
                        }
                        catch (Exception)
                        {
                            return false;
                        }

                        tagId = newTag.id;
                    }

                    tags_article tagArticle = new tags_article();
                    tagArticle.articlesid = id;
                    tagArticle.tagsid = tagId;

                    artDataCntx.tags_articles.InsertOnSubmit(tagArticle);
                    
                }

                try
                {
                    artDataCntx.SubmitChanges();
                }
                catch (Exception)
                {
                    return false;
                }

                CMS_Form_Element_Select cats = (CMS_Form_Element_Select)form["categories"];
                
                foreach (string cat in cats.getValues())
                {
                    long catId = long.Parse(cat);

                    articles_category articleCategory = new articles_category();
                    articleCategory.date = DateTime.Now;
                    articleCategory.articlesid = id;
                    articleCategory.categoriesid = catId;

                    artDataCntx.articles_categories.InsertOnSubmit(articleCategory);
                }

                try
                {
                    artDataCntx.SubmitChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            if (Request.Files.Count != 2)
            {
                CMS_Services_Message.getInstance().addError("Unexpected count of uploaded files, skipping.");
            }
            else
            {

                HttpPostedFileBase small = Request.Files[0];
                if (small.ContentLength > 0 && small.ContentType == "image/jpeg")
                {
                    string filename = id.ToString();
                    var path = Path.Combine(Request.MapPath("./../images"), filename + "_small.jpg");

                    small.SaveAs(path);

                    System.Drawing.Image i = System.Drawing.Image.FromFile(path);
                    if (i.Width != 100 || i.Height != 100)
                    {
                        CMS_Services_Message.getInstance().addError("Invalid image size - small icon should be 100x100 pixels");
                        FileInfo f = new FileInfo(path);
                        f.Delete();
                    }
                }
                else
                {
                    CMS_Services_Message.getInstance().addError("Invalid image - small icon");
                }


                HttpPostedFileBase big = Request.Files[1];
                if (big.ContentLength > 0 && big.ContentType == "image/jpeg")
                {
                    string filename = id.ToString();
                    var path = Path.Combine(Request.MapPath("./../images"), filename + "_big.jpg");

                    big.SaveAs(path);

                    System.Drawing.Image i = System.Drawing.Image.FromFile(path);
                    if (i.Width != 320 || i.Height != 240)
                    {
                        CMS_Services_Message.getInstance().addError("Invalid image size - big icon should be 320x240 pixels");
                        FileInfo f = new FileInfo(path);
                        f.Delete();
                    }
                }
                else
                {
                    CMS_Services_Message.getInstance().addError("Invalid image - big icon");
                }

            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public bool saveArticle(CMS_Form form, HttpRequestBase Request)
        {
            long id = long.Parse(Request.Params["id"]);

            using (LangDataContext dataContext = new LangDataContext())
            {

                DateTime published = this._app.dateFromString(form["published"].getValue());
                DateTime pullback = this._app.dateFromString(form["pullback"].getValue());

                try
                {
                    article original = dataContext.articles.Where(x => x.id == id).Single();
                    article modified = dataContext.articles.Where(x => x.id == id).Single();

                    modified.date_published = published;
                    modified.date_lastmod = DateTime.Now;
                    modified.alias = this._app.makeAlias(form["title"].getValue());
                    modified.date_pullback = pullback;
                    modified.fulltext = form["text"].getValue();
                    modified.modifications_count++;
                    modified.title = form["title"].getValue();
                    modified.introtext = form["perex"].getValue();
                    modified.level = int.Parse(form["roles"].getValue());
                    modified.published = int.Parse(form["published_bool"].getValue());

                    dataContext.articles_authors.DeleteAllOnSubmit(dataContext.articles_authors.Where(x => x.articlesid == id));

                    try
                    {
                        dataContext.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    foreach (string articleAuthorIdString in ((CMS_Form_Element_Select)form["authors"]).getValues())
                    {
                        articles_author authorArticles = new articles_author();
                        authorArticles.articlesid = id;
                        authorArticles.authorsid = long.Parse(articleAuthorIdString);
                        authorArticles.date = DateTime.Now;
                        modified.articles_authors.Add(authorArticles);
                    }

                    string[] tags = form["tags"].getValue().Split(' ');

                    dataContext.tags_articles.DeleteAllOnSubmit(dataContext.tags_articles.Where(x => x.articlesid == id));

                    try
                    {
                        dataContext.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    foreach (string t in tags)
                    {
                        long tagId = 0;
                        try
                        {
                            var tag = dataContext.tags.Where(x => x.name == t).Single();
                            tagId = tag.id;
                        }
                        catch (InvalidOperationException)
                        {
                            tag newTag = new tag();
                            newTag.name = t;
                            newTag.date = DateTime.Now;

                            dataContext.tags.InsertOnSubmit(newTag);

                            try
                            {
                                dataContext.SubmitChanges();
                            }
                            catch
                            {
                                return false;
                            }

                            tagId = newTag.id;
                        }

                        tags_article tagArticle = new tags_article();
                        tagArticle.articlesid = id;
                        tagArticle.tagsid = tagId;

                        dataContext.tags_articles.InsertOnSubmit(tagArticle);
                        
                    }

                    dataContext.articles_categories.DeleteAllOnSubmit(dataContext.articles_categories.Where(x => x.articlesid == id));

                    try
                    {
                        dataContext.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    CMS_Form_Element_Select cats = (CMS_Form_Element_Select)form["categories"];
                    
                    foreach (string cat in cats.getValues())
                    {
                        long catId = long.Parse(cat);

                        articles_category articleCategory = new articles_category();
                        articleCategory.date = DateTime.Now;
                        articleCategory.articlesid = id;
                        articleCategory.categoriesid = catId;

                        dataContext.articles_categories.InsertOnSubmit(articleCategory);
                    }

                    try
                    {
                        dataContext.SubmitChanges();
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    if (Request.Files.Count != 2)
                    {
                        CMS_Services_Message.getInstance().addError("Unexpected count of uploaded files, skipping.");
                    }
                    else
                    {

                        HttpPostedFileBase small = Request.Files[0];
                        if (small.ContentLength > 0 && small.ContentType == "image/jpeg")
                        {
                            string filename = id.ToString();
                            var path = Path.Combine(Request.MapPath("./../images"), filename + "_small.jpg");

                            small.SaveAs(path);

                            System.Drawing.Image i = System.Drawing.Image.FromFile(path);
                            if (i.Width != 100 || i.Height != 100)
                            {
                                CMS_Services_Message.getInstance().addError("Invalid image size - small icon should be 100x100 pixels");
                                FileInfo f = new FileInfo(path);
                                f.Delete();
                            }
                        }
                        else
                        {
                            CMS_Services_Message.getInstance().addError("Invalid image - small icon");
                        }


                        HttpPostedFileBase big = Request.Files[1];
                        if (big.ContentLength > 0 && big.ContentType == "image/jpeg")
                        {
                            string filename = id.ToString();
                            var path = Path.Combine(Request.MapPath("./../images"), filename + "_big.jpg");

                            big.SaveAs(path);

                            System.Drawing.Image i = System.Drawing.Image.FromFile(path);
                            if (i.Width != 320 || i.Height != 240)
                            {
                                CMS_Services_Message.getInstance().addError("Invalid image size - big icon should be 320x240 pixels");
                                FileInfo f = new FileInfo(path);
                                f.Delete();
                            }
                        }
                        else
                        {
                            CMS_Services_Message.getInstance().addError("Invalid image - big icon");
                        }
                    }

                }
                catch(InvalidOperationException) {
                    CMS_Services_Message.getInstance().addError("Article with specified ID does not exit");
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<articles_author> getAuthorsById(long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                return a.articles_authors.Where(x => x.articlesid == id).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<author> getAuthorsListById(long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                return a.articles_authors.Where(x => x.articlesid == id).Select(x => x.author).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<articles_category> getCategoriesById(long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                return a.articles_categories.Where(x => x.articlesid == id).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<category> getCategoriesListById(long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                return a.articles_categories.Where(x => x.articlesid == id).Select(x => x.category).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<tag> getTagsById(long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                return a.tags_articles.Where(x => x.articlesid == id).Select(x => x.tag).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void deleteArticle(long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                a.articles.DeleteAllOnSubmit(a.articles.Where(x => x.id == id));

                try
                {
                    a.SubmitChanges();
                }
                catch(Exception)
                {
                    CMS.Services.CMS_Services_Message.getInstance().addError("The article hasn't been deleted.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<long> getAllLevels()
        {
            using (LangDataContext a = new LangDataContext())
            {
                return a.articles.Select(x => x.level).Distinct().ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string getAuthorsStringById(long p)
        {

            using (LangDataContext a = new LangDataContext())
            {
                StringBuilder authorStr = new StringBuilder();
                bool first = true;

                foreach (author authorItem in a.articles_authors.Where(x => x.articlesid == p).Select(x => x.author).ToList())
                {
                    if (!first) authorStr.Append(", ");
                    authorStr.Append(authorItem.lastname + " " + authorItem.name);
                    first = false;
                }

                return authorStr.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<comment> getCommentsById(long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                return a.comments.Where(x => x.articlesid == id).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public List<article> getByCategoryId(long id, int count, int start)
        {
            using (LangDataContext a = new LangDataContext())
            {
                return a.articles_categories
                    .Where(x => x.categoriesid == id)
                    .Select(x => x.article)
                    .Skip(start)
                    .Take(count).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int getCountByCategoryId(long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                return a.articles_categories
                    .Where(x => x.categoriesid == id)
                    .Select(x => x.article)
                    .Count();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public List<article> getByAuthorId(long id, int count, int start)
        {
            using (LangDataContext a = new LangDataContext())
            {
                return a.articles_authors
                    .Where(x => x.authorsid == id)
                    .Select(x => x.article)
                    .Skip(start)
                    .Take(count).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int getCountByAuthorId(long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                return a.articles_authors
                    .Where(x => x.authorsid == id)
                    .Select(x => x.article)
                    .Count();
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
                return a.articles.Count();
            }
        }
    }
}
