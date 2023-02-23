using System.Linq;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Usuario;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;

namespace FusionCore.FusionNfce.Autorizacao
{
    public class GeraRegistroCaixa
    {
        private readonly Nfce _nfce;
        private readonly ISession _sessao;
        private readonly UsuarioNfce _usuario;

        public GeraRegistroCaixa(Nfce nfce, ISession sessao, UsuarioNfce usuario)
        {
            _nfce = nfce;
            _sessao = sessao;
            _usuario = usuario;
        }

        public void RegistrarCaixa()
        {
            if (IsGerouRegistroCaixa()) return;

            new ServicoRegistroDeCaixa(_sessao, ELocalEventoCaixa.Terminal).RegistrarVenda(_nfce, _usuario);

            foreach (var pgNfce in _nfce.ObterFormaPagamentoNfces())
            {
                new RepositorioNfce(_sessao).SalvarFormaPagamento(pgNfce);
            }
        }

        public void EstornarCaixa()
        {
            if (IsGerouRegistroCaixa())
                new ServicoRegistroDeCaixa(_sessao, ELocalEventoCaixa.Terminal).RegistrarEstorno(_nfce, _usuario);
        }

        private bool IsGerouRegistroCaixa()
        {
            if (_nfce.ObterFormaPagamentoNfces() == null || _nfce.ObterFormaPagamentoNfces().Count == 0)
            {
                return false;
            }

            return _nfce.ObterFormaPagamentoNfces().Any(x => x.IsGerouRegistroCaixa == true);
        }
    }
}