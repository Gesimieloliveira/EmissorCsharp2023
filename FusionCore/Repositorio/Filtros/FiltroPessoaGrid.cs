namespace FusionCore.Repositorio.Filtros
{
    public class FiltroPessoaGrid
    {
        public int? Codigo { get; set; }
        public string NomePessoaContenha { get; set; }
        public string NomeFantasiaPessoaContenha { get; set; }
        public string DocumentoUnicoIgualA { get; set; }
        public string EmailPessoaContenha { get; set; }
        public string TelefoneIgualA { get; set; }
        public bool Ativos { get; set; } = true;
    }
}