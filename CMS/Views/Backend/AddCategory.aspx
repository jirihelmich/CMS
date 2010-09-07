<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<List<CMS.CMS.OutputModels.CategoryOutputModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Pøidat kategorii
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
  <h3 id="add-category">Pøidat kategorii</h3>

  <div id="controls"><div id="save">Uložit</div><div id="cancel">Storno</div></div>

  <div id="add-category-container">

        <div class="left box-width-70" id="tabs">
          <ul>
            <% foreach (String lang in CMS.Helpers.LangHelper.langs) { %>
            <li><a href="#<%= lang %>">
              <img src="../../Content/flags/<%= lang %>.png" /></a></li>
            <% } %>
          </ul>

          <% foreach (String lang in CMS.Helpers.LangHelper.langs) { %>
            <div id="<%= lang %>">
                <input id="title-<%= lang %>" type="text" class="largeField" />
                <textarea id="description-<%= lang %>" class="ckeditor"></textarea>
            </div>

          <% } %>
              </div>        
        <div class="box left box-width-30 highlight" id="tree">
          <div class="box-content">
            <strong>Vyberte rodièovskou kategorii <a href="#" class="no-radio">Odznaèit</a></strong>:
            <%= CMS.Helpers.CategoryTree.PrintTree(Model, null, this, false, (int)CMS.Helpers.CategoryTree.SELECTMODE.RADIOS) %>
          </div>
        </div>
        <hr class="clear" />

  </div>
</asp:Content>
