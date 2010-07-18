<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CMS.Models.article>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Backend: List articles 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Backend: List articles</h2>
    
    <table>
    <tr>
        <th>ID</th>
        <th>Title</th>
        <th>Publish date</th>
        <th>Pullback date</th>
        <th>Hits</th>
        <th>Published</th>
        <th>Edit</th>
        <th>Delete</th>
    </tr>
    <% foreach (CMS.Models.article a in Model) { %>
        <tr>
            <td><%= a.id.ToString() %></td>
            <td><%= a.title %></td>
            <td><%= a.date_published %></td>
            <td><%= a.date_pullback %></td>
            <td><%= a.hits %></td>
            <td><%= a.published %></td>
            <td><a href="/backend/editArticle?id=<%= a.id %>">Edit</a></td>
            <td><a href="/backend/deleteArticle?id=<%= a.id %>">Delete</a></td>
        </tr>
    <% } %>
    </table>
<div id="pagination">
        Pages: 
        <%
            int articlesCount = (int) ViewData["articlesCount"];
            int perPage = (int)ViewData["listLength"];
            double pages = articlesCount / perPage;
            
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
</asp:Content>
