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

        // Modificar el método btnExportarExcel_Click para arreglar el problema de espacios y usar el nombre correcto
        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Guardar el estado original de la paginación
                bool paginacionOriginal = gvReporte.AllowPaging;
                int paginaOriginal = gvReporte.PageIndex;

                // Desactivar paginación para obtener todos los registros
                gvReporte.AllowPaging = false;

                // Regenerar el reporte para obtener todos los registros
                GenerarReporte();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Reporte");

                    string numeroPedido = txtNumeroPedido.Text.Trim();
                    string tipoReporte = ObtenerTipoReporteSeleccionado();

                    // Modificar el título si estamos filtrando por número de pedido
                    string tituloReporte = litTituloResultados.Text;
                    if (!string.IsNullOrEmpty(numeroPedido) && tipoReporte == "pedido")
                    {
                        tituloReporte = $"Reporte de Pedido: {numeroPedido}";
                    }

                    // Título del reporte
                    worksheet.Cell(1, 1).Value = tituloReporte;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    worksheet.Range(1, 1, 1, 15).Merge();
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Añadir información del número de pedido en una línea específica si se busca por pedido
                    if (!string.IsNullOrEmpty(numeroPedido) && tipoReporte == "pedido")
                    {
                        worksheet.Cell(2, 1).Value = $"Número de Pedido: {numeroPedido}";
                        worksheet.Cell(2, 1).Style.Font.Bold = true;
                        worksheet.Range(2, 1, 2, 15).Merge();
                        worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        // Período en línea 3
                        worksheet.Cell(3, 1).Value = "Período: " + txtFechaDesde.Text + " al " + txtFechaHasta.Text;
                        worksheet.Range(3, 1, 3, 15).Merge();
                    }
                    else
                    {
                        // Período en línea 2 si no hay pedido específico
                        worksheet.Cell(2, 1).Value = "Período: " + txtFechaDesde.Text + " al " + txtFechaHasta.Text;
                        worksheet.Range(2, 1, 2, 15).Merge();
                    }

                    // Calcular la fila donde empiezan los encabezados
                    int headerRow = (!string.IsNullOrEmpty(numeroPedido) && tipoReporte == "pedido") ? 5 : 4;

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
                        worksheet.Cell(headerRow, i + 1).Value = columnHeaders[i];
                        worksheet.Cell(headerRow, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(headerRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        worksheet.Cell(headerRow, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    // Datos - CORREGIDO: No eliminamos espacios
                    for (int rowIndex = 0; rowIndex < gvReporte.Rows.Count; rowIndex++)
                    {
                        GridViewRow row = gvReporte.Rows[rowIndex];

                        for (int colIdx = 0; colIdx < columnIndexes.Count; colIdx++)
                        {
                            int originalColIndex = columnIndexes[colIdx];
                            string cellValue = "";

                            if (originalColIndex < row.Cells.Count)
                            {
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

                                // Solo decodificar HTML y eliminar &nbsp;, preservando espacios normales
                                cellValue = HttpUtility.HtmlDecode(cellValue).Replace("&nbsp;", "").Trim();
                            }

                            worksheet.Cell(rowIndex + headerRow + 1, colIdx + 1).Value = cellValue;
                            worksheet.Cell(rowIndex + headerRow + 1, colIdx + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            // Destacar el número de pedido si coincide con el buscado
                            if (columnHeaders[colIdx] == "Nº Pedido" && cellValue == numeroPedido)
                            {
                                worksheet.Cell(rowIndex + headerRow + 1, colIdx + 1).Style.Fill.BackgroundColor = XLColor.LightYellow;
                                worksheet.Cell(rowIndex + headerRow + 1, colIdx + 1).Style.Font.Bold = true;
                            }

                            if (rowIndex % 2 == 1)
                            {
                                worksheet.Cell(rowIndex + headerRow + 1, colIdx + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#F9F9F9");
                            }
                        }
                    }

                    // Resumen
                    int summaryRow = (gvReporte.Rows.Count > 0 ? gvReporte.Rows.Count : 0) + headerRow + 3;
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

                    // Determinar el tipo de reporte para el nombre del archivo
                    string prefijo = "Reporte_";

                    // Seleccionar el prefijo según el tipo de reporte
                    switch (tipoReporte)
                    {
                        case "conductor":
                            prefijo += "Viajes_Conductor_";
                            break;
                        case "vehiculo":
                            prefijo += "Viajes_Vehiculo_";
                            break;
                        case "pedido":
                            if (!string.IsNullOrEmpty(numeroPedido))
                                prefijo += "Pedido_" + numeroPedido + "_";
                            else
                                prefijo += "Pedidos_";
                            break;
                        case "financiero":
                            prefijo += "Financiero_";
                            break;
                        case "combustible":
                            prefijo += "Combustible_";
                            break;
                        case "producto":
                            prefijo += "Producto_";
                            break;
                        case "personalizado":
                            prefijo += "Personalizado_";
                            break;
                        default:
                            prefijo += "General_";
                            break;
                    }

                    // Enviar al navegador
                    string fileName = prefijo + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
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

                // Restaurar la paginación original
                gvReporte.AllowPaging = paginacionOriginal;
                if (paginacionOriginal)
                {
                    gvReporte.PageIndex = paginaOriginal;
                    GenerarReporte(); // Volver a cargar el reporte con la paginación original
                }
            }
            catch (ThreadAbortException)
            {
                // Esperado con Response.End()
            }
            catch (Exception ex)
            {
                // Registrar error y mostrar error amigable
                System.Diagnostics.Debug.WriteLine("Error al exportar a Excel: " + ex.Message);

                ScriptManager.RegisterStartupScript(this, GetType(), "errorExport",
                    "alert('Ocurrió un error al exportar a Excel. Por favor, intente nuevamente.\\n" +
                    "Si el problema persiste, contacte al administrador del sistema.');", true);
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

            if (tipoReporte == "conductor")
            {
                // Reportes por conductor
                if (tipoReporteDetalle == "viajes_conductor")
                {
                    GenerarReporteViajesConductor();
                }
                else if (tipoReporteDetalle == "productos_conductor")
                {
                    GenerarReporteProductosConductor();
                }
                else if (tipoReporteDetalle == "financiero_conductor")
                {
                    // Implementar en el futuro
                    GenerarReporteFinancieroConductor();
                }
                else if (tipoReporteDetalle == "combustible_conductor")
                {
                    // Implementar en el futuro
                    GenerarReporteCombustibleConductor();
                }
                else
                {
                    GenerarReporteDePrueba();
                }
            }
            else if (tipoReporte == "pedido")
            {
                // Reportes por pedido
                if (tipoReporteDetalle == "vehiculos_pedido")
                {
                    GenerarReporteVehiculosAsignados();
                }
                else if (tipoReporteDetalle == "conductores_pedido")
                {
                    GenerarReporteConductoresAsignados();
                }
                else if (tipoReporteDetalle == "financiero_pedido")
                {
                    GenerarReporteBalanceFinanciero();
                }
                else
                {
                    // Para los demás tipos de reporte de pedido, usar el reporte general de pedido
                    GenerarReportePedido();
                }
            }
            else if (tipoReporte == "financiero")
            {
                // Reportes financieros
                if (tipoReporteDetalle == "balance_financiero")
                {
                    GenerarReporteBalanceFinanciero();
                }
                else
                {
                    GenerarReporteDePrueba();
                }
            }
            else if (tipoReporte == "vehiculo")
            {
                // Reportes por vehículo
                GenerarReporteDePrueba();
            }
            else if (tipoReporte == "combustible")
            {
                // Reportes de combustible
                GenerarReporteDePrueba();
            }
            else if (tipoReporte == "producto")
            {
                // Reportes por producto
                GenerarReporteDePrueba();
            }
            else if (tipoReporte == "personalizado")
            {
                // Reportes personalizados
                GenerarReporteDePrueba();
            }
            else
            {
                // Reporte por defecto
                GenerarReporteDePrueba();
            }

            pnlResultados.Visible = true;
        }



        // Implementamos el método para generar reportes de pedido
        private void GenerarReportePedido()
        {
            DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);
            DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text);
            string numeroPedido = txtNumeroPedido.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT 
                ov.idOrdenViaje,
                ov.numeroOrdenViaje AS NroOrdenViaje,
                f.numeroPedido AS NumeroPedido,
                cpic.numeroCPIC,
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
                (SELECT TOP 1 pd.nombre 
                 FROM GuiasTransportista gt
                 LEFT JOIN PlantaDescarga pd ON gt.plantaDescarga = pd.nombre OR TRY_CAST(gt.plantaDescarga AS INT) = pd.idPlanta
                 WHERE gt.numeroOrdenViaje = ov.numeroOrdenViaje AND pd.nombre IS NOT NULL) AS PlantaDescarga
            FROM OrdenViaje ov
            LEFT JOIN Conductor c ON ov.idConductor = c.idConductor
            LEFT JOIN Tracto t ON ov.idTracto = t.idTracto
            LEFT JOIN Carreta cr ON ov.idCarreta = cr.idCarreta
            LEFT JOIN Cliente cl ON ov.idCliente = cl.idCliente
            LEFT JOIN Producto p ON ov.idProducto = p.idProducto
            LEFT JOIN CPIC cpic ON ov.idCPIC = cpic.idCPIC
            LEFT JOIN Factura f ON cpic.idFactura = f.idFactura
            WHERE 1=1
            ";

                    // Aplicamos filtros según lo que haya ingresado el usuario
                    if (!string.IsNullOrEmpty(numeroPedido))
                    {
                        // Si hay número de pedido, ese es prioritario
                        query += " AND f.numeroPedido LIKE @numeroPedido";
                    }
                    else
                    {
                        // Si no hay número de pedido, filtramos por fecha
                        query += " AND ov.fechaSalida BETWEEN @fechaDesde AND @fechaHasta";
                    }

                    // Filtros opcionales del modal de filtros avanzados
                    if (!string.IsNullOrEmpty(ddlClientePedido.SelectedValue))
                    {
                        query += " AND ov.idCliente = @idCliente";
                    }

                    if (!string.IsNullOrEmpty(txtNumeroFactura.Text))
                    {
                        query += " AND f.numeroFactura LIKE @numeroFactura";
                    }

                    if (!string.IsNullOrEmpty(txtValorMinimo.Text))
                    {
                        query += " AND cpic.valorTotalFlete >= @valorMinimo";
                    }

                    if (!string.IsNullOrEmpty(txtValorMaximo.Text))
                    {
                        query += " AND cpic.valorTotalFlete <= @valorMaximo";
                    }

                    query += " ORDER BY ov.fechaSalida DESC, ov.horaSalida DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Añadimos parámetros a la consulta
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta);

                    if (!string.IsNullOrEmpty(numeroPedido))
                    {
                        cmd.Parameters.AddWithValue("@numeroPedido", "%" + numeroPedido + "%");
                    }

                    if (!string.IsNullOrEmpty(ddlClientePedido.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@idCliente", ddlClientePedido.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(txtNumeroFactura.Text))
                    {
                        cmd.Parameters.AddWithValue("@numeroFactura", "%" + txtNumeroFactura.Text + "%");
                    }

                    if (!string.IsNullOrEmpty(txtValorMinimo.Text))
                    {
                        cmd.Parameters.AddWithValue("@valorMinimo", decimal.Parse(txtValorMinimo.Text));
                    }

                    if (!string.IsNullOrEmpty(txtValorMaximo.Text))
                    {
                        cmd.Parameters.AddWithValue("@valorMaximo", decimal.Parse(txtValorMaximo.Text));
                    }

                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Configurar el GridView para mostrar los datos
                    ConfigurarGridViewPedido(dt);

                    // Calcular los indicadores para el reporte
                    CalcularIndicadoresPedido(dt);

                    // Actualizar la interfaz
                    string titulo = !string.IsNullOrEmpty(numeroPedido)
                        ? $"Reporte de Pedido: {numeroPedido}"
                        : "Reporte de Pedidos";

                    litTituloResultados.Text = titulo;
                    lblTotalRegistros.Text = $"Total registros: {dt.Rows.Count}";
                    gvReporte.DataSource = dt;
                    gvReporte.DataBind();
                }
            }
            catch (Exception ex)
            {
                // En caso de error, creamos una tabla vacía con las columnas esperadas
                DataTable dt = new DataTable();
                dt.Columns.Add("idOrdenViaje", typeof(int));
                dt.Columns.Add("NroOrdenViaje", typeof(string));
                dt.Columns.Add("NumeroPedido", typeof(string));
                dt.Columns.Add("numeroCPIC", typeof(string));
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

                ConfigurarGridViewPedido(dt);
                CalcularIndicadoresPedido(dt);

                string titulo = !string.IsNullOrEmpty(numeroPedido)
                    ? $"Reporte de Pedido: {numeroPedido}"
                    : "Reporte de Pedidos";

                litTituloResultados.Text = titulo;
                lblTotalRegistros.Text = "Total registros: 0";
                gvReporte.DataSource = dt;
                gvReporte.DataBind();

                // Registrar el error
                System.Diagnostics.Debug.WriteLine("Error al generar reporte de pedido: " + ex.Message);
            }
        }

        private void ConfigurarGridViewPedido(DataTable dt)
        {
            gvReporte.Columns.Clear();

            gvReporte.Columns.Add(new BoundField { DataField = "NroOrdenViaje", HeaderText = "Nº Orden", SortExpression = "NroOrdenViaje" });
            gvReporte.Columns.Add(new BoundField { DataField = "NumeroPedido", HeaderText = "Número de Pedido", SortExpression = "NumeroPedido" });
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
            gvReporte.Columns.Add(new BoundField { DataField = "PlantaDescarga", HeaderText = "Planta Descarga", SortExpression = "PlantaDescarga" });
        }

        private void CalcularIndicadoresPedido(DataTable dt)
        {
            decimal totalIngresos = 0;
            decimal totalEgresos = 0;
            int totalPedidos = dt.Rows.Count;
            int totalHorasViaje = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataRow row in dt.Rows)
                {
                    string numeroOrden = row["NroOrdenViaje"].ToString();

                    // Sumar las horas de viaje para el indicador adicional
                    if (row["HorasViaje"] != DBNull.Value)
                    {
                        totalHorasViaje += Convert.ToInt32(row["HorasViaje"]);
                    }

                    // Calcular ingresos
                    string queryIngresos = @"
            SELECT 
                ISNULL(SUM(despachoSoles), 0) + ISNULL(SUM(prestamoSoles), 0) + ISNULL(SUM(mensualidadSoles), 0) + ISNULL(SUM(otrosSoles), 0) +
                ISNULL(SUM(despachoDolares), 0) + ISNULL(SUM(prestamosDolares), 0) + ISNULL(SUM(mensualidadDolares), 0) + ISNULL(SUM(otrosDolares), 0)
            FROM Ingresos
            WHERE numeroOrdenViaje = @numeroOrdenViaje";

                    using (SqlCommand cmd = new SqlCommand(queryIngresos, conn))
                    {
                        cmd.Parameters.AddWithValue("@numeroOrdenViaje", numeroOrden);
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            totalIngresos += Convert.ToDecimal(result);
                        }
                    }

                    // Calcular egresos
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
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            totalEgresos += Convert.ToDecimal(result);
                        }
                    }

                    // Calcular gastos adicionales
                    string queryAdicionales = @"
            SELECT 
                ISNULL(SUM(soles), 0) + ISNULL(SUM(dolares), 0)
            FROM CategoriasAdicionales
            WHERE numeroOrdenViaje = @numeroOrdenViaje";

                    using (SqlCommand cmd = new SqlCommand(queryAdicionales, conn))
                    {
                        cmd.Parameters.AddWithValue("@numeroOrdenViaje", numeroOrden);
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            totalEgresos += Convert.ToDecimal(result);
                        }
                    }
                }
            }

            // Actualizar interfaz con los indicadores
            litTotalIngresos.Text = $"S/ {totalIngresos:N2}";
            litTotalEgresos.Text = $"S/ {totalEgresos:N2}";
            litBalance.Text = $"S/ {(totalIngresos - totalEgresos):N2}";
            litIndicadorAdicionalTitulo.Text = "Total Horas de Viaje";
            litIndicadorAdicional.Text = $"{totalHorasViaje} hrs";
        }



        //reporte vehivulkos asignados por pedido

        private void GenerarReporteVehiculosAsignados()
        {
            DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);
            DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text);
            string numeroPedido = txtNumeroPedido.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT 
                ov.idOrdenViaje,
                ov.numeroOrdenViaje AS NroOrdenViaje,
                f.numeroPedido AS NumeroPedido,
                cpic.numeroCPIC,
                t.placaTracto,
                t.marca AS MarcaTracto,
                t.modelo AS ModeloTracto,
                cr.placaCarreta,
                cr.marca AS MarcaCarreta,
                cr.modelo AS ModeloCarreta,
                CONCAT(c.nombre, ' ', c.apPaterno, ' ', c.apMaterno) AS Conductor,
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
                (SELECT TOP 1 pd.nombre 
                 FROM GuiasTransportista gt
                 LEFT JOIN PlantaDescarga pd ON gt.plantaDescarga = pd.nombre OR TRY_CAST(gt.plantaDescarga AS INT) = pd.idPlanta
                 WHERE gt.numeroOrdenViaje = ov.numeroOrdenViaje AND pd.nombre IS NOT NULL) AS PlantaDescarga
            FROM OrdenViaje ov
            LEFT JOIN Conductor c ON ov.idConductor = c.idConductor
            LEFT JOIN Tracto t ON ov.idTracto = t.idTracto
            LEFT JOIN Carreta cr ON ov.idCarreta = cr.idCarreta
            LEFT JOIN Cliente cl ON ov.idCliente = cl.idCliente
            LEFT JOIN Producto p ON ov.idProducto = p.idProducto
            LEFT JOIN CPIC cpic ON ov.idCPIC = cpic.idCPIC
            LEFT JOIN Factura f ON cpic.idFactura = f.idFactura
            WHERE 1=1
            ";

                    // Aplicamos filtros según lo que haya ingresado el usuario
                    if (!string.IsNullOrEmpty(numeroPedido))
                    {
                        // Si hay número de pedido, ese es prioritario
                        query += " AND f.numeroPedido LIKE @numeroPedido";
                    }
                    else
                    {
                        // Si no hay número de pedido, filtramos por fecha de salida
                        query += " AND ov.fechaSalida BETWEEN @fechaDesde AND @fechaHasta";
                    }

                    // Filtros opcionales del modal de filtros avanzados
                    if (!string.IsNullOrEmpty(ddlClientePedido.SelectedValue))
                    {
                        query += " AND ov.idCliente = @idCliente";
                    }

                    if (!string.IsNullOrEmpty(txtPlacaVehiculo.Text))
                    {
                        query += " AND (t.placaTracto LIKE @placaVehiculo OR cr.placaCarreta LIKE @placaVehiculo)";
                    }

                    if (!string.IsNullOrEmpty(ddlMarcaVehiculo.SelectedValue))
                    {
                        query += " AND (t.marca = @marcaVehiculo OR cr.marca = @marcaVehiculo)";
                    }

                    if (!string.IsNullOrEmpty(ddlModeloVehiculo.SelectedValue))
                    {
                        query += " AND (t.modelo = @modeloVehiculo OR cr.modelo = @modeloVehiculo)";
                    }

                    query += " ORDER BY ov.fechaSalida DESC, ov.horaSalida DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Añadimos parámetros a la consulta
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta);

                    if (!string.IsNullOrEmpty(numeroPedido))
                    {
                        cmd.Parameters.AddWithValue("@numeroPedido", "%" + numeroPedido + "%");
                    }

                    if (!string.IsNullOrEmpty(ddlClientePedido.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@idCliente", ddlClientePedido.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(txtPlacaVehiculo.Text))
                    {
                        cmd.Parameters.AddWithValue("@placaVehiculo", "%" + txtPlacaVehiculo.Text + "%");
                    }

                    if (!string.IsNullOrEmpty(ddlMarcaVehiculo.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@marcaVehiculo", ddlMarcaVehiculo.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(ddlModeloVehiculo.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@modeloVehiculo", ddlModeloVehiculo.SelectedValue);
                    }

                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Configurar el GridView para mostrar los datos
                    ConfigurarGridViewVehiculosAsignados(dt);

                    // Calcular los indicadores para el reporte
                    CalcularIndicadoresVehiculosAsignados(dt);

                    // Actualizar la interfaz
                    string titulo = !string.IsNullOrEmpty(numeroPedido)
                        ? $"Vehículos Asignados - Pedido: {numeroPedido}"
                        : "Vehículos Asignados por Período";

                    litTituloResultados.Text = titulo;
                    lblTotalRegistros.Text = $"Total registros: {dt.Rows.Count}";
                    gvReporte.DataSource = dt;
                    gvReporte.DataBind();
                }
            }
            catch (Exception ex)
            {
                // En caso de error, creamos una tabla vacía con las columnas esperadas
                DataTable dt = new DataTable();
                dt.Columns.Add("idOrdenViaje", typeof(int));
                dt.Columns.Add("NroOrdenViaje", typeof(string));
                dt.Columns.Add("NumeroPedido", typeof(string));
                dt.Columns.Add("numeroCPIC", typeof(string));
                dt.Columns.Add("placaTracto", typeof(string));
                dt.Columns.Add("MarcaTracto", typeof(string));
                dt.Columns.Add("ModeloTracto", typeof(string));
                dt.Columns.Add("placaCarreta", typeof(string));
                dt.Columns.Add("MarcaCarreta", typeof(string));
                dt.Columns.Add("ModeloCarreta", typeof(string));
                dt.Columns.Add("Conductor", typeof(string));
                dt.Columns.Add("Cliente", typeof(string));
                dt.Columns.Add("fechaSalida", typeof(DateTime));
                dt.Columns.Add("fechaLlegada", typeof(DateTime));
                dt.Columns.Add("HorasViaje", typeof(int));
                dt.Columns.Add("PlantaDescarga", typeof(string));

                ConfigurarGridViewVehiculosAsignados(dt);
                CalcularIndicadoresVehiculosAsignados(dt);

                string titulo = !string.IsNullOrEmpty(numeroPedido)
                    ? $"Vehículos Asignados - Pedido: {numeroPedido}"
                    : "Vehículos Asignados por Período";

                litTituloResultados.Text = titulo;
                lblTotalRegistros.Text = "Total registros: 0";
                gvReporte.DataSource = dt;
                gvReporte.DataBind();

                // Registrar el error
                System.Diagnostics.Debug.WriteLine("Error al generar reporte de vehículos asignados: " + ex.Message);
            }
        }

        private void ConfigurarGridViewVehiculosAsignados(DataTable dt)
        {
            gvReporte.Columns.Clear();

            gvReporte.Columns.Add(new BoundField { DataField = "NroOrdenViaje", HeaderText = "Nº Orden", SortExpression = "NroOrdenViaje" });
            gvReporte.Columns.Add(new BoundField { DataField = "NumeroPedido", HeaderText = "Número de Pedido", SortExpression = "NumeroPedido" });
            gvReporte.Columns.Add(new BoundField { DataField = "numeroCPIC", HeaderText = "CPIC", SortExpression = "numeroCPIC" });
            gvReporte.Columns.Add(new BoundField { DataField = "placaTracto", HeaderText = "Placa Tracto", SortExpression = "placaTracto" });
            gvReporte.Columns.Add(new BoundField { DataField = "MarcaTracto", HeaderText = "Marca", SortExpression = "MarcaTracto" });
            gvReporte.Columns.Add(new BoundField { DataField = "ModeloTracto", HeaderText = "Modelo", SortExpression = "ModeloTracto" });
            gvReporte.Columns.Add(new BoundField { DataField = "placaCarreta", HeaderText = "Placa Carreta", SortExpression = "placaCarreta" });
            gvReporte.Columns.Add(new BoundField { DataField = "Conductor", HeaderText = "Conductor", SortExpression = "Conductor" });
            gvReporte.Columns.Add(new BoundField { DataField = "Cliente", HeaderText = "Cliente", SortExpression = "Cliente" });
            gvReporte.Columns.Add(new BoundField { DataField = "fechaSalida", HeaderText = "Fecha Salida", DataFormatString = "{0:dd/MM/yyyy}", SortExpression = "fechaSalida" });
            gvReporte.Columns.Add(new BoundField { DataField = "fechaLlegada", HeaderText = "Fecha Llegada", DataFormatString = "{0:dd/MM/yyyy}", SortExpression = "fechaLlegada" });
            gvReporte.Columns.Add(new BoundField { DataField = "HorasViaje", HeaderText = "Horas Viaje", SortExpression = "HorasViaje" });
            gvReporte.Columns.Add(new BoundField { DataField = "PlantaDescarga", HeaderText = "Planta Descarga", SortExpression = "PlantaDescarga" });
        }

        private void CalcularIndicadoresVehiculosAsignados(DataTable dt)
        {
            int totalVehiculos = dt.DefaultView.ToTable(true, "placaTracto").Rows.Count;
            int totalCarretas = dt.DefaultView.ToTable(true, "placaCarreta").Rows.Count;
            int totalViajes = dt.Rows.Count;
            int totalHoras = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row["HorasViaje"] != DBNull.Value)
                {
                    totalHoras += Convert.ToInt32(row["HorasViaje"]);
                }
            }

            // Actualizar interfaz con los indicadores
            litTotalIngresos.Text = $"{totalVehiculos} vehículos";
            litTotalEgresos.Text = $"{totalCarretas} carretas";
            litBalance.Text = $"{totalViajes} viajes";
            litIndicadorAdicionalTitulo.Text = "Total Horas";
            litIndicadorAdicional.Text = $"{totalHoras} hrs";
        }

        //reporte conductores asignados por pedido

        private void GenerarReporteConductoresAsignados()
        {
            DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);
            DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text);
            string numeroPedido = txtNumeroPedido.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT 
                ov.idOrdenViaje,
                ov.numeroOrdenViaje AS NroOrdenViaje,
                f.numeroPedido AS NumeroPedido,
                cpic.numeroCPIC,
                c.DNI,
                c.carnetExtranjeria,
                CONCAT(c.nombre, ' ', c.apPaterno, ' ', c.apMaterno) AS NombreConductor,
                c.telefono,
                c.correo,
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
                (SELECT TOP 1 gt.ruta1 FROM GuiasTransportista gt WHERE gt.numeroOrdenViaje = ov.numeroOrdenViaje) AS Ruta,
                (SELECT TOP 1 pd.nombre 
                 FROM GuiasTransportista gt
                 LEFT JOIN PlantaDescarga pd ON gt.plantaDescarga = pd.nombre OR TRY_CAST(gt.plantaDescarga AS INT) = pd.idPlanta
                 WHERE gt.numeroOrdenViaje = ov.numeroOrdenViaje AND pd.nombre IS NOT NULL) AS PlantaDescarga
            FROM OrdenViaje ov
            JOIN CPIC cpic ON ov.idCPIC = cpic.idCPIC
            JOIN Factura f ON cpic.idFactura = f.idFactura
            LEFT JOIN Conductor c ON ov.idConductor = c.idConductor
            LEFT JOIN Tracto t ON ov.idTracto = t.idTracto
            LEFT JOIN Carreta cr ON ov.idCarreta = cr.idCarreta
            LEFT JOIN Cliente cl ON ov.idCliente = cl.idCliente
            LEFT JOIN Producto p ON ov.idProducto = p.idProducto
            WHERE 1=1
            ";

                    // Si hay número de pedido, ese es prioritario
                    if (!string.IsNullOrEmpty(numeroPedido))
                    {
                        query += " AND f.numeroPedido = @numeroPedido";
                    }
                    else
                    {
                        // Si no hay número de pedido, filtramos por fecha
                        query += " AND ov.fechaSalida BETWEEN @fechaDesde AND @fechaHasta";
                    }

                    // Filtros opcionales del modal de filtros avanzados
                    if (!string.IsNullOrEmpty(ddlClientePedido.SelectedValue))
                    {
                        query += " AND ov.idCliente = @idCliente";
                    }

                    if (!string.IsNullOrEmpty(txtNombreConductor.Text))
                    {
                        query += " AND (c.nombre LIKE @nombreConductor OR c.apPaterno LIKE @nombreConductor OR c.apMaterno LIKE @nombreConductor)";
                    }

                    if (!string.IsNullOrEmpty(txtDNIConductor.Text))
                    {
                        query += " AND c.DNI LIKE @dniConductor";
                    }

                    query += " ORDER BY c.apPaterno, c.apMaterno, c.nombre, ov.fechaSalida DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Añadimos parámetros a la consulta
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta);

                    if (!string.IsNullOrEmpty(numeroPedido))
                    {
                        cmd.Parameters.AddWithValue("@numeroPedido", numeroPedido);
                    }

                    if (!string.IsNullOrEmpty(ddlClientePedido.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@idCliente", ddlClientePedido.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(txtNombreConductor.Text))
                    {
                        cmd.Parameters.AddWithValue("@nombreConductor", "%" + txtNombreConductor.Text + "%");
                    }

                    if (!string.IsNullOrEmpty(txtDNIConductor.Text))
                    {
                        cmd.Parameters.AddWithValue("@dniConductor", "%" + txtDNIConductor.Text + "%");
                    }

                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Configurar el GridView para mostrar los datos
                    ConfigurarGridViewConductoresAsignados(dt);

                    // Calcular los indicadores para el reporte
                    CalcularIndicadoresConductoresAsignados(dt);

                    // Actualizar la interfaz
                    string titulo = !string.IsNullOrEmpty(numeroPedido)
                        ? $"Conductores Asignados - Pedido: {numeroPedido}"
                        : "Conductores Asignados por Período";

                    litTituloResultados.Text = titulo;
                    lblTotalRegistros.Text = $"Total registros: {dt.Rows.Count}";
                    gvReporte.DataSource = dt;
                    gvReporte.DataBind();
                }
            }
            catch (Exception ex)
            {
                // En caso de error, creamos una tabla vacía con las columnas esperadas
                DataTable dt = new DataTable();
                dt.Columns.Add("idOrdenViaje", typeof(int));
                dt.Columns.Add("NroOrdenViaje", typeof(string));
                dt.Columns.Add("NumeroPedido", typeof(string));
                dt.Columns.Add("numeroCPIC", typeof(string));
                dt.Columns.Add("DNI", typeof(string));
                dt.Columns.Add("NombreConductor", typeof(string));
                dt.Columns.Add("telefono", typeof(string));
                dt.Columns.Add("placaTracto", typeof(string));
                dt.Columns.Add("placaCarreta", typeof(string));
                dt.Columns.Add("Cliente", typeof(string));
                dt.Columns.Add("Producto", typeof(string));
                dt.Columns.Add("fechaSalida", typeof(DateTime));
                dt.Columns.Add("fechaLlegada", typeof(DateTime));
                dt.Columns.Add("HorasViaje", typeof(int));
                dt.Columns.Add("Ruta", typeof(string));
                dt.Columns.Add("PlantaDescarga", typeof(string));

                ConfigurarGridViewConductoresAsignados(dt);
                CalcularIndicadoresConductoresAsignados(dt);

                string titulo = !string.IsNullOrEmpty(numeroPedido)
                    ? $"Conductores Asignados - Pedido: {numeroPedido}"
                    : "Conductores Asignados por Período";

                litTituloResultados.Text = titulo;
                lblTotalRegistros.Text = "Total registros: 0";
                gvReporte.DataSource = dt;
                gvReporte.DataBind();

                // Registrar el error
                System.Diagnostics.Debug.WriteLine("Error al generar reporte de conductores asignados: " + ex.Message);
            }
        }

        private void ConfigurarGridViewConductoresAsignados(DataTable dt)
        {
            gvReporte.Columns.Clear();

            gvReporte.Columns.Add(new BoundField { DataField = "NroOrdenViaje", HeaderText = "Nº Orden", SortExpression = "NroOrdenViaje" });
            gvReporte.Columns.Add(new BoundField { DataField = "NumeroPedido", HeaderText = "Número de Pedido", SortExpression = "NumeroPedido" });
            gvReporte.Columns.Add(new BoundField { DataField = "NombreConductor", HeaderText = "Conductor", SortExpression = "NombreConductor" });
            gvReporte.Columns.Add(new BoundField { DataField = "DNI", HeaderText = "DNI", SortExpression = "DNI" });
            gvReporte.Columns.Add(new BoundField { DataField = "telefono", HeaderText = "Teléfono", SortExpression = "telefono" });
            gvReporte.Columns.Add(new BoundField { DataField = "placaTracto", HeaderText = "Placa Tracto", SortExpression = "placaTracto" });
            gvReporte.Columns.Add(new BoundField { DataField = "placaCarreta", HeaderText = "Placa Carreta", SortExpression = "placaCarreta" });
            gvReporte.Columns.Add(new BoundField { DataField = "Cliente", HeaderText = "Cliente", SortExpression = "Cliente" });
            gvReporte.Columns.Add(new BoundField { DataField = "Producto", HeaderText = "Producto", SortExpression = "Producto" });
            gvReporte.Columns.Add(new BoundField { DataField = "fechaSalida", HeaderText = "Fecha Salida", DataFormatString = "{0:dd/MM/yyyy}", SortExpression = "fechaSalida" });
            gvReporte.Columns.Add(new BoundField { DataField = "horaSalida", HeaderText = "Hora Salida", SortExpression = "horaSalida" });
            gvReporte.Columns.Add(new BoundField { DataField = "fechaLlegada", HeaderText = "Fecha Llegada", DataFormatString = "{0:dd/MM/yyyy}", SortExpression = "fechaLlegada" });
            gvReporte.Columns.Add(new BoundField { DataField = "horaLlegada", HeaderText = "Hora Llegada", SortExpression = "horaLlegada" });
            gvReporte.Columns.Add(new BoundField { DataField = "HorasViaje", HeaderText = "Horas Viaje", SortExpression = "HorasViaje" });
            gvReporte.Columns.Add(new BoundField { DataField = "Ruta", HeaderText = "Ruta", SortExpression = "Ruta" });
            gvReporte.Columns.Add(new BoundField { DataField = "PlantaDescarga", HeaderText = "Planta Descarga", SortExpression = "PlantaDescarga" });
        }

        private void CalcularIndicadoresConductoresAsignados(DataTable dt)
        {
            int totalConductores = dt.DefaultView.ToTable(true, "NombreConductor").Rows.Count;
            int totalViajes = dt.Rows.Count;
            int totalHoras = 0;
            double promedioHorasPorViaje = 0;

            // Contar cuántos viajes tiene cada conductor
            Dictionary<string, int> viajesPorConductor = new Dictionary<string, int>();
            Dictionary<string, int> horasPorConductor = new Dictionary<string, int>();

            foreach (DataRow row in dt.Rows)
            {
                string conductor = row["NombreConductor"].ToString();

                if (!viajesPorConductor.ContainsKey(conductor))
                {
                    viajesPorConductor[conductor] = 0;
                    horasPorConductor[conductor] = 0;
                }

                viajesPorConductor[conductor]++;

                if (row["HorasViaje"] != DBNull.Value)
                {
                    int horas = Convert.ToInt32(row["HorasViaje"]);
                    totalHoras += horas;
                    horasPorConductor[conductor] += horas;
                }
            }

            // Calcular promedio de horas por viaje
            if (totalViajes > 0)
            {
                promedioHorasPorViaje = (double)totalHoras / totalViajes;
            }

            // Encontrar el conductor con más viajes
            string conductorMasViajes = "";
            int maxViajes = 0;

            foreach (var kvp in viajesPorConductor)
            {
                if (kvp.Value > maxViajes)
                {
                    maxViajes = kvp.Value;
                    conductorMasViajes = kvp.Key;
                }
            }

            // Actualizar interfaz con los indicadores
            litTotalIngresos.Text = $"{totalConductores} conductores";
            litTotalEgresos.Text = $"{totalViajes} viajes";
            litBalance.Text = $"{promedioHorasPorViaje:F1} hrs/viaje";
            litIndicadorAdicionalTitulo.Text = "Conductor con más viajes";
            litIndicadorAdicional.Text = conductorMasViajes + " (" + maxViajes + " viajes)";
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

        //REPORTE BALANCEFINANCIERO  POR PEDIDO

        private void GenerarReporteBalanceFinanciero()
        {
            DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);
            DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text);
            string numeroPedido = txtNumeroPedido.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            -- Ingresos
            SELECT 
                ov.numeroOrdenViaje AS NroOrdenViaje,
                f.numeroPedido AS NumeroPedido,
                ov.fechaSalida AS FechaTransaccion,
                'Ingreso' AS TipoTransaccion,
                CASE 
                    WHEN i.despachoSoles > 0 OR i.despachoDolares > 0 THEN 'Despacho'
                    WHEN i.prestamoSoles > 0 OR i.prestamosDolares > 0 THEN 'Préstamo'
                    WHEN i.mensualidadSoles > 0 OR i.mensualidadDolares > 0 THEN 'Mensualidad'
                    WHEN i.otrosSoles > 0 OR i.otrosDolares > 0 THEN 'Otros'
                    ELSE 'No especificado'
                END AS Concepto,
                CONCAT(c.nombre, ' ', c.apPaterno, ' ', c.apMaterno) AS Conductor,
                cl.nombre AS Cliente,
                ISNULL(i.despachoSoles, 0) + ISNULL(i.prestamoSoles, 0) + 
                ISNULL(i.mensualidadSoles, 0) + ISNULL(i.otrosSoles, 0) AS IngresoSoles,
                ISNULL(i.despachoDolares, 0) + ISNULL(i.prestamosDolares, 0) + 
                ISNULL(i.mensualidadDolares, 0) + ISNULL(i.otrosDolares, 0) AS IngresoDolares,
                0 AS EgresoSoles,
                0 AS EgresoDolares
            FROM OrdenViaje ov
            JOIN CPIC cpic ON ov.idCPIC = cpic.idCPIC
            JOIN Factura f ON cpic.idFactura = f.idFactura
            JOIN Ingresos i ON ov.numeroOrdenViaje = i.numeroOrdenViaje
            LEFT JOIN Conductor c ON ov.idConductor = c.idConductor
            LEFT JOIN Cliente cl ON ov.idCliente = cl.idCliente
            WHERE 1=1
            
            UNION ALL
            
            -- Egresos
            SELECT 
                ov.numeroOrdenViaje AS NroOrdenViaje,
                f.numeroPedido AS NumeroPedido,
                ov.fechaSalida AS FechaTransaccion,
                'Egreso' AS TipoTransaccion,
                CASE 
                    WHEN e.peajesSoles > 0 OR e.peajesDolares > 0 THEN 'Peaje'
                    WHEN e.alimentacionSoles > 0 OR e.alimentacionDolares > 0 THEN 'Alimentación'
                    WHEN e.combustibleSoles > 0 OR e.combustibleDolares > 0 THEN 'Combustible'
                    WHEN e.hospedajeSoles > 0 OR e.hospedajeDolares > 0 THEN 'Hospedaje'
                    ELSE 'Otros gastos'
                END AS Concepto,
                CONCAT(c.nombre, ' ', c.apPaterno, ' ', c.apMaterno) AS Conductor,
                cl.nombre AS Cliente,
                0 AS IngresoSoles,
                0 AS IngresoDolares,
                ISNULL(e.peajesSoles, 0) + ISNULL(e.alimentacionSoles, 0) + 
                ISNULL(e.apoyoseguridadSoles, 0) + ISNULL(e.reparacionesVariosSoles, 0) + 
                ISNULL(e.movilidadSoles, 0) + ISNULL(e.hospedajeSoles, 0) + 
                ISNULL(e.combustibleSoles, 0) + ISNULL(e.encarpada_desencarpadaSoles, 0) AS EgresoSoles,
                ISNULL(e.peajesDolares, 0) + ISNULL(e.alimentacionDolares, 0) + 
                ISNULL(e.apoyoseguridadDolares, 0) + ISNULL(e.repacionesVariosDolares, 0) + 
                ISNULL(e.movilidadDolares, 0) + ISNULL(e.hospedajeDolares, 0) + 
                ISNULL(e.combustibleDolares, 0) + ISNULL(e.encarpada_desencarpadaDolares, 0) AS EgresoDolares
            FROM OrdenViaje ov
            JOIN CPIC cpic ON ov.idCPIC = cpic.idCPIC
            JOIN Factura f ON cpic.idFactura = f.idFactura
            JOIN Egresos e ON ov.numeroOrdenViaje = e.numeroOrdenViaje
            LEFT JOIN Conductor c ON ov.idConductor = c.idConductor
            LEFT JOIN Cliente cl ON ov.idCliente = cl.idCliente
            WHERE 1=1
            
            UNION ALL
            
            -- Categorías Adicionales (gastos adicionales)
            SELECT 
                ov.numeroOrdenViaje AS NroOrdenViaje,
                f.numeroPedido AS NumeroPedido,
                ov.fechaSalida AS FechaTransaccion,
                'Egreso' AS TipoTransaccion,
                ca.nombreCategoria AS Concepto,
                CONCAT(c.nombre, ' ', c.apPaterno, ' ', c.apMaterno) AS Conductor,
                cl.nombre AS Cliente,
                0 AS IngresoSoles,
                0 AS IngresoDolares,
                ISNULL(ca.soles, 0) AS EgresoSoles,
                ISNULL(ca.dolares, 0) AS EgresoDolares
            FROM OrdenViaje ov
            JOIN CPIC cpic ON ov.idCPIC = cpic.idCPIC
            JOIN Factura f ON cpic.idFactura = f.idFactura
            JOIN CategoriasAdicionales ca ON ov.numeroOrdenViaje = ca.numeroOrdenViaje
            LEFT JOIN Conductor c ON ov.idConductor = c.idConductor
            LEFT JOIN Cliente cl ON ov.idCliente = cl.idCliente
            WHERE 1=1
            ";

                    // Aplicamos filtros según lo que haya ingresado el usuario
                    if (!string.IsNullOrEmpty(numeroPedido))
                    {
                        // Si hay número de pedido, ese es prioritario
                        query += " AND f.numeroPedido = @numeroPedido";
                    }
                    else
                    {
                        // Si no hay número de pedido, filtramos por fecha
                        query += " AND ov.fechaSalida BETWEEN @fechaDesde AND @fechaHasta";
                    }

                    // Filtros adicionales si se seleccionó cliente o tipo de transacción
                    if (!string.IsNullOrEmpty(ddlClientePedido.SelectedValue))
                    {
                        query += " AND ov.idCliente = @idCliente";
                    }

                    if (!string.IsNullOrEmpty(ddlTipoTransaccion.SelectedValue))
                    {
                        query += " AND TipoTransaccion = @tipoTransaccion";
                    }

                    // Valores mínimos y máximos si están presentes
                    if (!string.IsNullOrEmpty(txtMontoMinimo.Text))
                    {
                        query += " AND ((IngresoSoles >= @montoMinimo) OR (IngresoDolares >= @montoMinimo) OR (EgresoSoles >= @montoMinimo) OR (EgresoDolares >= @montoMinimo))";
                    }

                    if (!string.IsNullOrEmpty(txtMontoMaximo.Text))
                    {
                        query += " AND ((IngresoSoles <= @montoMaximo) OR (IngresoDolares <= @montoMaximo) OR (EgresoSoles <= @montoMaximo) OR (EgresoDolares <= @montoMaximo))";
                    }

                    query += " ORDER BY FechaTransaccion, NroOrdenViaje, TipoTransaccion";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Añadimos parámetros a la consulta
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta);

                    if (!string.IsNullOrEmpty(numeroPedido))
                    {
                        cmd.Parameters.AddWithValue("@numeroPedido", numeroPedido);
                    }

                    if (!string.IsNullOrEmpty(ddlClientePedido.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@idCliente", ddlClientePedido.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(ddlTipoTransaccion.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@tipoTransaccion", ddlTipoTransaccion.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(txtMontoMinimo.Text))
                    {
                        cmd.Parameters.AddWithValue("@montoMinimo", decimal.Parse(txtMontoMinimo.Text));
                    }

                    if (!string.IsNullOrEmpty(txtMontoMaximo.Text))
                    {
                        cmd.Parameters.AddWithValue("@montoMaximo", decimal.Parse(txtMontoMaximo.Text));
                    }

                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Configurar el GridView para mostrar los datos
                    ConfigurarGridViewBalanceFinanciero(dt);

                    // Calcular los indicadores para el reporte
                    CalcularIndicadoresBalanceFinanciero(dt);

                    // Actualizar la interfaz
                    string titulo = !string.IsNullOrEmpty(numeroPedido)
                        ? $"Balance Financiero - Pedido: {numeroPedido}"
                        : "Balance Financiero";

                    litTituloResultados.Text = titulo;
                    lblTotalRegistros.Text = $"Total registros: {dt.Rows.Count}";
                    gvReporte.DataSource = dt;
                    gvReporte.DataBind();
                }
            }
            catch (Exception ex)
            {
                // En caso de error, creamos una tabla vacía con las columnas esperadas
                DataTable dt = new DataTable();
                dt.Columns.Add("NroOrdenViaje", typeof(string));
                dt.Columns.Add("NumeroPedido", typeof(string));
                dt.Columns.Add("FechaTransaccion", typeof(DateTime));
                dt.Columns.Add("TipoTransaccion", typeof(string));
                dt.Columns.Add("Concepto", typeof(string));
                dt.Columns.Add("Conductor", typeof(string));
                dt.Columns.Add("Cliente", typeof(string));
                dt.Columns.Add("IngresoSoles", typeof(decimal));
                dt.Columns.Add("IngresoDolares", typeof(decimal));
                dt.Columns.Add("EgresoSoles", typeof(decimal));
                dt.Columns.Add("EgresoDolares", typeof(decimal));

                ConfigurarGridViewBalanceFinanciero(dt);
                CalcularIndicadoresBalanceFinanciero(dt);

                string titulo = !string.IsNullOrEmpty(numeroPedido)
                    ? $"Balance Financiero - Pedido: {numeroPedido}"
                    : "Balance Financiero";

                litTituloResultados.Text = titulo;
                lblTotalRegistros.Text = "Total registros: 0";
                gvReporte.DataSource = dt;
                gvReporte.DataBind();

                // Registrar el error y mostrar mensaje
                System.Diagnostics.Debug.WriteLine("Error al generar balance financiero: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "errorReporte",
                    "alert('Error al generar el balance financiero: " + ex.Message.Replace("'", "\\'") + "');", true);
            }
        }

        private void ConfigurarGridViewBalanceFinanciero(DataTable dt)
        {
            gvReporte.Columns.Clear();

            gvReporte.Columns.Add(new BoundField { DataField = "NroOrdenViaje", HeaderText = "Nº Orden", SortExpression = "NroOrdenViaje" });
            gvReporte.Columns.Add(new BoundField { DataField = "NumeroPedido", HeaderText = "Número de Pedido", SortExpression = "NumeroPedido" });
            gvReporte.Columns.Add(new BoundField { DataField = "FechaTransaccion", HeaderText = "Fecha", DataFormatString = "{0:dd/MM/yyyy}", SortExpression = "FechaTransaccion" });
            gvReporte.Columns.Add(new BoundField { DataField = "Concepto", HeaderText = "Concepto", SortExpression = "Concepto" });
            gvReporte.Columns.Add(new BoundField { DataField = "TipoTransaccion", HeaderText = "Tipo", SortExpression = "TipoTransaccion" });
            gvReporte.Columns.Add(new BoundField { DataField = "Conductor", HeaderText = "Conductor", SortExpression = "Conductor" });
            gvReporte.Columns.Add(new BoundField { DataField = "Cliente", HeaderText = "Cliente", SortExpression = "Cliente" });
            gvReporte.Columns.Add(new BoundField { DataField = "IngresoSoles", HeaderText = "Ingreso S/", DataFormatString = "{0:N2}", SortExpression = "IngresoSoles", ItemStyle = { HorizontalAlign = HorizontalAlign.Right } });
            gvReporte.Columns.Add(new BoundField { DataField = "IngresoDolares", HeaderText = "Ingreso $", DataFormatString = "{0:N2}", SortExpression = "IngresoDolares", ItemStyle = { HorizontalAlign = HorizontalAlign.Right } });
            gvReporte.Columns.Add(new BoundField { DataField = "EgresoSoles", HeaderText = "Egreso S/", DataFormatString = "{0:N2}", SortExpression = "EgresoSoles", ItemStyle = { HorizontalAlign = HorizontalAlign.Right } });
            gvReporte.Columns.Add(new BoundField { DataField = "EgresoDolares", HeaderText = "Egreso $", DataFormatString = "{0:N2}", SortExpression = "EgresoDolares", ItemStyle = { HorizontalAlign = HorizontalAlign.Right } });
        }

        private void CalcularIndicadoresBalanceFinanciero(DataTable dt)
        {
            decimal totalIngresosSoles = 0;
            decimal totalIngresosDolares = 0;
            decimal totalEgresosSoles = 0;
            decimal totalEgresosDolares = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row["IngresoSoles"] != DBNull.Value)
                    totalIngresosSoles += Convert.ToDecimal(row["IngresoSoles"]);

                if (row["IngresoDolares"] != DBNull.Value)
                    totalIngresosDolares += Convert.ToDecimal(row["IngresoDolares"]);

                if (row["EgresoSoles"] != DBNull.Value)
                    totalEgresosSoles += Convert.ToDecimal(row["EgresoSoles"]);

                if (row["EgresoDolares"] != DBNull.Value)
                    totalEgresosDolares += Convert.ToDecimal(row["EgresoDolares"]);
            }

            decimal balanceSoles = totalIngresosSoles - totalEgresosSoles;
            decimal balanceDolares = totalIngresosDolares - totalEgresosDolares;

            // Actualizar interfaz con los indicadores
            litTotalIngresos.Text = $"S/ {totalIngresosSoles:N2} | $ {totalIngresosDolares:N2}";
            litTotalEgresos.Text = $"S/ {totalEgresosSoles:N2} | $ {totalEgresosDolares:N2}";
            litBalance.Text = $"S/ {balanceSoles:N2} | $ {balanceDolares:N2}";
            litIndicadorAdicionalTitulo.Text = "Transacciones";
            litIndicadorAdicional.Text = $"{dt.Rows.Count}";
        }



        //REPORTE VIAJE POR CONDUCTOR
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
                (SELECT TOP 1 pd.nombre 
                 FROM GuiasTransportista gt
                 LEFT JOIN PlantaDescarga pd ON gt.plantaDescarga = pd.nombre OR TRY_CAST(gt.plantaDescarga AS INT) = pd.idPlanta
                 WHERE gt.numeroOrdenViaje = ov.numeroOrdenViaje AND pd.nombre IS NOT NULL) AS PlantaDescarga
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

        //PRODUCTOR POR CONDUCTOR - REPORTE POR CONDUCTOR

        private void GenerarReporteProductosConductor()
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
                p.nombre AS Producto,
                ISNULL(dov.cantidadBolsas, 0) AS CantidadBolsas,
                ISNULL(cp.pesoKg, 0) AS PesoKg,
                gt.numeroGuiaTransportista AS GuiaTransportista,
                gt.numeroGuiaCliente AS GuiaCliente,
                ov.fechaSalida AS FechaSalida,
                ov.fechaLlegada AS FechaLlegada,
                (SELECT TOP 1 pd.nombre 
                 FROM GuiasTransportista gt2
                 LEFT JOIN PlantaDescarga pd ON gt2.plantaDescarga = pd.nombre OR TRY_CAST(gt2.plantaDescarga AS INT) = pd.idPlanta
                 WHERE gt2.numeroOrdenViaje = ov.numeroOrdenViaje AND pd.nombre IS NOT NULL) AS PlantaDescarga,
                cl.nombre AS Cliente
            FROM OrdenViaje ov
            JOIN Conductor c ON ov.idConductor = c.idConductor
            LEFT JOIN Cliente cl ON ov.idCliente = cl.idCliente
            LEFT JOIN GuiasTransportista gt ON ov.numeroOrdenViaje = gt.numeroOrdenViaje
            LEFT JOIN DetalleOrdenViaje dov ON gt.idGuia = dov.idGuia
            LEFT JOIN Producto p ON 
                CASE 
                    WHEN dov.idProducto IS NOT NULL THEN dov.idProducto
                    ELSE ov.idProducto
                END = p.idProducto
            LEFT JOIN CPIC cpic ON ov.idCPIC = cpic.idCPIC
            LEFT JOIN CPIC_Productos cp ON cpic.idCPIC = cp.idCPIC AND p.idProducto = cp.idProducto
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

                    // Si hay filtro de producto específico
                    if (!string.IsNullOrEmpty(ddlProducto.SelectedValue))
                    {
                        query += " AND p.idProducto = @idProducto";
                    }

                    query += " ORDER BY ov.fechaSalida DESC, ov.numeroOrdenViaje";

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

                    if (!string.IsNullOrEmpty(ddlProducto.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@idProducto", ddlProducto.SelectedValue);
                    }

                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Mostrar
                    ConfigurarGridViewProductosConductor(dt);
                    CalcularIndicadoresProductosConductor(dt);
                    litTituloResultados.Text = "Reporte de Productos Transportados por Conductor";
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
                dt.Columns.Add("Producto", typeof(string));
                dt.Columns.Add("CantidadBolsas", typeof(int));
                dt.Columns.Add("PesoKg", typeof(decimal));
                dt.Columns.Add("GuiaTransportista", typeof(string));
                dt.Columns.Add("GuiaCliente", typeof(string));
                dt.Columns.Add("FechaSalida", typeof(DateTime));
                dt.Columns.Add("FechaLlegada", typeof(DateTime));
                dt.Columns.Add("PlantaDescarga", typeof(string));
                dt.Columns.Add("Cliente", typeof(string));

                ConfigurarGridViewProductosConductor(dt);
                CalcularIndicadoresProductosConductor(dt);
                litTituloResultados.Text = "Reporte de Productos Transportados por Conductor";
                lblTotalRegistros.Text = "Total registros: 0";
                gvReporte.DataSource = dt;
                gvReporte.DataBind();

                // Registrar el error
                System.Diagnostics.Debug.WriteLine("Error al generar reporte de productos transportados: " + ex.Message);
            }
        }

        private void ConfigurarGridViewProductosConductor(DataTable dt)
        {
            gvReporte.Columns.Clear();

            gvReporte.Columns.Add(new BoundField { DataField = "NroOrdenViaje", HeaderText = "Nº Orden", SortExpression = "NroOrdenViaje" });
            gvReporte.Columns.Add(new BoundField { DataField = "NombreConductor", HeaderText = "Conductor", SortExpression = "NombreConductor" });
            gvReporte.Columns.Add(new BoundField { DataField = "Producto", HeaderText = "Producto", SortExpression = "Producto" });
            gvReporte.Columns.Add(new BoundField { DataField = "CantidadBolsas", HeaderText = "Cant. Bolsas", SortExpression = "CantidadBolsas", DataFormatString = "{0:N0}", ItemStyle = { HorizontalAlign = HorizontalAlign.Right } });
            gvReporte.Columns.Add(new BoundField { DataField = "PesoKg", HeaderText = "Peso (Kg)", SortExpression = "PesoKg", DataFormatString = "{0:N2}", ItemStyle = { HorizontalAlign = HorizontalAlign.Right } });
            gvReporte.Columns.Add(new BoundField { DataField = "Cliente", HeaderText = "Cliente", SortExpression = "Cliente" });
            gvReporte.Columns.Add(new BoundField { DataField = "GuiaTransportista", HeaderText = "Guía Transportista", SortExpression = "GuiaTransportista" });
            gvReporte.Columns.Add(new BoundField { DataField = "GuiaCliente", HeaderText = "Guía Cliente", SortExpression = "GuiaCliente" });
            gvReporte.Columns.Add(new BoundField { DataField = "FechaSalida", HeaderText = "Fecha Salida", DataFormatString = "{0:dd/MM/yyyy}", SortExpression = "FechaSalida" });
            gvReporte.Columns.Add(new BoundField { DataField = "FechaLlegada", HeaderText = "Fecha Llegada", DataFormatString = "{0:dd/MM/yyyy}", SortExpression = "FechaLlegada" });
            gvReporte.Columns.Add(new BoundField { DataField = "PlantaDescarga", HeaderText = "Planta Descarga", SortExpression = "PlantaDescarga" });
        }

        private void CalcularIndicadoresProductosConductor(DataTable dt)
        {
            int totalViajes = dt.DefaultView.ToTable(true, "NroOrdenViaje").Rows.Count;
            int totalProductos = dt.DefaultView.ToTable(true, "Producto").Rows.Count;
            int totalBolsas = 0;
            decimal totalPesoKg = 0;

            // Obtener el producto más transportado
            Dictionary<string, int> cantidadPorProducto = new Dictionary<string, int>();
            Dictionary<string, decimal> pesoPorProducto = new Dictionary<string, decimal>();

            foreach (DataRow row in dt.Rows)
            {
                string producto = row["Producto"].ToString();
                if (string.IsNullOrEmpty(producto))
                    continue;

                int bolsas = 0;
                if (row["CantidadBolsas"] != DBNull.Value)
                {
                    bolsas = Convert.ToInt32(row["CantidadBolsas"]);
                    totalBolsas += bolsas;
                }

                decimal peso = 0;
                if (row["PesoKg"] != DBNull.Value)
                {
                    peso = Convert.ToDecimal(row["PesoKg"]);
                    totalPesoKg += peso;
                }

                // Actualizar diccionarios para el conteo de productos
                if (!cantidadPorProducto.ContainsKey(producto))
                {
                    cantidadPorProducto[producto] = 0;
                    pesoPorProducto[producto] = 0;
                }
                cantidadPorProducto[producto] += bolsas;
                pesoPorProducto[producto] += peso;
            }

            // Encontrar el producto más transportado (por cantidad de bolsas)
            string productoMasTransportado = "Ninguno";
            int maxBolsas = 0;

            foreach (var kvp in cantidadPorProducto)
            {
                if (kvp.Value > maxBolsas)
                {
                    maxBolsas = kvp.Value;
                    productoMasTransportado = kvp.Key;
                }
            }

            // Actualizar interfaz con los indicadores
            litTotalIngresos.Text = $"{totalProductos} productos";
            litTotalEgresos.Text = $"{totalViajes} viajes";
            litBalance.Text = $"{totalBolsas:N0} bolsas";
            litIndicadorAdicionalTitulo.Text = "Peso Total";
            litIndicadorAdicional.Text = $"{totalPesoKg:N2} Kg";
        }

        //INGRESOS Y GASTOS - REPORTE POR CONDUCTOR

        private void GenerarReporteFinancieroConductor()
        {
            DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);
            DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text);
            string idConductor = ddlConductor.SelectedValue;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Primero definimos la condición de filtro del conductor
                    string condicionConductor = "";
                    if (!string.IsNullOrEmpty(idConductor))
                    {
                        condicionConductor = " AND c.idConductor = @idConductor";
                    }
                    else if (!string.IsNullOrEmpty(txtDNIConductor.Text))
                    {
                        condicionConductor = " AND c.DNI LIKE @dni";
                    }
                    else if (!string.IsNullOrEmpty(txtNombreConductor.Text))
                    {
                        condicionConductor = " AND (c.nombre LIKE @nombreConductor OR c.apPaterno LIKE @nombreConductor OR c.apMaterno LIKE @nombreConductor)";
                    }

                    string query = @"
            -- Ingresos
            SELECT 
                ov.numeroOrdenViaje AS NroOrdenViaje,
                ov.fechaSalida AS FechaTransaccion,
                'Ingreso' AS TipoTransaccion,
                CASE 
                    WHEN i.despachoSoles > 0 OR i.despachoDolares > 0 THEN 'Despacho'
                    WHEN i.prestamoSoles > 0 OR i.prestamosDolares > 0 THEN 'Préstamo'
                    WHEN i.mensualidadSoles > 0 OR i.mensualidadDolares > 0 THEN 'Mensualidad'
                    WHEN i.otrosSoles > 0 OR i.otrosDolares > 0 THEN 'Otros'
                    ELSE 'No especificado'
                END AS Concepto,
                CONCAT(c.nombre, ' ', c.apPaterno, ' ', c.apMaterno) AS Conductor,
                cl.nombre AS Cliente,
                ISNULL(i.despachoSoles, 0) + ISNULL(i.prestamoSoles, 0) + 
                ISNULL(i.mensualidadSoles, 0) + ISNULL(i.otrosSoles, 0) AS MontoSoles,
                ISNULL(i.despachoDolares, 0) + ISNULL(i.prestamosDolares, 0) + 
                ISNULL(i.mensualidadDolares, 0) + ISNULL(i.otrosDolares, 0) AS MontoDolares,
                CASE
                    WHEN i.despachoSoles > 0 THEN i.descDespacho
                    WHEN i.prestamoSoles > 0 THEN i.descPrestamo
                    WHEN i.mensualidadSoles > 0 THEN i.descMensualidad
                    WHEN i.otrosSoles > 0 THEN i.descOtrosAutorizados
                    ELSE NULL
                END AS Observaciones
            FROM OrdenViaje ov
            JOIN Conductor c ON ov.idConductor = c.idConductor
            LEFT JOIN Ingresos i ON ov.numeroOrdenViaje = i.numeroOrdenViaje
            LEFT JOIN Cliente cl ON ov.idCliente = cl.idCliente
            WHERE (i.despachoSoles > 0 OR i.despachoDolares > 0 OR i.prestamoSoles > 0 OR 
                   i.prestamosDolares > 0 OR i.mensualidadSoles > 0 OR i.mensualidadDolares > 0 OR
                   i.otrosSoles > 0 OR i.otrosDolares > 0)
            AND ov.fechaSalida BETWEEN @fechaDesde AND @fechaHasta
            " + condicionConductor + @"
            
            UNION ALL
            
            -- Egresos
            SELECT 
                ov.numeroOrdenViaje AS NroOrdenViaje,
                ov.fechaSalida AS FechaTransaccion,
                'Egreso' AS TipoTransaccion,
                CASE 
                    WHEN e.peajesSoles > 0 OR e.peajesDolares > 0 THEN 'Peaje'
                    WHEN e.alimentacionSoles > 0 OR e.alimentacionDolares > 0 THEN 'Alimentación'
                    WHEN e.apoyoseguridadSoles > 0 OR e.apoyoseguridadDolares > 0 THEN 'Apoyo Seguridad'
                    WHEN e.reparacionesVariosSoles > 0 OR e.repacionesVariosDolares > 0 THEN 'Reparaciones'
                    WHEN e.movilidadSoles > 0 OR e.movilidadDolares > 0 THEN 'Movilidad'
                    WHEN e.hospedajeSoles > 0 OR e.hospedajeDolares > 0 THEN 'Hospedaje'
                    WHEN e.combustibleSoles > 0 OR e.combustibleDolares > 0 THEN 'Combustible'
                    WHEN e.encarpada_desencarpadaSoles > 0 OR e.encarpada_desencarpadaDolares > 0 THEN 'Encarpada/Desencarpada'
                    ELSE 'Otros gastos'
                END AS Concepto,
                CONCAT(c.nombre, ' ', c.apPaterno, ' ', c.apMaterno) AS Conductor,
                cl.nombre AS Cliente,
                ISNULL(e.peajesSoles, 0) + ISNULL(e.alimentacionSoles, 0) + 
                ISNULL(e.apoyoseguridadSoles, 0) + ISNULL(e.reparacionesVariosSoles, 0) + 
                ISNULL(e.movilidadSoles, 0) + ISNULL(e.hospedajeSoles, 0) + 
                ISNULL(e.combustibleSoles, 0) + ISNULL(e.encarpada_desencarpadaSoles, 0) AS MontoSoles,
                ISNULL(e.peajesDolares, 0) + ISNULL(e.alimentacionDolares, 0) + 
                ISNULL(e.apoyoseguridadDolares, 0) + ISNULL(e.repacionesVariosDolares, 0) + 
                ISNULL(e.movilidadDolares, 0) + ISNULL(e.hospedajeDolares, 0) + 
                ISNULL(e.combustibleDolares, 0) + ISNULL(e.encarpada_desencarpadaDolares, 0) AS MontoDolares,
                CASE
                    WHEN e.peajesSoles > 0 THEN e.descPeajes
                    WHEN e.alimentacionSoles > 0 THEN e.descAlimentacion
                    WHEN e.apoyoseguridadSoles > 0 THEN e.descApoyoSeguridad
                    WHEN e.reparacionesVariosSoles > 0 THEN e.descReparacionesVarios
                    WHEN e.movilidadSoles > 0 THEN e.descMovilidad
                    WHEN e.hospedajeSoles > 0 THEN e.descHospedaje
                    WHEN e.combustibleSoles > 0 THEN e.descCombustible
                    WHEN e.encarpada_desencarpadaSoles > 0 THEN e.descEncarpadaDesencarpada
                    ELSE NULL
                END AS Observaciones
            FROM OrdenViaje ov
            JOIN Conductor c ON ov.idConductor = c.idConductor
            LEFT JOIN Egresos e ON ov.numeroOrdenViaje = e.numeroOrdenViaje
            LEFT JOIN Cliente cl ON ov.idCliente = cl.idCliente
            WHERE (e.peajesSoles > 0 OR e.peajesDolares > 0 OR e.alimentacionSoles > 0 OR e.alimentacionDolares > 0 OR 
                   e.apoyoseguridadSoles > 0 OR e.apoyoseguridadDolares > 0 OR e.reparacionesVariosSoles > 0 OR 
                   e.repacionesVariosDolares > 0 OR e.movilidadSoles > 0 OR e.movilidadDolares > 0 OR 
                   e.hospedajeSoles > 0 OR e.hospedajeDolares > 0 OR e.combustibleSoles > 0 OR 
                   e.combustibleDolares > 0 OR e.encarpada_desencarpadaSoles > 0 OR e.encarpada_desencarpadaDolares > 0)
            AND ov.fechaSalida BETWEEN @fechaDesde AND @fechaHasta
            " + condicionConductor + @"
            
            UNION ALL
            
            -- Categorías Adicionales
            SELECT 
                ov.numeroOrdenViaje AS NroOrdenViaje,
                ov.fechaSalida AS FechaTransaccion,
                'Egreso' AS TipoTransaccion,
                ca.nombreCategoria AS Concepto,
                CONCAT(c.nombre, ' ', c.apPaterno, ' ', c.apMaterno) AS Conductor,
                cl.nombre AS Cliente,
                ISNULL(ca.soles, 0) AS MontoSoles,
                ISNULL(ca.dolares, 0) AS MontoDolares,
                ca.descripcion AS Observaciones
            FROM OrdenViaje ov
            JOIN Conductor c ON ov.idConductor = c.idConductor
            JOIN CategoriasAdicionales ca ON ov.numeroOrdenViaje = ca.numeroOrdenViaje
            LEFT JOIN Cliente cl ON ov.idCliente = cl.idCliente
            WHERE (ca.soles > 0 OR ca.dolares > 0)
            AND ov.fechaSalida BETWEEN @fechaDesde AND @fechaHasta
            " + condicionConductor;

                    // Aplicamos filtros adicionales después de la UNION
                    string filtrosAdicionales = "";

                    // Si hay filtro de tipo de transacción
                    if (!string.IsNullOrEmpty(ddlTipoTransaccion.SelectedValue))
                    {
                        filtrosAdicionales += " AND TipoTransaccion = @tipoTransaccion";
                    }

                    // Si hay filtro de monto
                    if (!string.IsNullOrEmpty(txtMontoMinimo.Text))
                    {
                        filtrosAdicionales += " AND ((MontoSoles >= @montoMinimo) OR (MontoDolares >= @montoMinimo))";
                    }

                    if (!string.IsNullOrEmpty(txtMontoMaximo.Text))
                    {
                        filtrosAdicionales += " AND ((MontoSoles <= @montoMaximo) OR (MontoDolares <= @montoMaximo))";
                    }

                    // Si hay filtros adicionales, envolvemos la consulta en un SELECT exterior
                    if (!string.IsNullOrEmpty(filtrosAdicionales))
                    {
                        query = "SELECT * FROM (" + query + ") AS ResultadosCombinados WHERE 1=1 " + filtrosAdicionales;
                    }

                    query += " ORDER BY FechaTransaccion DESC, TipoTransaccion, Concepto";

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

                    if (!string.IsNullOrEmpty(ddlTipoTransaccion.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@tipoTransaccion", ddlTipoTransaccion.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(txtMontoMinimo.Text))
                    {
                        cmd.Parameters.AddWithValue("@montoMinimo", decimal.Parse(txtMontoMinimo.Text));
                    }

                    if (!string.IsNullOrEmpty(txtMontoMaximo.Text))
                    {
                        cmd.Parameters.AddWithValue("@montoMaximo", decimal.Parse(txtMontoMaximo.Text));
                    }

                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Mostrar
                    ConfigurarGridViewFinancieroConductor(dt);
                    CalcularIndicadoresFinancieroConductor(dt);

                    // Actualizar título según el conductor seleccionado
                    string tituloReporte = "Reporte de Ingresos y Gastos por Conductor";
                    if (!string.IsNullOrEmpty(idConductor))
                    {
                        string nombreConductor = "";
                        if (dt.Rows.Count > 0 && dt.Rows[0]["Conductor"] != DBNull.Value)
                        {
                            nombreConductor = dt.Rows[0]["Conductor"].ToString();
                        }
                        else
                        {
                            foreach (ListItem item in ddlConductor.Items)
                            {
                                if (item.Value == idConductor)
                                {
                                    nombreConductor = item.Text;
                                    break;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(nombreConductor))
                        {
                            tituloReporte += ": " + nombreConductor;
                        }
                    }

                    litTituloResultados.Text = tituloReporte;
                    lblTotalRegistros.Text = $"Total registros: {dt.Rows.Count}";
                    gvReporte.DataSource = dt;
                    gvReporte.DataBind();
                }
            }
            catch (Exception ex)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("NroOrdenViaje", typeof(string));
                dt.Columns.Add("FechaTransaccion", typeof(DateTime));
                dt.Columns.Add("TipoTransaccion", typeof(string));
                dt.Columns.Add("Concepto", typeof(string));
                dt.Columns.Add("Conductor", typeof(string));
                dt.Columns.Add("Cliente", typeof(string));
                dt.Columns.Add("MontoSoles", typeof(decimal));
                dt.Columns.Add("MontoDolares", typeof(decimal));
                dt.Columns.Add("Observaciones", typeof(string));

                ConfigurarGridViewFinancieroConductor(dt);
                CalcularIndicadoresFinancieroConductor(dt);
                litTituloResultados.Text = "Reporte de Ingresos y Gastos por Conductor";
                lblTotalRegistros.Text = "Total registros: 0";
                gvReporte.DataSource = dt;
                gvReporte.DataBind();

                // Registrar el error
                System.Diagnostics.Debug.WriteLine("Error al generar reporte financiero por conductor: " + ex.Message);
            }
        }

        private void ConfigurarGridViewFinancieroConductor(DataTable dt)
        {
            gvReporte.Columns.Clear();

            gvReporte.Columns.Add(new BoundField { DataField = "NroOrdenViaje", HeaderText = "Nº Orden", SortExpression = "NroOrdenViaje" });
            gvReporte.Columns.Add(new BoundField { DataField = "FechaTransaccion", HeaderText = "Fecha", DataFormatString = "{0:dd/MM/yyyy}", SortExpression = "FechaTransaccion" });
            gvReporte.Columns.Add(new BoundField { DataField = "TipoTransaccion", HeaderText = "Tipo", SortExpression = "TipoTransaccion" });
            gvReporte.Columns.Add(new BoundField { DataField = "Concepto", HeaderText = "Concepto", SortExpression = "Concepto" });
            gvReporte.Columns.Add(new BoundField { DataField = "Conductor", HeaderText = "Conductor", SortExpression = "Conductor" });
            gvReporte.Columns.Add(new BoundField { DataField = "Cliente", HeaderText = "Cliente", SortExpression = "Cliente" });
            gvReporte.Columns.Add(new BoundField { DataField = "MontoSoles", HeaderText = "Monto S/", DataFormatString = "{0:N2}", ItemStyle = { HorizontalAlign = HorizontalAlign.Right }, SortExpression = "MontoSoles" });
            gvReporte.Columns.Add(new BoundField { DataField = "MontoDolares", HeaderText = "Monto $", DataFormatString = "{0:N2}", ItemStyle = { HorizontalAlign = HorizontalAlign.Right }, SortExpression = "MontoDolares" });
            gvReporte.Columns.Add(new BoundField { DataField = "Observaciones", HeaderText = "Observaciones", SortExpression = "Observaciones" });
        }

        private void CalcularIndicadoresFinancieroConductor(DataTable dt)
        {
            decimal totalIngresosSoles = 0;
            decimal totalIngresosDolares = 0;
            decimal totalEgresosSoles = 0;
            decimal totalEgresosDolares = 0;
            int contadorIngresos = 0;
            int contadorEgresos = 0;

            foreach (DataRow row in dt.Rows)
            {
                string tipoTransaccion = row["TipoTransaccion"].ToString();
                decimal montoSoles = Convert.ToDecimal(row["MontoSoles"]);
                decimal montoDolares = Convert.ToDecimal(row["MontoDolares"]);

                if (tipoTransaccion == "Ingreso")
                {
                    totalIngresosSoles += montoSoles;
                    totalIngresosDolares += montoDolares;
                    contadorIngresos++;
                }
                else if (tipoTransaccion == "Egreso")
                {
                    totalEgresosSoles += montoSoles;
                    totalEgresosDolares += montoDolares;
                    contadorEgresos++;
                }
            }

            decimal balanceSoles = totalIngresosSoles - totalEgresosSoles;
            decimal balanceDolares = totalIngresosDolares - totalEgresosDolares;

            // Actualizar interfaz con los indicadores
            litTotalIngresos.Text = $"S/ {totalIngresosSoles:N2} | $ {totalIngresosDolares:N2}";
            litTotalEgresos.Text = $"S/ {totalEgresosSoles:N2} | $ {totalEgresosDolares:N2}";
            litBalance.Text = $"S/ {balanceSoles:N2} | $ {balanceDolares:N2}";
            litIndicadorAdicionalTitulo.Text = "Transacciones";
            litIndicadorAdicional.Text = $"{contadorIngresos} ingresos, {contadorEgresos} egresos";
        }


        //rendimiento de combustible - reporte por conductor
        private void GenerarReporteCombustibleConductor()
        {
            DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);
            DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text);
            string idConductor = ddlConductor.SelectedValue;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Definimos la condición de filtro del conductor
                    string condicionConductor = "";
                    if (!string.IsNullOrEmpty(idConductor) && idConductor != "0")
                    {
                        condicionConductor = " AND c.idConductor = @idConductor";
                    }

                    string query = @"
            SELECT 
                ov.numeroOrdenViaje AS NroOrdenViaje,
                ov.fechaSalida AS FechaSalida,
                ov.fechaLlegada AS FechaLlegada,
                CONCAT(c.nombre, ' ', c.apPaterno, ' ', c.apMaterno) AS Conductor,
                t.placaTracto AS Placa,
                t.marca AS Marca,
                t.modelo AS Modelo,
                la.nombreAbastecimiento AS EstacionServicio,
                ac.fechaHora AS FechaAbastecimiento,
                ac.distanciaRutaKM AS DistanciaKm,
                ac.galonesTotalAbastecidos AS Galones,
                ac.precioDolar AS PrecioUnitario,
                (ac.galonesTotalAbastecidos * ac.precioDolar) AS Total,
                ac.rendimientoPromedio AS RendimientoKmGalon,
                ac.observaciones AS Observaciones,
                cl.nombre AS Cliente
            FROM OrdenViaje ov
            JOIN Conductor c ON ov.idConductor = c.idConductor
            JOIN Tracto t ON ov.idTracto = t.idTracto
            LEFT JOIN AbastecimientoCombustible ac ON ac.idOrdenViaje = ov.idOrdenViaje
            LEFT JOIN LugarAbastecimiento la ON ac.idLugarAbastecimiento = la.idLugarAbastecimiento
            LEFT JOIN Cliente cl ON ov.idCliente = cl.idCliente
            WHERE ac.idAbastecimientoCombustible IS NOT NULL
            AND ov.fechaSalida BETWEEN @fechaDesde AND @fechaHasta
            " + condicionConductor;

                    query += " ORDER BY ov.fechaSalida DESC, ov.numeroOrdenViaje";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta);

                    if (!string.IsNullOrEmpty(idConductor) && idConductor != "0")
                    {
                        cmd.Parameters.AddWithValue("@idConductor", idConductor);
                    }

                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Mostrar
                    ConfigurarGridViewCombustibleConductor(dt);
                    CalcularIndicadoresCombustibleConductor(dt);

                    // Actualizar título según el conductor seleccionado
                    string tituloReporte = "Reporte de Rendimiento de Combustible";
                    if (!string.IsNullOrEmpty(idConductor) && idConductor != "0")
                    {
                        string nombreConductor = "";
                        if (dt.Rows.Count > 0)
                        {
                            nombreConductor = dt.Rows[0]["Conductor"].ToString();
                        }
                        else
                        {
                            foreach (ListItem item in ddlConductor.Items)
                            {
                                if (item.Value == idConductor)
                                {
                                    nombreConductor = item.Text;
                                    break;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(nombreConductor))
                        {
                            tituloReporte += " - Conductor: " + nombreConductor;
                        }
                    }

                    litTituloResultados.Text = tituloReporte;
                    lblTotalRegistros.Text = $"Total registros: {dt.Rows.Count}";
                    gvReporte.DataSource = dt;
                    gvReporte.DataBind();
                }
            }
            catch (Exception ex)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("NroOrdenViaje", typeof(string));
                dt.Columns.Add("FechaSalida", typeof(DateTime));
                dt.Columns.Add("FechaLlegada", typeof(DateTime));
                dt.Columns.Add("Conductor", typeof(string));
                dt.Columns.Add("Placa", typeof(string));
                dt.Columns.Add("Marca", typeof(string));
                dt.Columns.Add("Modelo", typeof(string));
                dt.Columns.Add("EstacionServicio", typeof(string));
                dt.Columns.Add("FechaAbastecimiento", typeof(DateTime));
                dt.Columns.Add("DistanciaKm", typeof(decimal));
                dt.Columns.Add("Galones", typeof(decimal));
                dt.Columns.Add("PrecioUnitario", typeof(decimal));
                dt.Columns.Add("Total", typeof(decimal));
                dt.Columns.Add("RendimientoKmGalon", typeof(decimal));
                dt.Columns.Add("Observaciones", typeof(string));
                dt.Columns.Add("Cliente", typeof(string));

                ConfigurarGridViewCombustibleConductor(dt);
                CalcularIndicadoresCombustibleConductor(dt);
                litTituloResultados.Text = "Reporte de Rendimiento de Combustible";
                lblTotalRegistros.Text = "Total registros: 0";
                gvReporte.DataSource = dt;
                gvReporte.DataBind();

                // Registrar el error
                System.Diagnostics.Debug.WriteLine("Error al generar reporte de combustible: " + ex.Message);
            }
        }

        private void ConfigurarGridViewCombustibleConductor(DataTable dt)
        {
            gvReporte.Columns.Clear();

            gvReporte.Columns.Add(new BoundField { DataField = "NroOrdenViaje", HeaderText = "Nº Orden", SortExpression = "NroOrdenViaje" });
            gvReporte.Columns.Add(new BoundField { DataField = "FechaSalida", HeaderText = "Fecha Salida", DataFormatString = "{0:dd/MM/yyyy}", SortExpression = "FechaSalida" });
            gvReporte.Columns.Add(new BoundField { DataField = "Conductor", HeaderText = "Conductor", SortExpression = "Conductor" });
            gvReporte.Columns.Add(new BoundField { DataField = "Placa", HeaderText = "Placa", SortExpression = "Placa" });
            gvReporte.Columns.Add(new BoundField { DataField = "EstacionServicio", HeaderText = "Est. Servicio", SortExpression = "EstacionServicio" });
            gvReporte.Columns.Add(new BoundField { DataField = "DistanciaKm", HeaderText = "Dist. (Km)", DataFormatString = "{0:N0}", ItemStyle = { HorizontalAlign = HorizontalAlign.Right }, SortExpression = "DistanciaKm" });
            gvReporte.Columns.Add(new BoundField { DataField = "Galones", HeaderText = "Galones", DataFormatString = "{0:N2}", ItemStyle = { HorizontalAlign = HorizontalAlign.Right }, SortExpression = "Galones" });
            gvReporte.Columns.Add(new BoundField { DataField = "PrecioUnitario", HeaderText = "Precio Unit.", DataFormatString = "{0:N2}", ItemStyle = { HorizontalAlign = HorizontalAlign.Right }, SortExpression = "PrecioUnitario" });
            gvReporte.Columns.Add(new BoundField { DataField = "Total", HeaderText = "Total $", DataFormatString = "{0:N2}", ItemStyle = { HorizontalAlign = HorizontalAlign.Right }, SortExpression = "Total" });
            gvReporte.Columns.Add(new BoundField { DataField = "RendimientoKmGalon", HeaderText = "Km/Galón", DataFormatString = "{0:N2}", ItemStyle = { HorizontalAlign = HorizontalAlign.Right }, SortExpression = "RendimientoKmGalon" });
            gvReporte.Columns.Add(new BoundField { DataField = "Cliente", HeaderText = "Cliente", SortExpression = "Cliente" });
            gvReporte.Columns.Add(new BoundField { DataField = "Observaciones", HeaderText = "Observaciones", SortExpression = "Observaciones" });
        }

        private void CalcularIndicadoresCombustibleConductor(DataTable dt)
        {
            if (dt.Rows.Count == 0)
            {
                litTotalIngresos.Text = "0";
                litTotalEgresos.Text = "0";
                litBalance.Text = "0";
                litIndicadorAdicionalTitulo.Text = "Rendimiento Promedio";
                litIndicadorAdicional.Text = "0 Km/Galón";
                return;
            }

            decimal totalGalones = 0;
            decimal totalDolares = 0;
            decimal totalKilometros = 0;
            decimal totalRendimiento = 0;
            int contadorAbastecimientos = 0;
            int contadorVehiculos = dt.DefaultView.ToTable(true, "Placa").Rows.Count;

            foreach (DataRow row in dt.Rows)
            {
                decimal galones = 0;
                decimal total = 0;
                decimal distancia = 0;
                decimal rendimiento = 0;

                if (row["Galones"] != DBNull.Value)
                {
                    galones = Convert.ToDecimal(row["Galones"]);
                    totalGalones += galones;
                }

                if (row["Total"] != DBNull.Value)
                {
                    total = Convert.ToDecimal(row["Total"]);
                    totalDolares += total;
                }

                if (row["DistanciaKm"] != DBNull.Value)
                {
                    distancia = Convert.ToDecimal(row["DistanciaKm"]);
                    totalKilometros += distancia;
                }

                if (row["RendimientoKmGalon"] != DBNull.Value)
                {
                    rendimiento = Convert.ToDecimal(row["RendimientoKmGalon"]);
                    if (rendimiento > 0)  // Solo contar rendimientos válidos
                    {
                        totalRendimiento += rendimiento;
                        contadorAbastecimientos++;
                    }
                }
            }

            // Calcular rendimiento general (total km / total galones)
            decimal rendimientoGeneral = (totalGalones > 0) ?
                (totalKilometros / totalGalones) : 0;

            // Actualizar interfaz con los indicadores
            litTotalIngresos.Text = $"{totalKilometros:N0} Km";
            litTotalEgresos.Text = $"{totalGalones:N2} Gal";
            litBalance.Text = $"$ {totalDolares:N2}";
            litIndicadorAdicionalTitulo.Text = "Rendimiento";
            litIndicadorAdicional.Text = $"{rendimientoGeneral:N2} Km/Gal";
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