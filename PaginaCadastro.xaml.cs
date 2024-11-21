namespace PrototipoMapeamentoAPP;

public partial class PaginaCadastro : ContentPage
{
	public PaginaCadastro()
	{
		InitializeComponent();
	}

    private async void SalvarPonto(object sender, EventArgs e)
    {
        try
        {
            string nome = NomePonto.Text;
            float posicaoX = float.TryParse(PosicaoX.Text, out float posicaoXCast) ? posicaoXCast : throw new Exception("Valor para Posição X é inválido.");
            float posicaoY = float.TryParse(PosicaoY.Text, out float posicaoYCast) ? posicaoYCast : throw new Exception("Valor para Posição Y é inválido."); ;

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

    private async void VoltarTelaInicial(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}