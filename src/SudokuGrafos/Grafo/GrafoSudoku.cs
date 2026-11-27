namespace SudokuGrafos;

public class GrafoSudoku
{
    public const int Tamanho = 9;
    public const int TotalVertices = Tamanho * Tamanho;

    private readonly List<int>[] _vizinhos = new List<int>[TotalVertices];

    public GrafoSudoku()
    {
        for (int v = 0; v < TotalVertices; v++)
            _vizinhos[v] = new List<int>();

        for (int a = 0; a < TotalVertices; a++)
            for (int b = a + 1; b < TotalVertices; b++)
                if (TemRestricao(a, b))
                {
                    _vizinhos[a].Add(b);
                    _vizinhos[b].Add(a);
                }
    }

    public static bool TemRestricao(int a, int b)
    {
        int linhaA = a / Tamanho, colunaA = a % Tamanho;
        int linhaB = b / Tamanho, colunaB = b % Tamanho;
        bool mesmoBloco = linhaA / 3 == linhaB / 3 && colunaA / 3 == colunaB / 3;
        return linhaA == linhaB || colunaA == colunaB || mesmoBloco;
    }

    public IReadOnlyList<int> Vizinhos(int v) => _vizinhos[v];

    public int Grau(int v) => _vizinhos[v].Count;

    public int TotalArestas => _vizinhos.Sum(l => l.Count) / 2;

    public static string NomeCelula(int v) => $"L{v / Tamanho + 1}C{v % Tamanho + 1}";
}
