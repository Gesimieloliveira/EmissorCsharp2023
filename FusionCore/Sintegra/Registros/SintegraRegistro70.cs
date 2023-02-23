using FusionCore.Sintegra.Dto;
using SintegraBr.Classes;

namespace FusionCore.Sintegra.Registros
{
    public class SintegraRegistro70
    {
        private readonly IRegistro70Dto _iRegistro70CteEntradaDto;

        public SintegraRegistro70(IRegistro70Dto iRegistro70CteEntradaDto)
        {
            _iRegistro70CteEntradaDto = iRegistro70CteEntradaDto;
        }

        public Registro70 MontaRegistro70()
        {
            var r70 = new Registro70();

            r70.Cnpj = _iRegistro70CteEntradaDto.GetDocumentoUnico();
            r70.InscrEstadual = _iRegistro70CteEntradaDto.GetInscricaoEstadual();
            r70.DataEmissaoRecebimento = _iRegistro70CteEntradaDto.GetEmissaoRecebimento();
            r70.Uf = _iRegistro70CteEntradaDto.GetUf();
            r70.Modelo = _iRegistro70CteEntradaDto.GetModelo();
            r70.Serie = _iRegistro70CteEntradaDto.GetSerie();
            r70.Subserie = _iRegistro70CteEntradaDto.GetSubSerie();
            r70.Numero = _iRegistro70CteEntradaDto.GetNumero();
            r70.Cfop = _iRegistro70CteEntradaDto.GetCfop();
            r70.ValorTotal = _iRegistro70CteEntradaDto.GetValorTotal();
            r70.BaseCalculoIcms = _iRegistro70CteEntradaDto.GetBaseCalculoIcms() == 0 ? (decimal?) null : _iRegistro70CteEntradaDto.GetBaseCalculoIcms();
            r70.ValorIcms = _iRegistro70CteEntradaDto.GetValorIcms() == 0 ? (decimal?) null : _iRegistro70CteEntradaDto.GetValorIcms();
            r70.ValorOutras = _iRegistro70CteEntradaDto.GetValorOutras() == 0 ? null : _iRegistro70CteEntradaDto.GetValorOutras();
            r70.ModalidadeFrete = _iRegistro70CteEntradaDto.GetCifFob();
            r70.Situacao = _iRegistro70CteEntradaDto.GetSituacaoNotaFiscal();

            return r70;
        }
    }
}