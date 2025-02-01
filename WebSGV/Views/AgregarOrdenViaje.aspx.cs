using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Services;
using System.Text.RegularExpressions;

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

            }
        }
        private void CargarClientes()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            string query = "SELECT idCliente, nombre FROM Cliente";

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

                        ddlCliente.DataSource = dt;
                        ddlCliente.DataTextField = "nombre";
                        ddlCliente.DataValueField = "idCliente";
                        ddlCliente.DataBind();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            ddlCliente.Items.Insert(0, new ListItem("Seleccione un cliente", ""));
        }

        private void CargarPlacasTracto()
        {
            ViewState["PlacasTracto"] = ObtenerDatosDeBD("SELECT placaTracto FROM Tracto");
        }

        private void CargarPlacasCarreta()
        {
            ViewState["PlacasCarreta"] = ObtenerDatosDeBD("SELECT placaCarreta FROM Carreta");
        }

        private void CargarConductores()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            string query = "SELECT CONCAT(nombre, ' ', apPaterno, ' ', apMaterno) AS nombreCompleto FROM Conductor";

            List<string> conductores = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            conductores.Add(reader["nombreCompleto"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al cargar conductores: " + ex.Message);
                    }
                }
            }

            // Guardamos los datos en el ViewState para usarlos en el cliente
            ViewState["Conductores"] = JsonConvert.SerializeObject(conductores);
        }


        private string ObtenerDatosDeBD(string query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            List<string> resultados = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            resultados.Add(reader[0].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al cargar datos: " + ex.Message);
                    }
                }
            }

            return JsonConvert.SerializeObject(resultados);
        }



        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            string cpic = txtCPI.Value.Trim();
            string ordenViaje = txtOrdenViaje.Value.Trim();
            DateTime fechaSalida = string.IsNullOrEmpty(txtFechaSalida.Value) ? DateTime.MinValue : DateTime.Parse(txtFechaSalida.Value);
            DateTime fechaLlegada = string.IsNullOrEmpty(txtFechaLlegada.Value) ? DateTime.MinValue : DateTime.Parse(txtFechaLlegada.Value);

            // Validaciones
            string errores = ValidarDatosViaje(cpic, ordenViaje, fechaSalida, fechaLlegada);

            if (!string.IsNullOrEmpty(errores))
            {
                // Mostrar errores al usuario
                lblErrores.Text = errores.Replace("\n", "<br/>"); // Muestra errores en el Label
                return;
            }

            // Enviar señal a JavaScript para cambiar de pestaña
            ClientScript.RegisterStartupScript(this.GetType(), "cambiarTab", "$('#liquidacion-tab').click();", true);
        }


        private string ValidarDatosViaje(string cpic, string ordenViaje, DateTime fechaSalida, DateTime fechaLlegada)
        {
            string mensajeError = "";

            // Validar CPIC
            if (string.IsNullOrEmpty(cpic))
            {
                mensajeError += "El campo 'N° CPI' es obligatorio.\n";
            }
            else if (!EsCPICValido(cpic))
            {
                mensajeError += "El N° CPI ingresado no existe en la base de datos.\n";
            }

            // Validar que la Orden de Viaje sea única
            if (string.IsNullOrEmpty(ordenViaje))
            {
                mensajeError += "El campo 'N° Orden Viaje' es obligatorio.\n ";

            }
            else if (!Regex.IsMatch(ordenViaje, @"^\d{6}$"))
            {
                mensajeError += "El N° Orden Viaje debe contener exactamente 6 dígitos.\n";
            }
            else if (!EsOrdenDeViajeUnica(ordenViaje))
            {
                mensajeError += "El N° Orden Viaje ya está registrado.\n variable:"+ordenViaje;
            }

            // Validar fechas
            DateTime fechaActual = DateTime.Now;
            if (fechaSalida > fechaActual)
            {
                mensajeError += "La Fecha de Salida no puede ser mayor a la fecha actual.\n";
            }
            if (fechaLlegada > fechaActual)
            {
                mensajeError += "La Fecha de Llegada no puede ser mayor a la fecha actual.\n";
            }
            if (fechaSalida != DateTime.MinValue && fechaLlegada != DateTime.MinValue && fechaSalida > fechaLlegada)
            {
                mensajeError += "La Fecha de Salida no puede ser posterior a la Fecha de Llegada.\n";
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
                    cmd.Parameters.AddWithValue("@cpic", cpic);
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0; // Retorna true si existe
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
                    cmd.Parameters.AddWithValue("@ordenViaje", ordenViaje);
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count == 0; // Devuelve true si NO existe en la BD
                }
            }
        }



       

    }


}

