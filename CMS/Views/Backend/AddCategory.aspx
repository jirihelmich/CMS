<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Backend: Add a new category
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Backend: Add a new category</h2>
    <p>You are about creating a new subcategory for the category <%= ViewData["name"] %>:</p>
    <%= ViewData["form"] %>

</asp:Content>
