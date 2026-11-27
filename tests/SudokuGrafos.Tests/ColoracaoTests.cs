namespace SudokuGrafos.Tests;

public class ColoracaoTests
{
    private readonly GrafoSudoku _grafo = new();

    public static IEnumerable<object[]> Exemplos() =>
        Tabuleiro.Exemplos.Select(e => new object[] { e.Nome, e.Valores });

    [Theory]
    [MemberData(nameof(Exemplos))]
    public void HeuristicaResolveExemplosMantendoDicas(string nome, string valores)
    {
        var dicas = Tabuleiro.Ler(valores)!;
        var cores = (int[])dicas.Clone();

        Assert.True(Coloracao.BacktrackingHeuristica(_grafo, cores, new Estatisticas()), nome);
        Assert.True(Coloracao.ColoracaoValida(_grafo, cores));
        for (int v = 0; v < GrafoSudoku.TotalVertices; v++)
            if (dicas[v] != 0) Assert.Equal(dicas[v], cores[v]);
    }

    [Theory]
    [MemberData(nameof(Exemplos))]
    public void SimplesEHeuristicaChegamNaMesmaSolucao(string nome, string valores)
    {
        var simples = Tabuleiro.Ler(valores)!;
        var heuristica = (int[])simples.Clone();

        Assert.True(Coloracao.BacktrackingSimples(_grafo, simples, new Estatisticas()), nome);
        Assert.True(Coloracao.BacktrackingHeuristica(_grafo, heuristica, new Estatisticas()), nome);
        Assert.Equal(simples, heuristica);
    }

    [Fact]
    public void HeuristicaFazMenosTentativasQueSimples()
    {
        var simples = Tabuleiro.Ler(Tabuleiro.Exemplos[0].Valores)!;
        var heuristica = (int[])simples.Clone();
        var e1 = new Estatisticas();
        var e2 = new Estatisticas();

        Coloracao.BacktrackingSimples(_grafo, simples, e1);
        Coloracao.BacktrackingHeuristica(_grafo, heuristica, e2);

        Assert.True(e2.Tentativas < e1.Tentativas);
    }

    [Fact]
    public void TabuleiroVazioPodeSerColoridoCom9Cores()
    {
        var cores = new int[GrafoSudoku.TotalVertices];
        Assert.True(Coloracao.BacktrackingHeuristica(_grafo, cores, new Estatisticas()));
        Assert.True(Coloracao.ColoracaoValida(_grafo, cores));
    }

    [Fact]
    public void DetectaSudokuSemSolucao()
    {
        var cores = Tabuleiro.Ler("123456780" + "000000009" + new string('0', 63))!;
        Assert.True(Tabuleiro.DicasConsistentes(_grafo, cores));
        Assert.False(Coloracao.BacktrackingHeuristica(_grafo, cores, new Estatisticas()));
        Assert.False(Coloracao.BacktrackingSimples(_grafo, (int[])cores.Clone(), new Estatisticas()));
    }

    [Fact]
    public void CoresDisponiveisExcluiCoresDosVizinhos()
    {
        var cores = Tabuleiro.Ler("123456780" + new string('0', 72))!;
        Assert.Equal(new[] { 9 }, Coloracao.CoresDisponiveis(_grafo, cores, 8));
        Assert.False(Coloracao.CorPermitida(_grafo, cores, 8, 1));
        Assert.True(Coloracao.CorPermitida(_grafo, cores, 8, 9));
    }

    [Fact]
    public void GulosaLinhaALinhaUsaMaisDe9Cores()
    {
        var cores = new int[GrafoSudoku.TotalVertices];
        int usadas = Coloracao.ColoracaoGulosa(_grafo, cores, Enumerable.Range(0, GrafoSudoku.TotalVertices));
        Assert.True(usadas > Coloracao.NumeroCores);
    }

    [Fact]
    public void GulosaNuncaColoreVizinhosComAMesmaCor()
    {
        var cores = new int[GrafoSudoku.TotalVertices];
        Coloracao.ColoracaoGulosa(_grafo, cores, Enumerable.Range(0, GrafoSudoku.TotalVertices));
        for (int v = 0; v < GrafoSudoku.TotalVertices; v++)
            foreach (int vizinho in _grafo.Vizinhos(v))
                Assert.NotEqual(cores[v], cores[vizinho]);
    }

    [Fact]
    public void ColoracaoIncompletaNaoEValida()
    {
        var cores = Tabuleiro.Ler(Tabuleiro.Exemplos[0].Valores)!;
        Assert.False(Coloracao.ColoracaoValida(_grafo, cores));
    }
}
