<%@ Page Title="Agregar Orden de Viaje" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AgregarOrdenViaje.aspx.cs" Inherits="WebSGV.Views.AgregarOrdenViaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tabs-container">
        <!-- Pestañas -->
        <ul class="nav nav-tabs" id="ordenViajeTabs" role="tablist">
            <li class="nav-item">
                <a class="nav-link active" id="datos-tab" data-toggle="tab" href="#datos" role="tab" aria-controls="datos" aria-selected="true">Datos del Viaje</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="liquidacion-tab" data-toggle="tab" href="#liquidacion" role="tab" aria-controls="liquidacion" aria-selected="false">Liquidación</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="guias-tab" data-toggle="tab" href="#guias" role="tab" aria-controls="guias" aria-selected="false">Guías</a>
            </li>
        </ul>

        <!-- Contenido de las pestañas -->
        <div class="tab-content" id="ordenViajeContent">
            <!-- Pestaña 1: Datos del Viaje -->
           <!-- Pestaña 1: Datos del Viaje -->
<div class="tab-pane fade show active" id="datos" role="tabpanel" aria-labelledby="datos-tab">
    <h3 class="tab-header">Datos del Viaje</h3>
    <form id="formDatosViaje">
        <div class="row">
            <div class="col-md-6 form-group">
                <label for="txtCPI">N° CPI:</label>
                <input type="text" id="txtCPI" runat="server" class="form-control" placeholder="Ingrese el N° CPI" />
            </div>
            <div class="col-md-6 form-group">
                <label for="txtOrdenViaje">N° Orden Viaje:</label>
                <input type="text" id="txtOrdenViaje" runat="server" class="form-control" placeholder="Ingrese el N° Orden de Viaje" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-3 form-group">
                <label for="txtFechaSalida">Fecha de Salida:</label>
                <input type="date" id="txtFechaSalida" runat="server" class="form-control" />
            </div>
            <div class="col-md-3 form-group">
                <label for="txtHoraSalida">Hora de Salida:</label>
                <input type="time" id="txtHoraSalida" runat="server" class="form-control" />
            </div>
            <div class="col-md-3 form-group">
                <label for="txtFechaLlegada">Fecha de Llegada:</label>
                <input type="date" id="txtFechaLlegada" runat="server" class="form-control" />
            </div>
            <div class="col-md-3 form-group">
                <label for="txtHoraLlegada">Hora de Llegada:</label>
                <input type="time" id="txtHoraLlegada" runat="server" class="form-control" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-4 form-group">
                <label for="ddlCliente">Cliente:</label>
                <asp:DropDownList ID="ddlCliente" runat="server" CssClass="form-control">
                <asp:ListItem Text="Seleccione un cliente" Value="" />
                </asp:DropDownList>
            </div>
            <div class="col-md-4 form-group">
                <label for="ddlTracto">Placa Tracto:</label>
                <asp:DropDownList id="ddlTracto" runat="server" class="form-control">
                <asp:ListItem Text="Seleccione un tracto" Value="" />
                </asp:DropDownList>
            </div>
            <div class="col-md-4 form-group">
                <label for="ddlCarreta">Placa Carreta:</label>
                <asp:DropDownList id="ddlCarreta" runat="server" class="form-control">
                <asp:ListItem Text="Seleccione una placa" Value="" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 form-group">
                <label for="ddlConductor">Conductor:</label>
                <asp:DropDownList id="ddlConductor" runat="server" class="form-control">
                 <asp:ListItem Text="Seleccione un conductor" Value="" />
                </asp:DropDownList>
            </div>
            <div class="col-md-6 form-group">
                <label for="txtObservaciones">Observaciones:</label>
                <textarea id="txtObservaciones" runat="server" class="form-control" rows="3" placeholder="Añadir observaciones"></textarea>
            </div>
        </div>
        <div class="form-group text-right">
            <button type="button" class="btn btn-primary" onclick="showNextTab('liquidacion-tab')">Siguiente</button>
        </div>
    </form>
</div>


            <!-- Pestaña 2: Liquidación -->
<div class="tab-pane fade" id="liquidacion" role="tabpanel" aria-labelledby="liquidacion-tab">
    <h3 class="tab-header">Liquidación</h3>
    <form id="formLiquidacion">
        <!-- Tabla de ingresos -->
