using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.InputModels
{
    [Serializable]
    public class ProductInputModel
    {
        [Serializable]
        public class Product
        {
            [Serializable]
            public class ProductData
            {
                public String Title { get; set; }
                public String Subtitle { get; set; }
                public String Shortdesc { get; set; }
                public String Content { get; set; }
            }
            public String lang { get; set; }
            public ProductData data { get; set; }
        }

        public long[] Categories { get; set; }
        public long[] Connections { get; set; }
        public long? prodId { get; set; }

        public DocGroupInputModel[] docGroups { get; set; }

        public long mainImage { get; set; }

        public Product[] request { get; set; }

    }
}