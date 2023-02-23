using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.SelecionarNfce;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.SelecionarNfce
{
    public class SelecionarUmClienteParaNfeFormularioModel : ViewModel
    {
        public event EventHandler<ClienteDto> ClienteSelecionadoComSucesso; 

        private ObservableCollection<ClienteDto> _listaDeClientesDisponiveis = new ObservableCollection<ClienteDto>();
        private ClienteDto _clienteSelecionado;

        public ClienteDto ClienteSelecionado
        {
            get => _clienteSelecionado;
            set
            {
                _clienteSelecionado = value;
                PropriedadeAlterada();
            }
        }
        public ObservableCollection<ClienteDto> ListaDeClientesDisponiveis
        {
            get => _listaDeClientesDisponiveis;
            set
            {
                _listaDeClientesDisponiveis = value;
                PropriedadeAlterada();
            }
        }

        public ICommand ComandoSelecionarClienteManualmente => GetSimpleCommand(SelecionarClienteManualmenteAcao);

        private void SelecionarClienteManualmenteAcao(object obj)
        {
            OnFechar();
        }


        public void CarregarClientes(List<ClienteDto> listaDeClientesDisponiveis)
        {
            ListaDeClientesDisponiveis = new ObservableCollection<ClienteDto>(listaDeClientesDisponiveis);
        }

        public void SelecionarCliente()
        {
            var cliente = BuscarCliente();

            if (cliente.GetEnderecoPrincipal() == null)
                throw new InvalidOperationException(
                    $"Cliente: {cliente.Nome} não possui endereço.\nCadastrar endereço para o cliente.");

            OnClienteSelecionadoComSucesso();
            OnFechar();
        }

        private Cliente BuscarCliente()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioPessoa(sessao).GetClientePeloId(ClienteSelecionado.Id);
            }
        }

        protected virtual void OnClienteSelecionadoComSucesso()
        {
            ClienteSelecionadoComSucesso?.Invoke(this, ClienteSelecionado);
        }
    }
}