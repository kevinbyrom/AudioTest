using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AudioTest.WaveFunctions.Effects
{
    public class NoiseWave : IWaveFunction
    {
        Random rnd = new Random();

        public float GetValue(int sample, int sampleRate, float frequency)
        {
            return ((float)rnd.NextDouble() - 0.5f) * 2.0f;
        }
    }
}
