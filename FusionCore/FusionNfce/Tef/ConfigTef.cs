namespace FusionCore.FusionNfce.Tef
{
    public class ConfigTef
    {
        public int Id { get; set; } = 1;
        public bool IsAtivo { get; set; }
        public string ArquivoRequisicao { get; set; }
        public string ArquivoResposta { get; set; }
        public string ArquivoSts { get; set; }
        public string ArquivoTemporario { get; set; }
        public bool NaoEstaAtivo => !IsAtivo;
        public string RegistroCertificado { get; set; }
        public Operadora Operadora { get; set; } = Operadora.TefDialHomologacao;
    }
}