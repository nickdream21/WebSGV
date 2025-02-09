<%@ Page Title="Agregar Orden de Viaje" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AgregarOrdenViaje.aspx.cs" Inherits="WebSGV.Views.AgregarOrdenViaje"  %>

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
<div class="tab-pane fade show active" id="datos" role="tabpanel" aria-labelledby="datos-tab">
    <h3 class="tab-header text-center mb-4">Datos del Viaje</h3>
    <form id="formDatosViaje">
        <asp:HiddenField ID="hfValidationError" runat="server" />
        <!-- Primera fila: N° CPI y N° Orden Viaje -->
        <div class="row mb-3">
            <div class="col-md-6 form-group">
                <label for="txtCPI">N° CPI:</label>
                <input type="text" id="txtCPI" runat="server" class="form-control" placeholder="Ingrese el N° CPI" />
            </div>
            <div class="col-md-6 form-group">
                <label for="txtOrdenViaje">N° Orden Viaje:</label>
                <input type="text" id="txtOrdenViaje" runat="server" class="form-control" placeholder="Ingrese el N° Orden de Viaje" />
            </div>
        </div>

        <!-- Segunda fila: Fechas y horas -->
        <div class="row mb-3">
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

        <!-- Tercera fila: Cliente, Placas, y Conductor -->
        <div class="row mb-3">
            <div class="col-md-4 form-group">
                <label for="ddlCliente">Cliente:</label>
                <asp:DropDownList ID="ddlCliente" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Seleccione un cliente" Value="" />
                </asp:DropDownList>
            </div>
            <div class="col-md-4 form-group">
                <label for="ddlPlacaTracto">Placa Tracto:</label>
                <select id="ddlPlacaTracto" class="form-control">
                    <!-- Opciones generadas dinámicamente -->
                </select>
            </div>
            <div class="col-md-4 form-group">
                <label for="ddlPlacaCarreta">Placa Carreta:</label>
                <select id="ddlPlacaCarreta" class="form-control">
                    <!-- Opciones generadas dinámicamente -->
                </select>
            </div>
        </div>

        <!-- Cuarta fila: Conductor y Observaciones -->
        <div class="row mb-4">
            <div class="col-md-6 form-group">
                <label for="ddlConductor">Conductor:</label>
                <select id="ddlConductor" class="form-control">
                    <!-- Opciones generadas dinámicamente -->
                </select>
            </div>
            <div class="col-md-6 form-group">
                <label for="txtObservaciones">Observaciones:</label>
                <textarea id="txtObservaciones" runat="server" class="form-control" rows="3" placeholder="Añadir observaciones"></textarea>
            </div>
        </div>

        <!-- Botón Siguiente -->
        <div class="form-group text-end">
    <asp:Button ID="btnSiguiente" runat="server" CssClass="btn btn-primary px-4 py-2" Text="Siguiente" OnClick="btnSiguiente_Click" />
</div>
<asp:Label ID="lblErrores" runat="server" CssClass="text-danger"></asp:Label>

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
    <tbody id="gastosFijosBody">
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
    <tbody id="gastosAdicionalesBody">
        <!-- Aquí se añadirán dinámicamente los gastos adicionales -->
    </tbody>
</table>
<!-- Botón para agregar filas -->
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
                        <button type="button" class="btn btn-secondary" onclick="showPreviousTab('datos')">Atrás</button>
                        <button type="button" class="btn btn-primary" onclick="showNextTab('guias')">Siguiente</button>
                    </div>
    </form>
</div>

            <!-- Guías -->
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
            <button type="button" class="btn btn-secondary" onclick="showPreviousTab('liquidacion')">Atrás</button>
            <button type="submit" class="btn btn-success" onclick="return validarFormulario()">Guardar</button>
        </div>
    </form>
</div>
        </div>
    </div>
<script src="<%= ResolveUrl("~/Scripts/Custom/JavaScript.js") %>"></script>


     <!-- Librerías para autocompletado -->
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <!-- CSS de Select2 -->
<link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.1.0-rc.0/css/select2.min.css" rel="stylesheet" />
<!-- JS de Select2 -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.1.0-rc.0/js/select2.min.js"></script>

<!-- Librerías necesarias -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.1.0-rc.0/css/select2.min.css" />
<script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.1.0-rc.0/js/select2.min.js"></script>

