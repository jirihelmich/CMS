<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CMS.Models.role>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Backend: List roles
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Backend: List roles</h2>
    <table>
        <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Create date</th>
            <th>Add inherited</th>
            <th>List inherited</th>
            <th>Edit</th>
            <th>Delete</th>
        </tr>
        <% if (Model.Count() == 0) { %>
            <tr><td colspan="7">Sorry, no roles found.</td></tr>
        <% } %>
        <% foreach (CMS.Models.role r in Model) { %>
            <tr>
                <td><%= r.id %></td>
                <td><%= r.name %></td>
                <td><%= r.date %></td>
                <td><a href="/backend/addRole?parent=<%= r.id %>">Add inherited</a></td>
                <td><a href="/backend/listRoles?parent=<%= r.id %>">List inherited</a></td>
                <td><a href="/backend/editRole?id=<%= r.id %>">Edit</a></td>
                <td><a href="/backend/deleteRole?id=<%= r.id %>">Delete</a></td>
            </tr>
        <%} %>
        <% if (ViewData.Keys.Contains("current") && long.Parse(ViewData["current"].ToString()) > 0)
           {%>
            <tr>
                <td colspan="7">
                    <a href="/backend/listRoles?parent=<%= ViewData["parent"] %>">Level up</a>
                </td>
            </tr>
        <% } %>
        
            <tr>
                <td colspan="7">
                    <a href="/backend/addRole?parent=<%= ViewData["current"] %>">Add sibling</a>
                </td>
            </tr>
    </table>
    <div id="pagination">
        Pages: 
        <%
            int rolesCount = (int) ViewData["rolesCount"];
            int perPage = (int)ViewData["listLength"];
            double pages = rolesCount / perPage;
            
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
