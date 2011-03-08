<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MyAuctionSite.Web.Controllers.VerifyViewModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Please finalize your account
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Please finalize your account</h2>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Registration was unsuccessful. Please correct the errors and try again.") %>
        <div>
            <fieldset>     
                <%: Html.HiddenFor(m=>m.RegistrationId) %>
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>
                <p>
                    <input type="submit" value="Continue" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
