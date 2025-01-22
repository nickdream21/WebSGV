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

    }
}

