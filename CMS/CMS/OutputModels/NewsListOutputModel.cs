using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.OutputModels
{
    public class NewsListOutputModel
    {
        public long Id { get; set; }
        public LangOutputModel Title { get; set; }
        public LangOutputModel Shortdesc { get; set; }
        public LangOutputModel Content { get; set; }

        //public List<DocumentOutputModel> documents { get; set; }
        //public List<DocumentOutputModel> images { get; set; }
    }
}