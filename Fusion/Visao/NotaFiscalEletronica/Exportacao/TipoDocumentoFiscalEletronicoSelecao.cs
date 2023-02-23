using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace Fusion.Visao.NotaFiscalEletronica.Exportacao
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoDocumentoFiscalEletronicoSelecao
    {
        [Description("[NF-e] Nota Fiscal Eletrônica")]
        NFe = 55,
        [Description("[NFC-e] Nota Fiscal de Consumidor Eletrônica")]
        NFCe = 65,
        [Description("[CT-e] Conhecimento de Transporte Eletrônico")]
        CTe = 57,
        [Description("[CT-e OS] Conhecimento de Transporte Eletrônico Outros Servicos")]
        CTeOs = 67,
        [Description("[MDF-e] Manifesto Eletrônico de Documentos")]
        MDFe = 58,
        [Description("[CF-e] SAT Fiscal ou Módulo Fiscal")]
        SAT = 59
    }
}