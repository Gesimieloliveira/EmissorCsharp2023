using System.Collections.Generic;
using System.Linq;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Repositorios;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace FusionWPF.SharedViews.ControleCaixa
{
    public class GridLancamentosCaixaContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;

        internal GridLancamentosCaixaContexto(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public IEnumerable<LancamentoAvulsoCaixaDTO> Items
        {
            get => GetValue<IEnumerable<LancamentoAvulsoCaixaDTO>>();
            set => SetValue(value);
        }

        public LancamentoAvulsoCaixaDTO ItemSelecionado
        {
            get => GetValue<LancamentoAvulsoCaixaDTO>();
            set => SetValue(value);
        }

        public void ListarItems()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioLancamentoAvulso(sessao);
                var items = repositorio.ListarLancamentos();

                Items = items.OrderByDescending(i => i.DataCriacao);
            }
        }
    }
}