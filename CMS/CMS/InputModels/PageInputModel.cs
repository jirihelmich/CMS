using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.InputModels
{
    [Serializable]
    public class PageInputModel
    {
        [Serializable]
        public class Page
        {
            [Serializable]
            public class PageData
            {
                public String title {get; set; }
                public String content {get; set; }
            }

            public String lang { get; set; }
            public PageData data { get; set; }
        }

        public Page[] request { get; set; }

    }
}