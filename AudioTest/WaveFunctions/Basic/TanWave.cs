using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTest.WaveFunctions.Basic
{
    public class TanWave : IWaveFunction
    {
        public float GetValue(int sample, int sampleRate, float frequency)
        {
            return (float)Math.Tan((2 * Math.PI * sample * frequency) / sampleRate) >= 0 ? 1 : -1;
        }
    }
}
