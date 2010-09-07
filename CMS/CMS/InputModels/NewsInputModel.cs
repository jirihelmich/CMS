using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.InputModels
{
    [Serializable]
    public class NewsInputModel
    {
        [Serializable]
        public class News
        {
            [Serializable]
            public class NewsData
            {
                public String title { get; set; }
                public String shortdesc { get; set; }
                public String content { get; set; }
            }

            public long? Id { get; set; }
            public String lang { get; set; }
            public NewsData data { get; set; }
        }

        public News[] request { get; set; }

    }
}