using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSGV.Views
{
    public partial class AgregarAbastecimiento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Solo para propósitos de prueba
                System.Diagnostics.Debug.WriteLine("Página cargada - inicializando valores");

                // Podemos establecer valores iniciales
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtHora.Text = DateTime.Now.ToString("HH:mm");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            // Aquí el código para guardar los datos
            // Por ejemplo:
            // GuardarAbastecimiento();
        }



        // Métodos para el autocompletado

        [WebMethod]
        public static List<string> GetTractoPlacas(string prefixText)
        {
            List<string> placas = new List<string>();
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT placaTracto FROM Tracto WHERE placaTracto LIKE @Search + '%'";
                cmd.Parameters.AddWithValue("@Search", prefixText);
                cmd.Connection = conn;

                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    placas.Add(sdr["placaTracto"].ToString());
                }
                conn.Close();
            }

            return placas;
        }


        [WebMethod]
        public static List<string> GetConductores(string prefixText)
        {
            List<string> conductores = new List<string>();
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT nombre + ' ' + apPaterno + ' ' + apMaterno AS nombreCompleto FROM Conductor " +
                                 "WHERE nombre LIKE @Search + '%' OR apPaterno LIKE @Search + '%' OR apMaterno LIKE @Search + '%'";
                cmd.Parameters.AddWithValue("@Search", prefixText);
                cmd.Connection = conn;

                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    conductores.Add(sdr["nombreCompleto"].ToString());
                }
                conn.Close();
            }

            return conductores;
        }


        [WebMethod]
        public static List<string> GetRutas(string prefixText)
        {
            List<string> rutas = new List<string>();
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT nombre FROM Ruta WHERE nombre LIKE @Search + '%'";
                cmd.Parameters.AddWithValue("@Search", prefixText);
                cmd.Connection = conn;

                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    rutas.Add(sdr["nombre"].ToString());
                }
                conn.Close();
            }

            return rutas;
        }


        [WebMethod]
        public static List<string> GetCarretas(string prefixText)
        {
            // Este es un método estático de prueba - reemplázalo con consulta a la base de datos real
            List<string> carretas = new List<string>();

            // Solo para pruebas (puedes cambiarlo a la lógica que necesites)
            carretas.Add("P1E-" + prefixText);
            carretas.Add("TOC-" + prefixText);
            carretas.Add("TDZ-" + prefixText);
            carretas.Add("TEA-" + prefixText);
            carretas.Add("TJI-" + prefixText);

            return carretas.Where(c => c.ToLower().Contains(prefixText.ToLower())).ToList();
        }


    }
}