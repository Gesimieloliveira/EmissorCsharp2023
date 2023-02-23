using FusionCore.Sintegra.Dto;
using SintegraBr.Classes;

namespace FusionCore.Sintegra.Registros
{
    public class SintegraRegistro54
    {
        private readonly IRegistro54Dto _iRegistro54;

        public SintegraRegistro54(IRegistro54Dto iRegistro54)
        {
            _iRegistro54 = iRegistro54;
        }

        public Registro54 MontaRegistro54()
        {
            var r54 = new Registro54();

            r54.Cnpj = _iRegistro54.GetDocumentoUnico();
            r54.Modelo = (int) _iRegistro54.GetModelo();
            r54.Serie = _iRegistro54.GetSerie();
            r54.Numero = _iRegistro54.GetNumero();
            r54.Cfop = _iRegistro54.GetCfop();
            r54.Cst = _iRegistro54.GetCst();
            r54.NumeroItem = _iRegistro54.GetNumeroItem();
            r54.CodProdutoServico = _iRegistro54.GetCodigoProdutoServico();
            r54.Quantidade = _iRegistro54.GetQuantidade();
            r54.VlProdutoServico = _iRegistro54.GetValorProduto();
            r54.VlDescontoDespesaAc = _iRegistro54.GetValorTotalDescontos() == 0 ? (decimal?) null : _iRegistro54.GetValorTotalDescontos();
            r54.BaseCalculoIcms = _iRegistro54.GetBaseCalculoIcms() == 0 ? (decimal?) null : _iRegistro54.GetBaseCalculoIcms();
            r54.BaseCalculoIcmsSt = _iRegistro54.GetBaseCalculoIcmsSt() == 0 ? (decimal?) null : _iRegistro54.GetBaseCalculoIcmsSt();
            r54.VlIpi = _iRegistro54.GetValorIpi() == 0 ? (decimal?) null : _iRegistro54.GetValorIpi();
            r54.AliquotaIcms = _iRegistro54.GetAliquotaIcms() == 0 ? (decimal?) null : _iRegistro54.GetAliquotaIcms();

            return r54;
        }
    }
}