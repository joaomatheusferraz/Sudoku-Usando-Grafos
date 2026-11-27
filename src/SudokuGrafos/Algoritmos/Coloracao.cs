namespace SudokuGrafos;

public static class Coloracao
{
    public const int NumeroCores = 9;

    public static bool CorPermitida(GrafoSudoku g, int[] cores, int v, int cor)
    {
        foreach (int vizinho in g.Vizinhos(v))
            if (cores[vizinho] == cor) return false;
        return true;
    }

    public static List<int> CoresDisponiveis(GrafoSudoku g, int[] cores, int v)
    {
        var usadas = new bool[NumeroCores + 1];
        foreach (int vizinho in g.Vizinhos(v))
            usadas[cores[vizinho]] = true;

        var livres = new List<int>();
        for (int cor = 1; cor <= NumeroCores; cor++)
            if (!usadas[cor]) livres.Add(cor);
        return livres;
    }

    public static bool BacktrackingSimples(GrafoSudoku g, int[] cores, Estatisticas est)
    {
        int v = Array.IndexOf(cores, 0);
        if (v == -1) return true;

        for (int cor = 1; cor <= NumeroCores; cor++)
        {
            if (!CorPermitida(g, cores, v, cor)) continue;

            cores[v] = cor;
            est.Tentativas++;
            if (BacktrackingSimples(g, cores, est)) return true;

            cores[v] = 0;
            est.Retrocessos++;
        }
        return false;
    }

    public static bool BacktrackingHeuristica(GrafoSudoku g, int[] cores, Estatisticas est)
    {
        int melhor = -1;
        List<int>? opcoesMelhor = null;

        for (int v = 0; v < GrafoSudoku.TotalVertices; v++)
        {
            if (cores[v] != 0) continue;
            var opcoes = CoresDisponiveis(g, cores, v);
            if (opcoesMelhor == null || opcoes.Count < opcoesMelhor.Count)
            {
                melhor = v;
                opcoesMelhor = opcoes;
                if (opcoes.Count <= 1) break;
            }
        }

        if (melhor == -1) return true;

        foreach (int cor in opcoesMelhor!)
        {
            cores[melhor] = cor;
            est.Tentativas++;
            if (BacktrackingHeuristica(g, cores, est)) return true;

            cores[melhor] = 0;
            est.Retrocessos++;
        }
        return false;
    }

    public static int ColoracaoGulosa(GrafoSudoku g, int[] cores, IEnumerable<int> ordem)
    {
        foreach (int v in ordem)
        {
            if (cores[v] != 0) continue;
            int cor = 1;
            while (!CorPermitida(g, cores, v, cor)) cor++;
            cores[v] = cor;
        }
        return cores.Max();
    }

    public static bool ColoracaoValida(GrafoSudoku g, int[] cores)
    {
        for (int v = 0; v < GrafoSudoku.TotalVertices; v++)
        {
            if (cores[v] < 1 || cores[v] > NumeroCores) return false;
            foreach (int vizinho in g.Vizinhos(v))
                if (cores[vizinho] == cores[v]) return false;
        }
        return true;
    }
}
