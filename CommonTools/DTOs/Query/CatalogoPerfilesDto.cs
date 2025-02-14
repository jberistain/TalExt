using CommonTools.DTOs.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class CatalogoPerfilesDto
    {
        public int IdRol { get; set; }
        public string Descripcion { get; set; }
        public List<ProcessCatalog> Modulos { get; set; }

        public string HtmlTablaModulos { get; set; }

        public int MODIFY_BY { get; set; }
        public int CREATED_BY { get; set; }

    }

    public class ProcessCatalog : ProcessDto
    {
        public bool moduloActivo { get; set; } = false;

    }
}
