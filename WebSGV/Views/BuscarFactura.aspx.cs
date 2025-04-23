using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace WebSGV.Views
{
    public partial class BusquedaFactura : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicializaciones necesarias
            }
        }

        protected void BuscarFacturaClick(object sender, EventArgs e)
        {
            string numeroFactura = txtBuscarFactura.Text.Trim();

            if (string.IsNullOrEmpty(numeroFactura))
            {
                MostrarMensaje("Por favor, ingrese un número de factura para buscar.", "danger");
                return;
            }

            try
            {
                // Buscar la factura en la base de datos
                FacturaModel factura = ObtenerFactura(numeroFactura);

                if (factura != null)
                {
                    // Mostrar datos de la factura
                    MostrarDatosFactura(factura);
                    pnlResultados.Visible = true;
                    pnlNoResultados.Visible = false;
                }
                else
                {
                    // No se encontró la factura
                    pnlResultados.Visible = false;
                    pnlNoResultados.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al buscar la factura: " + ex.Message, "danger");
            }
        }

        private FacturaModel ObtenerFactura(string numeroFactura)
        {
            FacturaModel factura = null;

            try
            {
                // Cadena de conexión - Ajustar según tu configuración
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta SQL para obtener la factura
                    string queryFactura = @"SELECT idFactura, numeroFactura, numeroPedido, valorTotal, fechaEmision 
                                          FROM Factura 
                                          WHERE numeroFactura = @numeroFactura";

                    using (SqlCommand command = new SqlCommand(queryFactura, connection))
                    {
                        command.Parameters.AddWithValue("@numeroFactura", numeroFactura);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                factura = new FacturaModel
                                {
                                    IdFactura = Convert.ToInt32(reader["idFactura"]),
                                    NumeroFactura = reader["numeroFactura"].ToString(),
                                    NumeroPedido = reader["numeroPedido"] != DBNull.Value ? reader["numeroPedido"].ToString() : "",
                                    ValorTotal = Convert.ToDecimal(reader["valorTotal"]),
                                    FechaEmision = Convert.ToDateTime(reader["fechaEmision"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la factura: " + ex.Message);
            }

            return factura;
        }

        private void MostrarDatosFactura(FacturaModel factura)
        {
            // Llenar los campos con la información de la factura
            txtNumFactura.Text = factura.NumeroFactura;
            txtNumPedido.Text = factura.NumeroPedido;
            txtFechaEmision.Text = factura.FechaEmision.ToString("yyyy-MM-dd");
            txtValorTotal.Text = factura.ValorTotal.ToString("N2");
        }

        protected void HabilitarEdicion(object sender, EventArgs e)
        {
            // Habilitar la edición de los campos
            txtNumPedido.ReadOnly = false;
            txtFechaEmision.ReadOnly = false;
            txtValorTotal.ReadOnly = false;

            // Mostrar el botón de guardar cambios
            btnHabilitarEdicion.Visible = false;
            btnGuardarCambios.Visible = true;

            MostrarMensaje("Modo de edición activado. Realice los cambios necesarios y presione 'Guardar Cambios'.", "info");
        }

        protected void GuardarCambios(object sender, EventArgs e)
        {
            try
            {
                // Recolectar datos actualizados
                string numeroFactura = txtNumFactura.Text;
                string numeroPedido = txtNumPedido.Text.Trim();

                // Validar número de pedido
                if (!string.IsNullOrEmpty(numeroPedido) && !ValidarNumeroPedido(numeroPedido))
                {
                    MostrarMensaje("El número de pedido debe tener exactamente 10 dígitos numéricos.", "danger");
                    return;
                }

                // Validar importe total
                if (!decimal.TryParse(txtValorTotal.Text, out decimal valorTotal) || valorTotal <= 0)
                {
                    MostrarMensaje("El valor total debe ser un número válido y mayor a 0.", "danger");
                    return;
                }

                // Validar fecha de emisión
                if (!DateTime.TryParse(txtFechaEmision.Text, out DateTime fechaEmision))
                {
                    MostrarMensaje("Formato de fecha inválido.", "danger");
                    return;
                }

                // Validar que la fecha no sea futura
                if (fechaEmision > DateTime.Now)
                {
                    MostrarMensaje("La fecha de emisión no puede ser mayor que la fecha actual.", "danger");
                    return;
                }

                // Actualizar la factura en la base de datos
                bool actualizado = ActualizarFactura(numeroFactura, numeroPedido, valorTotal, fechaEmision);

                if (actualizado)
                {
                    // Volver al modo de sólo lectura
                    txtNumPedido.ReadOnly = true;
                    txtFechaEmision.ReadOnly = true;
                    txtValorTotal.ReadOnly = true;
                    btnHabilitarEdicion.Visible = true;
                    btnGuardarCambios.Visible = false;

                    MostrarMensaje("Factura actualizada correctamente.", "success");
                }
                else
                {
                    MostrarMensaje("No se pudo actualizar la factura.", "danger");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar los cambios: " + ex.Message, "danger");
            }
        }

        private bool ActualizarFactura(string numeroFactura, string numeroPedido, decimal valorTotal, DateTime fechaEmision)
        {
            bool actualizado = false;

            try
            {
                // Cadena de conexión - Ajustar según tu configuración
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Verificar si el número de pedido ya está asociado a otra factura
                    if (!string.IsNullOrEmpty(numeroPedido))
                    {
                        string queryVerificarPedido = @"SELECT COUNT(*) 
                                                      FROM Factura 
                                                      WHERE numeroPedido = @numeroPedido 
                                                      AND numeroFactura <> @numeroFactura";

                        using (SqlCommand command = new SqlCommand(queryVerificarPedido, connection))
                        {
                            command.Parameters.AddWithValue("@numeroPedido", numeroPedido);
                            command.Parameters.AddWithValue("@numeroFactura", numeroFactura);

                            int count = (int)command.ExecuteScalar();
                            if (count > 0)
                            {
                                throw new Exception("El número de pedido ya está asociado a otra factura.");
                            }
                        }
                    }

                    // Actualizar factura
                    string queryActualizar = @"UPDATE Factura 
                                             SET numeroPedido = @numeroPedido, 
                                                 valorTotal = @valorTotal, 
                                                 fechaEmision = @fechaEmision 
                                             WHERE numeroFactura = @numeroFactura";

                    using (SqlCommand command = new SqlCommand(queryActualizar, connection))
                    {
                        command.Parameters.AddWithValue("@numeroFactura", numeroFactura);
                        command.Parameters.AddWithValue("@numeroPedido", string.IsNullOrEmpty(numeroPedido) ? (object)DBNull.Value : numeroPedido);
                        command.Parameters.AddWithValue("@valorTotal", valorTotal);
                        command.Parameters.AddWithValue("@fechaEmision", fechaEmision);

                        int rowsAffected = command.ExecuteNonQuery();
                        actualizado = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la factura: " + ex.Message);
            }

            return actualizado;
        }

        protected void Cancelar(object sender, EventArgs e)
        {
            // Limpiar los campos y ocultar paneles de resultados
            txtBuscarFactura.Text = "";
            pnlResultados.Visible = false;
            pnlNoResultados.Visible = false;
            lblMensaje.Text = "";
        }

        /// <summary>
        /// Valida que el número de pedido tenga exactamente 10 dígitos.
        /// </summary>
        private bool ValidarNumeroPedido(string numeroPedido)
        {
            string pattern = @"^\d{10}$"; // Exactamente 10 dígitos
            return Regex.IsMatch(numeroPedido, pattern);
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = $"text-{tipo}";
        }
    }

    // Clase modelo para la factura
    public class FacturaModel
    {
        public int IdFactura { get; set; }
        public string NumeroFactura { get; set; }
        public string NumeroPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime FechaEmision { get; set; }
    }
}