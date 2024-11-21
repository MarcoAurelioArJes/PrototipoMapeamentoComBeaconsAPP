namespace PrototipoMapeamentoAPP;

public partial class PaginaCadastro : ContentPage
{
	public PaginaCadastro()
	{
		InitializeComponent();
	}

    private async void SalvarPonto(object sender, EventArgs e)
    {
        //try
        //{
        //    string nome = NomePonto.Text;
        //    float posicaoX = float.Parse(PosicaoX.Text);
        //    float posicaoY = float.Parse(PosicaoY.Text);

        //    // Adicione lógica para salvar o ponto (ex: salvar em uma lista ou banco de dados local)
        //    App.Current.Properties["Ponto_" + Guid.NewGuid()] = new { Nome = nome, X = posicaoX, Y = posicaoY };

        //    await DisplayAlert("Sucesso", "Ponto de Interesse cadastrado!", "OK");

        //    // Limpar os campos
        //    NomePonto.Text = string.Empty;
        //    PosicaoX.Text = string.Empty;
        //    PosicaoY.Text = string.Empty;
        //}
        //catch (Exception ex)
        //{
        //    await DisplayAlert("Erro", "Verifique os valores inseridos. " + ex.Message, "OK");
        //}
    }

    private async void VoltarTelaInicial(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}