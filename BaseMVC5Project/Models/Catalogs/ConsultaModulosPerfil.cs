using MigracionTalentoExtranjero.Models.Session;
using MigracionTalentoExtranjero.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionTalentoExtranjero.Models.Catalogs
{
    public class ConsultaModulosPerfil
    {
        public async Task<List<ModuloDto>> ObtenerModulosPorIdUsuario(int idUsuario)
        {
            List<ModuloDto> resultadoList = new List<ModuloDto>();
            try
            {

                if (idUsuario > 0)
                {
                    HttpManager httpManager = new HttpManager(Constants.WebAPIUrl);
                    CRUDManager crud = new CRUDManager(httpManager);
                    var consultaModulos = await crud.DescargaCatalogoModulosPorIdUsuario(idUsuario);

                    if (consultaModulos != null && consultaModulos.code == 200)
                    {
                        foreach (var currentProcess in consultaModulos.response)
                        {
                            resultadoList.Add(new ModuloDto()
                            {
                                ACTIVE = true,
                                CREATED_DATE = DateTime.Now,
                                ID_PROCESS = currentProcess.iD_PROCESS,
                                DESC_PROCESS_SP = currentProcess.desC_PROCESS_SP,
                                DESC_PROCESS_EN = currentProcess.desC_PROCESS_EN,
                                URL_PROCESS = currentProcess.urL_PROCESS,
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return resultadoList;

        }


        public static List<ModuloDto> ObtenerModulosDeSesion()
        {
            var Modulos = new List<ModuloDto>();

            string serializedObject = SessionManager.GetMenus();
            
            if(!string.IsNullOrEmpty(serializedObject))
                Modulos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModuloDto>>(serializedObject);
                

            return Modulos;
        }
    }
}
