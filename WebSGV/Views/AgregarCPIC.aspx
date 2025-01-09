<%@ Page Title="Agregar CPIC" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AgregarCPIC.aspx.cs" Inherits="WebSGV.Views.AgregarCPIC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main-container agregar-cpic-container">
        <div class="form-container">
            <h1 class="header">Registro de CPIC</h1>

            <!-- Campos de Entrada -->
            <div class="row">
                <div class="col-md-6 form-group">
                    <label for="txtNumCPIC">N° CPIC:</label>
                    <asp:TextBox ID="txtNumCPIC" runat="server" CssClass="form-control" placeholder="Ingrese el N° CPIC"></asp:TextBox>
                </div>
                <div class="col-md-6 form-group">
                    <label for="txtNumFactura">N° Factura:</label>
                    <asp:TextBox ID="txtNumFactura" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="TxtNumFactura_TextChanged" placeholder="Ingrese el N° Factura"></asp:TextBox>
                    <asp:Label ID="lblErrorFactura" runat="server" CssClass="text-danger"></asp:Label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6 form-group">
                    <label for="txtFechaEmision">Fecha de Emisión:</label>
                    <asp:TextBox ID="txtFechaEmision" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-6 form-group">
                    <label for="txtTotalFlete">Valor Total del Flete:</label>
                    <asp:TextBox ID="txtTotalFlete" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>

            <!-- Tabla para Productos -->
            <h2>Productos</h2>
            <div class="table-responsive">
                <table class="table table-bordered" id="tablaProductos">
                    <thead>
                        <tr>
                            <th>Producto</th>
                            <th>Cantidad de Bolsas</th>
                            <th>Peso (Kg)</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- Filas dinámicas -->
                    </tbody>
                </table>
            </div>

            <div class="form-group text-center">
                <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary btn-lg" Text="Guardar" OnClick="GuardarCPIC" />
            </div>
            <div class="form-group text-center">
                <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger"></asp:Label>
            </div>
        </div>
    </div>

    <script>
        const productos = <%= ObtenerProductosJSON() %>; // Cargar productos desde la base de datos
        const productosSeleccionados = new Set(); // Para rastrear productos seleccionados

        // Inicializar la tabla al cargar
        document.addEventListener("DOMContentLoaded", () => {
            agregarFila(); // Agregar una fila inicial
        });

        // Agregar una nueva fila
        function agregarFila() {
            const tabla = document.querySelector("#tablaProductos tbody");

            // Validar la última fila antes de agregar otra
            const ultimaFila = tabla.lastElementChild;
            if (ultimaFila && !validarFila(ultimaFila)) {
                alert("Complete todos los datos de la fila actual antes de agregar una nueva.");
                return;
            }

            // Crear nueva fila
            const nuevaFila = document.createElement("tr");
            nuevaFila.innerHTML = `
                <td>
                    <select class="form-control producto-dropdown" onchange="actualizarSeleccion(this)">
                        <option value="0">Seleccione un producto</option>
                        ${productos.map(p => `<option value="${p.idProducto}">${p.nombre}</option>`).join('')}
                    </select>
                </td>
                <td>
                    <input type="number" class="form-control" placeholder="Cantidad" name="cantidad" min="1">
                </td>
                <td>
                    <input type="number" class="form-control" placeholder="Peso" name="peso" min="1">
                </td>
                <td>
                    <button type="button" class="btn btn-success" onclick="agregarFila()">Añadir</button>
                    <button type="button" class="btn btn-danger" onclick="eliminarFila(this)">Eliminar</button>
                </td>
            `;
            tabla.appendChild(nuevaFila);

            actualizarDropdowns(); // Actualizar los dropdowns para reflejar los productos seleccionados
        }

        // Actualizar selección de productos
        function actualizarSeleccion(dropdown) {
            const valorAnterior = dropdown.dataset.valorAnterior || "0";
            const valorNuevo = dropdown.value;

            // Liberar el producto previamente seleccionado
            if (valorAnterior !== "0") {
                productosSeleccionados.delete(valorAnterior);
            }

            // Validar la nueva selección
            if (valorNuevo !== "0") {
                if (productosSeleccionados.has(valorNuevo)) {
                    alert("Este producto ya está seleccionado.");
                    dropdown.value = "0"; // Restablecer selección
                    return;
                }
                productosSeleccionados.add(valorNuevo);
            }

            dropdown.dataset.valorAnterior = valorNuevo; // Guardar la nueva selección
            actualizarDropdowns();
        }

        // Deshabilitar productos seleccionados en otros dropdowns
        function actualizarDropdowns() {
            document.querySelectorAll(".producto-dropdown").forEach(dropdown => {
                const opciones = dropdown.querySelectorAll("option");
                opciones.forEach(opcion => {
                    if (productosSeleccionados.has(opcion.value) && opcion.value !== dropdown.value) {
                        opcion.disabled = true; // Deshabilitar producto seleccionado
                    } else {
                        opcion.disabled = false; // Habilitar si está disponible
                    }
                });
            });
        }

        // Validar una fila antes de agregar otra
        function validarFila(fila) {
            const producto = fila.querySelector(".producto-dropdown").value;
            const cantidad = fila.querySelector("input[name='cantidad']").value;
            const peso = fila.querySelector("input[name='peso']").value;

            if (producto === "0" || !cantidad || cantidad <= 0 || !peso || peso <= 0) {
                alert("Por favor, complete todos los campos correctamente.");
                return false;
            }
            return true;
        }

        // Eliminar fila y liberar producto
        function eliminarFila(boton) {
            const tabla = document.querySelector("#tablaProductos tbody");
            const filas = tabla.querySelectorAll("tr");

            // Evitar eliminar la última fila
            if (filas.length === 1) {
                alert("Debe existir al menos una fila en la tabla.");
                return;
            }

            const fila = boton.closest("tr");
            const dropdown = fila.querySelector(".producto-dropdown");
            const valorSeleccionado = dropdown.value;

            // Liberar el producto seleccionado
            if (valorSeleccionado !== "0") {
                productosSeleccionados.delete(valorSeleccionado);
            }

            fila.remove();
            actualizarDropdowns();
        }

    </script>
</asp:Content>
