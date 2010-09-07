using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.OutputModels
{
    public class DocumentGroupOutputModel
    {
        public List<DocumentOutputModel> documents = new List<DocumentOutputModel>();

        public DocumentGroupOutputModel(Models.docgroup dg)
        {
            foreach(Models.doc document in dg.docs)
            {
                documents.Add(new DocumentOutputModel(document));
            }
        }
    }
}