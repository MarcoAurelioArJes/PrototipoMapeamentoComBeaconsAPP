using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoMapeamentoAPP.Services
{
    /// <summary>
    /// Suavização de Média Móvel Simples, suaviliza os ultimos N valores. Sendo N o tamanho da janela.
    /// </summary>
    public class SuavizacaoSMA
    {
        private int _tamanhoJanela;
        private Queue<double> _valores;

        public SuavizacaoSMA(int tamanhoJanela)
        {
            _tamanhoJanela = tamanhoJanela;
            _valores = new Queue<double>();
        }

        public double AdicionarValor(double novoValor)
        {
            if (_valores.Count >= _tamanhoJanela)
                _valores.Dequeue();

            _valores.Enqueue(novoValor);

            return _valores.Average();
        }
    }

    /// <summary>
    /// Suavização de Média Móvel Exponencial, aplica mais peso aos valores recente.
    /// </summary>
    public class SuavizacaoEMA
    {
        private double _alpha;
        private double? _valorSuavizadoAnterior;


        public SuavizacaoEMA(double alpha)
        {
            if (alpha <= 0 || alpha > 1)
                throw new ArgumentException("Suavização EMA: Alpha deve estar entre 0 e 1.", nameof(alpha));

            _alpha = alpha;
            _valorSuavizadoAnterior = null;
        }

        public double AdicionarValor(double novoValor)
        {
            if (_valorSuavizadoAnterior == null)
                _valorSuavizadoAnterior = novoValor;

            double valorSuavizado = _alpha * novoValor + (1 - _alpha) * _valorSuavizadoAnterior.Value;
            _valorSuavizadoAnterior = valorSuavizado;

            return valorSuavizado;
        }
    }

    /// <summary>
    /// Média Móvel Ponderada aplica pesos diferentes aos valores na janela, frequentemente dando mais peso às leituras recentes.
    /// </summary>
    public class SuavizacaoMMP
    {
        private int _tamanhoJanela;
        private Queue<double> _valores;

        public SuavizacaoMMP(int tamanhoJanela)
        {
            _tamanhoJanela = tamanhoJanela;
            _valores = new Queue<double>();
        }

        public double AdicionarValor(double novoValor)
        {
            _valores.Enqueue(novoValor);

            if (_valores.Count > _tamanhoJanela)
                _valores.Dequeue();

            int n = _valores.Count;
            double denominador = n * (n + 1) / 2.0;
            double weightedSum = _valores.Select((value, index) => value * (index + 1)).Sum();

            return weightedSum / denominador;
        }
    }

    /// <summary>
    /// Filtro de Kalman considera o ruído de medição e de processo
    /// </summary>
    public class FiltroKalman
    {
        private double _valorEstimadoAnterior;
        private double _erroEstimadoAnterior;
        private double _varianciaProcesso;
        private double _varianciaMedicao;

        public FiltroKalman(double estimativaInicial, double erroEstimadoInicial = 1, double varianciaProcesso = 1, double varianciaMedicao = 4)
        {
            _valorEstimadoAnterior = estimativaInicial;
            _erroEstimadoAnterior = erroEstimadoInicial;
            _varianciaProcesso = varianciaProcesso;
            _varianciaMedicao = varianciaMedicao;
        }

        public double AdicionarValor(double novoValor)
        {
            double valorEstimado = _valorEstimadoAnterior;
            double erroEstimado = _erroEstimadoAnterior + _varianciaProcesso;

            double kalmanGain = erroEstimado / (erroEstimado + _varianciaMedicao);
            double valorEstimadoAtual = valorEstimado + kalmanGain * (novoValor - valorEstimado);
            double erroEstimadoAtual = (1 - kalmanGain) * erroEstimado;

            _valorEstimadoAnterior = valorEstimadoAtual;
            _erroEstimadoAnterior = erroEstimadoAtual;

            return valorEstimadoAtual;
        }
    }

    /// <summary>
    /// O Controlador PID ajusta a estimativa com base em termos proporcionais, integrais e derivativos.
    /// </summary>
    public class RSSIPIDControllerService
    {
        private double _kp;
        private double _ki;
        private double _kd;
        private double _previousError;
        private double _integral;
        private double? _setpoint;
        public RSSIPIDControllerService(double kp, double ki, double kd)
        {
            _kp = kp;
            _ki = ki;
            _kd = kd;
        }

        /// <summary>
        /// Adiciona um novo valor RSSI e computa o valor ajustado usando controle PID.
        /// </summary>
        /// <param name="novoValor">A nova medição de RSSI.</param>
        /// <returns>O valor RSSI suavizado.</returns>
        public double AdicionarValor(double novoValor)
        {
            if (_setpoint == null)
            {
                _setpoint = novoValor;
                return novoValor;
            }

            double error = _setpoint.Value - novoValor;
            _integral += error;
            double derivative = error - _previousError;

            double output = _kp * error + _ki * _integral + _kd * derivative;

            _previousError = error;

            _setpoint = novoValor + output;
            return _setpoint.Value;
        }
    }

}
