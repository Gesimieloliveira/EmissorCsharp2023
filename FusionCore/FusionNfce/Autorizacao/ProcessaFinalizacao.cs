using System;
using DFe.Utils;
using FusionCore.FusionAdm.Fiscal.NF.EnviaLote;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.TyneTypes;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using NFe.Classes.Servicos.Consulta;
using NFe.Utils.Consulta;

namespace FusionCore.FusionNfce.Autorizacao
{
    public class ServicoProcessaFinalizacaoNFCe
    {
        private readonly Configuracao _configuracao;

        public ServicoProcessaFinalizacaoNFCe(Configuracao configuracao)
        {
            _configuracao = configuracao;
        }


        public void Finaliza()
        {
            FinalizaHistoricoCorrente();
            FinalizarEmissao();
            FinalizarNFCe();
        }


        private void FinalizaHistoricoCorrente()
        {
            var emissaoHistoricoFinalizado = _configuracao.NfceEmissaoHistorico.ToBuilder();


            if (_configuracao.ResultadoConsulta != null)
            {
                emissaoHistoricoFinalizado = emissaoHistoricoFinalizado.ComXmlDeRetorno(
                        new XmlRetorno(_configuracao.ResultadoConsulta.ObterXmlString()))
                    .ComCodigoDeAutorizacao(
                        new CodigoAutorizacao((short) _configuracao.ResultadoConsulta.protNFe.infProt.cStat))
                    .ComMotivo(new Motivo(_configuracao.ResultadoConsulta.protNFe.infProt.xMotivo ?? string.Empty));
            }

            if (_configuracao.ProtocoloRecebimento != null)
            {
                emissaoHistoricoFinalizado = emissaoHistoricoFinalizado.ComXmlDeRetorno(
                        new XmlRetorno(FuncoesXml.ClasseParaXmlString(_configuracao.ProtocoloRecebimento.InformacaoProtocoloResposta)))
                    .ComCodigoDeAutorizacao(
                        new CodigoAutorizacao((short) _configuracao.ProtocoloRecebimento.InformacaoProtocoloResposta.CodigoStatus))
                    .ComMotivo(new Motivo(_configuracao.ProtocoloRecebimento.InformacaoProtocoloResposta.Motivo ?? string.Empty));
            }
            
            emissaoHistoricoFinalizado = emissaoHistoricoFinalizado.Finalizar();

            _configuracao.RepositorioNfce.SalvarHistorico(emissaoHistoricoFinalizado);
            _configuracao.NfceEmissaoHistorico = emissaoHistoricoFinalizado;
        }

        private void FinalizarEmissao()
        {
            _configuracao.Nfce.Emissao = new NfceEmissao(
                _configuracao.Nfce
                , _configuracao.Nfce.NumeroFiscal
                , _configuracao.Nfce.Serie
                , _configuracao.NfceEmissaoHistorico.AmbienteSefaz
            );

            var emissao = _configuracao.Nfce.Emissao;

            if (_configuracao.ResultadoConsulta != null)
            {
                emissao.Autorizado = true;
                emissao.RecebidoEm = _configuracao.ResultadoConsulta.protNFe.infProt.dhRecbto.DateTime;
                emissao.VersaoAplicativoAutorizacao = _configuracao.ResultadoConsulta.verAplic ?? string.Empty;
                emissao.DigestValue = _configuracao.ResultadoConsulta.protNFe.infProt.digVal ?? string.Empty;
                emissao.Protocolo = _configuracao.ResultadoConsulta.protNFe.infProt.nProt ?? string.Empty;
            }

            if (_configuracao.ProtocoloRecebimento != null)
            {
                emissao.Autorizado = true;
                emissao.RecebidoEm = _configuracao.ProtocoloRecebimento.InformacaoProtocoloResposta.DataEHoraDoProcessamento;
                emissao.VersaoAplicativoAutorizacao = _configuracao.ProtocoloRecebimento.InformacaoProtocoloResposta.VersaoAplicacao ?? string.Empty;
                emissao.DigestValue = _configuracao.ProtocoloRecebimento.InformacaoProtocoloResposta.DigestValue ?? string.Empty;
                emissao.Protocolo = _configuracao.ProtocoloRecebimento.InformacaoProtocoloResposta.NumeroProtocolo ?? string.Empty;
                
            }

            emissao.EmissorFiscal = SessaoSistemaNfce.Configuracao.EmissorFiscal;
            emissao.TagId = $"NFe{_configuracao.NfceEmissaoHistorico.ChaveTexto.Valor}";
            emissao.VersaoAplicativo = ResponsavelLegal.VersaoSistema;
            emissao.TipoEmissao = SessaoSistemaNfce.TipoEmissao;
            emissao.TipoAmbiente = _configuracao.NfceEmissaoHistorico.AmbienteSefaz;
            emissao.CodigoAutorizacao = _configuracao.NfceEmissaoHistorico.CodigoAutorizacao.Valor;
            emissao.Chave = _configuracao.NfceEmissaoHistorico.ChaveTexto.Valor;
            emissao.CodigoNumerico = _configuracao.NfceEmissaoHistorico.Chave.CodigoNumerico.Valor;
            emissao.EntrouEmContingenciaEm = _configuracao.NfceEmissaoHistorico.Contingencia.EntrouEm;
            emissao.JustificativaContingencia = _configuracao.NfceEmissaoHistorico.Contingencia.Justificativa ?? string.Empty;

            emissao.AutorizaXml(_configuracao.NfceEmissaoHistorico);

            _configuracao.RepositorioNfce.SalvarEmissao(_configuracao.Nfce.Emissao);
        }

        private void FinalizarNFCe()
        {
            _configuracao.Nfce.TransmitidaComSucesso();
            _configuracao.RepositorioNfce.SalvarESincronizar(_configuracao.Nfce);
        }

        public class Configuracao
        {
            public Configuracao(IRepositorioNfce repositorioNfce, NfceEmissaoHistorico nfceEmissaoHistorico, Nfce nfce, retConsSitNFe resultadoConsulta)
            {
                RepositorioNfce = repositorioNfce;
                NfceEmissaoHistorico = nfceEmissaoHistorico;
                Nfce = nfce;
                ResultadoConsulta = resultadoConsulta;
            }

            public Configuracao(IRepositorioNfce repositorioNfce,
                NfceEmissaoHistorico nfceEmissaoHistorico,
                Nfce nfce,
                ProtocoloRecebimentoNfe resultadoConsulta)
            {
                RepositorioNfce = repositorioNfce;
                NfceEmissaoHistorico = nfceEmissaoHistorico;
                Nfce = nfce;
                ProtocoloRecebimento = resultadoConsulta;
            }

            public ProtocoloRecebimentoNfe ProtocoloRecebimento { get;}

            public IRepositorioNfce RepositorioNfce { get; }
            public NfceEmissaoHistorico NfceEmissaoHistorico { get; set; }
            public Nfce Nfce { get; }
            public retConsSitNFe ResultadoConsulta { get; }
        }

        private static void IgnoraException(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception)
            {
                //igonre
            }
        }
    }
}