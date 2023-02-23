using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.Cupom.Nfce;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.ControlarNfces
{
    public class ListarTodasNfcesDados : ViewModel
    {
        private ObservableCollection<CupomFiscalDto> _cupons = new ObservableCollection<CupomFiscalDto>();
        private CupomFiscalDto _cupomSelecionado;
        private FiltroCupomFiscalDto _filtroCupomFiscal = new FiltroCupomFiscalDto();
        private EmitidaNo _emitidaNo;

        public ListarTodasNfcesDados()
        {
            EmitidaNo = EmitidaNo.TodosLocais;
        }

        public ObservableCollection<CupomFiscalDto> Cupons
        {
            get => _cupons;
            set
            {
                _cupons = value;
                PropriedadeAlterada();
            }
        }

        public CupomFiscalDto CupomSelecionado
        {
            get => _cupomSelecionado;
            set
            {
                _cupomSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public FiltroCupomFiscalDto FiltroCupomFiscal
        {
            get => _filtroCupomFiscal;
            set
            {
                if (Equals(value, _filtroCupomFiscal)) return;
                _filtroCupomFiscal = value;
                PropriedadeAlterada();
            }
        }

        public EmitidaNo EmitidaNo
        {
            get => _emitidaNo;
            set
            {
                _emitidaNo = value;
                PropriedadeAlterada();
            }
        }

        public void PesquisarNfces()
        {
            IList<CupomFiscalDto> nfcesPesquisadas = new List<CupomFiscalDto>();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioNfce = new RepositorioNfceAdm(sessao);

                if (EmitidaNo == EmitidaNo.TerminalOffline || EmitidaNo == EmitidaNo.TodosLocais)
                    nfcesPesquisadas = repositorioNfce.PesquisarNfces(FiltroCupomFiscal);

                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                var cupons = EmitidaNo == EmitidaNo.Faturamento || EmitidaNo == EmitidaNo.TodosLocais 
                    ? repositorioCupomFiscal.PesquisarCupons(FiltroCupomFiscal) 
                    : new List<CupomFiscalDto>();
                

                foreach (var cupomFiscalDto in nfcesPesquisadas)
                {
                    cupomFiscalDto.ResolveCupomSituacaoNfce();
                }

                foreach (var cupomFiscalDto in cupons)
                {
                    cupomFiscalDto.ResolveCupomSituacao();

                    if (cupomFiscalDto.IsContingencia)
                        cupomFiscalDto.Chave = repositorioCupomFiscal.BuscarChaveFiscalNosHistoricos(cupomFiscalDto.Id);

                    nfcesPesquisadas.Add(cupomFiscalDto);
                }

                nfcesPesquisadas = nfcesPesquisadas.OrderByDescending(x => x.CriadoEm).ToList();
            }

            Cupons = new ObservableCollection<CupomFiscalDto>(nfcesPesquisadas);
        }

        public IEnumerable<CupomFiscalSelecionado> ObterNfcesSelecionadas()
        {
            return _cupons.Where(x => x.IsSelecionado == true)
                .Select(c => new CupomFiscalSelecionado(c.Id, c.IsFaturamento, c.CupomSituacao,
                    c.NomeEmpresa, c.VendaId))
                .ToList();
        }
    }
}