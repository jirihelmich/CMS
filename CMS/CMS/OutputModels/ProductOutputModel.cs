using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Models;

namespace CMS.CMS.OutputModels
{
    public class ProductOutputModel
    {

        public LangOutputModel Title { get; set; }
        public LangOutputModel Subtitle { get; set; }
        public LangOutputModel Shortdesc { get; set; }
        public LangOutputModel Content { get; set; }

        public List<ProductOutputModel> ConnectedProducts { get; set; }
        public List<CategoryOutputModel> Categories { get; set; }

        public long Id { get; set; }

        public ImageOutputModel mainImage { get; set; }
        public List<DocumentGroupOutputModel> documents { get; set; }

        public ProductOutputModel(product p)
        {
            Id = p.id;
            Title = new LangOutputModel(p.text_title);
            //SEO
            Subtitle = new LangOutputModel(p.text_subtitle);
            Shortdesc = new LangOutputModel(p.text_description);
            Content = new LangOutputModel(p.text_text);

            ConnectedProducts = new List<ProductOutputModel>();

            foreach (product conn in p.products_connections.Select(x => (x.product == p) ? x.product1 : x.product))
            {
                ConnectedProducts.Add(new ProductOutputModel(conn));
            }

            Categories = p.products_categories.Select(x => new CategoryOutputModel(x.category)).ToList<CategoryOutputModel>();
            if (p.image != null)
            {
                mainImage = new ImageOutputModel(p.image);
            }

            documents = new List<DocumentGroupOutputModel>();

            foreach (docgroup dg in p.docgroups_products.Select(x => x.docgroup))
            {
                documents.Add(new DocumentGroupOutputModel(dg));
            }
        }

    }
}