<h5>Ingresos</h5>
<table class="table table-bordered liquidacion-tabla-ingresos"">
    <thead class="liquidacion-tabla-cabecera">
        <tr>
            <th>#</th>
            <th>Ingresos</th>
            <th>Descripción</th>
            <th>Soles (S/)</th>
            <th>Dólares ($)</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>Despacho</td>
            <td><input type="text" class="form-control" placeholder="Descripción"></td>
            <td><input type="number" class="form-control" placeholder="Soles"></td>
            <td><input type="number" class="form-control" placeholder="Dólares"></td>
        </tr>
        <tr>
            <td>2</td>
            <td>Mensualidad</td>
            <td><input type="text" class="form-control" placeholder="Descripción"></td>
            <td><input type="number" class="form-control" placeholder="Soles"></td>
            <td><input type="number" class="form-control" placeholder="Dólares"></td>
        </tr>
        <tr>
            <td>3</td>
            <td>Otros Autorizados</td>
            <td><input type="text" class="form-control" placeholder="Descripción"></td>
            <td><input type="number" class="form-control" placeholder="Soles"></td>
            <td><input type="number" class="form-control" placeholder="Dólares"></td>
        </tr>
        <tr>
            <td>4</td>
            <td>Préstamo</td>
            <td><input type="text" class="form-control" placeholder="Descripción"></td>
            <td><input type="number" class="form-control" placeholder="Soles"></td>
            <td><input type="number" class="form-control" placeholder="Dólares"></td>
        </tr>
    </tbody>
</table>

        <!-- Tabla de Gastos -->
<h5>Gastos</h5>
<table class="table table-bordered liquidacion-tabla-gastos" id="tablaGastos">
    <thead class="liquidacion-tabla-cabecera">
        <tr>
            <th>#</th>
            <th>Gastos</th>
            <th>Descripción</th>
            <th>Soles (S/)</th>
            <th>Dólares ($)</th>
            <th>Acción</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>Peajes</td>
            <td><input type="text" class="form-control" placeholder="Descripción"></td>
            <td><input type="number" class="form-control" placeholder="Soles"></td>
            <td><input type="number" class="form-control" placeholder="Dólares"></td>
            <td></td>
        </tr>
        <tr>
            <td>2</td>
            <td>Alimentación</td>
            <td><input type="text" class="form-control" placeholder="Descripción"></td>
            <td><input type="number" class="form-control" placeholder="Soles"></td>
            <td><input type="number" class="form-control" placeholder="Dólares"></td>
            <td></td>
        </tr>
        <tr>
            <td>3</td>
            <td>Apoyo-Seguridad</td>
            <td><input type="text" class="form-control" placeholder="Descripción"></td>
            <td><input type="number" class="form-control" placeholder="Soles"></td>
            <td><input type="number" class="form-control" placeholder="Dólares"></td>
            <td></td>
        </tr>
        <tr>
            <td>4</td>
            <td>Reparaciones Varios</td>
            <td><input type="text" class="form-control" placeholder="Descripción"></td>
            <td><input type="number" class="form-control" placeholder="Soles"></td>
            <td><input type="number" class="form-control" placeholder="Dólares"></td>
            <td></td>
        </tr>
        <tr>
            <td>5</td>
            <td>Movilidad</td>
            <td><input type="text" class="form-control" placeholder="Descripción"></td>
            <td><input type="number" class="form-control" placeholder="Soles"></td>
            <td><input type="number" class="form-control" placeholder="Dólares"></td>
            <td></td>
        </tr>
        <tr>
            <td>6</td>
            <td>Encapada/Descencarpada</td>
            <td><input type="text" class="form-control" placeholder="Descripción"></td>
            <td><input type="number" class="form-control" placeholder="Soles"></td>
            <td><input type="number" class="form-control" placeholder="Dólares"></td>
            <td></td>
        </tr>
        <tr>
            <td>7</td>
            <td>Hospedaje</td>
            <td><input type="text" class="form-control" placeholder="Descripción"></td>
            <td><input type="number" class="form-control" placeholder="Soles"></td>
            <td><input type="number" class="form-control" placeholder="Dólares"></td>
            <td></td>
        </tr>
        <tr>
            <td>8</td>
            <td>Combustible</td>
            <td><input type="text" class="form-control" placeholder="Descripción"></td>
            <td><input type="number" class="form-control" placeholder="Soles"></td>
            <td><input type="number" class="form-control" placeholder="Dólares"></td>
            <td></td>
        </tr>
    </tbody>
</table>
<!-- Botones para agregar y eliminar filas -->
<div class="text-right">
    <button type="button" class="btn btn-success" onclick="agregarFila()">Añadir Fila</button>
