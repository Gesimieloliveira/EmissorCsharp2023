using System;
using System.Linq.Expressions;
using FusionCore.FusionAdm.Fiscal.Contratos.Componentes;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Componentes;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using DestinoOperacao = FusionCore.FusionAdm.Fiscal.Flags.DestinoOperacao;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class DestinatarioNfe
    {
        public static class Expression
        {
            public static readonly Expression<Func<DestinatarioNfe, object>> DocumentoUnico = x => x._documentoUnico;
        }

        private int _nfeId;
        private string _documentoUnico;
        private string _nome;
        private string _inscricaoEstadual;
        private string _email;
        private int _pessoaId;
        private Cliente _clienteCache;
        public Nfeletronica Nfe { get; private set; }
        public IEnderecoFiscal Endereco { get; set; }

        private DestinatarioNfe()
        {
            Email = string.Empty;
            IndicadorIE = IndicadorIE.NaoContribuinte;
            InscricaoEstadual = string.Empty;
            Email = string.Empty;
        }

        public DestinatarioNfe(Nfeletronica nfe, IEnderecoFiscal endereco) : this()
        {
            Nfe = nfe;
            Endereco = endereco;
        }

        public string InscricaoEstadual
        {
            get => _inscricaoEstadual;
            set => _inscricaoEstadual = value ?? string.Empty;
        }

        public string DocumentoUnico
        {
            get => _documentoUnico;
            set
            {
                var doc = new DocumentoUnico(value);

                if (!ResideExterior() && !doc.IsValido)
                    throw new InvalidOperationException("Você não pode emitir a NF com CPF/CNPJ do destinatário errado");

                _documentoUnico = doc.ToString();
            }
        }

        public string Nome
        {
            get => _nome;
            set => _nome = value;
        }

        public string Email
        {
            get => _email;
            set => _email = value ?? string.Empty;
        }

        public bool SolicitaPedido { get; set; }
        public IndicadorIE IndicadorIE { get; set; }
        public IndicadorOperacaoFinal? IndicadorOperacaoFinal { get; set; }
        public IndicadorComprador? IndicadorPresenca { get; set; }
        public DestinoOperacao? IndicadorDestinoOperacao { get; set; }

        public int GetPessoaId()
        {
            return _pessoaId;
        }

        public void ReferenciaUmaPessoaId(int pessoaId)
        {
            if (pessoaId == 0)
                throw new InvalidOperationException("Pessoa referênciada no destinatario não é válida");

            _pessoaId = pessoaId;
        }

        public bool ResideExterior()
        {
            return Endereco.Localizacao.CodigoUF == 99;
        }

        public bool ResideForaDoEstado()
        {
            return Nfe.Emitente.Empresa.EstadoDTO.CodigoIbge != Endereco.Localizacao.CodigoUF;
        }

        public Cliente FindCliente()
        {
            if (_clienteCache != null && _clienteCache.Id == _pessoaId)
            {
                return _clienteCache;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                _clienteCache = repositorio.GetClientePeloId(_pessoaId);

                return _clienteCache;
            }
        }

        public string ObterInscricaoEstadualZeus()
        {
            return string.IsNullOrWhiteSpace(InscricaoEstadual) ? null : InscricaoEstadual;
        }

        public void AutoDefinirIndicadores()
        {
            IndicadorPresenca = IndicadorComprador.NaoSeAplica;
            IndicadorDestinoOperacao = DestinoOperacao.Interna;
            IndicadorOperacaoFinal = Flags.IndicadorOperacaoFinal.ConsumidorFinal;

            if (IndicadorIE == IndicadorIE.ContribuinteIcms)
                IndicadorOperacaoFinal = Flags.IndicadorOperacaoFinal.Normal;

            if (ResideForaDoEstado())
                IndicadorDestinoOperacao = DestinoOperacao.Interestadual;
            else if (ResideExterior())
                IndicadorDestinoOperacao = DestinoOperacao.Exterior;
        }
    }
}