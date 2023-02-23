using System.Security.Cryptography.X509Certificates;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.NotaFiscalEletronica.Emissao
{
    public class EmissorModel : ModelBase
    {
        private TipoAmbiente _ambiente;
        private int _serie;

        public TipoAmbiente Ambiente
        {
            get { return _ambiente; }
            set
            {
                if (value == _ambiente) return;
                _ambiente = value;
                PropriedadeAlterada();
            }
        }

        public int Serie
        {
            get { return _serie; }
            set
            {
                if (value == _serie) return;
                _serie = value;
                PropriedadeAlterada();
            }
        }

        public X509Certificate2 Certificado { get; set; }
        public EmissorFiscal Emissor { get; set; }

        public void CarregaEmissorPeloPefil(int perfilId)
        {
            Emissor = GetEmissor(perfilId);
            Certificado = CertificadoDigitalFactory.Cria(Emissor);
            Ambiente = Emissor.EmissorFiscalNfe.Ambiente;
            Serie = Emissor.EmissorFiscalNfe.Serie;
        }

        private static EmissorFiscal GetEmissor(int perfilId)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var rPerfil = new RepositorioPerfilNfe(sessao);
                var perfil = rPerfil.GetPeloId(short.Parse(perfilId.ToString()));
                return perfil.EmissorFiscal;
            }
        }
    }
}