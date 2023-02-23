using System;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Perfil;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.PedidoVenda.Fabricas;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using FusionCore.Sessao;

namespace FusionCore.FusionAdm.ConverterVendaParaNfe
{
    public class ConvertePedidoDeVendaParaNFe : IConverteVendaParaNFe
    {
        private readonly PedidoVenda.PedidoVenda _pedidoVenda;
        private AbaPerfilNfeDTO _perfilNfe;
        private readonly ISessaoManager _sessaoManager;
        private readonly UsuarioDTO _usuario;
        private readonly Cliente _cliente;
        private EmissorFiscal _emissorDoPerfil;
        private Transportadora _transportadoraDoPerfil;
        private Veiculo _veiculoDoPerfil;
        private PerfilNfeSimplesNacional _simplesNacionalPerfil;

        public ConvertePedidoDeVendaParaNFe(PedidoVenda.PedidoVenda pedidoVenda,
            ISessaoManager sessaoManager,
            UsuarioDTO usuario,
            Cliente cliente)
        {
            _pedidoVenda = pedidoVenda;
            _sessaoManager = sessaoManager;
            _usuario = usuario;
            _cliente = cliente;
        }

        public int Executar()
        {
            if (_pedidoVenda.IsOrcamento)
            {
                throw new InvalidOperationException("Preciso que transforme o Orçamento em um Pedido de Venda");
            }

            CarregarInformacoesDoPerfil();

            var emitente = new EmitenteNfe(_emissorDoPerfil);
            var builderNfe = new BuilderPedidoVendaParaNFe(_pedidoVenda, emitente, _usuario);

            builderNfe.ComPerfilNfe(_perfilNfe);
            builderNfe.ComDestinatario(MontaDestinatario());
            builderNfe.ComPerfilSimplesNacional(_simplesNacionalPerfil);

            if (_transportadoraDoPerfil != null)
            {
                builderNfe.ComTransportadora(_transportadoraDoPerfil);

                if (_veiculoDoPerfil != null)
                {
                    builderNfe.ComVeiculoTransportadora(_veiculoDoPerfil);
                }
            }

            builderNfe.ComDescontoFixo(_pedidoVenda.TotalDesconto);
            builderNfe.ComProdutos(_pedidoVenda.ItensPedidoVenda);

            var nfe = builderNfe.Construir();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repoNfe = new RepositorioNfe(sessao);
                var repoPedidoVenda = FactoryRepositorioPedidoVenda.CriaRepositorio(sessao);

                repoNfe.SalvarAlteracoes(nfe);

                repoPedidoVenda.RetirarDaReservaEstoquePedidoVenda(_pedidoVenda, _usuario, OrigemEventoEstoque.PedidoVendaReservaEfetuadaNfe);
                repoPedidoVenda.BaixaDeOrçamentoEstoquePedidoVenda(_pedidoVenda, _usuario, OrigemEventoEstoque.PedidoVendaOrcamentoEfetuadaNfce); 
                
                _pedidoVenda.Faturar();

                repoPedidoVenda.SalvarAlteracoes(_pedidoVenda);
                transacao.Commit();
            }

            return nfe.Id;
        }

        private PedidoDestinatario MontaDestinatario()
        {
            if (_pedidoVenda.Destinatario?.Cliente != null) return _pedidoVenda.Destinatario;

            return new PedidoDestinatario(_cliente, _pedidoVenda);
        }

        public void AdicionarPerfilNfe(AbaPerfilNfeDTO perfilNfe)
        {
            _perfilNfe = perfilNfe;
        }

        private void CarregarInformacoesDoPerfil()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorioEmissor = new RepositorioEmissorFiscal(sessao);
                var repositorioPessoa = new RepositorioPessoa(sessao);
                var repositorioVeiculo = new RepositorioVeiculo(sessao);
                var repositorioPerfilNfe = new RepositorioPerfilNfe(sessao);

                _emissorDoPerfil = repositorioEmissor.GetPeloId(_perfilNfe.EmissorFiscalId);
                _transportadoraDoPerfil = repositorioPessoa.GetTransportadoraPeloId(_perfilNfe.TransportadoraId);
                _veiculoDoPerfil = repositorioVeiculo.GetPeloId(_perfilNfe.VeiculoId);
                _simplesNacionalPerfil = repositorioPerfilNfe.GetSimplesNacional(_perfilNfe.Id);
            }
        }
    }
}