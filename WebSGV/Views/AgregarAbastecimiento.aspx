<%@ Page Title="Agregar Abastecimiento de Combustible" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AgregarAbastecimiento.aspx.cs" Inherits="WebSGV.Views.AgregarAbastecimiento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid abastecimiento-container">
        <!-- Encabezado con número -->
        <div class="d-flex justify-content-between align-items-center mb-4 header-container">
            <h3 class="abastecimiento-header text-uppercase">Parte de Abastecimiento de Combustible</h3>
            <div class="numero-container">
                <label for="numeroAbastecimiento" class="numero-label">N°:</label>
                <input type="text" id="numeroAbastecimiento" class="numero-input" placeholder="Ingrese el N°" value="018885" />
            </div>
        </div>

        <!-- Información General -->
        <div class="card mb-4 shadow-sm">
            <div class="card-header text-white bg-primary">
                Información General
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-3">
                        <label for="tipoVehiculo" class="form-label">Tipo:</label>
                        <select id="tipoVehiculo" class="form-control">
                            <option value="camioneta">Camioneta</option>
                            <option value="camion">Camión</option>
                            <option value="trailer">Trailer</option>
                        </select>
                    </div>
                    <div class="col-md-3">
                        <label for="txtPlaca" class="form-label">Placa:</label>
                        <input type="text" id="txtPlaca" class="form-control" placeholder="Ingrese la placa" />
                    </div>
                    <div class="col-md-3">
                        <label for="txtCarreta" class="form-label">Carreta:</label>
                        <input type="text" id="txtCarreta" class="form-control" placeholder="Ingrese carreta (opcional)" />
                    </div>
                    <div class="col-md-3">
                        <label for="txtConductor" class="form-label">Conductor:</label>
                        <input type="text" id="txtConductor" class="form-control" placeholder="Nombre del conductor" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Ruta y Producto -->
        <div class="card mb-4 shadow-sm">
            <div class="card-header text-white bg-primary">
                Ruta y Producto
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-6">
                        <label for="txtRuta" class="form-label">Ruta:</label>
                        <input type="text" id="txtRuta" class="form-control" placeholder="Ingrese la ruta" />
                    </div>
                    <div class="col-md-6">
                        <label for="txtProducto" class="form-label">Producto:</label>
                        <input type="text" id="txtProducto" class="form-control" placeholder="Ingrese el producto" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Control de Combustible -->
        <div class="card mb-4 shadow-sm">
            <div class="card-header text-white bg-primary">
                Control de Combustible
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-6">
                        <label for="lugarAbastecimiento" class="form-label">Lugar de Abastecimiento:</label>
                        <select id="lugarAbastecimiento" class="form-control">
                            <option value="grifo">Grifo Cochera 03</option>
                            <option value="otro">Otro</option>
                        </select>
                    </div>
                    <div class="col-md-3">
                        <label for="txtFecha" class="form-label">Fecha:</label>
                        <input type="date" id="txtFecha" class="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label for="txtHora" class="form-label">Hora:</label>
                        <input type="time" id="txtHora" class="form-control" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Detalles de Consumo -->
        <div class="card mb-4 shadow-sm">
            <div class="card-header text-white bg-primary">
                Detalles de Consumo
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-3 form-group">
                        <label for="txtGLRuta" class="form-label">GL Ruta Asignada:</label>
                        <input type="number" id="txtGLRuta" class="form-control" placeholder="Ej: 50" />
                    </div>
                    <div class="col-md-3 form-group">
                        <label for="txtGLComprados" class="form-label">GL Comprados en Ruta:</label>
                        <input type="number" id="txtGLComprados" class="form-control" placeholder="Ej: 193.5" />
                    </div>
                    <div class="col-md-3 form-group">
                        <label for="txtTotalGL" class="form-label">GL Total Abastecidos:</label>
                        <input type="number" id="txtTotalGL" class="form-control" placeholder="Ej: 243.5" />
                    </div>
                    <div class="col-md-3 form-group">
                        <label for="txtGLFinal" class="form-label">GL Trae al Finalizar:</label>
                        <input type="number" id="txtGLFinal" class="form-control" placeholder="Ej: 62" />
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-md-3 form-group">
                        <label for="txtGLConsumidos" class="form-label">GL Total Consumidos:</label>
                        <input type="number" id="txtGLConsumidos" class="form-control" placeholder="Ej: 181.65" />
                    </div>
                    <div class="col-md-3 form-group">
                        <label for="txtPrecioDolar" class="form-label">Precio del Dólar:</label>
                        <input type="number" step="0.01" id="txtPrecioDolar" class="form-control" placeholder="Ej: 1.795" />
                    </div>
                    <div class="col-md-3 form-group">
                        <label for="txtMontoTotal" class="form-label">Monto Total GL:</label>
                        <input type="number" step="0.01" id="txtMontoTotal" class="form-control" placeholder="Ej: 193.5" />
                    </div>
                    <div class="col-md-3 form-group">
                        <label for="txtDistancia" class="form-label">Distancia en KM:</label>
                        <input type="number" id="txtDistancia" class="form-control" placeholder="Ej: 1935.9" />
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-md-6 form-group">
                        <label for="txtConsumoComputador" class="form-label">Consumo Computador:</label>
                        <input type="number" step="0.01" id="txtConsumoComputador" class="form-control" placeholder="Ej: 184.2" />
                    </div>
                    <div class="col-md-6 form-group">
                        <label for="txtHoraRetorno" class="form-label">Hora Retorno:</label>
                        <input type="time" id="txtHoraRetorno" class="form-control" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Botones -->
        <div class="text-right">
            <button type="reset" class="btn btn-secondary">Limpiar</button>
            <button type="submit" class="btn btn-primary">Guardar</button>
        </div>
    </div>
</asp:Content>
