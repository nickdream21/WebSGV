<%@ Page Title="Iniciar Sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebSGV.Views.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main-container">
        <img src="/Content/favicon.png" alt="Logo" class="logo" />
        <h1 class="header">Sistema de Gestión de Transporte</h1>
        <p class="subheader">Bienvenido al sistema <strong>Servicios Generales Viviana (SGV)</strong>. Inicia sesión para continuar.</p>
        <div class="login-container">
            <h2 class="title">Iniciar Sesión</h2>
            <div class="form-group">
                <label for="username">Usuario:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Ingrese su usuario"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="password">Contraseña:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Ingrese su contraseña"></asp:TextBox>
            </div>
            <div class="form-group checkbox">
                <asp:CheckBox ID="chkRemember" runat="server" />
                <label for="chkRemember">Recordarme</label>
            </div>
            <asp:Button ID="btnLogin" runat="server" CssClass="login-btn" Text="Acceder"  OnClick="btnLogin_Click" />
        </div>
    </div>
</asp:Content>
