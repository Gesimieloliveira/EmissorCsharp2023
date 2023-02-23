using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using static System.String;

namespace Fusion.Visao.Pessoa.SubFormularios
{
    public sealed class EnderecoFormModel : ViewModel
    {
        private PessoaEndereco _endereco;
        private readonly LocalidadesServico _localidades = LocalidadesServico.GetInstancia();
        private readonly bool _novo;
        private bool _isPessoaPodeAlterar;
        private bool _principal;
        private bool _entrega;
        private bool _outros;

        public bool Outros
        {
            get => _outros;
            set
            {
                _outros = value;
                PropriedadeAlterada();
            }
        }

        public bool Entrega
        {
            get => _entrega;
            set
            {
                _entrega = value;
                PropriedadeAlterada();
            }
        }

        public bool Principal
        {
            get => _principal;
            set
            {
                _principal = value;
                PropriedadeAlterada();
            }
        }

        public string Cep
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Logradouro
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Numero
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Bairro
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Complemento
        {
            get => GetValue();
            set => SetValue(value);
        }

        public EstadoDTO Uf
        {
            get => GetValue<EstadoDTO>();
            set => SetValue(value);
        }

        public CidadeDTO Cidade
        {
            get => GetValue<CidadeDTO>();
            set => SetValue(value);
        }

        public bool IsPrincipal
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsEntrega
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsCobranca
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsCorrespondencia
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ICollection<EstadoDTO> Estados
        {
            get => GetValue<ICollection<EstadoDTO>>();
            set => SetValue(value);
        }

        public ICollection<CidadeDTO> Cidades
        {
            get => GetValue<ICollection<CidadeDTO>>();
            set => SetValue(value);
        }

        public EnderecoFormModel()
        {
            IsPessoaPodeAlterar = true;
            _novo = true;
        }

        public EnderecoFormModel(PessoaEndereco endereco, bool isPessoaAlterar)
        {
            IsPessoaPodeAlterar = isPessoaAlterar;
            _endereco = (PessoaEndereco) endereco.Clone();
        }

        public bool IsPessoaPodeAlterar
        {
            get => _isPessoaPodeAlterar;
            set
            {
                if (value == _isPessoaPodeAlterar) return;
                _isPessoaPodeAlterar = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<PessoaEndereco> EnderecoAdicionado;
        public event EventHandler<PessoaEndereco> EnderecoEditado;
        public event EventHandler<PessoaEndereco> EnderecoDeletado;
        public event EventHandler Finalizado;

        private void OnFinalizado()
        {
            Finalizado?.Invoke(this, EventArgs.Empty);
        }

        private void OnEnderecoDeletado(PessoaEndereco e)
        {
            EnderecoDeletado?.Invoke(this, e);
        }

        private void OnEnderecoEditado(PessoaEndereco e)
        {
            EnderecoEditado?.Invoke(this, e);
        }

        private void OnEnderecoAdicionado(PessoaEndereco e)
        {
            EnderecoAdicionado?.Invoke(this, e);
        }

        public void CarregarDados()
        {
            Estados = _localidades.GetEstados();

            Cep = _endereco?.Cep ?? Empty;
            Logradouro = _endereco?.Logradouro ?? Empty;
            Numero = _endereco?.Numero ?? Empty;
            Bairro = _endereco?.Bairro ?? Empty;
            Uf = Estados.SingleOrDefault(e => e.Sigla == _endereco?.Cidade?.SiglaUf);
            Cidade = _endereco?.Cidade;
            Complemento = _endereco?.Complemento ?? Empty;
            Principal = _endereco?.Principal ?? false;
            Entrega = _endereco?.Entrega ?? false;
            Outros = _endereco?.Outros ?? true;
        }

        public void LoadCidadesComBaseNoEstado()
        {
            if (Uf == null)
            {
                Cidades.Clear();
                return;
            }

            Cidades = _localidades.GetCidades(c => c.SiglaUf == Uf.Sigla);
        }

        public void ConfirmarEndereco()
        {
            try
            {
                PreencherEndereco();

                if (_novo)
                {
                    OnEnderecoAdicionado(_endereco);
                    OnFinalizado();
                    return;
                }

                OnEnderecoEditado(_endereco);
                OnFinalizado();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void PreencherEndereco()
        {
            if (Bairro?.Trim().Length <= 1)
            {
                throw new InvalidOperationException("O Bairro precisa ter no mínimo 2 letras");
            }

            if (IsNullOrWhiteSpace(Numero))
            {
                throw new RegraNegocioException("Número não pode ser vazio. Dica: use SN");
            }

            if (_endereco == null)
            {
                _endereco = new PessoaEndereco(Logradouro, Numero, Bairro, Cep, Cidade);
            }

            _endereco.Logradouro = Logradouro?.Trim() ?? Empty;
            _endereco.Bairro = Bairro?.Trim() ?? Empty;
            _endereco.Cep = Cep?.Trim() ?? Empty;
            _endereco.Cidade = Cidade;
            _endereco.Complemento = Complemento?.Trim() ?? Empty;
            _endereco.Numero = Numero.TrimOrEmpty();
            _endereco.Entrega = Entrega;
            _endereco.Principal = Principal;
            _endereco.Outros = Outros;
        }

        public void DeletarEndereco()
        {
            if (_endereco == null)
            {
                DialogBox.MostraAviso("Nenhum endereço para ser deletado");
                return;
            }

            OnEnderecoDeletado(_endereco);
            OnFinalizado();
        }
    }
}