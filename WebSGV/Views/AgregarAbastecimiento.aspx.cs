using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
                CargarPlacasTracto();
                CargarPlacasCarreta();
                CargarConductores();
                CargarRutas();
            }
        }

        private void CargarPlacasTracto()
        {
            string query = "SELECT placaTracto, placaTracto as nombre FROM Tracto";
            DataTable dt = ObtenerDatosDeBD(query);
            if (dt.Rows.Count > 0)
            {
                ddlPlaca.DataSource = dt;
                ddlPlaca.DataTextField = "nombre";
                ddlPlaca.DataValueField = "placaTracto";
                ddlPlaca.DataBind();
            }
            ddlPlaca.Items.Insert(0, new ListItem("Seleccione una placa", ""));
        }

        private void CargarPlacasCarreta()
        {
            // Suponiendo que tienes una tabla de Carretas
            string query = "SELECT placaCarreta, placaCarreta as nombre FROM Carreta";
            DataTable dt = ObtenerDatosDeBD(query);
            if (dt.Rows.Count > 0)
            {
                ddlCarreta.DataSource = dt;
                ddlCarreta.DataTextField = "nombre";
                ddlCarreta.DataValueField = "placaCarreta";
                ddlCarreta.DataBind();
            }
            ddlCarreta.Items.Insert(0, new ListItem("Seleccione una carreta", ""));
        }

        private void CargarConductores()
        {
            string query = "SELECT idConductor, nombre + ' ' + apPaterno + ' ' + apMaterno AS nombreCompleto FROM Conductor";
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

        private DataTable ObtenerDatosDeBD(string query)
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarDatos())
                {
                    GuardarAbastecimiento();
                    LimpiarFormulario();
                    MostrarMensaje("Abastecimiento guardado correctamente", "success");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar: " + ex.Message, "error");
            }
        }

        private bool ValidarDatos()
        {
            // Validar campos obligatorios
            if (ddlPlaca.SelectedIndex == 0)
            {
                MostrarMensaje("Debe seleccionar una placa", "warning");
                return false;
            }

            if (ddlConductor.SelectedIndex == 0)
            {
                MostrarMensaje("Debe seleccionar un conductor", "warning");
                return false;
            }

            if (string.IsNullOrEmpty(txtProducto.Text))
            {
                MostrarMensaje("Debe ingresar el producto", "warning");
                return false;
            }

            // Validar campos numéricos
            if (!decimal.TryParse(txtGLRuta.Text, out decimal glRuta) || glRuta < 0)
            {
                MostrarMensaje("Ingrese un valor válido para Galones Ruta Asignada", "warning");
                return false;
            }

            // Agrega más validaciones según sea necesario

            return true;
        }

        private void GuardarAbastecimiento()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertarAbastecimientoCombustible", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros básicos
                    cmd.Parameters.Add("@numeroAbastecimientoCombustible", SqlDbType.Char, 6).Value = numeroAbastecimiento.Text;
                    cmd.Parameters.Add("@idTracto", SqlDbType.Int).Value = Convert.ToInt32(ddlPlaca.SelectedValue);
                    cmd.Parameters.Add("@idCarreta", SqlDbType.Int).Value = (ddlCarreta.SelectedIndex > 0) ? Convert.ToInt32(ddlCarreta.SelectedValue) : (object)DBNull.Value;
                    cmd.Parameters.Add("@idConductor", SqlDbType.Int).Value = Convert.ToInt32(ddlConductor.SelectedValue);
                    cmd.Parameters.Add("@idRuta", SqlDbType.Int).Value = (ddlRuta.SelectedIndex > 0) ? Convert.ToInt32(ddlRuta.SelectedValue) : (object)DBNull.Value;
                    cmd.Parameters.Add("@producto", SqlDbType.VarChar, 100).Value = txtProducto.Text;
                    cmd.Parameters.Add("@idLugarAbastecimiento", SqlDbType.Int).Value = Convert.ToInt32(lugarAbastecimiento.SelectedValue);

                    // Fecha y hora
                    DateTime fecha = Convert.ToDateTime(txtFecha.Text);
                    TimeSpan hora = TimeSpan.Parse(txtHora.Text);
                    DateTime fechaHora = fecha.Date.Add(hora);
                    cmd.Parameters.Add("@fechaHora", SqlDbType.DateTime).Value = fechaHora;

                    // Valores numéricos
                    cmd.Parameters.Add("@galonesRutaAsignada", SqlDbType.Decimal).Value = Convert.ToDecimal(txtGLRuta.Text);
                    cmd.Parameters.Add("@galonesCompradosRuta", SqlDbType.Decimal).Value = Convert.ToDecimal(txtGLComprados.Text);
                    cmd.Parameters.Add("@galonesTotalAbastecidos", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTotalGL.Text);
                    cmd.Parameters.Add("@galonesAlFinalizar", SqlDbType.Decimal).Value = Convert.ToDecimal(txtGLFinal.Text);
                    cmd.Parameters.Add("@galonesTotalConsumidos", SqlDbType.Decimal).Value = Convert.ToDecimal(txtGLConsumidos.Text);
                    cmd.Parameters.Add("@precioDolar", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPrecioDolar.Text);
                    cmd.Parameters.Add("@montoTotalGalonesComprados", SqlDbType.Decimal).Value = Convert.ToDecimal(txtMontoTotal.Text);
                    cmd.Parameters.Add("@distanciaRutaKM", SqlDbType.Decimal).Value = Convert.ToDecimal(txtDistancia.Text);
                    cmd.Parameters.Add("@consumoComputador", SqlDbType.Decimal).Value = Convert.ToDecimal(txtConsumoComputador.Text);

                    // Calcular rendimiento
                    decimal distancia = Convert.ToDecimal(txtDistancia.Text);
                    decimal consumido = Convert.ToDecimal(txtGLConsumidos.Text);
                    decimal rendimiento = (consumido > 0) ? distancia / consumido : 0;
                    cmd.Parameters.Add("@rendimientoPromedio", SqlDbType.Decimal).Value = rendimiento;

                    // Otros campos
                    cmd.Parameters.Add("@observaciones", SqlDbType.VarChar, 300).Value = txtObservaciones.Text;

                    if (!string.IsNullOrEmpty(txtHoraRetorno.Text))
                    {
                        cmd.Parameters.Add("@horaRetorno", SqlDbType.Time).Value = TimeSpan.Parse(txtHoraRetorno.Text);
                    }
                    else
                    {
                        cmd.Parameters.Add("@horaRetorno", SqlDbType.Time).Value = DBNull.Value;
                    }

                    cmd.Parameters.Add("@idTipoCarro", SqlDbType.Int).Value = Convert.ToInt32(tipoVehiculo.SelectedValue);

                    // Buscar y vincular con la orden de viaje correspondiente
                    int? idOrdenViaje = BuscarOrdenViajeRelacionada(Convert.ToInt32(ddlConductor.SelectedValue), fecha);
                    cmd.Parameters.Add("@idOrdenViaje", SqlDbType.Int).Value = (idOrdenViaje.HasValue) ? idOrdenViaje.Value : (object)DBNull.Value;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private int? BuscarOrdenViajeRelacionada(int idConductor, DateTime fecha)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    // Buscar órdenes de viaje con el mismo conductor y fecha similar (mismo día)
                    cmd.CommandText = @"SELECT TOP 1 idOrdenViaje 
                               FROM OrdenViaje 
                               WHERE idConductor = @idConductor 
                               AND CONVERT(date, fechaLlegada) = CONVERT(date, @fecha)
                               ORDER BY fechaLlegada DESC";

                    cmd.Parameters.AddWithValue("@idConductor", idConductor);
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Connection = conn;

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }

                    return null;
                }
            }
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            // Implementar según tu sistema de notificaciones
            // Por ejemplo, usando ScriptManager:
            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje",
                $"alert('{mensaje}');", true);
        }

        private void LimpiarFormulario()
        {
            // Reutilizar la función JavaScript existente
            ScriptManager.RegisterStartupScript(this, this.GetType(), "limpiarForm",
                "limpiarFormulario();", true);
        }





    }
}