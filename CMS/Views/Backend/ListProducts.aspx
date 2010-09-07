<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<CMS.CMS.OutputModels.CategoriesProductsOutputModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Výpis produktů
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 id="products-list">Výpis produktů</h2>

   <% System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer(); %>
    
    <script>
      window.pageData = <%= ser.Serialize(Model.products) %>;
    </script>

<div class="box left box-width-30 highlight"> 
	<h3 class="box-name"><span><span>Kategorie</span></span></h3> 
	<div class="box-content"> 

        <%= CMS.Helpers.CategoryTree.PrintTree(Model.categories, null, this) %>
		
		<span class="left-corner"></span> 
		<span class="right-corner"></span> 
	</div> 
</div>

<div class="box left box-width-70"> 
	<h3 class="box-name"><span><span>Produkty</span></span></h3> 
	<div class="box-content"> 

    <div id="products-placeholder">
        Klikněte na název kategorie v levém menu pro výpis produktů v ní umístěných.
    </div>
		
		<span class="left-corner"></span> 
		<span class="right-corner"></span> 
	</div> 
</div>

</asp:Content>
