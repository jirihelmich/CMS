using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Models;

namespace CMS.CMS.OutputModels
{
    public class CategorizedProductOutputModel
    {         
        public List<CategorizedProductOutputModel> Subcategories { get; set; }

        public CategoryOutputModel Category { get; set; }

        public List<ProductOutputModel> Products { get; set; }

        public CategorizedProductOutputModel(category root)
        {
            Category = new CategoryOutputModel(root);

            Subcategories = new List<CategorizedProductOutputModel>();

            foreach (category c in root.categories.Where(x => x.category1 == root))
            {
                Subcategories.Add(new CategorizedProductOutputModel(c));
            }

            Products = new List<ProductOutputModel>();

            foreach (product p in root.products_categories.Select(x => x.product))
            {
                Products.Add(new ProductOutputModel(p));
            }
        }

    }
}