using System;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Type;

namespace FusionCore.Repositorio.Filtros
{
    public class FiltroMdfe : ViewModel, IFiltro
    {
        private int? _numeroFiscal;
        private string _placaVeiculo;
        private bool _naoEncerrados;
        private EstadoDTO _estadoDescarregamento;
        private EstadoDTO _estadoCarregamento;
        private DateTime? _criadoEm;

        public FiltroMdfe()
        {
            CriadoEm = DateTime.Now.AddMonths(-1);
        }

        public DateTime? EmissaoInicial
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public DateTime? EmissaoFinal
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public string NomeEmitenteContenha
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int? NumeroFiscal
        {
            get => _numeroFiscal;
            set
            {
                _numeroFiscal = value;
                PropriedadeAlterada();
            }
        }

        public string PlacaVeiculo
        {
            get => _placaVeiculo;
            set
            {
                if (value == _placaVeiculo) return;
                _placaVeiculo = value;
                PropriedadeAlterada();
            }
        }

        public bool NaoEncerrados
        {
            get => _naoEncerrados;
            set
            {
                if (value == _naoEncerrados) return;
                _naoEncerrados = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO EstadoDescarregamento
        {
            get => _estadoDescarregamento;
            set
            {
                if (Equals(value, _estadoDescarregamento)) return;
                _estadoDescarregamento = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO EstadoCarregamento
        {
            get => _estadoCarregamento;
            set
            {
                if (Equals(value, _estadoCarregamento)) return;
                _estadoCarregamento = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? CriadoEm
        {
            get => _criadoEm;
            set
            {
                if (value.Equals(_criadoEm)) return;
                _criadoEm = value;
                PropriedadeAlterada();
            }
        }

        public void Aplicar<TRoot, TSub>(IQueryOver<TRoot, TSub> queryover)
        {
            //Nome da variável deve ser igual nome da informada no queryover
            EmpresaDTO tbEmpresa = null;
            MDFeEletronico tbMdfe = null;
            Veiculo veiculo = null;

            var and = Restrictions.Conjunction();

            if (CriadoEm != null)
            {
                and.Add(FiltroHelper.DataGe(() => tbMdfe.CriadoEm, CriadoEm.Value));
            }

            if (EmissaoInicial != null)
            {
                var restriction = Restrictions.Ge(
                    Projections.Cast(new DateType(), Projections.Property(() => tbMdfe.EmissaoEm)),
                    EmissaoInicial
                );

                and.Add(restriction);
            }

            if (EmissaoFinal != null)
            {
                var restriction = Restrictions.Le(
                    Projections.Cast(new DateType(), Projections.Property(() => tbMdfe.EmissaoEm)),
                    EmissaoFinal
                );

                and.Add(restriction);
            }

            if (!string.IsNullOrWhiteSpace(NomeEmitenteContenha))
            {
                var restriction = Restrictions.Like(
                    Projections.Property(() => tbEmpresa.RazaoSocial),
                    NomeEmitenteContenha,
                    MatchMode.Anywhere
                );

                and.Add(restriction);
            }

            if (NumeroFiscal != null)
            {
                var restricion = Restrictions.Eq(Projections.Property(() => tbMdfe.NumeroFiscalEmissao), NumeroFiscal);

                and.Add(restricion);
            }

            if (!string.IsNullOrEmpty(PlacaVeiculo))
            {
                var restricion = Restrictions.Eq(Projections.Property(() => veiculo.Placa), PlacaVeiculo);

                and.Add(restricion);
            }

            if (NaoEncerrados)
            {
                var restricion = Restrictions.Eq(Projections.Property(() => tbMdfe.Status), MDFeStatus.Autorizado);

                and.Add(restricion);
            }

            if (EstadoCarregamento != null)
            {
                var restricion = Restrictions.Eq(Projections.Property(() => tbMdfe.EstadoCarregamento), EstadoCarregamento);

                and.Add(restricion);
            }

            if (EstadoDescarregamento != null)
            {
                var restricion = Restrictions.Eq(Projections.Property(() => tbMdfe.EstadoDescarregamento), EstadoDescarregamento);

                and.Add(restricion);
            }

            queryover.And(and);
        }
    }
}