using System.Globalization;
using System.Text;
using SudokuGrafos;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

var menu = new Menu(new GrafoSudoku());

if (args.Contains("--demo")) menu.Demo();
else if (args.Contains("--comparar")) menu.Comparar();
else menu.Executar();
