using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace nQueens
{
    public partial class MainWindow : Window
    {
        private string[] soluciones; // Array para almacenar las soluciones
        private int currentSolutionIndex = 0; // Índice de la solución actual

        public MainWindow()
        {
            InitializeComponent();
            var output = ExecutePrologSolution(); // Obtén la salida de Prolog
            RenderSolutions(output); // Renderiza las soluciones
        }

        private string ExecutePrologSolution()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "swipl",
                    Arguments = $"-s {Directory.GetCurrentDirectory()}/8queens.pl -g main -t halt",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        private void RenderSolutions(string output)
        {
            // Limpia el tablero
            soluciones = output.Split("Tablero:", StringSplitOptions.TrimEntries)
                               .Skip(1)
                               .Select(sol => sol.Trim())
                               .ToArray(); // Elimina el primer elemento y almacena las soluciones

            // Muestra la cantidad de soluciones encontradas
            int totalSoluciones = soluciones.Length;

            // Habilita o deshabilita los botones según el número de soluciones
            NextButton.IsEnabled = totalSoluciones > 1;
            PrevButton.IsEnabled = totalSoluciones > 1;

            // Muestra la primera solución al cargar
            currentSolutionIndex = 0;
            ShowSolution(currentSolutionIndex);
        }

        private void ShowSolution(int index)
        {
            if (index >= 0 && index < soluciones.Length)
            {
                SolutionsTextBlock.Text = soluciones[index];

                // Actualiza el estado de los botones
                PrevButton.IsEnabled = index > 0;
                NextButton.IsEnabled = index < soluciones.Length - 1;
            }
        }

        private void OnPrevButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentSolutionIndex > 0)
            {
                currentSolutionIndex--;
                ShowSolution(currentSolutionIndex);
            }
        }

        private void OnNextButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentSolutionIndex < soluciones.Length - 1)
            {
                currentSolutionIndex++;
                ShowSolution(currentSolutionIndex);
            }
        }
    }
}
