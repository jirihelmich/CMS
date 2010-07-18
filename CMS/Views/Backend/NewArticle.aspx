<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Backend: Publish a new article
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Backend: Publish a new article</h2>
    <p>Please notice that article is splitted into chapters by heading of size 1.</p>
    
    <%= ViewData["form"] %>

</asp:Content>
