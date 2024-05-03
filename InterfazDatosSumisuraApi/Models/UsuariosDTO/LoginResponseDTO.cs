namespace InterfazDatosSumisuraApi.Models.UsuariosDTO
{
    public class LoginResponseDTO
    {
        public Usuario Usuario { get; set; }
        public string Token { get; set; }
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
    }

    public class LoginRequestDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
