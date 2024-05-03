using InterfazDatosSumisuraApi.Models.Habers;
using InterfazDatosSumisuraApi.Models;
using InterfazDatosSumisuraApi.ServiceDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InterfazDatosSumisuraApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TailoredController : ControllerBase
    {
        private readonly IRepositorioHabers _repositorioHabers;
        private ApiResponse _ApiResponse = new();

        public TailoredController(IRepositorioHabers repositorioHabers)
        {
            this._repositorioHabers = repositorioHabers;
        }

        [HttpPost]
        [Route("PostDataCrossReference")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostDataCrossReference([FromBody] string[] CrossReference)
        {
            try
            {
                if (CrossReference is null)
                {
                    _ApiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _ApiResponse.IsSuccess = false;
                    _ApiResponse.IsError.Add("Ingresa una Referencia Cruzada");
                    return BadRequest(_ApiResponse);
                }

                List<string[]> subarreglos = DividirArreglo(CrossReference);
                List<ResponseGetDataByCrossReference> modelos = new List<ResponseGetDataByCrossReference>();
                foreach (var item in subarreglos)
                {
                    var data = await _repositorioHabers.GetDataByCrossReference(item);
                    modelos.AddRange(data);
                }

                if (modelos == null)
                {
                    _ApiResponse.StatusCode = HttpStatusCode.NotFound;
                    _ApiResponse.IsSuccess = false;
                    _ApiResponse.IsError.Add("Error al generar la consulta a la Base de Datos");
                    return NotFound(_ApiResponse);
                }

                _ApiResponse.StatusCode = HttpStatusCode.OK;
                _ApiResponse.IsSuccess = true;
                _ApiResponse.Resultado = modelos;

                return Ok(_ApiResponse);
            }
            catch (Exception ex)
            {
                _ApiResponse.StatusCode = HttpStatusCode.BadRequest;
                _ApiResponse.IsSuccess = false;
                _ApiResponse.IsError.Add($"Mensaje Error: {ex.Message}; StackTrace: {ex.StackTrace}");
                return BadRequest(_ApiResponse);
            }

        }
        //
        public static List<string[]> DividirArreglo(string[] arreglo, int tamañoSubarreglo = 25)
        {
            List<string[]> subarreglos = new List<string[]>();

            for (int i = 0; i < arreglo.Length; i += tamañoSubarreglo)
            {
                string[] subarreglo = new string[Math.Min(tamañoSubarreglo, arreglo.Length - i)];
                Array.Copy(arreglo, i, subarreglo, 0, subarreglo.Length);
                subarreglos.Add(subarreglo);
            }

            return subarreglos;
        }
    }
}
