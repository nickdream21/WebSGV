<%@ Page Title="Dashboard - SGV" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WebSGV.Views.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Añadir UpdatePanel -->
    <asp:UpdatePanel ID="upDashboard" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- Timer para actualización automática (opcional) -->
            <asp:Timer ID="tmrRefresh" runat="server" Interval="60000" OnTick="tmrRefresh_Tick" Enabled="false" />

            <div class="dashboard-container">
                <div class="dashboard-header">
                    <h1 class="dashboard-title">INDICADORES - SERVICIOS GENERALES VIVIANA E.I.R.L</h1>
                    <!-- Botón server-side -->
                    <asp:Button ID="btnRefresh" runat="server" Text="Actualizar" OnClick="btnRefresh_Click" CssClass="refresh-btn" />
                </div>

                <div class="date-filter">
                    <label for="ddlMes">Mes:</label>
                    <asp:DropDownList ID="ddlMes" runat="server" CssClass="filter-dropdown">
                        <asp:ListItem Value="1">Enero</asp:ListItem>
                        <asp:ListItem Value="2">Febrero</asp:ListItem>
                        <asp:ListItem Value="3">Marzo</asp:ListItem>
                        <asp:ListItem Value="4">Abril</asp:ListItem>
                        <asp:ListItem Value="5">Mayo</asp:ListItem>
                        <asp:ListItem Value="6">Junio</asp:ListItem>
                        <asp:ListItem Value="7">Julio</asp:ListItem>
                        <asp:ListItem Value="8">Agosto</asp:ListItem>
                        <asp:ListItem Value="9">Septiembre</asp:ListItem>
                        <asp:ListItem Value="10">Octubre</asp:ListItem>
                        <asp:ListItem Value="11">Noviembre</asp:ListItem>
                        <asp:ListItem Value="12">Diciembre</asp:ListItem>
                    </asp:DropDownList>

                    <label for="ddlAnio">Año:</label>
                    <asp:DropDownList ID="ddlAnio" runat="server" CssClass="filter-dropdown">
                        <asp:ListItem Value="2024">2024</asp:ListItem>
                        <asp:ListItem Value="2025">2025</asp:ListItem>
                    </asp:DropDownList>

                    <asp:Button ID="btnFiltrar" runat="server" Text="Aplicar filtro" OnClick="btnFiltrar_Click" CssClass="refresh-btn" />
                </div>

                <!-- Tabs de navegación usando LinkButtons para postbacks limpios -->
                <div class="tabs-container">
                    <asp:LinkButton ID="lnkGeneral" runat="server" CssClass="tab active" OnClick="lnkGeneral_Click">General</asp:LinkButton>
                    <asp:LinkButton ID="lnkTramiteAduanero" runat="server" CssClass="tab" OnClick="lnkTramiteAduanero_Click">Trámite Aduanero</asp:LinkButton>
                    <asp:LinkButton ID="lnkTiemposAdicionales" runat="server" CssClass="tab" OnClick="lnkTiemposAdicionales_Click">Tiempos Adicionales</asp:LinkButton>
                    <asp:LinkButton ID="lnkBodegasDistancias" runat="server" CssClass="tab" OnClick="lnkBodegasDistancias_Click">Bodegas y Distancias</asp:LinkButton>
                </div>

                <div id="loader" class="loader" runat="server" visible="false"></div>

                <!-- Panel 1: General -->
                <asp:Panel ID="pnlGeneral" runat="server" CssClass="tab-content active">
                    <div class="dashboard-row">
                        <div class="kpi-card">
                            <div class="kpi-title">% Cump hora prog.</div>
                            <div class="kpi-value">
                                <asp:Literal ID="litCumplimiento" runat="server">0</asp:Literal>
                            </div>
                        </div>
                        <div class="kpi-card">
                            <div class="kpi-title">Total camiones</div>
                            <div class="kpi-value">
                                <asp:Literal ID="litCamiones" runat="server">0</asp:Literal>
                            </div>
                        </div>
                        <div class="kpi-card">
                            <div class="kpi-title">Total Pedidos</div>
                            <div class="kpi-value">
                                <asp:Literal ID="litPedidos" runat="server">0</asp:Literal>
                            </div>
                        </div>
                        <div class="kpi-card">
                            <div class="kpi-title">Camiones x Pedido</div>
                            <div class="kpi-value">
                                <asp:Literal ID="litCamionesPedido" runat="server">0</asp:Literal>
                            </div>
                        </div>
                    </div>

                    <div class="chart-row">
                        <div class="chart-container full">
                            <div class="chart-title">Tiempos promedio en Trujillo (Hrs)</div>
                            <canvas id="chartTrujillo"></canvas>
                        </div>
                    </div>

                    <div class="chart-row">
                        <div class="chart-container half">
                            <div class="chart-title">Tiempo prom. deTrujillo-Planta Ecuador (días)</div>
                            <canvas id="chartTrujilloEcuador"></canvas>
                        </div>
                        <div class="chart-container half">
                            <div class="chart-title">Tiempo promedio en Base (Hrs)</div>
                            <canvas id="chartBase"></canvas>
                        </div>
                    </div>

                    <div class="chart-row">
                        <div class="chart-container half">
                            <div class="chart-title">Tiempos promedio en Inbalnor (Hrs)</div>
                            <canvas id="chartInbalnor"></canvas>
                        </div>
                        <div class="chart-container half">
                            <div class="chart-title">Tiempos promedio en Jave (Hrs)</div>
                            <canvas id="chartJave"></canvas>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Panel 2: Trámite Aduanero -->
                <asp:Panel ID="pnlTramiteAduanero" runat="server" CssClass="tab-content" Visible="false">
                    <div class="dashboard-row">
                        <div class="kpi-card">
                            <div class="kpi-title">TOTAL PROM.</div>
                            <div class="kpi-value">
                                <asp:Literal ID="litTotalPromDepsa" runat="server">0</asp:Literal>
                            </div>
                        </div>
                        <div class="kpi-card">
                            <div class="kpi-title">TOTAL PROM.</div>
                            <div class="kpi-value">
                                <asp:Literal ID="litTotalPromComplex" runat="server">0</asp:Literal>
                            </div>
                        </div>
                    </div>

                    <div class="chart-row">
                        <div class="chart-container half">
                            <div class="chart-title">ESPERA PARA INGRESAR A DEPSA (HRS)</div>
                            <canvas id="chartEsperaDepsa"></canvas>
                        </div>
                        <div class="chart-container half">
                            <div class="chart-title">ESPERA PARA INGRESAR A COMPLEX (HRS)</div>
                            <canvas id="chartEsperaComplex"></canvas>
                        </div>
                    </div>

                    <div class="chart-row">
                        <div class="chart-container half">
                            <div class="chart-title">TIEMPO PROMEDIO EN DEPSA</div>
                            <canvas id="chartTiempoDepsa"></canvas>
                        </div>
                        <div class="chart-container half">
                            <div class="chart-title">TIEMPO PROMEDIO EN COMPLEX</div>
                            <canvas id="chartTiempoComplex"></canvas>
                        </div>
                    </div>

                    <div class="chart-row">
                        <div class="chart-container full">
                            <div class="chart-title">TIEMPO PROMEDIO EN CEBAF (MIN)</div>
                            <canvas id="chartTiempoCebaf"></canvas>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Panel 3: Tiempos Adicionales -->
                <asp:Panel ID="pnlTiemposAdicionales" runat="server" CssClass="tab-content" Visible="false">
                    <div class="dashboard-row">
                        <div class="kpi-card">
                            <div class="kpi-title">TOTAL PROM.</div>
                            <div class="kpi-value">
                                <asp:Literal ID="litTotalPromTCI" runat="server">0</asp:Literal>
                            </div>
                        </div>
                        <div class="kpi-card">
                            <div class="kpi-title">TOTAL PROM.</div>
                            <div class="kpi-value">
                                <asp:Literal ID="litTotalPromPuyango" runat="server">0</asp:Literal>
                            </div>
                        </div>
                    </div>

                    <div class="chart-row">
                        <div class="chart-container half">
                            <div class="chart-title">TIEMPO PROMEDIO EN TCI (HRS)</div>
                            <canvas id="chartTiempoTCI"></canvas>
                        </div>
                        <div class="chart-container half">
                            <div class="chart-title">TIEMPO PROMEDIO EN PUYANGO (HRS)</div>
                            <canvas id="chartTiempoPuyango"></canvas>
                        </div>
                    </div>

                    <div class="chart-row">
                        <div class="chart-container full">
                            <div class="chart-title">ESPERA DE NACIONALIZACIÓN (HRS)</div>
                            <canvas id="chartEsperaNacionalizacion"></canvas>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Panel 4: Bodegas y Distancias -->
                <asp:Panel ID="pnlBodegasDistancias" runat="server" CssClass="tab-content" Visible="false">
                    <div class="chart-row">
                        <div class="chart-container half">
                            <div class="chart-title">TIEMPO PROMEDIO DE BODEGA NACIONAL A INBALNOR (HRS)</div>
                            <canvas id="chartBodegaInbalnor"></canvas>
                        </div>
                        <div class="chart-container half">
                            <div class="chart-title">TIEMPO PROMEDIO DE BODEGA NACIONAL A JAVE (HRS)</div>
                            <canvas id="chartBodegaJave"></canvas>
                        </div>
                    </div>

                    <div class="chart-row">
                        <div class="chart-container half">
                            <div class="chart-title">TIEMPO PROMEDIO DE BODEGA ECUATORIANA A BODEGA INBALNOR (HRS)</div>
                            <canvas id="chartEcuatorianaInbalnor"></canvas>
                        </div>
                        <div class="chart-container half">
                            <div class="chart-title">TIEMPO PROMEDIO DE BODEGA ECUATORIANA A BODEGA JAVE (HRS)</div>
                            <canvas id="chartEcuatorianaJave"></canvas>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Div oculto para almacenar datos JSON para los gráficos -->
                <asp:HiddenField ID="hdnDatosGraficos" runat="server" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnFiltrar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="tmrRefresh" EventName="Tick" />
            <asp:AsyncPostBackTrigger ControlID="lnkGeneral" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkTramiteAduanero" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkTiemposAdicionales" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkBodegasDistancias" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <style>
        * {
            box-sizing: border-box;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        body {
            background-color: #121212;
            color: white;
            overflow-x: hidden;
        }

        .dashboard-container {
            width: 100%;
            padding: 20px;
        }

        .dashboard-header {
            background: linear-gradient(to right, #121212, #1e1e1e);
            padding: 15px 20px;
            margin-bottom: 20px;
            border-bottom: 1px solid #333;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .dashboard-title {
            font-size: 28px;
            font-weight: 600;
            color: white;
            text-shadow: 0 0 10px rgba(255, 255, 255, 0.2);
        }

        .refresh-btn {
            background-color: #0078d7;
            color: white;
            border: none;
            padding: 8px 15px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
            transition: background-color 0.3s;
        }

            .refresh-btn:hover {
                background-color: #005ca3;
            }

        .tabs-container {
            display: flex;
            margin-bottom: 20px;
            border-bottom: 1px solid #333;
        }

        .tab {
            padding: 10px 20px;
            background-color: transparent;
            color: #999;
            border: none;
            cursor: pointer;
            font-size: 16px;
            transition: all 0.3s;
            text-decoration: none;
        }

            .tab.active {
                color: white;
                border-bottom: 3px solid #0078d7;
            }

        .filter-dropdown {
            background-color: #333;
            color: white;
            border: none;
            padding: 5px 10px;
            border-radius: 4px;
            margin-right: 10px;
        }

        .dashboard-row {
            display: flex;
            gap: 20px;
            margin-bottom: 20px;
            flex-wrap: wrap;
        }

        .kpi-card {
            background-color: #1e1e1e;
            border-radius: 4px;
            flex: 1;
            min-width: 200px;
            padding: 15px;
            text-align: center;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

        .kpi-title {
            font-size: 14px;
            color: #999;
            margin-bottom: 10px;
        }

        .kpi-value {
            font-size: 28px;
            font-weight: 700;
            color: white;
        }

        .chart-container {
            background-color: #1e1e1e;
            border-radius: 4px;
            padding: 15px;
            margin-bottom: 20px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            height: 300px;
        }

            .chart-container.half {
                width: calc(50% - 10px);
            }

            .chart-container.full {
                width: 100%;
            }

        .chart-title {
            font-size: 16px;
            color: white;
            margin-bottom: 15px;
            font-weight: 500;
        }

        .date-filter {
            display: flex;
            gap: 10px;
            align-items: center;
            margin-bottom: 20px;
        }

            .date-filter label {
                color: #999;
                font-size: 14px;
            }

        canvas {
            width: 100% !important;
            height: 250px !important;
        }

        .chart-row {
            display: flex;
            gap: 20px;
            flex-wrap: wrap;
        }

        @media (max-width: 768px) {
            .chart-container.half {
                width: 100%;
            }

            .dashboard-row {
                flex-direction: column;
            }
        }

        .tab-content {
            display: block;
        }

        /* Loader */
        .loader {
            border: 5px solid #333;
            border-top: 5px solid #0078d7;
            border-radius: 50%;
            width: 40px;
            height: 40px;
            animation: spin 1s linear infinite;
            margin: 20px auto;
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsSection" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js@3.7.1/dist/chart.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels@2.0.0"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/js/all.min.js"></script>

    <script type="text/javascript">
        // Todos los gráficos inicializados
        var charts = {};

        // Inicializar todos los gráficos después de un postback
        function pageLoad() {
            try {
                console.log("Iniciando carga de página...");

                // Obtener el elemento oculto - NOTA: usar la referencia dinámica correcta
                var hiddenField = document.getElementById('<%= hdnDatosGraficos.ClientID %>');

                if (!hiddenField || !hiddenField.value) {
                    console.error("Campo oculto no encontrado o vacío");
                    return;
                }

                // Pre-procesar la cadena JSON para eliminar caracteres problemáticos
                var rawValue = hiddenField.value;
                console.log("Valor original:", rawValue.substring(0, 100) + "..."); // Muestra los primeros 100 caracteres

                try {
                    // Intenta parsear directamente
                    var datosJSON = JSON.parse(rawValue);
                    console.log("JSON parseado correctamente");
                    inicializarGraficos(datosJSON);
                } catch (parseError) {
                    console.error("Error en primer intento de parseo:", parseError);

                    // Si falla, intenta limpiar la cadena y volver a intentar
                    try {
                        // Reemplazar caracteres problemáticos y volver a intentar
                        var cleanedValue = rawValue
                            .replace(/\n/g, "\\n")
                            .replace(/\r/g, "\\r")
                            .replace(/\t/g, "\\t")
                            .replace(/\f/g, "\\f");

                        var datosJSON = JSON.parse(cleanedValue);
                        console.log("JSON parseado después de limpieza");
                        inicializarGraficos(datosJSON);
                    } catch (finalError) {
                        console.error("Error final en parseo:", finalError);
                        alert("No se pudieron cargar los datos. Por favor intente de nuevo.");
                    }
                }
            } catch (e) {
                console.error("Error general:", e);
                alert("Ocurrió un error: " + e.message);
            }
        }

        // Inicializar gráficos con los datos recibidos
        function inicializarGraficos(datos) {
            // Configuración común para todos los gráficos
            const commonOptions = {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'right',
                        labels: { color: '#fff', font: { size: 12 } }
                    },
                    tooltip: { mode: 'index', intersect: false }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: { color: 'rgba(255, 255, 255, 0.1)' },
                        ticks: { color: '#fff' }
                    },
                    x: {
                        grid: { color: 'rgba(255, 255, 255, 0.1)' },
                        ticks: { color: '#fff' }
                    }
                }
            };

            // Gráfico Tiempos promedio en Trujillo
            inicializarGraficoTrujillo(datos, commonOptions);

            // Gráfico Tiempo Trujillo-Ecuador
            inicializarGraficoTrujilloEcuador(datos, commonOptions);

            // Gráfico Tiempo Base
            inicializarGraficoBase(datos, commonOptions);

            // Gráfico Inbalnor
            inicializarGraficoInbalnor(datos, commonOptions);

            // Gráfico Jave
            inicializarGraficoJave(datos, commonOptions);

            // Inicializar otros gráficos según la pestaña visible
            if (document.getElementById('chartEsperaDepsa')) {
                inicializarGraficoEsperaDepsa(datos, commonOptions);
                inicializarGraficoEsperaComplex(datos, commonOptions);
                inicializarGraficoTiempoDepsa(datos, commonOptions);
                inicializarGraficoTiempoComplex(datos, commonOptions);
                inicializarGraficoTiempoCebaf(datos, commonOptions);
            }

            // Más inicializaciones para otros gráficos según sea necesario
        }

        // Función para inicializar el gráfico de Tiempos promedio en Trujillo
        function inicializarGraficoTrujillo(datos, commonOptions) {
            const ctxTrujillo = document.getElementById('chartTrujillo');
            if (!ctxTrujillo) return;

            // Destruir gráfico existente si lo hay
            if (charts.chartTrujillo) {
                charts.chartTrujillo.destroy();
            }

            charts.chartTrujillo = new Chart(ctxTrujillo, {
                type: 'bar',
                data: {
                    labels: datos.MesesTrujillo || [],
                    datasets: [
                        {
                            label: 'Espera a ingreso Trujillo',
                            data: datos.EsperaIngresoTrujillo || [],
                            backgroundColor: 'rgba(255, 99, 132, 0.7)',
                            borderColor: 'rgba(255, 99, 132, 1)',
                            borderWidth: 1
                        },
                        {
                            label: 'Espera inicio',
                            data: datos.EsperaInicio || [],
                            backgroundColor: 'rgba(255, 159, 64, 0.7)',
                            borderColor: 'rgba(255, 159, 64, 1)',
                            borderWidth: 1
                        },
                        {
                            label: 'Carga',
                            data: datos.Carga || [],
                            backgroundColor: 'rgba(255, 205, 86, 0.7)',
                            borderColor: 'rgba(255, 205, 86, 1)',
                            borderWidth: 1
                        },
                        {
                            label: 'Permanencia en planta Trujillo',
                            data: datos.PermanenciaTrujillo || [],
                            backgroundColor: 'rgba(75, 192, 192, 0.7)',
                            borderColor: 'rgba(75, 192, 192, 1)',
                            borderWidth: 1
                        }
                    ]
                },
                options: commonOptions
            });
        }

        // Función para inicializar el gráfico de Tiempo Trujillo-Ecuador
        function inicializarGraficoTrujilloEcuador(datos, commonOptions) {
            const ctx = document.getElementById('chartTrujilloEcuador');
            if (!ctx) return;

            if (charts.chartTrujilloEcuador) {
                charts.chartTrujilloEcuador.destroy();
            }

            charts.chartTrujilloEcuador = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: datos.MesesTrujilloEcuador || [],
                    datasets: [
                        {
                            label: 'Días',
                            data: datos.TiempoTrujilloEcuador || [],
                            backgroundColor: 'rgba(54, 162, 235, 0.7)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 1
                        }
                    ]
                },
                options: commonOptions
            });
        }

        // Función para inicializar el gráfico de Tiempo Base
        function inicializarGraficoBase(datos, commonOptions) {
            const ctx = document.getElementById('chartBase');
            if (!ctx) return;

            if (charts.chartBase) {
                charts.chartBase.destroy();
            }

            charts.chartBase = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: datos.MesesBase || [],
                    datasets: [
                        {
                            label: 'Horas',
                            data: datos.TiempoBase || [],
                            backgroundColor: 'rgba(54, 162, 235, 0.7)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 1
                        }
                    ]
                },
                options: commonOptions
            });
        }

        // Función para inicializar el gráfico de Inbalnor
        function inicializarGraficoInbalnor(datos, commonOptions) {
            const ctx = document.getElementById('chartInbalnor');
            if (!ctx) return;

            if (charts.chartInbalnor) {
                charts.chartInbalnor.destroy();
            }

            charts.chartInbalnor = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: datos.DiasInbalnor || [],
                    datasets: [
                        {
                            label: 'Espera descarga',
                            data: datos.EsperaDescargaInbalnor || [],
                            backgroundColor: 'rgba(255, 99, 132, 0.7)',
                            borderColor: 'rgba(255, 99, 132, 1)',
                            borderWidth: 1
                        },
                        {
                            label: 'Descarga',
                            data: datos.DescargaInbalnor || [],
                            backgroundColor: 'rgba(255, 159, 64, 0.7)',
                            borderColor: 'rgba(255, 159, 64, 1)',
                            borderWidth: 1
                        }
                    ]
                },
                options: commonOptions
            });
        }

        // Función para inicializar el gráfico de Jave
        function inicializarGraficoJave(datos, commonOptions) {
            const ctx = document.getElementById('chartJave');
            if (!ctx) return;

            if (charts.chartJave) {
                charts.chartJave.destroy();
            }

            charts.chartJave = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: datos.DiasJave || [],
                    datasets: [
                        {
                            label: 'Espera descarga',
                            data: datos.EsperaDescargaJave || [],
                            backgroundColor: 'rgba(255, 99, 132, 0.7)',
                            borderColor: 'rgba(255, 99, 132, 1)',
                            borderWidth: 1
                        },
                        {
                            label: 'Descarga',
                            data: datos.DescargaJave || [],
                            backgroundColor: 'rgba(255, 159, 64, 0.7)',
                            borderColor: 'rgba(255, 159, 64, 1)',
                            borderWidth: 1
                        }
                    ]
                },
                options: commonOptions
            });
        }

        // Función para inicializar el gráfico de Espera Depsa
        function inicializarGraficoEsperaDepsa(datos, commonOptions) {
            const ctx = document.getElementById('chartEsperaDepsa');
            if (!ctx) return;

            if (charts.chartEsperaDepsa) {
                charts.chartEsperaDepsa.destroy();
            }

            charts.chartEsperaDepsa = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: datos.DiasEsperaDepsa || [],
                    datasets: [
                        {
                            label: 'Horas',
                            data: datos.EsperaDepsa || [],
                            backgroundColor: 'rgba(54, 162, 235, 0.2)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 2,
                            fill: true,
                            tension: 0.4
                        }
                    ]
                },
                options: commonOptions
            });
        }

        // Inicializaciones para los demás gráficos (similar a las anteriores)
        function inicializarGraficoEsperaComplex(datos, commonOptions) {
            const ctx = document.getElementById('chartEsperaComplex');
            if (!ctx) return;

            if (charts.chartEsperaComplex) {
                charts.chartEsperaComplex.destroy();
            }

            charts.chartEsperaComplex = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: datos.DiasEsperaComplex || [],
                    datasets: [
                        {
                            label: 'Horas',
                            data: datos.EsperaComplex || [],
                            backgroundColor: 'rgba(54, 162, 235, 0.2)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 2,
                            fill: true,
                            tension: 0.4
                        }
                    ]
                },
                options: commonOptions
            });
        }

        function inicializarGraficoTiempoDepsa(datos, commonOptions) {
            const ctx = document.getElementById('chartTiempoDepsa');
            if (!ctx) return;

            if (charts.chartTiempoDepsa) {
                charts.chartTiempoDepsa.destroy();
            }

            charts.chartTiempoDepsa = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: datos.DiasTiempoDepsa || [],
                    datasets: [
                        {
                            label: 'Horas',
                            data: datos.TiempoDepsa || [],
                            backgroundColor: 'rgba(255, 99, 132, 0.2)',
                            borderColor: 'rgba(255, 99, 132, 1)',
                            borderWidth: 2,
                            fill: true,
                            tension: 0.4
                        }
                    ]
                },
                options: commonOptions
            });
        }

        function inicializarGraficoTiempoComplex(datos, commonOptions) {
            const ctx = document.getElementById('chartTiempoComplex');
            if (!ctx) return;

            if (charts.chartTiempoComplex) {
                charts.chartTiempoComplex.destroy();
            }

            charts.chartTiempoComplex = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: datos.DiasTiempoComplex || [],
                    datasets: [
                        {
                            label: 'Horas',
                            data: datos.TiempoComplex || [],
                            backgroundColor: 'rgba(255, 99, 132, 0.2)',
                            borderColor: 'rgba(255, 99, 132, 1)',
                            borderWidth: 2,
                            fill: true,
                            tension: 0.4
                        }
                    ]
                },
                options: commonOptions
            });
        }

        function inicializarGraficoTiempoCebaf(datos, commonOptions) {
            const ctx = document.getElementById('chartTiempoCebaf');
            if (!ctx) return;

            if (charts.chartTiempoCebaf) {
                charts.chartTiempoCebaf.destroy();
            }

            charts.chartTiempoCebaf = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: datos.DiasCebaf || [],
                    datasets: [
                        {
                            label: 'Minutos',
                            data: datos.TiempoCebaf || [],
                            backgroundColor: 'rgba(153, 102, 255, 0.2)',
                            borderColor: 'rgba(153, 102, 255, 1)',
                            borderWidth: 2,
                            fill: true,
                            tension: 0.4
                        }
                    ]
                },
                options: commonOptions
            });
        }
    </script>
</asp:Content>
