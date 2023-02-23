using System;
using FusionCore.Repositorio.Base;

namespace FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Dominio
{
    public class ContingenciaNfce : EntidadeBase<int>
    {
        public int Id { get; private set; }

        public DateTime EntrouEm { get; private set; }

        public DateTime FinalizaEm { get; private set; }

        public void Ativar()
        {
            EntrouEm = DateTime.Now;
            FinalizaEm = EntrouEm.AddMinutes(40);
        }

        protected override int ChaveUnica => Id;
    }
}