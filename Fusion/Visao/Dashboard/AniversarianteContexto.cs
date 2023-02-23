using System;
using System.Collections.Generic;
using System.Linq;
using Fusion.FastReport.DataSources;
using Fusion.FastReport.Repositorios;
using FusionCore.Repositorio.Filtros;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Dashboard
{
    public class AniversarianteContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private IList<DsAniversariante> _listaDeAniversariantes;
        private bool _possuiAniversariantes;

        public AniversarianteContexto(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public IList<DsAniversariante> ListaDeAniversariantes
        {
            get => _listaDeAniversariantes;
            set
            {
                _listaDeAniversariantes = value;
                PropriedadeAlterada();
            }
        }

        public bool PossuiAniversariantes
        {
            get => _possuiAniversariantes;
            set
            {
                _possuiAniversariantes = value;
                PropriedadeAlterada();
            }
        }

        public void Refresh()
        {
            var today = DateTime.Now;
            var mesNascimento = new FiltroPeriodoNascimento(today.Month, today.Month);

            using (var sessao = _sessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioAniversariante(sessao);

                ListaDeAniversariantes = repositorio.BuscaAniversariantes(mesNascimento);
                PossuiAniversariantes = ListaDeAniversariantes.Any();
            }
        }
    }
}