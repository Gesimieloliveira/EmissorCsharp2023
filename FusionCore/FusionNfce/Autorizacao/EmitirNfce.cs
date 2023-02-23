using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FusionCore.DFe.RegrasNegocios.Chave;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.ConfigNumeroFiscal;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.TyneTypes;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using LoadChaveBuilder = FusionCore.Helpers.ChaveFiscal.ChaveSefazHelper;

namespace FusionCore.FusionNfce.Autorizacao
{
    public class EmitirNfce
    {
        private readonly X509Certificate2 _certificado;

        public EmitirNfce(X509Certificate2 certificado)
        {
            _certificado = certificado;
        }

        public NfceEmissaoHistorico Emite(Nfce nfce)
        {
            if (nfce.Emissao?.Autorizado == true)
            {
                throw new ArgumentException("NFCe não pode ser emitida; JÁ AUTORIZADA");
            }

            if (nfce.NumeroFiscal == 0)
                AlocaNumeracao(nfce);

            return GeraEmissaoHistorico(nfce);
        }

        private void AlocaNumeracao(Nfce nfce)
        {
            new AlocarNumeroFiscalNfce().Alocar(nfce
                , SessaoSistemaNfce.Configuracao.EmissorFiscal.EmissorFiscalNfce
                , SessaoSistemaNfce.TipoEmissao);
        }

        private NfceEmissaoHistorico GeraEmissaoHistorico(Nfce nfce)
        {
            var emissorFiscal = SessaoSistemaNfce.Configuracao.EmissorFiscal;

            var emissaoHistoricoBuilder = (NfceEmissaoHistorico) new NfceEmissaoHistorico.Builder()
                .ComAmbiente(emissorFiscal.Ambiente)
                .ComContingencia(new Contingencia.Builder().ComJustificativa(string.Empty).EntrouNaData(null))
                .ComVersao(Versao.V400)
                .ComNfce(nfce)
                .ComTentouEm(new TentouEm(DateTime.Now));


            if (TemContingencia())
            {
                emissaoHistoricoBuilder = Contingencia(emissaoHistoricoBuilder);
            }

            emissaoHistoricoBuilder = NfceRejeicaoOffline(nfce, emissaoHistoricoBuilder);

            var chaveBuilder = CriaChave(nfce, emissaoHistoricoBuilder);
            var chaveSefaz = ChaveSefazHelper.GerarChave(new ComponentesChaveNfce(chaveBuilder));

            GerarChaveFiscal.ValidarChave(chaveSefaz.Chave);

            var chaveCompleta = LoadChaveBuilder.LoadChave(chaveSefaz.Chave);

            emissaoHistoricoBuilder = emissaoHistoricoBuilder
                .ToBuilder()
                .ComChave(chaveCompleta)
                .ComChaveTexto(new ChaveTexto(chaveSefaz.Chave));


            return SalvarHistorico(nfce, emissaoHistoricoBuilder);
        }

        private static NfceEmissaoHistorico NfceRejeicaoOffline(Nfce nfce, NfceEmissaoHistorico emissaoHistoricoBuilder)
        {
            if (nfce.Status == Status.PendenteOffline)
            {
                using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
                {
                    var repositorio = new RepositorioNfce(sessao);

                    var ultimosHistorico = repositorio.BuscarHistoricoEmissao(nfce);

                    var ordenadoDesc = ultimosHistorico.OrderByDescending(n => n.Id).ToList();

                    if (ordenadoDesc.IsNotNullOrEmpty())
                    {
                        emissaoHistoricoBuilder = ordenadoDesc.FirstOrDefault() ??
                                                  throw new InvalidOperationException(
                                                      "Não foi possivel recuperar o ultimo historico");
                        emissaoHistoricoBuilder = emissaoHistoricoBuilder.ToBuilder(true).ComId(0).NaoFinalizou();
                    }
                }
            }
            return emissaoHistoricoBuilder;
        }

        private NfceEmissaoHistorico SalvarHistorico(Nfce nfce, NfceEmissaoHistorico emissaoHistoricoBuilder)
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var emissaoHistoricoComXmlEnvio = new AssinadorNFCe(_certificado, emissaoHistoricoBuilder, nfce).Assinar();

                emissaoHistoricoComXmlEnvio = SalvarEmissaoHistorico(emissaoHistoricoComXmlEnvio, sessao, nfce);

                transacao.Commit();
                return emissaoHistoricoComXmlEnvio;
            }
        }

        private NfceEmissaoHistorico Contingencia(NfceEmissaoHistorico emissaoHistoricoBuilder)
        {
            return emissaoHistoricoBuilder.ToBuilder().ComContingencia(
                    new Contingencia.Builder().ComJustificativa(SessaoSistemaNfce.Contingencia.Motivo).EntrouNaData(SessaoSistemaNfce.Contingencia.EntrouEm));
        }

        private bool TemContingencia()
        {
            return SessaoSistemaNfce.TipoEmissao != TipoEmissao.Normal;
        }

        private NfceEmissaoHistorico SalvarEmissaoHistorico(NfceEmissaoHistorico emissaoHistorico, ISession sessao, Nfce nfce)
        {
            var repositorio = new RepositorioNfce(sessao);

            repositorio.SalvarHistorico(emissaoHistorico);
            var emissaoHistoricoCriado = new NfceEmissaoHistorico.Builder(emissaoHistorico);

            return emissaoHistoricoCriado;
        }

        private Chave.Builder CriaChave(Nfce nfce, NfceEmissaoHistorico emissaoHistoricoBuilder)
        {

            var codigoNumerico = nfce.CodigoNumerico;

            var builder = new Chave.Builder()
                .ComAnoMes(DateTime.Now)
                .ComCnpjEmitente(new CnpjEmitente(nfce.CnpjCpfEmitente))
                .ComCodigoIbgeUf(nfce.Emitente.Empresa.Estado.CodigoIbge)
                .ComCodigoNumerico(new CodigoNumerico(codigoNumerico))
                .ComFormaEmissao(SessaoSistemaNfce.TipoEmissao)
                .ComModeloDocumento(ModeloDocumento.NFCe)
                .ComNumeroFiscal(new NumeroFiscal(nfce.NumeroFiscal))
                .ComSerie(new Serie(nfce.Serie))
                .ComDigitoVerificador(new DigitoVerificador(0));


            if (nfce.Status != Status.PendenteOffline) return builder;

            builder = builder.ComCodigoNumerico(emissaoHistoricoBuilder.Chave.CodigoNumerico);
            builder = builder.ComAnoMes(emissaoHistoricoBuilder.Chave.AnoMes);
            builder = builder.ComFormaEmissao(emissaoHistoricoBuilder.Chave.FormaEmissao);
            builder = builder.ComNumeroFiscal(emissaoHistoricoBuilder.Chave.NumeroFiscal);
            builder = builder.ComSerie(emissaoHistoricoBuilder.Chave.Serie);

            return builder;
        }
    }
}