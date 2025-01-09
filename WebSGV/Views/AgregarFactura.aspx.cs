using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace WebSGV.Views
{
    public partial class AgregarFactura : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void GuardarFactura(object sender, EventArgs e)
        {
            try
            {
                // Validar y formatear el número de factura
                string numeroFactura = ValidarYFormatearNumeroFactura(txtNumFactura.Text);

                // Validar el importe total
                if (!decimal.TryParse(txtImporteTotal.Text, out decimal valorTotal) || valorTotal <= 0)
                {
                    lblMensaje.Text = "El importe total debe ser un número válido y mayor a 0.";
                    lblMensaje.CssClass = "text-danger";
                    return;
                }

                // Validar la fecha de emisión
                if (!DateTime.TryParse(txtFechaEmision.Text, out DateTime fechaEmision))
                {
                    lblMensaje.Text = "La fecha de emisión no es válida.";
                    lblMensaje.CssClass = "text-danger";
                    return;
                }

                // Validar que la fecha no sea futura
                if (fechaEmision > DateTime.Now)
                {
                    lblMensaje.Text = "La fecha de emisión no puede ser mayor que la fecha actual.";
                    lblMensaje.CssClass = "text-danger";
                    return;
                }


                // Conexión a la base de datos
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_InsertarFactura", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Pasar los parámetros al procedimiento almacenado
                        command.Parameters.AddWithValue("@numeroFactura", numeroFactura);
                        command.Parameters.AddWithValue("@valorTotal", valorTotal);
                        command.Parameters.AddWithValue("@fechaEmision", fechaEmision);

                        command.ExecuteNonQuery();

                        // Mostrar mensaje de éxito
                        lblMensaje.Text = "Factura registrada correctamente.";
                        lblMensaje.CssClass = "text-success";
                    }
                }
            }
            catch (FormatException ex)
            {
                lblMensaje.Text = "Error en el formato de entrada: " + ex.Message;
                lblMensaje.CssClass = "text-danger";
            }
            catch (SqlException ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
                lblMensaje.CssClass = "text-danger";
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al registrar la factura: " + ex.Message;
                lblMensaje.CssClass = "text-danger";
            }
        }




        /// <summary>
        /// Valida y ajusta el formato del número de factura.
        /// </summary>
        private string ValidarYFormatearNumeroFactura(string numeroFactura)
        {
            // Expresión regular para el formato correcto
            string pattern = @"^F\d{3} - \d{8}$";

            if (Regex.IsMatch(numeroFactura, pattern))
            {
                // Si ya cumple el formato, retornarlo sin cambios
                return numeroFactura;
            }

            // Intentar corregir el formato
            string soloNumerosYLetras = Regex.Replace(numeroFactura, @"[^A-Za-z0-9]", "");

            if (soloNumerosYLetras.Length == 12 && soloNumerosYLetras.StartsWith("F"))
            {
                string codigo = soloNumerosYLetras.Substring(0, 4); // Ejemplo: "F222"
                string secuencia = soloNumerosYLetras.Substring(4); // Ejemplo: "00004267"                     
                return $"{codigo} - {secuencia}";
            }

            // Si no es posible corregir, lanzar excepción
            throw new FormatException("El número de factura debe tener el formato 'F222 - 00004267'.");
        }
    }

}