</div>


        <!-- Resumen -->
        <h5>Resumen</h5>
        <div class="table-responsive">
            <table class="table table-bordered">
                <thead class="table-light">
                    <tr>
                        <th>Total Ingresos</th>
                        <th>Total Gastos</th>
                        <th>Diferencia de Saldo</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><input type="number" class="form-control" readonly></td>
                        <td><input type="number" class="form-control" readonly></td>
                        <td><input type="number" class="form-control" readonly></td>
                    </tr>
                </tbody>
            </table>
        </div>

        <!-- Botones de navegación -->
        <div class="form-group text-right mt-3">
            <button type="button" class="btn btn-secondary" onclick="showPreviousTab('datos-tab')">Atrás</button>
            <button type="button" class="btn btn-primary" onclick="showNextTab('guias-tab')">Siguiente</button>
        </div>
    </form>
</div>

             <!--  Guías -->
          <div class="tab-pane fade" id="guias" role="tabpanel" aria-labelledby="guias-tab">
    <h3 class="tab-header">Guías de Transporte</h3>
    <form id="formGuias">
        <!-- Número de Guías -->
        <div class="row">
            <div class="col-md-6 form-group">
                <label for="txtGuiaTransportista">N° Guía Transportista:</label>
                <input type="text" id="txtGuiaTransportista" class="form-control" placeholder="Ingrese N° Guía Transportista">
            </div>
            <div class="col-md-6 form-group">
                <label for="txtGuiaCliente">N° Guía Cliente:</label>
                <input type="text" id="txtGuiaCliente" class="form-control" placeholder="Ingrese N° Guía Cliente">
            </div>
        </div>

            <!-- Campo Rutas -->
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <label for="ddlRuta">Ruta:</label>
                         <asp:DropDownList ID="ddlRuta" runat="server" CssClass="form-control" ClientIDMode="Static" AutoPostBack="false" onchange="toggleRutaDetails()">
                            <asp:ListItem Text="Seleccione una ruta" Value="" />
                            <asp:ListItem Text="Ruta 1" Value="1" />
                            <asp:ListItem Text="Ruta 2" Value="2" />
                        </asp:DropDownList>

                        </div>
                    </div>

                    <!-- Campos adicionales para Ruta 2 -->
                    
        <div id="rutaDetails" style="display: none;">
    <div class="row">
        <div class="col-md-6 form-group">
            <label for="ddlPlantaDescarga">Planta de Descarga:</label>
            <asp:DropDownList ID="ddlPlantaDescarga" runat="server" CssClass="form-control">
                <asp:ListItem Text="Seleccione una planta" Value="" />
                <asp:ListItem Text="Planta 1" Value="planta1" />
                <asp:ListItem Text="Planta 2" Value="planta2" />
            </asp:DropDownList>
        </div>
        <div class="col-md-6 form-group">
            <label for="txtNumManifiesto">N° Manifiesto:</label>
            <asp:TextBox ID="txtNumManifiesto" runat="server" CssClass="form-control" Placeholder="Ingrese N° Manifiesto"></asp:TextBox>
        </div>
    </div>
</div>

        






        <h5>Productos asociados</h5>
        <!-- Tabla Productos -->
        <table class="table table-bordered guias-tabla-productos" id="tablaProductos">
            <thead class="guias-tabla-cabecera">
                <tr>
                    <th>Producto</th>
                    <th>Cantidad de Bolsas</th>
                    <th>Añadir</th>
                    <th>Eliminar</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <select class="form-control">
                            <option>Producto 1</option>
                            <option>Producto 2</option>
                        </select>
                    </td>
                    <td>
                        <input type="number" class="form-control" placeholder="Cantidad">
                    </td>
                    <td class="text-center">
                        <button type="button" class="btn btn-primary btn-add-row">Añadir</button>
                    </td>
                    <td class="text-center">
                        <button type="button" class="btn btn-danger btn-remove-row">Eliminar</button>
                    </td>
                </tr>
            </tbody>
        </table>

        <!-- Botones de navegación -->
        <div class="form-group text-right">
            <button type="button" class="btn btn-secondary" onclick="showPreviousTab('liquidacion-tab')">Atrás</button>
            <button type="submit" class="btn btn-success" onclick="return validarFormulario()">Guardar</button>
        </div>
    </form>
</div>


        </div>
    </div>
<script src="<%= ResolveUrl("~/Scripts/Custom/JavaScript.js") %>"></script>


</asp:Content>
