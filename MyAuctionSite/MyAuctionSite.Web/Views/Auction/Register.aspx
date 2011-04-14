<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Register
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Register</h2>
	<%Html.BeginForm("Register", "Auction"); %>
        Description:
        <%= Html.TextBox("Description") %>
	    <input type="submit" value="submit" />
	<% Html.EndForm(); %>
</asp:Content>
