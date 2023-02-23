using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using SintegraBr.Classes;

namespace FusionCore.Sintegra.Registro61RNfce
{
    public class Registro61RServico
    {
        private readonly DateTime _filtroDataInicio;
        private readonly DateTime _filtroDataFinal;
        private readonly EmpresaDTO _daEmpresa;

        public Registro61RServico(DateTime filtroDataInicio, DateTime filtroDataFinal, EmpresaDTO daEmpresa)
        {
            _filtroDataInicio = filtroDataInicio;
            _filtroDataFinal = filtroDataFinal;
            _daEmpresa = daEmpresa;
        }

        public IEnumerable<Registro61R> ObterRegistros61R()
        {
            IList<Registro61RProdutoDTO> itemRegistro61R;
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                itemRegistro61R = new RepositorioNfceAdm(sessao).BuscarRegistro61R(_filtroDataInicio, _filtroDataFinal, _daEmpresa);
            }

            return itemRegistro61R.Select(registro61RProdutoDTO => new Registro61R
                {
                    BaseCalculoIcms = registro61RProdutoDTO.BaseCalculoIcmsMensal,
                    DataEmissao = _filtroDataFinal,
                    AliquotaIcms = registro61RProdutoDTO.AliquotaMensal,
                    CodItem = registro61RProdutoDTO.Codigo.ToString(),
                    Quantidade = registro61RProdutoDTO.QuantidadeMovimentadaMensal,
                    ValorItem = registro61RProdutoDTO.ValorBrutoMensal + registro61RProdutoDTO.ValorAcrescimoMensal - registro61RProdutoDTO.ValorDescontoMensal
                })
                .ToList();
        }
    }
}