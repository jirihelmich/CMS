using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.InputModels
{
    [Serializable]
    public class ChangeProductCategoryModel
    {

        public long oldCatId { get; set; }
        public long newCatId { get; set; }
        public long productId { get; set; }

    }
}