using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Fusion.Visao.Validacoes.CteOs;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.ConsultaCnpj;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Cidade;
using FusionCore.Repositorio.Legacy.Buscas.Adm.UF;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Flags;
using FusionLibrary.Helper.Conversores;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.Helper.ExtImagens;
using FusionLibrary.Validacao;
using FusionLibrary.Validacao.Regras;
using FusionLibrary.ValidacaoAnotacao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using Microsoft.Win32;
using NHibernate.Util;

namespace Fusion.Visao.Empresa
{
    public sealed class EmpresaFormModel : ViewModel
    {
        private RegimeTributario _regimeTributario;

        private string _taf;
        private string _numeroRegistroEstadual;
        private string _arquivoLogo;
        private ImageSource _logoMarca;
        private ImageSource _logoMarcaNfce;
        private string _arquivoLogoNfce;

        public ObservableCollection<EstadoDTO> Estados
        {
            get => GetValue<ObservableCollection<EstadoDTO>>();
            set => SetValue(value);
        }

        public ObservableCollection<CidadeDTO> Cidades
        {
            get => GetValue<ObservableCollection<CidadeDTO>>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"CEP é obrigatorio")]
        public string Cep
        {
            get => GetValue();
            set => SetValue(value?.RemoveNaoNumericos());
        }

        [Required(ErrorMessage = @"Logradouro é obrigatório")]
        public string Logradouro
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Complemento
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Bairro é obrigatório")]
        public string Bairro
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Numero é obrigatório")]
        public string Numero
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Fone1 é obrigatório")]
        public string Fone1
        {
            get => GetValue();
            set => SetValue(value?.RemoveNaoNumericos());
        }

        public string Fone2
        {
            get => GetValue<string>();
            set => SetValue(value?.RemoveNaoNumericos());
        }

        [Required(ErrorMessage = @"UF é obrigatório")]
        public EstadoDTO Estado
        {
            get => GetValue<EstadoDTO>();
            set
            {
                SetValue(value);
                CarregarCidades(Estado);
            }
        }

