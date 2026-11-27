namespace SudokuGrafos.Tests;

public class TabuleiroTests
{
    private readonly GrafoSudoku _grafo = new();

    [Fact]
    public void LerAceitaPontoEspacosEQuebrasDeLinha()
    {
        string texto = ". 2 3 4 5 6 7 8 9\n" + new string('0', 72);
        var cores = Tabuleiro.Ler(texto);

        Assert.NotNull(cores);
        Assert.Equal(81, cores!.Length);
        Assert.Equal(0, cores[0]);
        Assert.Equal(9, cores[8]);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    public void LerRejeitaTamanhoErrado(string texto) => Assert.Null(Tabuleiro.Ler(texto));

    [Fact]
    public void LerRejeita82Digitos() => Assert.Null(Tabuleiro.Ler(new string('0', 82)));

    [Fact]
    public void DetectaDicasEmConflito()
    {
        var cores = Tabuleiro.Ler("55" + new string('0', 79))!;
        Assert.False(Tabuleiro.DicasConsistentes(_grafo, cores));
    }

    [Theory]
    [InlineData(0, 30)]
    [InlineData(1, 21)]
    [InlineData(2, 17)]
    public void ContaDicasDosExemplos(int indice, int esperado)
    {
        var cores = Tabuleiro.Ler(Tabuleiro.Exemplos[indice].Valores)!;
        Assert.Equal(esperado, Tabuleiro.ContarDicas(cores));
        Assert.True(Tabuleiro.DicasConsistentes(_grafo, cores));
    }

    [Fact]
    public void DesenharMostraTodasAsLinhas()
    {
        var cores = Tabuleiro.Ler(Tabuleiro.Exemplos[0].Valores)!;
        var linhas = Tabuleiro.Desenhar(cores).Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal(13, linhas.Length);
        Assert.Contains("| 5 3 . | . 7 . | . . . |", linhas[1]);
    }
}
