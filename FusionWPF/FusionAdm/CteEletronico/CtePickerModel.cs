using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Filtros;
using FusionWPF.FusionAdm.CteEletronico.FiltrosCustom;
using NHibernate.Util;

namespace FusionWPF.FusionAdm.CteEletronico
{
    public sealed class CtePickerFiltro : ModelBase
    {

        private DateTime? _dataEmissaoFinal;
        private DateTime? _dataEmissaoInicial;
        private string _textoSearch;

        public string TextoSearch
        {
            get { return _textoSearch; }
            set
            {
                if (value == _textoSearch) return;
                _textoSearch = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? DataEmissaoFinal
        {
            get { return _dataEmissaoFinal; }
            set
            {
                _dataEmissaoFinal = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? DataEmissaoInicial
        {
            get { return _dataEmissaoInicial; }
            set
            {
                _dataEmissaoInicial = value;
                PropriedadeAlterada();
            }
        }
    }

    public class CtePickerModel : GridPickerModel
    {
        public CtePickerModel()
        {
            Titulo = "Busca de CT-e";
            BuscaSelecionada = new OpcaoPadraoWatermark("Destinatário, Emitente, Remetente, Tomador, Chave, ID/Código");
        }

        private IList<CtePickerDTO> _cacheLista;

        protected override void OnAplicaFiltro(FiltroRetorno filtroRetorno)
        {
            var filtro = filtroRetorno.FiltroModel as CtePickerFiltro;

            TextoPesquisado = filtro.TextoSearch;

            IEnumerable<CtePickerDTO> query = _cacheLista.AsQueryable();

            if (filtro.TextoSearch.IsNotNullOrEmpty())
            {
                var search = filtro.TextoSearch.TrimOrEmpty().ToUpper();

                query = query.Where(x => x.NomeDestinatario.ToUpper().Contains(search)
                                         || x.NomeEmitente.ToUpper().Contains(search)
                                         || x.NomeRemetente.ToUpper().Contains(search)
                                         || x.NomeTomador.ToUpper().Contains(search)
                                         || x.Chave.Contains(search)
                                         || x.Id.ToString() == search
                    );
            }


            if (filtro.DataEmissaoInicial != null)
                query = query.Where(x => x.EmitidaEm >= filtro.DataEmissaoInicial);


            if (filtro.DataEmissaoFinal != null)
                query = query.Where(x => x.EmitidaEm <= filtro.DataEmissaoFinal);

            var lista = query.Take(50).OrderByDescending(x => x.Id).ToList();

            PreecherListaCom(lista);
        }

        protected override void UsarFiltro(Filtro.FiltroBuilder builder)
        {
            builder.UsarUserControlFiltro(new CteFiltroPorData())
                .UsarModel(new CtePickerFiltro());
        }

        protected override void OnAntesDeAtivarFiltro(object modelFiltro)
        {
            var model = modelFiltro as CtePickerFiltro;

            model.TextoSearch = TextoPesquisado;
        }

        protected override void OnInicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCte(sessao);
                _cacheLista = repositorio.BuscarTodosParaPicker();
            }

            PreecherListaCom(_cacheLista.Take(50).ToList());
        }

        private void PreecherListaCom(IList<CtePickerDTO> cacheLista)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (cacheLista == null)
                throw new InvalidDataException("Nenhum CT-e para listar");

            cacheLista.ForEach(AddCteModel);

            QtdeMaximaItens = 50;
            QtdeMaximaFoiAlcancada = cacheLista.Count == QtdeMaximaItens;
        }

        private void AddCteModel(CtePickerDTO ctePicker)
        {
            ItensLista.Add(new GridPickerItem
            {
                Titulo = "Chave: " + ctePicker.Chave,
                Subtitulo = "Emitente: " + ctePicker.NomeEmitente,
                Coluna1 = "Remetente: " + ctePicker.NomeRemetente,
                Coluna2 = "Destinatário: " + ctePicker.NomeDestinatario,
                Coluna3 = "Tomador: " + ctePicker.NomeTomador,
                Coluna4 = "Valor Serviço: " + ctePicker.ValorServico,
                ItemReal = ctePicker
            });
        }

        public override void AplicaPesquisa(string input)
        {
            if (input.IsNullOrEmpty())
            {
                Inicializar();
                return;
            }

            var search = input.ToUpper();

            var query = _cacheLista.Where(x => x.NomeDestinatario.ToUpper().Contains(search)
                                               || x.NomeEmitente.ToUpper().Contains(search)
                                               || x.NomeRemetente.ToUpper().Contains(search)
                                               || x.NomeTomador.ToUpper().Contains(search)
                                               || x.Chave.Contains(search)
                                               || x.Id.ToString() == search
                );

            var lista = query.Take(50).ToList();

            PreecherListaCom(lista);
        }
    }
}