# Sudoku e a Teoria dos Grafos (C#)

O Sudoku é um quebra-cabeça jogado em um tabuleiro de 9x9 células, dividido em 9 blocos de 3x3. Algumas células já começam preenchidas (as dicas) e o objetivo é completar o tabuleiro com números de 1 a 9 de forma que nenhum número se repita na mesma linha, na mesma coluna ou no mesmo bloco.

Este programa resolve o Sudoku tratando o jogo como um problema de **coloração de grafos**:

| Sudoku | Grafo |
|---|---|
| Cada uma das 81 células | Vértice |
| Duas células na mesma linha, coluna ou bloco 3x3 | Aresta |
| Números de 1 a 9 | Cores |
| Resolver o Sudoku | Colorir o grafo com 9 cores sem que vértices vizinhos tenham a mesma cor |

O grafo tem 81 vértices e 810 arestas, e cada vértice tem exatamente 20 vizinhos.

O jogador pode ver as informações do grafo, consultar os vizinhos de qualquer célula, resolver os Sudokus de exemplo (fácil, difícil e com 17 dicas), digitar o próprio Sudoku e comparar os algoritmos de resolução: backtracking simples, backtracking com heurística MRV e coloração gulosa.

## Como executar

Requisito: [.NET SDK 8.0](https://dotnet.microsoft.com/download) ou superior.

```bash
dotnet run --project src/SudokuGrafos
dotnet run --project src/SudokuGrafos -- --demo
dotnet run --project src/SudokuGrafos -- --comparar
dotnet test
```
