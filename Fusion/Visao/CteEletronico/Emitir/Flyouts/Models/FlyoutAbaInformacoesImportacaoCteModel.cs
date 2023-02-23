using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using DFe.Classes.Extensoes;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Emitente;
using NFe.Utils.NFe;
using NFeZeus = NFe.Classes.NFe;

namespace Fusion.Visao.CteEletronico.Emitir.Flyouts.Models
{
    public class RetornoImportacaoXmlCteEventArgs : EventArgs
    {
        public string Chave { get; set; }
        public decimal Total { get; set; }
        public PessoaEntidade PessoaDestinatario { get; set; }
        public PessoaEntidade PessoaEmitente { get; set; }
        public bool IsImportarDocumentoNFe { get; set; }
        public bool IsDestinatarioRemetente { get; set; }
        public bool IsDestinatarioDestinatario { get; set; }
        public bool IsDestinatarioExpedidor { get; set; }
        public bool IsDestinatarioRecebedor { get; set; }
        public bool IsDestinatarioTomador { get; set; }
        public bool IsEmitenteRemetente { get; set; }
        public bool IsEmitenteDestinatario { get; set; }
        public bool IsEmitenteExpedidor { get; set; }
        public bool IsEmitenteRecebedor { get; set; }
        public bool IsEmitenteTomador { get; set; }
    }

    public sealed class FlyoutAbaInformacoesImportacaoCteModel : ViewModel
    {
        private bool _isOpen;
        private string _chave;
        private decimal _total;
        private string _nomeEmitente;
        private string _documentoUnico;
        private string _inscricaoEstadual;
        private string _cepEmitente;
        private string _logradouroEmitente;
        private string _bairroEmitente;
        private string _nomeDestinatario;
        private string _documentoUnicoDestinatario;
        private string _inscricaoEstadualDestinatario;
        private string _destinatarioCep;
        private string _destinatarioLogradouro;
        private string _destinatarioNumero;
        private string _destinatarioBairro;
        private string _destinatarioTelefone;
        private string _destinatarioUF;
        private string _destinatarioCidade;
        private string _regimeTributario;
        private bool _destinatarioDestinatarioIsCheck;
        private bool _emitenteDestinatarioIsCheck;
        private bool _emitenteRemetenteIsCheck;
        private bool _destinatarioRemetenteIsCheck;
        private bool _emitenteExpedidorIsCheck;
        private bool _destinatarioExpedidorIsCheck;
        private bool _emitenteRecebedorIsCheck;
        private bool _destinatarioRecebedorIsCheck;
        private bool _emitenteTomadorIsCheck;
        private bool _destinatarioTomadorIsCheck;
        private bool _importarDocumentoNFe;
        private PessoaEntidade _pessoaDestinatario;
        private PessoaEntidade _pessoaEmitente;
        private string _destinatarioComplemento;

        public ICommand CommandAdicionarImportacao => GetSimpleCommand(AdicionarImportacaoAction);

        public bool ImportarDocumentoNFe
        {
            get { return _importarDocumentoNFe; }
            set
            {
                if (value == _importarDocumentoNFe) return;
                _importarDocumentoNFe = value;
                PropriedadeAlterada();
            }
        }

        public bool EmitenteTomadorIsCheck
        {
            get { return _emitenteTomadorIsCheck; }
            set
            {
                _emitenteTomadorIsCheck = value;
                _destinatarioTomadorIsCheck = false;
                PropriedadeAlterada();
                PropriedadeAlterada(nameof(DestinatarioTomadorIsCheck));
            }
        }

        public bool DestinatarioTomadorIsCheck
        {
            get { return _destinatarioTomadorIsCheck; }
            set
            {
                _destinatarioTomadorIsCheck = value;
                _emitenteTomadorIsCheck = false;
                PropriedadeAlterada();
                PropriedadeAlterada(nameof(EmitenteTomadorIsCheck));
            }
        }

