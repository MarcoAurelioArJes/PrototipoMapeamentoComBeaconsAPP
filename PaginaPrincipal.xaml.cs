using PrototipoMapeamentoAPP.Model;
using PrototipoMapeamentoAPP.Services;
using System.Diagnostics;

namespace PrototipoMapeamentoAPP;

public partial class PaginaPrincipal : ContentPage
{
    //private List<string> items;
    private Mapa _mapa;
    private AEstrelaService aEstrelaService;

    public PaginaPrincipal()
    {
        InitializeComponent();

        canvasView.Drawable = new MapaDrawable();

        //items  = new List<string>
        //{
        //        "Maçã"
        //};

        //listView.ItemsSource = items;

        _mapa = new Mapa();
        aEstrelaService = new AEstrelaService(_mapa);
    }


    //private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    //{
    //    string searchText = e.NewTextValue?.ToLower() ?? string.Empty;
    //    var filteredItems = items.Where(item => item.ToLower().Contains(searchText)).ToList();
    //    listView.ItemsSource = filteredItems;
    //}

    //private void OnSearchButtonPressed(object sender, EventArgs e)
    //{
    //    string searchText = searchBar.Text?.ToLower() ?? string.Empty;
    //    var filteredItems = items.Where(item => item.ToLower().Contains(searchText)).ToList();
    //    listView.ItemsSource = filteredItems;
    //}

    private void AoClicarEmAtualizarPosicaoDoUsuario(object sender, EventArgs e)
    {
        ConfiguracaoDoMapa.PosicaoDoUsuarioX = float.Parse(PosicaoX.Text);
        ConfiguracaoDoMapa.PosicaoDoUsuarioY = float.Parse(PosicaoY.Text);

        //_mapa.ExibirMapa();

        var posicaoDoUsuario = new No((int)ConfiguracaoDoMapa.PosicaoDoUsuarioX, (int)ConfiguracaoDoMapa.PosicaoDoUsuarioY, true);
        var destino = new No(10, 10, true);
        var caminho = aEstrelaService.EncontrarCaminho(posicaoDoUsuario, destino);

        for (int i = 0; i < caminho.Count; i++)
        {
            var no = caminho[i];
            Trace.WriteLine(no.PodeAndar ? "C" : "O");
        }

        //if (caminho.Any())
        //{
        //    // Obter o contexto de desenho
        //    var drawable = canvasView.Drawable as MapaDrawable;

        //    // Itera sobre cada nó no caminho encontrado
        //    foreach (var no in caminho)
        //    {
        //        // Chama um método do MapaDrawable para desenhar o nó
        //        drawable?.DesenharNo(no);
        //    }
        //}

        canvasView.Invalidate();
    }
}