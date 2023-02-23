using System;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.Tributacoes.Interestadual
{
    public class AliquotaInterestadual : EntidadeBase<Guid>
    {
        public Guid Id { get; private set; }
        public EstadoDTO Origem { get; private set; }
        public EstadoDTO Destino { get; private set; }
        public decimal Aliquota { get; private set; }

        protected override Guid ChaveUnica => Id;
    }
}