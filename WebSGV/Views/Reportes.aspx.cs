using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Configuration;

namespace WebSGV.Views
{
    public partial class Reportes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicializar controles y cargar datos iniciales
                CargarConductores();
                CargarVehiculos();
                CargarRutas();
                CargarTiposReporteDetalle("conductor"); // Por defecto, cargamos los reportes de conductor
            }
        }

        #region Métodos de carga de datos iniciales

        private void CargarConductores()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT idConductor, CONCAT(nombres, ' ', apellidos) AS NombreCompleto FROM Conductor ORDER BY apellidos", conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ddlConductor.DataSource = dt;
                        ddlConductor.DataBind();
                        ddlConductor.Items.Insert(0, new ListItem("Todos los conductores", ""));
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar el error
                Response.Write("<script>alert('Error al cargar conductores: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }

        private void CargarVehiculos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT idTracto, placaTracto FROM Tracto ORDER BY placaTracto", conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ddlVehiculo.DataSource = dt;
                        ddlVehiculo.DataBind();
                        ddlVehiculo.Items.Insert(0, new ListItem("Todos los vehículos", ""));
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar el error
                Response.Write("<script>alert('Error al cargar vehículos: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }

        private void CargarRutas()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT idRuta, nombre FROM Ruta ORDER BY nombre", conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ddlRuta.DataSource = dt;
                        ddlRuta.DataBind();
                        ddlRuta.Items.Insert(0, new ListItem("Todas las rutas", ""));
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar el error
                Response.Write("<script>alert('Error al cargar rutas: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }

        private void CargarTiposReporteDetalle(string tipoReporte)
        {
            ddlTipoReporteDetalle.Items.Clear();

            switch (tipoReporte)
            {
                case "conductor":
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Rendimiento por conductor", "rendimiento_conductor"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Viajes realizados", "viajes_conductor"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Ingresos generados", "ingresos_conductor"));
                    break;
                case "vehiculo":
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Consumo de combustible", "consumo_vehiculo"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Mantenimientos realizados", "mantenimiento_vehiculo"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Kilometraje por vehículo", "kilometraje_vehiculo"));
                    break;
                case "ruta":
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Rentabilidad por ruta", "rentabilidad_ruta"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Tiempo promedio por ruta", "tiempo_ruta"));
                    break;
                case "financiero":
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Ingresos vs Gastos", "ingresos_gastos"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Rentabilidad mensual", "rentabilidad_mensual"));
                    break;
                case "combustible":
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Consumo por vehículo", "consumo_por_vehiculo"));
                    ddlTipoReporteDetalle.Items.Add(new ListItem("Gasto en combustible", "gasto_combustible"));
                    break;
            }
        }

        #endregion

        #region Eventos de UI

        protected void lnkTipoReporte_Click(object sender, EventArgs e)
        {
            LinkButton lnkButton = (LinkButton)sender;
            string tipoReporte = lnkButton.CommandArgument;

            // Actualizar título
            switch (tipoReporte)
            {
                case "conductor":
                    litTituloReporte.Text = "Reportes por Conductor";
                    break;
                case "vehiculo":
                    litTituloReporte.Text = "Reportes por Vehículo";
                    break;
                case "ruta":
                    litTituloReporte.Text = "Reportes por Ruta";
                    break;
                case "financiero":
                    litTituloReporte.Text = "Reportes Financieros";
                    break;
                case "combustible":
                    litTituloReporte.Text = "Reportes de Combustible";
                    break;
            }

            // Actualizar estado de botones
            lnkConductor.CssClass = lnkConductor.CssClass.Replace(" active", "");
            lnkVehiculo.CssClass = lnkVehiculo.CssClass.Replace(" active", "");
            lnkRuta.CssClass = lnkRuta.CssClass.Replace(" active", "");
            lnkFinanciero.CssClass = lnkFinanciero.CssClass.Replace(" active", "");
            lnkCombustible.CssClass = lnkCombustible.CssClass.Replace(" active", "");

            lnkButton.CssClass += " active";

            // Mostrar/ocultar paneles de filtros
            pnlFiltroConductor.Visible = tipoReporte == "conductor";
            pnlFiltroVehiculo.Visible = tipoReporte == "vehiculo";
            pnlFiltroRuta.Visible = tipoReporte == "ruta";

            // Mostrar/ocultar paneles de filtros avanzados
            pnlFiltrosAvanzadosConductor.Visible = tipoReporte == "conductor";
            pnlFiltrosAvanzadosVehiculo.Visible = tipoReporte == "vehiculo";

            // Cargar tipos de reporte detalle
            CargarTiposReporteDetalle(tipoReporte);
        }

        protected void ddlTipoReporteDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Esta función se dispara cuando se cambia el tipo de reporte detalle
            // Por ahora simplemente ocultamos el panel de resultados para que 
            // el usuario genere un nuevo reporte
            pnlResultados.Visible = false;
        }

        protected void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            // Obtener tipo de reporte seleccionado
            string tipoReporte = "";
            if (lnkConductor.CssClass.Contains("active")) tipoReporte = "conductor";
            else if (lnkVehiculo.CssClass.Contains("active")) tipoReporte = "vehiculo";
            else if (lnkRuta.CssClass.Contains("active")) tipoReporte = "ruta";
            else if (lnkFinanciero.CssClass.Contains("active")) tipoReporte = "financiero";
            else if (lnkCombustible.CssClass.Contains("active")) tipoReporte = "combustible";

            string tipoReporteDetalle = ddlTipoReporteDetalle.SelectedValue;

            // Generar el reporte según el tipo
            GenerarReporte(tipoReporte, tipoReporteDetalle);
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            // Implementar la exportación a Excel
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Reporte_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            // Estilo para la tabla Excel
            Response.Write("<style> .textmode { mso-number-format:\\@; } </style>");

            // Renderizar solo el GridView en el Excel
            gvReporte.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        // Este método es necesario para la exportación a Excel
        public override void VerifyRenderingInServerForm(Control control)
        {
            // No hacer nada, este método se sobrescribe para evitar errores
            // durante la exportación a Excel
        }

        protected void gvReporte_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReporte.PageIndex = e.NewPageIndex;

            // Obtener tipo de reporte seleccionado
            string tipoReporte = "";
            if (lnkConductor.CssClass.Contains("active")) tipoReporte = "conductor";
            else if (lnkVehiculo.CssClass.Contains("active")) tipoReporte = "vehiculo";
            else if (lnkRuta.CssClass.Contains("active")) tipoReporte = "ruta";
            else if (lnkFinanciero.CssClass.Contains("active")) tipoReporte = "financiero";
            else if (lnkCombustible.CssClass.Contains("active")) tipoReporte = "combustible";

            string tipoReporteDetalle = ddlTipoReporteDetalle.SelectedValue;

            // Regenerar el reporte según el tipo
            GenerarReporte(tipoReporte, tipoReporteDetalle);
        }

        protected void gvReporte_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Implementar la lógica de ordenamiento del GridView
            // Esta funcionalidad se puede implementar si es necesario
        }

        protected void btnAplicarFiltrosAvanzados_Click(object sender, EventArgs e)
        {
            // Cerrar el modal de filtros avanzados y regenerar el reporte
            ScriptManager.RegisterStartupScript(this, GetType(), "cerrarModal", "$('#filtrosAvanzadosModal').modal('hide');", true);

            // Obtener tipo de reporte seleccionado
            string tipoReporte = "";
            if (lnkConductor.CssClass.Contains("active")) tipoReporte = "conductor";
            else if (lnkVehiculo.CssClass.Contains("active")) tipoReporte = "vehiculo";
            else if (lnkRuta.CssClass.Contains("active")) tipoReporte = "ruta";
            else if (lnkFinanciero.CssClass.Contains("active")) tipoReporte = "financiero";
            else if (lnkCombustible.CssClass.Contains("active")) tipoReporte = "combustible";

            string tipoReporteDetalle = ddlTipoReporteDetalle.SelectedValue;

            // Regenerar el reporte según el tipo
            GenerarReporte(tipoReporte, tipoReporteDetalle);
        }

        #endregion

        #region Métodos de generación de reportes

        private void GenerarReporte(string tipoReporte, string tipoReporteDetalle)
        {
            // Este método generará el reporte según el tipo y subtipo seleccionados
            switch (tipoReporte)
            {
                case "conductor":
                    GenerarReporteConductor(tipoReporteDetalle);
                    break;
                case "vehiculo":
                    GenerarReporteVehiculo(tipoReporteDetalle);
                    break;
                case "ruta":
                    GenerarReporteRuta(tipoReporteDetalle);
                    break;
                case "financiero":
                    GenerarReporteFinanciero(tipoReporteDetalle);
                    break;
                case "combustible":
                    GenerarReporteCombustible(tipoReporteDetalle);
                    break;
            }
        }

        private void GenerarReporteConductor(string tipoReporteDetalle)
        {
            try
            {
                string fechaDesde = txtFechaDesde.Text;
                string fechaHasta = txtFechaHasta.Text;
                string idConductor = ddlConductor.SelectedValue;
                string tipoLicencia = ddlTipoLicencia.SelectedValue;
                string experienciaMinima = txtExperienciaMinima.Text;

                // Crear DataTable para simular resultados (en producción conectaría a la BD)
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Nombre", typeof(string));
                dt.Columns.Add("FechaViaje", typeof(DateTime));
                dt.Columns.Add("Ruta", typeof(string));
                dt.Columns.Add("Vehiculo", typeof(string));
                dt.Columns.Add("Kilometraje", typeof(decimal));
                dt.Columns.Add("Ingresos", typeof(decimal));
                dt.Columns.Add("Gastos", typeof(decimal));
                dt.Columns.Add("Rendimiento", typeof(decimal));

                // Datos de ejemplo (en producción estos vendrían de la BD)
                dt.Rows.Add(1, "Juan Pérez", DateTime.Now.AddDays(-30), "Lima - Arequipa", "ABC-123", 550.5m, 2500.0m, 800.0m, 78.5m);
                dt.Rows.Add(1, "Juan Pérez", DateTime.Now.AddDays(-25), "Lima - Cusco", "ABC-123", 620.3m, 2800.0m, 950.0m, 72.3m);
                dt.Rows.Add(2, "Carlos López", DateTime.Now.AddDays(-20), "Lima - Trujillo", "XYZ-456", 380.2m, 1800.0m, 650.0m, 82.1m);
                dt.Rows.Add(2, "Carlos López", DateTime.Now.AddDays(-15), "Lima - Chiclayo", "XYZ-456", 420.8m, 2000.0m, 700.0m, 80.5m);
                dt.Rows.Add(3, "María García", DateTime.Now.AddDays(-10), "Lima - Tacna", "DEF-789", 850.4m, 3500.0m, 1200.0m, 75.8m);

                // Configurar el GridView con columnas dinámicas
                gvReporte.Columns.Clear();

                // Añadir columnas según el tipo de reporte
                switch (tipoReporteDetalle)
                {
                    case "rendimiento_conductor":
                        // Columnas para reporte de rendimiento
                        gvReporte.Columns.Add(new BoundField { HeaderText = "ID", DataField = "ID", SortExpression = "ID" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Conductor", DataField = "Nombre", SortExpression = "Nombre" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Fecha", DataField = "FechaViaje", SortExpression = "FechaViaje", DataFormatString = "{0:dd/MM/yyyy}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Vehículo", DataField = "Vehiculo", SortExpression = "Vehiculo" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Rendimiento", DataField = "Rendimiento", SortExpression = "Rendimiento", DataFormatString = "{0:F2}%" });

                        litTituloResultados.Text = "Reporte de Rendimiento por Conductor";
                        litTituloGrafico.Text = "Rendimiento por Conductor";
                        break;

                    case "viajes_conductor":
                        // Columnas para reporte de viajes
                        gvReporte.Columns.Add(new BoundField { HeaderText = "ID", DataField = "ID", SortExpression = "ID" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Conductor", DataField = "Nombre", SortExpression = "Nombre" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Fecha", DataField = "FechaViaje", SortExpression = "FechaViaje", DataFormatString = "{0:dd/MM/yyyy}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Ruta", DataField = "Ruta", SortExpression = "Ruta" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Vehículo", DataField = "Vehiculo", SortExpression = "Vehiculo" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Kilometraje", DataField = "Kilometraje", SortExpression = "Kilometraje", DataFormatString = "{0:F2} km" });

                        litTituloResultados.Text = "Reporte de Viajes por Conductor";
                        litTituloGrafico.Text = "Viajes por Conductor";
                        break;

                    case "ingresos_conductor":
                        // Columnas para reporte de ingresos
                        gvReporte.Columns.Add(new BoundField { HeaderText = "ID", DataField = "ID", SortExpression = "ID" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Conductor", DataField = "Nombre", SortExpression = "Nombre" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Fecha", DataField = "FechaViaje", SortExpression = "FechaViaje", DataFormatString = "{0:dd/MM/yyyy}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Ruta", DataField = "Ruta", SortExpression = "Ruta" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Ingresos", DataField = "Ingresos", SortExpression = "Ingresos", DataFormatString = "S/ {0:F2}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Gastos", DataField = "Gastos", SortExpression = "Gastos", DataFormatString = "S/ {0:F2}" });

                        litTituloResultados.Text = "Reporte de Ingresos por Conductor";
                        litTituloGrafico.Text = "Ingresos vs Gastos por Conductor";
                        break;
                }

                // Asignar datos al GridView
                gvReporte.DataSource = dt;
                gvReporte.DataBind();

                // Generar gráfico según el tipo de reporte
                GenerarGrafico(dt, tipoReporteDetalle);

                // Mostrar panel de resultados
                pnlResultados.Visible = true;
            }
            catch (Exception ex)
            {
                // Manejar el error
                Response.Write("<script>alert('Error al generar reporte: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }

        private void GenerarReporteVehiculo(string tipoReporteDetalle)
        {
            try
            {
                // Crear DataTable para simular resultados
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Placa", typeof(string));
                dt.Columns.Add("Marca", typeof(string));
                dt.Columns.Add("Modelo", typeof(string));
                dt.Columns.Add("FechaMantenimiento", typeof(DateTime));
                dt.Columns.Add("KilometrajeAcumulado", typeof(decimal));
                dt.Columns.Add("ConsumoCombustible", typeof(decimal));
                dt.Columns.Add("Costo", typeof(decimal));

                // Datos de ejemplo
                dt.Rows.Add(1, "ABC-123", "Volvo", "FH16", DateTime.Now.AddDays(-30), 125000.5m, 38.5m, 850.0m);
                dt.Rows.Add(2, "XYZ-456", "Scania", "R500", DateTime.Now.AddDays(-25), 98000.3m, 35.2m, 780.0m);
                dt.Rows.Add(3, "DEF-789", "Freightliner", "Cascadia", DateTime.Now.AddDays(-20), 110500.8m, 40.1m, 920.0m);

                // Configurar el GridView con columnas dinámicas
                gvReporte.Columns.Clear();
                gvReporte.Columns.Add(new BoundField { HeaderText = "ID", DataField = "ID", SortExpression = "ID" });
                gvReporte.Columns.Add(new BoundField { HeaderText = "Placa", DataField = "Placa", SortExpression = "Placa" });

                // Añadir columnas según el tipo de reporte
                switch (tipoReporteDetalle)
                {
                    case "consumo_vehiculo":
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Marca", DataField = "Marca", SortExpression = "Marca" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Modelo", DataField = "Modelo", SortExpression = "Modelo" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Kilometraje", DataField = "KilometrajeAcumulado", SortExpression = "KilometrajeAcumulado", DataFormatString = "{0:N1} km" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Consumo", DataField = "ConsumoCombustible", SortExpression = "ConsumoCombustible", DataFormatString = "{0:F2} gal/100km" });
                        litTituloResultados.Text = "Reporte de Consumo de Combustible por Vehículo";
                        litTituloGrafico.Text = "Consumo por Vehículo";
                        break;

                    case "mantenimiento_vehiculo":
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Marca", DataField = "Marca", SortExpression = "Marca" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Modelo", DataField = "Modelo", SortExpression = "Modelo" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Fecha Mantenimiento", DataField = "FechaMantenimiento", SortExpression = "FechaMantenimiento", DataFormatString = "{0:dd/MM/yyyy}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Costo", DataField = "Costo", SortExpression = "Costo", DataFormatString = "S/ {0:F2}" });
                        litTituloResultados.Text = "Reporte de Mantenimientos por Vehículo";
                        litTituloGrafico.Text = "Costos de Mantenimiento";
                        break;

                    case "kilometraje_vehiculo":
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Marca", DataField = "Marca", SortExpression = "Marca" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Modelo", DataField = "Modelo", SortExpression = "Modelo" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Kilometraje Acumulado", DataField = "KilometrajeAcumulado", SortExpression = "KilometrajeAcumulado", DataFormatString = "{0:N1} km" });
                        litTituloResultados.Text = "Reporte de Kilometraje por Vehículo";
                        litTituloGrafico.Text = "Kilometraje por Vehículo";
                        break;
                }

                // Asignar datos al GridView
                gvReporte.DataSource = dt;
                gvReporte.DataBind();

                // Generar gráfico básico (esto debería adaptarse al tipo de reporte)
                chartReporte.Series.Clear();
                chartReporte.ChartAreas.Clear();

                ChartArea chartArea = new ChartArea("ChartArea1");
                chartArea.BackColor = Color.Transparent;
                chartArea.AxisX.Interval = 1;
                chartArea.AxisY.Title = "Valores";
                chartReporte.ChartAreas.Add(chartArea);

                Series serie = new Series("Datos");
                serie.ChartType = SeriesChartType.Column;
                serie.Color = Color.FromArgb(41, 128, 185);

                foreach (DataRow row in dt.Rows)
                {
                    switch (tipoReporteDetalle)
                    {
                        case "consumo_vehiculo":
                            serie.Points.AddXY(row["Placa"].ToString(), Convert.ToDouble(row["ConsumoCombustible"]));
                            break;
                        case "mantenimiento_vehiculo":
                            serie.Points.AddXY(row["Placa"].ToString(), Convert.ToDouble(row["Costo"]));
                            break;
                        case "kilometraje_vehiculo":
                            serie.Points.AddXY(row["Placa"].ToString(), Convert.ToDouble(row["KilometrajeAcumulado"]));
                            break;
                    }
                }

                chartReporte.Series.Add(serie);

                // Mostrar panel de resultados
                pnlResultados.Visible = true;
                litTituloResultados.Text = "Reporte por Vehículo: " + ddlTipoReporteDetalle.SelectedItem.Text;
            }
            catch (Exception ex)
            {
                // Manejar el error
                Response.Write("<script>alert('Error al generar reporte: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }


        private void GenerarReporteRuta(string tipoReporteDetalle)
        {
            // Implementar lógica similar a los otros reportes
            try
            {
                // Crear datos de ejemplo
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Ruta", typeof(string));
                dt.Columns.Add("Distancia", typeof(decimal));
                dt.Columns.Add("TiempoPromedio", typeof(decimal));
                dt.Columns.Add("Ingresos", typeof(decimal));
                dt.Columns.Add("Gastos", typeof(decimal));
                dt.Columns.Add("Rentabilidad", typeof(decimal));

                // Datos de ejemplo
                dt.Rows.Add(1, "Lima - Arequipa", 1020.5m, 14.5m, 5500.0m, 3200.0m, 41.8m);
                dt.Rows.Add(2, "Lima - Trujillo", 560.3m, 8.2m, 3800.0m, 2100.0m, 44.7m);
                dt.Rows.Add(3, "Lima - Cusco", 1105.8m, 15.7m, 6200.0m, 3500.0m, 43.5m);

                // Configurar GridView
                gvReporte.Columns.Clear();
                gvReporte.Columns.Add(new BoundField { HeaderText = "ID", DataField = "ID", SortExpression = "ID" });
                gvReporte.Columns.Add(new BoundField { HeaderText = "Ruta", DataField = "Ruta", SortExpression = "Ruta" });
                gvReporte.Columns.Add(new BoundField { HeaderText = "Distancia", DataField = "Distancia", SortExpression = "Distancia", DataFormatString = "{0:N1} km" });

                // Añadir columnas según el tipo de reporte
                switch (tipoReporteDetalle)
                {
                    case "rentabilidad_ruta":
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Ingresos", DataField = "Ingresos", SortExpression = "Ingresos", DataFormatString = "S/ {0:N2}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Gastos", DataField = "Gastos", SortExpression = "Gastos", DataFormatString = "S/ {0:N2}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Rentabilidad", DataField = "Rentabilidad", SortExpression = "Rentabilidad", DataFormatString = "{0:N2}%" });

                        litTituloResultados.Text = "Reporte de Rentabilidad por Ruta";
                        litTituloGrafico.Text = "Rentabilidad por Ruta";
                        break;

                    case "tiempo_ruta":
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Tiempo Promedio", DataField = "TiempoPromedio", SortExpression = "TiempoPromedio", DataFormatString = "{0:N1} horas" });

                        litTituloResultados.Text = "Reporte de Tiempo Promedio por Ruta";
                        litTituloGrafico.Text = "Tiempo Promedio por Ruta";
                        break;
                }

                // Asignar datos al GridView
                gvReporte.DataSource = dt;
                gvReporte.DataBind();

                // Generar gráfico
                chartReporte.Series.Clear();
                chartReporte.ChartAreas.Clear();

                ChartArea chartArea = new ChartArea("ChartArea1");
                chartArea.BackColor = Color.Transparent;
                chartArea.AxisX.Interval = 1;
                chartArea.AxisY.Title = tipoReporteDetalle == "rentabilidad_ruta" ? "Rentabilidad (%)" : "Tiempo (horas)";
                chartReporte.ChartAreas.Add(chartArea);

                Series serie = new Series("Datos");
                serie.ChartType = SeriesChartType.Column;
                serie.Color = Color.FromArgb(41, 128, 185);

                foreach (DataRow row in dt.Rows)
                {
                    if (tipoReporteDetalle == "rentabilidad_ruta")
                        serie.Points.AddXY(row["Ruta"].ToString(), Convert.ToDouble(row["Rentabilidad"]));
                    else
                        serie.Points.AddXY(row["Ruta"].ToString(), Convert.ToDouble(row["TiempoPromedio"]));
                }

                chartReporte.Series.Add(serie);

                // Mostrar panel de resultados
                pnlResultados.Visible = true;
            }
            catch (Exception ex)
            {
                // Manejar el error
                Response.Write("<script>alert('Error al generar reporte: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }

        private void GenerarReporteFinanciero(string tipoReporteDetalle)
        {
            try
            {
                // Crear datos de ejemplo
                DataTable dt = new DataTable();
                dt.Columns.Add("Periodo", typeof(string));
                dt.Columns.Add("Ingresos", typeof(decimal));
                dt.Columns.Add("Gastos", typeof(decimal));
                dt.Columns.Add("Utilidad", typeof(decimal));
                dt.Columns.Add("Rentabilidad", typeof(decimal));

                // Datos de ejemplo (últimos 6 meses)
                DateTime ahora = DateTime.Now;
                for (int i = 5; i >= 0; i--)
                {
                    DateTime fecha = ahora.AddMonths(-i);
                    string periodo = fecha.ToString("MMM yyyy");
                    decimal ingresos = 25000m + (decimal)new Random().Next(-5000, 5000);
                    decimal gastos = 15000m + (decimal)new Random().Next(-3000, 3000);
                    decimal utilidad = ingresos - gastos;
                    decimal rentabilidad = (ingresos > 0) ? (utilidad / ingresos * 100) : 0;

                    dt.Rows.Add(periodo, ingresos, gastos, utilidad, rentabilidad);
                }

                // Configurar GridView
                gvReporte.Columns.Clear();
                gvReporte.Columns.Add(new BoundField { HeaderText = "Periodo", DataField = "Periodo", SortExpression = "Periodo" });

                // Añadir columnas según el tipo de reporte
                switch (tipoReporteDetalle)
                {
                    case "ingresos_gastos":
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Ingresos", DataField = "Ingresos", SortExpression = "Ingresos", DataFormatString = "S/ {0:N2}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Gastos", DataField = "Gastos", SortExpression = "Gastos", DataFormatString = "S/ {0:N2}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Utilidad", DataField = "Utilidad", SortExpression = "Utilidad", DataFormatString = "S/ {0:N2}" });

                        litTituloResultados.Text = "Reporte de Ingresos vs Gastos";
                        litTituloGrafico.Text = "Ingresos vs Gastos (Últimos 6 meses)";
                        break;

                    case "rentabilidad_mensual":
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Ingresos", DataField = "Ingresos", SortExpression = "Ingresos", DataFormatString = "S/ {0:N2}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Utilidad", DataField = "Utilidad", SortExpression = "Utilidad", DataFormatString = "S/ {0:N2}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Rentabilidad", DataField = "Rentabilidad", SortExpression = "Rentabilidad", DataFormatString = "{0:N2}%" });

                        litTituloResultados.Text = "Reporte de Rentabilidad Mensual";
                        litTituloGrafico.Text = "Rentabilidad Mensual (Últimos 6 meses)";
                        break;
                }

                // Asignar datos al GridView
                gvReporte.DataSource = dt;
                gvReporte.DataBind();

                // Generar gráfico
                chartReporte.Series.Clear();
                chartReporte.ChartAreas.Clear();

                ChartArea chartArea = new ChartArea("ChartArea1");
                chartArea.BackColor = Color.Transparent;
                chartArea.AxisX.Interval = 1;
                chartArea.AxisY.Title = "Valores (S/)";
                chartReporte.ChartAreas.Add(chartArea);

                if (tipoReporteDetalle == "ingresos_gastos")
                {
                    Series serieIngresos = new Series("Ingresos");
                    serieIngresos.ChartType = SeriesChartType.Column;
                    serieIngresos.Color = Color.FromArgb(40, 167, 69); // Verde

                    Series serieGastos = new Series("Gastos");
                    serieGastos.ChartType = SeriesChartType.Column;
                    serieGastos.Color = Color.FromArgb(220, 53, 69); // Rojo

                    foreach (DataRow row in dt.Rows)
                    {
                        serieIngresos.Points.AddXY(row["Periodo"].ToString(), Convert.ToDouble(row["Ingresos"]));
                        serieGastos.Points.AddXY(row["Periodo"].ToString(), Convert.ToDouble(row["Gastos"]));
                    }

                    chartReporte.Series.Add(serieIngresos);
                    chartReporte.Series.Add(serieGastos);
                }
                else
                {
                    Series serieRentabilidad = new Series("Rentabilidad");
                    serieRentabilidad.ChartType = SeriesChartType.Line;
                    serieRentabilidad.Color = Color.FromArgb(0, 123, 255); // Azul
                    serieRentabilidad.BorderWidth = 3;
                    serieRentabilidad.MarkerStyle = MarkerStyle.Circle;
                    serieRentabilidad.MarkerSize = 8;

                    foreach (DataRow row in dt.Rows)
                    {
                        serieRentabilidad.Points.AddXY(row["Periodo"].ToString(), Convert.ToDouble(row["Rentabilidad"]));
                    }

                    chartReporte.Series.Add(serieRentabilidad);
                }

                // Configurar leyenda
                Legend legend = new Legend("Legend1");
                legend.Alignment = StringAlignment.Center;
                legend.Docking = Docking.Bottom;
                chartReporte.Legends.Add(legend);

                // Mostrar panel de resultados
                pnlResultados.Visible = true;
            }
            catch (Exception ex)
            {
                // Manejar el error
                Response.Write("<script>alert('Error al generar reporte: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }

        private void GenerarReporteCombustible(string tipoReporteDetalle)
        {
            try
            {
                // Crear datos de ejemplo
                DataTable dt = new DataTable();
                dt.Columns.Add("Vehiculo", typeof(string));
                dt.Columns.Add("Fecha", typeof(DateTime));
                dt.Columns.Add("Combustible", typeof(decimal));
                dt.Columns.Add("PrecioUnitario", typeof(decimal));
                dt.Columns.Add("Total", typeof(decimal));
                dt.Columns.Add("Kilometraje", typeof(decimal));
                dt.Columns.Add("Rendimiento", typeof(decimal));

                // Datos de ejemplo
                dt.Rows.Add("ABC-123", DateTime.Now.AddDays(-30), 50.5m, 14.50m, 732.25m, 450.0m, 8.91m);
                dt.Rows.Add("ABC-123", DateTime.Now.AddDays(-20), 48.2m, 14.80m, 713.36m, 420.0m, 8.71m);
                dt.Rows.Add("XYZ-456", DateTime.Now.AddDays(-25), 55.8m, 14.50m, 809.10m, 480.0m, 8.60m);
                dt.Rows.Add("XYZ-456", DateTime.Now.AddDays(-15), 52.3m, 14.80m, 774.04m, 460.0m, 8.80m);
                dt.Rows.Add("DEF-789", DateTime.Now.AddDays(-10), 60.2m, 14.80m, 890.96m, 510.0m, 8.47m);

                // Configurar GridView
                gvReporte.Columns.Clear();
                gvReporte.Columns.Add(new BoundField { HeaderText = "Vehículo", DataField = "Vehiculo", SortExpression = "Vehiculo" });
                gvReporte.Columns.Add(new BoundField { HeaderText = "Fecha", DataField = "Fecha", SortExpression = "Fecha", DataFormatString = "{0:dd/MM/yyyy}" });

                // Añadir columnas según el tipo de reporte
                switch (tipoReporteDetalle)
                {
                    case "consumo_por_vehiculo":
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Combustible (gal)", DataField = "Combustible", SortExpression = "Combustible", DataFormatString = "{0:N2}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Kilometraje", DataField = "Kilometraje", SortExpression = "Kilometraje", DataFormatString = "{0:N1} km" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Rendimiento", DataField = "Rendimiento", SortExpression = "Rendimiento", DataFormatString = "{0:N2} km/gal" });

                        litTituloResultados.Text = "Reporte de Consumo por Vehículo";
                        litTituloGrafico.Text = "Rendimiento por Vehículo";
                        break;

                    case "gasto_combustible":
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Combustible (gal)", DataField = "Combustible", SortExpression = "Combustible", DataFormatString = "{0:N2}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Precio Unitario", DataField = "PrecioUnitario", SortExpression = "PrecioUnitario", DataFormatString = "S/ {0:N2}" });
                        gvReporte.Columns.Add(new BoundField { HeaderText = "Total", DataField = "Total", SortExpression = "Total", DataFormatString = "S/ {0:N2}" });

                        litTituloResultados.Text = "Reporte de Gasto en Combustible";
                        litTituloGrafico.Text = "Gasto en Combustible por Vehículo";
                        break;
                }

                // Asignar datos al GridView
                gvReporte.DataSource = dt;
                gvReporte.DataBind();

                // Generar gráfico
                chartReporte.Series.Clear();
                chartReporte.ChartAreas.Clear();

                ChartArea chartArea = new ChartArea("ChartArea1");
                chartArea.BackColor = Color.Transparent;
                chartArea.AxisX.Interval = 1;
                chartArea.AxisY.Title = tipoReporteDetalle == "consumo_por_vehiculo" ? "Rendimiento (km/gal)" : "Gasto Total (S/)";
                chartReporte.ChartAreas.Add(chartArea);

                Series serie = new Series("Datos");
                serie.ChartType = SeriesChartType.Column;
                serie.Color = Color.FromArgb(41, 128, 185);

                // Agrupar datos para el gráfico
                var datosAgrupados = dt.AsEnumerable()
                    .GroupBy(r => r.Field<string>("Vehiculo"))
                    .Select(g => new
                    {
                        Vehiculo = g.Key,
                        Rendimiento = g.Average(r => r.Field<decimal>("Rendimiento")),
                        GastoTotal = g.Sum(r => r.Field<decimal>("Total"))
                    })
                    .ToList();

                foreach (var item in datosAgrupados)
                {
                    if (tipoReporteDetalle == "consumo_por_vehiculo")
                        serie.Points.AddXY(item.Vehiculo, Convert.ToDouble(item.Rendimiento));
                    else
                        serie.Points.AddXY(item.Vehiculo, Convert.ToDouble(item.GastoTotal));
                }

                chartReporte.Series.Add(serie);

                // Mostrar panel de resultados
                pnlResultados.Visible = true;
            }
            catch (Exception ex)
            {
                // Manejar el error
                Response.Write("<script>alert('Error al generar reporte: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }

        private void GenerarGrafico(DataTable dt, string tipoReporteDetalle)
        {
            try
            {
                // Limpiar gráfico anterior
                chartReporte.Series.Clear();
                chartReporte.ChartAreas.Clear();

                // Añadir área del gráfico
                ChartArea chartArea = new ChartArea("ChartArea1");
                chartArea.BackColor = Color.Transparent;
                chartArea.AxisX.Interval = 1;
                chartArea.AxisY.Title = "Valores";
                chartArea.AxisY.TitleFont = new Font("Microsoft Sans Serif", 10);
                chartReporte.ChartAreas.Add(chartArea);

                // Configurar según el tipo de reporte
                switch (tipoReporteDetalle)
                {
                    case "rendimiento_conductor":
                        // Gráfico de rendimiento
                        Series serieRendimiento = new Series("Rendimiento");
                        serieRendimiento.ChartType = SeriesChartType.Column;
                        serieRendimiento.Color = Color.FromArgb(41, 128, 185);

                        // Agrupar por conductor
                        var conductores = dt.AsEnumerable()
                            .GroupBy(r => r.Field<string>("Nombre"))
                            .Select(g => new
                            {
                                Conductor = g.Key,
                                Rendimiento = g.Average(r => r.Field<decimal>("Rendimiento"))
                            })
                            .OrderByDescending(x => x.Rendimiento)
                            .Take(5)
                            .ToList();

                        foreach (var conductor in conductores)
                        {
                            serieRendimiento.Points.AddXY(conductor.Conductor, (double)conductor.Rendimiento);
                        }

                        chartReporte.Series.Add(serieRendimiento);
                        break;

                    case "viajes_conductor":
                        // Gráfico de viajes
                        Series serieViajes = new Series("Viajes");
                        serieViajes.ChartType = SeriesChartType.Column;
                        serieViajes.Color = Color.FromArgb(155, 89, 182);

                        // Agrupar por conductor
                        var viajesConductor = dt.AsEnumerable()
                            .GroupBy(r => r.Field<string>("Nombre"))
                            .Select(g => new
                            {
                                Conductor = g.Key,
                                Viajes = g.Count()
                            })
                            .OrderByDescending(x => x.Viajes)
                            .Take(5)
                            .ToList();

                        foreach (var conductor in viajesConductor)
                        {
                            serieViajes.Points.AddXY(conductor.Conductor, conductor.Viajes);
                        }

                        chartReporte.Series.Add(serieViajes);
                        break;

                    case "ingresos_conductor":
                        // Gráfico de ingresos vs gastos
                        Series serieIngresos = new Series("Ingresos");
                        serieIngresos.ChartType = SeriesChartType.Column;
                        serieIngresos.Color = Color.FromArgb(40, 167, 69); // Verde

                        Series serieGastos = new Series("Gastos");
                        serieGastos.ChartType = SeriesChartType.Column;
                        serieGastos.Color = Color.FromArgb(220, 53, 69); // Rojo

                        // Agrupar por conductor
                        var ingresosConductor = dt.AsEnumerable()
                            .GroupBy(r => r.Field<string>("Nombre"))
                            .Select(g => new
                            {
                                Conductor = g.Key,
                                Ingresos = g.Sum(r => r.Field<decimal>("Ingresos")),
                                Gastos = g.Sum(r => r.Field<decimal>("Gastos"))
                            })
                            .OrderByDescending(x => x.Ingresos)
                            .Take(5)
                            .ToList();

                        foreach (var conductor in ingresosConductor)
                        {
                            serieIngresos.Points.AddXY(conductor.Conductor, (double)conductor.Ingresos);
                            serieGastos.Points.AddXY(conductor.Conductor, (double)conductor.Gastos);
                        }

                        chartReporte.Series.Add(serieIngresos);
                        chartReporte.Series.Add(serieGastos);
                        break;
                }

                // Configurar leyenda
                Legend legend = new Legend("Legend1");
                legend.Alignment = StringAlignment.Center;
                legend.Docking = Docking.Bottom;
                chartReporte.Legends.Add(legend);
            }
            catch (Exception ex)
            {
                // Manejar el error
                Response.Write("<script>alert('Error al generar gráfico: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }

        #endregion
    }
}