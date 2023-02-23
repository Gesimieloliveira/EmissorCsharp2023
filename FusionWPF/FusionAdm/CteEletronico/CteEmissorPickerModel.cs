using System.Collections.Generic;
using System.IO;
using System.Linq;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using NHibernate.Util;

namespace FusionWPF.FusionAdm.CteEletronico
{
    public class CteEmissorPickerModel : GridPickerModel
    {
        private IList<EmissorFiscal> _cacheLista;

        protected override void OnInicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);

                _cacheLista = repositorio.BuscaEmissorCTe();
                PreecherListaCom(_cacheLista);
            }
        }

        private void PreecherListaCom(IList<EmissorFiscal> cacheLista)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (cacheLista == null)
                throw new InvalidDataException("Nenhum Emissor CT-e para listar");

            cacheLista.ForEach(AddEmissorFiscal);
        }

        private void AddEmissorFiscal(EmissorFiscal emissorFiscal)
        {
            const string tipo = "CT-e";
            const string tipoCteOs = "CT-e Os";

            ItensLista.Add(new GridPickerItem
            {
                Titulo = emissorFiscal.Descricao,
                Coluna1 = emissorFiscal.FlagCteOs ? tipoCteOs : tipo,
                Coluna2 = $"#Código: {emissorFiscal.Id}",
                ItemReal = emissorFiscal
            });
        }

        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();

            if (!input.IsNullOrEmpty())
            {
                var listaComFiltro = _cacheLista.Where(
                item =>
                    item.Descricao.Contains(input) || item.Id.ToString().Equals(input))
                .ToList();

                listaComFiltro.ForEach(AddEmissorFiscal);

                return;
            }

            var lista = _cacheLista.ToList();

            lista.ForEach(AddEmissorFiscal);
        }
    }
}