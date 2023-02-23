using System.Collections.Generic;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.Core.Tributario;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.GridPicker;
using NHibernate.Util;

namespace Fusion.Visao.PerfilCfop
{
    public class PerfilCfopFiltro
    {
        public OpercaoCfop Operacao { private get; set; }
        public OrigemOperacao Origem { private get; set; }

        public IEnumerable<PerfilCfopDTO> AplicaFiltro(IEnumerable<PerfilCfopDTO> lista)
        {
            return lista.Where(i => i.Origem == Origem && i.Operacao == Operacao);
        }
    }

    public class PerfilCfopPickerModel : GridPickerModel
    {
        private IEnumerable<PerfilCfopDTO> _cacheLista;
        private PerfilCfopFiltro _filtro;

        protected override void OnInicializar()
        {
            Titulo = "Perfil CFOP";

            _cacheLista = PerfilCfopFacade.FindTodos();

            AplicaPesquisa(string.Empty);
        }

        public void ComFiltro(PerfilCfopFiltro filtro)
        {
            _filtro = filtro;
        }

        private IEnumerable<PerfilCfopDTO> GetApenasPermitidosPeloFiltro()
        {
            return _filtro != null ? _filtro.AplicaFiltro(_cacheLista) : _cacheLista.ToList();
        }

        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            var filtrados = GetApenasPermitidosPeloFiltro();

            if (string.IsNullOrEmpty(input))
            {
                Listar(filtrados);
                return;
            }

            var pesquisa = filtrados.Where(i
                => i.Id.ToString() == input
                   || i.Codigo.Contains(input)
                   || i.Descricao.ToUpper().Contains(input.ToUpper()));

            Listar(pesquisa);
        }

        private void Listar(IEnumerable<PerfilCfopDTO> lista)
        {
            var novaLista = new List<GridPickerItem>();

            lista.ForEach(i =>
            {
                var item = new GridPickerItem
                {
                    Titulo = i.Descricao,
                    Coluna1 = $"#Código: {i.Codigo}",
                    ItemReal = i
                };

                novaLista.Add(item);
            });

            SetItensLista(novaLista);
        }
    }
}