using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebSGV.Views
{
    public partial class AgregarOrdenViaje : System.Web.UI.Page
    {
        public class GastoAdicional
        {
            public string nombreCategoria { get; set; }
            public decimal soles { get; set; }
            public decimal dolares { get; set; }
            public string descripcion { get; set; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarClientes();
                CargarPlacasTracto();
                CargarPlacasCarreta();
                CargarConductores();
                CargarRutas();
                CargarPlantasDescarga();
            }
        }

        private void CargarClientes()
        {
            string query = "SELECT idCliente, nombre FROM Cliente";
            DataTable dt = ObtenerDatosDeBD(query);

            if (dt.Rows.Count > 0)
            {
                ddlCliente.DataSource = dt;
                ddlCliente.DataTextField = "nombre";
                ddlCliente.DataValueField = "idCliente";
                ddlCliente.DataBind();
            }

            ddlCliente.Items.Insert(0, new ListItem("Seleccione un cliente", ""));
        }

        private void CargarPlacasTracto()
        {
            string query = "SELECT idTracto, placaTracto FROM Tracto";
            DataTable dt = ObtenerDatosDeBD(query);

            if (dt.Rows.Count > 0)
            {
                ddlPlacaTracto.DataSource = dt;
                ddlPlacaTracto.DataTextField = "placaTracto";
                ddlPlacaTracto.DataValueField = "idTracto";
                ddlPlacaTracto.DataBind();
            }

            ddlPlacaTracto.Items.Insert(0, new ListItem("Seleccione una placa", ""));
        }

        private void CargarPlacasCarreta()
        {
            string query = "SELECT idCarreta, placaCarreta FROM Carreta";
            DataTable dt = ObtenerDatosDeBD(query);

            if (dt.Rows.Count > 0)
            {
                ddlPlacaCarreta.DataSource = dt;
                ddlPlacaCarreta.DataTextField = "placaCarreta";
                ddlPlacaCarreta.DataValueField = "idCarreta";
                ddlPlacaCarreta.DataBind();
            }

            ddlPlacaCarreta.Items.Insert(0, new ListItem("Seleccione una placa", ""));
        }

        private void CargarConductores()
        {
            string query = "SELECT idConductor, CONCAT(nombre, ' ', apPaterno, ' ', apMaterno) AS nombreCompleto FROM Conductor";
            DataTable dt = ObtenerDatosDeBD(query);

            if (dt.Rows.Count > 0)
            {
                ddlConductor.DataSource = dt;
                ddlConductor.DataTextField = "nombreCompleto";
                ddlConductor.DataValueField = "idConductor";
                ddlConductor.DataBind();
            }

            ddlConductor.Items.Insert(0, new ListItem("Seleccione un conductor", ""));
        }

        private void CargarRutas()
        {
            string query = "SELECT idRuta, nombre FROM Ruta";
            DataTable dt = ObtenerDatosDeBD(query);

            if (dt.Rows.Count > 0)
            {
                ddlRuta.DataSource = dt;
                ddlRuta.DataTextField = "nombre";
                ddlRuta.DataValueField = "idRuta";
                ddlRuta.DataBind();
            }

            ddlRuta.Items.Insert(0, new ListItem("Seleccione una ruta", ""));
        }

        private void CargarPlantasDescarga(int? idCliente = null)
        {
            string query = "SELECT idPlanta, nombre FROM PlantaDescarga";
            if (idCliente.HasValue)
            {
                query += " WHERE idCliente = @idCliente";
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (idCliente.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@idCliente", idCliente.Value);
                    }

                    try
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ddlPlantaDescarga.Items.Clear(); // Limpiar el dropdown antes de cargar nuevos datos
                        if (dt.Rows.Count > 0)
                        {
                            ddlPlantaDescarga.DataSource = dt;
                            ddlPlantaDescarga.DataTextField = "nombre";
                            ddlPlantaDescarga.DataValueField = "idPlanta";
                            ddlPlantaDescarga.DataBind();
                        }

                        ddlPlantaDescarga.Items.Insert(0, new ListItem("Seleccione una planta", ""));
                    }
                    catch (Exception ex)
                    {
                        lblErrores.Text = "Error al cargar las plantas de descarga: " + ex.Message;
                    }
                }
            }
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlCliente.SelectedValue))
            {
                int idCliente = Convert.ToInt32(ddlCliente.SelectedValue);
                CargarPlantasDescarga(idCliente);
            }
            else
            {
                CargarPlantasDescarga(); // Si no se selecciona un cliente, cargar todas las plantas
            }
        }

        protected string ObtenerProductosJSON()
        {
            string query = "SELECT idProducto, nombre FROM Producto";
            DataTable dt = ObtenerDatosDeBD(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }

        private DataTable ObtenerDatosDeBD(string query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        lblErrores.Text = "Error al obtener datos de la base de datos: " + ex.Message;
                        return new DataTable();
                    }
                }
            }
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            string cpic = txtCPI.Value.Trim();
            string ordenViaje = txtOrdenViaje.Value.Trim();
            DateTime fechaSalida = DateTime.MinValue;
            DateTime fechaLlegada = DateTime.MinValue;
            string horaSalida = txtHoraSalida.Value;
            string horaLlegada = txtHoraLlegada.Value;

            // Intentar parsear las fechas
            try
            {
                if (!string.IsNullOrEmpty(txtFechaSalida.Value))
                {
                    fechaSalida = DateTime.Parse(txtFechaSalida.Value);
                }
                if (!string.IsNullOrEmpty(txtFechaLlegada.Value))
                {
                    fechaLlegada = DateTime.Parse(txtFechaLlegada.Value);
                }
            }
            catch (FormatException ex)
            {
                lblErrores.Text = "Formato de fecha inválido: " + ex.Message;
                return;
            }

            // Validar dropdowns
            string cliente = ddlCliente.SelectedValue;
            string placaTracto = ddlPlacaTracto.SelectedValue;
            string placaCarreta = ddlPlacaCarreta.SelectedValue;
            string conductor = ddlConductor.SelectedValue;

            // Validaciones
            string errores = ValidarDatosViaje(cpic, ordenViaje, fechaSalida, fechaLlegada, horaSalida, horaLlegada, cliente, placaTracto, placaCarreta, conductor);

            if (!string.IsNullOrEmpty(errores))
            {
                lblErrores.Text = errores.Replace("\n", "<br/>");
                hfValidationError.Value = "false";
                return;
            }

            lblErrores.Text = "";
            hfValidationError.Value = "true"; // Indicar que las validaciones pasaron
        }

        private string ValidarDatosViaje(string cpic, string ordenViaje, DateTime fechaSalida, DateTime fechaLlegada, string horaSalida, string horaLlegada, string cliente, string placaTracto, string placaCarreta, string conductor)
        {
            string mensajeError = "";

            // Validar campos de texto
            if (string.IsNullOrEmpty(cpic))
            {
                mensajeError += "Por favor, ingrese el 'N° CPI'.\n";
            }
            else if (!EsCPICValido(cpic))
            {
                mensajeError += "El 'N° CPI' ingresado no está registrado en el sistema. Verifique e intente nuevamente.\n";
            }

            if (string.IsNullOrEmpty(ordenViaje))
            {
                mensajeError += "Por favor, ingrese el 'N° Orden Viaje'.\n";
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(ordenViaje, @"^\d{6}$"))
            {
                mensajeError += "El 'N° Orden Viaje' debe contener exactamente 6 dígitos.\n";
            }
            else if (!EsOrdenDeViajeUnica(ordenViaje))
            {
                mensajeError += "El 'N° Orden Viaje' ya está registrado en el sistema. Por favor, use un número diferente.\n";
            }

            // Validar fechas y horas
            DateTime fechaActual = DateTime.Now;
            if (fechaSalida == DateTime.MinValue)
            {
                mensajeError += "Por favor, seleccione una 'Fecha de Salida'.\n";
            }
            if (string.IsNullOrEmpty(horaSalida))
            {
                mensajeError += "Por favor, seleccione una 'Hora de Salida'.\n";
            }
            if (fechaLlegada == DateTime.MinValue)
            {
                mensajeError += "Por favor, seleccione una 'Fecha de Llegada'.\n";
            }
            if (string.IsNullOrEmpty(horaLlegada))
            {
                mensajeError += "Por favor, seleccione una 'Hora de Llegada'.\n";
            }

            if (fechaSalida != DateTime.MinValue && fechaLlegada != DateTime.MinValue)
            {
                if (fechaSalida > fechaLlegada)
                {
                    mensajeError += "La 'Fecha de Salida' no puede ser mayor a la 'Fecha de Llegada'. Por ejemplo, el vehículo no puede salir el 27 de marzo y llegar el 10 de marzo.\n";
                }
                if (fechaLlegada > fechaActual)
                {
                    mensajeError += "La 'Fecha de Llegada' no puede ser mayor a la fecha actual (" + fechaActual.ToString("dd/MM/yyyy") + "). El vehículo no puede llegar en una fecha futura.\n";
                }
            }

            // Validar dropdowns
            if (string.IsNullOrEmpty(cliente))
            {
                mensajeError += "Por favor, seleccione un 'Cliente'.\n";
            }
            if (string.IsNullOrEmpty(placaTracto))
            {
                mensajeError += "Por favor, seleccione una 'Placa Tracto'.\n";
            }
            if (string.IsNullOrEmpty(placaCarreta))
            {
                mensajeError += "Por favor, seleccione una 'Placa Carreta'.\n";
            }
            if (string.IsNullOrEmpty(conductor))
            {
                mensajeError += "Por favor, seleccione un 'Conductor'.\n";
            }

            return mensajeError;
        }

        private bool EsCPICValido(string cpic)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            string query = "SELECT COUNT(*) FROM CPIC WHERE numeroCPIC = @cpic";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@cpic", cpic);
                        conn.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                    catch (Exception ex)
                    {
                        lblErrores.Text = "Error al validar el 'N° CPI': " + ex.Message;
                        return false;
                    }
                }
            }
        }

        private bool EsOrdenDeViajeUnica(string ordenViaje)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            string query = "SELECT COUNT(*) FROM OrdenViaje WHERE numeroOrdenViaje = @ordenViaje";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@ordenViaje", ordenViaje);
                        conn.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count == 0;
                    }
                    catch (Exception ex)
                    {
                        lblErrores.Text = "Error al validar el 'N° Orden Viaje': " + ex.Message;
                        return false;
                    }
                }
            }
        }

        private bool GuiaTransportistaExiste(string guiaTransportista)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            string query = "SELECT COUNT(*) FROM GuiasTransportista WHERE numeroGuiaTransportista = @guiaTransportista";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@guiaTransportista", guiaTransportista);
                        conn.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                    catch (Exception ex)
                    {
                        lblErrores.Text = "Error al validar el 'N° Guía Transportista': " + ex.Message;
                        return false;
                    }
                }
            }
        }

        private bool GuiaClienteExiste(string guiaCliente)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            string query = "SELECT COUNT(*) FROM GuiasTransportista WHERE numeroGuiaCliente = @guiaCliente";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@guiaCliente", guiaCliente);
                        conn.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                    catch (Exception ex)
                    {
                        lblErrores.Text = "Error al validar el 'N° Guía Cliente': " + ex.Message;
                        return false;
                    }
                }
            }
        }

        private bool ManifiestoExiste(string numeroManifiesto)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            string query = "SELECT COUNT(*) FROM GuiasTransportista WHERE numeroManifiesto = @numeroManifiesto";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@numeroManifiesto", numeroManifiesto);
                        conn.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                    catch (Exception ex)
                    {
                        lblErrores.Text = "Error al validar el 'N° Manifiesto': " + ex.Message;
                        return false;
                    }
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener los datos de la pestaña "Datos del Viaje"
                string cpic = txtCPI.Value.Trim();
                string ordenViaje = txtOrdenViaje.Value.Trim();
                DateTime fechaSalida = string.IsNullOrEmpty(txtFechaSalida.Value) ? DateTime.MinValue : DateTime.Parse(txtFechaSalida.Value);
                DateTime fechaLlegada = string.IsNullOrEmpty(txtFechaLlegada.Value) ? DateTime.MinValue : DateTime.Parse(txtFechaLlegada.Value);
                string horaSalida = txtHoraSalida.Value;
                string horaLlegada = txtHoraLlegada.Value;
                string cliente = ddlCliente.SelectedValue;
                string placaTracto = ddlPlacaTracto.SelectedValue;
                string placaCarreta = ddlPlacaCarreta.SelectedValue;
                string conductor = ddlConductor.SelectedValue;
                string observaciones = txtObservaciones.Value;

                // Obtener los datos de la pestaña "Guías"
                string guiaTransportista = txtGuiaTransportista.Text.Trim();
                string guiaCliente = txtGuiaCliente.Text.Trim();
                string ruta = ddlRuta.SelectedValue;
                string plantaDescarga = ddlPlantaDescarga.SelectedValue;
                string numManifiesto = txtManifiesto.Text.Trim();

                // Obtener los datos de la pestaña "Liquidación"
                // Nota: Ajusta los nombres de los campos según tu HTML

                string descDespacho = Request.Form["txtDescDespacho"] ?? "";
                decimal despachoSoles = decimal.TryParse(Request.Form["txtDespachoSoles"], out var ds) ? ds : 0;
                decimal despachoDolares = decimal.TryParse(Request.Form["txtDespachoDolares"], out var dd) ? dd : 0;

                string descMensualidad = Request.Form["txtDescMensualidad"] ?? "";
                decimal mensualidadSoles = decimal.TryParse(Request.Form["txtMensualidadSoles"], out var ms) ? ms : 0;
                decimal mensualidadDolares = decimal.TryParse(Request.Form["txtMensualidadDolares"], out var md) ? md : 0;

                string descOtros = Request.Form["txtDescOtros"] ?? "";
                decimal otrosSoles = decimal.TryParse(Request.Form["txtOtrosSoles"], out var os) ? os : 0;
                decimal otrosDolares = decimal.TryParse(Request.Form["txtOtrosDolares"], out var od) ? od : 0;

                string descPrestamo = Request.Form["txtDescPrestamo"] ?? "";
                decimal prestamoSoles = decimal.TryParse(Request.Form["txtPrestamoSoles"], out var ps) ? ps : 0;
                decimal prestamoDolares = decimal.TryParse(Request.Form["txtPrestamoDolares"], out var pd) ? pd : 0;



                // Variables temporales únicas
                decimal temp;

                // Peajes
                string descPeajes = Request.Form["txtDescPeajes"] ?? "";
                decimal peajesSoles = decimal.TryParse(Request.Form["txtPeajesSoles"], out temp) ? temp : 0;
                decimal peajesDolares = decimal.TryParse(Request.Form["txtPeajesDolares"], out temp) ? temp : 0;

                // Alimentación
                string descAlimentacion = Request.Form["txtDescAlimentacion"] ?? "";
                decimal alimentacionSoles = decimal.TryParse(Request.Form["txtAlimentacionSoles"], out temp) ? temp : 0;
                decimal alimentacionDolares = decimal.TryParse(Request.Form["txtAlimentacionDolares"], out temp) ? temp : 0;

                // Apoyo-Seguridad
                string descApoyoSeguridad = Request.Form["txtDescApoyoSeguridad"] ?? "";
                decimal apoyoSeguridadSoles = decimal.TryParse(Request.Form["txtApoyoSeguridadSoles"], out temp) ? temp : 0;
                decimal apoyoSeguridadDolares = decimal.TryParse(Request.Form["txtApoyoSeguridadDolares"], out temp) ? temp : 0;

                // Reparaciones
                string descReparaciones = Request.Form["txtDescReparaciones"] ?? "";
                decimal reparacionesSoles = decimal.TryParse(Request.Form["txtReparacionesSoles"], out temp) ? temp : 0;
                decimal reparacionesDolares = decimal.TryParse(Request.Form["txtReparacionesDolares"], out temp) ? temp : 0;

                // Movilidad
                string descMovilidad = Request.Form["txtDescMovilidad"] ?? "";
                decimal movilidadSoles = decimal.TryParse(Request.Form["txtMovilidadSoles"], out temp) ? temp : 0;
                decimal movilidadDolares = decimal.TryParse(Request.Form["txtMovilidadDolares"], out temp) ? temp : 0;

                // Encapada
                string descEncapada = Request.Form["txtDescEncapada"] ?? "";
                decimal encapadaSoles = decimal.TryParse(Request.Form["txtEncapadaSoles"], out temp) ? temp : 0;
                decimal encapadaDolares = decimal.TryParse(Request.Form["txtEncapadaDolares"], out temp) ? temp : 0;

                // Hospedaje
                string descHospedaje = Request.Form["txtDescHospedaje"] ?? "";
                decimal hospedajeSoles = decimal.TryParse(Request.Form["txtHospedajeSoles"], out temp) ? temp : 0;
                decimal hospedajeDolares = decimal.TryParse(Request.Form["txtHospedajeDolares"], out temp) ? temp : 0;

                // Combustible
                string descCombustible = Request.Form["txtDescCombustible"] ?? "";
                decimal combustibleSoles = decimal.TryParse(Request.Form["txtCombustibleSoles"], out temp) ? temp : 0;
                decimal combustibleDolares = decimal.TryParse(Request.Form["txtCombustibleDolares"], out temp) ? temp : 0;




                Response.Write($"CPIC: {cpic}, OrdenViaje: {ordenViaje}, GuiaTransportista: {Request.Form["txtGuiaTransportista"]}<br/>");


                // Validar datos de la pestaña "Datos del Viaje"
                string errores = ValidarDatosViaje(cpic, ordenViaje, fechaSalida, fechaLlegada, horaSalida, horaLlegada, cliente, placaTracto, placaCarreta, conductor);

                // Validar datos de la pestaña "Guías"
                if (string.IsNullOrEmpty(guiaTransportista))
                {
                    errores += "El campo 'N° Guía Transportista' es obligatorio.\n";
                }
                else if (GuiaTransportistaExiste(guiaTransportista))
                {
                    errores += "El 'N° Guía Transportista' ya está registrado.\n";
                }

                if (string.IsNullOrEmpty(guiaCliente))
                {
                    errores += "El campo 'N° Guía Cliente' es obligatorio.\n";
                }
                else if (GuiaClienteExiste(guiaCliente))
                {
                    errores += "El 'N° Guía Cliente' ya está registrado.\n";
                }

                if (string.IsNullOrEmpty(ruta))
                {
                    errores += "Debe seleccionar una 'Ruta'.\n";
                }

                if (ruta == "2") // Sullana-Guayaquil-Sullana
                {
                    if (string.IsNullOrEmpty(plantaDescarga))
                    {
                        errores += "Debe seleccionar una 'Planta de Descarga' para la ruta Sullana-Guayaquil-Sullana.\n";
                    }

                    if (string.IsNullOrEmpty(numManifiesto))
                    {
                        errores += "El campo 'N° Manifiesto' es obligatorio para la ruta Sullana-Guayaquil-Sullana.\n";
                    }
                    else if (ManifiestoExiste(numManifiesto))
                    {
                        errores += "El 'N° Manifiesto' ya está registrado.\n";
                    }
                }

                // Validar datos de la pestaña "Liquidación"
                // Aquí puedes agregar validaciones adicionales si es necesario
                if (Convert.ToDecimal(peajesSoles) < 0 || Convert.ToDecimal(peajesDolares) < 0)
                {
                    errores += "Los valores de 'Peajes' no pueden ser negativos.\n";
                }
                if (Convert.ToDecimal(alimentacionSoles) < 0 || Convert.ToDecimal(alimentacionDolares) < 0)
                {
                    errores += "Los valores de 'Alimentación' no pueden ser negativos.\n";
                }
                if (Convert.ToDecimal(combustibleSoles) < 0 || Convert.ToDecimal(combustibleDolares) < 0)
                {
                    errores += "Los valores de 'Combustible' no pueden ser negativos.\n";
                }
                if (Convert.ToDecimal(despachoSoles) < 0 || Convert.ToDecimal(despachoDolares) < 0)
                {
                    errores += "Los valores de 'Despacho' no pueden ser negativos.\n";
                }

                // Obtener los datos de los productos
                string productosJson = Request.Form["productosData"];
                List<ProductoOrdenViaje> productos = string.IsNullOrEmpty(productosJson)
                    ? new List<ProductoOrdenViaje>()
                    : JsonConvert.DeserializeObject<List<ProductoOrdenViaje>>(productosJson);

                // Obtener gastos adicionales (categorías dinámicas)
                string gastosAdicionalesJson = Request.Form["gastosAdicionales"]; // Este campo lo debes llenar desde JS
                List<GastoAdicional> gastosAdicionales = string.IsNullOrEmpty(gastosAdicionalesJson)
                    ? new List<GastoAdicional>()
                    : JsonConvert.DeserializeObject<List<GastoAdicional>>(gastosAdicionalesJson);


                if (productos.Count == 0)
                {
                    errores += "Debe agregar al menos un producto en la pestaña 'Guías'.\n";
                }

                if (!string.IsNullOrEmpty(errores))
                {
                    Response.Write("Errores de validación: " + errores.Replace("\n", "<br/>") + "<br/>");
                    lblErrores.Text = errores.Replace("\n", "<br/>");
                    ClientScript.RegisterStartupScript(this.GetType(), "cambiarTab", "$('#guias-tab').click();", true);
                    return;
                }

                // Guardar en la base de datos
                string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Insertar en la tabla OrdenViaje
                            string queryOrdenViaje = "INSERT INTO OrdenViaje (numeroOrdenViaje, fechaSalida, horaSalida, fechaLlegada, horaLlegada, idCliente, idTracto, idCarreta, idConductor, observaciones, idCPIC) " +
                                                     "OUTPUT INSERTED.idOrdenViaje " +
                                                     "VALUES (@numeroOrdenViaje, @fechaSalida, @horaSalida, @fechaLlegada, @horaLlegada, @idCliente, @idTracto, @idCarreta, @idConductor, @observaciones, @idCPIC)";
                            int idOrdenViaje;
                            using (SqlCommand cmd = new SqlCommand(queryOrdenViaje, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@numeroOrdenViaje", ordenViaje);
                                cmd.Parameters.AddWithValue("@fechaSalida", fechaSalida == DateTime.MinValue ? (object)DBNull.Value : fechaSalida);
                                cmd.Parameters.AddWithValue("@horaSalida", string.IsNullOrEmpty(horaSalida) ? (object)DBNull.Value : horaSalida);
                                cmd.Parameters.AddWithValue("@fechaLlegada", fechaLlegada == DateTime.MinValue ? (object)DBNull.Value : fechaLlegada);
                                cmd.Parameters.AddWithValue("@horaLlegada", string.IsNullOrEmpty(horaLlegada) ? (object)DBNull.Value : horaLlegada);
                                cmd.Parameters.AddWithValue("@idCliente", cliente);
                                cmd.Parameters.AddWithValue("@idTracto", placaTracto);
                                cmd.Parameters.AddWithValue("@idCarreta", placaCarreta);
                                cmd.Parameters.AddWithValue("@idConductor", conductor);
                                cmd.Parameters.AddWithValue("@observaciones", observaciones ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@idCPIC", ObtenerIdCPIC(cpic));

                                idOrdenViaje = (int)cmd.ExecuteScalar();
                            }

                            // Insertar en la tabla GuiasTransportista
                            string queryGuia = "INSERT INTO GuiasTransportista (numeroOrdenViaje, numeroGuiaTransportista, numeroGuiaCliente, ruta1, plantaDescarga, numeroManifiesto) " +
                                              "OUTPUT INSERTED.idGuia " +
                                              "VALUES (@numeroOrdenViaje, @numeroGuiaTransportista, @numeroGuiaCliente, @ruta1, @plantaDescarga, @numeroManifiesto)";
                            int idGuia;
                            using (SqlCommand cmd = new SqlCommand(queryGuia, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@numeroOrdenViaje", ordenViaje);
                                cmd.Parameters.AddWithValue("@numeroGuiaTransportista", guiaTransportista);
                                cmd.Parameters.AddWithValue("@numeroGuiaCliente", guiaCliente);
                                cmd.Parameters.AddWithValue("@ruta1", ruta);
                                cmd.Parameters.AddWithValue("@plantaDescarga", string.IsNullOrEmpty(plantaDescarga) ? (object)DBNull.Value : plantaDescarga);
                                cmd.Parameters.AddWithValue("@numeroManifiesto", string.IsNullOrEmpty(numManifiesto) ? (object)DBNull.Value : numManifiesto);

                                idGuia = (int)cmd.ExecuteScalar();
                            }

                            // Insertar los productos asociados en la tabla DetalleOrdenViaje
                            string queryProducto = "INSERT INTO DetalleOrdenViaje (idGuia, idProducto, cantidadBolsas) VALUES (@idGuia, @idProducto, @cantidad)";
                            foreach (var producto in productos)
                            {
                                using (SqlCommand cmd = new SqlCommand(queryProducto, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@idGuia", idGuia);
                                    cmd.Parameters.AddWithValue("@idProducto", producto.idProducto);
                                    cmd.Parameters.AddWithValue("@cantidad", producto.cantidad);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            //INSERTAR INGRESOS EN LA BASE DE DATOS
                            using (SqlCommand cmd = new SqlCommand("InsertarIngresos", conn, transaction))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@numeroOrdenViaje", ordenViaje);
                                cmd.Parameters.AddWithValue("@despachoSoles", despachoSoles);
                                cmd.Parameters.AddWithValue("@despachoDolares", despachoDolares);
                                cmd.Parameters.AddWithValue("@prestamoSoles", prestamoSoles);
                                cmd.Parameters.AddWithValue("@prestamosDolares", prestamoDolares);
                                cmd.Parameters.AddWithValue("@mensualidadSoles", mensualidadSoles);
                                cmd.Parameters.AddWithValue("@mensualidadDolares", mensualidadDolares);
                                cmd.Parameters.AddWithValue("@otrosSoles", otrosSoles);
                                cmd.Parameters.AddWithValue("@otrosDolares", otrosDolares);

                                // Puedes calcular total aquí si no lo haces desde frontend
                                decimal totalSoles = despachoSoles + prestamoSoles + mensualidadSoles + otrosSoles;
                                decimal totalDolares = despachoDolares + prestamoDolares + mensualidadDolares + otrosDolares;

                                cmd.Parameters.AddWithValue("@totalSoles", totalSoles);
                                cmd.Parameters.AddWithValue("@totalDolares", totalDolares);

                                cmd.Parameters.AddWithValue("@descDespacho", descDespacho);
                                cmd.Parameters.AddWithValue("@descMensualidad", descMensualidad);
                                cmd.Parameters.AddWithValue("@descOtrosAutorizados", descOtros);
                                cmd.Parameters.AddWithValue("@descPrestamo", descPrestamo);

                                cmd.ExecuteNonQuery();
                            }


                            //insertar egresos a la base de datos

                            using (SqlCommand cmd = new SqlCommand("InsertarEgresos", conn, transaction))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@numeroOrdenViaje", ordenViaje);
                                cmd.Parameters.AddWithValue("@peajesSoles", peajesSoles);
                                cmd.Parameters.AddWithValue("@peajesDolares", peajesDolares);
                                cmd.Parameters.AddWithValue("@descPeajes", descPeajes);

                                cmd.Parameters.AddWithValue("@alimentacionSoles", alimentacionSoles);
                                cmd.Parameters.AddWithValue("@alimentacionDolares", alimentacionDolares);
                                cmd.Parameters.AddWithValue("@descAlimentacion", descAlimentacion);

                                cmd.Parameters.AddWithValue("@apoyoseguridadSoles", apoyoSeguridadSoles);
                                cmd.Parameters.AddWithValue("@apoyoseguridadDolares", apoyoSeguridadDolares);
                                cmd.Parameters.AddWithValue("@descApoyoSeguridad", descApoyoSeguridad);

                                cmd.Parameters.AddWithValue("@reparacionesVariosSoles", reparacionesSoles);
                                cmd.Parameters.AddWithValue("@repacionesVariosDolares", reparacionesDolares);
                                cmd.Parameters.AddWithValue("@descReparacionesVarios", descReparaciones);

                                cmd.Parameters.AddWithValue("@movilidadSoles", movilidadSoles);
                                cmd.Parameters.AddWithValue("@movilidadDolares", movilidadDolares);
                                cmd.Parameters.AddWithValue("@descMovilidad", descMovilidad);

                                cmd.Parameters.AddWithValue("@encarpada_desencarpadaSoles", encapadaSoles);
                                cmd.Parameters.AddWithValue("@encarpada_desencarpadaDolares", encapadaDolares);
                                cmd.Parameters.AddWithValue("@descEncarpadaDesencarpada", descEncapada);

                                cmd.Parameters.AddWithValue("@hospedajeSoles", hospedajeSoles);
                                cmd.Parameters.AddWithValue("@hospedajeDolares", hospedajeDolares);
                                cmd.Parameters.AddWithValue("@descHospedaje", descHospedaje);

                                cmd.Parameters.AddWithValue("@combustibleSoles", combustibleSoles);
                                cmd.Parameters.AddWithValue("@combustibleDolares", combustibleDolares);
                                cmd.Parameters.AddWithValue("@descCombustible", descCombustible);

                                cmd.ExecuteNonQuery();
                            }

                            // Insertar gastos adicionales en CategoriasAdicionales
                            foreach (var gasto in gastosAdicionales)
                            {
                                using (SqlCommand cmd = new SqlCommand("InsertarGastoAdicional", conn, transaction))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@numeroOrdenViaje", ordenViaje);
                                    cmd.Parameters.AddWithValue("@nombreCategoria", gasto.nombreCategoria);
                                    cmd.Parameters.AddWithValue("@soles", gasto.soles);
                                    cmd.Parameters.AddWithValue("@dolares", gasto.dolares);
                                    cmd.Parameters.AddWithValue("@descripcion", gasto.descripcion);
                                    cmd.ExecuteNonQuery();
                                }
                            }




                            transaction.Commit();
                            lblErrores.Text = "✅ Orden de viaje guardada correctamente.";
                            ClientScript.RegisterStartupScript(this.GetType(), "cambiarTab", "$('#guias-tab').click();", true);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            lblErrores.Text = "Error al guardar la orden de viaje: " + ex.Message;
                            ClientScript.RegisterStartupScript(this.GetType(), "cambiarTab", "$('#guias-tab').click();", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error general: " + ex.Message;
                Response.Write("Error general: " + ex.Message + "<br/>");
                ClientScript.RegisterStartupScript(this.GetType(), "cambiarTab", "$('#guias-tab').click();", true);
            }
        }

        private int ObtenerIdCPIC(string numeroCPIC)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            string query = "SELECT idCPIC FROM CPIC WHERE numeroCPIC = @numeroCPIC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@numeroCPIC", numeroCPIC);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
    }

    public class ProductoOrdenViaje
    {
        public int idProducto { get; set; }
        public int cantidad { get; set; }
    }
}