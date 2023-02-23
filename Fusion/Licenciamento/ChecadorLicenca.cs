using System;
using System.ServiceModel;
using System.Threading;
using System.Timers;
using Fusion.Factories;
using Fusion.Sessao;
using FusionCore.Seguranca.Licenciamento.Dominio;
using Timer = System.Timers.Timer;

namespace Fusion.Licenciamento
{
    public class ChecadorLicenca
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private readonly Timer _timer;
        private static ChecadorLicenca _intancia;
        private AcessoConcedido _acessoChecar;

        private ChecadorLicenca()
        {
            _timer = new Timer(ConstantesLicenciamento.IntervaloChecadorSegundos * 1000);
            _timer.Elapsed += TimerChecadorElapsed;
        }

        public static ChecadorLicenca Instancia => _intancia ?? (_intancia = new ChecadorLicenca());
        public event EventHandler<Exception> ChecagemErro;

        public void IniciarChecagem(AcessoConcedido acesso)
        {
            _acessoChecar = acesso;

            if (_timer.Enabled) return;
            _timer.Start();
        }

        public void ParaChecagem()
        {
            _timer?.Stop();
        }

        private void TimerChecadorElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            try
            {
                /*var tentativa = 0;

                do
                {
                    try
                    {
                        // TODO 1612 - Checagem de licença válida 
                    }
                    catch (FaultException)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        if (++tentativa >= ConstantesLicenciamento.NumeroTenativas)
                        {
                            throw;
                        }

                        Thread.Sleep(ConstantesLicenciamento.IntervaloTentativaSegundos * 1000);
                    }
                } while (true);*/
            }
            catch (Exception ex)
            {
                ChecagemErro?.Invoke(this, ex);
            }
            finally
            {
                _timer.Start();
            }
        }
    }
}