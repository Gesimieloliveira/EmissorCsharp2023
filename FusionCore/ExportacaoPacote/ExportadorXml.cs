using System;
using System.Collections.Generic;
using System.IO;
using FusionCore.ExportacaoPacote.AmazonNuvemS3;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Validacao;
using FusionLibrary.Validacao.Regras;
using NHibernate.Util;

namespace FusionCore.ExportacaoPacote
{
    public class ExportadorXml
    {
        private readonly IRepositorioExportacaoXml _repositorio;
        private readonly TipoDocumentoFiscalEletronico _documentoFiscalEletronico;
        private IList<IEnvelope> _envelopes;
        private FileInfo _pacoteGerado;
        private ConfiguracaoEmailDTO _configuracaoEmail;
        private readonly Empacotador _empacotador;

        public ExportadorXml(IRepositorioExportacaoXml repositorio,
            TipoDocumentoFiscalEletronico documentoFiscalEletronico)
        {
            _repositorio = repositorio;
            _documentoFiscalEletronico = documentoFiscalEletronico;
            _empacotador = new Empacotador();
        }

        public void Filtrar(DateTime inicio, DateTime final, EmpresaDTO empresa)
        {
            if (final < inicio)
            {
                throw new InvalidOperationException("Filtro inválido: data final menor que data inicial");
            }

            var xmls = _repositorio.BuscarXmlExportacao(inicio, final, empresa);
            _envelopes = new List<IEnvelope>(xmls);
        }

        public void GeraPacote()
        {
            if (_envelopes == null || !_envelopes.Any())
            {
                throw new InvalidOperationException("Filtro não retornou nenhum resultado");
            }

            TrataDocumento();

            _empacotador.ComEnvelopes(_envelopes);
            _pacoteGerado = _empacotador.GeraPacote();
        }

        private void TrataDocumento()
        {
            if (_documentoFiscalEletronico != TipoDocumentoFiscalEletronico.NFe &&
                _documentoFiscalEletronico != TipoDocumentoFiscalEletronico.NFCe && 
                _documentoFiscalEletronico != TipoDocumentoFiscalEletronico.NotaFiscalCompra) return;

            foreach (var envelope in _envelopes)
            {
                envelope.Conteudo = envelope.Conteudo.Replace("<NFe>",
                    "<NFe xmlns=\"http://www.portalfiscal.inf.br/nfe\">");
            }
        }

        public void ArmazenaZipEmDisco(string destino)
        {
            ThrowExceptionSeNaoGerouPacote();

            if (new ArquivoExtensaoZip().NaoValido(destino))
                throw new ArgumentException("Destino precisa ter a extensão .zip");

            if (File.Exists(destino))
                File.Delete(destino);

            _pacoteGerado.CopyTo(destino);
        }

        private void ThrowExceptionSeNaoGerouPacote()
        {
            if (_pacoteGerado == null || !_pacoteGerado.Exists)
                throw new InvalidOperationException("Nenhum pacote gerado");
        }

        public void SetConfiguracaoEmail(ConfiguracaoEmailDTO configuracaoEmail)
        {
            _configuracaoEmail = configuracaoEmail;
        }

        public void EnviaPorEmail(string email, string assunto)
        {
            ThrowExceptionSeNaoGerouPacote();

            var anexo = GetAnexo();

            //TODO 1612 - para enviar com S3 altere aqui para True
            if (false)
            {
                var urlS3ArquivoZip = EnviaNuvemAwsS3.UploadArquivoZip(anexo.FullName);

                var builder = new EmailBuilder(_configuracaoEmail);

                builder.AddDestinatario(email)
                    .Assunto(assunto)
                    .Mensagem($"<p>Utilize o link abaixo para baixar o pacote de xml.</p><a href=\"{urlS3ArquivoZip}\">Clique aqui para efetuar o download do pacote xml</a>")
                    .Enviar();
            }
            else
            {
                var builder = new EmailBuilder(_configuracaoEmail);

                builder.AddDestinatario(email)
                    .Assunto(assunto)
                    .Mensagem("<p>Pacote XML em Anexo</a>")
                    .AddAnexo(anexo.FullName)
                    .Enviar();
            }

            

            
        }

        private FileInfo GetAnexo()
        {
            var anexo = Path.Combine(Path.GetTempPath(), "pacote-de-xml.zip");
            File.Delete(anexo);

            return _pacoteGerado.CopyTo(anexo);
        }
    }
}