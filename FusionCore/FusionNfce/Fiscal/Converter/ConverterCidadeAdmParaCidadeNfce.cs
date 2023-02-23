using FusionCore.FusionNfce.Cidade;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Fiscal.Converter
{
    public class ConverterCidadeAdmParaCidadeNfce
    {
        private readonly CidadeDTO _cidade;

        public ConverterCidadeAdmParaCidadeNfce(CidadeDTO cidade)
        {
            _cidade = cidade;
        }

        public CidadeNfce Execute()
        {
            return new CidadeNfce
            {
                Id = _cidade.Id,
                CodigoIbge = _cidade.CodigoIbge,
                Nome = _cidade.Nome,
                SiglaUf = _cidade.SiglaUf
            };
        }
    }
}