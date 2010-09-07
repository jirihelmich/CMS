using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.OutputModels
{
    public class DocumentOutputModel
    {
        public long Id { get; set; }
        public LangOutputModel Title { get; set; }
        public String Path { get; set; }

        public DocumentOutputModel(Models.doc document)
        {
            Id = document.id;
            Path = document.path;
            Title = new LangOutputModel(document.text);
        }

        public string Culture
        {
            get
            {
                return Helpers
                    .LangHelper
                    .langs
                    .Where(x => !String.IsNullOrEmpty(Title.getByCulture(x))).Single();
            }
        }
    }
}