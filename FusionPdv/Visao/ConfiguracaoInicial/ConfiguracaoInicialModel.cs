using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using FusionCore.FusionPdv.ModeloEcf;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using FusionPdv.Acbr.Paf;
using FusionPdv.Ecf;
using FusionPdv.Servicos.ArquivoAuxiliar;
using FusionPdv.Servicos.Ecf;
using FusionPdv.Servicos.ValidacaoInicial;
using NHibernate.Util;

namespace FusionPdv.Visao.ConfiguracaoInicial
{
    public class ConfiguracaoInicialModel : ModelBase
    {
        private IList _modelosEcf = (IList) BuscarListaDeEcfAtiva();
        private ApresentaEcf _modeloEcfSelecionado;
        private string _portaSelecionada;
        private EcfDt _ecf;
        private string _serie;
        private string _numeroEcf;
        private string _md5;
        private string _velocidadeEcf;
        private bool _controlePorta;

        public string VelocidadeSelecionada
        {
            get { return _velocidadeEcf; }
            set
            {
                _velocidadeEcf = value;
                PropriedadeAlterada();
            }
        }

        public IList ModelosEcf
        {
            get { return _modelosEcf; }
            set
            {
                _modelosEcf = value;
                PropriedadeAlterada();
            }
        }

        public ApresentaEcf ModeloEcfSelecionado
        {
            get { return _modeloEcfSelecionado; }
            set
            {
                _modeloEcfSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public string PortaSelecionada
        {
            get { return _portaSelecionada; }
            set
            {
                _portaSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public string Serie
        {
            get { return _serie; }
            set
            {
                _serie = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroEcf
        {
            get { return _numeroEcf; }
            set
            {
                _numeroEcf = value;
                PropriedadeAlterada();
            }
        }

        public bool ControlePorta
        {
            get { return _controlePorta; }
            set
            {
                if (value == _controlePorta) return;
                _controlePorta = value;
                PropriedadeAlterada();
            }
        }

        public ConfiguracaoInicialModel()
        {
            InicializaVariaveis();
        }

        private void InicializaVariaveis()
        {
            _ecf = new ObterEcfEmUso().Buscar() ?? new EcfDt();
            RecuperarEcfAtiva();
        }

        public void RecuperarEcfAtiva()
        {
            var modeloAcbr = ModeloEmissor.Nenhum;

            if (!string.IsNullOrEmpty(_ecf.ModeloAcbr))
            {
                modeloAcbr = Enum.GetValues(typeof (ModeloEmissor))
                    .Cast<ModeloEmissor>().First(modelo => modelo.ToString().Equals(_ecf.ModeloAcbr));
            }

            ModeloEcfSelecionado = new ApresentaEcf
            {
                ModeloEcfTemplate = new ModeloEcfTemplate
                {
                    ObterModeloEcf = _ecf.Modelo,
                    ModeloAcbrEcf = modeloAcbr
                },
                Serie = _ecf.Serie,
                Numero = _ecf.NumeroEcf
            };

            var velocidade = "9600";

            if (!string.IsNullOrEmpty(_ecf.Velocidade))
            {
                velocidade = _ecf.Velocidade;
            }

            PortaSelecionada = _ecf.Porta;
            VelocidadeSelecionada = velocidade;
            Serie = _ecf.Serie;
            NumeroEcf = _ecf.NumeroEcf;

            Dispositivo.Porta = _ecf.Porta;
            Dispositivo.ControlePorta = _ecf.ControlePorta;
            Dispositivo.Velocidade = int.Parse(velocidade);
            Dispositivo.Modelo = _ecf.ModeloAcbr;
            Dispositivo.ModeloCompleto = _ecf.Modelo;
        }

        public bool ExisteCodigoNaEcf()
        {
            var naoEstaAtiva = _ecf.Id == 0;

            return naoEstaAtiva;
        }

        private EcfDt ObterEcf()
        {
            ObterEcfPorSerie();

            if (ModeloEcfSelecionado == null) return null;
            _ecf.ModeloAcbr = ModeloEcfSelecionado.ModeloEcfTemplate.ModeloAcbrEcf.ToString();
            _ecf.Modelo = ModeloEcfSelecionado.ModeloEcfTemplate.ObterModeloEcf;
            _ecf.Porta = PortaSelecionada;
            _ecf.Velocidade = VelocidadeSelecionada;
            _ecf.Serie = Serie;
            _ecf.NumeroEcf = NumeroEcf;
            _ecf.ControlePorta = ControlePorta;

            Dispositivo.Modelo = _ecf.ModeloAcbr;
            Dispositivo.Porta = _ecf.Porta;
            Dispositivo.ControlePorta = _ecf.ControlePorta;
            Dispositivo.Velocidade = int.Parse(_ecf.Velocidade);
            Dispositivo.ModeloCompleto = _ecf.Modelo;

            return _ecf;
        }

        private void ObterEcfPorSerie()
        {
            using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
            {
                _ecf = new EcfRepositorio(sessao).BuscarEcfPorSerie(Serie);
            }

            if (_ecf != null) return;

            Clipboard.SetText(Serie);
            throw new ArgumentException(
                "Serie da ECF incorreta\nNão existe ECF cadastrada com essa Serie\nPorfavor Ajustar\nDica Confira a serie com a ComboBox é o Texto Serie abaixo.\nEsta e a serie da ECF: " +
                Serie
                + "\nFoi feito uma copia para o clipboard (contrl + c) para você. \nOu seja Basta usar contrl + v");
        }

        public void Salvar()
        {
            try
            {
                if (!CriadorArquivoAuxiliar.ArquivoExiste)
                {
                    var criadorArquivo = new CriadorArquivoAuxiliar();
                    criadorArquivo.CriaArquivo();
                }

                PegarDadosEcf();

                new EcfAtivo().EstaAtiva();

                var ecfDt = ObterEcf();

                ecfDt.EmUso = 1;
                ecfDt.AlteradoEm = DateTime.Now;

                using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
                {
                    new EcfRepositorio(sessao).Salvar(ecfDt);
                }

                SessaoEcf.EcfFiscal.IdentificaPaf(
                    ResponsavelLegal.NomeAplicacaoPdv + " " + ResponsavelLegal.VersaoAplicacaoPdv, _md5);

                new FazerBackupArquivoAuxiliar("1").EfetuarBackup();
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (ExceptionMd5 ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        private void PegarDadosEcf()
        {
            var md5 = new GerarMd5();

            try
            {
                md5.Executar();
            }
            catch (AccessViolationException ex)
            {
                
            }

            _md5 = md5.Md5Final;

            try
            {
                new AtualizarMd5(md5.Md5Final).Executar();
            }
            catch (ExceptionMd5 ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (ArquivoAuxiliarInvalidoException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }

            Dispositivo.Porta = PortaSelecionada;
            Dispositivo.ControlePorta = ControlePorta;
            Dispositivo.Modelo = ModeloEcfSelecionado.ModeloEcfTemplate.ModeloAcbrEcf.ToString();
            Dispositivo.ModeloCompleto = ModeloEcfSelecionado.ModeloEcfTemplate.ObterModeloEcf;
            Dispositivo.Velocidade = int.Parse(VelocidadeSelecionada);

            SessaoEcf.EcfFiscal = CriaEcfFiscal.ObterEcfFiscal(ModeloEcfSelecionado.ModeloEcfTemplate.Instancia);
            SessaoEcf.EcfFiscal.Ativar();

            Serie = SessaoEcf.EcfFiscal.Serie();
            NumeroEcf = SessaoEcf.EcfFiscal.NumeroEcf();

            try
            {
                ObterEcfPorSerie();
                new AtualizarNumeroEcf(_numeroEcf).Executar();
                new AtualizarSerieEcf(_serie).Executar();
                new AtualizarGt(SessaoEcf.EcfFiscal.GrandeTotal().ToString(CultureInfo.CurrentCulture)).Executar();
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public bool ExisteEcfAtiva()
        {
            EcfDt ecf;

            using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
            {
                ecf = (EcfDt) new EcfRepositorio(sessao).BuscarEcfEmUso().FirstOrNull();
            }

            return ecf != null;
        }

        public void DesativarEcf()
        {
            try
            {
                _ecf.EmUso = 0;

                using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
                {
                    new EcfRepositorio(sessao).Salvar(_ecf);
                }

                ModeloEcfSelecionado = new ApresentaEcf
                {
                    ModeloEcfTemplate = new ModeloEcfTemplate
                    {
                        ModeloAcbrEcf = ModeloEmissor.Nenhum
                    },
                    Numero = "",
                    Serie = ""
                };
                PortaSelecionada = "";
                Serie = "";
                NumeroEcf = "";
                VelocidadeSelecionada = "9600";
                Dispositivo.Porta = "";
                Dispositivo.ControlePorta = false;
                Dispositivo.Modelo = "";
                Dispositivo.Velocidade = int.Parse(VelocidadeSelecionada);
                Dispositivo.ModeloCompleto = "";
                _ecf = new EcfDt();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public static IList<ApresentaEcf> BuscarListaDeEcfAtiva()
        {
            var todosModelos = new ListaModeloEcf().ObterModelosEcf();
            var listaComboBox = new List<ApresentaEcf>();


            try
            {
                IList<EcfDt> ecfs;
                using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
                {
                    ecfs = new EcfRepositorio(sessao).BuscarEcfsAtivos();
                }

                ecfs.ForEach(ecf =>
                {
                    listaComboBox.Add(new ApresentaEcf
                    {
                        ModeloEcfTemplate = todosModelos.First(modelo => modelo.ObterModeloEcf.Equals(ecf.Modelo)),
                        Serie = ecf.Serie,
                        Numero = ecf.NumeroEcf
                    });
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Ocorreu um erro na listagem de ecfs ativas. (ConfiguraçãoInicial)", ex);
            }

            return listaComboBox;
        }
    }
}