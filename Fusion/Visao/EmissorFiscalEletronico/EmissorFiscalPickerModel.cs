using System.Collections.Generic;
using System.IO;
using System.Linq;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.EmissorFiscalNfe;
using FusionWPF.Base.GridPicker;
using NHibernate.Util;

namespace Fusion.Visao.EmissorFiscalEletronico
{
    public class EmissorFiscalPickerModel : GridPickerModel
    {
        private IList<EmissorFiscal> _cacheLista;

        protected override void OnInicializar()
        {
            using (var repositorio = new RepositorioComun<EmissorFiscal>(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                _cacheLista = repositorio.Busca(new TodosEmissorFiscalNfe());
                PreecherListaCom(_cacheLista);
            }
        }

        private void PreecherListaCom(IList<EmissorFiscal> cacheLista)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (cacheLista == null)
                throw new InvalidDataException("Nenhum cfop para listar");

            cacheLista.ForEach(AddEmissorFiscal);
        }

        private void AddEmissorFiscal(EmissorFiscal emissorFiscal)
        {
            var tipo = "NF-e";

            if (emissorFiscal.FlagNfce)
                tipo = "NFC-e";

            ItensLista.Add(new GridPickerItem
            {
                Titulo = emissorFiscal.Descricao,
                Coluna1 = tipo,
                Coluna2 = $"#Código: {emissorFiscal.Id}",
                ItemReal = emissorFiscal
            });
        }

        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();

            if (string.IsNullOrWhiteSpace(input))
            {
                _cacheLista.ForEach(AddEmissorFiscal);
                return;
            }

            var lista = _cacheLista
                .Where(
                    item => item.Descricao.Contains(input) ||
                            item.Id.ToString() == input
                ).ToList();

            lista.ForEach(AddEmissorFiscal);
        }
    }
}