namespace FusionCore.FusionPdv.Flags
{
    public enum TipoTransacao
    {
        CartaoDeCreditoAvista = 10,
        CartaoDeCreditoParceladoPeloEstabelecimento = 11,
        CartaoDeCreditoParceladoPeloEmissor = 12,
        CartaoDeDebitoAvista = 20,
        CartaoDeDebitoParceladoPeloEstabelecimento = 22,
        CartaoDeDebitoPreDatado = 21,
        CartaoDeDebitoPreDatadoForcado = 24,
        CdcDebitoParceladoPeloEmissor = 40,
        VoucherOuPat = 60,
        OutroTipoCartao = 30,
        NaoDefinido = 99,

        // Operações administrativas
        PreAutorizacaoComCartaoDeCredito = 13,
        ConsultaCdcDebitoParceladoPeloEmissor = 41,
        ConsultaDeCheque = 70,
        GarantiaDeCheque = 71,
        FechamentoFinalizacao = 01,
        OutraOperacaoAdministrativa = 00

    }
}