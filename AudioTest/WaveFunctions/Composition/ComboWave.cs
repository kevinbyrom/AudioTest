using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTest.WaveFunctions.Composition
{
    public class ComboWave : IWaveFunction
    {
        public IWaveFunction[] WaveFunctions;

        public float GetValue(int sample, int sampleRate, float frequency)
        {
            float val = 0.0f;

            if (this.WaveFunctions.Length > 0)
            {
                foreach (var func in this.WaveFunctions)
                {
                    val += func.GetValue(sample, sampleRate, frequency);
                }

                //val = val / this.WaveFunctions.Length;
            }

            return val;
        }

        public static ComboWave Create(params IWaveFunction[] funcs)
        {
            var combo = new ComboWave() { WaveFunctions = funcs };

            return combo;
        }
    }
}
