<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="WebSGV.Views.Reportes" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

        <div class="row">
            <!-- Panel lateral de selección de reportes -->
            <div class="col-md-3">
                <div class="card shadow-sm mb-4">
                    <div class="card-body">
                        <h5 class="card-title mb-3">
                            <i class="fas fa-file-alt text-primary mr-2"></i>
                            Tipos de Reportes
                        </h5>

                        <div class="list-group">
                            <asp:LinkButton ID="lnkConductor" runat="server" CssClass="list-group-item list-group-item-action d-flex align-items-center active" OnClick="lnkTipoReporte_Click" CommandArgument="conductor">
                                <i class="fas fa-user mr-2"></i> Reportes por Conductor
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkVehiculo" runat="server" CssClass="list-group-item list-group-item-action d-flex align-items-center" OnClick="lnkTipoReporte_Click" CommandArgument="vehiculo">
                                <i class="fas fa-truck mr-2"></i> Reportes por Vehículo
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkRuta" runat="server" CssClass="list-group-item list-group-item-action d-flex align-items-center" OnClick="lnkTipoReporte_Click" CommandArgument="ruta">
                                <i class="fas fa-map mr-2"></i> Reportes por Ruta
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkFinanciero" runat="server" CssClass="list-group-item list-group-item-action d-flex align-items-center" OnClick="lnkTipoReporte_Click" CommandArgument="financiero">
                                <i class="fas fa-dollar-sign mr-2"></i> Reportes Financieros
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkCombustible" runat="server" CssClass="list-group-item list-group-item-action d-flex align-items-center" OnClick="lnkTipoReporte_Click" CommandArgument="combustible">
                                <i class="fas fa-chart-line mr-2"></i> Reportes de Combustible
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Contenido principal -->
            <div class="col-md-9">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center mb-4">
                            <h4 class="card-title font-weight-bold mb-0">
                                <asp:Literal ID="litTituloReporte" runat="server" Text="Reportes por Conductor"></asp:Literal>
                            </h4>

                            <div>
                                <button type="button" class="btn btn-primary mr-2" data-toggle="modal" data-target="#filtrosAvanzadosModal">
                                    <i class="fas fa-filter mr-1"></i>Filtros Avanzados
                                </button>
                                <asp:Button ID="btnExportarExcel" runat="server" CssClass="btn btn-success" Text="Exportar a Excel" OnClick="btnExportarExcel_Click" />
                            </div>
                        </div>

                        <!-- Filtros -->
                        <div class="bg-light p-3 rounded mb-4">
                            <div class="row">
                                <div class="col-md-3 mb-3">
                                    <div class="form-group">
                                        <label>Fecha Desde</label>
                                        <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3 mb-3">
                                    <div class="form-group">
                                        <label>Fecha Hasta</label>
                                        <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-3 mb-3">
                                    <asp:Panel ID="pnlFiltroConductor" runat="server" CssClass="form-group">
                                        <label>Conductor</label>
                                        <asp:DropDownList ID="ddlConductor" runat="server" CssClass="form-control" DataTextField="NombreCompleto" DataValueField="idConductor">
                                            <asp:ListItem Value="" Text="Todos los conductores" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlFiltroVehiculo" runat="server" CssClass="form-group" Visible="false">
                                        <label>Vehículo</label>
                                        <asp:DropDownList ID="ddlVehiculo" runat="server" CssClass="form-control" DataTextField="placaTracto" DataValueField="idTracto">
                                            <asp:ListItem Value="" Text="Todos los vehículos" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlFiltroRuta" runat="server" CssClass="form-group" Visible="false">
                                        <label>Ruta</label>
                                        <asp:DropDownList ID="ddlRuta" runat="server" CssClass="form-control" DataTextField="nombre" DataValueField="idRuta">
                                            <asp:ListItem Value="" Text="Todas las rutas" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:Panel>
                                </div>

                                <div class="col-md-3 mb-3">
                                    <div class="form-group">
                                        <label>Tipo de Reporte</label>
                                        <asp:DropDownList ID="ddlTipoReporteDetalle" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoReporteDetalle_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="text-right">
                                <asp:Button ID="btnGenerarReporte" runat="server" CssClass="btn btn-primary" Text="Generar Reporte" OnClick="btnGenerarReporte_Click" />
                            </div>
                        </div>

                        <!-- Visualización del reporte -->
                        <asp:Panel ID="pnlResultados" runat="server" Visible="false">
                            <h5 class="mb-3 font-weight-bold">
                                <asp:Literal ID="litTituloResultados" runat="server"></asp:Literal>
                            </h5>

                            <!-- Panel para gráfico -->
                            <div class="border rounded p-3 mb-4">
                                <h6 class="mb-3">
                                    <asp:Literal ID="litTituloGrafico" runat="server" Text="Ingresos vs Gastos (Últimos 6 meses)"></asp:Literal>
                                </h6>

                                <div style="height: 350px;">
                                    <asp:Chart ID="chartReporte" runat="server" Height="300px" Width="600px" BackColor="Transparent">
                                        <Series>
                                            <asp:Series Name="Ingresos" Color="#28a745" IsValueShownAsLabel="false"></asp:Series>
                                            <asp:Series Name="Gastos" Color="#dc3545" IsValueShownAsLabel="false"></asp:Series>
                                        </Series>
                                        <ChartAreas>
                                            <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                                                <AxisX Interval="1"></AxisX>
                                                <AxisY Title="Valores (S/)" TitleFont="Microsoft Sans Serif, 10pt"></AxisY>
                                            </asp:ChartArea>
                                        </ChartAreas>
                                        <Legends>
                                            <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom"></asp:Legend>
                                        </Legends>
                                    </asp:Chart>
                                </div>
                            </div>

                            <!-- Tabla de resultados -->
                            <div class="table-responsive">
                                <asp:GridView ID="gvReporte" runat="server" CssClass="table table-striped table-bordered"
                                    AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                                    PageSize="10" OnPageIndexChanging="gvReporte_PageIndexChanging"
                                    OnSorting="gvReporte_Sorting">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                                        <%-- Las demás columnas se generarán dinámicamente en el code-behind --%>
                                    </Columns>
                                    <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                                    <HeaderStyle CssClass="bg-light" />
                                    <FooterStyle CssClass="bg-light font-weight-bold" />
                                    <EmptyDataTemplate>
                                        <div class="alert alert-info text-center">
                                            No se encontraron datos para los criterios seleccionados.
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal de Filtros Avanzados -->
    <div class="modal fade" id="filtrosAvanzadosModal" tabindex="-1" role="dialog" aria-labelledby="filtrosAvanzadosModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="filtrosAvanzadosModalLabel">Filtros Avanzados</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <!-- Filtros para reportes de conductor -->
                        <asp:Panel ID="pnlFiltrosAvanzadosConductor" runat="server" CssClass="col-12">
                            <div class="form-group">
                                <label>Tipo de Licencia</label>
                                <asp:DropDownList ID="ddlTipoLicencia" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Todas" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="A-III" Text="A-III"></asp:ListItem>
                                    <asp:ListItem Value="A-IIIc" Text="A-IIIc"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="form-group">
                                <label>Experiencia Mínima (años)</label>
                                <asp:TextBox ID="txtExperienciaMinima" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                            </div>
                        </asp:Panel>

                        <!-- Filtros para reportes de vehículo -->
                        <asp:Panel ID="pnlFiltrosAvanzadosVehiculo" runat="server" CssClass="col-12" Visible="false">
                            <div class="form-group">
                                <label>Marca</label>
                                <asp:DropDownList ID="ddlMarcaVehiculo" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Todas" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="form-group">
                                <label>Modelo</label>
                                <asp:DropDownList ID="ddlModeloVehiculo" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Todos" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </asp:Panel>

                        <!-- Otros filtros avanzados según el tipo de reporte -->
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    <asp:Button ID="btnAplicarFiltrosAvanzados" runat="server" CssClass="btn btn-primary" Text="Aplicar Filtros" OnClick="btnAplicarFiltrosAvanzados_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Referencias a librerías JavaScript -->
    <script type="text/javascript">
        $(document).ready(function () {
            // Inicialización de componentes
            //$('.selectpicker').selectpicker();

            // Actualizar UI basado en el tipo de reporte seleccionado
            function actualizarInterfazSegunTipoReporte() {
                var tipoReporte = $(".list-group-item.active").data("tipo");

                // Lógica para mostrar/ocultar elementos según el tipo de reporte
                // Esta parte se maneja principalmente desde el servidor con UpdatePanels
            }

            // Inicialización
            actualizarInterfazSegunTipoReporte();
        });
    </script>
</asp:Content>
