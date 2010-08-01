using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.OutputModels
{
    public class CategoryOutputModel
    {

        public long Id {get; set; }
        public LangOutputModel Title { get; set; }
        public LangOutputModel Content { get; set; }

        public long Level { get; set; }
        public CategoryOutputModel Parent { get; set; }

    }
}