using CommonTools.DTOs.Query;

namespace Migracion.Talento.CoreWebApi.Interfaces
{
    public interface IDocumentsEvents
    {
        public Task<ResponseDto> GetAllDocsBySecretCode(string secretCode);
    }

}
