using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DSPLib;

public class VoiceRecognizer : MonoBehaviour
{
    public static DSPLib DSPlibrary;


    public static class VoiceCommand
    {


        // --------------SECTION: CONSTANTS -------------- 

        //Learned weights 
        /*

             private static double[][] WEIGHTS = 
             {  //1:Down 80%,  2:Go 4%,  3:Up 82% --> 55.7%
                new double[] {-3.24730129,  0.90973467,  0.45426938, -0.90857456,  0.91835969, -0.63348943,  0.911782,    0.54843435,  0.16583765},
                new double[] {-1.43423523, -0.18685531, -0.23223188,  0.19156274, -0.42568255, -1.06881343,  1.29043552,  0.12993019, -0.04290935},
                new double[] { 4.68153652, -0.72287937, -0.22203751,  0.71701182, -0.49267714,  1.70230286, -2.20221752, -0.67836454, -0.12292831}
             };

            private static double[][] WEIGHTS = 
             {  //1:Down 73%,  2:Go 62%,  3:Up 41%  --> 58.7%
                new double[] {-1.68289028,  0.4370663 ,  0.27004041, -0.55632377,  0.39684296, -0.47692714,  0.69260697,  0.31059444,  0.09663902},
                new double[] {-0.92691717, -0.14806735, -0.05490557,  0.18390689, -0.18431841, -0.74816391,  0.82489817,  0.0239964 ,  0.06126582},
                new double[] { 2.60980744, -0.28899895, -0.21513484,  0.37241687, -0.21252454,  1.22509105, -1.51750514, -0.33459084, -0.15790484}
             };


             private static double[][] WEIGHTS = 
             {  //1:Down 84%,  2:Go 23%,  3:Up 73%  --> 59.8%
                new double[] {-1.7017979 ,  0.3576827 ,  0.27744992, -0.52615328,  0.39669743, -0.46818909,  0.65228983,  0.33588468,  0.04645981},
                new double[] {-0.90056194, -0.12798019, -0.01099475,  0.10343884, -0.18892729, -0.75155723,  0.78546954,  0.0989614 , -0.05750858},
                new double[] { 2.60235984, -0.22970251, -0.26645517,  0.42271445, -0.20777014,  1.21974632, -1.43775937, -0.43484608,  0.01104877}
             };


            private static double[][] WEIGHTS = 
             {  //1:Down 64%,  2:Go 52%,  3:Up 72%  --> 62.5%
                new double[] {-1.74971304,  0.32019363,  0.28934383, -0.51306131,  0.41478223, -0.51648603,  0.6796491 ,  0.28852585,  0.03309349},
                new double[] {-0.96265879, -0.12211856,  0.02227827,  0.13277866, -0.19545902, -0.81227834,  0.77805648,  0.09466808, -0.02186277},
                new double[] { 2.71237183, -0.19807507, -0.3116221 ,  0.38028265, -0.21932321,  1.32876437, -1.45770559, -0.38319393, -0.01123072}
             };

            private static double[][] WEIGHTS = 
             {  //1:Down 66%,  2:Go 50%,  3:Up 74%  --> 63.3%
                new double[] {-4.28682521,  1.33594945,  5.29114825e-01, -1.09395579,  1.64046956, -5.24626291e-01, 9.68188690e-01,  6.60281630e-01,  6.41950095e-02},
                new double[] {-1.90816958, -2.06124925e-01, -7.29567387e-02, 4.03820022e-01, -7.69794217e-01, -1.19003491, 1.17844376,  8.83221262e-02, -5.98989217e-03},
                new double[] { 6.19499479, -1.12982452, -4.56158086e-01, 6.90135771e-01, -8.70675343e-01,  1.71466120, -2.14663245, -7.48603756e-01, -5.82051174e-02}
             };

        */

        private static double[][] WEIGHTS =
        {  //1:Down 80%,  2:Go 4%,  3:Up 82% --> 55.7%
        new double[] {-3.24730129,  0.90973467,  0.45426938, -0.90857456,  0.91835969, -0.63348943,  0.911782,    0.54843435,  0.16583765},
        new double[] {-1.43423523, -0.18685531, -0.23223188,  0.19156274, -0.42568255, -1.06881343,  1.29043552,  0.12993019, -0.04290935},
        new double[] { 4.68153652, -0.72287937, -0.22203751,  0.71701182, -0.49267714,  1.70230286, -2.20221752, -0.67836454, -0.12292831}
     };




