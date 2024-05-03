using Dapper;
using InterfazDatosSumisuraApi.Models.UsuariosDTO;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InterfazDatosSumisuraApi.ServiceDB
{
    public interface IRepositorioUsuarios
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    }
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly string ConnectionStringUsuarios;
        private readonly string ScretKey;
        public RepositorioUsuarios(IConfiguration configuration)
        {
            ConnectionStringUsuarios = configuration.GetConnectionString("ConnectionUsers");
            ScretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            using var Connection = new SqlConnection(ConnectionStringUsuarios);
            var Usuario = await Connection.QueryFirstOrDefaultAsync<Usuario>(@"SELECT [id]
                                                                                      ,[UserName]
                                                                                      ,[Nombre]
                                                                                      ,[Rol]
                                                                                 FROM [dbHaberHoldingCore].[dbo].[AspNetUsersAPI]
                                                                                WHERE [UserName] = @UserName AND [Password] = @Password", loginRequestDTO);

            if (Usuario == null)
            {
                return new LoginResponseDTO
                {
                    Token = "",
                    Usuario = null
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ScretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Usuario.Id.ToString()),
                    new Claim(ClaimTypes.Role, Usuario.Rol),
                }),
                NotBefore = DateTime.UtcNow.ToLocalTime(),
                Expires = DateTime.UtcNow.ToLocalTime().AddMinutes(10),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                Usuario = Usuario
            };
            return loginResponseDTO;
        }
    }
}
