using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTest.WaveFunctions.Effects
{
    public class DarkWave : IWaveFunction
    {
        public float GetValue(int sample, int sampleRate, float frequency)
        {
            // Lower sample to nearest value

            int x = (int)(sample / ((sampleRate / frequency) / 4));
            return (float)Math.Sin((2 * Math.PI * x * frequency) / sampleRate);
        }
    }
}
