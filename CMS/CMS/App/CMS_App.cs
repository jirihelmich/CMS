using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Models;
using System.Text;
using CMS.CMS.Acl;
using CMS.CMS.Login;
using System.Web.Mvc;
using CMS.CMS.App.Articles;
using CMS.CMS.App.Pages;
using CMS.CMS.App.News;
using CMS.CMS.App.Users;
using CMS.CMS.App.Roles;
using CMS.CMS.App.Categories;
using CMS.CMS.App.Resources;
using System.Globalization;
using System.Xml;
using System.IO;
using System.Configuration;
using CMS.Forms;
using CMS.CMS.OutputModels;
using CMS.Models;

namespace CMS.CMS.App
{
    public class CMS_App
    {
        /// <summary>
        /// CMS_App_Articles Instance
        /// </summary>
        private CMS_App_Articles _articles;

        private CMS_App_Products _products;

        /// <summary>
        /// CMS_App_Users Instance
        /// </summary>
        private CMS_App_Users _users;

        /// <summary>
        /// CMS_App_Roles Instance
        /// </summary>
        private CMS_App_Roles _roles;

        /// <summary>
        /// CMS_App_Categories Instance
        /// </summary>
        private CMS_App_Categories _categories;

        /// <summary>
        /// CMS_App_Resources Instance
        /// </summary>
        private CMS_App_Resources _resources;

        /// <summary>
        /// CMS_Acl Instance
        /// </summary>
        private CMS_Acl _acl;

        /// <summary>
        /// CMS_App_Comments Instance
        /// </summary>
        private CMS_App_Comments _comments;

        private CMS_App_Pages _pages;
        private CMS_App_News _news;

        /// <summary>
        /// Length of lists
        /// </summary>
        private int _listLength;

        /// <summary>
        /// Property representing length of lists
        /// </summary>
        public int ListLength
        {
            get
            {
                return _listLength;
            }
            set
            {
                _listLength = value;
            }
        }

        /// <summary>
        /// Is unregistered user 
        /// </summary>
        private bool _unregComm;

