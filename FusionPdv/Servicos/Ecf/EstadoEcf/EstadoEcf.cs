using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionPdv.Ecf;

namespace FusionPdv.Servicos.Ecf.EstadoEcf
{
    public class EstadoEcf
    {
        private readonly EstadoEcfFiscal _estadoEcf;
        public VendaEcfDt VendaEmAndamento { get; set; }

        public EstadoEcf() { }

        public EstadoEcf(EstadoEcfFiscal estadoEcf)
        {
            _estadoEcf = estadoEcf;
        }

        public void ProcessaEstadoEcf()
        {
            var naoInicializado = new NaoInicializada();
            var desconhecido = new Desconhecido();
            var bloqueada = new Bloqueada();
            var vendaOuPagamento = new VendaOuPagamento();
            var leituraX = new RequerLeituraX();
            var leituraZ = new RequerLeituraZ();

            naoInicializado.Proximo(desconhecido);
            desconhecido.Proximo(bloqueada);
            bloqueada.Proximo(vendaOuPagamento);
            vendaOuPagamento.Proximo(leituraZ);
            leituraZ.Proximo(leituraX);

            naoInicializado.Verificar();
            VendaEmAndamento = vendaOuPagamento.VendaEcfDt;
        }

        public void ValidarEstadoDoEcf()
        {
            var naoInicializado = new NaoInicializada(_estadoEcf);
            var desconhecido = new Desconhecido(_estadoEcf);
            var bloqueada = new Bloqueada(_estadoEcf);
            var leituraZ = new RequerLeituraZ(_estadoEcf);

            naoInicializado.Proximo(desconhecido);
            desconhecido.Proximo(bloqueada);
            bloqueada.Proximo(leituraZ);

            naoInicializado.Verificar();
        }
    }
}
