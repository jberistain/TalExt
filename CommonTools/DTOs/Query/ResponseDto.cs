using CommonTools.Enums;
using System;

namespace CommonTools.DTOs.Query
{
    public class ResponseDto : ResponseDtoExtens
    {


        public ResponseDto(ResponseDtoEnum status) 
        {
            switch (status)
            {
                case ResponseDtoEnum.Success:
                    code = 200;
                    message = "Resultado exitoso";
                    error = false;
                    break;
                case ResponseDtoEnum.Unknow:
                    code = 300;
                    message = "Ocurrio un error al procesar la solicitud";
                    error= true;
                    break;
                case ResponseDtoEnum.Error:
                    code = 500;
                    message= "Ocurrio un error al procesar la solicitud";
                    error = true;
                    break;
                case ResponseDtoEnum.NoData:
                    code = 400;
                    message = "No hay registros";
                    error = true;
                    break;
                case ResponseDtoEnum.Duplicated:
                    code = 401;
                    message = "Ya existe un registro con esa información";
                    error = true;
                    break;
                case ResponseDtoEnum.RegisterWithDependency:
                    code = 406;
                    message = "Este registro tiene dependencia con otra tabla, primero elimina todos los elementos que hacen referencia a este registro";
                    error = true;
                    break;


            }



            if (status == ResponseDtoEnum.Success)
            {
                
            }

        }
        public ResponseDto() { }
        public int code { get; set; }
        public string message { get; set; }
        public bool error { get; set; }
        public dynamic response { get; set; }
    }


    public class ResponseDtoExtens 
    {
        public dynamic secondResponse { get; set; }
    }
}
