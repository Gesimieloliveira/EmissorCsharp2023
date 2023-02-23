using FusionCore.FusionAdm.MdfeEletronico;

namespace Fusion.Visao.MdfeEletronico.Aba.Entidades
{
    public class GirdValePedagio
    {
        public string CnpjEmpresaFornecedora { get; set; }
        public string CnpjResponsavel { get; set; }
        public string CpfResponsavel { get; set; }
        public string NumeroCompra { get; set; }
        public decimal Valor { get; set; }
        public MDFeValePedagio MDFeValePedagio { get; set; }
    }
}