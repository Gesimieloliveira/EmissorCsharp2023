using System;
using FusionCore.Sintegra.Dto;
using SintegraBr.Classes;

namespace FusionCore.Sintegra.Registros
{
    public class SintegraRegistro75
    {
        private readonly IRegistro75Dto _iRegistro75;
        private readonly DateTime _dataInicial;
        private readonly DateTime _dataFinal;

        public SintegraRegistro75(IRegistro75Dto iRegistro75, DateTime dataInicial, DateTime dataFinal)
        {
            _iRegistro75 = iRegistro75;
            _dataInicial = dataInicial;
            _dataFinal = dataFinal;
        }

        public Registro75 MontaRegistro75()
        {
            var r75 = new Registro75
            {
                AliquotaIcms = _iRegistro75.GetAliquotaIcms(),
                BaseCalculoSt = _iRegistro75.GetBaseCalculoIcmsSt(),
                UnidadeMedida = _iRegistro75.GetUnidadeMedida(),
                AliquotaIpi = _iRegistro75.GetAliquotaIpi(),
                Descricao = _iRegistro75.GetDescricao(),
                CodItem = _iRegistro75.GetCodigoProdutoServico(),
                CodNcm = _iRegistro75.GetCodigoNcm(),
                DataFinal = _dataFinal,
                DataInicial = _dataInicial,
                ReducaoBaseIcms = _iRegistro75.GetReducaoIcms()
            };

            return r75;
        }
    }
}