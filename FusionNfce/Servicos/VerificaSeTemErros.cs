using System.Collections.Generic;
using System.Windows.Input;
using FontAwesome.WPF;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Repositorio.FusionNfce;
using FusionNfce.Visao.Avisos;

namespace FusionNfce.Servicos
{
    public class VerificaSeTemErros
    {
        private readonly List<Aviso> _avisos; 
        public VerificaSeTemErros()
        {
            _avisos = new List<Aviso>();
        }

        public IList<Nfce> ListaDeNfces { get; set; }

        private bool _temErros;

        public bool ExecutaVerificacao()
        {
            _temErros = VerificaSeTemNFCeOffline();

            return _temErros;
        }

        private bool VerificaSeTemNFCeOffline()
        {
            int quantidade;

            using (var repositorio = new RepositorioNfce(GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao()))
            {
                quantidade = new QuantidadeDeNFCeComErros(repositorio)
                .ObterQuantidade();
            }
            

            if (quantidade > 0)
            {
                AddAviso("Existem " + quantidade + " NFC-e que foram emitida em modo offline que precisam ser enviadas, vamos enviar para você, logo retorno", FontAwesomeIcon.Warning);

                var repositorioNfce =
                    new RepositorioNfce(GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao());

                var lista = repositorioNfce.NfceEmitidaOffline();

                ListaDeNfces = lista;
            }

            return quantidade > 0;
        }

        public void VerificaSeTemNFCeOfflineComErrosDeTransmissao()
        {
            using (var repositorio = new RepositorioNfce(GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao()))
            {
                ListaDeNfcesComErros = repositorio.BuscaNfceOfflineComErrosNaEmissao();
            }

            if (ListaDeNfcesComErros.Count > 0)
                AddAviso("Existem " + ListaDeNfcesComErros.Count + " NFC-e que foram emitida em modo offline que estão com erros e precisam de correção nos erros", FontAwesomeIcon.Warning);
        }

        public IList<Nfce> ListaDeNfcesComErros { get; set; }

        public IList<Aviso> ObterAvisos()
        {
            return _avisos;
        } 

        private void AddAviso(string mensagem, FontAwesomeIcon icone, ICommand action = null)
        {
            _avisos.Insert(0, new Aviso
            {
                Action = action,
                Mensagem = mensagem,
                Icone = icone
            });
        }
    }
}