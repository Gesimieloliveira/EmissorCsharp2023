using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Perfil;
using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.SelecionarNfce;
using FusionCore.Sessao;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Autorizadores.Nfce;
using NHibernate.Util;

namespace FusionCore.FusionAdm.ConverterVendaParaNfe
{
    public class ConverterNfcesParaNfe : IConverteVendaParaNFe
    {
        private readonly IList<NfceSelecionadaDto> _nfces;
        private readonly ISessaoManager _sessaoGerenciador;
        private readonly UsuarioDTO _usuario;
        private readonly ClienteDto _cliente;
        private short _perfilNfeId;
        private PerfilNfe _perfilNfe;

        public ConverterNfcesParaNfe(IList<NfceSelecionadaDto> nfces,
            ISessaoManager sessaoGerenciador,
            UsuarioDTO usuario,
            ClienteDto cliente)
        {
            _nfces = nfces;
            _sessaoGerenciador = sessaoGerenciador;
            _usuario = usuario;
            _cliente = cliente;
        }

        public int Executar()
        {
            CarregarInformacoesDoPerfil();

            var nfces = CarregarTodasNFces();
            var cupons = CarregarCuponsFiscais();

            var emitente = new EmitenteNfe(_perfilNfe.EmissorFiscal);

            var cliente = BuscarCliente();

            if (cliente.GetEnderecoPrincipal() == null)
                throw new InvalidOperationException(
                    $"Cliente: {cliente.Nome} não possui endereço.\nCadastrar endereço para o cliente.");

            var builderNfe = new BuilderNfcesParaNfe(emitente, _usuario, cliente);

            builderNfe.ComPerfilNfe(_perfilNfe);


            var totalDesconto = 0.00M;

            nfces.ForEach(nfce =>
            {
                totalDesconto += nfce.ObterOsItens().Sum(x => x.Desconto);
            });

            cupons.ForEach(cupom =>
            {
                totalDesconto += cupom.Venda.Produtos.Sum(x => x.TotalDesconto);
            });

            builderNfe.ComDescontoFixo(totalDesconto);

            foreach (var nfceAdm in nfces)
            {
                builderNfe.ComProdutos(nfceAdm.ObterOsItens());
                builderNfe.ComChave(nfceAdm.Emissao.Chave);
            }

            foreach (var cupomFiscal in cupons)
            {
                builderNfe.ComProdutos(cupomFiscal.Venda.Produtos);
                builderNfe.ComChave(cupomFiscal.CupomFiscalFinalizado.Chave);
            }

            var nfe = builderNfe.Construir();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repoNfe = new RepositorioNfe(sessao);
                var repositorioNfce = new RepositorioNfceAdm(sessao);
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                nfces.ForEach(nfce =>
                {
                    nfce.ImportadaParaNfe = true;
                    repositorioNfce.Merge(nfce);
                });

                cupons.ForEach(cupom =>
                {
                    cupom.FoiImportadaParaNfe();
                    repositorioCupomFiscal.Merge(cupom);
                });

                nfe.SemPagamento = true;
                repoNfe.SalvarAlteracoes(nfe);
                nfe.Referencias.ForEach(repoNfe.Persistir);

                transacao.Commit();
            }

            return nfe.Id;
        }

        private IEnumerable<NfceAdm> CarregarTodasNFces()
        {
            var nfces = new List<NfceAdm>();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioNfce = new RepositorioNfceAdm(sessao);

                foreach (var nfceSelecionadaDto in _nfces.Where(x => x.PontoDeVendaNfce))
                {
                    nfces.Add(repositorioNfce.GetPeloId(nfceSelecionadaDto.Id));
                }
            }

            return nfces;
        }

        private IEnumerable<CupomFiscal> CarregarCuponsFiscais()
        {
            var cuponsFiscais = new List<CupomFiscal>();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                foreach (var nfceSelecionadaDto in _nfces.Where(x => x.Faturamento))
                {
                    cuponsFiscais.Add(repositorioCupomFiscal.GetPeloId(nfceSelecionadaDto.Id));
                }
            }

            return cuponsFiscais;
        }

        private Cliente BuscarCliente()
        {
            using (var sessao = new SessaoManagerAdm().CriaSessao())
            {
                return new RepositorioPessoa(sessao).GetClientePeloId(_cliente.Id);
            }
        }

        public void AdicionarPerfilNfe(AbaPerfilNfeDTO perfilNfe)
        {
            _perfilNfeId = perfilNfe.Id;
        }

        private void CarregarInformacoesDoPerfil()
        {
            using (var sessao = _sessaoGerenciador.CriaSessao())
            {
                var repositorioPerfilNfe = new RepositorioPerfilNfe(sessao);

                _perfilNfe = repositorioPerfilNfe.GetPeloId(_perfilNfeId);
            }
        }
    }
}