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
    public abstract class WaveProvider32 : IWaveProvider
    {
        private WaveFormat waveFormat;

        public WaveProvider32()
            : this(44100, 1)
        {
        }

        public WaveProvider32(int sampleRate, int channels)
        {
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

        public abstract int Read(float[] buffer, int offset, int sampleCount);

        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }
    }


    public class SineWaveProvider32 : WaveProvider32
    {
        int sample;

        public SineWaveProvider32()
        {
            Frequency = 1000;
            Amplitude = 0.25f; // let's not hurt our ears   
            this.WaveFunction = new SquareWaveFunction();
        }

        public float Frequency { get; set; }
        public float Amplitude { get; set; }
        public IWaveFunction WaveFunction { get; set; }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            Debug.WriteLine($"{offset} - {sampleCount}");
            Random rnd = new Random();

            int sampleRate = WaveFormat.SampleRate;
            for (int n = 0; n < sampleCount; n++)
            {
                var rndVal = rnd.Next(3);

                //var val = (float)(Amplitude * Math.Sin((2 * Math.PI * sample * Frequency) / sampleRate));

                //var val = Math.Sin(Frequency * sample) >= 0 ? Amplitude : -1 * Amplitude;
                //var val = (float)sample / sampleRate < 0.5f ? -Amplitude * Frequency : Amplitude * Frequency;//  (float)(Amplitude * Math.Sin((Math.PI * sample * Frequency) / sampleRate));
                //Debug.WriteLine($"{val}");

                var val = this.Amplitude * this.WaveFunction.GetValue(sample, sampleRate, this.Frequency);

                buffer[n + offset] = val; //(float)(Amplitude * Math.Sin((2 * (Math.PI / 4) * sample * Frequency) / sampleRate));
                
                sample++;
                if (sample >= sampleRate) sample = 0;
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


    public class SquareWaveFunction : IWaveFunction
    {
        public float GetValue(int sample, int sampleRate, float frequency)
        {
            return Math.Sin(frequency * sample) >= 0 ? 1 : -1;
        }
    }
}
