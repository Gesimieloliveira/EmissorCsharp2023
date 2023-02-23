using System;
using FusionCore.FusionAdm.Csrt;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioResponsavelTecnico : Repositorio<ResponsavelTecnico, Guid>
    {
        public RepositorioResponsavelTecnico(ISession sessao) : base(sessao)
        {
        }

        public void Persistir(ResponsavelTecnico responsavelTecnico)
        {
            Sessao.Save(responsavelTecnico);
        }

        public bool ExisteResponsavelTecnico(int ufId, TipoDocumentoFiscalEletronico tipoDocumentoFiscalEletronico)
        {
            ResponsavelTecnico responsavelTecnico = null;
            EstadoDTO estadoDTO = null;

            var query = Sessao.QueryOver(() => responsavelTecnico)
                .JoinAlias(() => responsavelTecnico.Uf, () => estadoDTO)
                .Where(() => estadoDTO.Id == ufId);

            WherePorTipoDocumento(tipoDocumentoFiscalEletronico, query, responsavelTecnico);

            return query.RowCount() > 0;
        }

        private static void WherePorTipoDocumento(TipoDocumentoFiscalEletronico tipoDocumentoFiscalEletronico,
            IQueryOver<ResponsavelTecnico, ResponsavelTecnico> query,
            ResponsavelTecnico responsavelTecnico)
        {
            switch (tipoDocumentoFiscalEletronico)
            {
                case TipoDocumentoFiscalEletronico.NFe:
                    query.Where(() => responsavelTecnico.IsNFe == true);
                    break;
                case TipoDocumentoFiscalEletronico.NFCe:
                    query.Where(() => responsavelTecnico.IsNFCe == true);
                    break;
                case TipoDocumentoFiscalEletronico.CTe:
                    query.Where(() => responsavelTecnico.IsCTe == true);
                    break;
                case TipoDocumentoFiscalEletronico.CTeOs:
                    query.Where(() => responsavelTecnico.IsCTeOs == true);
                    break;
                case TipoDocumentoFiscalEletronico.MDFe:
                    query.Where(() => responsavelTecnico.IsMDFe == true);
                    break;
                case TipoDocumentoFiscalEletronico.SAT:
                    throw new ArgumentOutOfRangeException(nameof(tipoDocumentoFiscalEletronico),
                        tipoDocumentoFiscalEletronico,
                        null);
                default:
                    throw new ArgumentOutOfRangeException(nameof(tipoDocumentoFiscalEletronico),
                        tipoDocumentoFiscalEletronico,
                        null);
            }
        }

        public bool ExisteCsrt(int ufId, TipoDocumentoFiscalEletronico tipoDocumentoFiscalEletronico)
        {
            ResponsavelTecnico responsavelTecnico = null;
            EstadoDTO estadoDTO = null;

            var query = Sessao.QueryOver(() => responsavelTecnico)
                .JoinAlias(() => responsavelTecnico.Uf, () => estadoDTO)
                .Where(() => estadoDTO.Id == ufId);

            WherePorTipoDocumento(tipoDocumentoFiscalEletronico, query, responsavelTecnico);

            var responsavel = query.SingleOrDefault<ResponsavelTecnico>();

            return responsavel != null && responsavel.Csrt.IsNotNullOrEmpty();
        }



        public ResponsavelTecnico BuscarPorUf(int ufId)
        {
            ResponsavelTecnico responsavelTecnico = null;
            EstadoDTO estadoDTO = null;

            return Sessao.QueryOver(() => responsavelTecnico).JoinAlias(() => responsavelTecnico.Uf, () => estadoDTO)
                .Where(() => estadoDTO.Id == ufId).SingleOrDefault<ResponsavelTecnico>();
        }

        public void DeletarTudo()
        {
            Sessao.Delete($"from {nameof(ResponsavelTecnico)}");
        }
    }
}