        // --------------SECTION: Methods --------------

        // get the spectrum of the sample by calculating
        // the Fast Fourier Transform. 
        //    sample: an array containing the amplitudes of the wave
        //            values are double
        //    zeroPadding: number of frames with value zero required
        //            to complete a length of power of two
        public static double[] getSpectrum(double[] sample, int zeroPadding)
        {
            // Instantiate & Initialize a new FFT
            DSPLib.FFT fft = new DSPLib.FFT();
            fft.Initialize((UInt32)sample.Length, (UInt32)zeroPadding);

            // Call the DFT and get the scaled spectrum back
            Complex[] cSpectrum = fft.Execute(sample);
            double[] spectrum = DSP.ConvertComplex.ToMagnitude(cSpectrum);

           
            
            
            

            //Results without considering the padded values
            double[] result = new double[spectrum.Length - (zeroPadding / 2) - 1];
            for (int i = 0; i < result.Length; i++)
                result[i] = spectrum[i];

            return (result);

        } //End of getSpectrum(...)


        // get the spectrum of a sample by calculating
        // the Fast Fourier Transform of snips of the sample.
        // The sample is splited in snips of snipSize.
        //    sample: an array containing the amplitudes of the wave
        //    zeroPadding: number of frames with value zero required
        //            to complete a length of power of two    
        //    snipSize: number of frames for each snip
        public static double[] getSpectrum(double[] sample, int zeroPadding, int snipSize)
        {
            double[] result = new double[sample.Length / 2];

            //Process the sample snip by snip
            for (int index = 0; index < sample.Length / snipSize; index++)
            {
                //get a snip of the sample
                double[] snip = new double[snipSize];
                for (int i = 0; i < snip.Length; i++)
                    snip[i] = sample[(index * snipSize) + i];

                //Get the spectrum of the snip
                double[] spectrum = getSpectrum(snip, zeroPadding);

                //Store the results in the buffer
                for (int i = 0; i < spectrum.Length; i++)
                    result[(index * spectrum.Length) + i] = spectrum[i];

            }// End of outter loop (for)

            return (result);

        } //End of getSpectrum(...)


        // get the average spectrum by calculating
        // the weighted average of each snip.
        //    spectrum: computed spectrum from the sample
        //    snipSize: snip size used to computed the spectrum
        public static double[] getAvgSpectrum(double[] spectrum, int snipSize)
        {
            snipSize = snipSize / 2;
            int steps = spectrum.Length / snipSize;
            double[] result = new double[steps];

            //Process the spectrum snip by snip
            for (int index = 0; index < steps; index++)
            {
                //get a snip of the sample
                double total = 0.0;
                for (int i = 0; i < snipSize; i++)
                    total += spectrum[(index * snipSize) + i] * (i + 1);

                //Store the results in the buffer
                result[index] = 2 * total / (snipSize * (snipSize + 1));

            }// End of outter loop (for)

            return (result);

        } //End of getSpectrum(...)


        //Scales the data to fit it in a standard window
        public static double[] normalization(double[] data, double scale)
        {
            double[] result = new double[data.Length];
            double max = data.Max();

            //Scale the sample frame by frame
            for (int i = 0; i < data.Length; i++)
                result[i] = data[i] * scale / max;

            return (result);
        }


        // Low pass and high pass filters for the spectrum
        // remove the lower and higher frecuencies and low amplitude
        public static double[] spectrumFilter(double[] spectrum, int snipSize, double lowF, double highF, double lowA)
        {
            double[] filtered = new double[spectrum.Length];
            double freqRange = spectrum.Length / snipSize;

            //Filter frame by frame
            for (int i = 0; i < spectrum.Length; i++)
            {
                double freq = (i % snipSize + 1) * freqRange;
                filtered[i] = 0;
                if ((freq >= lowF && freq <= highF) && spectrum[i] >= lowA)
                    filtered[i] = spectrum[i];
            }

            return (filtered);
        }


