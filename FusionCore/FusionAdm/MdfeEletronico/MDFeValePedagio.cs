using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeValePedagio : EntidadeBase<int>
    {
        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public MDFeRodoviario Rodoviario { get; set; }
        public string CnpjEmpresaFornecedora { get; set; }
        public string CnpjResponsavelPagamento { get; set; }
        public string CpfResponsavel { get; set; }
        public string NumeroComprovante { get; set; }
        public decimal Valor { get; set; }
    }
}