using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using System.Diagnostics;
using AudioTest.WaveFunctions;
using AudioTest.WaveFunctions.Basic;
using AudioTest.WaveFunctions.Effects;
using AudioTest.WaveFunctions.Composition;


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

            this.WaveFunction = new SquareWave();
            //this.WaveFunction = new SineWave();
            //this.WaveFunction = new TanWave();
            //this.WaveFunction = new TriangleWave();
            //this.WaveFunction = new DarkWave();
            //this.WaveFunction = new NoiseWave();
            //this.WaveFunction = new SawWave();
            this.WaveFunction = ComboWave.Create(new SineWave(), new SquareWave(), new NoiseWave(), new TanWave(), new CosineWave());
            //this.WaveFunction = ComboWave.Create(new SineWave(), new SquareWave(), new TanWave(), new CosineWave());

            //this.WaveFunction = SequenceWave.Create(new SineWave(), new SquareWave(), new TanWave(), new CosWave());

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
}
