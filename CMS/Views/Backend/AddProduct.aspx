<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<List<CMS.CMS.OutputModels.CategoryOutputModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Přidat produkt
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h3 id="add-product">Přidat produkt</h3>
    <div id="controls">
        <div id="save">Uložit</div>
        <div id="cancel">Storno</div>
    </div>
    <div id="add-product-container">
        <div class="left box-width-70">
            <ul>
                <% foreach (String lang in CMS.Helpers.LangHelper.langs)
                   { %>
                        <li><a href="#<%= lang %>">&nbsp;<img src="../../Content/flags/<%= lang %>.png" />&nbsp;</a></li>
                <% } %>
                <li><a href="#documents">Dokumenty</a></li>
                <li><a href="#connections">Připojené produkty</a></li>
            </ul>
            <% foreach (String lang in CMS.Helpers.LangHelper.langs)
               { %>
            <div id="<%= lang %>">
                Hlavní titulek:
                <input id="title-<%= lang %>" type="text" class="largeField" />
                <br />
                Podtitulek:
                <input id="subtitle-<%= lang %>" type="text" class="largeField" />
                <br />
                Krátký popis:
                <textarea id="shortdesc-<%= lang %>" class="ckeditor"></textarea>
                <br />
                Text:
                <textarea id="text-<%= lang %>" class="ckeditor"></textarea>
                <hr class="clear" />
            </div>
            <% } %>
            <div id="documents">
                Dokumenty:
                <table id="docgroups">
                    <tr>
                        <th>
                            <img src="../../Content/flags/cz.png" />
                        </th>
                        <th>
                            <img src="../../Content/flags/gb.png" />
                        </th>
                        <th>
                            <img src="../../Content/flags/de.png" />
                        </th>
                        <th>
                            <img src="../../Content/flags/ru.png" />
                        </th>
                        <th>
                            <img src="../../Content/flags/fr.png" />
                        </th>
                        <th>
                            <img src="../../Content/flags/pl.png" />
                        </th>
                        <th>
                            [X]
                        </th>
                    </tr>
                </table>
                <a href="#" id="add-docs">Přidat dokument</a>
                <div id="docs_upload" title="Nahrát skupinu dokumentů">
                    <form id="ajaxUploadFormDoc" action="<%= Url.Action("Multiupload", "Backend")%>"
                    method="post" enctype="multipart/form-data">
                    <input type="hidden" name="type" value="doc" />
                    <% foreach (String lang in CMS.Helpers.LangHelper.langs)
                       { %>
                    <div id="doc-<%= lang %>">
                        <label for="document-<%= lang %>">
                            <img src="../../Content/flags/<%= lang %>.png" /></label>
                        <label for="input-title-<%= lang %>">
                        </label>
                        <input type="text" name="title-<%= lang %>" id="input-title-<%= lang %>" />
                        <input type="file" name="<%= lang %>" id="document-<%= lang %>" />
                    </div>
                    <% } %>
                    </form>
                </div>
            </div>
            <div id="connections">
                <div id="searchbox">
                    <input type="text" id="autocomplete" value="Zadejte název produktu nebo jeho část" />
                </div>
                <div id="results">
                    Žádné produkty.
                </div>
                <div id="connected">
                    Žádné produkty.
                </div>
            </div>
        </div>
        <div class="box left box-width-30 highlight" id="files">
            <div class="box-content">
                <strong>Vyberte kategorie:</strong>
                <div id="categories-select">
                    <%= CMS.Helpers.CategoryTree.PrintTree(Model, null, this, false, (int)CMS.Helpers.CategoryTree.SELECTMODE.CHECKBOXES) %>
                </div>
                <strong>Obrázek produktu</strong>
                <div id="product-image">
                </div>
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
        <span class="clear">&nbsp;</span>
    </div>
</asp:Content>
