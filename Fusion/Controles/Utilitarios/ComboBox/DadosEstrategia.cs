using Fusion.Controles.Utilitarios.ComboBox.Dados;

namespace Fusion.Controles.Utilitarios.ComboBox
{
    internal enum DadosEstrategia
    {
        [Dados(typeof(DadosEmpresas))]
        Empresas,

        [Dados(typeof(DadosTiposDocumento))]
        TiposDeDocumento,

        [Dados(typeof(DadosTabelaPrecos))]
        TabelaPrecos
    }
}