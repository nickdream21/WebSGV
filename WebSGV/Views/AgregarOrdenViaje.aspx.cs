using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WebSGV.Views
{
    public partial class AgregarOrdenViaje : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
           
            }
        }

        private void CargarOpcionesRuta()
        {

        }

        private void CargarOpcionesPlanta()
        {

        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar los campos
                string numeroCPI = Request.Form["txtCPI"];
                string numeroOrdenViaje = Request.Form["txtOrdenViaje"];
                string placaTracto = Request.Form["ddlTracto"];
                string placaCarreta = Request.Form["ddlCarreta"];
                string conductor = Request.Form["ddlConductor"];
                string observaciones = Request.Form["txtObservaciones"];

                if (string.IsNullOrWhiteSpace(numeroCPI) || string.IsNullOrWhiteSpace(numeroOrdenViaje))
                {
                    MostrarMensaje("Los campos N° CPI y N° Orden Viaje son obligatorios.", "danger");
                    return;
                }

                DateTime fechaSalida = DateTime.Parse(Request.Form["txtFechaSalida"]);
                TimeSpan horaSalida = TimeSpan.Parse(Request.Form["txtHoraSalida"]);
                DateTime fechaLlegada = DateTime.Parse(Request.Form["txtFechaLlegada"]);
                TimeSpan horaLlegada = TimeSpan.Parse(Request.Form["txtHoraLlegada"]);

                if (fechaLlegada < fechaSalida || (fechaLlegada == fechaSalida && horaLlegada <= horaSalida))
                {
                    MostrarMensaje("La fecha y hora de llegada deben ser posteriores a la salida.", "danger");
                    return;
                }

                // Guardar en la base de datos
                GuardarOrdenDeViaje(numeroCPI, numeroOrdenViaje, fechaSalida, horaSalida, fechaLlegada, horaLlegada, placaTracto, placaCarreta, conductor, observaciones);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Ocurrió un error al guardar la orden de viaje: {ex.Message}", "danger");
            }
        }

        private void GuardarOrdenDeViaje(string cpi, string ordenViaje, DateTime fechaSalida, TimeSpan horaSalida, DateTime fechaLlegada, TimeSpan horaLlegada, string tracto, string carreta, string conductor, string observaciones)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_InsertarOrdenViaje", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    command.Parameters.AddWithValue("@numeroCPI", cpi);
                    command.Parameters.AddWithValue("@numeroOrdenViaje", ordenViaje);
                    command.Parameters.AddWithValue("@fechaSalida", fechaSalida);
                    command.Parameters.AddWithValue("@horaSalida", horaSalida);
                    command.Parameters.AddWithValue("@fechaLlegada", fechaLlegada);
                    command.Parameters.AddWithValue("@horaLlegada", horaLlegada);
                    command.Parameters.AddWithValue("@placaTracto", tracto);
                    command.Parameters.AddWithValue("@placaCarreta", carreta);
                    command.Parameters.AddWithValue("@conductor", conductor);
                    command.Parameters.AddWithValue("@observaciones", observaciones);

                    conexion.Open();
                    command.ExecuteNonQuery();

                    MostrarMensaje("Orden de viaje guardada exitosamente.", "success");
                }
            }
        }

        protected void MostrarMensaje(string mensaje, string tipo)
        {
            string script = $"alert('{mensaje}');";
            ClientScript.RegisterStartupScript(this.GetType(), "Mensaje", script, true);
        }


        [System.Web.Services.WebMethod]
        public static List<string> BuscarConductores(string termino)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            List<string> resultados = new List<string>();

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string query = "SELECT RTRIM(apPaterno) + ' ' + RTRIM(apMaterno) + ' ' + RTRIM(nombre) AS NombreCompleto FROM Conductor WHERE nombre LIKE @termino + '%' OR apPaterno LIKE @termino + '%' OR apMaterno LIKE @termino + '%'";
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@termino", termino);
                    conexion.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultados.Add(reader["NombreCompleto"].ToString());
                        }
                    }
                }
            }

            return resultados;
        }

        [System.Web.Services.WebMethod]
        public static List<string> BuscarPlacasTracto(string termino)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            List<string> resultados = new List<string>();

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string query = "SELECT placaTracto FROM Tracto WHERE placaTracto LIKE @termino + '%'";
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@termino", termino);
                    conexion.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultados.Add(reader["placaTracto"].ToString());
                        }
                    }
                }
            }

            return resultados;
        }


        [System.Web.Services.WebMethod]
        public static List<string> BuscarPlacasCarreta(string termino)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            List<string> resultados = new List<string>();

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string query = "SELECT placaCarreta FROM Carreta WHERE placaCarreta LIKE @termino + '%'";
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@termino", termino);
                    conexion.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultados.Add(reader["placaCarreta"].ToString());
                        }
                    }
                }
            }

            return resultados;
        }




    }

}