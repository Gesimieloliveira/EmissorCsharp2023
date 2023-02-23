using System;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class PdvFormaPagamentoDTO : IEntidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime? AlteradoEm { get; set; }
    }
}