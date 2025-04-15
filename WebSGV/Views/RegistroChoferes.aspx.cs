using System;
using System.Web.Script.Services;
using System.Web.Services;
using WebSGV.Models;

namespace WebSGV.Views
{
    [ScriptService]
    public partial class RegistroChoferes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        [WebMethod]
        public static DniResponse BuscarPorDNI(string numero)
        {
            try
            {
                return DniService.ConsultarPorDNI(numero);
            }
            catch (Exception ex)
            {
                return new DniResponse
                {
                    error = true,
                    message = "Error interno: " + ex.Message
                };
            }
        }
    }
}
