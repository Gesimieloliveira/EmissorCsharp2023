using System.ComponentModel;

namespace FusionCore.Papeis.Enums
{
    public enum PermissaoGrupo
    {
        [Description("Cadastros")]
        Cadastros,

        [Description("Financeiro")]
        Financeiro,

        [Description("Movimentações")]
        Movimentacoes,

        [Description("NF-e")]
        NFe,

        [Description("Transportes")]
        Transportes,

        [Description("Sintegra")]
        Sintegra,

        [Description("Relatório")]
        Relatorio,

        [Description("Pdv")]
        Pdv,

        [Description("Configurações")]
        Configuracoes,

        [Description("NFC-e")]
        NFCe
    }
}