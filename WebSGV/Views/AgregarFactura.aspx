<%@ Page Title="Registro de Factura" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AgregarFactura.aspx.cs" Inherits="WebSGV.Views.AgregarFactura" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main-container agregar-factura-container">
        <div class="form-container">
            <h1 class="header">Registro de Factura</h1>
            <!-- Formulario de registro de factura -->
            <div class="form-group">
                <!-- N° Factura -->
                <label for="txtNumFactura">N° Factura:</label>
                <asp:TextBox ID="txtNumFactura" runat="server" CssClass="form-control" Placeholder="Ingrese el N° de Factura" />
            </div>
            <!-- Nuevo campo: N° Pedido -->
            <div class="form-group">
                <label for="txtNumPedido">N° Pedido:</label>
                <asp:TextBox ID="txtNumPedido" runat="server" CssClass="form-control" Placeholder="Ingrese el N° de Pedido (10 dígitos)" MaxLength="10" />
            </div>
            <!-- Fecha de Emisión -->
            <div class="form-group">
                <label for="txtFechaEmision">Fecha de Emisión:</label>
                <asp:TextBox ID="txtFechaEmision" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <!-- Importe Total -->
            <div class="form-group">
                <label for="txtImporteTotal">Importe Total:</label>
                <asp:TextBox ID="txtImporteTotal" runat="server" CssClass="form-control" Placeholder="Ingrese el Importe Total" />
            </div>
            <!-- Mensaje de error o éxito -->
            <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger"></asp:Label>
            <!-- Botón Guardar -->
            <div class="form-group">
                <asp:Button ID="btnGuardarFactura" runat="server" CssClass="btn btn-primary" Text="Guardar"
                    OnClick="GuardarFactura" />
            </div>
        </div>
    </div>
     <script>
         // Bloquear letras en el campo "Importe Total"
         document.addEventListener('DOMContentLoaded', function () {
             const inputImporteTotal = document.getElementById('<%= txtImporteTotal.ClientID %>');
            inputImporteTotal.addEventListener('keypress', function (e) {
                const key = e.key;
                // Permitir números, punto decimal y teclas de control como backspace
                if (!/[0-9.]/.test(key) && e.keyCode !== 8) {
                    e.preventDefault();
                }
            });
            inputImporteTotal.addEventListener('input', function () {
                // Asegurarse de que solo hay un punto decimal y limpiar caracteres inválidos
                this.value = this.value.replace(/[^0-9.]/g, '');
                if ((this.value.match(/\./g) || []).length > 1) {
                    this.value = this.value.replace(/\.+$/, '');
                }
            });

            // Validación para el número de pedido: solo permitir dígitos y limitar a 10
            const inputNumPedido = document.getElementById('<%= txtNumPedido.ClientID %>');
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
        });
     </script>
</asp:Content>