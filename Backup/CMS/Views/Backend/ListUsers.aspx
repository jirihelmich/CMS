<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CMS.Models.user>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Backend: List users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Backend: List users</h2>
    <table>
        <tr>
            <th>ID</th>
            <th>Username</th>
            <th>E-mail</th>
            <th>Create date</th>
            <th>Promote</th>
            <th>Demote</th>
            <th>Edit auhor</th>
            <th>Edit</th>
            <th>Delete</th>
        </tr>
        <% if (Model.Count() == 0) { %>
            <tr><td colspan="9">Sorry, no users found.</td></tr>
        <% } %>
        <% foreach (CMS.Models.user u in Model) {
               
               bool author = ((List<long>)ViewData["authors"]).Contains(u.id);
               
               %>
            <tr>
                <td><%= u.id %></td>
                <td><%= u.username %></td>
                <td><%= u.email %></td>
                <td><%= u.date %></td>
                <td><% if (!author){ %><a href="/backend/promoteUser?id=<%= u.id %>">Promote to author</a><% } %></td>
                <td><% if (author){ %><a href="/backend/demoteUser?id=<%= u.id %>">Demote to user</a><% } %></td>
                <td><% if (author){ %><a href="/backend/editAuthor?uid=<%= u.id %>">Edit author data</a><% } %></td>
                <td><a href="/backend/editUser?id=<%= u.id %>">Edit</a></td>
                <td><a href="/backend/deleteUser?id=<%= u.id %>">Delete</a></td>
            </tr>
        <%} %>        
        <tr>
            <td colspan="9">
                <a href="/backend/AddUser">Add User</a>
            </td>
        </tr>
    </table>
    <div id="pagination">
        Pages: 
        <%
            int usersCount = (int) ViewData["usersCount"];
            int perPage = (int)ViewData["listLength"];
            double pages = usersCount / perPage;
            
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
