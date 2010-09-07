using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.CMS.OutputModels;
using CMS.Models;

namespace CMS.CMS.App
{
    public class CMS_App_Products
    {

        /// <summary>
        /// 
        /// </summary>
        protected CMS_App _app;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public CMS_App_Products(CMS_App app)
        {
            _app = app;
        }

        public List<ProductOutputModel> getAll()
        {
            using (LangDataContext dc = new LangDataContext())
            {
                var data = dc.products;

                List<ProductOutputModel> output = new List<ProductOutputModel>();

                foreach (product p in data)
                {
                    output.Add(new ProductOutputModel(p));
                }

                return output;
            }
        }


        public object add(InputModels.ProductInputModel p)
        {
            using (LangDataContext dc = new LangDataContext())
            {
                product prod = new product();
                prod.text_title = new text();
                prod.text_subtitle = new text();
                prod.text_description = new text();
                prod.text_text = new text();

                foreach (InputModels.ProductInputModel.Product inProd in p.request)
                {
                    texts_value titleval = new texts_value();
                    titleval.culture = inProd.lang;
                    titleval.value = inProd.data.Title;
                    titleval.text = prod.text_title;
                    titleval.seo_value = this._app.makeAlias(inProd.data.Title);

                    texts_value subtitleval = new texts_value();
                    subtitleval.culture = inProd.lang;
                    subtitleval.value = inProd.data.Subtitle;
                    subtitleval.text = prod.text_subtitle;

                    texts_value descval = new texts_value();
                    descval.culture = inProd.lang;
                    descval.value = inProd.data.Shortdesc;
                    descval.text = prod.text_description;

                    texts_value textval = new texts_value();
                    textval.culture = inProd.lang;
                    textval.value = inProd.data.Content;
                    textval.text = prod.text_text;
                }

                if (p.docGroups != null)
                {
                    foreach (InputModels.DocGroupInputModel dg in p.docGroups)
                    {
                        if (dg.Id == null) continue;

                        docgroups_product dgp = new docgroups_product();
                        dgp.docgroup_id = (long)dg.Id;
                        dgp.product = prod;
                    }
                }

                foreach (long cat in p.Categories)
                {
                    products_category pc = new products_category();
                    pc.product = prod;
                    pc.category_id = cat;
                }

                foreach (long conn in p.Connections)
                {
                    products_connection prodConn = new products_connection();
                    prodConn.product = prod;
                    prodConn.product_id_2 = conn;
                }

                prod.image_id = p.mainImage;

                dc.products.InsertOnSubmit(prod);

                dc.SubmitChanges();

                return new { Id = prod.id };
            }
        }

        public object delete(long id)
        {
            using (LangDataContext dc = new LangDataContext())
            {
                try
                {
                    var product = dc.products.Single(x => x.id == id);
                    dc.products.DeleteOnSubmit(product);

                    return new { result = true };
                }
                catch (Exception e)
                {
                    return new { result = false, errMsg = e.Message};
                }
            }
        }

        public object changeCategory(long oldCatId, long newCatId, long productId)
        {

            using (LangDataContext dc = new LangDataContext())
            {
                try
                {
                    var product = dc.products.Single(x => x.id == productId);

                    product
                        .products_categories
                        .Single(x => x.category_id == oldCatId && x.product_id == productId)
                        .category_id = newCatId;

                    return new { result = true };
                }
                catch (Exception e)
                {
                    return new { result = false, errMsg = e.Message };
                }
            }
        }

        public object addCategory(long catId, long productId)
        {
            using (LangDataContext dc = new LangDataContext())
            {
                try
                {
                    products_category rel = new products_category();
                    rel.category_id = catId;
                    rel.product_id = productId;

                    dc.products_categories.InsertOnSubmit(rel);

                    return new { result = true };
                }
                catch (Exception e)
                {
                    return new { result = false, errMsg = e.Message };
                }
            }
        }

        public ProductOutputModel getById(long id)
        {
            using (LangDataContext dc = new LangDataContext())
            {
                var product = dc.products.SingleOrDefault(x => x.id == id);
                return new ProductOutputModel(product);
            }
        }

        public List<ProductOutputModel> search(CMS.InputModels.StringInputModel s)
        {
            using (LangDataContext dc = new LangDataContext())
            {
                var products = dc.products.Where(u => dc.texts_values.Where(x => x.value.Contains(s.str)).Select(x=>x.text).Contains(u.text_title));

                List<ProductOutputModel> list = new List<ProductOutputModel>();
                
                foreach (product p in products)
                {
                    list.Add(new ProductOutputModel(p));
                }

                return list;
            }
        }


        public object edit(InputModels.ProductInputModel p)
        {
            try
            {
                using (LangDataContext dc = new LangDataContext())
                {

                    var prod = dc.products.Single(x => x.id == p.prodId);

                    foreach (string lang in Helpers.LangHelper.langs)
                    {
                        prod.text_title.texts_values.Single(x => x.culture == lang).value = p.request.Single(x => x.lang == lang).data.Title;
                        prod.text_subtitle.texts_values.Single(x => x.culture == lang).value = p.request.Single(x => x.lang == lang).data.Subtitle;
                        prod.text_description.texts_values.Single(x => x.culture == lang).value = p.request.Single(x => x.lang == lang).data.Shortdesc;
                        prod.text_text.texts_values.Single(x => x.culture == lang).value = p.request.Single(x => x.lang == lang).data.Content;
                    }

                    if (p.docGroups != null)
                    {
                        foreach (docgroups_product dgp in prod.docgroups_products.Where(x => p.docGroups.Where(u => u.Id == x.docgroup_id).Count() == 0))
                        {
                            prod.docgroups_products.Remove(dgp);
                        }

                        foreach (InputModels.DocGroupInputModel dg in p.docGroups)
                        {
                            if (dg.Id == null || prod.docgroups_products.Where(x => x.docgroup_id == dg.Id).Count() > 0) continue;

                            docgroups_product dgp = new docgroups_product();
                            dgp.docgroup_id = (long)dg.Id;
                            dgp.product = prod;
                        }
                    }

                    foreach (products_connection pdc in prod.products_connections.Where(x => !p.Connections.Contains(x.product_id)))
                    {
                        prod.products_connections.Remove(pdc);
                    }

                    foreach (long conn in p.Connections)
                    {
                        if (prod.products_connections.Select(x => x.product_id_2).Contains(conn)) continue;

                        products_connection prodConn = new products_connection();
                        prodConn.product = prod;
                        prodConn.product_id_2 = conn;
                    }

                    prod.image_id = p.mainImage;

                    dc.products.InsertOnSubmit(prod);

                    dc.SubmitChanges();

                    return new { result = true };
                }
            }
            catch (Exception e)
            {
                return new { result = false, errMsg = e.Message };
            }
        }
    }
}