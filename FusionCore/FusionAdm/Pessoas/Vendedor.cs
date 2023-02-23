namespace FusionCore.FusionAdm.Pessoas
{
    public class Vendedor : PessoaExtensao
    {
        public int Id { get; set; }

        private Vendedor()
        {
            //nhibernate
        }

        public Vendedor(PessoaEntidade pessoa) : this()
        {
            Pessoa = pessoa;
        }
    }
}