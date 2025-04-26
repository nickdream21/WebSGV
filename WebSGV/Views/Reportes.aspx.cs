using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.Linq;
using System.Threading;

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
                //CargarPedidos();
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
        }

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            // Limpiar todos los filtros
            txtFechaDesde.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtFechaHasta.Text = DateTime.Now.ToString("yyyy-MM-dd");

            // Restablecer dropdowns a su valor inicial
            if (ddlConductor.Items.Count > 0) ddlConductor.SelectedIndex = 0;
            if (ddlVehiculo.Items.Count > 0) ddlVehiculo.SelectedIndex = 0;
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
            // Cierra el modal de filtros avanzados; los filtros se aplican al generar el reporte
        }

        protected void gvReporte_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReporte.PageIndex = e.NewPageIndex;
            GenerarReporte(); // Volver a cargar los datos con la página actualizada
        }

        protected void gvReporte_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["SortExpression"] = e.SortExpression;
            ViewState["SortDirection"] =
                ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() == e.SortExpression &&
                ViewState["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";

            GenerarReporte(); // Volver a cargar los datos con el ordenamiento actualizado
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Si no hay datos, generar el reporte primero
                if (gvReporte.Rows.Count == 0)
                {
                    GenerarReporte();
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Reporte Viajes");

                    // Título
                    string tituloReporte = litTituloResultados.Text;
                    worksheet.Cell(1, 1).Value = tituloReporte;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    worksheet.Range(1, 1, 1, 15).Merge();
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Período
                    worksheet.Cell(2, 1).Value = "Período: " + txtFechaDesde.Text + " al " + txtFechaHasta.Text;
                    worksheet.Range(2, 1, 2, 15).Merge();

                    // Obtener columnas visibles (excluyendo botones)
                    List<string> columnHeaders = new List<string>();
                    List<int> columnIndexes = new List<int>();

                    for (int i = 0; i < gvReporte.Columns.Count; i++)
                    {
                        if (!(gvReporte.Columns[i] is ButtonField))
                        {
                            columnHeaders.Add(gvReporte.Columns[i].HeaderText);
                            columnIndexes.Add(i);
                        }
                    }

                    // Encabezados
                    for (int i = 0; i < columnHeaders.Count; i++)
                    {
                        worksheet.Cell(4, i + 1).Value = columnHeaders[i];
                        worksheet.Cell(4, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(4, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        worksheet.Cell(4, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    // Datos
                    for (int rowIndex = 0; rowIndex < gvReporte.Rows.Count; rowIndex++)
                    {
                        GridViewRow row = gvReporte.Rows[rowIndex];

                        for (int colIdx = 0; colIdx < columnIndexes.Count; colIdx++)
                        {
                            int originalColIndex = columnIndexes[colIdx];
                            string cellValue = "";

                            if (row.Cells[originalColIndex].Controls.Count > 0)
                            {
                                foreach (Control control in row.Cells[originalColIndex].Controls)
                                {
                                    if (control is Label)
                                        cellValue = ((Label)control).Text;
                                    else if (control is LinkButton)
                                        cellValue = ((LinkButton)control).Text;
                                    else if (control is HyperLink)
                                        cellValue = ((HyperLink)control).Text;
                                }
                            }
                            else
                            {
                                cellValue = row.Cells[originalColIndex].Text;
                            }

                            cellValue = HttpUtility.HtmlDecode(cellValue).Replace(" ", "").Trim();
                            worksheet.Cell(rowIndex + 5, colIdx + 1).Value = cellValue;
                            worksheet.Cell(rowIndex + 5, colIdx + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            if (rowIndex % 2 == 1)
                            {
                                worksheet.Cell(rowIndex + 5, colIdx + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#F9F9F9");
                            }
                        }
                    }

                    // Resumen
                    int summaryRow = (gvReporte.Rows.Count > 0 ? gvReporte.Rows.Count : 0) + 7;
                    worksheet.Cell(summaryRow, 1).Value = "Resumen";
                    worksheet.Cell(summaryRow, 1).Style.Font.Bold = true;
                    summaryRow++;

                    worksheet.Cell(summaryRow, 1).Value = "Total Ingresos:";
                    worksheet.Cell(summaryRow, 2).Value = litTotalIngresos.Text;
                    worksheet.Cell(summaryRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(summaryRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    summaryRow++;

                    worksheet.Cell(summaryRow, 1).Value = "Total Egresos:";
                    worksheet.Cell(summaryRow, 2).Value = litTotalEgresos.Text;
                    worksheet.Cell(summaryRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(summaryRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    summaryRow++;

                    worksheet.Cell(summaryRow, 1).Value = "Balance:";
                    worksheet.Cell(summaryRow, 2).Value = litBalance.Text;
                    worksheet.Cell(summaryRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(summaryRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    summaryRow++;

                    worksheet.Cell(summaryRow, 1).Value = litIndicadorAdicionalTitulo.Text + ":";
                    worksheet.Cell(summaryRow, 2).Value = litIndicadorAdicional.Text;
                    worksheet.Cell(summaryRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(summaryRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    // Ajustar ancho de columnas
                    worksheet.Columns().AdjustToContents();

                    // Enviar al navegador
                    string fileName = "Reporte_Viajes_Conductor_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        ms.Position = 0;
                        Response.BinaryWrite(ms.ToArray());
                        Response.End();
                    }
                }
            }
            catch (ThreadAbortException)
            {
                // Esperado con Response.End()
            }
            catch (Exception ex)
            {
                // En lugar de mostrar el error, generamos un Excel vacío con solo los encabezados
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Reporte Viajes");

                    // Título
                    string tituloReporte = litTituloResultados.Text;
                    worksheet.Cell(1, 1).Value = tituloReporte;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    worksheet.Range(1, 1, 1, 15).Merge();
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Período
                    worksheet.Cell(2, 1).Value = "Período: " + txtFechaDesde.Text + " al " + txtFechaHasta.Text;
                    worksheet.Range(2, 1, 2, 15).Merge();

                    // Encabezados
                    string[] columnHeaders = new string[]
                    {
                "ID", "Nº Orden", "DNI", "Conductor", "Tracto", "Carreta", "Cliente",
                "Producto", "Fecha Salida", "Horas Viaje", "CPIC", "Flete (S/)", "Planta Descarga"
                    };

                    for (int i = 0; i < columnHeaders.Length; i++)
                    {
                        worksheet.Cell(4, i + 1).Value = columnHeaders[i];
                        worksheet.Cell(4, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(4, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        worksheet.Cell(4, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    // Resumen (vacío)
                    int summaryRow = 7;
                    worksheet.Cell(summaryRow, 1).Value = "Resumen";
                    worksheet.Cell(summaryRow, 1).Style.Font.Bold = true;
                    summaryRow++;

                    worksheet.Cell(summaryRow, 1).Value = "Total Ingresos:";
                    worksheet.Cell(summaryRow, 2).Value = "S/ 0.00";
                    worksheet.Cell(summaryRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(summaryRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    summaryRow++;

                    worksheet.Cell(summaryRow, 1).Value = "Total Egresos:";
                    worksheet.Cell(summaryRow, 2).Value = "S/ 0.00";
                    worksheet.Cell(summaryRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(summaryRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    summaryRow++;

                    worksheet.Cell(summaryRow, 1).Value = "Balance:";
                    worksheet.Cell(summaryRow, 2).Value = "S/ 0.00";
                    worksheet.Cell(summaryRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(summaryRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    summaryRow++;

                    worksheet.Cell(summaryRow, 1).Value = "Total Combustible:";
                    worksheet.Cell(summaryRow, 2).Value = "0.00 gal";
                    worksheet.Cell(summaryRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(summaryRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    // Ajustar ancho de columnas
                    worksheet.Columns().AdjustToContents();

                    // Enviar al navegador
                    string fileName = "Reporte_Viajes_Conductor_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        ms.Position = 0;
                        Response.BinaryWrite(ms.ToArray());
                        Response.End();
                    }
                }
            }
        }

        protected void btnExportarPDF_Click(object sender, EventArgs e)
        {
            Response.Write("<script>alert('Función de exportación a PDF en desarrollo.');</script>");
        }

        protected void btnExportarDetalleExcel_Click(object sender, EventArgs e)
        {
            Response.Write("<script>alert('Función de exportación de detalle a Excel en desarrollo.');</script>");
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
                ScriptManager.RegisterStartupScript(this, GetType(), "errorReporte",
                    "alert('Error al generar el reporte: " + ex.Message.Replace("'", "\\'") + "');", true);
            }
        }

        private void GenerarReporte()
        {
            string tipoReporte = ObtenerTipoReporteSeleccionado();
            string tipoReporteDetalle = ddlTipoReporteDetalle.SelectedValue;

            if (tipoReporte == "conductor" && tipoReporteDetalle == "viajes_conductor")
            {
                GenerarReporteViajesConductor();
            }
            else
            {
                GenerarReporteDePrueba();
            }

            pnlResultados.Visible = true;
        }

        private string ObtenerTipoReporteSeleccionado()
        {
            if (lnkConductor.CssClass.Contains("active")) return "conductor";
            if (lnkVehiculo.CssClass.Contains("active")) return "vehiculo";
            if (lnkPedido.CssClass.Contains("active")) return "pedido";
            if (lnkFinanciero.CssClass.Contains("active")) return "financiero";
            if (lnkCombustible.CssClass.Contains("active")) return "combustible";
            if (lnkProducto.CssClass.Contains("active")) return "producto";
            if (lnkPersonalizado.CssClass.Contains("active")) return "personalizado";
            return "conductor"; // Por defecto
        }

        private void GenerarReporteViajesConductor()
        {
            DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);
            DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text);
            string idConductor = ddlConductor.SelectedValue;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT 
                ov.numeroOrdenViaje AS NroOrdenViaje,
                CONCAT(c.nombre, ' ', c.apPaterno, ' ', c.apMaterno) AS NombreConductor,
                t.placaTracto,
                cr.placaCarreta,
                cl.nombre AS Cliente,
                p.nombre AS Producto,
                ov.fechaSalida,
                ov.horaSalida,
                ov.fechaLlegada,
                ov.horaLlegada,
                CASE 
                    WHEN ov.fechaSalida IS NULL OR ov.horaSalida IS NULL 
                      OR ov.fechaLlegada IS NULL OR ov.horaLlegada IS NULL THEN NULL
                    ELSE DATEDIFF(HOUR, 
                        DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ov.horaSalida), CAST(ov.fechaSalida AS datetime)),
                        DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ov.horaLlegada), CAST(ov.fechaLlegada AS datetime))
                    )
                END AS HorasViaje,
                (SELECT TOP 1 plantaDescarga FROM GuiasTransportista 
                 WHERE numeroOrdenViaje = ov.numeroOrdenViaje) AS PlantaDescarga,
                cpic.numeroCPIC
            FROM OrdenViaje ov
            LEFT JOIN Conductor c ON ov.idConductor = c.idConductor
            LEFT JOIN Tracto t ON ov.idTracto = t.idTracto
            LEFT JOIN Carreta cr ON ov.idCarreta = cr.idCarreta
            LEFT JOIN Cliente cl ON ov.idCliente = cl.idCliente
            LEFT JOIN Producto p ON ov.idProducto = p.idProducto
            LEFT JOIN CPIC cpic ON ov.idCPIC = cpic.idCPIC
            WHERE ov.fechaSalida BETWEEN @fechaDesde AND @fechaHasta
            ";

                    if (!string.IsNullOrEmpty(idConductor))
                    {
                        query += " AND c.idConductor = @idConductor";
                    }

                    if (!string.IsNullOrEmpty(txtDNIConductor.Text))
                    {
                        query += " AND c.DNI LIKE @dni";
                    }

                    if (!string.IsNullOrEmpty(txtNombreConductor.Text))
                    {
                        query += " AND (c.nombre LIKE @nombreConductor OR c.apPaterno LIKE @nombreConductor OR c.apMaterno LIKE @nombreConductor)";
                    }

                    query += " ORDER BY ov.fechaSalida DESC, ov.horaSalida DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta);

                    if (!string.IsNullOrEmpty(idConductor))
                    {
                        cmd.Parameters.AddWithValue("@idConductor", idConductor);
                    }

                    if (!string.IsNullOrEmpty(txtDNIConductor.Text))
                    {
                        cmd.Parameters.AddWithValue("@dni", "%" + txtDNIConductor.Text + "%");
                    }

                    if (!string.IsNullOrEmpty(txtNombreConductor.Text))
                    {
                        cmd.Parameters.AddWithValue("@nombreConductor", "%" + txtNombreConductor.Text + "%");
                    }

                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Mostrar
                    ConfigurarGridViewViajesConductor(dt);
                    CalcularIndicadoresConductor(dt);
                    litTituloResultados.Text = "Reporte de Viajes por Conductor";
                    lblTotalRegistros.Text = $"Total registros: {dt.Rows.Count}";
                    gvReporte.DataSource = dt;
                    gvReporte.DataBind();
                }
            }
            catch (Exception ex)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("NroOrdenViaje", typeof(string));
                dt.Columns.Add("NombreConductor", typeof(string));
                dt.Columns.Add("placaTracto", typeof(string));
                dt.Columns.Add("placaCarreta", typeof(string));
                dt.Columns.Add("Cliente", typeof(string));
                dt.Columns.Add("Producto", typeof(string));
                dt.Columns.Add("fechaSalida", typeof(DateTime));
                dt.Columns.Add("horaSalida", typeof(TimeSpan));
                dt.Columns.Add("fechaLlegada", typeof(DateTime));
                dt.Columns.Add("horaLlegada", typeof(TimeSpan));
                dt.Columns.Add("HorasViaje", typeof(int));
                dt.Columns.Add("PlantaDescarga", typeof(string));
                dt.Columns.Add("numeroCPIC", typeof(string));

                ConfigurarGridViewViajesConductor(dt);
                CalcularIndicadoresConductor(dt);
                litTituloResultados.Text = "Reporte de Viajes por Conductor";
                lblTotalRegistros.Text = "Total registros: 0";
                gvReporte.DataSource = dt;
                gvReporte.DataBind();
            }
        }



        private void ConfigurarGridViewViajesConductor(DataTable dt)
        {
            gvReporte.Columns.Clear();

            gvReporte.Columns.Add(new BoundField { DataField = "NroOrdenViaje", HeaderText = "Nº Orden", SortExpression = "NroOrdenViaje" });
            gvReporte.Columns.Add(new BoundField { DataField = "NombreConductor", HeaderText = "Conductor", SortExpression = "NombreConductor" });
            gvReporte.Columns.Add(new BoundField { DataField = "placaTracto", HeaderText = "Tracto", SortExpression = "placaTracto" });
            gvReporte.Columns.Add(new BoundField { DataField = "placaCarreta", HeaderText = "Carreta", SortExpression = "placaCarreta" });
            gvReporte.Columns.Add(new BoundField { DataField = "Cliente", HeaderText = "Cliente", SortExpression = "Cliente" });
            gvReporte.Columns.Add(new BoundField { DataField = "Producto", HeaderText = "Producto", SortExpression = "Producto" });
            gvReporte.Columns.Add(new BoundField { DataField = "fechaSalida", HeaderText = "Fecha Salida", DataFormatString = "{0:dd/MM/yyyy}", SortExpression = "fechaSalida" });
            gvReporte.Columns.Add(new BoundField { DataField = "horaSalida", HeaderText = "Hora Salida", SortExpression = "horaSalida" });
            gvReporte.Columns.Add(new BoundField { DataField = "fechaLlegada", HeaderText = "Fecha Llegada", DataFormatString = "{0:dd/MM/yyyy}", SortExpression = "fechaLlegada" });
            gvReporte.Columns.Add(new BoundField { DataField = "horaLlegada", HeaderText = "Hora Llegada", SortExpression = "horaLlegada" });
            gvReporte.Columns.Add(new BoundField { DataField = "HorasViaje", HeaderText = "Horas Viaje", SortExpression = "HorasViaje" });
            gvReporte.Columns.Add(new BoundField { DataField = "numeroCPIC", HeaderText = "CPIC", SortExpression = "numeroCPIC" });
            gvReporte.Columns.Add(new BoundField { DataField = "PlantaDescarga", HeaderText = "Planta Descarga", SortExpression = "PlantaDescarga" });
        }


        private void CalcularIndicadoresConductor(DataTable dt)
        {
            decimal totalIngresos = 0;
            decimal totalEgresos = 0;
            decimal totalGalones = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataRow row in dt.Rows)
                {
                    string numeroOrden = row["NroOrdenViaje"].ToString();

                    // IN-GRE-SOS (Soles y Dólares)
                    string queryIngresos = @"
                SELECT 
                    ISNULL(SUM(despachoSoles), 0) + ISNULL(SUM(prestamoSoles), 0) + ISNULL(SUM(mensualidadSoles), 0) + ISNULL(SUM(otrosSoles), 0) +
                    ISNULL(SUM(despachoDolares), 0) + ISNULL(SUM(prestamosDolares), 0) + ISNULL(SUM(mensualidadDolares), 0) + ISNULL(SUM(otrosDolares), 0)
                FROM Ingresos
                WHERE numeroOrdenViaje = @numeroOrdenViaje";
                    using (SqlCommand cmd = new SqlCommand(queryIngresos, conn))
                    {
                        cmd.Parameters.AddWithValue("@numeroOrdenViaje", numeroOrden);
                        totalIngresos += Convert.ToDecimal(cmd.ExecuteScalar());
                    }

                    // E-GRE-SOS base
                    string queryEgresos = @"
                SELECT 
                    ISNULL(SUM(peajesSoles + peajesDolares + alimentacionSoles + alimentacionDolares +
                               apoyoseguridadSoles + apoyoseguridadDolares + 
                               reparacionesVariosSoles + repacionesVariosDolares + 
                               movilidadSoles + movilidadDolares + 
                               hospedajeSoles + hospedajeDolares + 
                               combustibleSoles + combustibleDolares + 
                               encarpada_desencarpadaSoles + encarpada_desencarpadaDolares), 0)
                FROM Egresos
                WHERE numeroOrdenViaje = @numeroOrdenViaje";
                    using (SqlCommand cmd = new SqlCommand(queryEgresos, conn))
                    {
                        cmd.Parameters.AddWithValue("@numeroOrdenViaje", numeroOrden);
                        totalEgresos += Convert.ToDecimal(cmd.ExecuteScalar());
                    }

                    // GASTOS ADICIONALES
                    string queryAdicionales = @"
                SELECT 
                    ISNULL(SUM(soles), 0) + ISNULL(SUM(dolares), 0)
                FROM CategoriasAdicionales
                WHERE numeroOrdenViaje = @numeroOrdenViaje";
                    using (SqlCommand cmd = new SqlCommand(queryAdicionales, conn))
                    {
                        cmd.Parameters.AddWithValue("@numeroOrdenViaje", numeroOrden);
                        totalEgresos += Convert.ToDecimal(cmd.ExecuteScalar());
                    }
                }
            }

            // Seteo a la interfaz
            litTotalIngresos.Text = $"S/ {totalIngresos:N2}";
            litTotalEgresos.Text = $"S/ {totalEgresos:N2}";
            litBalance.Text = $"S/ {(totalIngresos - totalEgresos):N2}";
            litIndicadorAdicionalTitulo.Text = "Total Combustible";
            litIndicadorAdicional.Text = $"{totalGalones:N2} gal";
        }


        private void GenerarReporteDePrueba()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Valor", typeof(decimal));

            for (int i = 1; i <= 10; i++)
            {
                dt.Rows.Add(i, $"Item de prueba {i}", i * 100.50m);
            }

            // Configurar GridView
            gvReporte.Columns.Clear();
            BoundField bfId = new BoundField { DataField = "ID", HeaderText = "ID", SortExpression = "ID" };
            BoundField bfDesc = new BoundField { DataField = "Descripción", HeaderText = "Descripción", SortExpression = "Descripción" };
            BoundField bfValor = new BoundField { DataField = "Valor", HeaderText = "Valor", SortExpression = "Valor", DataFormatString = "{0:N2}" };
            gvReporte.Columns.Add(bfId);
            gvReporte.Columns.Add(bfDesc);
            gvReporte.Columns.Add(bfValor);

            // Actualizar indicadores
            decimal totalValor = dt.AsEnumerable().Sum(row => row.Field<decimal>("Valor"));
            litTotalIngresos.Text = $"S/ {totalValor:N2}";
            litTotalEgresos.Text = "S/ 0.00";
            litBalance.Text = $"S/ {totalValor:N2}";
            litIndicadorAdicionalTitulo.Text = "Registros";
            litIndicadorAdicional.Text = dt.Rows.Count.ToString();

            // Actualizar UI
            litTituloResultados.Text = "Reporte de Prueba";
            lblTotalRegistros.Text = $"Total registros: {dt.Rows.Count}";
            gvReporte.DataSource = dt;
            gvReporte.DataBind();
        }

        #endregion
    }
}