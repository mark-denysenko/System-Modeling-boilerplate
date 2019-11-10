using System;

namespace Domain.RandomNumberGenerators
{

    public class RandomGeneratorsFactory
    {
        private static int createdGenerators = 0;
        private static int NewSeed
        {
            get
            {
                return 
                    DateTime.UtcNow.Year 
                    + DateTime.UtcNow.Month
                    + DateTime.UtcNow.Day 
                    + DateTime.UtcNow.Hour
                    + DateTime.UtcNow.Minute 
                    + DateTime.UtcNow.Millisecond 
                    + createdGenerators++;
            }
        }

        public static Func<double> GetExponential(double timeMean)
        {
            var random = new Random(NewSeed);
            return () => -timeMean * Math.Log(random.NextDouble());
        }

        public static Func<double> GetUniform(double timeMin, double timeMax)
        {
            var random = new Random(NewSeed);
            return () => timeMin + random.NextDouble() * (timeMax - timeMin);
        }

        public static Func<double> GetNormal(double timeMean, double timeDeviation)
        {
            var random = new Random(NewSeed);
            return () =>  timeMean + timeDeviation * NextGaussian(random);
        }

        private static double NextGaussian(Random random)
        {
            double u1 = 1.0 - random.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)

            return randStdNormal;
        }

        /// <param name="k">shape</param>
        /// <param name="alpha">rate</param>
        public static Func<double> Erlang(int k, double alpha)
        {
            var random = new Random(NewSeed);

            return () =>
            {
                double g = 0.0;

                for (int i = 0; i < k; i++)
                {
                    // uniform (0; 1]
                    g += Math.Log(random.NextDouble());
                }   // construct the gamma distribution

                return (-1 / alpha) * g;
            };
        }
    }
}
