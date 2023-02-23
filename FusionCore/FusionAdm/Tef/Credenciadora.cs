using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Tef
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum Credenciadora
    {
        [Description("Administradora de Cartões Sicredi Ltda.")
         , CnpjCredenciadora("03106213000190", "001")]
        AdmCartaoSicredi = 1,

        [Description("Administradora de Cartões Sicredi Ltda.(filial RS)")
         , CnpjCredenciadora("03106213000271", "002")]
        AdmCartaoSicrediFilial = 2,

        [Description("Banco American Express S/A - AMEX")
         , CnpjCredenciadora("60419645000195", "003")]
        Amex = 3,

        [Description("BANCO GE - CAPITAL")
         , CnpjCredenciadora("62421979000129", "004")]
        BancoGeCapital = 4,

        [Description("BANCO SAFRA S/A")
         , CnpjCredenciadora("58160789000128", "005")]
        BancoSafraSa = 5,

        [Description("BANCO TOPÁZIO S/A")
         , CnpjCredenciadora("07679404000100", "006")]
        BancoTopazioSa = 6,

        [Description("BANCO TRIANGULO S/A")
         , CnpjCredenciadora("17351180000159", "007")]
        BancoTrianguloSa = 7,

        [Description("BIGCARD Adm. de Convenios e Serv.")
         , CnpjCredenciadora("04627085000193", "008")]
        BirgCardConvSeguros = 8,

        [Description("BOURBON Adm. de Cartões de Crédito")
         , CnpjCredenciadora("01418852000166", "009")]
        BouronAdmCartCredito = 9,

        [Description("CABAL Brasil Ltda.")
         , CnpjCredenciadora("03766873000106", "010")]
        CabalBrasil = 10,
        
        [Description("CETELEM Brasil S/A - CFI")
         , CnpjCredenciadora("03722919000187", "011")]
        CetelemBrasilSa = 11,

        [Description("CIELO S/A")
         , CnpjCredenciadora("01027058000191", "012")]
        CieloSa = 12,

        [Description("CREDI 21 Participações Ltda.")
         , CnpjCredenciadora("03529067000106", "013")]
        Credi21Participacoes = 13,

        [Description("ECX CARD Adm. e Processadora de Cartões S/A")
            , CnpjCredenciadora("71225700000122", "014")]
        EcxCardAdm = 14,

        [Description("Empresa Bras. Tec. Adm. Conv. Hom. Ltda. - EMBRATEC")
         , CnpjCredenciadora("03506307000157", "015")]
        EmpresaBras = 15,

        [Description("EMPÓRIO CARD LTDA")
         , CnpjCredenciadora("04432048000120", "016")]
        EmporioCard = 16,

        [Description("FREEDDOM e Tecnologia e Serviços S/A")
         , CnpjCredenciadora("07953674000150", "017")]
        Freeddom = 17,

        [Description("FUNCIONAL CARD LTDA")
         , CnpjCredenciadora("03322366000175", "018")]
        FuncionalCard = 18,

        [Description("HIPERCARD Banco Multiplo S/A")
         , CnpjCredenciadora("03012230000169", "019")]
        HipercardBanco = 19,

        [Description("MAPA Admin. Conv. e Cartões Ltda.")
         , CnpjCredenciadora("03966317000175", "020")]
        MapaAdmin = 20,

        [Description("Novo Pag Adm. e Proc. de Meios Eletrônicos de Pagto.Ltda")
         , CnpjCredenciadora("00163051000134", "021")]
        NovoPagAdm = 21,

        [Description("PERNAMBUCANAS Financiadora S/A Crédito, Fin. e Invest")
         , CnpjCredenciadora("43180355000112", "022")]
        PernambucanasFinanciadora = 22,

        [Description("POLICARD Systems e Serviços Ltda.")
         , CnpjCredenciadora("00904951000195", "023")]
        PoicardSystems = 23,

        [Description("PROVAR Negócios de Varejo Ltda.")
         , CnpjCredenciadora("33098658000137", "024")]
        ProvarNegocios = 24,

        [Description("REDECARD S/A")
         , CnpjCredenciadora("01425787000104", "025")]
        RedecardSa = 25,

        [Description("RENNER Adm. Cartões de Crédito Ltda.")
         , CnpjCredenciadora("90055609000150", "026")]
        RennerAdm = 26,

        [Description("RP Administração de Convênios Ltda.")
         , CnpjCredenciadora("03007699000100", "027")]
        RpAdmConvenios = 27,

        [Description("SANTINVEST S/A Crédito, Financiamento e Investimentos")
         , CnpjCredenciadora("00122327000136", "028")]
        SantinvestSa = 28,

        [Description("SODEXHO Pass do Brasil Serviços e Comércio S/A")
         , CnpjCredenciadora("69034668000156", "029")]
        Sodexho = 29,

        [Description("SOROCRED Meios de Pagamentos Ltda.")
         , CnpjCredenciadora("60114865000100", "030")]
        SorocredMeiosPagamentos = 30,

        [Description("Tecnologia Bancária S/A - TECBAN")
         , CnpjCredenciadora("51427102000471", "031")]
        TecnologiaBancariaSa = 31,

        [Description("TICKET Serviços S/A")
         , CnpjCredenciadora("47866934000174", "032")]
        TicketServicosSa = 32,

        [Description("TRIVALE Administração Ltda.")
         , CnpjCredenciadora("00604122000197", "033")]
        TrivaleAdmLtda = 33,

        [Description("Unicard Banco Múltiplo S/A - TRICARD")
         , CnpjCredenciadora("61.071.387/0001-61", "034")]
        UnicardBancoMultiplo = 34,

        [Description("Outros")
         , CnpjCredenciadora("", "999")]
        Outros = 999
    }
}