<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<CMS.CMS.OutputModels.CategoryOutputModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Upravit kategorii
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
  <h3 id="edit-category" rel="<%= Model.Id %>">Upravit kategorii</h3>

  <div id="controls"><div id="save">Uložit</div><div id="cancel">Storno</div></div>

  <div id="edit-category-container">

        <div class="left box-width-70" id="tabs">
          <ul>
            <% foreach (String lang in CMS.Helpers.LangHelper.langs)
               { %>
            <li><a href="#<%= lang %>">
              <img src="../../Content/flags/<%= lang %>.png" /></a></li>
            <% } %>
          </ul>

          <% foreach (String lang in CMS.Helpers.LangHelper.langs)
             { %>
            <div id="<%= lang %>">
                <input id="title-<%= lang %>" type="text" class="largeField" value="<%= Model.Title.getByCulture(lang) %>" />
                <textarea id="description-<%= lang %>" class="ckeditor"><%= Model.Content.getByCulture(lang) %></textarea>
            </div>

          <% } %>
              </div>        
        <div class="box left box-width-30 highlight" id="tree">
          <div class="box-content">
            
          </div>
        </div>
        <hr class="clear" />

  </div>
</asp:Content>
