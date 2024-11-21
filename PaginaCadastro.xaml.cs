namespace PrototipoMapeamentoAPP;
using PrototipoMapeamentoAPP.Repository;
using PrototipoMapeamentoAPP.Model;
using PrototipoMapeamentoAPP.Configuracao;
public partial class PaginaCadastro : ContentPage
{
    private readonly PontoDeInteresseRepository _pontoDeInteresseRepository;
    private readonly Mapa _mapa;
    public PaginaCadastro(Mapa mapa)
    {
        InitializeComponent();
        _pontoDeInteresseRepository = new PontoDeInteresseRepository();
        _mapa = mapa;
    }

    private async void SalvarPonto(object sender, EventArgs e)
    {
        try
        {
            var pontoInteresse = new PontoDeInteresse();
            pontoInteresse.Nome = NomePonto.Text;
            double posicaoX = double.TryParse(PosicaoX.Text, out double posicaoXCast) ? posicaoXCast : throw new Exception("Valor para Posição X é inválido.");
            double posicaoY = double.TryParse(PosicaoY.Text, out double posicaoYCast) ? posicaoYCast : throw new Exception("Valor para Posição Y é inválido.");
            var destino = _mapa.ObterDestinoXY(posicaoX / ConfiguracaoDoMapa.DivisorPixelParaMatriz, posicaoY / ConfiguracaoDoMapa.DivisorPixelParaMatriz);
            pontoInteresse.PosicaoRealX = destino.X;
            pontoInteresse.PosicaoRealY = destino.Y;

             _pontoDeInteresseRepository.Criar(pontoInteresse);

            var pontos = await _pontoDeInteresseRepository.ObterPorFiltro("");

            await DisplayAlert("Sucesso", "Ponto de Interesse cadastrado!", "OK");

            NomePonto.Text = string.Empty;
            PosicaoX.Text = string.Empty;
            PosicaoY.Text = string.Empty;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Verifique os valores inseridos. " + ex.Message, "OK");
        }
    }   
    
    //private async void SalvarPonto(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //var pontoInteresse = new PontoDeInteresse();
    //        //float posicaoX = float.TryParse(PosicaoX.Text, out float posicaoXCast) ? posicaoXCast : throw new Exception("Valor para Posição X é inválido.");
    //        //float posicaoY = float.TryParse(PosicaoY.Text, out float posicaoYCast) ? posicaoYCast : throw new Exception("Valor para Posição Y é inválido."); 

    //        var pontoInteresse = new PontoDeInteresse();
    //        pontoInteresse.Nome = NomePonto.Text;
    //        pontoInteresse.PosicaoRealX = double.Parse(PosicaoX.Text);
    //        pontoInteresse.PosicaoRealY = double.Parse(PosicaoY.Text);

    //        _pontoDeInteresseRepository.Criar(pontoInteresse);

    //        await DisplayAlert("Sucesso", "Ponto de Interesse cadastrado!", "OK");

    //        NomePonto.Text = string.Empty;
    //        PosicaoX.Text = string.Empty;
    //        PosicaoY.Text = string.Empty;
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Erro", "Verifique os valores inseridos. " + ex.Message, "OK");
    //    }
    //}

    private async void VoltarTelaInicial(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}