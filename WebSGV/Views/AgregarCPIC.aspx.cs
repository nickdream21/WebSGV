using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSGV.Views
{
    public partial class AgregarCPIC : System.Web.UI.Page
    {
        private static readonly Dictionary<string, decimal> facturaCache = new Dictionary<string, decimal>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        [WebMethod]
        public static decimal? GetValorTotalFactura(string numeroFactura)
        {
            try
            {
                Debug.WriteLine($"Buscando valor total para la factura: {numeroFactura}");

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT valorTotal FROM Factura WHERE numeroFactura = @numeroFactura";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@numeroFactura", numeroFactura);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            decimal valorTotal = Convert.ToDecimal(result);
                            Debug.WriteLine($"Valor total encontrado: {valorTotal}");
                            return valorTotal;
                        }
                        else
                        {
                            Debug.WriteLine("Factura no encontrada.");
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

       
        private void BindGridView()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Producto");
            dt.Columns.Add("Cantidad de Bolsas");
            dt.Columns.Add("Peso (Kg)");

            dt.Rows.Add("Producto 1", "10", "50");
            dt.Rows.Add("Producto 2", "5", "25");
            dt.Rows.Add("Producto 3", "15", "75");
        }



        protected void GuardarCPIC(object sender, EventArgs e)
        {
            try
            {
                // Validar datos del formulario
                string numeroFactura = txtNumFactura.Text.Trim();
                string numeroCPIC = txtNumCPIC.Text.Trim();
                DateTime fechaEmision = ValidarFechaEmision(txtFechaEmision.Text);
                decimal valorTotalFlete = ValidarValorTotalFlete(txtTotalFlete.Text);

                // Verificar duplicados en CPIC y Factura
                if (ExisteCPIC(numeroCPIC))
                {
                    MostrarMensaje("El número de CPIC ya existe. Por favor, ingrese un número único.", "error");
                    return;
                }

                if (ExisteFactura(numeroFactura))
                {
                    MostrarMensaje("El número de factura ya está asociado a otro CPIC. Por favor, ingrese una factura única.", "error");
                    return;
                }

                // Obtener datos de los productos
                DataTable productosTable = ObtenerProductosDeTabla();
                if (productosTable.Rows.Count == 0)
                {
                    MostrarMensaje("Debe agregar al menos un producto válido.", "error");
                    return;
                }

                // Registrar el CPIC llamando al procedimiento almacenado
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_RegistrarCPIC", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@numeroCPIC", numeroCPIC);
                        command.Parameters.AddWithValue("@numeroFactura", numeroFactura);
                        command.Parameters.AddWithValue("@valorTotalFlete", valorTotalFlete);
                        command.Parameters.AddWithValue("@fechaEmision", fechaEmision);

                        SqlParameter productosParam = command.Parameters.AddWithValue("@productos", productosTable);
                        productosParam.SqlDbType = SqlDbType.Structured;

                        command.ExecuteNonQuery();
                        MostrarMensaje("CPIC registrado correctamente.", "success");
                    }
                }
            }
            catch (SqlException ex)
            {
                // Analizar errores específicos de SQL Server
                if (ex.Message.Contains("El número de CPIC ya existe"))
                {
                    MostrarMensaje("El número de CPIC ya está registrado en el sistema. Por favor, ingrese un número único.", "error");
                }
                else if (ex.Message.Contains("El número de factura no existe"))
                {
                    MostrarMensaje("El número de factura ingresado no se encuentra en el sistema. Por favor, verifique e intente nuevamente.", "error");
                }
                else if (ex.Message.Contains("El número de factura ya está asociado"))
                {
                    MostrarMensaje("El número de factura ya está asociado a otro CPIC. Por favor, ingrese una factura única.", "error");
                }
                else
                {
                    MostrarMensaje("Error inesperado en la base de datos: " + ex.Message, "error");
                }
            }
            catch (Exception ex)
            {
                // Manejo genérico de excepciones
                MostrarMensaje("Error inesperado: " + ex.Message, "error");
            }
        }






        // Método para verificar si el número de CPIC ya existe
        private bool ExisteCPIC(string numeroCPIC)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM CPIC WHERE numeroCPIC = @numeroCPIC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@numeroCPIC", numeroCPIC);
                    int count = (int)command.ExecuteScalar();
                    return count > 0; // Si el número de CPIC ya existe, retorna true
                }
            }
        }

        // Método para verificar si el número de factura ya existe
        private bool ExisteFactura(string numeroFactura)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM CPIC WHERE numeroFactura = @numeroFactura";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@numeroFactura", numeroFactura);
                    int count = (int)command.ExecuteScalar();
                    return count > 0; // Si la factura ya existe, retorna true
                }
            }
        }
  

        private string ValidarNumeroCPIC(string numeroCPIC)
        {
            if (string.IsNullOrWhiteSpace(numeroCPIC) || numeroCPIC.Length != 7)
                throw new FormatException("El número de CPIC debe tener exactamente 7 caracteres.");
            return numeroCPIC.Trim();
        }

        private string ValidarNumeroFactura(string numeroFactura)
        {
            string pattern = @"^F\d{3} - \d{8}$";
            if (string.IsNullOrWhiteSpace(numeroFactura) || !Regex.IsMatch(numeroFactura, pattern))
                throw new FormatException("El número de factura debe tener el formato 'F### - ########'.");
            return numeroFactura.Trim();
        }

        private DateTime ValidarFechaEmision(string fechaTexto)
        {
            if (!DateTime.TryParse(fechaTexto, out DateTime fechaEmision))
                throw new FormatException("La fecha de emisión no es válida.");
            if (fechaEmision > DateTime.Now)
                throw new FormatException("La fecha de emisión no puede ser mayor que la fecha actual.");
            return fechaEmision;
        }

        private decimal ValidarValorTotalFlete(string valorTexto)
        {
            if (string.IsNullOrWhiteSpace(valorTexto) || !decimal.TryParse(valorTexto, out decimal valorTotal) || valorTotal <= 0)
            {
                throw new FormatException("El valor total del flete debe ser un número positivo.");
            }
            return valorTotal;
        }

        protected void TxtNumFactura_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroFactura = txtNumFactura.Text.Trim();

                if (string.IsNullOrWhiteSpace(numeroFactura))
                {
                    lblErrorFactura.Text = "Debe ingresar un número de factura.";
                    txtTotalFlete.Text = string.Empty;
                    return;
                }

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT valorTotal FROM Factura WHERE numeroFactura = @numeroFactura";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@numeroFactura", numeroFactura);
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            decimal valorTotal = Convert.ToDecimal(result);
                            txtTotalFlete.Text = valorTotal.ToString("0.00");
                        }
                        else
                        {
                            lblErrorFactura.Text = "El número de factura no existe.";
                            txtTotalFlete.Text = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrorFactura.Text = "Error al obtener el valor total del flete: " + ex.Message;
                txtTotalFlete.Text = string.Empty;
            }
        }

        private DataTable ObtenerProductosDeTabla()
        {
            DataTable productosTable = new DataTable();
            productosTable.Columns.Add("idProducto", typeof(int));
            productosTable.Columns.Add("cantidadBolsasProducto", typeof(int));
            productosTable.Columns.Add("pesoKg", typeof(decimal));

            // Obtener valores de los inputs en el formulario
            string[] productos = Request.Form.GetValues("productosDropdown");
            string[] cantidades = Request.Form.GetValues("cantidad");
            string[] pesos = Request.Form.GetValues("peso");

            if (productos == null || cantidades == null || pesos == null)
            {
                throw new Exception("No se han encontrado productos válidos.");
            }

            for (int i = 0; i < productos.Length; i++)
            {
                if (int.TryParse(productos[i], out int idProducto) && idProducto > 0 &&
                    int.TryParse(cantidades[i], out int cantidad) && cantidad > 0 &&
                    decimal.TryParse(pesos[i], out decimal peso) && peso > 0)
                {
                    // Verificar si el producto ya existe en la tabla
                    foreach (DataRow row in productosTable.Rows)
                    {
                        if ((int)row["idProducto"] == idProducto)
                        {
                            throw new Exception("El producto seleccionado ya ha sido agregado.");
                        }
                    }

                    productosTable.Rows.Add(idProducto, cantidad, peso);
                }
                else
                {
                    throw new Exception($"Error en la fila {i + 1}: Datos inválidos.");
                }
            }

            if (productosTable.Rows.Count == 0)
            {
                throw new Exception("Debe agregar al menos un producto válido.");
            }

            return productosTable;
        }




        protected string ObtenerProductosJSON()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString))
            {
                connection.Open();
                string query = "SELECT idProducto, nombre FROM Producto";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }


        private void MostrarMensaje(string mensaje, string tipo)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = tipo == "success" ? "text-success" : "text-danger";
        }
    }
}
