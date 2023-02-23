namespace FusionCore.CadastroUsuario
{
    public class GridUsuarioDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }

        public override string ToString()
        {
            return $"{Login}";
        }
    }
}