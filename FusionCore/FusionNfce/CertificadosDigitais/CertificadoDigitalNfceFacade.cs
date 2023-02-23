using FusionCore.FusionNfce.Empresa;
using FusionCore.Sessao;

namespace FusionCore.FusionNfce.CertificadosDigitais
{
    public class CertificadoDigitalNfceFacade
    {
        public CertificadoDigitalNfce CarregarPorEmpresa(EmpresaNfce empresa)
        {
            using (var sessao = new SessaoManagerNfce().CriaSessao())
            {
                return new RepositorioCertificadoDigitalNfce(sessao).CarregarPorEmpresa(empresa);
            }
        }


        public void Salva(CertificadoDigitalNfce certificado)
        {
            using (var sessao = new SessaoManagerNfce().CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                new RepositorioCertificadoDigitalNfce(sessao).Salva(certificado);
                
                transacao.Commit();
            }
        }

        public void ExisteCertificadoDigital(EmpresaNfce empresa)
        {
            using (var sessao = new SessaoManagerNfce().CriaSessao())
            {
                if (new RepositorioCertificadoDigitalNfce(sessao).Existe(empresa) == false)
                {
                    throw new NaoExisteCertificadoDigitalException("Cadastre um certificado digital para esta empresa");
                }
            }
        }
    }
}