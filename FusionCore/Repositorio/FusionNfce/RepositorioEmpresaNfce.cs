using System.Collections.Generic;
using FusionCore.CadastroEmpresa;
using FusionCore.FusionNfce.Empresa;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioEmpresaNfce : Repositorio<EmpresaNfce, int>, IRepositorioEmpresa
    {
        public RepositorioEmpresaNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(EmpresaNfce empresa)
        {
            Sessao.SaveOrUpdate(empresa);
        }

        public IEnumerable<IEmpresa> BuscarTodas()
        {
            var query = Sessao.QueryOver<EmpresaNfce>();

            return query.List<EmpresaNfce>();
        }

        public IEnumerable<EmpresaComboBoxDTO> BuscarParaComboBox()
        {
            EmpresaNfce empresaAlias = null;
            EmpresaComboBoxDTO resultado = null;

            var query = Sessao.QueryOver(() => empresaAlias)
                .SelectList(list => list.Select(() => empresaAlias.NomeFantasia).WithAlias(() => resultado.Nome)
                    .Select(() => empresaAlias.Id).WithAlias(() => resultado.Id));

            query.TransformUsing(Transformers.AliasToBean<EmpresaComboBoxDTO>());

            return query.List<EmpresaComboBoxDTO>();
        }
    }
}