using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.OutputModels
{
    public class CategoriesProductsOutputModel
    {
        public List<CategoryOutputModel> categories;
        public List<ProductOutputModel> products;

        public CategoriesProductsOutputModel(List<CategoryOutputModel> categories, List<ProductOutputModel> products)
        {
            this.categories = categories;
            this.products = products;
        }
    }
}