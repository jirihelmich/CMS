<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<List<CMS.CMS.OutputModels.CategoryOutputModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Administrace: Výpis kategorií
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<ul>
    <li><a href="/Backend/addCategory">Vytvoøit kategorii</a></li>
</ul>

    <h2>Výpis kategorií</h2>
    <div id="categories-list">
    
    <%= CMS.Helpers.CategoryTree.PrintTree(Model, null, this, true) %>

    </div>
</asp:Content>
