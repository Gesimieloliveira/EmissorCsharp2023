using System.ComponentModel.DataAnnotations;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.ProdutoGrupo
{
    public class ProdutoGrupoFormModel : ViewModel
    {
        private readonly ProdutoGrupoDTO _grupo;

        public ProdutoGrupoFormModel(ProdutoGrupoDTO produtoGrupo)
        {
            _grupo = produtoGrupo;
            if (_grupo.Id == 0)
            {
                NovoRegistro = true;
            }
        }

        [Required(ErrorMessage = @"Nome do grupo é obrigatório")]
        public string Nome
        {
            get => GetValue();
            set => SetValue(value);
        }

        public bool NovoRegistro
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void Inicializa()
        {
            Nome = _grupo.Nome;
        }

        public void SalvarModel()
        {
            ThrowExceptionSeExistirErros();

            _grupo.Nome = Nome;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioComun<ProdutoGrupoDTO>(sessao);
                repositorio.Salva(_grupo);
            }
        }

        public void DeletarModel()
        {
            if (_grupo.Id == 0) return;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioComun<ProdutoGrupoDTO>(sessao);
                repositorio.Deleta(_grupo);
            }
        }
    }
}