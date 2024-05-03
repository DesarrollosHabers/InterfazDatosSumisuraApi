using InterfazDatosSumisuraApi.Models.UsuariosDTO;
using InterfazDatosSumisuraApi.Models;
using InterfazDatosSumisuraApi.ServiceDB;
using InterfazDatosSumisuraApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InterfazDatosSumisuraApi.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IRepositorioUsuarios _repositorioUsuarios;
        private readonly ITools _tools;
        private ApiResponse _ApiResponse = new();
        public UsuariosController(IRepositorioUsuarios repositorioUsuarios, ITools tools)
        {
            this._repositorioUsuarios = repositorioUsuarios;
            this._tools = tools;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            if ((loginRequestDTO.UserName == null && loginRequestDTO.Password == null) || (loginRequestDTO.UserName == null || loginRequestDTO.Password == null))
            {
                return BadRequest();
            }

            loginRequestDTO.Password = _tools.ToolsSha256(loginRequestDTO.Password);

            var loginresponse = await _repositorioUsuarios.Login(loginRequestDTO);
            if (loginresponse.Usuario == null || string.IsNullOrEmpty(loginresponse.Token))
            {
                _ApiResponse.StatusCode = HttpStatusCode.BadRequest;
                _ApiResponse.IsSuccess = false;
                _ApiResponse.IsError.Add("UserName o Password son incorrectos.");
                return BadRequest(_ApiResponse);
            }

            _ApiResponse.IsSuccess = true;
            _ApiResponse.StatusCode = HttpStatusCode.OK;
            _ApiResponse.Resultado = loginresponse;

            return Ok(_ApiResponse);
        }
    }
}
