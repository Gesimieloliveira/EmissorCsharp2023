namespace FusionCore.FusionAdm.Pessoas
{
    public class Fornecedor : PessoaExtensao
    {
        public int Id { get; set; }

        private Fornecedor()
        {
            //nhibernate
        }

        public Fornecedor(PessoaEntidade pessoa) : this()
        {
            Pessoa = pessoa;
        }
    }
}