using System;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sintegra.Dto;
using SintegraBr.Classes;

namespace FusionCore.Sintegra.Registros
{
    public class SintegraRegistro74
    {
        private readonly IRegistro74Dto _iRegistro74;
        private readonly DateTime _dataFinal;
        private readonly EmpresaDTO _empresa;

        public SintegraRegistro74(IRegistro74Dto iRegistro74, DateTime dataFinal, EmpresaDTO empresa)
        {
            _iRegistro74 = iRegistro74;
            _dataFinal = dataFinal;
            _empresa = empresa;
        }

        public Registro74 MontaRegistro74()
        {
            var r74 = new Registro74
            {
                DataInventario = _dataFinal,
                Quantidade = _iRegistro74.GetQuantidade(),
                CnpjProprietario = "00000000000000",
                CodigoPosseMerc = "1",
                CodigoProduto = _iRegistro74.GetCodigoProdutoServico(),
                InscrEstadualProprietario = "              ",
                Tipo = "74",
                UfProprietario = _empresa.EstadoDTO.Sigla,
                ValorProduto = _iRegistro74.GetValorBurto()
            };

            return r74;
        }
    }
}