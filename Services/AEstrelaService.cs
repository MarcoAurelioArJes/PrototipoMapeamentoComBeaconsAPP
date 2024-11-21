using System;
using System.Collections.Generic;
using System.Linq;
using PrototipoMapeamentoAPP.Model;

namespace PrototipoMapeamentoAPP.Services
{
    public class AEstrelaService
    {
        private Mapa _mapa;

        public AEstrelaService(Mapa mapa)
        {
            _mapa = mapa;
        }

        public List<No> EncontrarCaminho(No inicio, No destino)
        {
            var caminho = new List<No>();
            var nosInexplorados = new SortedList<double, No>(); //Fila de prioridade
            var nosExplorados = new HashSet<No>();

            inicio.CustoCaminhoAtual = 0;
            inicio.CustoEstimadoAteDestino = EstimarCusto(inicio, destino);
            inicio.Pai = null;

            nosInexplorados.Add(inicio.CustoTotal, inicio);

            while (nosInexplorados.Any())
            {
                var noAtual = nosInexplorados.Values.First();
                nosInexplorados.RemoveAt(0);

                if (noAtual.X == destino.X && noAtual.Y == destino.Y)
                {
                    caminho = ReconstruirCaminho(noAtual);
                    break;
                }

                nosExplorados.Add(noAtual);
                var CustoCaminhoAteVizinho = noAtual.CustoCaminhoAtual + 1;

                foreach (var vizinho in ObterVizinhos(noAtual))
                {
                    if (nosExplorados.Contains(vizinho) || !vizinho.PodeAndar)
                        continue;

                    if (!nosInexplorados.ContainsValue(vizinho) || CustoCaminhoAteVizinho < vizinho.CustoCaminhoAtual)
                    {
                        vizinho.Pai = noAtual;
                        vizinho.CustoCaminhoAtual = CustoCaminhoAteVizinho;
                        vizinho.CustoEstimadoAteDestino = EstimarCusto(vizinho, destino);
                        vizinho.CustoTotal = vizinho.CustoCaminhoAtual + vizinho.CustoEstimadoAteDestino;

                        if (!nosInexplorados.ContainsValue(vizinho))
                        {
                            nosInexplorados.Add(vizinho.CustoTotal, vizinho);
                        }

                        //nosInexplorados.Add(vizinho.CustoTotal, vizinho);
                    }
                }
            }

            return caminho;
        }

        private List<No> ReconstruirCaminho(No destino)
        {
            var caminho = new List<No>();
            var noAtual = destino;

            while (noAtual.Pai != null)
            {
                caminho.Add(noAtual);
                noAtual = noAtual.Pai;
            }

            caminho.Reverse();
            return caminho;
        }

        private double EstimarCusto(No no, No destino)
        {
            return Math.Abs(no.X - destino.X) + Math.Abs(no.Y - destino.Y);
        }

        private List<No> ObterVizinhos(No no)
        {
            var vizinhos = new List<No>();

            var direcoes = new (int dx, int dy)[]
            {
                (-1, 0), (1, 0), (0, -1), (0, 1)
            };

            foreach (var (dx, dy) in direcoes)
            {
                var novoX = no.X + dx;
                var novoY = no.Y + dy;

                if (novoX >= 0 && novoX < Mapa.LarguraMapa && novoY >= 0 && novoY < Mapa.AlturaMapa)
                {
                    vizinhos.Add(_mapa.Nos[novoX, novoY]);
                }
            }

            return vizinhos;
        }
        //public void LimparMapa()
        //{
        //    for (int x = 0; x < larguraMa; x++)
        //    {
        //        for (int y = 0; y < _gridMap.Height; y++)
        //        {
        //            var node = _gridMap.Nodes[x, y];
        //            node.GCost = double.PositiveInfinity;
        //            node.HCost = 0;
        //            node.Parent = null;
        //        }
        //    }
        //}
    }
}
