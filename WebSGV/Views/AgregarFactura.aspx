<%@ Page Title="Registro de Factura" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AgregarFactura.aspx.cs" Inherits="WebSGV.Views.AgregarFactura" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        /* Estilos base y reseteo */
        .main-container {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color: #333;
            background-color: #fff;
            padding: 0;
        }

        .agregar-factura-container {
            max-width: 1200px;
            margin: 0 auto;
        }

        /* Estilos del encabezado */
        .header {
            font-size: 28px;
            font-weight: 500;
            color: #333;
            margin-bottom: 30px;
            padding-bottom: 10px;
            border-bottom: 1px solid #eaeaea;
        }

        /* Organización del formulario en grid */
        .form-container {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 25px;
            padding: 20px;
        }
        
        /* Para pantallas pequeñas, una sola columna */
        @media (max-width: 768px) {
            .form-container {
                grid-template-columns: 1fr;
            }
        }

        /* Estilo para grupos de formularios */
        .form-group {
            margin-bottom: 25px;
        }

        .form-group label {
            display: block;
            font-weight: 500;
            margin-bottom: 8px;
            color: #444;
            font-size: 14px;
        }

        /* Estilos de los campos de entrada */
        .form-control {
            width: 100%;
            height: 38px;
            padding: 8px 12px;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-sizing: border-box;
            font-size: 14px;
            transition: border-color 0.2s, box-shadow 0.2s;
            background-color: #fafafa;
        }

        .form-control:focus {
            border-color: #2196F3;
            outline: none;
            box-shadow: 0 0 0 2px rgba(33, 150, 243, 0.1);
            background-color: #fff;
        }

        /* Estilo para el botón */
        .btn {
            background-color: #2196F3;
            color: white;
            border: none;
            border-radius: 4px;
            padding: 10px 20px;
            font-size: 14px;
            font-weight: 500;
            cursor: pointer;
            transition: background-color 0.2s;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            min-width: 120px;
        }

        .btn:hover {
            background-color: #1976D2;
        }

        /* Contenedor para el botón */
        .btn-container {
            grid-column: 1 / -1;
            margin-top: 15px;
            display: flex;
            justify-content: flex-start;
        }

        /* Mensajes de error */
        .text-danger {
            color: #f44336;
            font-size: 13px;
            margin-top: 5px;
        }

        /* Ajustes para campos específicos */
        input[type="date"] {
            background-color: #fafafa;
        }

        /* Ajustes para el encabezado y controles de página completa */
        .header-container {
            grid-column: 1 / -1;
        }
    </style>

    <div class="main-container agregar-factura-container">
        <div class="form-container">
            <div class="header-container">
                <h1 class="header">Registro de Factura</h1>
            </div>
            
            <!-- Primera columna -->
            <div>
                <div class="form-group">
                    <label for="txtNumFactura">N° Factura:</label>
                    <asp:TextBox ID="txtNumFactura" runat="server" CssClass="form-control" Placeholder="Ingrese el N° de Factura" />
                </div>
                
                <div class="form-group">
                    <label for="txtFechaEmision">Fecha de Emisión:</label>
                    <asp:TextBox ID="txtFechaEmision" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
            </div>
            
            <!-- Segunda columna -->
            <div>
                <div class="form-group">
                    <label for="txtNumPedido">N° Pedido:</label>
                    <asp:TextBox ID="txtNumPedido" runat="server" CssClass="form-control" Placeholder="Ingrese el N° de Pedido (10 dígitos)" MaxLength="10" />
                </div>
                
                <div class="form-group">
                    <label for="txtImporteTotal">Importe Total:</label>
                    <asp:TextBox ID="txtImporteTotal" runat="server" CssClass="form-control" Placeholder="Ingrese el Importe Total" />
                </div>
            </div>
            
            <!-- Mensaje de error (ocupa ambas columnas) -->
            <div class="header-container">
                <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger"></asp:Label>
            </div>
            
            <!-- Botón de guardar -->
            <div class="btn-container">
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