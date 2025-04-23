<%@ Page Title="Buscar Factura" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscarFactura.aspx.cs" Inherits="WebSGV.Views.BusquedaFactura" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main-container buscar-factura-container">
        <div class="form-container">
            <h1 class="header">Búsqueda de Factura</h1>

            <!-- Sección de Búsqueda -->
            <div class="row search-section">
                <div class="col-md-8 form-group">
                    <label for="txtBuscarFactura">N° Factura:</label>
                    <asp:TextBox ID="txtBuscarFactura" runat="server" CssClass="form-control" placeholder="Ingrese el N° de Factura a buscar"></asp:TextBox>
                </div>
                <div class="col-md-4 form-group">
                    <label>&nbsp;</label> <!-- Espacio para alinear con el campo de texto -->
                    <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary form-control" Text="Buscar" OnClick="BuscarFacturaClick" />
                </div>
            </div>

            <asp:Panel ID="pnlResultados" runat="server" Visible="false">
                <!-- Información Principal de la Factura -->
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h2>Información de la Factura</h2>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6 form-group">
                                <label for="txtNumFactura">N° Factura:</label>
                                <asp:TextBox ID="txtNumFactura" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-6 form-group">
                                <label for="txtNumPedido">N° Pedido:</label>
                                <asp:TextBox ID="txtNumPedido" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 form-group">
                                <label for="txtFechaEmision">Fecha de Emisión:</label>
                                <asp:TextBox ID="txtFechaEmision" runat="server" CssClass="form-control" TextMode="Date" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-6 form-group">
                                <label for="txtValorTotal">Valor Total:</label>
                                <asp:TextBox ID="txtValorTotal" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Botones de Acción -->
                <div class="row">
                    <div class="col-md-12 form-group text-center">
                        <asp:Button ID="btnHabilitarEdicion" runat="server" CssClass="btn btn-primary" Text="Habilitar Edición" OnClick="HabilitarEdicion" />
                        <asp:Button ID="btnGuardarCambios" runat="server" CssClass="btn btn-success" Text="Guardar Cambios" OnClick="GuardarCambios" Visible="false" />
                        <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="Cancelar" />
                    </div>
                </div>

                <div class="form-group text-center">
                    <asp:Label ID="lblMensaje" runat="server" CssClass="text-info"></asp:Label>
                </div>
            </asp:Panel>

            <!-- Panel de No Resultados -->
            <asp:Panel ID="pnlNoResultados" runat="server" Visible="false">
                <div class="alert alert-warning">
                    <strong>No se encontró ninguna factura con el número especificado.</strong>
                    <p>Verifique el número e intente nuevamente o <a href="AgregarFactura.aspx" class="alert-link">cree una nueva factura</a>.</p>
                </div>
            </asp:Panel>
        </div>
    </div>
    
    <script>
        // Script para validar números de pedido al editar
        document.addEventListener('DOMContentLoaded', function () {
            const inputNumPedido = document.getElementById('<%= txtNumPedido.ClientID %>');
            if (inputNumPedido) {
                inputNumPedido.addEventListener('keypress', function (e) {
                    const key = e.key;
                    // Permitir solo números y teclas de control
                    if (!/[0-9]/.test(key) && e.keyCode !== 8) {
                        e.preventDefault();
                    }
                });
                inputNumPedido.addEventListener('input', function () {
                    // Eliminar caracteres no numéricos
                    this.value = this.value.replace(/[^0-9]/g, '');
                    // Limitar a 10 dígitos
                    if (this.value.length > 10) {
                        this.value = this.value.slice(0, 10);
                    }
                });
            }
        });
    </script>
</asp:Content>