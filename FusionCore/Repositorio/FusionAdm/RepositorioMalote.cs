using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.Repositorio.Contratos;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioMalote : Repositorio<Malote, int>, IRepositorioMalote
    {
        public RepositorioMalote(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(Malote malote)
        {
            Sessao.SaveOrUpdate(malote);
        }

        public void Persiste(Malote malote)
        {
            ThrowExceptionSeNaoExisteTransacao();

            Sessao.Persist(malote);
            SalvarDocumentosReceber(malote.DocumentosReceber);
            SalvarDocumentosPagar(malote.DocumentosPagar);

            Sessao.Flush();
        }

        private void SalvarDocumentosReceber(IEnumerable<DocumentoReceber> documentos)
        {
            foreach (var documento in documentos)
            {
                if (documento.Id == 0)
                {
                    Sessao.Persist(documento);
                }
            }
        }

        private void SalvarDocumentosPagar(IEnumerable<DocumentoPagar> documentos)
        {
            foreach (var documento in documentos)
            {
                if (documento.Id == 0)
                {
                    Sessao.Persist(documento);
                }
            }
        }

        public Malote BuscarMalotePorOrigemUuid(string uuidVenda)
        {
            var query = Sessao.QueryOver<Malote>();

            query.Where(i => i.OrigemUuid == uuidVenda);

            return query.SingleOrDefault<Malote>();
        }

        public bool ExisteMalote(string uuidVenda)
        {
            var query = Sessao.QueryOver<Malote>();

            query.Where(i => i.OrigemUuid == uuidVenda);

            return query.RowCount() == 1;
        }
    }
}