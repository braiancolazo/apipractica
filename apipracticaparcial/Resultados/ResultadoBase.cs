using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Net;

namespace apipracticaparcial.Resultados
{
    public class ResultadoBase
    {
        public bool OK { get; set; } = true;
        public string MensajeError { get; set; } = "";
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public void SetMensajeError(string MensajeError2,HttpStatusCode Statuscode2)
        {
            OK = true;
            MensajeError = MensajeError2;
            StatusCode = Statuscode2;

        }




    }
}
