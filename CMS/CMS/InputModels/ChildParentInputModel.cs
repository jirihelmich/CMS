using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.InputModels
{
    [Serializable]
    public class ChildParentInputModel
    {
        public long childId { get; set; }
        public long parentId { get; set; }
    }
}