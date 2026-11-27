using System.Text;

namespace SudokuGrafos;

public static class Tabuleiro
{
    public static readonly (string Nome, string Valores)[] Exemplos =
    {
        ("Fácil",    "530070000600195000098000060800060003400803001700020006060000280000419005000080079"),
        ("Difícil",  "800000000003600000070090200050007000000045700000100030001000068008500010090000400"),
        ("17 dicas", "000000010400000000020000000000050407008000300001090000300400200050100000000806000"),
    };

    public static int[]? Ler(string texto)
    {
        var digitos = texto.Where(c => char.IsDigit(c) || c == '.').ToArray();
        if (digitos.Length != GrafoSudoku.TotalVertices) return null;
        return digitos.Select(c => c == '.' ? 0 : c - '0').ToArray();
    }

    public static bool DicasConsistentes(GrafoSudoku g, int[] cores)
    {
        for (int v = 0; v < GrafoSudoku.TotalVertices; v++)
            if (cores[v] != 0)
                foreach (int vizinho in g.Vizinhos(v))
                    if (cores[vizinho] == cores[v]) return false;
        return true;
    }

    public static int ContarDicas(int[] cores) => cores.Count(c => c != 0);

    public static string Desenhar(int[] cores)
    {
        var sb = new StringBuilder();
        for (int linha = 0; linha < GrafoSudoku.Tamanho; linha++)
        {
            if (linha % 3 == 0) sb.AppendLine("  +-------+-------+-------+");
            sb.Append("  ");
            for (int coluna = 0; coluna < GrafoSudoku.Tamanho; coluna++)
            {
                if (coluna % 3 == 0) sb.Append("| ");
                int c = cores[linha * GrafoSudoku.Tamanho + coluna];
                sb.Append(c == 0 ? ". " : $"{c} ");
            }
            sb.AppendLine("|");
        }
        sb.AppendLine("  +-------+-------+-------+");
        return sb.ToString();
    }
}
