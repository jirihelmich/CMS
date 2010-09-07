using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.CMS.OutputModels;

namespace CMS.Helpers
{
    public class CategoryTree
    {
        public enum SELECTMODE {
            NOTHING, RADIOS, CHECKBOXES
        };

        public static string PrintTree(List<CategoryOutputModel> tree, long? parentId, System.Web.Mvc.ViewPage page)
        {
            return PrintTree(tree, parentId, page, false, (int)SELECTMODE.NOTHING);
        }
        public static string PrintTree(List<CategoryOutputModel> tree, long? parentId, System.Web.Mvc.ViewPage page, bool editMode)
        {
            return PrintTree(tree, parentId, page, editMode, (int)SELECTMODE.NOTHING);
        }

        public static string PrintTree(List<CategoryOutputModel> tree, long? parentId, System.Web.Mvc.ViewPage page, bool editMode, int select)
        {
            string output = "<ul class=\"categories\">";
            foreach (CategoryOutputModel com in tree)
            {
                if (com.Parent == parentId)
                {
                    output += "<li class=\"category\" id=\""+com.Id+"\">";

                    if (select > 0)
                    {
                        if (select == (int)SELECTMODE.RADIOS)
                        {
                            output += "<input type=\"radio\" name=\"radio\" value=\"" + com.Id + "\" />";
                        }else
                            if (select == (int)SELECTMODE.CHECKBOXES)
                        {
                            output += "<input type=\"checkbox\" name=\"checkbox[" + com.Id + "]\" value=\"1\" />";
                        }
                    }

                    if (editMode) output += "<span class=\"move\">Přesunout</span>";

                    output += "<span class=\"name\">"+com.Title.cz+"</span>";

                    if (editMode) output += "<div class=\"controls\"><a href=\"" + page.Url.Action("editCategory", "Backend") + "?id=" + com.Id + "\" class=\"edit\">Upravit</a><span class=\"delete\">Smazat</span></div>";

                    output += PrintTree(tree, com.Id, page, editMode, select);

                    output += "</li>";
                }
            }
            output += "</ul>";
            return output;
        }
    }
}