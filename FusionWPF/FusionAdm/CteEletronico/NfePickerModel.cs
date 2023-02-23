using System.Collections.Generic;
using System.IO;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Filtros;
using FusionWPF.FusionAdm.CteEletronico.FiltrosCustom;

namespace FusionWPF.FusionAdm.CteEletronico
{
    public class NfePickerModel : GridPickerModel
    {
        public NfePickerModel()
        {
            Titulo = "Busca de NF-e";
            BuscaSelecionada = new OpcaoPadraoWatermark("Destinatário, Emitente, Chave, ID/Código");
        }

        private IList<NfePickerDTO> _cacheLista;

        protected override void OnAplicaFiltro(FiltroRetorno filtroRetorno)
        {
            var filtro = filtroRetorno.FiltroModel as NfePickerFiltro;

            TextoPesquisado = filtro.TextoSearch;

            var query = _cacheLista.ToList().AsQueryable();

            if (filtro.TextoSearch.IsNotNullOrEmpty())
            {
                var search = filtro.TextoSearch.TrimOrEmpty().ToUpper();

                query = query.Where(x => x.NomeDestinatario.ToUpper().Contains(search)
                                         || x.NomeEmitente.ToUpper().Contains(search)
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
            builder.UsarModel(new NfePickerFiltro())
                .UsarUserControlFiltro(new NfeFiltroPorData());
        }

        protected override void OnAntesDeAtivarFiltro(object modelFiltro)
        {
            var model = modelFiltro as NfePickerFiltro;

            model.TextoSearch = TextoPesquisado;
        }
        protected override void OnInicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioNfe(sessao);
                _cacheLista = repositorio.BuscarTodosParaPicker();
            }

            PreecherListaCom(_cacheLista.Take(50).ToList());
        }

        private void PreecherListaCom(List<NfePickerDTO> lista)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (lista == null)
            {
                throw new InvalidDataException("Nenhum NF-e para listar");
            }

            lista.ForEach(AddNFeModel);

            QtdeMaximaItens = 50;
            QtdeMaximaFoiAlcancada = lista.Count == QtdeMaximaItens;
        }

        private void AddNFeModel(NfePickerDTO nfe)
        {
            ItensLista.Add(new GridPickerItem
            {
                Titulo = "Chave: " + nfe.Chave,
                Subtitulo = "Emitente: " + nfe.NomeEmitente,
                Coluna1 = "Destinatário: " + nfe.NomeDestinatario,
                Coluna2 = "Emitida Em: " + nfe.EmitidaEm,
                Coluna3 = "Total: " + nfe.TotalNf,
                ItemReal = nfe
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
                                               || x.Chave.Contains(search)
                                               || x.Id.ToString() == search
                );

            var lista = query.Take(50).ToList();

            PreecherListaCom(lista);
        }
    }
}