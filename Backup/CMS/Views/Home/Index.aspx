﻿<%@ Import Namespace="System.IO" %>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CMS.Models.article>>" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
    <h2>Latest articles</h2>
    
    <div id="ordering_form">
        
        <form action="/" method="get">
        
            <select name="ordering" id="ordering"> 
                <option value="id">Creation date</option> 
                <option value="date">Published date</option> 
                <option value="hits">Hits</option> 
                <option value="title">Title</option> 
            </select> 
            
            <input type="submit" value="Reorder" />
        
        </form>
        
    </div>
    
    <div id="articles">
    
    <% foreach (var a in Model){ %>
        <h3><a href="/article/detail?chapter=0&id=<%= a.id %>"><%= a.title %></a></h3>
        <%
        FileInfo f = new FileInfo(Request.MapPath("~/images/") + a.id + "_small.jpg");
        if (f.Exists)
        {
         %>
            <img src="/images/<%= a.id %>_small.jpg" alt="<%= a.title %>" style="float: left;" />
        <% } %>
        <p><%= a.introtext %></p>
        <div class="info">
            Published on: <%= a.date_published.ToString() %>, hits: <%= a.hits %>
        </div>
        <hr />
    <% } %>
    
    </div>
    
    <div id="pagination">
        Pages: 
        <%
            int articlesCount = (int) ViewData["articlesCount"];
            int perPage = 10;
            double pages = articlesCount/10;
            
            pages = Math.Ceiling(pages);

            string currentPage = Request.Params["page"];
            if (String.Empty == currentPage || null == currentPage) currentPage = "0";
            
            for (int i = 0; i < pages+1; ++i) {
                if ((i).ToString() == currentPage)
                {
                    %>
                        <span><%= i+1 %></span>
                    <%
                }
                else {
                    %>
                        <a href="?page=<%= i %>"><%= i+1 %></a>
                    <%
                }
            } %>
    </div>
    </form>
</asp:Content>
