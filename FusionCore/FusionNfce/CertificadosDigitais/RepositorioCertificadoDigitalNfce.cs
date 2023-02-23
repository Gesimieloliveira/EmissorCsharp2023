using FusionCore.FusionNfce.Empresa;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.FusionNfce.CertificadosDigitais
{
    public class RepositorioCertificadoDigitalNfce : Repositorio<CertificadoDigitalNfce, int>
    {
        public RepositorioCertificadoDigitalNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salva(CertificadoDigitalNfce certificadoDigital)
        {
            Sessao.SaveOrUpdate(certificadoDigital);
        }

        public CertificadoDigitalNfce CarregarPorEmpresa(EmpresaNfce empresa)
        {
            return Sessao.QueryOver<CertificadoDigitalNfce>().Where(x => x.Empresa == empresa)
                .SingleOrDefault<CertificadoDigitalNfce>();
        }

        public bool Existe(EmpresaNfce empresa)
        {
            return Sessao.QueryOver<CertificadoDigitalNfce>().Where(x => x.Empresa == empresa).RowCount() > 0;
        }
    }
}