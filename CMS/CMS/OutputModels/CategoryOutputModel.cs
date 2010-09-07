using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Models;

namespace CMS.CMS.OutputModels
{
    public class CategoryOutputModel
    {

        public long Id {get; set; }
        public LangOutputModel Title { get; set; }
        public LangOutputModel Content { get; set; }

        public long Level { get; set; }
        public long? Parent { get; set; }

        public CategoryOutputModel()
        {
        
        }

        public CategoryOutputModel(category c)
        {
            this.Id = c.id;
            this.Title = new LangOutputModel(c.title);
            this.Content = new LangOutputModel(c.text);
            this.Parent = c.parentid;
        }

    }
}