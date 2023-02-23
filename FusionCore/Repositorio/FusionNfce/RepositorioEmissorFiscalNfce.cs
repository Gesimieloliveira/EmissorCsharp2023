using System.Collections.Generic;
using FusionCore.FusionNfce.EmissorFiscal;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioEmissorFiscalNfce : Repositorio<NfceEmissorFiscal, byte>, IRepositorioEmissorFiscalNfce
    {
        public RepositorioEmissorFiscalNfce(ISession sessao) : base(sessao)
        {
        }

        public void SalvarESincronizar(NfceEmissorFiscal emissorFiscal)
        {
            emissorFiscal.Sincronizado = false;
            Sessao.Merge(emissorFiscal);
        }

        public void SalvarENaoSincronizar(NfceEmissorFiscal emissorFiscal)
        {
            emissorFiscal.Sincronizado = true;
            Sessao.Merge(emissorFiscal);
        }

        public void Salva(NfceEmissorFiscal emissorFiscal)
        {
            Sessao.Merge(emissorFiscal);
        }

        public IEnumerable<NfceEmissorFiscal> BuscarTodosEmissoresSincronivaveis()
        {
            var queryOver = Sessao.QueryOver<NfceEmissorFiscal>().Where(n => n.Sincronizado == false);

            var lista = queryOver.List<NfceEmissorFiscal>();

            return lista;
        }

        public IEnumerable<NfceEmissorFiscal> BuscarTodos()
        {
            return Sessao.QueryOver<NfceEmissorFiscal>().List<NfceEmissorFiscal>();
        }
    }
}