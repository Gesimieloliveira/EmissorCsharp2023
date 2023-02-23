using FusionCore.Repositorio.Base;

namespace FusionCore.SelecionarNfce
{
    public class ClienteDto : EntidadeBase<int>
    {
        public ClienteDto(string nomeCliente, int idCliente)
        {
            Id = idCliente;
            Nome = nomeCliente;
        }

        public int Id { get; }
        public string Nome { get; }
        protected override int ChaveUnica => Id;
    }
}