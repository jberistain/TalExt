namespace MigracionTalentoExtranjero.Models.Utils
{
    public class ResponseModel
    {
        public dynamic result { get; set; }
        public bool response { get; set; }
        public string message { get; set; }
        public string href { get; set; }
        public string function { get; set; }
        public string idUtilizado { get; set; }

        public ResponseModel()
        {
            this.response = false;
            this.message = "Ocurrio un error inesperado";
        }

        public void SetResponse(bool r, string m = "")
        {
            this.response = r;
            this.message = m;

            if (!r && m == "") this.message = "Ocurrio un error inesperado";
        }

        /// <summary>
        /// Coloca el response en "true" y el mensaje "Redireccionando"
        /// </summary>
        public void SetDefaultSuccessConfiguration()
        {
            this.response = true;
            this.message = "Redireccionando";
        }
    }
}
