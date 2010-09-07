using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.InputModels
{
    [Serializable]
    public class DocGroupInputModel
    {
        public long? Id { get; set; }
        public DocumentInputModel[] docs { get; set; }
    }
}