        // Low pass and high pass filters for the wave
        // remove lower and higher amplitude
        public static double[] waveFilter(double[] sample, double lowA, double highA)
        {
            double[] filtered = new double[sample.Length];

            //Filter frame by frame
            for (int i = 0; i < sample.Length; i++)
            {
                filtered[i] = 0.0;
                if (sample[i] >= lowA && sample[i] <= highA)
                    filtered[i] = sample[i];
            }

            return (filtered);
        }


        // Extract the features of an array of data 
        public static double[] getDataFeatures(double[] data)
        {
            double[] features = new double[3];
            double sum_1 = 0.0, sum_2 = 0.0;
            int counter = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > 0) ++counter;
                sum_1 += data[i];
                sum_2 += data[i] * counter;
            }

            //Compute the features
            features[0] = (double)counter / (double)data.Length;                  //Ratio
            features[1] = (double)(sum_1 / (double)counter);                      //Average
            features[2] = (double)(2.0 * sum_2 / (double)(counter * (counter + 1))); //Weighted Average

            return (features);
        }


        // Extract the features related to the wave
        public static double[] extractFeatures(double[] sample, double[] spectrum, double[] spectAvg)
        {
            double[] features = new double[9];

            double[] f1 = getDataFeatures(sample);
            double[] f2 = getDataFeatures(spectrum);
            double[] f3 = getDataFeatures(spectAvg);

            features[0] = 1;       //Bias
            features[1] = f1[0];   //Ratio of Sample
            features[2] = f1[1];   //Average of Sample
            features[3] = f1[2];   //Weighted Average of Sample
            features[4] = f2[0];   //Ratio of spectrum
            features[5] = f2[1];   //Average of spectrum
            features[6] = f2[2];   //Weighted Average of spectrum
            features[7] = f3[1];   //Average of average spectrum
            features[8] = f3[2];   //Weighted Average of average spectrum

            return (features);
        }


        //Dot Product of two vectors
        private static double dotProd(double[] a, double[] b)
        {
            double result = 0.0;
            for (int i = 0; i < a.Length; i++)
                result = result + a[i] * b[i];

            return (result);
        }


        // Get the most probable Class according
        // to the learned weights
        public static int identifyClass(double[] x)
        {
            //Number of classes
            int classes = WEIGHTS.Length;

            //Term to normalize: sum (e^(w[k].x[n])) where k=0,1,...classes
            double normalizer = 0;
            for (int k = 0; k < classes; k++)
                normalizer += Math.Exp(dotProd(WEIGHTS[k], x));

            //Compute column vector h(x)
            double[] h = new double[classes];
            for (int k = 0; k < classes; k++)
                h[k] = (1.0 / normalizer) * Math.Exp(dotProd(WEIGHTS[k], x));

            //Choose the h[k] with best probability
            int i_class = 0;
            for (int k = 0; k < classes; k++)
            {
                //             Debug.Log (h[k]);
                if (h[i_class] < h[k])
                    i_class = k;
            }



            return (i_class + 1);
        }


        // Return the most probable voice command
        // from a voice sample:
        //    1 = Down
        //    2 = Go
        //    3 = Up
        public static int getCommand(double[] sample)
        {
            //Get the information of the sample
            double[] spectrum_snip = getSpectrum(sample, 0, 128);
            double[] spectrum_avg = getAvgSpectrum(spectrum_snip, 128);

            //normalization of the results
            spectrum_snip = normalization(spectrum_snip, 50);
            spectrum_avg = normalization(spectrum_avg, 50);
            sample = normalization(sample, 50);

            //Filter and get the features
            double[] upperWing = waveFilter(sample, 1, sample.Max());
            double[] spectrum_flt = spectrumFilter(spectrum_snip, 128, 100, 3500, 1);
            double[] features = extractFeatures(upperWing, spectrum_flt, spectrum_avg);

            return (identifyClass(features));
        }


        //Version for a sample of floats
        public static int getCommand(float[] sample)
        {
            //Convert to double
            double[] sample_dbl = new double[sample.Length];
            for (int i = 0; i < sample_dbl.Length; i++)
                sample_dbl[i] = sample[i];

            return (getCommand(sample_dbl));
        }


        //Version for AudioSource
        public static int getCommand(AudioSource audioSource)
        {
            //Get the sample
            float[] sample = new float[audioSource.clip.samples];
            audioSource.clip.GetData(sample, 0);

            return (getCommand(sample));
        }


    } //End of Class


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
