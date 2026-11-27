namespace SudokuGrafos.Tests;

public class GrafoSudokuTests
{
    private readonly GrafoSudoku _grafo = new();

    [Fact]
    public void Tem81Vertices() => Assert.Equal(81, GrafoSudoku.TotalVertices);

    [Fact]
    public void Tem810Arestas() => Assert.Equal(810, _grafo.TotalArestas);

    [Fact]
    public void TodoVerticeTemGrau20()
    {
        for (int v = 0; v < GrafoSudoku.TotalVertices; v++)
            Assert.Equal(20, _grafo.Grau(v));
    }

    [Theory]
    [InlineData(0, 8)]
    [InlineData(0, 72)]
    [InlineData(0, 20)]
    [InlineData(80, 60)]
    public void CelulasRelacionadasTemAresta(int a, int b)
    {
        Assert.True(GrafoSudoku.TemRestricao(a, b));
        Assert.Contains(b, _grafo.Vizinhos(a));
        Assert.Contains(a, _grafo.Vizinhos(b));
    }

    [Theory]
    [InlineData(0, 30)]
    [InlineData(0, 40)]
    [InlineData(10, 80)]
    public void CelulasSemRelacaoNaoTemAresta(int a, int b)
    {
        Assert.False(GrafoSudoku.TemRestricao(a, b));
        Assert.DoesNotContain(b, _grafo.Vizinhos(a));
    }

    [Fact]
    public void VerticeNaoEVizinhoDeSiMesmo()
    {
        for (int v = 0; v < GrafoSudoku.TotalVertices; v++)
            Assert.DoesNotContain(v, _grafo.Vizinhos(v));
    }

    [Theory]
    [InlineData(0, "L1C1")]
    [InlineData(80, "L9C9")]
    [InlineData(10, "L2C2")]
    public void NomeCelula(int v, string esperado) => Assert.Equal(esperado, GrafoSudoku.NomeCelula(v));
}
