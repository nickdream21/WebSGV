using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Web.UI.DataVisualization.Charting;

namespace WebSGV.Views
{
    public partial class Reportes : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Establecer fechas predeterminadas (último mes)
                txtFechaDesde.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtFechaHasta.Text = DateTime.Now.ToString("yyyy-MM-dd");

                // Cargar datos iniciales en los dropdown
                CargarConductores();
                CargarVehiculos();
                CargarPedidos();
                CargarProductos();
                CargarLugaresAbastecimiento();
                CargarTiposReporteDetalle("conductor"); // Por defecto carga los tipos de reporte para conductor
            }
        }

        #region Carga de Datos Iniciales

        private void CargarConductores()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT idConductor, 
                                    CONCAT(nombre, ' ', apPaterno, ' ', apMaterno) as NombreCompleto 
                                    FROM Conductor 
                                    ORDER BY apPaterno, apMaterno, nombre";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ddlConductor.DataSource = dt;
                    ddlConductor.DataBind();

                    // Agregar el item por defecto
                    ddlConductor.Items.Insert(0, new ListItem("Todos los conductores", ""));
                }
            }
            catch (Exception ex)
            {
                // En caso de error, simplemente asegurarse de que el dropdown tenga al menos un item
                if (ddlConductor.Items.Count == 0)
                    ddlConductor.Items.Add(new ListItem("Todos los conductores", ""));

                // En un entorno de producción, se debería registrar este error
                System.Diagnostics.Debug.WriteLine("Error al cargar conductores: " + ex.Message);
            }
        }

        private void CargarVehiculos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT idTracto, placaTracto, marca, modelo 
                                  FROM Tracto 
                                  ORDER BY placaTracto";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ddlVehiculo.DataSource = dt;
                    ddlVehiculo.DataBind();

                    // Agregar el item por defecto
                    ddlVehiculo.Items.Insert(0, new ListItem("Todos los vehículos", ""));
                }
            }
            catch (Exception ex)
            {
                // En caso de error, simplemente asegurarse de que el dropdown tenga al menos un item
                if (ddlVehiculo.Items.Count == 0)
                    ddlVehiculo.Items.Add(new ListItem("Todos los vehículos", ""));

                System.Diagnostics.Debug.WriteLine("Error al cargar vehículos: " + ex.Message);
            }
        }

        private void CargarPedidos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT idCPIC, numeroCPIC 
                                  FROM CPIC 
                                  ORDER BY fechaEmision DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ddlCPIC.DataSource = dt;
                    ddlCPIC.DataBind();

                    // Agregar el item por defecto
                    ddlCPIC.Items.Insert(0, new ListItem("Todos los pedidos", ""));
                }
            }
            catch (Exception ex)
            {
                // En caso de error, simplemente asegurarse de que el dropdown tenga al menos un item
                if (ddlCPIC.Items.Count == 0)
                    ddlCPIC.Items.Add(new ListItem("Todos los pedidos", ""));

                System.Diagnostics.Debug.WriteLine("Error al cargar pedidos: " + ex.Message);
            }
        }

        private void CargarProductos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT idProducto, nombre 
                                  FROM Producto 
                                  ORDER BY nombre";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ddlProducto.DataSource = dt;
                    ddlProducto.DataBind();

                    // Agregar el item por defecto
                    ddlProducto.Items.Insert(0, new ListItem("Todos los productos", ""));
                }
            }
            catch (Exception ex)
            {
                // En caso de error, simplemente asegurarse de que el dropdown tenga al menos un item
                if (ddlProducto.Items.Count == 0)
                    ddlProducto.Items.Add(new ListItem("Todos los productos", ""));

                System.Diagnostics.Debug.WriteLine("Error al cargar productos: " + ex.Message);
            }
        }

        private void CargarLugaresAbastecimiento()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT idLugarAbastecimiento, nombreAbastecimiento 
                                  FROM LugarAbastecimiento 
                                  ORDER BY nombreAbastecimiento";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ddlLugarAbastecimiento.DataSource = dt;
                    ddlLugarAbastecimiento.DataBind();

                    // Agregar el item por defecto
                    ddlLugarAbastecimiento.Items.Insert(0, new ListItem("Todos los lugares", ""));
                }
            }
            catch (Exception ex)
            {
                // En caso de error, simplemente asegurarse de que el dropdown tenga al menos un item
                if (ddlLugarAbastecimiento.Items.Count == 0)
                    ddlLugarAbastecimiento.Items.Add(new ListItem("Todos los lugares", ""));

                System.Diagnostics.Debug.WriteLine("Error al cargar lugares de abastecimiento: " + ex.Message);
            }
        }

        private void CargarTiposReporteDetalle(string tipoReporte)
        {
            ddlTipoReporteDetalle.Items.Clear();

            switch (tipoReporte)
            {
                case "conductor":
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Viajes realizados", "viajes_conductor"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Productos transportados", "productos_conductor"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Ingresos y gastos", "financiero_conductor"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Rendimiento de combustible", "combustible_conductor"));
                    break;

                case "vehiculo":
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Historial de viajes", "viajes_vehiculo"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Consumo de combustible", "combustible_vehiculo"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Rendimiento por ruta", "rendimiento_vehiculo"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Gastos de mantenimiento", "mantenimiento_vehiculo"));
                    break;

                case "pedido":
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Detalle de pedido", "detalle_pedido"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Vehículos asignados", "vehiculos_pedido"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Conductores asignados", "conductores_pedido"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Balance financiero", "financiero_pedido"));
                    break;

                case "financiero":
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Balance general", "balance_financiero"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Ingresos detallados", "ingresos_detalle"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Egresos detallados", "egresos_detalle"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Análisis comparativo", "comparativo_financiero"));
                    break;

                case "combustible":
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Consumo general", "consumo_combustible"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Rendimiento por vehículo", "rendimiento_combustible"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Rendimiento por ruta", "ruta_combustible"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Análisis de sobrantes", "sobrante_combustible"));
                    break;

                case "producto":
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Productos más transportados", "ranking_producto"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Productos por cliente", "cliente_producto"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Productos por destino", "destino_producto"));
                    break;

                case "personalizado":
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Reporte dinámico", "dinamico_personalizado"));
                    pnlFiltrosPersonalizados.Visible = true;
                    break;

                default:
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Listado general", "listado_general"));
                    break;
            }
        }

        #endregion

        #region Eventos de Navegación y UI

        protected void lnkTipoReporte_Click(object sender, EventArgs e)
        {
            // Obtener el tipo de reporte seleccionado desde el CommandArgument
            LinkButton lnkButton = (LinkButton)sender;
            string tipoReporte = lnkButton.CommandArgument;

            // Actualizar título del reporte
            switch (tipoReporte)
            {
                case "conductor":
                    litTituloReporte.Text = "Reportes por Conductor";
                    break;
                case "vehiculo":
                    litTituloReporte.Text = "Reportes por Vehículo";
                    break;
                case "pedido":
                    litTituloReporte.Text = "Reportes por Pedido";
                    break;
                case "financiero":
                    litTituloReporte.Text = "Reportes Financieros";
                    break;
                case "combustible":
                    litTituloReporte.Text = "Reportes de Combustible";
                    break;
                case "producto":
                    litTituloReporte.Text = "Reportes por Producto";
                    break;
                case "personalizado":
                    litTituloReporte.Text = "Reporte Personalizado";
                    break;
            }

            // Cambiar clase active en los botones de navegación
            lnkConductor.CssClass = lnkConductor.CssClass.Replace(" active", "");
            lnkVehiculo.CssClass = lnkVehiculo.CssClass.Replace(" active", "");
            lnkPedido.CssClass = lnkPedido.CssClass.Replace(" active", "");
            lnkFinanciero.CssClass = lnkFinanciero.CssClass.Replace(" active", "");
            lnkCombustible.CssClass = lnkCombustible.CssClass.Replace(" active", "");
            lnkProducto.CssClass = lnkProducto.CssClass.Replace(" active", "");
            lnkPersonalizado.CssClass = lnkPersonalizado.CssClass.Replace(" active", "");

            lnkButton.CssClass += " active";

            // Mostrar/Ocultar los filtros específicos según el tipo de reporte
            pnlFiltroConductor.Visible = (tipoReporte == "conductor");
            pnlFiltroVehiculo.Visible = (tipoReporte == "vehiculo");
            pnlFiltroPedido.Visible = (tipoReporte == "pedido");
            pnlFiltroFinanciero.Visible = (tipoReporte == "financiero");
            pnlFiltroCombustible.Visible = (tipoReporte == "combustible");
            pnlFiltroProducto.Visible = (tipoReporte == "producto");
            pnlFiltrosPersonalizados.Visible = (tipoReporte == "personalizado");

            // Mostrar/Ocultar los paneles de filtros avanzados correspondientes
            pnlFiltrosAvanzadosConductor.Visible = (tipoReporte == "conductor");
            pnlFiltrosAvanzadosVehiculo.Visible = (tipoReporte == "vehiculo");
            pnlFiltrosAvanzadosPedido.Visible = (tipoReporte == "pedido");
            pnlFiltrosAvanzadosFinanciero.Visible = (tipoReporte == "financiero");
            pnlFiltrosAvanzadosCombustible.Visible = (tipoReporte == "combustible");
            pnlFiltrosAvanzadosProducto.Visible = (tipoReporte == "producto");
            pnlFiltrosAvanzadosPersonalizado.Visible = (tipoReporte == "personalizado");

            // Cargar los tipos de reportes detallados
            CargarTiposReporteDetalle(tipoReporte);

            // Resetear los resultados
            pnlResultados.Visible = false;
        }

        protected void ddlTipoReporteDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Actualizar UI según el tipo de reporte detallado seleccionado
            // Esta función se amplía según las necesidades específicas de cada tipo de reporte
        }

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            // Limpiar todos los filtros
            txtFechaDesde.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtFechaHasta.Text = DateTime.Now.ToString("yyyy-MM-dd");

            // Restablecer dropdowns a su valor inicial (primer item)
            if (ddlConductor.Items.Count > 0) ddlConductor.SelectedIndex = 0;
            if (ddlVehiculo.Items.Count > 0) ddlVehiculo.SelectedIndex = 0;
            if (ddlCPIC.Items.Count > 0) ddlCPIC.SelectedIndex = 0;
            if (ddlTipoTransaccion.Items.Count > 0) ddlTipoTransaccion.SelectedIndex = 0;
            if (ddlLugarAbastecimiento.Items.Count > 0) ddlLugarAbastecimiento.SelectedIndex = 0;
            if (ddlProducto.Items.Count > 0) ddlProducto.SelectedIndex = 0;

            // Limpiar filtros avanzados
            txtDNIConductor.Text = "";
            txtNombreConductor.Text = "";
            txtPlacaVehiculo.Text = "";

            if (ddlMarcaVehiculo.Items.Count > 0) ddlMarcaVehiculo.SelectedIndex = 0;
            if (ddlModeloVehiculo.Items.Count > 0) ddlModeloVehiculo.SelectedIndex = 0;

            txtNumeroFactura.Text = "";
            txtValorMinimo.Text = "";
            txtValorMaximo.Text = "";

            if (ddlMoneda.Items.Count > 0) ddlMoneda.SelectedIndex = 0;

            txtMontoMinimo.Text = "";
            txtMontoMaximo.Text = "";
            txtProductoCombustible.Text = "";
            txtNumeroAbastecimiento.Text = "";
            txtGalonesMinimos.Text = "";
            txtRendimientoMinimo.Text = "";

            // Limpiar filtros personalizados
            if (pnlFiltrosPersonalizados.Visible)
            {
                chkConductorInfo.Checked = false;
                chkVehiculoInfo.Checked = false;
                chkRutaInfo.Checked = false;
                chkProductoInfo.Checked = false;
                chkIngresoInfo.Checked = false;
                chkEgresoInfo.Checked = false;
                chkCombustibleInfo.Checked = false;
                chkClienteInfo.Checked = false;
                chkFacturaInfo.Checked = false;

                if (ddlAgrupamiento.Items.Count > 0) ddlAgrupamiento.SelectedIndex = 0;
                if (ddlOrdenamiento.Items.Count > 0) ddlOrdenamiento.SelectedIndex = 0;
            }

            // Ocultar resultados
            pnlResultados.Visible = false;
        }

        protected void btnAplicarFiltrosAvanzados_Click(object sender, EventArgs e)
        {
            // Esta función simplemente cierra el modal de filtros avanzados
            // La aplicación de los filtros ocurre al generar el reporte
        }

        protected void gvReporte_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReporte.PageIndex = e.NewPageIndex;
            GenerarReporte(); // Volver a cargar los datos con la página actualizada
        }

        protected void gvReporte_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Implementar la lógica de ordenamiento básica
            ViewState["SortExpression"] = e.SortExpression;
            ViewState["SortDirection"] =
                ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() == e.SortExpression &&
                ViewState["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";

            GenerarReporte(); // Volver a cargar los datos con el ordenamiento actualizado
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            // Implementación básica para exportar a Excel
            // Esta función se ampliará cuando los reportes estén operativos
            Response.Write("<script>alert('Función de exportación a Excel en desarrollo.');</script>");
        }

        protected void btnExportarPDF_Click(object sender, EventArgs e)
        {
            // Implementación básica para exportar a PDF
            // Esta función se ampliará cuando los reportes estén operativos
            Response.Write("<script>alert('Función de exportación a PDF en desarrollo.');</script>");
        }

        #endregion

        #region Generación de Reportes

        protected void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            try
            {
                GenerarReporte();
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error genérico
                Response.Write("<script>alert('Error al generar el reporte: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }

        private void GenerarReporte()
        {
            // Para esta versión básica, generaremos un reporte de muestra
            // con datos ficticios solo para probar la interfaz
            GenerarReporteDePrueba();

            // Mostrar panel de resultados
            pnlResultados.Visible = true;
        }

        private void GenerarReporteDePrueba()
        {
            // Crear una tabla con datos de muestra para probar la interfaz
            DataTable dt = new DataTable();

            // Agregar columnas según el tipo de reporte seleccionado
            string tipoReporte = ObtenerTipoReporteSeleccionado();
            string tipoReporteDetalle = ddlTipoReporteDetalle.SelectedValue;

            // Columnas básicas que todos los reportes tendrán
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Fecha", typeof(DateTime));

            // Añadir columnas según el tipo de reporte
            switch (tipoReporte)
            {
                case "conductor":
                    dt.Columns.Add("DNI", typeof(string));
                    dt.Columns.Add("Nombre", typeof(string));
                    dt.Columns.Add("Vehículo", typeof(string));
                    dt.Columns.Add("Destino", typeof(string));
                    break;

                case "vehiculo":
                    dt.Columns.Add("Placa", typeof(string));
                    dt.Columns.Add("Marca", typeof(string));
                    dt.Columns.Add("Conductor", typeof(string));
                    dt.Columns.Add("Kilometraje", typeof(decimal));
                    break;

                case "pedido":
                    dt.Columns.Add("Número", typeof(string));
                    dt.Columns.Add("Cliente", typeof(string));
                    dt.Columns.Add("Valor", typeof(decimal));
                    dt.Columns.Add("Estado", typeof(string));
                    break;

                case "financiero":
                    dt.Columns.Add("Concepto", typeof(string));
                    dt.Columns.Add("Ingresos", typeof(decimal));
                    dt.Columns.Add("Egresos", typeof(decimal));
                    dt.Columns.Add("Balance", typeof(decimal));
                    break;

                case "combustible":
                    dt.Columns.Add("Vehículo", typeof(string));
                    dt.Columns.Add("Galones", typeof(decimal));
                    dt.Columns.Add("Costo", typeof(decimal));
                    dt.Columns.Add("Rendimiento", typeof(decimal));
                    break;

                case "producto":
                    dt.Columns.Add("Producto", typeof(string));
                    dt.Columns.Add("Cantidad", typeof(int));
                    dt.Columns.Add("Cliente", typeof(string));
                    dt.Columns.Add("Destino", typeof(string));
                    break;

                case "personalizado":
                    // Para reporte personalizado añadimos varias columnas posibles
                    dt.Columns.Add("Conductor", typeof(string));
                    dt.Columns.Add("Vehículo", typeof(string));
                    dt.Columns.Add("Cliente", typeof(string));
                    dt.Columns.Add("Producto", typeof(string));
                    dt.Columns.Add("Valor", typeof(decimal));
                    break;
            }

            // Añadir filas de datos de muestra
            for (int i = 1; i <= 10; i++)
            {
                DataRow row = dt.NewRow();
                row["ID"] = i;
                row["Fecha"] = DateTime.Now.AddDays(-i);

                switch (tipoReporte)
                {
                    case "conductor":
                        row["DNI"] = "4589" + i.ToString("D4");
                        row["Nombre"] = "Conductor " + i;
                        row["Vehículo"] = "ABC-" + i.ToString("D3");
                        row["Destino"] = "Destino " + i;
                        break;

                    case "vehiculo":
                        row["Placa"] = "ABC-" + i.ToString("D3");
                        row["Marca"] = "Marca " + (i % 3 + 1);
                        row["Conductor"] = "Conductor " + i;
                        row["Kilometraje"] = 10000 + (i * 500);
                        break;

                    case "pedido":
                        row["Número"] = "PED-" + i.ToString("D4");
                        row["Cliente"] = "Cliente " + (i % 5 + 1);
                        row["Valor"] = 1000 * i;
                        row["Estado"] = i % 3 == 0 ? "Pendiente" : "Completado";
                        break;

                    case "financiero":
                        row["Concepto"] = "Concepto " + i;
                        row["Ingresos"] = 2000 * i;
                        row["Egresos"] = 1000 * i;
                        row["Balance"] = 1000 * i;
                        break;

                    case "combustible":
                        row["Vehículo"] = "ABC-" + i.ToString("D3");
                        row["Galones"] = 50 + (i * 2);
                        row["Costo"] = 200 + (i * 10);
                        row["Rendimiento"] = 10 + (i % 5);
                        break;

                    case "producto":
                        row["Producto"] = "Producto " + (i % 4 + 1);
                        row["Cantidad"] = 100 * i;
                        row["Cliente"] = "Cliente " + (i % 5 + 1);
                        row["Destino"] = "Destino " + (i % 3 + 1);
                        break;

                    case "personalizado":
                        row["Conductor"] = "Conductor " + (i % 3 + 1);
                        row["Vehículo"] = "ABC-" + i.ToString("D3");
                        row["Cliente"] = "Cliente " + (i % 4 + 1);
                        row["Producto"] = "Producto " + (i % 5 + 1);
                        row["Valor"] = 1000 * i;
                        break;
                }

                dt.Rows.Add(row);
            }

            // Configurar GridView para mostrar los datos
            gvReporte.DataSource = dt;
            gvReporte.Columns.Clear();

            // Agregar columnas al GridView según el tipo de reporte
            BoundField bfID = new BoundField();
            bfID.DataField = "ID";
            bfID.HeaderText = "ID";
            bfID.SortExpression = "ID";
            gvReporte.Columns.Add(bfID);

            BoundField bfFecha = new BoundField();
            bfFecha.DataField = "Fecha";
            bfFecha.HeaderText = "Fecha";
            bfFecha.SortExpression = "Fecha";
            bfFecha.DataFormatString = "{0:dd/MM/yyyy}";
            gvReporte.Columns.Add(bfFecha);

            // Añadir columnas específicas según el tipo de reporte
            switch (tipoReporte)
            {
                case "conductor":
                    BoundField bfDNI = new BoundField();
                    bfDNI.DataField = "DNI";
                    bfDNI.HeaderText = "DNI";
                    bfDNI.SortExpression = "DNI";
                    gvReporte.Columns.Add(bfDNI);

                    BoundField bfNombre = new BoundField();
                    bfNombre.DataField = "Nombre";
                    bfNombre.HeaderText = "Nombre";
                    bfNombre.SortExpression = "Nombre";
                    gvReporte.Columns.Add(bfNombre);

                    BoundField bfVehiculo = new BoundField();
                    bfVehiculo.DataField = "Vehículo";
                    bfVehiculo.HeaderText = "Vehículo";
                    bfVehiculo.SortExpression = "Vehículo";
                    gvReporte.Columns.Add(bfVehiculo);

                    BoundField bfDestino = new BoundField();
                    bfDestino.DataField = "Destino";
                    bfDestino.HeaderText = "Destino";
                    bfDestino.SortExpression = "Destino";
                    gvReporte.Columns.Add(bfDestino);
                    break;

                case "vehiculo":
                    BoundField bfPlaca = new BoundField();
                    bfPlaca.DataField = "Placa";
                    bfPlaca.HeaderText = "Placa";
                    bfPlaca.SortExpression = "Placa";
                    gvReporte.Columns.Add(bfPlaca);

                    BoundField bfMarca = new BoundField();
                    bfMarca.DataField = "Marca";
                    bfMarca.HeaderText = "Marca";
                    bfMarca.SortExpression = "Marca";
                    gvReporte.Columns.Add(bfMarca);

                    BoundField bfConductorVeh = new BoundField();
                    bfConductorVeh.DataField = "Conductor";
                    bfConductorVeh.HeaderText = "Conductor";
                    bfConductorVeh.SortExpression = "Conductor";
                    gvReporte.Columns.Add(bfConductorVeh);

                    BoundField bfKilometraje = new BoundField();
                    bfKilometraje.DataField = "Kilometraje";
                    bfKilometraje.HeaderText = "Kilometraje";
                    bfKilometraje.SortExpression = "Kilometraje";
                    bfKilometraje.DataFormatString = "{0:N0}";
                    gvReporte.Columns.Add(bfKilometraje);
                    break;

                case "pedido":
                    BoundField bfNumero = new BoundField();
                    bfNumero.DataField = "Número";
                    bfNumero.HeaderText = "Nº Pedido";
                    bfNumero.SortExpression = "Número";
                    gvReporte.Columns.Add(bfNumero);

                    BoundField bfCliente = new BoundField();
                    bfCliente.DataField = "Cliente";
                    bfCliente.HeaderText = "Cliente";
                    bfCliente.SortExpression = "Cliente";
                    gvReporte.Columns.Add(bfCliente);

                    BoundField bfValor = new BoundField();
                    bfValor.DataField = "Valor";
                    bfValor.HeaderText = "Valor";
                    bfValor.SortExpression = "Valor";
                    bfValor.DataFormatString = "{0:C}";
                    gvReporte.Columns.Add(bfValor);

                    BoundField bfEstado = new BoundField();
                    bfEstado.DataField = "Estado";
                    bfEstado.HeaderText = "Estado";
                    bfEstado.SortExpression = "Estado";
                    gvReporte.Columns.Add(bfEstado);
                    break;

                case "financiero":
                    BoundField bfConcepto = new BoundField();
                    bfConcepto.DataField = "Concepto";
                    bfConcepto.HeaderText = "Concepto";
                    bfConcepto.SortExpression = "Concepto";
                    gvReporte.Columns.Add(bfConcepto);

                    BoundField bfIngresos = new BoundField();
                    bfIngresos.DataField = "Ingresos";
                    bfIngresos.HeaderText = "Ingresos";
                    bfIngresos.SortExpression = "Ingresos";
                    bfIngresos.DataFormatString = "{0:C}";
                    gvReporte.Columns.Add(bfIngresos);

                    BoundField bfEgresos = new BoundField();
                    bfEgresos.DataField = "Egresos";
                    bfEgresos.HeaderText = "Egresos";
                    bfEgresos.SortExpression = "Egresos";
                    bfEgresos.DataFormatString = "{0:C}";
                    gvReporte.Columns.Add(bfEgresos);

                    BoundField bfBalance = new BoundField();
                    bfBalance.DataField = "Balance";
                    bfBalance.HeaderText = "Balance";
                    bfBalance.SortExpression = "Balance";
                    bfBalance.DataFormatString = "{0:C}";
                    gvReporte.Columns.Add(bfBalance);
                    break;

                case "combustible":
                    BoundField bfVehiculoComb = new BoundField();
                    bfVehiculoComb.DataField = "Vehículo";
                    bfVehiculoComb.HeaderText = "Vehículo";
                    bfVehiculoComb.SortExpression = "Vehículo";
                    gvReporte.Columns.Add(bfVehiculoComb);

                    BoundField bfGalones = new BoundField();
                    bfGalones.DataField = "Galones";
                    bfGalones.HeaderText = "Galones";
                    bfGalones.SortExpression = "Galones";
                    bfGalones.DataFormatString = "{0:N2}";
                    gvReporte.Columns.Add(bfGalones);

                    BoundField bfCosto = new BoundField();
                    bfCosto.DataField = "Costo";
                    bfCosto.HeaderText = "Costo";
                    bfCosto.SortExpression = "Costo";
                    bfCosto.DataFormatString = "{0:C}";
                    gvReporte.Columns.Add(bfCosto);

                    BoundField bfRendimiento = new BoundField();
                    bfRendimiento.DataField = "Rendimiento";
                    bfRendimiento.HeaderText = "Rendimiento (km/gal)";
                    bfRendimiento.SortExpression = "Rendimiento";
                    bfRendimiento.DataFormatString = "{0:N2}";
                    gvReporte.Columns.Add(bfRendimiento);
                    break;

                case "producto":
                    BoundField bfProducto = new BoundField();
                    bfProducto.DataField = "Producto";
                    bfProducto.HeaderText = "Producto";
                    bfProducto.SortExpression = "Producto";
                    gvReporte.Columns.Add(bfProducto);

                    BoundField bfCantidad = new BoundField();
                    bfCantidad.DataField = "Cantidad";
                    bfCantidad.HeaderText = "Cantidad";
                    bfCantidad.SortExpression = "Cantidad";
                    bfCantidad.DataFormatString = "{0:N0}";
                    gvReporte.Columns.Add(bfCantidad);

                    BoundField bfClienteProd = new BoundField();
                    bfClienteProd.DataField = "Cliente";
                    bfClienteProd.HeaderText = "Cliente";
                    bfClienteProd.SortExpression = "Cliente";
                    gvReporte.Columns.Add(bfClienteProd);

                    BoundField bfDestinoProd = new BoundField();
                    bfDestinoProd.DataField = "Destino";
                    bfDestinoProd.HeaderText = "Destino";
                    bfDestinoProd.SortExpression = "Destino";
                    gvReporte.Columns.Add(bfDestinoProd);
                    break;

                case "personalizado":
                    // Para reporte personalizado, verificamos qué campos ha seleccionado el usuario
                    if (chkConductorInfo.Checked)
                    {
                        BoundField bfConductorPers = new BoundField();
                        bfConductorPers.DataField = "Conductor";
                        bfConductorPers.HeaderText = "Conductor";
                        bfConductorPers.SortExpression = "Conductor";
                        gvReporte.Columns.Add(bfConductorPers);
                    }

                    if (chkVehiculoInfo.Checked)
                    {
                        BoundField bfVehiculoPers = new BoundField();
                        bfVehiculoPers.DataField = "Vehículo";
                        bfVehiculoPers.HeaderText = "Vehículo";
                        bfVehiculoPers.SortExpression = "Vehículo";
                        gvReporte.Columns.Add(bfVehiculoPers);
                    }

                    if (chkClienteInfo.Checked)
                    {
                        BoundField bfClientePers = new BoundField();
                        bfClientePers.DataField = "Cliente";
                        bfClientePers.HeaderText = "Cliente";
                        bfClientePers.SortExpression = "Cliente";
                        gvReporte.Columns.Add(bfClientePers);
                    }

                    if (chkProductoInfo.Checked)
                    {
                        BoundField bfProductoPers = new BoundField();
                        bfProductoPers.DataField = "Producto";
                        bfProductoPers.HeaderText = "Producto";
                        bfProductoPers.SortExpression = "Producto";
                        gvReporte.Columns.Add(bfProductoPers);
                    }

                    if (chkIngresoInfo.Checked || chkEgresoInfo.Checked)
                    {
                        BoundField bfValorPers = new BoundField();
                        bfValorPers.DataField = "Valor";
                        bfValorPers.HeaderText = "Valor";
                        bfValorPers.SortExpression = "Valor";
                        bfValorPers.DataFormatString = "{0:C}";
                        gvReporte.Columns.Add(bfValorPers);
                    }
                    break;
            }

            // Añadir botón de detalles para todos los reportes
            ButtonField btnDetalles = new ButtonField();
            btnDetalles.ButtonType = ButtonType.Button;
            btnDetalles.Text = "Ver Detalles";
            btnDetalles.CommandName = "VerDetalles";
            btnDetalles.ControlStyle.CssClass = "btn btn-sm btn-info btn-detalle-orden";
            gvReporte.Columns.Add(btnDetalles);

            gvReporte.DataBind();

            // Configurar el título de los resultados según el tipo de reporte
            litTituloResultados.Text = ObtenerTituloReporte(tipoReporte, tipoReporteDetalle);

            // Configurar el contador de registros
            lblTotalRegistros.Text = $"Total de registros: {dt.Rows.Count}";

            // Generar datos de muestra para los indicadores
            GenerarIndicadoresMuestra(tipoReporte);

            // Generar gráfico de muestra
            GenerarGraficoMuestra(tipoReporte);
        }

        private string ObtenerTituloReporte(string tipoReporte, string tipoReporteDetalle)
        {
            switch (tipoReporte)
            {
                case "conductor":
                    return "Reporte de Conductor: " + (string.IsNullOrEmpty(ddlConductor.SelectedValue) ?
                        "Todos los conductores" : ddlConductor.SelectedItem.Text);

                case "vehiculo":
                    return "Reporte de Vehículo: " + (string.IsNullOrEmpty(ddlVehiculo.SelectedValue) ?
                        "Todos los vehículos" : ddlVehiculo.SelectedItem.Text);

                case "pedido":
                    return "Reporte de Pedido: " + (string.IsNullOrEmpty(ddlCPIC.SelectedValue) ?
                        "Todos los pedidos" : ddlCPIC.SelectedItem.Text);

                case "financiero":
                    return "Reporte Financiero: " + ddlTipoReporteDetalle.SelectedItem.Text;

                case "combustible":
                    return "Reporte de Combustible: " + ddlTipoReporteDetalle.SelectedItem.Text;

                case "producto":
                    return "Reporte de Producto: " + (string.IsNullOrEmpty(ddlProducto.SelectedValue) ?
                        "Todos los productos" : ddlProducto.SelectedItem.Text);

                case "personalizado":
                    return "Reporte Personalizado";

                default:
                    return "Reporte General";
            }
        }

        private void GenerarIndicadoresMuestra(string tipoReporte)
        {
            // Generar datos de muestra para los indicadores según el tipo de reporte
            switch (tipoReporte)
            {
                case "conductor":
                    litTotalIngresos.Text = "S/ 25,000.00";
                    litTotalEgresos.Text = "S/ 15,000.00";
                    litBalance.Text = "S/ 10,000.00";
                    litIndicadorAdicionalTitulo.Text = "Total Viajes";
                    litIndicadorAdicional.Text = "15";
                    break;

                case "vehiculo":
                    litTotalIngresos.Text = "S/ 18,500.00";
                    litTotalEgresos.Text = "S/ 12,300.00";
                    litBalance.Text = "S/ 6,200.00";
                    litIndicadorAdicionalTitulo.Text = "Rendimiento Promedio";
                    litIndicadorAdicional.Text = "12.5 km/gal";
                    break;

                case "pedido":
                    litTotalIngresos.Text = "S/ 35,000.00";
                    litTotalEgresos.Text = "S/ 22,000.00";
                    litBalance.Text = "S/ 13,000.00";
                    litIndicadorAdicionalTitulo.Text = "Viajes Realizados";
                    litIndicadorAdicional.Text = "8";
                    break;

                case "financiero":
                    litTotalIngresos.Text = "S/ 125,000.00";
                    litTotalEgresos.Text = "S/ 95,000.00";
                    litBalance.Text = "S/ 30,000.00";
                    litIndicadorAdicionalTitulo.Text = "Margen";
                    litIndicadorAdicional.Text = "24%";
                    break;

                case "combustible":
                    litTotalIngresos.Text = "S/ 42,000.00";
                    litTotalEgresos.Text = "S/ 8,500.00";
                    litBalance.Text = "S/ 33,500.00";
                    litIndicadorAdicionalTitulo.Text = "Total Combustible";
                    litIndicadorAdicional.Text = "1,250 gal";
                    break;

                case "producto":
                    litTotalIngresos.Text = "S/ 65,000.00";
                    litTotalEgresos.Text = "S/ 45,000.00";
                    litBalance.Text = "S/ 20,000.00";
                    litIndicadorAdicionalTitulo.Text = "Cantidad Transportada";
                    litIndicadorAdicional.Text = "2,500 und";
                    break;

                case "personalizado":
                    litTotalIngresos.Text = "S/ 55,000.00";
                    litTotalEgresos.Text = "S/ 35,000.00";
                    litBalance.Text = "S/ 20,000.00";
                    litIndicadorAdicionalTitulo.Text = "Indicador Personalizado";
                    litIndicadorAdicional.Text = "Variable";
                    break;

                default:
                    litTotalIngresos.Text = "S/ 0.00";
                    litTotalEgresos.Text = "S/ 0.00";
                    litBalance.Text = "S/ 0.00";
                    litIndicadorAdicionalTitulo.Text = "Total";
                    litIndicadorAdicional.Text = "0";
                    break;
            }
        }

        private void GenerarGraficoMuestra(string tipoReporte)
        {
            // En esta versión básica, configuramos un gráfico de muestra
            // que se actualizará con datos reales cuando implementemos la lógica específica

            // Configurar títulos y ejes del gráfico según el tipo de reporte
            switch (tipoReporte)
            {
                case "conductor":
                    litTituloGrafico.Text = "Viajes realizados por mes";
                    chartReporte.ChartAreas[0].AxisX.Title = "Mes";
                    chartReporte.ChartAreas[0].AxisY.Title = "Cantidad de viajes";
                    break;

                case "vehiculo":
                    litTituloGrafico.Text = "Consumo de combustible por mes";
                    chartReporte.ChartAreas[0].AxisX.Title = "Mes";
                    chartReporte.ChartAreas[0].AxisY.Title = "Galones";
                    break;

                case "pedido":
                    litTituloGrafico.Text = "Distribución de viajes por conductor";
                    chartReporte.ChartAreas[0].AxisX.Title = "Conductor";
                    chartReporte.ChartAreas[0].AxisY.Title = "Cantidad de viajes";
                    break;

                case "financiero":
                    litTituloGrafico.Text = "Ingresos vs Egresos por mes";
                    chartReporte.ChartAreas[0].AxisX.Title = "Mes";
                    chartReporte.ChartAreas[0].AxisY.Title = "Monto (S/)";
                    break;

                case "combustible":
                    litTituloGrafico.Text = "Rendimiento por vehículo";
                    chartReporte.ChartAreas[0].AxisX.Title = "Vehículo";
                    chartReporte.ChartAreas[0].AxisY.Title = "Rendimiento (km/gal)";
                    break;

                case "producto":
                    litTituloGrafico.Text = "Productos más transportados";
                    chartReporte.ChartAreas[0].AxisX.Title = "Producto";
                    chartReporte.ChartAreas[0].AxisY.Title = "Cantidad";
                    break;

                case "personalizado":
                    litTituloGrafico.Text = "Gráfico personalizado";
                    chartReporte.ChartAreas[0].AxisX.Title = "Categoría";
                    chartReporte.ChartAreas[0].AxisY.Title = "Valor";
                    break;

                default:
                    litTituloGrafico.Text = "Análisis de datos";
                    chartReporte.ChartAreas[0].AxisX.Title = "Categoría";
                    chartReporte.ChartAreas[0].AxisY.Title = "Valor";
                    break;
            }

            // Limpiar series existentes y crear nuevas
            chartReporte.Series.Clear();

            // Crear la primera serie
            Series serie1 = new Series();
            serie1.Name = "Serie1";
            serie1.ChartType = SeriesChartType.Column;
            serie1.Color = Color.FromArgb(46, 204, 113); // Verde
            serie1.IsValueShownAsLabel = true;

            // Para algunos tipos de reporte, añadir una segunda serie
            Series serie2 = null;
            if (tipoReporte == "financiero" || tipoReporte == "conductor" || tipoReporte == "vehiculo")
            {
                serie2 = new Series();
                serie2.Name = "Serie2";
                serie2.ChartType = SeriesChartType.Column;
                serie2.Color = Color.FromArgb(231, 76, 60); // Rojo
                serie2.IsValueShownAsLabel = true;
            }

            // Añadir puntos de datos de muestra
            Random rnd = new Random();
            string[] categorias = { "Ene", "Feb", "Mar", "Abr", "May", "Jun" };

            for (int i = 0; i < categorias.Length; i++)
            {
                serie1.Points.AddXY(categorias[i], rnd.Next(10, 100));

                if (serie2 != null)
                {
                    serie2.Points.AddXY(categorias[i], rnd.Next(5, 80));
                }
            }

            // Añadir las series al gráfico
            chartReporte.Series.Add(serie1);
            if (serie2 != null)
            {
                chartReporte.Series.Add(serie2);

                // Actualizar leyendas para gráficos específicos
                if (tipoReporte == "financiero")
                {
                    serie1.Name = "Ingresos";
                    serie2.Name = "Egresos";
                }
                else if (tipoReporte == "conductor")
                {
                    serie1.Name = "Viajes";
                    serie2.Name = "Productos";
                }
                else if (tipoReporte == "vehiculo")
                {
                    serie1.Name = "Kilometraje";
                    serie2.Name = "Combustible";
                }
            }
        }

        private string ObtenerTipoReporteSeleccionado()
        {
            if (lnkConductor.CssClass.Contains("active"))
                return "conductor";
            else if (lnkVehiculo.CssClass.Contains("active"))
                return "vehiculo";
            else if (lnkPedido.CssClass.Contains("active"))
                return "pedido";
            else if (lnkFinanciero.CssClass.Contains("active"))
                return "financiero";
            else if (lnkCombustible.CssClass.Contains("active"))
                return "combustible";
            else if (lnkProducto.CssClass.Contains("active"))
                return "producto";
            else if (lnkPersonalizado.CssClass.Contains("active"))
                return "personalizado";
            else
                return "conductor"; // Default
        }

        // Aquí se implementarán los métodos específicos para cada tipo de reporte
        // Estos métodos serán implementados posteriormente

        #endregion

        #region Métodos para Detalles de Orden de Viaje

        protected void btnExportarDetalleExcel_Click(object sender, EventArgs e)
        {
            // Esta función será implementada cuando se complete la funcionalidad del modal de detalles
            Response.Write("<script>alert('Exportación de detalles en desarrollo.');</script>");
        }

        #endregion

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Necesario para la exportación a Excel/PDF
            // No realizar validación para permitir la exportación
        }
    }
}