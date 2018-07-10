using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTest.WaveFunctions
{
    public interface IWaveFunction
    {
        float GetValue(int sampleCount, int sampleRate, float frequency);
    }
}
