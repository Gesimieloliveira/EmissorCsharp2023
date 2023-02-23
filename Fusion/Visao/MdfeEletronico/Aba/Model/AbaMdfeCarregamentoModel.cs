using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fusion.Visao.MdfeEletronico.Aba.Entidades;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace Fusion.Visao.MdfeEletronico.Aba.Model
{
    public class RetornoModelEventArgs : EventArgs
    {
        public RetornoModelEventArgs(AbaMdfeCarregamentoModel model)
        {
            Model = model;
        }

        public AbaMdfeCarregamentoModel Model { get; set; }
    }

    public class AbaMdfeCarregamentoModel : ViewModel
    {
        private MDFeEletronico _mdfe;
        private bool _habilitado;
        private bool _selecionado;
        private decimal _valorTotalCarga;
        private ObservableCollection<GridLacre> _listaLacres;
        private GridLacre _itemSelecionadoLacre;
        private ObservableCollection<GridPercurso> _listaPercurso;
        private GridPercurso _itemSelecionadoPercurso;
        private ObservableCollection<GridMunicipioCarregamento> _listaMunicipioCarregamento;
        private GridMunicipioCarregamento _itemSelecionadoMunicipioCarregamento;
        private GridSeguroCarga _itemSelecionadoSeguroCarga;
        private ObservableCollection<GridSeguroCarga> _listaSeguroCarga;
        private bool _isCalcularTotalCargaAutomatico;
        private MDFeUnidadeMedida _unidadeMedida;
        private decimal _pesoBrutoCarga;
        private ObservableCollection<MdfeSeguroAverbacao> _averbacoes = new ObservableCollection<MdfeSeguroAverbacao>();

        public bool Habilitado
        {
            get { return _habilitado; }
            set
            {
                _habilitado = value;
                PropriedadeAlterada();
            }
        }

        public bool Selecionado
        {
            get { return _selecionado; }
            set
            {
                _selecionado = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridLacre> ListaLacres
        {
            get { return _listaLacres; }
            set
            {
                if (Equals(value, _listaLacres)) return;
                _listaLacres = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridPercurso> ListaPercurso
        {
            get { return _listaPercurso; }
            set
            {
                if (Equals(value, _listaPercurso)) return;
                _listaPercurso = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridMunicipioCarregamento> ListaMunicipioCarregamento
        {
            get { return _listaMunicipioCarregamento; }
            set
            {
                if (Equals(value, _listaMunicipioCarregamento)) return;
                _listaMunicipioCarregamento = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridSeguroCarga> ListaSeguroCarga
        {
            get { return _listaSeguroCarga; }
            set
            {
                if (Equals(value, _listaSeguroCarga)) return;
                _listaSeguroCarga = value;
                PropriedadeAlterada();
            }
        }

        public GridMunicipioCarregamento ItemSelecionadoMunicipioCarregamento
        {
            get { return _itemSelecionadoMunicipioCarregamento; }
            set
            {
                if (Equals(value, _itemSelecionadoMunicipioCarregamento)) return;
                _itemSelecionadoMunicipioCarregamento = value;
                PropriedadeAlterada();
            }
        }

        public GridLacre ItemSelecionadoLacre
        {
            get { return _itemSelecionadoLacre; }
            set
            {
                if (Equals(value, _itemSelecionadoLacre)) return;
                _itemSelecionadoLacre = value;
                PropriedadeAlterada();
            }
        }

        public GridPercurso ItemSelecionadoPercurso
        {
            get { return _itemSelecionadoPercurso; }
            set
            {
                if (Equals(value, _itemSelecionadoPercurso)) return;
                _itemSelecionadoPercurso = value;
                PropriedadeAlterada();
            }
        }

        public GridSeguroCarga ItemSelecionadoSeguroCarga
        {
            get { return _itemSelecionadoSeguroCarga; }
            set
            {
                if (Equals(value, _itemSelecionadoSeguroCarga)) return;
                _itemSelecionadoSeguroCarga = value;
                PropriedadeAlterada();
            }
        }

        public MDFeTipoEmitente TipoEmitente
        {
            get => GetValue<MDFeTipoEmitente>();
            private set => SetValue(value);
        }

        public decimal ValorTotalCarga
        {
            get { return _valorTotalCarga; }
            set
            {
                _valorTotalCarga = value;
                PropriedadeAlterada();
            }
        }

        public int QuantidadeDeCtes
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int QuantidadeDeNfes
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public ObservableCollection<MDFeDescarregamento> ListaDeDescarregamento
        {
            get => GetValue<ObservableCollection<MDFeDescarregamento>>();
            set => SetValue(value);
        }

        public MDFeDescarregamento DescarregamentoSelecionado
        {
            get => GetValue<MDFeDescarregamento>();
            set => SetValue(value);
        }

        public bool IsCalcularTotalCargaAutomatico
        {
            get => _isCalcularTotalCargaAutomatico;
            set
            {
                _isCalcularTotalCargaAutomatico = value;
                PropriedadeAlterada();

                SalvarConfiguracao();
            }
        }

        public MDFeUnidadeMedida UnidadeMedida
        {
            get { return _unidadeMedida; }
            set
            {
                if (value == _unidadeMedida) return;
                _unidadeMedida = value;
                PropriedadeAlterada();
            }
        }

        public decimal PesoBrutoCarga
        {
            get { return _pesoBrutoCarga; }
            set
            {
                _pesoBrutoCarga = value;
                PropriedadeAlterada();
            }
        }

        private void SalvarConfiguracao()
        {
            AtualizaTotal();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _mdfe.IsCalcularTotalCargaAutomatico = IsCalcularTotalCargaAutomatico;
                new RepositorioMdfe(sessao).Salvar(_mdfe);

                transacao.Commit();
            }
        }

        public AbaMdfeCarregamentoModel()
        {
            UnidadeMedida = MDFeUnidadeMedida.KG;
            ListaDeDescarregamento = new ObservableCollection<MDFeDescarregamento>();
            ListaLacres = new ObservableCollection<GridLacre>();
            ListaPercurso = new ObservableCollection<GridPercurso>();
            ListaMunicipioCarregamento = new ObservableCollection<GridMunicipioCarregamento>();
            ListaSeguroCarga = new ObservableCollection<GridSeguroCarga>();
        }

        public event EventHandler AnteriorHandler;
        public event EventHandler AbrirFlyoutAddSeguroHandler;
        public event EventHandler AbrirFlyoutAddLacreHandler;
        public event EventHandler AbrirFlyoutAddPercursoHandler;
        public event EventHandler AbrirFlyoutAddMunicipioCarregamentoHandler;
        public event EventHandler<RetornoModelEventArgs> ProximoHandler; 

        public void ThrowExceptionSeInvalido()
        {
            if (ListaMunicipioCarregamento.Count == 0)
                throw new InvalidOperationException("MDF-e deve ter no mínimo um municipio de carregamento");

            if (QuantidadeDeCtes == 0 && QuantidadeDeNfes == 0) 
                throw new InvalidOperationException("MDF-e deve conter no mínimo um documento de descarregamento");

            if (_mdfe.TipoEmitente == MDFeTipoEmitente.PrestadorServicoDeTransporte && QuantidadeDeNfes > 0)
                throw new InvalidOperationException("MDF-e para Prestação de Serviço de Transporte deve conter apenas CT-e");

            if (_mdfe.TipoEmitente == MDFeTipoEmitente.TransportadorDeCargaPropria && QuantidadeDeCtes > 0)
                throw new InvalidOperationException("MDF-e para Transporte de Carga Própria deve conter apenas NF-e");
        }

        public void Anterior()
        {
            OnAnteriorHandler();
        }

        protected virtual void OnAnteriorHandler()
        {
            AnteriorHandler?.Invoke(this, EventArgs.Empty);
        }

        public void AbrirFlyoutAddSeguro()
        {
            OnAbrirFlyoutAddSeguro();
        }

        public void ComMdfe(MDFeEletronico mdfe)
        {
            _mdfe = mdfe;

            TipoEmitente = mdfe.TipoEmitente;
            PesoBrutoCarga = mdfe.PesoBrutoCarga;
            ValorTotalCarga = mdfe.ValorTotalCarga;
            UnidadeMedida = mdfe.UnidadeMedida;
            IsCalcularTotalCargaAutomatico = mdfe.IsCalcularTotalCargaAutomatico;

            LoadMunicipioCarregamento(mdfe.MunicipioCarregamentos);
            LoadLacres(mdfe.Lacres);
            LoadPercurso(mdfe.Percursos);
            LoadSeguroCarga(mdfe.SeguroCargas);

            LoadDescarregamento();
        }

        public void AbrirFlyoutAddLacre()
        {
            OnAbrirFlyoutAddLacreHandler();
        }

        protected virtual void OnAbrirFlyoutAddLacreHandler()
        {
            AbrirFlyoutAddLacreHandler?.Invoke(this, EventArgs.Empty);
        }

        public void AdicionarLacre(GridLacre lacre)
        {
            ListaLacres.Add(lacre);
        }

        public void DeletaLacreSelecionado()
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);
                repositorio.DeletarLacre(ItemSelecionadoLacre.Lacre);
                transacao.Commit();
            }

            _mdfe.Lacres.Remove(ItemSelecionadoLacre.Lacre);

            ListaLacres.Remove(ItemSelecionadoLacre);
        }

        protected virtual void OnAbrirFlyoutAddPercursoHandler()
        {
            AbrirFlyoutAddPercursoHandler?.Invoke(this, EventArgs.Empty);
        }

        public void AbrirFlyoutAddPercurso()
        {
            OnAbrirFlyoutAddPercursoHandler();
        }

        public void AdicionarPercurso(GridPercurso percurso)
        {
            ListaPercurso.Add(percurso);
        }

        public void DeletarPercursoSelecionado()
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);
                repositorio.DeletarPercurso(ItemSelecionadoPercurso.Percurso);
                transacao.Commit();
            }

            _mdfe.Percursos.Remove(ItemSelecionadoPercurso.Percurso);

            ListaPercurso.Remove(ItemSelecionadoPercurso);
        }

        public void Proximo()
        {
            PesoBrutoCarga = Convert.ToDecimal(PesoBrutoCarga.ToString("N4"));
            ValorTotalCarga = Convert.ToDecimal(ValorTotalCarga.ToString("N2"));

            if (PesoBrutoCarga < 0) throw new ArgumentException("Peso bruto da carga não pode ser negativo");
            if (ValorTotalCarga < 0) throw new ArgumentException("Valor total carga não pode ser negativo");

            OnProximoHandler();
        }

        protected virtual void OnProximoHandler()
        {
            ProximoHandler?.Invoke(this, new RetornoModelEventArgs(this));
        }

        public void AbrirFlyoutMunicipioCarregamento()
        {
            OnAbrirFlyoutAddMunicipioCarregamentoHandler();
        }

        protected virtual void OnAbrirFlyoutAddMunicipioCarregamentoHandler()
        {
            AbrirFlyoutAddMunicipioCarregamentoHandler?.Invoke(this, EventArgs.Empty);
        }

        public void AddMunicipioCarregamento(GridMunicipioCarregamento municipioCarregamento)
        {
            ListaMunicipioCarregamento.Add(municipioCarregamento);
        }

        public void DeletarMunicipioCarregamentoSelecionado()
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);
                repositorio.DeletarMunicipioCarregamento(ItemSelecionadoMunicipioCarregamento.MunicipioCarregamento);
                transacao.Commit();
            }

            _mdfe.MunicipioCarregamentos.Remove(ItemSelecionadoMunicipioCarregamento.MunicipioCarregamento);

            ListaMunicipioCarregamento.Remove(ItemSelecionadoMunicipioCarregamento);
        }

        private void LoadDescarregamento()
        {
            ListaDeDescarregamento = new ObservableCollection<MDFeDescarregamento>(_mdfe.Descarregamentos.OrderByDescending(x => x.Id));

            QuantidadeDeNfes = _mdfe.QuantidadeNFe;
            QuantidadeDeCtes = _mdfe.QuantidadeCTe;

            AtualizaTotal();
        }

        private void AtualizaTotal()
        {
            if (IsCalcularTotalCargaAutomatico == false) return;

            if (_mdfe.TipoEmitente == MDFeTipoEmitente.TransportadorDeCargaPropria)
                ValorTotalCarga = ListaDeDescarregamento.Where(x => x.ModeloDocumento == ModeloDocumento.NFe)
                    .Sum(x => x.ValorTotal);

            if (_mdfe.TipoEmitente == MDFeTipoEmitente.PrestadorServicoDeTransporte)
                ValorTotalCarga = ListaDeDescarregamento.Where(x => x.ModeloDocumento == ModeloDocumento.CTe)
                    .Sum(x => x.ValorTotal);

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _mdfe.ValorTotalCarga = ValorTotalCarga;
                new RepositorioMdfe(sessao).Salvar(_mdfe);

                transacao.Commit();
            }
        }

        private void LoadSeguroCarga(IList<MDFeSeguroCarga> seguroCargas)
        {
            ListaSeguroCarga.Clear();

            seguroCargas.ForEach(s =>
            {
                var item = new GridSeguroCarga
                {
                    CnpjResponsavel = s.CnpjResponsavel,
                    CpfResponsavel = s.CpfResponsavel,
                    NomeSeguradora = s.NomeSeguradora,
                    CnpjSeguradora = s.CnpjSeguradora,
                    NumeroApolice = s.NumeroApolice,
                    ResponsavelSeguro = s.Responsavel,
                    MDFeSeguroCarga = s
                };

                s.Averbacoes.ForEach(x =>
                {
                    item.Averbacoes.Add(new MdfeSeguroAverbacao
                    {
                        Averbacao = x.Averbacao,
                        Id = x.Id,
                        SeguroCarga = s
                    });
                });

                ListaSeguroCarga.Add(item);
            });
        }

        private void LoadMunicipioCarregamento(IEnumerable<MDFeMunicipioCarregamento> municipioCarregamentos)
        {
            ListaMunicipioCarregamento.Clear();

            municipioCarregamentos?.ForEach(m =>
            {
                ListaMunicipioCarregamento.Add(new GridMunicipioCarregamento
                {
                    Cidade = m.Cidade,
                    MunicipioCarregamento = m
                });
            });
        }

        private void LoadLacres(IEnumerable<MDFeLacre> lacres)
        {
            ListaLacres.Clear();

            lacres?.ForEach(l =>
            {
                ListaLacres.Add(new GridLacre
                {
                    Numero = l.Numero,
                    Lacre = l
                });
            });
        }

        private void LoadPercurso(IEnumerable<MDFePercurso> percursos)
        {
            ListaPercurso.Clear();

            percursos?.ForEach(p =>
            {
                ListaPercurso.Add(new GridPercurso
                {
                    Percurso = p,
                    EstadoUf = p.Estado
                });
            });
        }

        protected virtual void OnAbrirFlyoutAddSeguro()
        {
            AbrirFlyoutAddSeguroHandler?.Invoke(this, EventArgs.Empty);
        }

        public void DeletarSeguroCargaSelecionado()
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);
                repositorio.DeletarSeguroCarga(ItemSelecionadoSeguroCarga.MDFeSeguroCarga);
                transacao.Commit();
            }

            _mdfe.SeguroCargas.Remove(ItemSelecionadoSeguroCarga.MDFeSeguroCarga);

            ListaSeguroCarga.Remove(ItemSelecionadoSeguroCarga);
        }

        public void AdicionarDescarregamento(MDFeDescarregamento novo)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _mdfe.AdicionarDescarregamento(novo);

                new RepositorioMdfe(sessao).Salvar(_mdfe);

                transacao.Commit();
            }

            LoadDescarregamento();
        }

        public void DeletarDescarregamentoSelecionado()
        {
            if (DescarregamentoSelecionado == null)
            {
                throw new InvalidOperationException("Nenhum descarregamento foi selecionado!");
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                _mdfe.RemoverDescarregamento(DescarregamentoSelecionado);

                new RepositorioMdfe(sessao).Salvar(_mdfe);

                transacao.Commit();
            }

            DescarregamentoSelecionado = null;
            LoadDescarregamento();
        }

        public bool ChaveDescarregamentoJaInformada(string chave)
        {
            return _mdfe.Descarregamentos.Any(i => i.ChaveDocumento == chave);
        }
    }
}