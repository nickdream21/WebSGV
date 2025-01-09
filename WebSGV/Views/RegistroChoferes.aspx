<%@ Page Title="Registro de Choferes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistroChoferes.aspx.cs" Inherits="WebSGV.Views.RegistroChoferes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center vh-100">
        <div class="card registro-choferes-card">
            <div class="card-header registro-choferes-header text-center">
                <h2 class="registro-header-title">Registro de Choferes</h2>
            </div>
            <div class="card-body">
                <form>
                    <!-- Primera fila -->
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <label for="ddlTipoDocumento" class="form-label">
                                Tipo Documento:
                            </label>
                            <asp:DropDownList ID="ddlTipoDocumento" runat="server" CssClass="form-control">
                                <asp:ListItem>DNI</asp:ListItem>
                                <asp:ListItem>Carnet de Extranjería</asp:ListItem>
                                <asp:ListItem>Pasaporte</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6 form-group">
                            <label for="txtDNI" class="form-label">
                                DNI:
                            </label>
                            <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Segunda fila -->
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <label for="txtCarnetExtranjeria" class="form-label">
                                Carnet Extranjería:
                            </label>
                            <asp:TextBox ID="txtCarnetExtranjeria" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label for="txtNombres" class="form-label">
                                Nombres:
                            </label>
                            <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Tercera fila -->
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <label for="txtApellidoPaterno" class="form-label">
                                Apellido Paterno:
                            </label>
                            <asp:TextBox ID="txtApellidoPaterno" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label for="txtApellidoMaterno" class="form-label">
                                Apellido Materno:
                            </label>
                            <asp:TextBox ID="txtApellidoMaterno" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Cuarta fila -->
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <label for="txtTelefono" class="form-label">
                                Teléfono:
                            </label>
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label for="txtFechaNacimiento" class="form-label">
                                Fecha de Nacimiento:
                            </label>
                            <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Dirección y correo -->
                    <div class="form-group">
                        <label for="txtDireccion" class="form-label">
                            Dirección:
                        </label>
                        <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="txtCorreo" class="form-label">
                            Correo:
                        </label>
                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <!-- Botón -->
                    <div class="text-center mt-4">
                        <asp:Button ID="btnRegistrar" runat="server" CssClass="btn btn-primary btn-lg" Text="Registrar" />
                    </div>
                </form>
            </div>
        </div>
    </div>
</asp:Content>
