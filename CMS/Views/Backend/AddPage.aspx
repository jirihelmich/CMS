<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Přidat statickou stránku
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
  <h3 id="add-static-page">Přidat statickou stránku</h3>

  <div id="controls"><div id="save">Uložit</div><div id="cancel">Storno</div></div>

    <% String[] langs = new String[] { "cz", "gb", "de", "ru" }; %>
  <div id="add-page-container">

    <ul>
      <% foreach (String lang in langs) { %>
      <li><a href="#<%= lang %>">
        <img src="../../Content/flags/<%= lang %>.png" /></a></li>
      <% } %>
    </ul>

    <% foreach (String lang in langs) { %>
      <div id="<%= lang %>">
        <div class="left box-width-70">
          <input id="title-<%= lang %>" type="text" class="largeField" />
          <br />
          <textarea id="text-<%= lang %>" class="ckeditor"></textarea>
        </div>
        <div class="box left box-width-30 highlight" id="files-<%= lang %>">
          <div class="box-content">
            <form id="ajaxUploadFormDoc-<%= lang %>" action="<%= Url.Action("Upload", "Backend")%>" method="post" enctype="multipart/form-data">
            <fieldset>
              <legend>Přidat dokument:</legend>
              <input type="hidden" name="type" value="doc" />
              <label><input type="file" name="file" id="document-<%= lang %>" /></label>
            </fieldset>
            </form>
            <div class="uploaded docs"></div>
            <hr />
            <form id="ajaxUploadFormImg-<%= lang %>" action="<%= Url.Action("Upload", "Backend")%>" method="post" enctype="multipart/form-data">
            <fieldset>
              <legend>Přidat obrázek:</legend>
              <label><input type="file" name="file" id="image-<%= lang %>" /></label>
              <input type="hidden" name="type" value="img" />
            </fieldset>
            </form>
            <div class="uploaded img"></div>
          </div>
        </div>
        <hr class="clear" />
      </div>

    <% } %>

    <div id="title-change" title="Zadejte název dokumentu/obrázku">
      <input type="text" class="largeField" />
    </div>

  </div>
</asp:Content>
