using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.Cliente
{
    public class ClienteEmailNfce : Entidade
    {
        public ClienteEmailNfce()
        {
        }

        public ClienteEmailNfce(PessoaEmail email, ClienteNfce cliente)
        {
            CopiaInformacoes(email, cliente);
        }

        public int Id { get; set; }
        public ClienteNfce Cliente { get; set; }
        public string Email { get; set; }

        protected override int ReferenciaUnica => Id;

        private void CopiaInformacoes(PessoaEmail email, ClienteNfce cliente)
        {
            Id = email.Id;
            Cliente = cliente;
            Email = email.Email.Valor;
        }
    }
}