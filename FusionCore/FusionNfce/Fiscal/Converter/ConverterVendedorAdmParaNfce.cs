using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionNfce.Vendedores;
using FusionCore.Helpers.Pessoa;

namespace FusionCore.FusionNfce.Fiscal.Converter
{
    public class ConverterVendedorAdmParaNfce
    {
        private readonly PessoaEntidade _pessoaEntidade;

        public ConverterVendedorAdmParaNfce(PessoaEntidade pessoaEntidade)
        {
            _pessoaEntidade = pessoaEntidade;
        }

        public VendedorNfce Converter()
        {
            return new VendedorNfce
            {
                DocumentoUnico = _pessoaEntidade.GetDocumentoUnico(),
                Ativo = _pessoaEntidade.Ativo,
                Id = _pessoaEntidade.Id,
                Nome = _pessoaEntidade.Nome
            };
        }
    }
}