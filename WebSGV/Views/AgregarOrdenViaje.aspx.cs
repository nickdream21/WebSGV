using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace WebSGV.Views
{
    public partial class AgregarOrdenViaje : System.Web.UI.Page
    {
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
            string query = "SELECT placaTracto FROM Tracto";
            DataTable dt = ObtenerDatosDeBD(query);

            if (dt.Rows.Count > 0)
            {
                ddlPlacaTracto.DataSource = dt;
                ddlPlacaTracto.DataTextField = "placaTracto";
                ddlPlacaTracto.DataValueField = "placaTracto";
                ddlPlacaTracto.DataBind();
            }

            ddlPlacaTracto.Items.Insert(0, new ListItem("Seleccione una placa", ""));
        }

        private void CargarPlacasCarreta()
        {
            string query = "SELECT placaCarreta FROM Carreta";
            DataTable dt = ObtenerDatosDeBD(query);

            if (dt.Rows.Count > 0)
            {
                ddlPlacaCarreta.DataSource = dt;
                ddlPlacaCarreta.DataTextField = "placaCarreta";
                ddlPlacaCarreta.DataValueField = "placaCarreta";
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

        private void CargarPlantasDescarga()
        {
            string query = "SELECT idPlanta, nombre FROM PlantaDescarga";
            DataTable dt = ObtenerDatosDeBD(query);

            if (dt.Rows.Count > 0)
            {
                ddlPlantaDescarga.DataSource = dt;
                ddlPlantaDescarga.DataTextField = "nombre";
                ddlPlantaDescarga.DataValueField = "idPlanta";
                ddlPlantaDescarga.DataBind();
            }

            ddlPlantaDescarga.Items.Insert(0, new ListItem("Seleccione una planta", ""));
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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener los datos de la pestaña "Datos del Viaje"
                string cpic = txtCPI.Value.Trim();
                string ordenViaje = txtOrdenViaje.Value.Trim();
                DateTime fechaSalida = string.IsNullOrEmpty(txtFechaSalida.Value) ? DateTime.MinValue : DateTime.Parse(txtFechaSalida.Value);
                DateTime fechaLlegada = string.IsNullOrEmpty(txtFechaLlegada.Value) ? DateTime.MinValue : DateTime.Parse(txtFechaLlegada.Value);
                string cliente = ddlCliente.SelectedValue;
                string placaTracto = ddlPlacaTracto.SelectedValue;
                string placaCarreta = ddlPlacaCarreta.SelectedValue;
                string conductor = ddlConductor.SelectedValue;
                string observaciones = txtObservaciones.Value;

                // Obtener los datos de la pestaña "Guías"
                string guiaTransportista = Request.Form["txtGuiaTransportista"];
                string guiaCliente = Request.Form["txtGuiaCliente"];
                string ruta = ddlRuta.SelectedValue;
                string plantaDescarga = ddlPlantaDescarga.SelectedValue;
                string numManifiesto = txtNumManifiesto.Text;

                // Validar datos antes de guardar
                string errores = ValidarDatosViaje(cpic, ordenViaje, fechaSalida, fechaLlegada, txtHoraSalida.Value, txtHoraLlegada.Value, cliente, placaTracto, placaCarreta, conductor);
                if (string.IsNullOrEmpty(guiaTransportista))
                {
                    errores += "El campo 'N° Guía Transportista' es obligatorio.\n";
                }
                if (string.IsNullOrEmpty(guiaCliente))
                {
                    errores += "El campo 'N° Guía Cliente' es obligatorio.\n";
                }

                if (!string.IsNullOrEmpty(errores))
                {
                    lblErrores.Text = errores.Replace("\n", "<br/>");
                    ClientScript.RegisterStartupScript(this.GetType(), "cambiarTab", "$('#guias-tab').click();", true);
                    return;
                }

                // Guardar en la base de datos
                string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO OrdenViaje (numeroCPIC, numeroOrdenViaje, fechaSalida, fechaLlegada, idCliente, placaTracto, placaCarreta, idConductor, observaciones, guiaTransportista, guiaCliente, idRuta, plantaDescarga, numeroManifiesto) " +
                                                           "VALUES (@numeroCPIC, @numeroOrdenViaje, @fechaSalida, @fechaLlegada, @idCliente, @placaTracto, @placaCarreta, @idConductor, @observaciones, @guiaTransportista, @guiaCliente, @idRuta, @plantaDescarga, @numeroManifiesto)", conn))
                    {
                        cmd.Parameters.AddWithValue("@numeroCPIC", cpic);
                        cmd.Parameters.AddWithValue("@numeroOrdenViaje", ordenViaje);
                        cmd.Parameters.AddWithValue("@fechaSalida", fechaSalida == DateTime.MinValue ? (object)DBNull.Value : fechaSalida);
                        cmd.Parameters.AddWithValue("@fechaLlegada", fechaLlegada == DateTime.MinValue ? (object)DBNull.Value : fechaLlegada);
                        cmd.Parameters.AddWithValue("@idCliente", cliente);
                        cmd.Parameters.AddWithValue("@placaTracto", placaTracto);
                        cmd.Parameters.AddWithValue("@placaCarreta", placaCarreta);
                        cmd.Parameters.AddWithValue("@idConductor", conductor);
                        cmd.Parameters.AddWithValue("@observaciones", observaciones ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@guiaTransportista", guiaTransportista);
                        cmd.Parameters.AddWithValue("@guiaCliente", guiaCliente);
                        cmd.Parameters.AddWithValue("@idRuta", string.IsNullOrEmpty(ruta) ? (object)DBNull.Value : ruta);
                        cmd.Parameters.AddWithValue("@plantaDescarga", string.IsNullOrEmpty(plantaDescarga) ? (object)DBNull.Value : plantaDescarga);
                        cmd.Parameters.AddWithValue("@numeroManifiesto", string.IsNullOrEmpty(numManifiesto) ? (object)DBNull.Value : numManifiesto);

                        cmd.ExecuteNonQuery();
                    }
                }

                lblErrores.Text = "✅ Orden de viaje guardada correctamente.";
                ClientScript.RegisterStartupScript(this.GetType(), "cambiarTab", "$('#guias-tab').click();", true);
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al guardar la orden de viaje: " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "cambiarTab", "$('#guias-tab').click();", true);
            }
        }
    }
}