using System;
using System.Linq;
using FusionCore.CadastroUsuario;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Financeiro.Repositorios;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.FusionNfce.Servicos
{
    public class ServicoControleFinanceiroNfce : IDisposable
    {
        private ISession _sessao;
        private readonly IUsuario _usuario;

        public ServicoControleFinanceiroNfce(IUsuario usuario)
        {
            _usuario = usuario;
        }

        public void GerarFinanceiroParaNfce(Nfce nfce)
        {
            try
            {
                if (nfce.ExisteCobrancaQueGeraFinancerio() == false)
                {
                    return;
                }

                IniciarConexaoComServidor();

                var repositorioMalote = new RepositorioMalote(_sessao);

                var uuidVendaNfce = nfce.UuidVenda;

                var jaGerouFinanceiro = repositorioMalote.ExisteMalote(uuidVendaNfce);

                if (jaGerouFinanceiro) return;

                var usuarioGeracao = _sessao.Get<UsuarioDTO>(_usuario.Id);
                var malote = Malote.Cria(OrigemDocumento.Nfce, uuidVendaNfce, usuarioGeracao, 0.00M);

                foreach (var cob in nfce.Cobranca.CobrancaDuplicatas)
                {
                    var doc = new DocumentoReceber
                    {
                        UsuarioCriacao = usuarioGeracao,
                        ValorDocumento = cob.Valor,
                        ValorOriginal = cob.Valor,
                        EmitidoEm = DateTime.Now,
                        Parcela = cob.NumeroDuplicata,
                        Vencimento = cob.VenceEm.Value,
                        Empresa = _sessao.Get<EmpresaDTO>(nfce.Emitente.Empresa.Id),
                        Cliente = _sessao.Get<FusionAdm.Pessoas.Cliente>(nfce.Destinatario.Cliente.Id),
                        TipoDocumento = nfce.Cobranca.TipoDocumento,
                        Malote = malote
                    };

                    malote.DocumentosReceber.Add(doc);
                }

                repositorioMalote.Persiste(malote);
            }
            catch (Exception e)
            {
                throw new FinanceiroServidorException($"Não foi possível gerar os documentos a receber: {e.Message}", e);
            }
        }

        private void IniciarConexaoComServidor()
        {
            if (_sessao?.IsOpen == true )
            {
                return;
            }

            try
            {
                _sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao();
                _sessao.BeginTransaction();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Falha ao conectar com o servidor {e.Message}", e);
            }
        }

        public void CancelarFinanceiroNfce(Nfce nfce)
        {
            if (nfce.ExisteCobrancaQueGeraFinancerio() == false)
            {
                return;
            }

            try
            {
                IniciarConexaoComServidor();

                var usuarioAcao = _sessao.Get<UsuarioDTO>(_usuario.Id);

                var repositorioMalote = new RepositorioMalote(_sessao);
                var repositorioReceber = new RepositorioDocumentoReceber(_sessao);

                var malote = repositorioMalote.BuscarMalotePorOrigemUuid(nfce.UuidVenda);
                var documentos = repositorioReceber.BuscarPeloMalote(malote);

                foreach (var d in documentos)
                {
                    if (d.Lancamentos.Any(i => i.Estornado == false))
                    {
                        throw new InvalidOperationException(
                            $"Documento a Receber com #ID {d.Id} possui lançamentos não estornados");
                    }
                }

                foreach (var d in documentos)
                {
                    if (d.EstaCancelado()) continue;

                    d.CancelarDocumento(usuarioAcao);

                    repositorioReceber.SalvarAlteracoes(d);
                }
            }
            catch (Exception e)
            {
                throw new FinanceiroServidorException($"Não foi possível verificar se o cupom possui documentos a receber para cancela-los: {e.Message}", e);
            }
        }

        public void ComitarAlteracoes()
        {
            try
            {
                _sessao?.Transaction.Commit();
            }
            catch (Exception e)
            {
                throw new FinanceiroServidorException(e.Message, e);
            }
        }

        public void Dispose()
        {
            try
            {
                if (_sessao?.Transaction.IsActive == true)
                {
                    _sessao.Transaction.Rollback();
                }

                _sessao?.Dispose();
            }
            catch (Exception e)
            {
                throw new FinanceiroServidorException(e.Message, e);
            }
        }
    }
}