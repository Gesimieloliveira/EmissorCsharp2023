namespace FusionCore.FusionAdm.Fiscal.Contratos.Componentes
{
    public interface IEnderecoFiscal
    {
        string Cep { get; set; }
        string Logradouro { get; set; }
        string Numero { get; set; }
        string Complemento { get; set; }
        string Bairro { get; set; }
        string Telefone { get; set; }
        ILocalizacaoFiscal Localizacao { get; set; }
    }
}