        public bool EmitenteRecebedorIsCheck
        {
            get { return _emitenteRecebedorIsCheck; }
            set
            {
                if (value == _emitenteRecebedorIsCheck) return;
                _emitenteRecebedorIsCheck = value;
                _destinatarioRecebedorIsCheck = false;
                PropriedadeAlterada();
                PropriedadeAlterada(nameof(DestinatarioRecebedorIsCheck));
            }
        }

        public bool DestinatarioRecebedorIsCheck
        {
            get { return _destinatarioRecebedorIsCheck; }
            set
            {
                if (value == _destinatarioRecebedorIsCheck) return;
                _destinatarioRecebedorIsCheck = value;
                _emitenteRecebedorIsCheck = false;
                PropriedadeAlterada();
                PropriedadeAlterada(nameof(EmitenteRecebedorIsCheck));
            }
        }

        public bool EmitenteExpedidorIsCheck
        {
            get { return _emitenteExpedidorIsCheck; }
            set
            {
                if (value == _emitenteExpedidorIsCheck) return;
                _emitenteExpedidorIsCheck = value;
                _destinatarioExpedidorIsCheck = false;
                PropriedadeAlterada();
                PropriedadeAlterada(nameof(DestinatarioExpedidorIsCheck));
            }
        }

        public bool DestinatarioExpedidorIsCheck
        {
            get { return _destinatarioExpedidorIsCheck; }
            set
            {
                if (value == _destinatarioExpedidorIsCheck) return;
                _destinatarioExpedidorIsCheck = value;
                _emitenteExpedidorIsCheck = false;
                PropriedadeAlterada();
                PropriedadeAlterada(nameof(EmitenteExpedidorIsCheck));
            }
        }

        public bool EmitenteRemetenteIsCheck
        {
            get { return _emitenteRemetenteIsCheck; }
            set
            {
                if (value == _emitenteRemetenteIsCheck) return;
                _emitenteRemetenteIsCheck = value;
                _destinatarioRemetenteIsCheck = false;
                PropriedadeAlterada();
                PropriedadeAlterada(nameof(DestinatarioRemetenteIsCheck));
            }
        }

        public bool DestinatarioRemetenteIsCheck
        {
            get { return _destinatarioRemetenteIsCheck; }
            set
            {
                if (value == _destinatarioRemetenteIsCheck) return;
                _destinatarioRemetenteIsCheck = value;
                _emitenteRemetenteIsCheck = false;
                PropriedadeAlterada();
                PropriedadeAlterada(nameof(EmitenteRemetenteIsCheck));
            }
        }

        public bool EmitenteDestinatarioIsCheck
        {
            get { return _emitenteDestinatarioIsCheck; }
            set
            {
                if (value == _emitenteDestinatarioIsCheck) return;
                _emitenteDestinatarioIsCheck = value;
                _destinatarioDestinatarioIsCheck = false;
                PropriedadeAlterada();
                PropriedadeAlterada(nameof(DestinatarioDestinatarioIsCheck));
            }
        }

