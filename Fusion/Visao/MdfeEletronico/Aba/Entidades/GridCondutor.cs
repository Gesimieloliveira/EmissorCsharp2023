using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.Pessoas;

namespace Fusion.Visao.MdfeEletronico.Aba.Entidades
{
    public class GridCondutor
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public PessoaEntidade Condutor { get; set; }
        public MDFeCondutor MDFeCondutor { get; set; }
    }
}