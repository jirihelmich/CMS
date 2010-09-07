<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<CMS.CMS.OutputModels.PageListOutputModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Upravit statickou stránku
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
  <h3 id="edit-static-page">Upravit statickou stránku</h3>

  <div id="controls"><div id="save">Uložit</div><div id="cancel">Storno</div></div>

  <div id="edit-page-container" rel="<%= Model.Id %>">
    
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
          <br />
          <textarea id="text-<%= lang %>" class="ckeditor"><%= Model.Content.getByCulture(lang) %></textarea>
      </div>

    <% } %>
        </div>
        <div class="box left box-width-30 highlight" id="files">
            <div class="box-content">
                <form id="ajaxUploadFormImg" action="<%= Url.Action("Upload", "Backend")%>" method="post"
                enctype="multipart/form-data">
                <fieldset>
                    <legend>Přidat obrázek:</legend>
                    <label>
                        <input type="file" name="file" id="image" /></label>
                    <input type="hidden" name="type" value="img" />
                </fieldset>
                </form>
                <div class="uploaded img">
                </div>
            </div>
        </div>

  </div>
</asp:Content>
