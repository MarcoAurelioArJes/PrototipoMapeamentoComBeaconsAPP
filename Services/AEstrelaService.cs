using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using PrototipoMapeamentoAPP.Model;
using PrototipoMapeamentoAPP.Configuracao;

namespace PrototipoMapeamentoAPP.Services
{
    public class AEstrelaService
    {
        private Mapa _mapa;

        public AEstrelaService(Mapa mapa)
        {
            _mapa = mapa;
        }

        public List<(float X, float Y)> EncontrarCaminho(No inicio, No destino)
        {
            var nosInexplorados = new PriorityQueue<No, double>();
            var nosExplorados = new HashSet<No>();

            inicio.CustoCaminhoAtual = 0;
            inicio.CustoEstimadoAteDestino = EstimarCusto(inicio, destino);
            nosInexplorados.Enqueue(inicio, inicio.CustoTotal);

            while (nosInexplorados.Count > 0)
            {
                var noAtual = nosInexplorados.Dequeue();

                if (noAtual == destino)
                    return ReconstruirCaminho(destino);

                nosExplorados.Add(noAtual);
                var CustoCaminhoAteVizinho = noAtual.CustoCaminhoAtual + 1;

                foreach (var vizinho in ObterVizinhos(noAtual))
                {
                    if (nosExplorados.Contains(vizinho))
                        continue;

                    if (CustoCaminhoAteVizinho < vizinho.CustoCaminhoAtual)
                    {
                        vizinho.CustoCaminhoAtual = CustoCaminhoAteVizinho;
                        vizinho.CustoEstimadoAteDestino = EstimarCusto(vizinho, destino);
                        vizinho.Pai = noAtual;

                        nosInexplorados.Enqueue(vizinho, vizinho.CustoTotal);
                    }
                }
            }

            return null;
        }

        public List<(float X, float Y)> EncontrarCaminho2(No inicio, No destino)
        {
            var nosInexplorados = new List<No> { inicio };
            var nosExplorados = new HashSet<No>();

            inicio.CustoCaminhoAtual = 0;
            inicio.CustoEstimadoAteDestino = EstimarCusto(inicio, destino);

            while (nosInexplorados.Any())
            {
                var noAtual = nosInexplorados.OrderBy(n => n.CustoTotal).First();

                if (noAtual == destino)
                {
                    return ReconstruirCaminho(destino);
                }

                nosInexplorados.Remove(noAtual);
                nosExplorados.Add(noAtual);
                var CustoCaminhoAteVizinho = noAtual.CustoCaminhoAtual + 1;

                foreach (var vizinho in ObterVizinhos(noAtual))
                {
                    if (nosExplorados.Contains(vizinho))
                        continue;


                    if (!nosInexplorados.Contains(vizinho) || CustoCaminhoAteVizinho < vizinho.CustoCaminhoAtual)
                    {
                        vizinho.CustoCaminhoAtual = CustoCaminhoAteVizinho;
                        vizinho.CustoEstimadoAteDestino = EstimarCusto(vizinho, destino);
                        vizinho.Pai = noAtual;

                        if (!nosInexplorados.Contains(vizinho))
                            nosInexplorados.Add(vizinho);
                    }
                }
            }

            return null;
        }

        private List<(float X, float Y)> ReconstruirCaminho(No noAtual)
        {
            var caminho = new List<(float X, float Y)>();

            while (noAtual != null)
            {
                caminho.Add((noAtual.X * ConfiguracaoDoMapa.DivisorPixelParaMatriz, 
                            noAtual.Y * ConfiguracaoDoMapa.DivisorPixelParaMatriz));
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

            int x = no.X;
            int y = no.Y;

            int[,] direcoes = new int[,]
            {
              { 0, -1 }, { 0, 1 },
              { -1, 0 }, { 1, 0 },
            };

            for (int i = 0; i < direcoes.GetLength(0); i++)
            {
                int vizinhoX = x + direcoes[i, 0];
                int vizinhoY = y + direcoes[i, 1];


                //if (_mapa.ValidarPosicao(vizinhoX, vizinhoY) && _mapa.Nos[vizinhoX, vizinhoY].PodeAndar)
                //    vizinhos.Add(_mapa.Nos[vizinhoX, vizinhoY]);     

                if (!_mapa.ValidarPosicao(vizinhoX, vizinhoY))
                    continue;
                if (_mapa.Nos[vizinhoX, vizinhoY].PodeAndar)
                    vizinhos.Add(_mapa.Nos[vizinhoX, vizinhoY]);
            }

            return vizinhos;
        }
        //public void limparmapa()
        //{
        //    for (int x = 0; x < largurama; x++)
        //    {
        //        for (int y = 0; y < _gridmap.height; y++)
        //        {
        //            var node = _gridmap.nodes[x, y];
        //            node.gcost = double.positiveinfinity;
        //            node.hcost = 0;
        //            node.parent = null;
        //        }
        //    }
        //}
    }
}