<script>
    $(document).ready(function () {
        $(document).ready(function () {
            // Inicializar Select2 para Placa Tracto
            $("#ddlPlacaTracto").select2({
                placeholder: "Buscar una placa de tracto...",
                data: JSON.parse('<%= ViewState["PlacasTracto"] %>').map(function (item) {
                    return { id: item, text: item };
                }),
                minimumInputLength: 1
            });

            // Inicializar Select2 para Placa Carreta
            $("#ddlPlacaCarreta").select2({
                placeholder: "Buscar una placa de carreta...",
                data: JSON.parse('<%= ViewState["PlacasCarreta"] %>').map(function (item) {
                    return { id: item, text: item };
                }),
                minimumInputLength: 1
            });

            // Inicializar Select2 para Conductor
            $("#ddlConductor").select2({
                placeholder: "Buscar un conductor...",
                dropdownParent: $('#ddlConductor').parent(),
                data: JSON.parse('<%= ViewState["Conductores"] %>').map(function (item) {
                    return { id: item, text: item };
                }),
                minimumInputLength: 1
            });
        });

    });
    function agregarFila() {
        // Crear una nueva fila
        const nuevaFila =
            <tr>
                <td class="numeroFila"></td>
                <td><input type="text" class="form-control" placeholder="Gasto Adicional"></td>
                <td><input type="text" class="form-control" placeholder="Descripción"></td>
                <td><input type="number" class="form-control" placeholder="Soles"></td>
                <td><input type="number" class="form-control" placeholder="Dólares"></td>
                <td class="text-center">
                    <button type="button" class="btn btn-danger btnEliminarFila">Eliminar</button>
                </td>
            </tr>
            ;

        // Añadir la fila al cuerpo de gastos adicionales
        $("#gastosAdicionalesBody").append(nuevaFila);

        // Recalcular los números de las filas
        recalcularNumeros();
    }

    // Evento para eliminar una fila
    $(document).on("click", ".btnEliminarFila", function () {
        // Eliminar la fila seleccionada
        $(this).closest("tr").remove();

        // Recalcular los números de las filas
        recalcularNumeros();
    });

    // Función para recalcular los números de las filas
    function recalcularNumeros() {
        // Iterar sobre las filas y asignar números consecutivos
        $("#gastosAdicionalesBody tr").each(function (index) {
            $(this).find(".numeroFila").text(index + 9); // Comienza en 9 porque los gastos fijos terminan en 8
        });
    }

    function showNextTab(nextTabId) {
        $('.nav-tabs .nav-link.active').removeClass('active').attr('aria-selected', 'false');
        $('.tab-content .tab-pane.active').removeClass('show active');
        $(#${ nextTabId } - tab).addClass('active').attr('aria-selected', 'true');
        $(#${ nextTabId }).addClass('show active');
    }

    // Función para cambiar a la pestaña anterior
    function showPreviousTab(prevTabId) {
        // Mover la clase 'active' y mostrar la pestaña anterior
        $('.nav-tabs .nav-link.active').removeClass('active').attr('aria-selected', 'false');
        $('.tab-content .tab-pane.active').removeClass('show active');
        $(#${ prevTabId } - tab).addClass('active').attr('aria-selected', 'true');
        $(#${ prevTabId }).addClass('show active');

        // Mantener los datos visibles si ya fueron rellenados
        if (prevTabId === "liquidacion") {
            $('#liquidacion').find('input, select, textarea').each(function () {
                $(this).val($(this).val());
            });
        }
    }

    // Función para mostrar los campos adicionales al seleccionar Ruta 2
    function toggleRutaDetails() {
        const selectedRoute = $('#ddlRuta').val();
        if (selectedRoute === "2") {
            $('#rutaDetails').show();
        } else {
            $('#rutaDetails').hide();
        }
    }

    // Función para validar los datos antes de guardar
    function validarFormulario() {
        let isValid = true;

        // Validar campos requeridos
        $('#formGuias').find('input, select').each(function () {
            if ($(this).val() === "" && $(this).attr('required')) {
                isValid = false;
                alert('Por favor, complete todos los campos obligatorios.');
                return false;
            }
        });

        return isValid;
    }


    $(document).ready(function () {
        // Agregar una fila
        $(document).on("click", ".btn-add-row", function () {
            const nuevaFila =
                <tr>
                    <td>
                        <select class="form-control">
                            <option value="">Seleccione un producto</option>
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
                ;

            $("#tablaProductos tbody").append(nuevaFila);
            actualizarOpcionesProductos();
        });

        // Eliminar una fila
        $(document).on("click", ".btn-remove-row", function () {
            if ($("#tablaProductos tbody tr").length > 1) {
                $(this).closest("tr").remove();
                actualizarOpcionesProductos();
            } else {
                alert("No puede eliminar todas las filas. Debe haber al menos una.");
            }
        });

        // Actualizar las opciones disponibles en los select
        function actualizarOpcionesProductos() {
            const productosSeleccionados = [];

            // Recopilar productos seleccionados en la tabla
            $("#tablaProductos tbody tr").each(function () {
                const productoSeleccionado = $(this).find("select").val();
                if (productoSeleccionado) {
                    productosSeleccionados.push(productoSeleccionado);
                }
            });

            // Actualizar cada select de la tabla
            $("#tablaProductos tbody tr").each(function () {
                const selectActual = $(this).find("select");
                const productoSeleccionadoActual = selectActual.val();

                // Guardar la selección actual y limpiar opciones
                selectActual.empty();
                selectActual.append('<option value="">Seleccione un producto</option>');
                selectActual.append('<option>Producto 1</option>');
                selectActual.append('<option>Producto 2</option>');

                // Deshabilitar los productos ya seleccionados en otros selects
                productosSeleccionados.forEach(function (producto) {
                    if (producto !== productoSeleccionadoActual) {
                        selectActual.find(option: contains(${ producto })).attr("disabled", true);
                    }
                });

                // Restaurar la selección actual
                selectActual.val(productoSeleccionadoActual);
            });
        }

        // Detectar cambio en el select y actualizar opciones
        $(document).on("change", "#tablaProductos tbody select", function () {
            actualizarOpcionesProductos();
        });
    });

    $(document).ready(function () {
        $('#btnSiguienteDatos').on('click', function () {
                showNextTab('liquidacion'); // Cambia de pestaña
        });
    });

    // Función para mostrar la siguiente pestaña
    function showNextTab(nextTabId) {
        $('.nav-tabs .nav-link.active').removeClass('active').attr('aria-selected', 'false');
        $('.tab-content .tab-pane.active').removeClass('show active');
        $(#${ nextTabId } - tab).addClass('active').attr('aria-selected', 'true');
        $(#${ nextTabId }).addClass('show active');
    }




</script>



</asp:Content>
