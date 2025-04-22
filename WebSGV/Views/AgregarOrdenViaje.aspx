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
            <li class="nav-item">&nbsp;</li>
        </ul>

        <!-- Contenido de las pestañas -->
        <div class="tab-content" id="ordenViajeContent">
            <!-- Pestaña 1: Datos del Viaje -->
            <div class="tab-pane fade show active" id="datos" role="tabpanel" aria-labelledby="datos-tab">
                <h3 class="tab-header text-center mb-4">Datos del Viaje</h3>
                <div id="formDatosViaje">
                    <asp:HiddenField ID="hfValidationError" runat="server" />
                    <!-- Primera fila: N° CPI y N° Orden Viaje -->
                    <div class="row mb-3">
                        <div class="col-md-6 form-group">
                            <label for="txtCPI">N° CPI:</label>
                            <input type="text" id="txtCPI" runat="server" class="form-control" placeholder="Ingrese el N° CPI" required />
                        </div>
                        <div class="col-md-6 form-group">
                            <label for="txtOrdenViaje">N° Orden Viaje:</label>
                            <input type="text" id="txtOrdenViaje" runat="server" class="form-control" placeholder="Ingrese el N° Orden de Viaje" required />
                        </div>
                    </div>

                    <!-- Segunda fila: Fechas y horas -->
                    <div class="row mb-3">
                        <div class="col-md-3 form-group">
                            <label for="txtFechaSalida">Fecha de Salida:</label>
                            <input type="date" id="txtFechaSalida" runat="server" class="form-control" required />
                        </div>
                        <div class="col-md-3 form-group">
                            <label for="txtHoraSalida">Hora de Salida:</label>
                            <input type="time" id="txtHoraSalida" runat="server" class="form-control" required />
                        </div>
                        <div class="col-md-3 form-group">
                            <label for="txtFechaLlegada">Fecha de Llegada:</label>
                            <input type="date" id="txtFechaLlegada" runat="server" class="form-control" required />
                        </div>
                        <div class="col-md-3 form-group">
                            <label for="txtHoraLlegada">Hora de Llegada:</label>
                            <input type="time" id="txtHoraLlegada" runat="server" class="form-control" required />
                        </div>
                    </div>

                    <!-- Tercera fila: Cliente, Placas y Conductor -->
                    <div class="row mb-3">
                        <div class="col-md-4 form-group">
                            <label for="ddlCliente">Cliente:</label>
                            <asp:DropDownList ID="ddlCliente" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlCliente_SelectedIndexChanged" required>
                                <asp:ListItem Text="Seleccione un cliente" Value="" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 form-group">
                            <label for="ddlPlacaTracto">Placa Tracto:</label>
                            <asp:DropDownList ID="ddlPlacaTracto" runat="server" CssClass="form-control select2" required>
                                <asp:ListItem Text="Seleccione una placa" Value="" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 form-group">
                            <label for="ddlPlacaCarreta">Placa Carreta:</label>
                            <asp:DropDownList ID="ddlPlacaCarreta" runat="server" CssClass="form-control select2" required>
                                <asp:ListItem Text="Seleccione una placa" Value="" />
                            </asp:DropDownList>
                        </div>
                    </div>

                    <!-- Cuarta fila: Conductor y Observaciones -->
                    <div class="row mb-4">
                        <div class="col-md-6 form-group">
                            <label for="ddlConductor">Conductor:</label>
                            <asp:DropDownList ID="ddlConductor" runat="server" CssClass="form-control select2" required>
                                <asp:ListItem Text="Seleccione un conductor" Value="" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6 form-group">
                            <label for="txtObservaciones">Observaciones:</label>
                            <textarea id="txtObservaciones" runat="server" class="form-control" rows="3" placeholder="Añadir observaciones"></textarea>
                        </div>
                    </div>

                    <!-- Botón Siguiente -->
                    <div class="form-group text-end">
                        <asp:Button ID="btnSiguiente" runat="server" CssClass="btn btn-primary px-4 py-2" Text="Siguiente" OnClientClick="return validarDatosViaje();" OnClick="btnSiguiente_Click" />
                    </div>

                    <!-- Etiqueta para mostrar errores -->
                    <div class="form-group">
                        <asp:Label ID="lblErrores" runat="server" CssClass="text-danger" EnableViewState="false"></asp:Label>
                    </div>
                </div>
            </div>

            <!-- Pestaña 2: Liquidación -->
            <div class="tab-pane fade" id="liquidacion" role="tabpanel" aria-labelledby="liquidacion-tab">
                <h3 class="tab-header">Liquidación</h3>
                <div id="formLiquidacion">

                    <!-- Campo oculto para gastos adicionales - ESTE ES EL CAMBIO IMPORTANTE -->
                    <input type="hidden" id="hiddenGastosAdicionales" name="gastosAdicionales" value="[]" />

                    <!-- Tabla de ingresos -->
                    <h5>Ingresos</h5>
                    <table class="table table-bordered liquidacion-tabla-ingresos">
                        <thead class="liquidacion-tabla-cabecera">
                            <tr>
                                <th>#</th>
                                <th>Ingresos</th>
                                <th>Descripción</th>
                                <th>Soles (S/)</th>
                                <th>Dólares ($)</th>
                            </tr>
                        </thead>
                        <tbody id="ingresosBody">
                            <tr>
                                <td>1</td>
                                <td>Despacho</td>
                                <td>
                                    <input type="text" class="form-control" name="txtDescDespacho" placeholder="Descripción"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtDespachoSoles" placeholder="Soles" min="0" step="0.01"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtDespachoDolares" placeholder="Dólares" min="0" step="0.01"></td>
                            </tr>
                            <tr>
                                <td>2</td>
                                <td>Mensualidad</td>
                                <td>
                                    <input type="text" class="form-control" name="txtDescMensualidad" placeholder="Descripción"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtMensualidadSoles" placeholder="Soles" min="0" step="0.01"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtMensualidadDolares" placeholder="Dólares" min="0" step="0.01"></td>
                            </tr>
                            <tr>
                                <td>3</td>
                                <td>Otros Autorizados</td>
                                <td>
                                    <input type="text" class="form-control" name="txtDescOtros" placeholder="Descripción"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtOtrosSoles" placeholder="Soles" min="0" step="0.01"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtOtrosDolares" placeholder="Dólares" min="0" step="0.01"></td>
                            </tr>
                            <tr>
                                <td>4</td>
                                <td>Préstamo</td>
                                <td>
                                    <input type="text" class="form-control" name="txtDescPrestamo" placeholder="Descripción"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtPrestamoSoles" placeholder="Soles" min="0" step="0.01"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtPrestamoDolares" placeholder="Dólares" min="0" step="0.01"></td>
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
                                <td>
                                    <input type="text" class="form-control" name="txtDescPeajes" placeholder="Descripción"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtPeajesSoles" placeholder="Soles" value="213" min="0" step="0.01"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtPeajesDolares" placeholder="Dólares" value="123" min="0" step="0.01"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>2</td>
                                <td>Alimentación</td>
                                <td>
                                    <input type="text" class="form-control" name="txtDescAlimentacion" placeholder="Descripción"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtAlimentacionSoles" placeholder="Soles" value="342" min="0" step="0.01"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtAlimentacionDolares" placeholder="Dólares" value="123" min="0" step="0.01"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>3</td>
                                <td>Apoyo-Seguridad</td>
                                <td>
                                    <input type="text" class="form-control" name="txtDescApoyoSeguridad" placeholder="Descripción"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtApoyoSeguridadSoles" placeholder="Soles" value="324" min="0" step="0.01"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtApoyoSeguridadDolares" placeholder="Dólares" value="123" min="0" step="0.01"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>4</td>
                                <td>Reparaciones Varios</td>
                                <td>
                                    <input type="text" class="form-control" name="txtDescReparaciones" placeholder="Descripción"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtReparacionesSoles" placeholder="Soles" value="324" min="0" step="0.01"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtReparacionesDolares" placeholder="Dólares" value="142" min="0" step="0.01"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>5</td>
                                <td>Movilidad</td>
                                <td>
                                    <input type="text" class="form-control" name="txtDescMovilidad" placeholder="Descripción"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtMovilidadSoles" placeholder="Soles" value="324" min="0" step="0.01"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtMovilidadDolares" placeholder="Dólares" value="142" min="0" step="0.01"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>6</td>
                                <td>Encapada/Descencarpada</td>
                                <td>
                                    <input type="text" class="form-control" name="txtDescEncapada" placeholder="Descripción"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtEncapadaSoles" placeholder="Soles" value="123" min="0" step="0.01"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtEncapadaDolares" placeholder="Dólares" value="214" min="0" step="0.01"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>7</td>
                                <td>Hospedaje</td>
                                <td>
                                    <input type="text" class="form-control" name="txtDescHospedaje" placeholder="Descripción"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtHospedajeSoles" placeholder="Soles" value="324" min="0" step="0.01"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtHospedajeDolares" placeholder="Dólares" value="124" min="0" step="0.01"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>8</td>
                                <td>Combustible</td>
                                <td>
                                    <input type="text" class="form-control" name="txtDescCombustible" placeholder="Descripción"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtCombustibleSoles" placeholder="Soles" value="123" min="0" step="0.01"></td>
                                <td>
                                    <input type="number" class="form-control" name="txtCombustibleDolares" placeholder="Dólares" value="142" min="0" step="0.01"></td>
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
                                    <th>Concepto</th>
                                    <th>Soles (S/)</th>
                                    <th>Dólares ($)</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Total Ingresos</td>
                                    <td id="totalIngresosSoles">0.00</td>
                                    <td id="totalIngresosDolares">0.00</td>
                                </tr>
                                <tr>
                                    <td>Total Gastos</td>
                                    <td id="totalGastosSoles">0.00</td>
                                    <td id="totalGastosDolares">0.00</td>
                                </tr>
                                <tr>
                                    <td>Diferencia de Saldo</td>
                                    <td id="diferenciaSaldoSoles">0.00</td>
                                    <td id="diferenciaSaldoDolares">0.00</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!-- Botones de navegación -->
                    <div class="form-group text-right mt-3">
                        <button type="button" class="btn btn-secondary" onclick="showPreviousTab('datos')">Atrás</button>
                        <button type="button" class="btn btn-primary" onclick="showNextTab('guias')">Siguiente</button>
                    </div>
                </div>
            </div>

            <!-- Pestaña 3: Guías -->
            <div class="tab-pane fade" id="guias" role="tabpanel" aria-labelledby="guias-tab">
                <h3 class="tab-header">Guías de Transporte</h3>
                <div id="formGuias">
                    <!-- Número de Guías -->
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <label for="txtGuiaTransportista">N° Guía Transportista:</label>
                            <asp:TextBox ID="txtGuiaTransportista" runat="server" CssClass="form-control" placeholder="Ingrese N° Guía Transportista" required="required"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label for="txtGuiaCliente">N° Guía Cliente:</label>
                            <asp:TextBox ID="txtGuiaCliente" runat="server" CssClass="form-control" placeholder="Ingrese N° Guía Cliente" required="required"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Campo Rutas -->
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <label for="ddlRuta">Ruta:</label>
                            <asp:DropDownList ID="ddlRuta" runat="server" CssClass="form-control" ClientIDMode="Static" AutoPostBack="false" onchange="toggleRutaDetails()">
                                <asp:ListItem Text="Seleccione una ruta" Value="" />
                                <asp:ListItem Text="Ruta 1" Value="1" />
                                <asp:ListItem Text="Sullana-Guayaquil-Sullana" Value="2" />
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
                                <label for="txtManifiesto">N° Manifiesto:</label>
                                <asp:TextBox ID="txtManifiesto" runat="server" CssClass="form-control" Placeholder="Ingrese N° Manifiesto"></asp:TextBox>
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
                            <!-- Las filas se agregarán dinámicamente con JavaScript -->
                        </tbody>
                    </table>

                    <!-- Botones de navegación -->
                    <div class="form-group text-right">
                        <button type="button" class="btn btn-secondary" onclick="showPreviousTab('liquidacion')">Atrás</button>
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success" OnClientClick="return validarFormulario();" OnClick="btnGuardar_Click" />
                    </div>
                </div>
            </div>

            <!-- Librerías necesarias -->
            <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
            <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.1.0-rc.0/css/select2.min.css" />
            <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
            <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
            <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.1.0-rc.0/js/select2.min.js"></script>
            <script src="<%= ResolveUrl("~/Scripts/Custom/JavaScript.js") %>"></script>

            <style>
                /* Estilos para las tablas de Ingresos y Gastos */
                .liquidacion-tabla-ingresos th:nth-child(4),
                .liquidacion-tabla-ingresos td:nth-child(4),
                .liquidacion-tabla-ingresos th:nth-child(5),
                .liquidacion-tabla-ingresos td:nth-child(5),
                .liquidacion-tabla-gastos th:nth-child(4),
                .liquidacion-tabla-gastos td:nth-child(4),
                .liquidacion-tabla-gastos th:nth-child(5),
                .liquidacion-tabla-gastos td:nth-child(5) {
                    width: 15%; /* Ancho igual para las columnas Soles y Dólares */
                    min-width: 120px; /* Ancho mínimo para asegurar legibilidad */
                }

                /* Estilos para la tabla de Resumen */
                .table-responsive table th:nth-child(2),
                .table-responsive table td:nth-child(2),
                .table-responsive table th:nth-child(3),
                .table-responsive table td:nth-child(3) {
                    width: 20%; /* Ancho igual para las columnas Soles y Dólares en el Resumen */
                    min-width: 120px;
                }

                .table-responsive table th:nth-child(1),
                .table-responsive table td:nth-child(1) {
                    width: 60%; /* Ancho para la columna Concepto en el Resumen */
                }
            </style>

            <script>
                $(document).ready(function () {
                    // Inicializar Select2
                    $('.select2').select2();

                    // Evento para detectar cambios en los dropdowns
                    $("#<%= ddlPlacaTracto.ClientID %>").on('change', function () {
                        var selectedPlaca = $(this).val();
                        console.log("Placa Tracto seleccionada: " + selectedPlaca);
                    });

                    $("#<%= ddlPlacaCarreta.ClientID %>").on('change', function () {
                        var selectedPlaca = $(this).val();
                        console.log("Placa Carreta seleccionada: " + selectedPlaca);
                    });

                    $("#<%= ddlConductor.ClientID %>").on('change', function () {
                        var selectedConductor = $(this).val();
                        console.log("Conductor seleccionado: " + selectedConductor);
                    });

                    // Verificar si hay un mensaje de éxito desde el servidor para cambiar de pestaña
                    if ($('#<%= lblErrores.ClientID %>').text() === "") {
                        var validationSuccess = $('#<%= hfValidationError.ClientID %>').val();
                        if (validationSuccess === "true") {
                            showNextTab('liquidacion');
                        }
                    }

                    // Mostrar mensaje de error del servidor en un alert
                    var errorMessage = $('#<%= lblErrores.ClientID %>').text();
                    if (errorMessage !== "") {
                        alert(errorMessage.replace(/<br\/>/g, "\n"));
                    }

                    // Escuchar cambios en los campos de las tablas de Ingresos y Gastos
                    $(document).on('input change', '#ingresosBody input, #gastosFijosBody input, #gastosAdicionalesBody input', function () {
                        actualizarResumen();
                        actualizarGastosAdicionales(); // Actualizar el campo oculto al cambiar los datos
                    });

                    // Escuchar cambios específicamente en los campos de gastos adicionales
                    $(document).on('change input', ".nombreCategoria, .descripcion, .soles, .dolares", function () {
                        actualizarGastosAdicionales();
                    });

                    // Limpiar campos y calcular los totales iniciales al cargar la página
                    limpiarCamposLiquidacion();
                    actualizarResumen();

                    // Asegurarse de que la tabla de productos esté vacía antes de agregar la fila inicial
                    $("#tablaProductos tbody").empty();
                    // Agregar una fila inicial a la tabla de productos al cargar la página
                    agregarFilaProducto();
                });

                // Validar datos de la pestaña "Datos del Viaje" antes de pasar a la siguiente pestaña
                function validarDatosViaje() {
                    let isValid = true;
                    let errores = [];

                    // Validar campos de texto
                    if ($('#<%= txtCPI.ClientID %>').val().trim() === "") {
                        errores.push("El campo 'N° CPI' es obligatorio.");
                        $('#<%= txtCPI.ClientID %>').addClass('is-invalid');
                    } else {
                        $('#<%= txtCPI.ClientID %>').removeClass('is-invalid');
                    }

                    if ($('#<%= txtOrdenViaje.ClientID %>').val().trim() === "") {
                        errores.push("El campo 'N° Orden Viaje' es obligatorio.");
                        $('#<%= txtOrdenViaje.ClientID %>').addClass('is-invalid');
        } else {
            $('#<%= txtOrdenViaje.ClientID %>').removeClass('is-invalid');
                    }

                    // Validar fechas
                    let fechaSalida = $('#<%= txtFechaSalida.ClientID %>').val();
                    let fechaLlegada = $('#<%= txtFechaLlegada.ClientID %>').val();
                    let horaSalida = $('#<%= txtHoraSalida.ClientID %>').val();
                    let horaLlegada = $('#<%= txtHoraLlegada.ClientID %>').val();
                    let fechaActual = new Date(); // Fecha actual
                    fechaActual.setHours(0, 0, 0, 0); // Normalizar a medianoche para comparación

                    if (!fechaSalida) {
                        errores.push("La 'Fecha de Salida' es obligatoria.");
                        $('#<%= txtFechaSalida.ClientID %>').addClass('is-invalid');
        } else {
            $('#<%= txtFechaSalida.ClientID %>').removeClass('is-invalid');
                    }

                    if (!horaSalida) {
                        errores.push("La 'Hora de Salida' es obligatoria.");
                        $('#<%= txtHoraSalida.ClientID %>').addClass('is-invalid');
        } else {
            $('#<%= txtHoraSalida.ClientID %>').removeClass('is-invalid');
                    }

                    if (!fechaLlegada) {
                        errores.push("La 'Fecha de Llegada' es obligatoria.");
                        $('#<%= txtFechaLlegada.ClientID %>').addClass('is-invalid');
        } else {
            $('#<%= txtFechaLlegada.ClientID %>').removeClass('is-invalid');
                    }

                    if (!horaLlegada) {
                        errores.push("La 'Hora de Llegada' es obligatoria.");
                        $('#<%= txtHoraLlegada.ClientID %>').addClass('is-invalid');
        } else {
            $('#<%= txtHoraLlegada.ClientID %>').removeClass('is-invalid');
                    }

                    if (fechaSalida && fechaLlegada) {
                        let fechaSalidaDate = new Date(fechaSalida);
                        let fechaLlegadaDate = new Date(fechaLlegada);

                        if (fechaSalidaDate > fechaLlegadaDate) {
                            errores.push("La 'Fecha de Salida' no puede ser mayor a la 'Fecha de Llegada'.");
                            $('#<%= txtFechaSalida.ClientID %>').addClass('is-invalid');
                $('#<%= txtFechaLlegada.ClientID %>').addClass('is-invalid');
            }

            if (fechaLlegadaDate > fechaActual) {
                errores.push("La 'Fecha de Llegada' no puede ser mayor a la fecha actual (" + fechaActual.toLocaleDateString() + ").");
                $('#<%= txtFechaLlegada.ClientID %>').addClass('is-invalid');
                        }
                    }

                    // Validar dropdowns
                    if ($('#<%= ddlCliente.ClientID %>').val() === "") {
                        errores.push("Debe seleccionar un 'Cliente'.");
                        $('#<%= ddlCliente.ClientID %>').addClass('is-invalid');
        } else {
            $('#<%= ddlCliente.ClientID %>').removeClass('is-invalid');
                    }

                    if ($('#<%= ddlPlacaTracto.ClientID %>').val() === "") {
                        errores.push("Debe seleccionar una 'Placa Tracto'.");
                        $('#<%= ddlPlacaTracto.ClientID %>').addClass('is-invalid');
        } else {
            $('#<%= ddlPlacaTracto.ClientID %>').removeClass('is-invalid');
                    }

                    if ($('#<%= ddlPlacaCarreta.ClientID %>').val() === "") {
                        errores.push("Debe seleccionar una 'Placa Carreta'.");
                        $('#<%= ddlPlacaCarreta.ClientID %>').addClass('is-invalid');
        } else {
            $('#<%= ddlPlacaCarreta.ClientID %>').removeClass('is-invalid');
                    }

                    if ($('#<%= ddlConductor.ClientID %>').val() === "") {
                        errores.push("Debe seleccionar un 'Conductor'.");
                        $('#<%= ddlConductor.ClientID %>').addClass('is-invalid');
        } else {
            $('#<%= ddlConductor.ClientID %>').removeClass('is-invalid');
                    }

                    // Mostrar errores si los hay
                    if (errores.length > 0) {
                        alert(errores.join("\n"));
                        return false;
                    }

                    // Limpiar el campo oculto antes de enviar al servidor
                    $('#<%= hfValidationError.ClientID %>').val("");
                    return true; // Permitir el postback para validaciones del servidor
                }

                // Función para agregar una fila en la tabla de gastos - CORREGIDA
                function agregarFila() {
                    const nuevaFila = `
            <tr>
                <td class="numeroFila"></td>
                <td><input type="text" class="form-control nombreCategoria" placeholder="Gasto Adicional"></td>
                <td><input type="text" class="form-control descripcion" placeholder="Descripción"></td>
                <td><input type="number" class="form-control soles" placeholder="Soles" min="0" value="0" step="0.01"></td>
                <td><input type="number" class="form-control dolares" placeholder="Dólares" min="0" value="0" step="0.01"></td>
                <td class="text-center">
                    <button type="button" class="btn btn-danger btnEliminarFila">Eliminar</button>
                </td>
            </tr>
        `;

                    $("#gastosAdicionalesBody").append(nuevaFila);
                    recalcularNumeros();
                    actualizarResumen();
                    actualizarGastosAdicionales(); // Actualizar el campo oculto al agregar una fila
                }

                // Evento para eliminar una fila - CORREGIDO
                $(document).on("click", ".btnEliminarFila", function () {
                    $(this).closest("tr").remove();
                    recalcularNumeros();
                    actualizarResumen();
                    actualizarGastosAdicionales(); // Actualizar el campo oculto al eliminar una fila
                });

                // Función para recalcular los números de las filas
                function recalcularNumeros() {
                    $("#gastosAdicionalesBody tr").each(function (index) {
                        $(this).find(".numeroFila").text(index + 9); // Comienza en 9 porque los gastos fijos terminan en 8
                    });
                }

                // Función para limpiar los campos numéricos de las tablas Ingresos y Gastos
                function limpiarCamposLiquidacion() {
                    // Limpiar campos de Ingresos (Soles y Dólares)
                    $("#ingresosBody tr").each(function () {
                        $(this).find("td:eq(3) input").val(""); // Soles
                        $(this).find("td:eq(4) input").val(""); // Dólares
                    });

                    // Limpiar campos de Gastos Fijos (Soles y Dólares)
                    $("#gastosFijosBody tr").each(function () {
                        $(this).find("td:eq(3) input").val(""); // Soles
                        $(this).find("td:eq(4) input").val(""); // Dólares
                    });

                    // Limpiar campos de Gastos Adicionales (Soles y Dólares)
                    $("#gastosAdicionalesBody tr").each(function () {
                        $(this).find("td:eq(3) input").val(""); // Soles
                        $(this).find("td:eq(4) input").val(""); // Dólares
                    });

                    actualizarGastosAdicionales(); // Actualizar el campo oculto después de limpiar
                }

                // Función para actualizar el resumen de totales
                function actualizarResumen() {
                    // Calcular totales de Ingresos
                    let totalIngresosSoles = 0;
                    let totalIngresosDolares = 0;

                    $("#ingresosBody tr").each(function () {
                        let soles = parseFloat($(this).find("td:eq(3) input").val()) || 0;
                        let dolares = parseFloat($(this).find("td:eq(4) input").val()) || 0;
                        totalIngresosSoles += soles;
                        totalIngresosDolares += dolares;
                    });

                    // Calcular totales de Gastos (gastos fijos + adicionales)
                    let totalGastosSoles = 0;
                    let totalGastosDolares = 0;

                    // Gastos fijos
                    $("#gastosFijosBody tr").each(function () {
                        let soles = parseFloat($(this).find("td:eq(3) input").val()) || 0;
                        let dolares = parseFloat($(this).find("td:eq(4) input").val()) || 0;
                        totalGastosSoles += soles;
                        totalGastosDolares += dolares;
                    });

                    // Gastos adicionales
                    $("#gastosAdicionalesBody tr").each(function () {
                        let soles = parseFloat($(this).find("td:eq(3) input").val()) || 0;
                        let dolares = parseFloat($(this).find("td:eq(4) input").val()) || 0;
                        totalGastosSoles += soles;
                        totalGastosDolares += dolares;
                    });

                    // Calcular diferencia de saldo
                    let diferenciaSaldoSoles = totalIngresosSoles - totalGastosSoles;
                    let diferenciaSaldoDolares = totalIngresosDolares - totalGastosDolares;

                    // Actualizar los valores en la tabla de Resumen
                    $("#totalIngresosSoles").text(totalIngresosSoles.toFixed(2));
                    $("#totalIngresosDolares").text(totalIngresosDolares.toFixed(2));
                    $("#totalGastosSoles").text(totalGastosSoles.toFixed(2));
                    $("#totalGastosDolares").text(totalGastosDolares.toFixed(2));
                    $("#diferenciaSaldoSoles").text(diferenciaSaldoSoles.toFixed(2));
                    $("#diferenciaSaldoDolares").text(diferenciaSaldoDolares.toFixed(2));
                }

                // Función para mostrar la siguiente pestaña
                function showNextTab(nextTabId) {
                    $('.nav-tabs .nav-link.active').removeClass('active').attr('aria-selected', 'false');
                    $('.tab-content .tab-pane.active').removeClass('show active');
                    $('#' + nextTabId + '-tab').addClass('active').attr('aria-selected', 'true');
                    $('#' + nextTabId).addClass('show active');

                    // Limpiar campos si se muestra la pestaña Liquidación
                    if (nextTabId === "liquidacion") {
                        limpiarCamposLiquidacion();
                        actualizarResumen();
                    }
                }

                // Función para cambiar a la pestaña anterior
                function showPreviousTab(prevTabId) {
                    $('.nav-tabs .nav-link.active').removeClass('active').attr('aria-selected', 'false');
                    $('.tab-content .tab-pane.active').removeClass('show active');
                    $('#' + prevTabId + '-tab').addClass('active').attr('aria-selected', 'true');
                    $('#' + prevTabId).addClass('show active');

                    // Limpiar campos si se muestra la pestaña Liquidación
                    if (prevTabId === "liquidacion") {
                        limpiarCamposLiquidacion();
                        actualizarResumen();
                    }
                }

                // Función para mostrar los campos adicionales al seleccionar Sullana-Guayaquil-Sullana
                function toggleRutaDetails() {
                    const selectedRouteValue = $('#ddlRuta').val();
                    if (selectedRouteValue === "2") { // idRuta = 2 para Sullana-Guayaquil-Sullana
                        $('#rutaDetails').show();
                    } else {
                        $('#rutaDetails').hide();
                    }
                }

                // Función para validar los datos antes de guardar
                function validarFormulario() {
                    let isValid = true;
                    let errores = [];

                    // Validar N° Guía Transportista
                    const guiaTransportista = $('#<%= txtGuiaTransportista.ClientID %>').val().trim();
        if (guiaTransportista === "") {
            errores.push("El campo 'N° Guía Transportista' es obligatorio.");
            $('#<%= txtGuiaTransportista.ClientID %>').addClass('is-invalid');
        } else {
            $('#<%= txtGuiaTransportista.ClientID %>').removeClass('is-invalid');
        }

        // Validar N° Guía Cliente
        const guiaCliente = $('#<%= txtGuiaCliente.ClientID %>').val().trim();
        if (guiaCliente === "") {
            errores.push("El campo 'N° Guía Cliente' es obligatorio.");
            $('#<%= txtGuiaCliente.ClientID %>').addClass('is-invalid');
        } else {
            $('#<%= txtGuiaCliente.ClientID %>').removeClass('is-invalid');
        }

        // Validar Ruta
        const ruta = $('#ddlRuta').val();
        if (ruta === "") {
            errores.push("Debe seleccionar una 'Ruta'.");
            $('#ddlRuta').addClass('is-invalid');
        } else {
            $('#ddlRuta').removeClass('is-invalid');
        }

        // Validar Planta de Descarga y N° Manifiesto si la ruta es "Sullana-Guayaquil-Sullana"
        if (ruta === "2") { // idRuta = 2 para Sullana-Guayaquil-Sullana
            const plantaDescarga = $('#<%= ddlPlantaDescarga.ClientID %>').val();
            if (plantaDescarga === "") {
                errores.push("Debe seleccionar una 'Planta de Descarga' para la ruta Sullana-Guayaquil-Sullana.");
                $('#<%= ddlPlantaDescarga.ClientID %>').addClass('is-invalid');
            } else {
                $('#<%= ddlPlantaDescarga.ClientID %>').removeClass('is-invalid');
            }

            const manifiesto = $('#<%= txtManifiesto.ClientID %>').val().trim();
            if (manifiesto === "") {
                errores.push("El campo 'N° Manifiesto' es obligatorio para la ruta Sullana-Guayaquil-Sullana.");
                $('#<%= txtManifiesto.ClientID %>').addClass('is-invalid');
            } else {
                $('#<%= txtManifiesto.ClientID %>').removeClass('is-invalid');
                        }
                    }

                    // Validar la tabla de productos
                    let productosValidos = true;
                    let productosData = [];
                    $("#tablaProductos tbody tr").each(function () {
                        const producto = $(this).find(".producto-dropdown").val();
                        const cantidad = $(this).find("input[name='cantidad']").val();

                        if (producto === "0" || !cantidad || cantidad <= 0) {
                            productosValidos = false;
                            $(this).find(".producto-dropdown").addClass('is-invalid');
                            $(this).find("input[name='cantidad']").addClass('is-invalid');
                        } else {
                            $(this).find(".producto-dropdown").removeClass('is-invalid');
                            $(this).find("input[name='cantidad']").removeClass('is-invalid');
                            // Agregar los datos del producto a la lista para enviar al servidor
                            productosData.push({
                                idProducto: producto,
                                cantidad: cantidad
                            });
                        }
                    });

                    if (!productosValidos) {
                        errores.push('Por favor, seleccione un producto y una cantidad válida en todas las filas de "Productos asociados".');
                    }

                    // Validar gastos adicionales
                    let gastosValidos = true;
                    $("#gastosAdicionalesBody tr").each(function () {
                        const nombreCategoria = $(this).find(".nombreCategoria").val().trim();
                        const soles = parseFloat($(this).find(".soles").val()) || 0;
                        const dolares = parseFloat($(this).find(".dolares").val()) || 0;

                        if (nombreCategoria && (soles < 0 || dolares < 0)) {
                            errores.push(`Los valores de 'Soles' y 'Dólares' para el gasto "${nombreCategoria}" no pueden ser negativos.`);
                            $(this).find(".soles").addClass('is-invalid');
                            $(this).find(".dolares").addClass('is-invalid');
                            gastosValidos = false;
                        } else {
                            $(this).find(".soles").removeClass('is-invalid');
                            $(this).find(".dolares").removeClass('is-invalid');
                        }
                    });

                    // Mostrar errores si los hay
                    if (errores.length > 0) {
                        alert(errores.join("\n"));
                        return false;
                    }

                    // Enviar los datos de los productos al servidor
                    $('<input>').attr({
                        type: 'hidden',
                        name: 'productosData',
                        value: JSON.stringify(productosData)
                    }).appendTo('#formGuias');

                    // Actualizar los gastos adicionales antes de enviar el formulario
                    actualizarGastosAdicionales();

                    // Si las validaciones del cliente pasan, permitir el postback para validaciones del servidor
                    return true;
                }

                // Gestión de la tabla de productos
                const productos = <%= ObtenerProductosJSON() %>;
                const productosSeleccionados = new Set(); // Para rastrear productos seleccionados

                // Agregar una fila
                $(document).on("click", ".btn-add-row", function () {
                    const ultimaFila = $("#tablaProductos tbody tr:last");
                    if (ultimaFila.length && !validarFila(ultimaFila)) {
                        alert("Complete todos los datos de la fila actual antes de agregar una nueva.");
                        return;
                    }
                    agregarFilaProducto();
                });

                // Eliminar una fila
                $(document).on("click", ".btn-remove-row", function () {
                    if ($("#tablaProductos tbody tr").length > 1) {
                        const fila = $(this).closest("tr");
                        const dropdown = fila.find(".producto-dropdown");
                        const valorSeleccionado = dropdown.val();

                        // Liberar el producto seleccionado
                        if (valorSeleccionado !== "0") {
                            productosSeleccionados.delete(valorSeleccionado);
                        }

                        fila.remove();
                        actualizarDropdowns();
                    } else {
                        alert("No puede eliminar todas las filas. Debe haber al menos una.");
                    }
                });

                // Función para agregar una fila
                function agregarFilaProducto() {
                    const nuevaFila = `
            <tr>
                <td>
                    <select class="form-control producto-dropdown" onchange="actualizarSeleccion(this)">
                        <option value="0">Seleccione un producto</option>
                        ${productos.map(p => `<option value="${p.idProducto}">${p.nombre}</option>`).join('')}
                    </select>
                </td>
                <td>
                    <input type="number" class="form-control" placeholder="Cantidad" name="cantidad" min="1">
                </td>
                <td class="text-center">
                    <button type="button" class="btn btn-primary btn-add-row">Añadir</button>
                </td>
                <td class="text-center">
                    <button type="button" class="btn btn-danger btn-remove-row">Eliminar</button>
                </td>
            </tr>
        `;

                    $("#tablaProductos tbody").append(nuevaFila);
                    actualizarDropdowns();
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
                    $("#tablaProductos .producto-dropdown").each(function () {
                        const dropdown = $(this);
                        const opciones = dropdown.find("option");
                        opciones.each(function () {
                            const opcion = $(this);
                            if (productosSeleccionados.has(opcion.val()) && opcion.val() !== dropdown.val()) {
                                opcion.prop("disabled", true); // Deshabilitar producto seleccionado
                            } else {
                                opcion.prop("disabled", false); // Habilitar si está disponible
                            }
                        });
                    });
                }

                // Validar una fila antes de agregar otra
                function validarFila(fila) {
                    const producto = fila.find(".producto-dropdown").val();
                    const cantidad = fila.find("input[name='cantidad']").val();

                    if (producto === "0" || !cantidad || cantidad <= 0) {
                        return false;
                    }
                    return true;
                }

                // FUNCIÓN CORREGIDA: Actualizar el campo oculto con los datos de gastos adicionales
                function actualizarGastosAdicionales() {
                    // Array para almacenar los datos de gastos adicionales
                    let gastosAdicionales = [];

                    // Recorrer cada fila en la tabla de gastos adicionales
                    $("#gastosAdicionalesBody tr").each(function () {
                        // Obtener los valores de cada campo
                        let nombreCategoria = $(this).find(".nombreCategoria").val();
                        let descripcion = $(this).find(".descripcion").val();
                        let soles = parseFloat($(this).find(".soles").val()) || 0;
                        let dolares = parseFloat($(this).find(".dolares").val()) || 0;

                        // Añadir al array solo si hay un nombre de categoría
                        if (nombreCategoria) {
                            gastosAdicionales.push({
                                nombreCategoria: nombreCategoria,
                                descripcion: descripcion,
                                soles: soles,
                                dolares: dolares
                            });
                        }
                    });

                    // Convertir el array a JSON y guardarlo en el campo oculto
                    $('input[name="gastosAdicionales"]').val(JSON.stringify(gastosAdicionales));

                    // Mostrar en consola para depuración
                    console.log("Gastos adicionales actualizados:", JSON.stringify(gastosAdicionales));
                }
            </script>
</asp:Content>
