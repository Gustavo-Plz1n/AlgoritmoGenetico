using System;
using System.Collections.Generic;
using System.Linq;

namespace Atividade.Models
{
    public class AlgoritmoGenético
    {
        public int Weight { get; set; }
        public int Value { get; set; }

        public enum SelectionMethod
        {
            Tournament,
            Roulette
        }

        // Método principal que implementa o algoritmo genético para resolver o problema da mochila
        public static int[] GeneticAlgorithmKnapsack(List<Item> items, int capacity, int populationSize, double crossoverRate, double mutationRate, int numGenerations, SelectionMethod selectionMethod)
        {
            // Gera uma população inicial de soluções aleatórias
            var population = GenerateInitialPopulation(items.Count, populationSize);

            // Loop para cada geração do algoritmo genético
            for (int gen = 0; gen < numGenerations; gen++)
            {
                // Lista para armazenar a próxima geração de soluções
                var nextGeneration = new List<int[]>();

                // Loop para criar a próxima geração de soluções
                for (int i = 0; i < populationSize; i++)
                {
                    // Seleciona dois pais para cruzamento
                    var parent1 = selectionMethod switch
                    {
                        SelectionMethod.Tournament => TournamentSelection(population, items, capacity),
                        SelectionMethod.Roulette => RouletteSelection(population, items, capacity),
                        _ => throw new ArgumentException("Método de seleção inválido.")
                    };

                    var parent2 = selectionMethod switch
                    {
                        SelectionMethod.Tournament => TournamentSelection(population, items, capacity),
                        SelectionMethod.Roulette => RouletteSelection(population, items, capacity),
                        _ => throw new ArgumentException("Método de seleção inválido.")
                    };

                    // Produz um descendente a partir dos pais selecionados
                    var offspring = Crossover(parent1, parent2, crossoverRate);

                    // Aplica mutação ao descendente gerado
                    Mutate(offspring, mutationRate);

                    // Adiciona o descendente à próxima geração
                    nextGeneration.Add(offspring);
                }

                // Atualiza a população com a próxima geração
                population = nextGeneration;
            }

            // Seleciona a melhor solução da população final
            var bestSolution = population.OrderByDescending(individual => FitnessFunction(individual, items, capacity)).First();

            // Retorna a melhor solução encontrada
            return bestSolution;
        }

        // Função para gerar uma população inicial de soluções aleatórias
        public static List<int[]> GenerateInitialPopulation(int size, int populationSize)
        {
            var population = new List<int[]>();
            var rand = new Random();
            for (int i = 0; i < populationSize; i++)
            {
                var individual = new int[size];
                for (int j = 0; j < size; j++)
                {
                    individual[j] = rand.Next(2); // 0 ou 1 (selecionado ou não selecionado)
                }
                population.Add(individual);
            }
            return population;
        }

        // Função para calcular a aptidão (fitness) de uma solução
        public static int FitnessFunction(int[] solution, List<Item> items, int capacity)
        {
            int totalValue = 0;
            int totalWeight = 0;
            for (int i = 0; i < solution.Length; i++)
            {
                if (solution[i] == 1)
                {
                    totalValue += items[i].Value;
                    totalWeight += items[i].Weight;
                }
            }
            // Penalize soluções que excedam a capacidade da mochila
            if (totalWeight > capacity)
            {
                totalValue = 0;
            }
            return totalValue;
        }

        // Função para realizar a seleção por torneio
        public static int[] TournamentSelection(List<int[]> population, List<Item> items, int capacity)
        {
            var tournamentSize = 5; // Tamanho do torneio
            var rand = new Random();
            var tournament = population.OrderBy(x => rand.Next()).Take(tournamentSize).ToList();
            var bestSolution = tournament.OrderByDescending(individual => FitnessFunction(individual, items, capacity)).First();
            return bestSolution;
        }

        // Função para realizar a seleção por roleta
        public static int[] RouletteSelection(List<int[]> population, List<Item> items, int capacity)
        {
            var totalFitness = population.Sum(individual => FitnessFunction(individual, items, capacity));
            var randFitness = new Random().NextDouble() * totalFitness;
            var cumulativeFitness = 0;
            foreach (var individual in population)
            {
                cumulativeFitness += FitnessFunction(individual, items, capacity);
                if (cumulativeFitness >= randFitness)
                {
                    return individual;
                }
            }
            return population.Last();
        }

        // Função para realizar o crossover
        public static int[] Crossover(int[] parent1, int[] parent2, double crossoverRate)
        {
            Random rand = new Random();
            int[] offspring = new int[parent1.Length];
            int crossoverPoint = rand.Next(parent1.Length);

            if (rand.NextDouble() < crossoverRate)
            {
                for (int i = 0; i < parent1.Length; i++)
                {
                    if (i < crossoverPoint)
                    {
                        offspring[i] = parent1[i];
                    }
                    else
                    {
                        offspring[i] = parent2[i];
                    }
                }
            }
            else
            {
                offspring = (rand.NextDouble() < 0.5) ? (int[])parent1.Clone() : (int[])parent2.Clone();
            }
            return offspring;
        }

        // Função para realizar a mutação
        public static void Mutate(int[] solution, double mutationRate)
        {
            Random rand = new Random();
            for (int i = 0; i < solution.Length; i++)
            {
                if (rand.NextDouble() < mutationRate)
                {
                    solution[i] = 1 - solution[i]; // Inverte o gene
                }
            }
        }
    }
}
