using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.OutputModels
{
    public class ImageOutputModel
    {
        public long Id { get; set; }
        public String path { get; set; }

        public ImageOutputModel(Models.image image)
        {
            Id = image.id;
            path = image.path;
        }
    }
}