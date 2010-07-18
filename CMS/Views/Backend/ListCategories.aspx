<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CMS.Models.category>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Backend: List categories
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Backend: List categories</h2>
    <table>
        <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Create date</th>
            <th>Add Child</th>
            <th>List Categories</th>
            <th>Edit</th>
            <th>Delete</th>
        </tr>
        <% if (Model.Count() == 0) { %>
            <tr><td colspan="7">Sorry, no categories found.</td></tr>
        <% } %>
        <% foreach (CMS.Models.category c in Model) { %>
            <tr>
                <td><%= c.id %></td>
                <td><%= //c.name %></td>
                <td><%= c.date %></td>
                <td><a href="/backend/addCategory?parent=<%= c.id %>">Add child</a></td>
                <td><a href="/backend/listCategories?parent=<%= c.id %>">List children</a></td>
                <td><a href="/backend/editCategory?id=<%= c.id %>">Edit</a></td>
                <td><a href="/backend/deleteCategory?id=<%= c.id %>">Delete</a></td>
            </tr>
        <%} %>
        <% if (ViewData.Keys.Contains("current") && long.Parse(ViewData["current"].ToString()) > 0)
           {%>
            <tr>
                <td colspan="7">
                    <a href="/backend/listCategories?parent=<%= ViewData["parent"] %>">Level up</a>
                </td>
            </tr>
        <% } %>
        
            <tr>
                <td colspan="7">
                    <a href="/backend/AddCategory?parent=<%= ViewData["current"] %>">Add sibling</a>
                </td>
            </tr>
    </table>

    <div id="pagination">
        Pages: 
        <%
            int categoriesCount = (int)ViewData["categoriesCount"];
            int perPage = (int)ViewData["listLength"];
            double pages = categoriesCount / perPage;
            
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
