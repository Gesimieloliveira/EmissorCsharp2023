using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum CategoriaComercialVeiculo
    {
        [Description("Veículo Comercial 2 eixos")]
        VeiculoComercial2Eixos = 2,

        [Description("Veículo Comercial 3 eixos")]
        VeiculoComercial3Eixos = 3,

        [Description("Veículo Comercial 4 eixos")]
        VeiculoComercial4Eixos = 4,

        [Description("Veículo Comercial 5 eixos")]
        VeiculoComercial5Eixos = 5,

        [Description("Veículo Comercial 6 eixos")]
        VeiculoComercial6Eixos = 6,

        [Description("Veículo Comercial 7 eixos")]
        VeiculoComercial7Eixos = 7,

        [Description("Veículo Comercial 8 eixos")]
        VeiculoComercial8Eixos = 8,

        [Description("Veículo Comercial 9 eixos")]
        VeiculoComercial9Eixos = 9,

        [Description("Veículo Comercial 10 eixos")]
        VeiculoComercial10Eixos = 10,

        [Description("Veículo Comercial Acima de 10 eixos")]
        VeiculoComercialAcimaDe10Eixos = 14
    }
}