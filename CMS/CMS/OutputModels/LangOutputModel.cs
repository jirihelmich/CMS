using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CMS.OutputModels
{
    public class LangOutputModel
    {
        public String cz { get; set; }
        public String gb { get; set; }
        public String ru { get; set; }
        public String de { get; set; }

        public String getByCulture(String culture)
        {
            switch (culture)
            {
                case "cz":
                    return cz;
                case "de":
                    return de;
                case "gb":
                    return gb;
                case "ru":
                    return ru;
                default:
                    return String.Empty;
            }
        }
    }
}