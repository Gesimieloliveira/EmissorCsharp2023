using System.Collections.Generic;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.Wpf.Conversores;
using FusionWPF.Base.GridPicker;
using NHibernate.Util;

namespace FusionWPF.FusionAdm.Empresas
{
    public class EmpresaPickerModel : GridPickerModel
    {
        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmpresa(sessao);

                var empresas = repositorio.BuscarEmpresaPickerModelDtos(input);

                AddLista(empresas);
            }
        }

        private void AddLista(ICollection<EmpresaPickerModelDto> empresas)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (empresas == null || empresas.Count == 0)
            {
                return;
            }

            empresas.ForEach(AddLista);
        }

        private void AddLista(EmpresaPickerModelDto empresa)
        {
            ItensLista.Add(new GridPickerItem
            {
                Titulo = empresa.RazaoSocial,
                Coluna1 = $"Código: {empresa.Id:D11}",
                Coluna2 = $"Cnpj: {new CpfCnpjMaskConverter().Convert(empresa.Cnpj, empresa.Cnpj.GetType(), null, null)}",
                Coluna3 = $"Inscrição Estadual: {empresa.InscricaoEstadual}",
                Coluna4 = $"{empresa.GetCidadeUf()}",
                ItemReal = empresa
            });
        }
    }
}