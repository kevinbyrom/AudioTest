using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTest.WaveFunctions.Basic
{
    public class SawtoothWave : IWaveFunction
    {
        public float GetValue(int sample, int sampleRate, float frequency)
        {
            return sample % sampleRate; //(float)(sample * frequency) / sampleRate;
        }
    }
}
