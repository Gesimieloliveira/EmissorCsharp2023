using System.ComponentModel.DataAnnotations;
using FusionCore.FusionPdv.Flags;
using FusionCore.FusionPdv.Models;
using FusionLibrary.VisaoModel;
using FusionPdv.Servicos.Tef;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.Tef
{
    public class ConfiguraTefFormModel : ModelValidation
    {
        private readonly ManipulaTef _manipulaTef;
        private readonly EntidadeTef _entidadeTef;
        private OperadorasTef _operadoraTef;

        [Required(ErrorMessage = @"Porfavor preencher este campo")]
        public string ArqReq
        {
            get { return GetValue(() => ArqReq); }
            set { SetValue(value); }
        }

        [Required(ErrorMessage = @"Porfavor preencher este campo")]
        public string ArqResp
        {
            get { return GetValue(() => ArqResp); }
            set { SetValue(value); }
        }

        [Required(ErrorMessage = @"Porfavor preencher este campo")]
        public string ArqSts
        {
            get { return GetValue(() => ArqSts); }
            set { SetValue(value); }
        }

        [Required(ErrorMessage = @"Porfavor preencher este campo")]
        public string ArqTemp
        {
            get { return GetValue(() => ArqTemp); }
            set { SetValue(value); }
        }

        [Required(ErrorMessage = @"Porfavor preencher este campo")]
        public string GpExeName
        {
            get { return GetValue(() => GpExeName); }
            set { SetValue(value); }
        }

        private bool Ativo { get; set; }

        public OperadorasTef OperadoraTef
        {
            get { return _operadoraTef; }
            set
            {
                if (value == _operadoraTef) return;
                _operadoraTef = value;
                PropriedadeAlterada();
            }
        }

        public ConfiguraTefFormModel()
        {
            _manipulaTef = new ManipulaTef();
            _manipulaTef.CriarArquivoSeNaoExistir();

            _entidadeTef = _manipulaTef.LerArquivo();
            AtualizaModel();
        }

        private void AtualizaModel()
        {
            ArqReq = _entidadeTef.ArqReq;
            ArqResp = _entidadeTef.ArqResp;
            ArqSts = _entidadeTef.ArqSts;
            ArqTemp = _entidadeTef.ArqTemp;
            GpExeName = _entidadeTef.GpExeName;
            Ativo = _entidadeTef.Ativo;
            OperadoraTef = _entidadeTef.OperadoraTef;
        }

        public void Salvar()
        {
            _entidadeTef.ArqReq = ArqReq;
            _entidadeTef.ArqResp = ArqResp;
            _entidadeTef.ArqSts = ArqSts;
            _entidadeTef.ArqTemp = ArqTemp;
            _entidadeTef.GpExeName = GpExeName;
            _entidadeTef.OperadoraTef = OperadoraTef;
            _entidadeTef.Ativo = _entidadeTef.OperadoraTef != OperadorasTef.Nenhuma;


            _manipulaTef.SalvaXml(_entidadeTef);

            DialogBox.MostraInformacao("Configurações foram salvas com sucesso");
        }
    }
}