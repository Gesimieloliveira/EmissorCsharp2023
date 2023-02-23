using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.ValidacaoAnotacao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Ncm
{
    public class NcmFormModel : ViewModel
    {
        private readonly NcmDTO _ncm;
        private bool _novoRegistro;

        public NcmFormModel(NcmDTO ncmDto)
        {
            _ncm = ncmDto;
            _novoRegistro = ncmDto.Id.IsNullOrEmpty();

            PreencherViewModel();
        }

        [Required(ErrorMessage = "Código NCM é obrigatório!")]
        public string Id
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Descrição do NCM é obrigatório!")]
        public string Descricao
        {
            get => GetValue();
            set => SetValue(value);
        }

        [TamanhoMinimo(7, ErrorMessage = "CEST deve ter 7 digitos")]
        public string Cest
        {
            get => GetValue();
            set => SetValue(value);
        }

        public void PreencherViewModel()
        {
            Id = _ncm.Id;
            Descricao = _ncm.Descricao;
            Cest = _ncm.Cest;
        }

        public void SalvarModel()
        {
            ThrowExceptionSeExistirErros();
            CodigoNcmSemLetras();

            _ncm.Id = Id;
            _ncm.Descricao = Descricao;
            _ncm.Cest = Cest ?? string.Empty;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioNcm(sessao);

                if (_novoRegistro && repositorio.JaExisteNcm(_ncm.Id))
                {
                    throw new InvalidOperationException("Código NCM Já cadastrado!");
                }

                repositorio.Salva(_ncm);

                _novoRegistro = false;
            }
        }

        private void CodigoNcmSemLetras()
        {
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                int.Parse(Id);
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException("Código NCM não pode ser vazio ou conter letras!");
            }

            Id = new Regex(@"[^\d]").Replace(Id, "");
        }

        public void DeletarModel()
        {
            if (string.IsNullOrEmpty(Id)) throw new InvalidOperationException("Não a nada para ser deletado por aqui, obrigado!");

            using (var repositorio = new RepositorioComun<NcmDTO>(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                repositorio.Deleta(_ncm);
            }
        }
    }
}
