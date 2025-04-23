using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
// Asegúrate de tener esta referencia si necesitas DataTable, etc.
using System.Data;

namespace WebSGV.Views
{
    public partial class BusquedaCPIC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicializaciones necesarias
            }
        }

        protected void BuscarCPICClick(object sender, EventArgs e)
        {
            string numeroCPIC = txtBuscarCPIC.Text.Trim();

            if (string.IsNullOrEmpty(numeroCPIC))
            {
                MostrarMensaje("Por favor, ingrese un número de CPIC para buscar.", "danger");
                return;
            }

            try
            {
                // Buscar el CPIC en la base de datos
                CPIC_Corregido cpic = ObtenerCPIC(numeroCPIC);

                if (cpic != null)
                {
                    // Mostrar datos del CPIC
                    MostrarDatosCPIC(cpic);
                    pnlResultados.Visible = true;
                    pnlNoResultados.Visible = false;
                }
                else
                {
                    // No se encontró el CPIC
                    pnlResultados.Visible = false;
                    pnlNoResultados.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al buscar el CPIC: " + ex.Message, "danger");
            }
        }

        private CPIC_Corregido ObtenerCPIC(string numeroCPIC)
        {
            // Implementa la lógica para obtener el CPIC desde la base de datos utilizando ADO.NET
            CPIC_Corregido cpic = null;

            try
            {
                // Cadena de conexión - Ajustar según tu configuración
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();

                    // 1. Obtener información del CPIC
                    string queryCPIC = @"SELECT c.idCPIC, c.numeroCPIC, c.idFactura, c.valorTotalFlete, c.fechaEmision, f.numeroFactura 
                                        FROM CPIC c 
                                        LEFT JOIN Factura f ON c.idFactura = f.idFactura 
                                        WHERE c.numeroCPIC = @numeroCPIC";

                    using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(queryCPIC, connection))
                    {
                        command.Parameters.AddWithValue("@numeroCPIC", numeroCPIC);

                        using (System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cpic = new CPIC_Corregido
                                {
                                    IdCPIC = Convert.ToInt32(reader["idCPIC"]),
                                    NumeroCPIC = reader["numeroCPIC"].ToString(),
                                    IdFactura = reader["idFactura"] != DBNull.Value ? Convert.ToInt32(reader["idFactura"]) : 0,
                                    NumeroFactura = reader["numeroFactura"] != DBNull.Value ? reader["numeroFactura"].ToString() : "",
                                    FechaEmision = Convert.ToDateTime(reader["fechaEmision"]),
                                    ValorTotalFlete = Convert.ToDecimal(reader["valorTotalFlete"]),
                                    Productos = new List<ProductoCPIC_Corregido>()
                                };
                            }
                        }
                    }

                    // 2. Si encontramos el CPIC, obtener sus productos
                    if (cpic != null)
                    {
                        string queryProductos = @"SELECT cp.idCPIC, cp.idProducto, cp.cantidadBolsasProducto, cp.pesoKg, p.nombre as nombreProducto 
                                                FROM CPIC_Productos cp 
                                                JOIN Producto p ON cp.idProducto = p.idProducto 
                                                WHERE cp.idCPIC = @idCPIC";

                        using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(queryProductos, connection))
                        {
                            command.Parameters.AddWithValue("@idCPIC", cpic.IdCPIC);

                            using (System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                            {
                                int id = 1; // ID para el GridView
                                while (reader.Read())
                                {
                                    cpic.Productos.Add(new ProductoCPIC_Corregido
                                    {
                                        ID = id++,
                                        IdCPIC = cpic.IdCPIC,
                                        IdProducto = Convert.ToInt32(reader["idProducto"]),
                                        NombreProducto = reader["nombreProducto"].ToString(),
                                        CantidadBolsas = Convert.ToInt32(reader["cantidadBolsasProducto"]),
                                        PesoKg = Convert.ToDecimal(reader["pesoKg"])
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Registrar el error o manejarlo adecuadamente
                throw new Exception("Error al obtener el CPIC: " + ex.Message);
            }

            return cpic;
        }

        private void MostrarDatosCPIC(CPIC_Corregido cpic)
        {
            // Llenar los campos con la información del CPIC
            txtNumCPIC.Text = cpic.NumeroCPIC;
            txtNumFactura.Text = cpic.NumeroFactura;
            txtFechaEmision.Text = cpic.FechaEmision.ToString("yyyy-MM-dd");
            txtTotalFlete.Text = cpic.ValorTotalFlete.ToString("N2");

            // Cargar la tabla de productos
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("IdProducto", typeof(int));
            dt.Columns.Add("NombreProducto", typeof(string));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Peso", typeof(decimal));

            foreach (var producto in cpic.Productos)
            {
                dt.Rows.Add(producto.ID, producto.IdProducto, producto.NombreProducto, producto.CantidadBolsas, producto.PesoKg);
            }

            gvProductos.DataSource = dt;
            gvProductos.DataBind();
        }

        protected void HabilitarEdicion(object sender, EventArgs e)
        {
            // Habilitar la edición de los campos
            txtNumFactura.ReadOnly = false;
            txtFechaEmision.ReadOnly = false;

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
                string numeroCPIC = txtNumCPIC.Text;
                string numeroFactura = txtNumFactura.Text;
                DateTime fechaEmision;

                if (!DateTime.TryParse(txtFechaEmision.Text, out fechaEmision))
                {
                    MostrarMensaje("Formato de fecha inválido.", "danger");
                    return;
                }

                // Actualizar el CPIC en la base de datos
                bool actualizado = ActualizarCPIC(numeroCPIC, numeroFactura, fechaEmision);

                if (actualizado)
                {
                    // Volver al modo de sólo lectura
                    txtNumFactura.ReadOnly = true;
                    txtFechaEmision.ReadOnly = true;
                    btnHabilitarEdicion.Visible = true;
                    btnGuardarCambios.Visible = false;

                    MostrarMensaje("CPIC actualizado correctamente.", "success");
                }
                else
                {
                    MostrarMensaje("No se pudo actualizar el CPIC.", "danger");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar los cambios: " + ex.Message, "danger");
            }
        }

        private bool ActualizarCPIC(string numeroCPIC, string numeroFactura, DateTime fechaEmision)
        {
            bool actualizado = false;

            try
            {
                // Cadena de conexión - Ajustar según tu configuración
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();

                    // 1. Primero verifica si la factura existe o necesitas crearla
                    int idFactura = 0;

                    // Buscar factura por número
                    string queryBuscarFactura = "SELECT idFactura FROM Factura WHERE numeroFactura = @numeroFactura";
                    using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(queryBuscarFactura, connection))
                    {
                        command.Parameters.AddWithValue("@numeroFactura", numeroFactura);
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            idFactura = Convert.ToInt32(result);
                        }
                        else
                        {
                            // Si la factura no existe, busca los detalles para crearla
                            // (En un sistema real, deberías tener una interfaz para agregar la factura)
                            // Aquí solo vamos a actualizar la referencia a la factura si ya existe
                        }
                    }

                    // 2. Actualizar el CPIC
                    string queryActualizarCPIC = "UPDATE CPIC SET fechaEmision = @fechaEmision";

                    // Solo actualizar la factura si se encontró una factura válida
                    if (idFactura > 0)
                    {
                        queryActualizarCPIC += ", idFactura = @idFactura";
                    }

                    queryActualizarCPIC += " WHERE numeroCPIC = @numeroCPIC";

                    using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(queryActualizarCPIC, connection))
                    {
                        command.Parameters.AddWithValue("@fechaEmision", fechaEmision);
                        command.Parameters.AddWithValue("@numeroCPIC", numeroCPIC);

                        if (idFactura > 0)
                        {
                            command.Parameters.AddWithValue("@idFactura", idFactura);
                        }

                        int rowsAffected = command.ExecuteNonQuery();
                        actualizado = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Registrar el error
                throw new Exception("Error al actualizar el CPIC: " + ex.Message);
            }

            return actualizado;
        }

        protected void Cancelar(object sender, EventArgs e)
        {
            // Limpiar los campos y ocultar paneles de resultados
            txtBuscarCPIC.Text = "";
            pnlResultados.Visible = false;
            pnlNoResultados.Visible = false;
            lblMensaje.Text = "";
        }

        protected void gvProductos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProductos.EditIndex = e.NewEditIndex;

            // Recargar los datos en el GridView
            CPIC_Corregido cpic = ObtenerCPIC(txtNumCPIC.Text);
            if (cpic != null)
            {
                MostrarDatosCPIC(cpic);

                // Cargar los productos en el DropDownList al entrar en modo edición
                DropDownList ddlProductos = (DropDownList)gvProductos.Rows[e.NewEditIndex].FindControl("ddlProductos");
                if (ddlProductos != null)
                {
                    CargarProductos(ddlProductos);
                    // Seleccionar el producto actual
                    DataRowView dr = gvProductos.Rows[e.NewEditIndex].DataItem as DataRowView;
                    if (dr != null)
                    {
                        ddlProductos.SelectedValue = dr["IdProducto"].ToString();
                    }
                }
            }
        }

        private void CargarProductos(DropDownList ddl)
        {
            // Cargar los productos desde la base de datos utilizando ADO.NET
            DataTable dt = new DataTable();
            dt.Columns.Add("IdProducto", typeof(int));
            dt.Columns.Add("Nombre", typeof(string));

            try
            {
                // Cadena de conexión - Ajustar según tu configuración
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT idProducto, nombre FROM Producto ORDER BY nombre";

                    using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection))
                    {
                        using (System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dt.Rows.Add(
                                    Convert.ToInt32(reader["idProducto"]),
                                    reader["nombre"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar error de conexión o consulta
                MostrarMensaje("Error al cargar los productos: " + ex.Message, "danger");

                // Agregar algunos productos por defecto en caso de error
                dt.Rows.Add(1, "Cemento");
                dt.Rows.Add(2, "Arena");
                dt.Rows.Add(3, "Piedra");
            }

            ddl.DataSource = dt;
            ddl.DataBind();
        }

        protected void gvProductos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProductos.EditIndex = -1;

            // Recargar los datos en el GridView
            CPIC_Corregido cpic = ObtenerCPIC(txtNumCPIC.Text);
            if (cpic != null)
            {
                MostrarDatosCPIC(cpic);
            }
        }

        protected void gvProductos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // Obtener los controles de la fila en edición
                GridViewRow row = gvProductos.Rows[e.RowIndex];
                int id = Convert.ToInt32(gvProductos.DataKeys[e.RowIndex].Value);

                DropDownList ddlProductos = (DropDownList)row.FindControl("ddlProductos");
                TextBox txtCantidad = (TextBox)row.FindControl("txtCantidad");
                TextBox txtPeso = (TextBox)row.FindControl("txtPeso");

                int idProducto = Convert.ToInt32(ddlProductos.SelectedValue);
                string nombreProducto = ddlProductos.SelectedItem.Text;
                int cantidad = Convert.ToInt32(txtCantidad.Text);
                double peso = Convert.ToDouble(txtPeso.Text);

                // Validar datos
                if (idProducto <= 0 || cantidad <= 0 || peso <= 0)
                {
                    MostrarMensaje("Por favor, complete todos los campos correctamente.", "danger");
                    return;
                }

                // Actualizar el producto en la base de datos
                bool actualizado = ActualizarProductoCPIC(id, idProducto, nombreProducto, cantidad, peso);

                if (actualizado)
                {
                    gvProductos.EditIndex = -1;

                    // Recargar los datos en el GridView
                    CPIC_Corregido cpic = ObtenerCPIC(txtNumCPIC.Text);
                    if (cpic != null)
                    {
                        MostrarDatosCPIC(cpic);
                        MostrarMensaje("Producto actualizado correctamente.", "success");
                    }
                }
                else
                {
                    MostrarMensaje("No se pudo actualizar el producto.", "danger");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al actualizar el producto: " + ex.Message, "danger");
            }
        }

        private bool ActualizarProductoCPIC(int id, int idProducto, string nombreProducto, int cantidad, double peso)
        {
            bool actualizado = false;

            try
            {
                // Obtener primero el idCPIC basado en la fila actual
                CPIC_Corregido cpic = ObtenerCPIC(txtNumCPIC.Text);
                if (cpic == null)
                {
                    return false;
                }

                int idCPIC = cpic.IdCPIC;
                ProductoCPIC_Corregido productoOriginal = cpic.Productos.FirstOrDefault(p => p.ID == id);

                // Cadena de conexión - Ajustar según tu configuración
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSGV"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();

                    // La tabla CPIC_Productos no tiene un ID específico, por lo que debemos usar idCPIC e idProducto para identificar la fila
                    // Primero verificamos si el registro existe para decidir si actualizar o insertar

                    string queryComprobar = "SELECT COUNT(*) FROM CPIC_Productos WHERE idCPIC = @idCPIC AND idProducto = @idProductoOriginal";
                    int existeRegistro = 0;
                    int idProductoOriginal = productoOriginal != null ? productoOriginal.IdProducto : idProducto;

                    using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(queryComprobar, connection))
                    {
                        command.Parameters.AddWithValue("@idCPIC", idCPIC);
                        command.Parameters.AddWithValue("@idProductoOriginal", idProductoOriginal);
                        existeRegistro = (int)command.ExecuteScalar();
                    }

                    // Si estamos cambiando el producto, necesitamos eliminar el registro anterior y crear uno nuevo
                    if (existeRegistro > 0 && idProductoOriginal != idProducto)
                    {
                        // Eliminar el registro anterior
                        string queryEliminar = "DELETE FROM CPIC_Productos WHERE idCPIC = @idCPIC AND idProducto = @idProductoOriginal";
                        using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(queryEliminar, connection))
                        {
                            command.Parameters.AddWithValue("@idCPIC", idCPIC);
                            command.Parameters.AddWithValue("@idProductoOriginal", idProductoOriginal);
                            command.ExecuteNonQuery();
                        }

                        existeRegistro = 0; // Ahora no existe, lo vamos a crear
                    }

                    // Actualizar o insertar según corresponda
                    if (existeRegistro > 0)
                    {
                        // Actualizar
                        string queryActualizar = @"UPDATE CPIC_Productos 
                                                SET cantidadBolsasProducto = @cantidad, 
                                                    pesoKg = @peso 
                                                WHERE idCPIC = @idCPIC AND idProducto = @idProducto";

                        using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(queryActualizar, connection))
                        {
                            command.Parameters.AddWithValue("@idCPIC", idCPIC);
                            command.Parameters.AddWithValue("@idProducto", idProducto);
                            command.Parameters.AddWithValue("@cantidad", cantidad);
                            command.Parameters.AddWithValue("@peso", peso);

                            int rowsAffected = command.ExecuteNonQuery();
                            actualizado = rowsAffected > 0;
                        }
                    }
                    else
                    {
                        // Insertar
                        string queryInsertar = @"INSERT INTO CPIC_Productos 
                                            (idCPIC, idProducto, cantidadBolsasProducto, pesoKg) 
                                            VALUES (@idCPIC, @idProducto, @cantidad, @peso)";

                        using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(queryInsertar, connection))
                        {
                            command.Parameters.AddWithValue("@idCPIC", idCPIC);
                            command.Parameters.AddWithValue("@idProducto", idProducto);
                            command.Parameters.AddWithValue("@cantidad", cantidad);
                            command.Parameters.AddWithValue("@peso", peso);

                            int rowsAffected = command.ExecuteNonQuery();
                            actualizado = rowsAffected > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Registrar el error
                throw new Exception("Error al actualizar el producto: " + ex.Message);
            }

            return actualizado;
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = $"text-{tipo}";
        }
    }

    // Renombrando la clase para evitar conflictos con la tabla CPIC
    public class CPIC_Corregido
    {
        public int IdCPIC { get; set; }
        public string NumeroCPIC { get; set; }
        public int IdFactura { get; set; }
        public string NumeroFactura { get; set; } // Propiedad para visualización
        public DateTime FechaEmision { get; set; }
        public decimal ValorTotalFlete { get; set; }
        public List<ProductoCPIC_Corregido> Productos { get; set; }
    }

    public class ProductoCPIC_Corregido
    {
        public int ID { get; set; }
        public int IdCPIC { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadBolsas { get; set; }
        public decimal PesoKg { get; set; }
    }
}