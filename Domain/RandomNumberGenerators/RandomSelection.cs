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
