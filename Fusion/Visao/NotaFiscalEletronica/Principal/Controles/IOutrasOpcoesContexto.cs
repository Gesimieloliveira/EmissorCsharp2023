namespace Fusion.Visao.NotaFiscalEletronica.Principal.Controles
{
    public interface IOutrasOpcoesContexto
    {
        bool AutoAjustarImposto { get; set; }
        bool AutoCalcularTotaisItem { get; }
        bool GeraIcmsInterstadual { get; set; }
        bool MovimentarEstoqueConfiguracao { get; }
        bool EnviarInformacoesCreditoNaObsItem { get; set; }
        bool NaoEditar { get; set; }
        bool UsarIpiTagPropria { get; }
    }
}