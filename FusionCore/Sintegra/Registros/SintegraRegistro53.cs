using FusionCore.Sintegra.Dto;
using SintegraBr.Classes;

namespace FusionCore.Sintegra.Registros
{
    public class SintegraRegistro53
    {
        private readonly IRegistro53Dto _registro53;

        public SintegraRegistro53(IRegistro53Dto registro53)
        {
            _registro53 = registro53;
        }

        public Registro53 MontaRegistro53()
        {
            var r53 = new Registro53();

            r53.Cnpj = _registro53.GetDocumentoUnico();
            r53.InscrEstadual = _registro53.GetInscricaoEstadual();
            r53.DataEmissaoRecbto = _registro53.GetEmissaoRecebimento();
            r53.Uf = _registro53.GetUf();
            r53.Modelo = (int) _registro53.GetModelo();
            r53.Serie = _registro53.GetSerie();
            r53.Numero = _registro53.GetNumero();
            r53.Cfop = _registro53.GetCfop();
            r53.Emitente = _registro53.GetEmitente();
            r53.BaseIcmsSt = _registro53.GetBaseCalculoIcmsSt() == 0 ? (decimal?) null : _registro53.GetBaseCalculoIcmsSt();
            r53.VlIcmsRetido = _registro53.GetIcmsRetido() == 0 ? (decimal?) null : _registro53.GetIcmsRetido();
            r53.VlDespesasAcessorias = _registro53.GetDespesasAcessorias() == 0 ? (decimal?) null : _registro53.GetDespesasAcessorias();
            r53.Situacao = _registro53.GetSituacaoNotaFiscal();
            r53.CodigoAntecipacao = _registro53.GetCodigoAntecipacao();

            return r53;
        }
    }
}