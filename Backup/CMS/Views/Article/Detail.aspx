<%@ Import Namespace="System.IO" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <% CMS.Models.article a = (CMS.Models.article)ViewData["article"]; %>
	<%= a.title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% CMS.Models.article a = (CMS.Models.article)ViewData["article"]; %>
    <h2><%= a.title %></h2>
    
    <%
    FileInfo f = new FileInfo(Request.MapPath("~/images/") + a.id + "_big.jpg");
    if (f.Exists)
    {
     %>
        <img src="/images/<%= a.id %>_big.jpg" alt="<%= a.title %>" style="float: left;" />
    <% } %>
    
    <%= a.introtext %>
    <hr />
    <%
        
        string[] chapters = Regex.Split(a.fulltext, @"<h1[^>]*>[^<]*</h1>", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);

        int chapter = (int)ViewData["chapter"];

        if (chapter+1 >= chapters.Length) chapter = chapters.Length - 1;
        if (chapter + 1 < 0) chapter = 0;

        if (chapters.Length > 1)
        { 
            %>
            <h3>Chapters:</h3>
            <ol>
            <%
            MatchCollection matches = Regex.Matches(a.fulltext, "<h1[^>]*>([^<]*)</h1>");

            int i = 0;
            foreach (Match m in matches)
            {
                Response.Write("<li><a href=\"?id="+a.id+"&chapter="+i+"\">"+m.Groups[1].Value+"</a></li>");
                ++i;
            }
            
            %>
            </ol>
            <%
            Response.Write(chapters[chapter + 1]);
        }
        else
        {
            Response.Write(chapters[chapter]);
        }
        
    %>
    <hr />
    <p>Written by:</p>
    <%
        List<CMS.Models.author> authors = (List<CMS.Models.author>)ViewData["authors"];

        bool first = true;
        foreach (CMS.Models.author au in authors)
        {
            if (!first) Response.Write(", ");
            Response.Write("<a href=\"/author/List?id="+au.id+"\">"+au.lastname+" "+au.name+"</a>");
            first = false;
        }
    %>
    <hr />
    <p>Filed under:</p>
    <%
        List<CMS.Models.category> cats = (List<CMS.Models.category>)ViewData["categories"];

        first = true;
        foreach (CMS.Models.category c in cats)
        {
            if (!first) Response.Write(", ");
            Response.Write("<a href=\"/category/List?id="+c.id+"\">"+c.name+"</a>");
            first = false;
        }
    %>
    <hr />
    <p>Tags:</p>
    <%
        List<CMS.Models.tag> tags = (List<CMS.Models.tag>)ViewData["tags"];

        first = true;
        foreach (CMS.Models.tag t in tags)
        {
            if (!first) Response.Write(", ");
            Response.Write(t.name);
            first = false;
        }
    %>
    <hr />
    <p>Comments (<% if((bool)ViewData["unregisteredComments"]){ %><a href="/comment/add?aid=<%= a.id %>">Add new</a><% } %>):</p>
    <%
        List<CMS.Models.comment> comments = (List<CMS.Models.comment>)ViewData["comments"];

        Dictionary<long, List<CMS.Models.comment>> tree = new Dictionary<long, List<CMS.Models.comment>>();
        
        foreach (CMS.Models.comment c in comments)
        {
            long parent = 0;
            if (c.parentid.HasValue) parent = (long)c.parentid;
            
            if (!tree.ContainsKey(parent))
            {
                tree.Add(parent, new List<CMS.Models.comment>());
            }

            tree[parent].Add(c);
        }
        
        if (tree.ContainsKey(0))
        {
            Response.Write(CMS.Helpers.CommentHelper.Help(0, tree, (bool)ViewData["unregisteredComments"]));
        }
        else {
            %><p>No comments yet.</p><%
        }
    %>

</asp:Content>
