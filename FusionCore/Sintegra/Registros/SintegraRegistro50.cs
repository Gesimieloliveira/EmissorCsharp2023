using FusionCore.Sintegra.Dto;
using SintegraBr.Classes;

namespace FusionCore.Sintegra.Registros
{
    public class SintegraRegistro50
    {
        private readonly IRegistro50Dto _registro50Dto;

        public SintegraRegistro50(IRegistro50Dto registro50Dto)
        {
            _registro50Dto = registro50Dto;
        }

        public Registro50 MontaRegistro50()
        {
            var r50 = new Registro50();

            r50.Cnpj = _registro50Dto.GetDocumentoUnico();
            r50.InscrEstadual = _registro50Dto.GetInscricaoEstadual();
            r50.DataEmissaoRecebimento = _registro50Dto.GetEmissaoRecebimento();
            r50.Uf = _registro50Dto.GetUf();
            r50.Modelo = _registro50Dto.GetModelo();
            r50.Serie = _registro50Dto.GetSerie();
            r50.Numero = _registro50Dto.GetNumero();
            r50.Cfop = _registro50Dto.GetCfop();
            r50.Emitente = _registro50Dto.GetEmitente();
            r50.ValorTotal = _registro50Dto.GetValorTotal();
            r50.BaseCalculoIcms = _registro50Dto.GetBaseCalculoIcms();
            r50.ValorIcms = _registro50Dto.GetValorIcms();
            r50.ValorIsentaOuNaoTributadas = 0;
            r50.ValorOutras = _registro50Dto.GetValorOutras();
            r50.AliquotaIcms = _registro50Dto.GetAliquotaIcms();
            r50.SituacaoNotaFiscal = _registro50Dto.GetSituacaoNotaFiscal();

            return r50;
        }
    }
}