        /// <summary>
        /// Property representing boolean value if an unregistered user user
        /// is able to post comments
        /// </summary>
        public bool UnregistredComments
        {
            get
            {
                return _unregComm;
            }
            set
            {
                _unregComm = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CMS_App()
        {
            //set class members
            _articles = new CMS_App_Articles(this);
            _users = new CMS_App_Users(this);
            _roles = new CMS_App_Roles(this);
            _categories = new CMS_App_Categories(this);
            _resources = new CMS_App_Resources(this);
            _acl = new CMS_Acl();
            _comments = new CMS_App_Comments(this);
            _pages = new CMS_App_Pages(this);
            _news = new CMS_App_News(this);
            _products = new CMS_App_Products(this);

            using (SettingsDataContext s = new SettingsDataContext())
            {
                _listLength = int.Parse(s.settings.Where(x => x.name == "list_length").Single().value);
                _unregComm = bool.Parse(s.settings.Where(x => x.name == "unregistered_comments").Single().value);
            }
        }

        /// <summary>
        /// Determines if logged user is allowed to access this resource level
        /// </summary>
        /// <param name="level">Resource level</param>
        /// <returns>bool</returns>
        public bool checkLevel(long level)
        {
            CMS_Acl fakeAcl = (CMS_Acl)_acl.Clone();

            CMS_Resource fakeResource = new CMS_Resource("fakeArticleLevelResource");
            fakeAcl.addResource(fakeResource);
            fakeAcl.allow(this.roles().getById(level).name, fakeResource.getName());
            try
            {
                this.checkACL(fakeResource.getName(),fakeAcl);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get a list of levels allowed for the current user
        /// </summary>
        /// <returns>List of levels</returns>
        public List<long> allowedLevels()
        {
            List<long> allowed = new List<long>();
            List<long> levels = this.articles().getAllLevels();

            foreach (long level in levels)
            {
                if (this.checkLevel(level))
                {
                    allowed.Add(level);
                }
            }

            return allowed;
        }

        /// <summary>
        /// Checks if current user has privilegies to access the given resource
        /// </summary>
        /// <param name="resource">Resource name</param>
        public void checkACL(string resource, CMS_Acl acl)
        {
            this.checkACL(resource, acl, true);
        }

        /// <summary>
        /// Checks if current user has privilegies to access the given resource
        /// </summary>
        /// <param name="resource">Resource name</param>
        /// <param name="acl">ACL</param>
        /// <param name="assign">Assig roles and resources to ACL</param>
        public void checkACL(string resource, CMS_Acl acl, bool assign)
        {
            CMS_Login login = new CMS_Login();

            if (assign)
            {

                using (ACLDataContext DataContext = new ACLDataContext())
                {
                    var roles = DataContext.roles
                        .OrderBy(x=>x.parentid)
                        .Select(x=>new{RoleName = x.name, RoleID = x.id,RoleParentId = x.parentid, RoleParentName = x.role1.name}).ToList();
                    ////var roles = from r in DataContext.roles
                    ////            join r2 in DataContext.roles on r.parentid equals r2.id into joined
                    ////            from a in joined.DefaultIfEmpty()
                    ////            orderby r.parentid
                    ////            select new { RoleName = r.name, RoleID = r.id, RoleParentId = r.parentid, RoleParentName = a.role1.name };

                    Dictionary<long?, CMS_Role> parentals = new Dictionary<long?, CMS_Role>();

                    foreach (var a in roles)
                    {
                        if (a.RoleParentId != null && parentals.ContainsKey(a.RoleParentId))
                        {
                            CMS_Role r = new CMS_Role(a.RoleName, parentals[a.RoleParentId]);
                            acl.addRole(r);
                            parentals.Add(a.RoleID, r);
                        }
                        else
                        {
                            CMS_Role r = new CMS_Role(a.RoleName);
                            acl.addRole(r);
                            parentals.Add(a.RoleID, r);
                        }
                    }

                    var resources = from res in DataContext.resources
                                    select new { ResourceName = res.name, Action = res.action, Controller = res.controller };

                    foreach (var a in resources)
                    {
                        acl.addResource(new CMS_Resource(a.Controller + ":" + a.Action));
                    }

                    var rules = from r in DataContext.roles
                                join cr in DataContext.role_resources on r.id equals cr.rolesid
                                join res in DataContext.resources on cr.resourcesid equals res.id
                                orderby r.id
                                select new { Role = r.name, Controller = res.controller, Action = res.action };

                    if (rules.Count() > 0)
                    {
                        foreach (var a in rules)
                        {
                            acl.allow(a.Role, a.Controller + ":" + a.Action);
                        }
                    }
                }
            }

            user user;
            string role;
            if (login.hasIdentity())
            {
                user = login.getIdentity();
                role = this.roles().getById(user.rolesid).name;
            }
            else
            {
                user = null;
                role = "guest";
            }

            if (!acl.isAllowed(role, resource))
            {
                if (!login.hasIdentity())
                {
                    throw new Exception("You are not logged in! Log in and try again.");
                }
                else
                {
                    //trigger error
                    throw new Exception("You are not allowed to view this datasource!"); //TODO
                }
            }

        }

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns></returns>
        public CMS_App_Articles articles()
        {
            return this._articles;
        }

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns></returns>
        public CMS_App_Users users()
        {
            return this._users;
        }

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns></returns>
        public CMS_App_Roles roles()
        {
            return this._roles;
        }

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns></returns>
        public CMS_App_Categories categories()
        {
            return this._categories;
        }

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns></returns>
        public CMS_App_Products products()
        {
            return this._products;
        }

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns></returns>
        public CMS_App_Resources resources()
        {
            return this._resources;
        }

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns></returns>
        public CMS_App_Comments comments()
        {
            return this._comments;
        }

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns></returns>
        public CMS_App_Pages pages()
        {
            return this._pages;
        }

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns></returns>
        public CMS_App_News news()
        {
            return this._news;
        }

        /// <summary>
        /// remove diacritics and whitespace
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>name without diacritics</returns>
        public string makeAlias(string name)
        {
            if (name == null) return String.Empty;

            name = name.ToLower();
            string normalized = name.Normalize(NormalizationForm.FormD);
            int length = normalized.Length;

            StringBuilder b = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(normalized[i]);


                if (uc != UnicodeCategory.LowercaseLetter)
                {
                    b.Append("-");
                }
                else if (uc != UnicodeCategory.NonSpacingMark)
                {
                    b.Append(normalized[i]);
                }
            }
            return (b.ToString().Normalize(NormalizationForm.FormC)).Replace("--", "-");
        }

        /// <summary>
        /// Constructs DateTime based on string
        /// </summary>
        /// <param name="str">date in Y-m-d H:i:s</param>
        /// <returns>DateTime object</returns>
        public DateTime dateFromString(string str)
        {
            string[] arr = str.Split(' ');
            string[] date = arr[0].Split('-');
            string[] time = arr[1].Split(':');

            DateTime d = new DateTime(
                    Convert.ToInt32(date[0]),
                    Convert.ToInt32(date[1]),
                    Convert.ToInt32(date[2]),
                    Convert.ToInt32(time[0]),
                    Convert.ToInt32(time[1]),
                    Convert.ToInt32(time[2])
               );

            return d;
        }

        /// <summary>
        /// DateTime to Y-m-d H:i:s
        /// </summary>
        /// <param name="d">DateTime object</param>
        /// <returns>date string</returns>
        public string dateToString(DateTime d)
        {
            return d.Year.ToString() + "-" + d.Month.ToString() + "-" + d.Day.ToString() +
                " " + d.Hour.ToString() + ":" + d.Minute.ToString() + ":" + d.Second.ToString();
        }

        /// <summary>
        /// Formats date from d.m.Y H:i:s to Y-m-d H:i:s
        /// </summary>
        /// <param name="str">date</param>
        /// <returns>date</returns>
        public static string reFormatDate(string str)
        {
            RegexStringValidator r = new RegexStringValidator("[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2}");

            try
            {
                r.Validate(str);
            }catch
            {

                string[] vals = str.Split(' ');

                string[] date = vals[0].Split('.');
                string[] time = vals[1].Split(':');

                string month = (date[1].Length == 2 ? date[1] : "0" + date[1]);
                string day = (date[0].Length == 2 ? date[0] : "0" + date[0]);

                return date[2] + "-" + month + "-" + date[0] + " " + vals[1];
            
            }

            return str;
        }

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns></returns>
        public CMS_Acl getAcl()
        {
            return _acl;
        }

        /// <summary>
        /// Make RSS
        /// </summary>
        /// <param name="list">List of articles</param>
        /// <returns>XML markup</returns>
        public string makeRss(List<article> list)
        {
            XmlDocument doc = new XmlDocument();

            XmlNode rss = doc.CreateElement("rss");
            doc.AppendChild(rss);

            XmlAttribute version = doc.CreateAttribute("version");
            version.Value = "2.0";

            rss.Attributes.Append(version);

            XmlNode channel = doc.CreateElement("channel");
            rss.AppendChild(channel);

            XmlNode channelTitle = doc.CreateTextNode("title");
            channelTitle.Value = "CMS RSS Feed (latest public articles)";
            channel.AppendChild(channelTitle);

            XmlNode channelDate = doc.CreateTextNode("lastBuildDate");
            channelDate.Value = DateTime.Now.ToLongTimeString();
            channel.AppendChild(channelDate);

            foreach (article a in list)
            {
                XmlNode item = doc.CreateElement("item");
                channel.AppendChild(item);

                XmlNode title = doc.CreateElement("title");
                title.AppendChild(doc.CreateCDataSection(a.title));
                item.AppendChild(title);

                XmlNode link = doc.CreateElement("link");
                link.AppendChild(doc.CreateTextNode("http://" + "/article/" + a.id.ToString()));
                item.AppendChild(link);

                XmlNode description = doc.CreateElement("description");
                description.AppendChild(doc.CreateCDataSection(a.introtext));
                item.AppendChild(description);

                XmlNode author = doc.CreateElement("author");
                author.AppendChild(doc.CreateTextNode(this.articles().getAuthorsStringById(a.id)));
                item.AppendChild(author);
            }

            return doc.OuterXml;            
        }

        /// <summary>
        /// Saves settings
        /// </summary>
        /// <param name="form">Form instance</param>
        /// <returns>boolean</returns>
        public bool saveSettings(Form_Settings form)
        {
            using (SettingsDataContext s = new SettingsDataContext())
            {
                try
                {
                    setting len = s.settings.Where(x => x.name == "list_length").Single();
                    len.value = form["list_length"].getValue();

                    setting comm = s.settings.Where(x => x.name == "unregistered_comments").Single();
                    comm.value = form["unregistered_comments"].getValue();

                    s.SubmitChanges();
                }
                catch (Exception){
                    return false;
                }

                return true;
            }
        }

        public Object MultiUpload(HttpRequestBase Request)
        {
            LangDataContext dc = new LangDataContext();

            docgroup dg = new docgroup();

            dc.docgroups.InsertOnSubmit(dg);

            dc.SubmitChanges();

            List<Object> output = new List<Object>();

            foreach (String key in Request.Files)
            {
                output.Add(new { lang = key, data = uploadFile(Request.Files[key], Request, dg.id, Request.Form["title-"+key], key) });
            }

            return new {id = dg.id, docs = output.ToArray()};
        }

        public Object saveUploadedFile(HttpRequestBase Request) {

            return uploadFile(Request.Files[0], Request, null, Request.Files[0].FileName, "cz");

        }

        private Object uploadFile(HttpPostedFileBase small, HttpRequestBase Request, long? docgroupId, String name, String culture)
        {
            var postfix = DateTime.Now.ToString().GetHashCode().ToString("x") + "_" + small.FileName;
            var path = Path.Combine(Request.MapPath("./../files"), postfix);

            small.SaveAs(path);

            LangDataContext l = new LangDataContext();

            switch (Request.Params["type"]) {
                case "doc":

                    doc d = new doc();
                    d.path = postfix;

                    if (name != null)
                    {
                        d.text = new text();

                        texts_value val = new texts_value();
                        val.culture = culture;
                        val.value = name;
                        val.text = d.text;                    
                    }

                    l.docs.InsertOnSubmit(d);
                    l.SubmitChanges();

                    d.docgroup_id = docgroupId; //can be null

                    return new { id = d.id, path = postfix, name = small.FileName };
                case "img":

                    image i = new image();
                    i.path = postfix;

                    System.Drawing.Image img = System.Drawing.Image.FromFile(path);

                    i.height = img.Height;
                    i.width = img.Width;

                    l.images.InsertOnSubmit(i);
                    l.SubmitChanges();

                    return new { id = i.id, path = postfix, name = small.FileName };
                default:
                    return new { id = 0, path = "", name = small.FileName };
            }
        }

        public LangOutputModel fillLangModel(System.Data.Linq.EntitySet<Models.texts_value> val)
        {
            LangOutputModel lmo = new LangOutputModel();

            foreach (string lang in Helpers.LangHelper.langs)
            {
                texts_value entity = val.SingleOrDefault(x => x.culture == lang);
                lmo.setByCulture(lang, (entity == null ? String.Empty : entity.value));
            }

            return lmo;
        }
    }

    
}
