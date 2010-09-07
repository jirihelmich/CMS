using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.InputModels
{
    [Serializable]
    public class DocumentInputModel
    {

        public long Id { get; set; }
        public String lang { get; set; }

    }
}