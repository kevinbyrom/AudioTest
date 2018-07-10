using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTest.WaveFunctions.Basic
{
    public class SquareWave : IWaveFunction
    {
        public float GetValue(int sample, int sampleRate, float frequency)
        {
            return (float)Math.Sin((2 * Math.PI * sample * frequency) / sampleRate) >= 0 ? 1 : -1;
        }
    }
}
