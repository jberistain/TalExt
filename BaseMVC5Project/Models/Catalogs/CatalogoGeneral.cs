using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionTalentoExtranjero.Models.Catalogs
{
    public class CatalogoGeneral
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string AtributoAdicionalStr1 { get; set; }
        public string AtributoAdicionalStr2 { get; set; }
        public string AtributoAdicionalStr3 { get; set; }
        public string AtributoAdicionalStr4 { get; set; }
        public string AtributoAdicionalStr5 { get; set; }
        public string AtributoAdicionalStr6 { get; set; }
        public string AtributoAdicionalStr7 { get; set; }
        public string AtributoAdicionalStr8 { get; set; }
        public string AtributoAdicionalStr9 { get; set; }
        public int AtributoAdicionalInt1 { get; set; }
        
        public string AtributoAdicional1Dte { get; set; }
        public string AtributoAdicional2Dte { get; set; }

        public bool Activo { get; set; }
        

        public string NombreCatalogo { get; set; }
        public CatalogoGeneral() { }


        public string BuscaDescripcionEnLista(int id, List<CatalogoGeneral> data)
        {
            string result = "";
            foreach (var item in data)
            {
                if(item.Id == id)
                    result = item.Descripcion;
            }
            return result;
        }
    }
}
