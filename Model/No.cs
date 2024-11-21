using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoMapeamentoAPP.Model
{
    public class No
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool PodeAndar { get; set; } = true;
        public double CustoCaminhoAtual { get; set; } = double.PositiveInfinity;
        public double CustoEstimadoAteDestino { get; set; } = 0;
        public double CustoTotal { get => CustoCaminhoAtual + CustoEstimadoAteDestino; set{} } 
        public No Pai { get; set; }

        public No(int x, int y, bool podeAndar)
        {
            X = x;
            Y = y;
            PodeAndar = podeAndar;
        }

    }
}
