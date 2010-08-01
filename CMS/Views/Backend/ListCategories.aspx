<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<List<CMS.CMS.OutputModels.CategoryOutputModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Backend: List categories
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Výpis kategorií</h2>
    <div id="categories-list">
    
    <% foreach (CMS.CMS.OutputModels.CategoryOutputModel c in Model){ %>

      <div class="category" id="<%= c.Id %>">
      
        <span class="move">Pøesunout</span>
        <span class="name"><%= c.Title.cz %></span>

        <div class="controls">
          <a href="<%= Url.Action("editCategory","Backend") %>?id=<%= c.Id %>" class="edit">Upravit</a>
          <span class="edit">Smazat</span>
        </div>

      </div>

    <% } %>
    
    </div>
</asp:Content>
