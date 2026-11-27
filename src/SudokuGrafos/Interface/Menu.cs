using System.Diagnostics;

namespace SudokuGrafos;

public class Menu
{
    private readonly GrafoSudoku _grafo;

    public Menu(GrafoSudoku grafo) => _grafo = grafo;

    public void Executar()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("========== SUDOKU COM COLORAÇÃO DE GRAFOS ==========");
            Console.WriteLine(" 1 - Informações do grafo");
            Console.WriteLine(" 2 - Ver os vizinhos (restrições) de uma célula");
            Console.WriteLine(" 3 - Resolver um Sudoku de exemplo");
            Console.WriteLine(" 4 - Digitar e resolver um Sudoku");
            Console.WriteLine(" 5 - Comparar algoritmos");
            Console.WriteLine(" 0 - Sair");
            Console.Write("Opção: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": InfoGrafo(); break;
                case "2": OpcaoVizinhos(); break;
                case "3": OpcaoExemplo(); break;
                case "4": OpcaoDigitar(); break;
                case "5": Comparar(); break;
                case "0": case null: Console.WriteLine("Até logo!"); return;
                default: Console.WriteLine("Opção inválida."); break;
            }
        }
    }

    public void Demo()
    {
        InfoGrafo();
        MostrarVizinhos(0);
        var (nome, valores) = Tabuleiro.Exemplos[0];
        Resolver(nome, valores);
    }

    public void Comparar()
    {
        Console.WriteLine("\nBacktracking simples x backtracking com heurística (MRV):");
        Console.WriteLine($"  {"Sudoku",-9} | {"Dicas",5} | {"Simples (tent.)",15} | {"Simples (ms)",12} | {"Heurística (tent.)",18} | {"Heurística (ms)",15}");
        Console.WriteLine("  " + new string('-', 89));

        foreach (var (nome, valores) in Tabuleiro.Exemplos)
        {
            var simples = Tabuleiro.Ler(valores)!;
            var heuristica = (int[])simples.Clone();
            int dicas = Tabuleiro.ContarDicas(simples);
            var e1 = new Estatisticas();
            var e2 = new Estatisticas();

            var sw = Stopwatch.StartNew();
            Coloracao.BacktrackingSimples(_grafo, simples, e1);
            double ms1 = sw.Elapsed.TotalMilliseconds;
            sw.Restart();
            Coloracao.BacktrackingHeuristica(_grafo, heuristica, e2);
            double ms2 = sw.Elapsed.TotalMilliseconds;

            string aviso = simples.SequenceEqual(heuristica) ? "" : "  (soluções diferentes!)";
            Console.WriteLine($"  {nome,-9} | {dicas,5} | {e1.Tentativas,15:N0} | {ms1,12:N1} | {e2.Tentativas,18:N0} | {ms2,15:N1}{aviso}");
        }

        Console.WriteLine("\nColoração gulosa (sem voltar atrás) em um tabuleiro VAZIO:");

        int c1 = Coloracao.ColoracaoGulosa(_grafo, new int[GrafoSudoku.TotalVertices], Enumerable.Range(0, GrafoSudoku.TotalVertices));
        Console.WriteLine($"  Ordem linha a linha: usou {c1} cores -> {ResultadoGuloso(c1)}");

        var ordemBlocos = Enumerable.Range(0, GrafoSudoku.TotalVertices).OrderBy(v => (v / 27, v % 9 / 3, v / 9 % 3, v % 3));
        int c2 = Coloracao.ColoracaoGulosa(_grafo, new int[GrafoSudoku.TotalVertices], ordemBlocos);
        Console.WriteLine($"  Ordem bloco a bloco: usou {c2} cores -> {ResultadoGuloso(c2)}");
    }

    private static string ResultadoGuloso(int cores) =>
        cores <= Coloracao.NumeroCores ? "Sudoku válido" : "NÃO é um Sudoku válido";

    private void InfoGrafo()
    {
        Console.WriteLine($"\nVértices (células): {GrafoSudoku.TotalVertices}");
        Console.WriteLine($"Arestas (restrições): {_grafo.TotalArestas}");
        Console.WriteLine($"Grau de cada vértice: {_grafo.Grau(0)} vizinhos (8 na linha + 8 na coluna + 4 no bloco)");
        Console.WriteLine($"Cores disponíveis: {Coloracao.NumeroCores} (números de 1 a 9)");
    }

    private void OpcaoVizinhos()
    {
        Console.Write("Linha (1-9): ");
        bool okL = int.TryParse(Console.ReadLine(), out int l);
        Console.Write("Coluna (1-9): ");
        bool okC = int.TryParse(Console.ReadLine(), out int c);
        if (!okL || !okC || l < 1 || l > 9 || c < 1 || c > 9)
        {
            Console.WriteLine("Posição inválida.");
            return;
        }
        MostrarVizinhos((l - 1) * GrafoSudoku.Tamanho + (c - 1));
    }

    private void MostrarVizinhos(int v)
    {
        var vizinhos = _grafo.Vizinhos(v).Select(GrafoSudoku.NomeCelula);
        Console.WriteLine($"\nVizinhos de {GrafoSudoku.NomeCelula(v)} ({_grafo.Grau(v)}): {string.Join(", ", vizinhos)}");
    }

    private void OpcaoExemplo()
    {
        for (int i = 0; i < Tabuleiro.Exemplos.Length; i++)
            Console.WriteLine($" {i + 1} - {Tabuleiro.Exemplos[i].Nome}");
        Console.Write("Escolha: ");

        if (int.TryParse(Console.ReadLine(), out int n) && n >= 1 && n <= Tabuleiro.Exemplos.Length)
        {
            var (nome, valores) = Tabuleiro.Exemplos[n - 1];
            Resolver(nome, valores);
        }
        else
        {
            Console.WriteLine("Opção inválida.");
        }
    }

    private void OpcaoDigitar()
    {
        Console.WriteLine("Digite os 81 números (linha por linha, 0 ou . para vazio):");
        Resolver("Digitado", Console.ReadLine() ?? "");
    }

    private void Resolver(string nome, string texto)
    {
        var cores = Tabuleiro.Ler(texto);
        if (cores is null) { Console.WriteLine("O Sudoku precisa ter exatamente 81 números."); return; }
        if (!Tabuleiro.DicasConsistentes(_grafo, cores)) { Console.WriteLine("As dicas já têm números repetidos."); return; }

        Console.WriteLine($"\nSudoku \"{nome}\" ({Tabuleiro.ContarDicas(cores)} dicas):");
        Console.Write(Tabuleiro.Desenhar(cores));

        var est = new Estatisticas();
        var sw = Stopwatch.StartNew();
        bool ok = Coloracao.BacktrackingHeuristica(_grafo, cores, est);
        sw.Stop();

        if (!ok) { Console.WriteLine("Este Sudoku não tem solução."); return; }

        Console.WriteLine("Solução (coloração com 9 cores):");
        Console.Write(Tabuleiro.Desenhar(cores));
        Console.WriteLine($"Coloração válida: {(Coloracao.ColoracaoValida(_grafo, cores) ? "sim" : "não")}");
        Console.WriteLine($"Tentativas: {est.Tentativas:N0}   Retrocessos: {est.Retrocessos:N0}   Tempo: {sw.Elapsed.TotalMilliseconds:N1} ms");
    }
}
