using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Models;

namespace CMS.CMS.OutputModels
{
    public class LangOutputModel
    {
        public String cz { get; set; }
        public String gb { get; set; }
        public String ru { get; set; }
        public String de { get; set; }
        public String fr { get; set; }
        public String pl { get; set; }

        public LangOutputModel() { }

        public LangOutputModel(text source)
        {
            foreach (String lang in Helpers.LangHelper.langs)
            {
                texts_value entity = source.texts_values.SingleOrDefault(x => x.culture == lang);

                this.setByCulture(
                    lang,
                    (entity == null ? String.Empty : entity.value)
                );
            }
        }

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
                case "fr":
                    return fr;
                case "pl":
                    return pl;
                default:
                    return String.Empty;
            }
        }

        public void setByCulture(String culture, String value)
        {
            switch (culture)
            {
                case "cz":
                    cz = value;
                    break;
                case "de":
                    de = value;
                    break;
                case "gb":
                    gb = value;
                    break;
                case "ru":
                    ru = value;
                    break;
                case "fr":
                    fr = value;
                    break;
                case "pl":
                    pl= value;
                    break;
                default:
                    break;
            } 
        }
    }
}