        [Required(ErrorMessage = @"Cidade é obrigatório")]
        public CidadeDTO Cidade
        {
            get => GetValue<CidadeDTO>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Razão social é obrigatório")]
        public string RazaoSocial
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Nome fantasia é obrigatório")]
        public string NomeFantasia
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Cnpj/Cpf é obrigatório")]
        public string DocumentoUnico
        {
            get => GetValue();
            set => SetValue(value?.RemoveNaoNumericos());
        }

        [ApenasNumeros(ErrorMessage = @"Deve conter apenas números")]
        public string InscricaoEstadual
        {
            get => GetValue();
            set => SetValue(value?.RemoveNaoNumericos());
        }

        public string InscricaoMunicipal
        {
            get => GetValue<string>();
            set => SetValue(value?.RemoveNaoNumericos() ?? string.Empty);
        }

        [Required(ErrorMessage = @"Atividade Iniciada Em é obrigatório")]
        public DateTime? AtividadeIniciadaEm
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public string Rntrc
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Taf
        {
            get => _taf;
            set
            {
                if (value == _taf) return;
                _taf = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroRegistroEstadual
        {
            get { return _numeroRegistroEstadual; }
            set
            {
                if (value == _numeroRegistroEstadual) return;
                _numeroRegistroEstadual = value;
                PropriedadeAlterada();
            }
        }

        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public EmpresaDTO Model
        {
            get => GetValue<EmpresaDTO>();
            set => SetValue(value);
        }

        public RegimeTributario RegimeTributario
        {
            get => _regimeTributario;
            set
            {
                _regimeTributario = value;
                PropriedadeAlterada();
            }
        }

        public bool IsNovo => Model == null || Model.Id == 0;

        public EmpresaFormModel(EmpresaDTO empresa)
        {
            Inicializa(empresa);
        }

        private void Inicializa(EmpresaDTO empresa)
        {
            Model = empresa;
            Estados = new ObservableCollection<EstadoDTO>();
            CarregarEstados();
        }

        public event EventHandler FecharTela;
        public event EventHandler<EmpresaDTO> RegistroSalvo;

        private void CarregarCidades(EstadoDTO estado)
        {
            if (estado == null)
            {
                Cidades = null;
                return;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioComun<CidadeDTO>(sessao);
                var cidades = repositorio.Busca(new CidadesPorSiglaUF(estado.Sigla));
                Cidades = new ObservableCollection<CidadeDTO>(cidades);
            }
        }

        private void CarregarEstados()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioComun<EstadoDTO>(sessao);
                var estados = new ObservableCollection<EstadoDTO>(repositorio.Busca(new TodosUF()));
                estados.ForEach(Estados.Add);
            }
        }

        public void PreencherViewModel()
        {
            AtividadeIniciadaEm = Model.AtividadeIniciadaEm;
            RazaoSocial = Model.RazaoSocial;
            NomeFantasia = Model.NomeFantasia;
            DocumentoUnico = Model.DocumentoUnico;
            InscricaoEstadual = Model.InscricaoEstadual;
            InscricaoMunicipal = Model.InscricaoMunicipal;
            Email = Model.Email;
            Cep = Model.Cep;
            Estado = Model.EstadoDTO;
            Cidade = Model.CidadeDTO;
            Logradouro = Model.Logradouro;
            Complemento = Model.Complemento;
            Bairro = Model.Bairro;
            Numero = Model.Numero;
            Fone1 = Model.Fone1;
            Fone2 = Model.Fone2;
            Rntrc = Model.Rntrc;
            Taf = Model.Taf;
            NumeroRegistroEstadual = Model.NumeroRegistroEstadual;
            RegimeTributario = Model.RegimeTributario;
            CommandBuscaLogo = GetSimpleCommand(BuscaLogoAction, true, "BuscaLogoAction");
            CommandBuscaLogoNfce = GetSimpleCommand(AcaoNfceLogoMarca, true, "BuscaLogoNfce");
            CommandLimpaLogo = GetSimpleCommand(AcaoLimpaLogo, true, "AcaoLimpaLogo");
            CommandLimpaLogoNfce = GetSimpleCommand(AcaoLimpaLogoNfce, true, "AcaoLimpaLogoNfce");
            LogoMarca = ConverteImage.ByteEmImagem(Model.LogoMarca);
            LogoMarcaNfce = ConverteImage.ByteEmImagem(Model.LogoMarcaNfce);
        }

        private void AcaoLimpaLogoNfce(object obj)
        {
            ArquivoLogoNfce = null;
            LogoMarcaNfce = null;
        }

        public ICommand CommandLimpaLogoNfce { get; set; }

        private void AcaoLimpaLogo(object obj)
        {
            ArquivoLogo = null;
            LogoMarca = null;
        }

        public ICommand CommandLimpaLogo { get; set; }

        public ICommand CommandBuscaLogoNfce { get; set; }

        public ImageSource LogoMarcaNfce
        {
            get => _logoMarcaNfce;
            set
            {
                if (Equals(value, _logoMarcaNfce)) return;
                _logoMarcaNfce = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandBuscaLogo { get; set; }

        public string ArquivoLogo
        {
            get => _arquivoLogo;
            set
            {
                if (value == _arquivoLogo) return;
                _arquivoLogo = value;
                PropriedadeAlterada();
            }
        }

        public string ArquivoLogoNfce
        {
            get => _arquivoLogoNfce;
            set
            {
                if (value == _arquivoLogoNfce) return;
                _arquivoLogoNfce = value;
                PropriedadeAlterada();
            }
        }

        public ImageSource LogoMarca
        {
            get => _logoMarca;
            set
            {
                if (Equals(value, _logoMarca)) return;
                _logoMarca = value;
                PropriedadeAlterada();
            }
        }

        private void BuscaLogoAction(object obj)
        {
            var janelaArquivo = new OpenFileDialog
            {
                Filter = "Logo (*.png)|*.png"
            };

            if (janelaArquivo.ShowDialog() == true)
            {
                ArquivoLogo = janelaArquivo.FileName;
            }

            if (janelaArquivo.FileName.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Selecione uma logo");
                return;
            }

            var image = Image.FromFile(ArquivoLogo);

            LogoMarca = image.ToBitmapImage();
        }

        private void AcaoNfceLogoMarca(object obj)
        {
            var janelaArquivo = new OpenFileDialog
            {
                Filter = "Logo Nfce(*.png)|*.png"
            };

            if (janelaArquivo.ShowDialog() == true)
            {
                ArquivoLogoNfce = janelaArquivo.FileName;
            }

            if (janelaArquivo.FileName.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Selecione uma logo para nfc-e");
                return;
            }

            var image = Image.FromFile(ArquivoLogoNfce);

            LogoMarcaNfce = image.ToBitmapImage();
        }

        public void SalvarModel()
        {
            try
            {
                if (RazaoSocial.IsNullOrEmpty() || RazaoSocial.Length < 2)
                {
                    throw new InvalidOperationException("Razão Social que me informou é inválido");
                }

                if (NomeFantasia.IsNullOrEmpty() || NomeFantasia.Length < 2)
                {
                    throw new InvalidOperationException("Nome Fantasia que me informou é inválido");
                }

                if (DocumentoUnico.Length == 11 && new CpfRegra().NaoValido(DocumentoUnico))
                {
                    throw new InvalidOperationException("CNPJ/CPF que me informou é inválido");
                }

                if (DocumentoUnico.Length == 14 && new CnpjRegra().NaoValido(DocumentoUnico))
                {
                    throw new InvalidOperationException("CNPJ/CPF que me informou é inválido");
                }

                if (DocumentoUnico.Length < 11 || DocumentoUnico.Length > 14)
                {
                    throw new InvalidOperationException("CNPJ/CPF que me informou é inválido");
                }

                if (DocumentoUnico.IsNullOrEmpty())
                {
                    throw new InvalidOperationException("CNPJ/CPF que me informou é inválido");
                }

                if (AtividadeIniciadaEm == null)
                {
                    throw new InvalidOperationException("Inicio Atividade que me informou é inválido");
                }

                if (Fone1.IsNullOrEmpty() || Fone1.Length < 10 || Fone1.Length > 11)
                {
                    throw new InvalidOperationException("Fone1 que me informou é inválido");
                }

                if (Cep.IsNullOrEmpty() || Cep.Length < 8)
                {
                    throw new InvalidOperationException("CEP que me informou é inválido");
                }

                if (Logradouro.IsNullOrEmpty() || Logradouro.Length < 2)
                {
                    throw new InvalidOperationException("Logradouro que me informou é inválido");
                }

                if (Numero.IsNullOrEmpty())
                {
                    throw new InvalidOperationException("Número que me informou é inválido");
                }

                if (Bairro.IsNullOrEmpty() || Bairro.Length < 2)
                {
                    throw new InvalidOperationException("Bairro que me informou é inválido");
                }

                if (Estado == null || Estado.Sigla.Length < 2)
                {
                    throw new InvalidOperationException("Estado que me informou é inválido");
                }

                if (Cidade == null || Cidade.Nome.Length < 3)
                {
                    throw new InvalidOperationException("Cidade que me informou é inválido");
                }

                if (ExisteEmpresaComEsseCnpj())
                {
                    throw new InvalidOperationException("CNPJ/CPF já está cadastrado para outra empresa");
                }

                ValidarTafRegistroEstadual.Executar(Taf, NumeroRegistroEstadual);

                SalvarEmBanco();

                DialogBox.MostraInformacao("Empresa foi salva com sucesso!");

                OnRegistroSalvo(Model);
                OnFecharTela();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (Exception e)
            {
                DialogBox.MostraErro(e.Message, e);
            }
        }

        private bool ExisteEmpresaComEsseCnpj()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmpresa(sessao);
                var empresas = repositorio.BuscaPeloCnpjCpf(DocumentoUnico);

                if (IsNovo && empresas.Count > 0)
                {
                    return true;
                }

                if (empresas.Any(e => e.Id != Model.Id))
                {
                    return true;
                }
            }

            return false;
        }

        private void SalvarEmBanco()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioComun<EmpresaDTO>(sessao);

                if (Model.Id <= 0)
                {
                    Model.CadastradoEm = DateTime.Now;
                }

                Model.AtividadeIniciadaEm = AtividadeIniciadaEm;

                Model.Cnpj = string.Empty;
                Model.Cpf = DocumentoUnico.TrimOrEmpty();
                if (DocumentoUnico.Length == 14)
                {
                    Model.Cnpj = DocumentoUnico.TrimOrEmpty();
                    Model.Cpf = string.Empty;
                }

                Model.Email = Email ?? string.Empty;
                Model.InscricaoEstadual = InscricaoEstadual.TrimOrEmpty();
                Model.InscricaoMunicipal = InscricaoMunicipal.TrimOrEmpty();
                Model.NomeFantasia = NomeFantasia.TrimOrEmpty();
                Model.RazaoSocial = RazaoSocial.TrimOrEmpty();
                Model.Cep = Cep.TrimOrEmpty();
                Model.EstadoDTO = Estado;
                Model.CidadeDTO = Cidade;
                Model.Logradouro = Logradouro.TrimOrEmpty();
                Model.Complemento = Complemento.TrimOrEmpty();
                Model.Bairro = Bairro.TrimOrEmpty();
                Model.Numero = Numero.TrimOrEmpty();
                Model.Fone1 = Fone1.TrimOrEmpty();
                Model.Fone2 = Fone2.TrimOrEmpty();
                Model.AlteradoEm = DateTime.Now;
                Model.RegimeTributario = RegimeTributario;
                Model.Rntrc = Rntrc.TrimOrEmpty();
                Model.Taf = Taf.TrimOrEmpty();
                Model.NumeroRegistroEstadual = NumeroRegistroEstadual.TrimOrEmpty();
                Model.LogoMarca = ConverteImage.ImagemEmByte(LogoMarca, new PngBitmapEncoder());
                Model.LogoMarcaNfce = ConverteImage.ImagemEmByte(LogoMarcaNfce, new PngBitmapEncoder());

                sessao.Clear();
                repositorio.Salva(Model);
                transacao.Commit();
            }
        }

        public void OnFecharTela()
        {
            FecharTela?.Invoke(this, EventArgs.Empty);
        }

        private void OnRegistroSalvo(EmpresaDTO e)
        {
            RegistroSalvo?.Invoke(this, e);
        }

        public void EmpresaReceitaHandler(object sender, EmpresaReceitaWs e)
        {
            AtividadeIniciadaEm = e.InicioAtividade;
            DocumentoUnico = e.Cnpj;
            Email = e.Email?.Valor;
            NomeFantasia = e.NomeFantasia;
            RazaoSocial = e.RazaoSocial;
            Cep = e.Cep;
            Logradouro = e.Logradouro;
            Complemento = e.Complemento;
            Bairro = e.Bairro;
            Numero = e.Numero;
            Cidade = e.Municipio;
            Estado = e.Uf;

            Fone1 = e.Telefone.ElementAtOrDefault(0);
            Fone2 = e.Telefone.ElementAtOrDefault(1);
        }
    }
}