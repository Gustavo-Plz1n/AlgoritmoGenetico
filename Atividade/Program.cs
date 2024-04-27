using Atividade.Models;

namespace Atividade
{

    public class Program
    {
        public static void Main(string[] args)
        {

            AlgoritmoGenético algoritmoGenético = new AlgoritmoGenético();
            Leitor leitor = new Leitor();

            string fileName = @"C:\Users\plush\Desktop\instancias-mochila\instancias-mochila\KNAPDATA40.txt";
            List<Item> items;
            int capacity;

                try
                {
                    items = leitor.ReadItems(fileName);
                    capacity = leitor.ReadCapacity(fileName);
                }
            catch (IOException e)
            {
                Console.WriteLine("Erro na leitura do arquivo: " + e.Message);
                return;
            }

            // Parâmetros do algoritmo genético
            int populationSize = 50;
            double crossoverRate = 0.8;
            double mutationRate = 0.1;
            int numGenerations = 500;

            AlgoritmoGenético.SelectionMethod selectionMethod = AlgoritmoGenético.SelectionMethod.Roulette;


            // Chamada para a função que implementa o algoritmo genético
            int[] solution = AlgoritmoGenético.GeneticAlgorithmKnapsack(items, capacity, populationSize, crossoverRate, mutationRate, numGenerations, selectionMethod);

            // Imprimir a solução encontrada
            Console.WriteLine("Itens selecionados:");
            for (int i = 0; i < items.Count; i++)
            {
                if (solution[i] == 1)
                {
                    Console.WriteLine($"Item {i + 1}: Peso = {items[i].Weight}, Valor = {items[i].Value}");
                }
            }

        }
    }
} 