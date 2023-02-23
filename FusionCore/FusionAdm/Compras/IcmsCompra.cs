using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Estadual;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.Compras
{
    public class IcmsCompra : EntidadeBase<int>
    {
        private int Id { get; set; }
        public ItemCompra Item { get; private set; }
        public TributacaoCst Icms { get; set; }
        public decimal Reducao { get; set; }
        public decimal Aliquota { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal ValorIcms { get; set; }
        public decimal ReducaoSt { get; set; }
        public decimal Mva { get; set; }
        public decimal AliquotaSt { get; set; }
        public decimal BaseCalculoSt { get; set; }
        public decimal ValorSt { get; set; }
        public decimal ValorFcpSt { get; set; }
        public decimal BaseCalculoFcpSt { get; set; }
        public decimal PercentualFcpSt { get; set; }

        protected override int ChaveUnica => Id;

        private IcmsCompra()
        {
            //nhibernate
        }

        public IcmsCompra(ItemCompra item) : this()
        {
            Item = item;
            Id = item.Id;
        }

    }
}
