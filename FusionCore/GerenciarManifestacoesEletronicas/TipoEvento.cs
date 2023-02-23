namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public enum TipoEvento
    {
        CartaCorrecao = 110110,
        Epec = 110140,
        Cancelamento = 110111,
        CancelamentoSubstituicao = 110112,
        ConfirmacaoOperacao = 210200,
        CienciaOperacao = 210210,
        DesconhecimentoOperacao = 210220,
        OperacaoNaoRealizada = 210240,
        AutorizadoCteParaNfe = 610600,
        MdfeAutorizadoComCte = 610614
    }
}