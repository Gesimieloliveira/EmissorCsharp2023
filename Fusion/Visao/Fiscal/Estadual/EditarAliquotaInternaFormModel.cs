using FusionCore.FusionAdm.Sessao;
using FusionCore.Tributacoes.Estadual;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Fiscal.Estadual
{
    public class EditarAliquotaInternaFormModel : ViewModel
    {
        private readonly AliquotaInterna _aliquotaInterna;
        private string _nomeEstadoUf;
        private decimal _aliquota;
        private decimal _aliquotaFcp;

        public EditarAliquotaInternaFormModel(AliquotaInterna aliquotaInterna)
        {
            _aliquotaInterna = aliquotaInterna;

            NomeEstadoUf = _aliquotaInterna.EstadoUf.Nome;
            Aliquota = _aliquotaInterna.Aliquota;
            AliquotaFcp = _aliquotaInterna.AliquotaFcp;
        }

        public string NomeEstadoUf
        {
            get => _nomeEstadoUf;
            set
            {
                _nomeEstadoUf = value;
                PropriedadeAlterada();
            }
        }

        public decimal Aliquota
        {
            get => _aliquota;
            set
            {
                _aliquota = value;
                PropriedadeAlterada();
            }
        }

        public decimal AliquotaFcp
        {
            get => _aliquotaFcp;
            set
            {
                if (value == _aliquotaFcp) return;
                _aliquotaFcp = value;
                PropriedadeAlterada();
            }
        }

        public void Salvar()
        {
            _aliquotaInterna.Aliquota = Aliquota;
            _aliquotaInterna.AliquotaFcp = AliquotaFcp;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                new RepositorioAliquotaInterna(sessao).Alterar(_aliquotaInterna);

                transacao.Commit();
            }

            OnFechar();   
        }
    }
}