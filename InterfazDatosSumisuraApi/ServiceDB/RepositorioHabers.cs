using Dapper;
using InterfazDatosSumisuraApi.Models.Habers;
using Microsoft.Data.SqlClient;

namespace InterfazDatosSumisuraApi.ServiceDB
{
    public interface IRepositorioHabers
    {
        Task<IEnumerable<ResponseGetDataByCrossReference>> GetDataByCrossReference(string[] CrossReference);
    }

    public class RepositorioHabers : IRepositorioHabers
    {
        private readonly string ConnectionStringHabers;
        public RepositorioHabers(IConfiguration configuration)
        {
            ConnectionStringHabers = configuration.GetConnectionString("ConnectionHabers");
        }

        public async Task<IEnumerable<ResponseGetDataByCrossReference>> GetDataByCrossReference(string[] CrossReference)
        {
            string skusConcatenados = string.Join(",", CrossReference);
            using var connection = new SqlConnection(ConnectionStringHabers);
            return await connection.QueryAsync<ResponseGetDataByCrossReference>(@"SELECT * FROM [VW_Tailored] WHERE [CrossReference] IN @CrossReference", new { CrossReference = skusConcatenados.Split(',') });
        }
    }
}
