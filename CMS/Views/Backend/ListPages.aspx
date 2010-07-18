﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CMS.CMS.OutputModels.PageListOutputModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Administrace: Výpis stránek
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Administrace: Výpis stránek</h2>
    
    <table>
    <tr>
        <th>ID</th>
        <th>Název <img src="../../Content/flags/cz.png" /></th>
        <th>Název <img src="../../Content/flags/gb.png" /></th>
        <th>Název <img src="../../Content/flags/de.png" /></th>
        <th>Název <img src="../../Content/flags/ru.png" /></th>
        <th>Upravit</th>
        <th>Smazat</th>
    </tr>
    <% foreach (CMS.CMS.OutputModels.PageListOutputModel p in Model) { %>
        <tr>
            <td><%= p.Id %></td>
            <td><%= p.Title.cz %></td>
            <td><%= p.Title.gb %></td>
            <td><%= p.Title.de %></td>
            <td><%= p.Title.ru %></td>
            <td><a href="/backend/editPage?id=<%= p.Id %>">Upravit</a></td>
            <td><a href="#" class="delete-page" rel="<%= p.Id %>">Smazat</a></td>
        </tr>
    <% } %>
    <% if (Model.Count() == 0) { %>
        <tr>
            <td colspan="4">Žádné statické stránky</td>
        </tr>
    <% } %>
    </table>
<div id="pagination">
        Stránky: 
        <%
            int pagesCount = (int) ViewData["pagesCount"];
            int perPage = (int)ViewData["listLength"];
            double pages = pagesCount / perPage;
            
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
