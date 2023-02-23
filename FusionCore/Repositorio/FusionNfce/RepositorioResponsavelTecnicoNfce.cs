using System;
using FusionCore.FusionNfce.Csrt;
using FusionCore.FusionNfce.Uf;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioResponsavelTecnicoNfce : Repositorio<ResponsavelTecnicoNfce, Guid>
    {
        public RepositorioResponsavelTecnicoNfce(ISession sessao) : base(sessao)
        {
        }

        public void DeletarTodos()
        {
            Sessao.Delete($"from {nameof(ResponsavelTecnicoNfce)}");
            Sessao.Flush();
        }

        public void SalvarOuAtualizar(ResponsavelTecnicoNfce responsavelTecnico)
        {
            Sessao.SaveOrUpdate(responsavelTecnico);
        }

        public bool ExisteResponsavelTecnico(byte ufId)
        {
            ResponsavelTecnicoNfce responsavelTecnico = null;
            UfNfce estadoDTO = null;

            return Sessao.QueryOver(() => responsavelTecnico).JoinAlias(() => responsavelTecnico.Uf, () => estadoDTO)
                       .Where(() => estadoDTO.Id == ufId).RowCount() > 0;
        }

        public bool ExisteCsrt(int ufId)
        {
            ResponsavelTecnicoNfce responsavelTecnico = null;
            UfNfce estadoDTO = null;

            var responsavel = Sessao.QueryOver(() => responsavelTecnico)
                .JoinAlias(() => responsavelTecnico.Uf, () => estadoDTO)
                .Where(() => estadoDTO.Id == ufId).SingleOrDefault<ResponsavelTecnicoNfce>();

            return responsavel != null && responsavel.Csrt.IsNotNullOrEmpty();
        }

        public ResponsavelTecnicoNfce BuscarPorUf(int ufId)
        {
            ResponsavelTecnicoNfce responsavelTecnico = null;
            UfNfce estadoDTO = null;

            return Sessao.QueryOver(() => responsavelTecnico).JoinAlias(() => responsavelTecnico.Uf, () => estadoDTO)
                .Where(() => estadoDTO.Id == ufId).SingleOrDefault<ResponsavelTecnicoNfce>();
        }
    }
}