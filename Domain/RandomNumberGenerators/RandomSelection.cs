using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.RandomNumberGenerators
{
    public class RandomSelection
    {
        Func<double> Randomizer = RandomGeneratorsFactory.GetUniform(0, 1);

        public int GetRandomIndex(IEnumerable<int> weights)
        {
            int totalWeight = weights.Sum();

            var probabilities = weights.Select(element => (double)element / totalWeight).ToList();

            return CalculateIndex(probabilities);
        }

        public int GetRandomIndex(IEnumerable<double> possibleProbability)
        {
            double totalPercent = possibleProbability.Sum();

            var probabilities = possibleProbability.Select(element => element / totalPercent).ToList();

            return CalculateIndex(probabilities);
        }

        public List<(T element, double probability)> WeightToPercent<T>(IList<(T element, int weight)> possibleElements)
        {
            int totalWeight = possibleElements.Sum(el => el.weight);

            return possibleElements.Select(element => (element.element, (double)element.weight / totalWeight)).ToList();
        }

        public List<(T element, double probability)> PercentToPercent<T>(IList<(T element, double percent)> possibleElements)
        {
            double totalPercent = possibleElements.Sum(el => el.percent);

            return possibleElements.Select(element => (element.element, element.percent / totalPercent)).ToList();
        }

        // sum of probabilities should be - 1.00
        // cumulative distribution
        private int CalculateIndex(IList<double> probabalities)
        {
            double diceRoll = Randomizer();

            double cumulative = 0.0;
            for (int i = 0; i < probabalities.Count; i++)
            {
                cumulative += probabalities[i];
                if (diceRoll < cumulative)
                {
                    return i;
                }
            }

            return probabalities.Count;
        }
    }
}
