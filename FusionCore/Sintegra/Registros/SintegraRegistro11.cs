using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using SintegraBr.Classes;

namespace FusionCore.Sintegra.Registros
{
    public class SintegraRegistro11
    {
        private readonly EmpresaDTO _empresa;

        public SintegraRegistro11(EmpresaDTO empresa)
        {
            _empresa = empresa;
        }

        public Registro11 MontarRegistro11()
        {
            var r11 = new Registro11(
                _empresa.Logradouro.TrimSefaz(34),
                _empresa.Numero.TrimSefaz(5),
                _empresa.Complemento.TrimSefaz(22),
                _empresa.Bairro.TrimSefaz(15),
                _empresa.Cep.TrimSefaz(8),
                _empresa.NomeFantasia.TrimSefaz(38),
                _empresa.Fone1.TrimSefaz(12)
                );

            return r11;
        }
    }
}