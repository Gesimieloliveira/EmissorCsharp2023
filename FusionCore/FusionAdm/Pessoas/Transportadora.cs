using FusionCore.FusionAdm.CteEletronico.Flags;

namespace FusionCore.FusionAdm.Pessoas
{
    public class Transportadora : PessoaExtensao
    {
        public int Id { get; set; }
        public sealed override PessoaEntidade Pessoa { get; set; }
        public string Rntrc { get; set; }
        public TipoProprietario TipoProprietario { get; set; }
        public string Taf { get; set; }
        public string NumeroDoRegistroEstadual { get; set; }

        private Transportadora()
        {
            //nhibernate
        }

        public Transportadora(PessoaEntidade pessoa) : this()
        {
            Pessoa = pessoa;
            Rntrc = string.Empty;
            Taf = string.Empty;
            NumeroDoRegistroEstadual = string.Empty;
        }
    }
}