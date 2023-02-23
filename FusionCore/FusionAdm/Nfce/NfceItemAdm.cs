using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Diversos;

namespace FusionCore.FusionAdm.Nfce
{
    public class NfceItemAdm
    {
        private decimal _valorTotal;

        public NfceItemAdm() { }

        private NfceImpostoCsosnAdm _impostoIcms;
        public int Id { get; set; }
        public NfceAdm Nfce { get; set; }
        public ProdutoDTO Produto { get; set; }
        public short NumeroItem { get; set; }
        public string Gtin { get; set; }
        public string CodigoNcm { get; set; }
        public string CodigoCest { get; set; }
        public string Nome { get; set; }
        public string SiglaUnidade { get; set; }
        public string SiglaUnidadeTributavel { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public bool Cancelado { get; set; }
        public decimal ValorTributoEstadual { get; set; }
        public decimal ValorTributoFederal { get; set; }
        public decimal ValorTributoAproximado { get; set; }
        public decimal Acrescimo { get; set; }
        public CfopDTO Cfop { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public string Observacao { get; set; }

        public NfceImpostoCofinsAdm ImpostoCofins
        {
            get => _impostoCofins;
            set
            {
                _impostoCofins = value;
                if (value != null)
                    _impostoCofins.Item = this;
            }
        }

        public NfceImpostoPisAdm ImpostoPis
        {
            get => _impostoPis;
            set
            {
                _impostoPis = value;
                if (value != null)
                    _impostoPis.Item = this;
            }
        }

        public NfceImpostoCsosnAdm ImpostoIcms
        {
            get { return _impostoIcms; }
            set
            {
                _impostoIcms = value;
                _impostoIcms.Item = this;
            }
        }

        public decimal SubTotal
        {
            get { return ValorTotal - Desconto; }
            private set { }
        }

        public decimal Desconto
        {
            get { return _desconto; }
            set { _desconto = value.Arredonda(); }
        }

        public decimal ValorTotal
        {
            get { return CalcularValorTotal(); }
            private set { _valorTotal = value; }
        }

        public decimal DescontoAlteraItem { get; set; }

        private decimal CalcularValorTotal()
        {
            var valorUnitario = ValorUnitario;

            var valorUnitarioFinal = (Quantidade * valorUnitario);

            return valorUnitarioFinal.Arredonda();
        }
        private decimal _desconto;
        private NfceImpostoCofinsAdm _impostoCofins;
        private NfceImpostoPisAdm _impostoPis;
    }
}