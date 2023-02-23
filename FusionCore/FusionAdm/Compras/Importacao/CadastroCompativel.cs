using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Compras.Importacao
{
    public class CadastroCompativel
    {
        public EmpresaDTO Empresa { get; set; }
        public PessoaEntidade Fornecedor { get; set; }
        public PessoaEntidade Transportadora { get; set; }
    }
}