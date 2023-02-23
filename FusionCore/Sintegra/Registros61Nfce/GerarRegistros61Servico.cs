using System.Collections.Generic;
using System.Linq;
using FusionCore.Sintegra.Dto;
using SintegraBr.Classes;

namespace FusionCore.Sintegra.Registros61Nfce
{
    public class GerarRegistros61Servico
    {
        private readonly IEnumerable<ItemRegistro61Nfce> _itemRegistro61;
        private readonly IEnumerable<PreRegistro61Nfce> _informacaoRegistro61;

        public GerarRegistros61Servico(IEnumerable<ItemRegistro61Nfce> itemRegistro61,
            IEnumerable<PreRegistro61Nfce> informacaoRegistro61)
        {
            _itemRegistro61 = itemRegistro61;
            _informacaoRegistro61 = informacaoRegistro61;
        }

        public IEnumerable<Registro61> Gerar()
        {
            var lista = _itemRegistro61.GroupBy(x => new
                {
                    x.EmissaoNoDia.Date,
                    x.Serie,
                    x.Cst
                })
                .Select(x =>
                {
                    var primeiroItem = x.First();
                    var itemInformacaoRegistro =
                        _informacaoRegistro61.First(aux =>
                            aux.EmissaoNoDia.Date == primeiroItem.EmissaoNoDia.Date
                            && aux.Serie == primeiroItem.Serie);

                    var somaPorCst = x.Sum(soma => soma.ValorTotal);

                    return new Registro61
                    {
                        Serie = primeiroItem.Serie.ToString(),
                        Subserie = "",
                        Modelo = "65",
                        Tipo = "61",
                        ValorTotal = somaPorCst,
                        BaseCalculoIcms = null,
                        NumInicial = TrataNumeroSintegra.Trata(itemInformacaoRegistro.NumeroInicialDia),
                        Aliquota = null,
                        Outras = somaPorCst,
                        DataEmissao = itemInformacaoRegistro.EmissaoNoDia,
                        NumFinal = TrataNumeroSintegra.Trata(itemInformacaoRegistro.NumeroFinalDia),
                        ValorIcms = null,
                        IsentaNaoTrib = null
                    };
                });

            return lista;
        }
    }
}