using System;
using System.Collections.Generic;
using System.Linq;
using Fusion.FastReport.DataSources;
using Fusion.FastReport.Repositorios;
using Fusion.Sessao;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Dashboard
{
    public class DashContasPagarContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private TotalizadoresContexto _totalizadoresContexto;
        private IList<DsDocumentosPagar> _documentosAbertos;
        private bool _possuiDocumentoAberto;
        private bool _possuiFusionGestor { get; }

        public DashContasPagarContexto(ISessaoManager sessaoManager, SessaoSistema sessaoSistema)
        {
            _sessaoManager = sessaoManager;
            _possuiFusionGestor = sessaoSistema.AcessoConcedido.PossuiFusionGestor;
        }

        public IList<DsDocumentosPagar> DocumentosAbertos
        {
            get => _documentosAbertos;
            set
            {
                _documentosAbertos = value;
                PropriedadeAlterada();

                PossuiDocumentoAberto = value.Any();
            }
        }

        public bool PossuiDocumentoAberto
        {
            get => _possuiDocumentoAberto;
            set
            {
                _possuiDocumentoAberto = value;
                PropriedadeAlterada();
            }
        }

        public void Refresh()
        {
            if (_possuiFusionGestor == false) return;

            var today = DateTime.Now;

            using (var sessao = _sessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioDsDocumentoPagar(sessao);
                var docs = repositorio.BuscaAbertosAteDataFinal(today);

                DocumentosAbertos = docs.OrderBy(i => i.Vencimento).ToList();
            }

            _totalizadoresContexto.DocumentosAbertos = DocumentosAbertos;
        }

        public void ComTotalizacao(TotalizadoresContexto totalizadoresContexto)
        {
            _totalizadoresContexto = totalizadoresContexto;
        }
    }
}