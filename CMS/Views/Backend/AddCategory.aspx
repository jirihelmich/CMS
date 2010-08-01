<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<List<CMS.CMS.OutputModels.CategoryOutputModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Pøidat kategorii
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
  <h3 id="add-category">Pøidat kategorii</h3>

  <div id="controls"><div id="save">Uložit</div><div id="cancel">Storno</div></div>

    <% String[] langs = new String[] { "cz", "gb", "de", "ru" }; %>
  <div id="add-category-container">

        <div class="left box-width-70" id="tabs">
          <ul>
            <% foreach (String lang in langs) { %>
            <li><a href="#<%= lang %>">
              <img src="../../Content/flags/<%= lang %>.png" /></a></li>
            <% } %>
          </ul>

          <% foreach (String lang in langs) { %>
            <div id="<%= lang %>">
                <input id="title-<%= lang %>" type="text" class="largeField" />
                <textarea id="description-<%= lang %>" class="ckeditor"></textarea>
            </div>

          <% } %>
              </div>        
        <div class="box left box-width-30 highlight" id="tree">
          <div class="box-content">
            Vyberte rodièovskou kategorii <a href="#" class="no-radio">Odznaèit</a>:
            <% foreach (CMS.CMS.OutputModels.CategoryOutputModel c in Model) { %>
              <br /><input type="radio" name="parent" id="<%= c.Id %>" /><%= c.Title.cz %>
            <% } %>
          </div>
        </div>
        <hr class="clear" />

  </div>
</asp:Content>
