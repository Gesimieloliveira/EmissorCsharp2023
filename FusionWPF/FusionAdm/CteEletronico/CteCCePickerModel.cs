using System.Collections.Generic;
using System.IO;
using System.Linq;
using FusionCore.FusionAdm.CteEletronico.CCe;
using FusionWPF.Base.GridPicker;
using NHibernate.Util;
using FusionCore.Helpers.Hidratacao;

namespace FusionWPF.FusionAdm.CteEletronico
{
    public class CteCCePickerModel : GridPickerModel
    {
        private IList<ElementoCCe> _cacheLista;

        protected override void OnInicializar()
        {
            _cacheLista = ListaCCe.ElementosCCes;
            PreecherListaCom(_cacheLista);
        }

        private void PreecherListaCom(IList<ElementoCCe> cacheLista)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (cacheLista == null)
                throw new InvalidDataException("Nenhum Emissor CT-e para listar");

            cacheLista.ForEach(AddEmissorFiscal);
        }

        private void AddEmissorFiscal(ElementoCCe elementoCCe)
        {
            var titulo = elementoCCe.Descricao;
            var subtitulo = string.Empty;

            if (elementoCCe.Pai != null)
            {
                subtitulo = elementoCCe.Pai.Descricao;
            }

            ItensLista.Add(new GridPickerItem
            {
                Titulo = "Campo - " + titulo,
                Subtitulo = "Grupo - " + subtitulo,
                ItemReal = elementoCCe
            });
        }

        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();

            if (!input.IsNullOrEmpty())
            {
                var listaComFiltro = _cacheLista.Where(
                item =>
                    item.Descricao.ToUpper().Contains(input.ToUpper()) || item.Tag.ToUpper().Contains(input.ToUpper()) 
                    || item.Pai.Tag.ToUpper().Contains(input.ToUpper())
                    || item.Pai.Descricao.ToUpper().Contains(input.ToUpper()))
                .ToList();

                listaComFiltro.ForEach(AddEmissorFiscal);

                return;
            }

            var lista = _cacheLista.ToList();

            lista.ForEach(AddEmissorFiscal);
        }
    }
}