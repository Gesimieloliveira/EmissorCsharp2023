using System;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.Tributacoes.Estadual
{
    public class AliquotaInterna : EntidadeBase<Guid>
    {
        public Guid Id { get; set; }
        public decimal Aliquota { get; set; }
        public decimal AliquotaFcp { get; set; }
        public EstadoDTO EstadoUf { get; set; }


        protected override Guid ChaveUnica => Id;
    }
}