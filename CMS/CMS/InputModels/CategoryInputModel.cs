using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.InputModels
{
    [Serializable]
    public class CategoryInputModel
    {
        [Serializable]
        public class Category
        {
            [Serializable]
            public class CategoryData
            {
                public String Title { get; set; }
                public String Content { get; set; }
            }
            public String lang { get; set; }
            public CategoryData data { get; set; }
        }

        public long? Parent { get; set; }
        public long? Id { get; set; }
        
        public Category[] request { get; set; }

    }
}