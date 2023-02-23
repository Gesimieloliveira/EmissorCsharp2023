using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum CteModeloDocumento
    {
        [Modelo("01")]
        [Description("Nota Fiscal")]
        NF = 1,

        [Modelo("1B")]
        [Description("Nota Fiscal Avulsa")]
        UmB = 2,

        [Modelo("02")]
        [Description("Nota Fiscal de Venda a Consumidor")]
        ZeroDois = 3,

        [Modelo("2D")]
        [Description("Cupom Fiscal emitido por ECF")]
        DoisD = 4,

        [Modelo("2E")]
        [Description("Bilhete de Passagem emitido por ECF")]
        DoisE = 5,

        [Modelo("04")]
        [Description("Nota Fiscal de Produtor")]
        ZeroQuato = 6,

        [Modelo("06")]
        [Description("Nota Fiscal/Conta de Energia Elétrica")]
        ZeroSeis = 7,

        [Modelo("07")]
        [Description("Nota Fiscal de Serviço de Transporte")]
        ZeroSete = 8,

        [Modelo("08")]
        [Description("Conhecimento de Transporte Rodoviário de Cargas")]
        ZeroOito = 9,

        [Modelo("8B")]
        [Description("Conhecimento de Transporte de Cargas Avulso")]
        OitoB = 10,

        [Modelo("09")]
        [Description("Conhecimento de Transporte Aquaviário de Cargas")]
        ZeroNove = 11,

        [Modelo("10")]
        [Description("Conhecimento Aéreo")]
        Dez = 12,

        [Modelo("11")]
        [Description("Conhecimento de Transporte Ferroviário de Cargas")]
        Onze = 13,

        [Modelo("13")]
        [Description("Bilhete de Passagem Rodoviário")]
        Treze = 14,

        [Modelo("14")]
        [Description("Bilhete de Passagem Aquaviário")]
        Quatorze = 15,

        [Modelo("15")]
        [Description("Bilhete de Passagem e Nota de Bagagem")]
        Quinze = 16,

        [Modelo("16")]
        [Description("Bilhete de Passagem Ferroviário")]
        Dezesseis = 17,

        [Modelo("17")]
        [Description("Despacho de Transporte")]
        Dezessete = 18,

        [Modelo("18")]
        [Description("Resumo de Movimento Diário")]
        Dezoito = 19,

        [Modelo("20")]
        [Description("Ordem de Coleta de Cargas")]
        Vinte = 20,

        [Modelo("21")]
        [Description("Nota Fiscal de Serviço de Comunicação")]
        VinteUm = 21,

        [Modelo("22")]
        [Description("Nota Fiscal de Serviço de Telecomunicação")]
        VinteDois = 22,

        [Modelo("23")]
        [Description("GNRE")]
        VinteTres = 23,

        [Modelo("24")]
        [Description("Autorização de Carregamento e Transporte")]
        VinteQuatro = 24,

        [Modelo("25")]
        [Description("Manifesto de Carga")]
        VinteCinco = 25,

        [Modelo("26")]
        [Description("Conhecimento de Transporte Multimodal de Cargas")]
        VinteSeis = 26,

        [Modelo("27")]
        [Description("Nota Fiscal de Transporte Ferroviário de Cargas")]
        VinteSete = 27,

        [Modelo("28")]
        [Description("Nota Fiscal/Conta de Fornecimento de Gás Canalizado")]
        VinteOito = 28,

        [Modelo("29")]
        [Description("Nota Fiscal/Conta de Fornecimento de Água Canalizado")]
        VinteNove = 29,

        [Modelo("30")]
        [Description("Bilhete/Recibo do Passageiro")]
        Trinta = 30,

        [Modelo("55")]
        [Description("Nota Fiscal Eletrônica")]
        CinquentaCinco = 31,

        [Modelo("57")]
        [Description("Conhecimento de Transporte Eletrônico - CTe")]
        CinquentaSete = 32,
    }
}