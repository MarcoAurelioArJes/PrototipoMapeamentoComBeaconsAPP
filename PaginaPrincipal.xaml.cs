using PrototipoMapeamentoAPP.Configuracao;

namespace PrototipoMapeamentoAPP;

public partial class PaginaPrincipal : ContentPage
{
    //private List<string> items;

    public PaginaPrincipal()
    {
        InitializeComponent();

        canvasView.Drawable = new MapaDrawable();

        //items  = new List<string>
        //{
        //        "Maçã"
        //};

        //listView.ItemsSource = items;
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

        canvasView.Invalidate();
    }
}