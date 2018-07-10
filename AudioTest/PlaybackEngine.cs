using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using System.Diagnostics;


namespace AudioTest
{
    public class PlaybackEngine : IWaveProvider
    {
        
        private WaveFormat waveFormat;

        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }


        public float Frequency { get; set; }
        public float Amplitude { get; set; }
        public IWaveFunction WaveFunction { get; set; }

        int sample;
        int totalSamples;


        public PlaybackEngine() : this(44100, 1)
        {
        }


        public PlaybackEngine(int sampleRate, int channels)
        {
            Frequency = 1000;
            Amplitude = 0.25f; // let's not hurt our ears   
            //this.WaveFunction = new SquareWaveFunction();
            //this.WaveFunction = new SineWaveFunction();
            //this.WaveFunction = new TanWaveFunction();
            //this.WaveFunction = new TriangleWaveFunction();
            //this.WaveFunction = new NoiseWaveFunction();
            //this.WaveFunction = new SawWaveFunction();
            //this.WaveFunction = ComboWaveFunction.Create(new SineWaveFunction(), new SquareWaveFunction(), new NoiseWaveFunction(), new TanWaveFunction(), new CosWaveFunction());
            this.WaveFunction = ComboWaveFunction.Create(new SineWaveFunction(), new SquareWaveFunction(), new TanWaveFunction(), new CosWaveFunction());

            SetWaveFormat(sampleRate, channels);
        }


        public void SetWaveFormat(int sampleRate, int channels)
        {
            this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
        }


        public int Read(byte[] buffer, int offset, int count)
        {
            WaveBuffer waveBuffer = new WaveBuffer(buffer);
            int samplesRequired = count / 4;
            int samplesRead = Read(waveBuffer.FloatBuffer, offset / 4, samplesRequired);
            return samplesRead * 4;
        }


        public int Read(float[] buffer, int offset, int sampleCount)
        {
            Debug.WriteLine($"{offset} - {sampleCount}");
            Random rnd = new Random();

            int sampleRate = WaveFormat.SampleRate;
            for (int n = 0; n < sampleCount; n++)
            {
                var val = this.Amplitude * this.WaveFunction.GetValue(sample, sampleRate, this.Frequency);

                buffer[n + offset] = val; 

                sample++;
                totalSamples++;
                if (sample >= sampleRate) sample = 0; // One second of data has passed
            }
            return sampleCount;
        }
    }


    public interface IWaveFunction
    {
        float GetValue(int sampleCount, int sampleRate, float frequency);
    }


    public class SineWaveFunction : IWaveFunction
    {
        public float GetValue(int sample, int sampleRate, float frequency)
        {
            return (float)Math.Sin((2 * Math.PI * sample * frequency) / sampleRate);
        }
    }


    public class CosWaveFunction : IWaveFunction
    {
        public float GetValue(int sample, int sampleRate, float frequency)
        {
            return (float)Math.Cos((2 * Math.PI * sample * frequency) / sampleRate);
        }
    }

    public class SquareWaveFunction : IWaveFunction
    {
        public float GetValue(int sample, int sampleRate, float frequency)
        {
            return (float)Math.Sin((2 * Math.PI * sample * frequency) / sampleRate) >= 0 ? 1 : -1;
        }
    }


    public class TriangleWaveFunction : IWaveFunction
    {
        public float GetValue(int sample, int sampleRate, float frequency)
        {
            return Math.Abs((sample % frequency) - frequency);
            //(float)sample / sampleRate
        }
    }

    public class SawWaveFunction : IWaveFunction
    {
        public float GetValue(int sample, int sampleRate, float frequency)
        {
            return sample % sampleRate; //(float)(sample * frequency) / sampleRate;
        }
    }


    public class TanWaveFunction : IWaveFunction
    {
        public float GetValue(int sample, int sampleRate, float frequency)
        {
            return (float)Math.Tan((2 * Math.PI * sample * frequency) / sampleRate) >= 0 ? 1 : -1;
        }
    }


    public class NoiseWaveFunction : IWaveFunction
    {
        Random rnd = new Random();

        public float GetValue(int sample, int sampleRate, float frequency)
        {
            return ((float)rnd.NextDouble() - 0.5f) * 2.0f;
        }
    }

    public class ComboWaveFunction : IWaveFunction
    {
        public IWaveFunction[] WaveFunctions;
        private SineWaveFunction sineWave = new SineWaveFunction();
        private SquareWaveFunction squareWave = new SquareWaveFunction();

        public float GetValue(int sample, int sampleRate, float frequency)
        {
            float val = 0.0f;

            if (this.WaveFunctions.Length > 0)
            {
                foreach (var func in this.WaveFunctions)
                {
                    val += func.GetValue(sample, sampleRate, frequency);
                }

                val = val / this.WaveFunctions.Length;
            }
            
            return val;
        }

        public static ComboWaveFunction Create(params IWaveFunction[] funcs)
        {
            var combo = new ComboWaveFunction() { WaveFunctions = funcs };

            return combo;
        }
    }
}
