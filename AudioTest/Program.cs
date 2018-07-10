using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTest
{
    class Program
    {
        static private WaveOut waveOut;

        static void Main(string[] args)
        {
            StartStopSineWave();
            Console.ReadLine();
            StartStopSineWave();
        }


        static private void StartStopSineWave()
        {
            if (waveOut == null)
            {
                var sineWaveProvider = new SineWaveProvider32();
                sineWaveProvider.SetWaveFormat(16000, 1); // 16kHz mono
                //sineWaveProvider.SetWaveFormat(10000, 1); // 16kHz mono
                sineWaveProvider.Frequency = (float)440;
                sineWaveProvider.Amplitude = 0.25f;
                waveOut = new WaveOut();
                waveOut.Init(sineWaveProvider);
                waveOut.Play();
            }
            else
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
        }
    }
}
