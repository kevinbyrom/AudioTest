using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTest.WaveFunctions.Composition
{
    public class SequenceWave : IWaveFunction
    {
        const int MAX_WAVE_COUNT = 100;

        public IWaveFunction[] WaveFunctions;
        private int currWave = 0;
        private int waveCount = 0;

        public float GetValue(int sample, int sampleRate, float frequency)
        {
            float val = 0.0f;

            val = this.WaveFunctions[currWave].GetValue(sample, sampleRate, frequency);

            this.waveCount++;

            if (this.waveCount >= MAX_WAVE_COUNT)
            {
                this.waveCount = 0;
                this.currWave++;

                if (this.currWave >= this.WaveFunctions.Length)
                    this.currWave = 0;
            }


            return val;
        }

        public static SequenceWave Create(params IWaveFunction[] funcs)
        {
            var combo = new SequenceWave() { WaveFunctions = funcs };

            return combo;
        }
    }
}
