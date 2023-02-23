using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Pdv
{
    public class ClienteDt : IEntidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string Endereco { get; set; }

        public decimal LimiteCredito { get; set; }

        public bool AplicaLimiteCredito { get; set; }
    }
}