<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CMS.Models.resource>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Backend: List resources
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Backend: List resources</h2>
    <a href="/backend/AddResource">Add new resource</a>
    <table>
        <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Controller</th>
            <th>Action</th>
            <th>Edit</th>
            <th>Delete</th>
        </tr>
        <% foreach (CMS.Models.resource res in Model){ %>
            <tr>
                <td><%= res.id %></td>
                <td><%= res.name %></td>
                <td><%= res.controller %></td>
                <td><%= res.action %></td>
                <td><a href="/backend/EditResource?id=<%= res.id %>">Edit</a></td>
                <td><a href="/backend/DeleteResource?id=<%= res.id %>">Delete</a></td>
            </tr>
        <% } %>
    </table>
    <div id="pagination">
        Pages: 
        <%
            int resourcesCount = (int)ViewData["resourcesCount"];
            int perPage = (int)ViewData["listLength"];
            double pages = resourcesCount / perPage;
            
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
