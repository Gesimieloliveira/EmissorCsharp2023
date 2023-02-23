using System;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Pdv
{
    public class FormaPagamentoEcfDt : IEntidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CodigoEcf { get; set; }
        public DateTime? AlteradoEm { get; set; }
    }
}