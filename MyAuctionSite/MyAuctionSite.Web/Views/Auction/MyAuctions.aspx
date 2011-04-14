<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	MyAuctions
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {
			$.ajax({
				url: 'http://localhost:8080/indexes/dynamic/AuctionSummaryItems',
				dataType: 'jsonp',
				jsonp: 'jsonp',
				success: function (data) {
					$.each(data.Results, function () {
						$("#auctions").append("<p>Description: " + this.Description + ", id: " + this.AuctionId + "</p>");
					});
				},
				error: function (data) { alert('error'); }
			});

		});
	</script>
	<h2>
		MyAuctions</h2>
	<div id="auctions" />
</asp:Content>