        public bool DestinatarioDestinatarioIsCheck
        {
            get { return _destinatarioDestinatarioIsCheck; }
            set
            {
                if (value == _destinatarioDestinatarioIsCheck) return;
                _destinatarioDestinatarioIsCheck = value;
                _emitenteDestinatarioIsCheck = false;
                PropriedadeAlterada();
                PropriedadeAlterada(nameof(EmitenteDestinatarioIsCheck));
            }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandImportarXml => GetSimpleCommand(ImportarXmlAction);

        public string DestinatarioCidade
        {
            get { return _destinatarioCidade; }
            set
            {
                if (value == _destinatarioCidade) return;
                _destinatarioCidade = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioUF
        {
            get { return _destinatarioUF; }
            set
            {
                if (value == _destinatarioUF) return;
                _destinatarioUF = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioTelefone
        {
            get { return _destinatarioTelefone; }
            set
            {
                if (value == _destinatarioTelefone) return;
                _destinatarioTelefone = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioBairro
        {
            get { return _destinatarioBairro; }
            set
            {
                if (value == _destinatarioBairro) return;
                _destinatarioBairro = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioNumero
        {
            get { return _destinatarioNumero; }
            set
            {
                if (value == _destinatarioNumero) return;
                _destinatarioNumero = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioLogradouro
        {
            get { return _destinatarioLogradouro; }
            set
            {
                if (value == _destinatarioLogradouro) return;
                _destinatarioLogradouro = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioCep
        {
            get { return _destinatarioCep; }
            set
            {
                if (value == _destinatarioCep) return;
                _destinatarioCep = value;
                PropriedadeAlterada();
            }
        }

        public string InscricaoEstadualDestinatario
        {
            get { return _inscricaoEstadualDestinatario; }
            set
            {
                if (value == _inscricaoEstadualDestinatario) return;
                _inscricaoEstadualDestinatario = value;
                PropriedadeAlterada();
            }
        }

        public string DocumentoUnicoDestinatario
        {
            get { return _documentoUnicoDestinatario; }
            set
            {
                if (value == _documentoUnicoDestinatario) return;
                _documentoUnicoDestinatario = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioComplemento
        {
            get { return _destinatarioComplemento; }
            set
            {
                if (value == _destinatarioComplemento) return;
                _destinatarioComplemento = value;
                PropriedadeAlterada();
            }
        }

        public string NomeDestinatario
        {
            get { return _nomeDestinatario; }
            set
            {
                if (value == _nomeDestinatario) return;
                _nomeDestinatario = value;
                PropriedadeAlterada();
            }
        }

        public string RegimeTributario
        {
            get { return _regimeTributario; }
            set
            {
                if (value == _regimeTributario) return;
                _regimeTributario = value;
                PropriedadeAlterada();
            }
        }

        public string BairroEmitente
        {
            get { return _bairroEmitente; }
            set
            {
                if (value == _bairroEmitente) return;
                _bairroEmitente = value;
                PropriedadeAlterada();
            }
        }

        public string LogradouroEmitente
        {
            get { return _logradouroEmitente; }
            set
            {
                if (value == _logradouroEmitente) return;
                _logradouroEmitente = value;
                PropriedadeAlterada();
            }
        }

        public string CepEmitente
        {
            get { return _cepEmitente; }
            set
            {
                if (value == _cepEmitente) return;
                _cepEmitente = value;
                PropriedadeAlterada();
            }
        }

        public string InscricaoEstadual
        {
            get { return _inscricaoEstadual; }
            set
            {
                if (value == _inscricaoEstadual) return;
                _inscricaoEstadual = value;
                PropriedadeAlterada();
            }
        }

        public string DocumentoUnico
        {
            get { return _documentoUnico; }
            set
            {
                if (value == _documentoUnico) return;
                _documentoUnico = value;
                PropriedadeAlterada();
            }
        }

        public string NomeEmitente
        {
            get { return _nomeEmitente; }
            set
            {
                if (value == _nomeEmitente) return;
                _nomeEmitente = value;
                PropriedadeAlterada();
            }
        }

        public decimal Total
        {
            get { return _total; }
            set
            {
                if (value == _total) return;
                _total = value;
                PropriedadeAlterada();
            }
        }

        public string Chave
        {
            get { return _chave; }
            set
            {
                if (value == _chave) return;
                _chave = value;
                PropriedadeAlterada();
            }
        }

        public long DestinatarioIbgeCidade { get; set; }
        public long IbgeCidadeEmitente { get; set; }
        public string EmitenteUf { get; set; }
        public string EmitenteCidade { get; set; }
        public string ComplementoEmitente { get; set; }
        public string NumeroEmitente { get; set; }

        public void CarregarXml(string fileName)
        {
            var nfe = new NFeZeus();
            nfe = nfe.CarregarDeArquivoXml(fileName);

            var emitente = nfe.infNFe.emit;
            var destinatario = nfe.infNFe.dest;
            var totais = nfe.infNFe.total;
            var chave = nfe.infNFe.Id?.SubstringWithTrim(3, 44);

            PreencherCabecalho(chave, totais.ICMSTot.vNF);
            PreencherEmitente(emitente);
            PreencherDestinatario(destinatario);

            ValidarInformacoes();
        }

        private void ValidarInformacoes()
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(DocumentoUnico))
            {
                erros.Add("Documento único do Emitente está vazio!");
            }

            if (string.IsNullOrWhiteSpace(DocumentoUnicoDestinatario))
            {
                erros.Add("Documento único do destinatário está vazio!");
            }

            if (erros.Any())
            {
                throw new RegraNegocioException("Falha ao importar XMl", erros);
            }
        }

        private void AdicionarImportacaoAction(object obj)
        {
            try
            {
                OnImportarXmlEventHandler();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ImportarXmlAction(object obj)
        {
            var dialog = new OpenFileDialog
            {
                Filter = @"Arquivos XML(*.xml)|*.xml"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var caminho = dialog.FileName;

                try
                {
                    var nfe = new NFeZeus();
                    nfe = nfe.CarregarDeArquivoXml(caminho);

                    var emitente = nfe.infNFe.emit;
                    var destinatario = nfe.infNFe.dest;
                    var totais = nfe.infNFe.total;
                    var chave = nfe.infNFe.Id?.SubstringWithTrim(3, 44);

                    PreencherCabecalho(chave, totais.ICMSTot.vNF);
                    PreencherEmitente(emitente);
                    PreencherDestinatario(destinatario);
                }
                catch (Exception)
                {
                    DialogBox.MostraInformacao("Xml selecionado incorreto");
                }
            }
        }

        private void PreencherDestinatario(dest destinatario)
        {
            NomeDestinatario = destinatario.xNome;
            DocumentoUnicoDestinatario = destinatario.CNPJ ?? destinatario.CPF;
            InscricaoEstadualDestinatario = destinatario.IE ?? string.Empty;
            DestinatarioCep = destinatario.enderDest.CEP ?? string.Empty;
            DestinatarioLogradouro = destinatario.enderDest.xLgr ?? string.Empty;
            DestinatarioNumero = destinatario.enderDest.nro ?? string.Empty;
            DestinatarioBairro = destinatario.enderDest.xBairro ?? string.Empty;
            DestinatarioTelefone = destinatario.enderDest.fone?.ToString();
            DestinatarioUF = destinatario.enderDest.UF ?? string.Empty;
            DestinatarioCidade = destinatario.enderDest.xMun ?? string.Empty;
            DestinatarioIbgeCidade = destinatario.enderDest.cMun;
            DestinatarioComplemento = destinatario.enderDest.xCpl.TrimOrEmpty() ?? string.Empty; ;
            DestinatarioDestinatarioIsCheck = true;
        }

        private void PreencherEmitente(emit emitente)
        {
            NomeEmitente = emitente.xNome ?? emitente.xFant;
            DocumentoUnico = emitente.CNPJ ?? emitente.CPF;
            InscricaoEstadual = emitente.IE ?? string.Empty;
            CepEmitente = emitente.enderEmit.CEP ?? string.Empty;
            LogradouroEmitente = emitente.enderEmit.xLgr ?? string.Empty;
            BairroEmitente = emitente.enderEmit.xBairro ?? string.Empty;
            RegimeTributario = emitente.CRT.ToString();
            NumeroEmitente = emitente.enderEmit.nro ?? string.Empty;
            IbgeCidadeEmitente = emitente.enderEmit.cMun;
            ComplementoEmitente = emitente.enderEmit.xCpl ?? string.Empty;
            EmitenteCidade = emitente.enderEmit.xMun ?? string.Empty;
            EmitenteUf = emitente.enderEmit.UF.GetSiglaUfString() ?? string.Empty;
            EmitenteRemetenteIsCheck = true;
            EmitenteTomadorIsCheck = true;
        }

        private void PreencherCabecalho(string chave, decimal total)
        {
            ImportarDocumentoNFe = true;
            Chave = chave;
            Total = total;
        }

        public event EventHandler<RetornoImportacaoXmlCteEventArgs> ImportarXmlEventHandler;

        private void OnImportarXmlEventHandler()
        {
            BuscarDestinatario();
            BuscarEmitente();

            if (_pessoaDestinatario != null)
                AlterarPessoaDestinatario();

            if (_pessoaDestinatario == null)
                InserirPessoaDestinatario();

            if (_pessoaEmitente != null)
                AlterarPessoaEmitente();

            if (_pessoaEmitente == null)
                InserirPessoaEmitente();

            var retorno = new RetornoImportacaoXmlCteEventArgs
            {
                Chave = Chave.TrimOrEmpty(),
                Total = Total,
                PessoaDestinatario = _pessoaDestinatario,
                PessoaEmitente = _pessoaEmitente,
                IsImportarDocumentoNFe = ImportarDocumentoNFe,
                IsDestinatarioRemetente = DestinatarioRemetenteIsCheck,
                IsDestinatarioDestinatario = DestinatarioDestinatarioIsCheck,
                IsDestinatarioExpedidor = DestinatarioExpedidorIsCheck,
                IsDestinatarioRecebedor = DestinatarioRecebedorIsCheck,
                IsDestinatarioTomador = DestinatarioTomadorIsCheck,
                IsEmitenteRemetente = EmitenteRemetenteIsCheck,
                IsEmitenteDestinatario = EmitenteDestinatarioIsCheck,
                IsEmitenteExpedidor = EmitenteExpedidorIsCheck,
                IsEmitenteRecebedor = EmitenteRecebedorIsCheck,
                IsEmitenteTomador = EmitenteTomadorIsCheck
            };

            try
            {
                ImportarXmlEventHandler?.Invoke(this, retorno);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void InserirPessoaDestinatario()
        {
            var builder = new BuilderPessoaImportacao(new PessoaEntidade(NomeDestinatario.Trim()), this);

            var pessoa = builder.ConstruirDestinatario().Construir();
            var endereco = builder.ConstruirEnderecoDestinatario();

            InserirPessoa(pessoa, endereco);
            _pessoaDestinatario = pessoa;
        }

        private void AlterarPessoaDestinatario()
        {
            var builder = new BuilderPessoaImportacao(_pessoaDestinatario, this);

            _pessoaDestinatario = builder.ConstruirDestinatario().Construir();
            var enderecoPessoa = builder.ConstruirEnderecoDestinatario();

            AlterarPessoa(_pessoaDestinatario, enderecoPessoa);
        }

        private void InserirPessoaEmitente()
        {
            var builder = new BuilderPessoaImportacao(new PessoaEntidade(NomeEmitente.Trim()), this);

            var pessoa = builder.ConstruirEmitente().Construir();
            var endereco = builder.ConstruirEnderecoEmitente();

            InserirPessoa(pessoa, endereco);
            _pessoaEmitente = pessoa;
        }

        private void AlterarPessoaEmitente()
        {
            var builder = new BuilderPessoaImportacao(_pessoaEmitente, this);

            _pessoaEmitente = builder.ConstruirEmitente().Construir();
            var enderecoPessoa = builder.ConstruirEnderecoEmitente();

            AlterarPessoa(_pessoaEmitente, enderecoPessoa);
        }

        private void BuscarDestinatario()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _pessoaDestinatario = new RepositorioPessoa(sessao).BuscarPessoaPorDocumentoUnico(DocumentoUnicoDestinatario);
            }
        }

        private void BuscarEmitente()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _pessoaEmitente = new RepositorioPessoa(sessao).BuscarPessoaPorDocumentoUnico(DocumentoUnico);
            }
        }

        private void InserirPessoa(PessoaEntidade pessoa, PessoaEndereco endereco)
        {
            pessoa.Cliente = new Cliente(pessoa);
            pessoa.AdicionarEndereco(endereco);

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPessoa(sessao);
                repositorio.SalvaAlteracoes(pessoa);

                transacao.Commit();
            }
        }

        private static void AlterarPessoa(PessoaEntidade pessoa, PessoaEndereco enderecoPessoa)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPessoa(sessao);

                if (pessoa.Cliente == null)
                {
                    pessoa.Cliente = new Cliente(pessoa);
                    repositorio.SalvaAlteracoes(pessoa);

                    transacao.Commit();
                    return;
                }

                pessoa.AdicionarEndereco(enderecoPessoa);
                repositorio.SalvaAlteracoes(pessoa);
                
                transacao.Commit();
            }
        }
    }
}