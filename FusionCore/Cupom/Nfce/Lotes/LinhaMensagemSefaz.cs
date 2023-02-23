namespace FusionCore.Cupom.Nfce.Lotes
{
    public class LinhaMensagemSefaz
    {
        public bool Autorizado { get; }
        public string Motivo { get; }
        public string Chave { get; }

        public LinhaMensagemSefaz(bool autorizado, string motivo, string chave)
        {
            Autorizado = autorizado;
            Motivo = motivo;
            Chave = chave;
        }

        public override string ToString()
        {
            return $"{Chave} - {Motivo}";
        }
    }
}