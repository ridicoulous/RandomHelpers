using System;
using System.Collections.Generic;
using System.Threading;

namespace RandomHelpers.Extensions
{
    public static class RandomExtensionMethods
    {
        /// <summary>
        /// Gets a number from exponential distribution.  <see href="https://en.wikipedia.org/wiki/Exponential_distribution">WIKI</see>
        /// </summary>
        /// <param name="r">Instance of Random class</param>
        /// <param name="minValue">Starting value of range of values. It would have maximum count of entries, start of ditribution</param>
        /// <param name="maxValue">Ending value of range, limit of ditribution</param>
        /// <param name="mu">is the mean or expectation of the distribution (and also its median and mode)</param>
        /// <param name="sigma">is the standard deviation, and</param>
        /// <param name="lambda">Here λ > 0 is the parameter of the distribution, often called the rate parameter. The distribution is supported on the interval [0, ∞). If a random variable X has this distribution, we write X ~ Exp(λ).</param>
        /// <returns>One number from exponential distribution. Use it many times to get ditribution</returns>
        public static double GetNextExponentialDistrubutedNumber(this Random r, double minValue, double maxValue, double mu = 0, double sigma = 1, double lambda = 1)
        {
            double result = maxValue;
            do
            {
                double u = r.NextDouble();
                double t = -Math.Log(u) / lambda;
                double increment = (maxValue - minValue) / 6.0;
                result = minValue + (t * increment);
            } while (result >= maxValue);

            return result;
        }
        /// <summary>
        /// Gets a number from normal distribution <see href="https://en.wikipedia.org/wiki/Normal_distribution">WIKI</see>
        /// </summary>
        /// <param name="r">Instance of Random class</param>
        /// <param name="mu">is the mean or expectation of the distribution (and also its median and mode). Center of chart will be here</param>
        /// <param name="sigma">is the standard deviation</param>
        /// <returns>One number from exponential distribution. Use it many times to get ditribution</returns>
        public static double GetNextNormalDitributedNumber(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();
            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            var rand_normal = mu + sigma * rand_std_normal;
            return rand_normal;
        }
        /// <summary>
        /// Gets a list of normal distributed numbers
        /// </summary>
        /// <param name="r">Instance of Random class</param>
        /// <param name="count">Count of values in result</param>
        /// <param name="mu">is the mean or expectation of the distribution (and also its median and mode). Center of chart will be here</param>
        /// <param name="sigma">is the standard deviation</param>
        /// <param name="decimals">count of decimals in results</param>
        /// <returns>list of normal ditributed values</returns>
        public static List<double> GetNormalDistributedList(this Random r, int count, double mu = 0, double sigma = 1, int? decimals = null)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < count; i++)
            {
                if (decimals.HasValue)
                {
                    result.Add(Math.Round(r.GetNextNormalDitributedNumber(mu, sigma), decimals.Value));
                }
                else
                    result.Add(r.GetNextNormalDitributedNumber(mu, sigma));

            }
            return result;
        }
        /// <summary>
        /// Gets a list of exponential distributed numbers
        /// </summary>
        /// <param name="r">Random</param>
        /// <param name="count">Count of results</param>
        /// <param name="minValue">Starting value of range of values. It would have maximum count of entries, start of ditribution</param>
        /// <param name="maxValue">Ending value of range, limit of ditribution</param>
        /// <param name="mu">is the mean or expectation of the distribution (and also its median and mode)</param>
        /// <param name="sigma">is the standard deviation, and</param>
        /// <param name="lambda">Here λ > 0 is the parameter of the distribution, often called the rate parameter. The distribution is supported on the interval [0, ∞). If a random variable X has this distribution, we write X ~ Exp(λ).</param>
        /// <returns>List of numbers from exponential distribution</returns>
        public static List<double> GetExponentialDistributedList(this Random r, int count, double minValue, double maxValue, double mu = 0, double sigma = 1, double lambda = 1, int? decimals = null)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < count; i++)
            {
                if (decimals.HasValue)
                {
                    result.Add(Math.Round(r.GetNextExponentialDistrubutedNumber(minValue, maxValue, mu, sigma, lambda), decimals.Value));
                }
                else
                    result.Add(r.GetNextExponentialDistrubutedNumber(minValue, maxValue, mu, sigma, lambda));
            }
            return result;
        }
        internal static class ThreadSafeRandom
        {
            [ThreadStatic] private static Random Local;

            public static Random ThisThreadsRandom
            {
                get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
            }
        }
        /// <summary>
        /// List randomizer from solution https://stackoverflow.com/questions/273313/randomize-a-listt. Used ThreadSafe random variant
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
