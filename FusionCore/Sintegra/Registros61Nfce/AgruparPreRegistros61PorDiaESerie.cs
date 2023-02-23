using System.Collections.Generic;
using System.Linq;
using FusionCore.Sintegra.Dto;

namespace FusionCore.Sintegra.Registros61Nfce
{
    public class AgruparPreRegistros61PorDiaESerie
    {
        private readonly IEnumerable<ItemRegistro61Nfce> _itemRegistro61;

        public AgruparPreRegistros61PorDiaESerie(IEnumerable<ItemRegistro61Nfce> itemRegistro61)
        {
            _itemRegistro61 = itemRegistro61;
        }

        public IEnumerable<PreRegistro61Nfce> Agrupar()
        {
            var informacaoRegistro61 = _itemRegistro61.GroupBy(x => new
                {
                    x.EmissaoNoDia.Date,
                    x.Serie
                })
                .Select(x =>
                {
                    var numeroFiscalMenor = x.Min(menor => menor.NumeroFiscal);
                    var numeroFiscalMaior = x.Min(maior => maior.NumeroFiscal);

                    var primeiroItem = x.SingleOrDefault(menor => menor.NumeroFiscal == numeroFiscalMenor);
                    var ultimoItem = x.SingleOrDefault(maior => maior.NumeroFiscal == numeroFiscalMaior);

                    return new PreRegistro61Nfce()
                    {
                        NumeroInicialDia = primeiroItem.NumeroFiscal,
                        NumeroFinalDia = ultimoItem.NumeroFiscal,
                        EmissaoNoDia = primeiroItem.EmissaoNoDia,
                        Serie = primeiroItem.Serie
                    };
                });

            return informacaoRegistro61;
        }
    }
}