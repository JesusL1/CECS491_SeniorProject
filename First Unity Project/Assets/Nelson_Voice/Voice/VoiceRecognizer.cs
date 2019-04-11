/*=========================================================================
**
** Class: VoiceCommand
**
** Purpose: 
**   Implement a library for recognizing a list of voice commands.
**   
**
===========================================================================*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static DSPLib;

namespace VoiceRecognizer
{

    public static class VoiceCommand
    {

        // --------------SECTION: VARS -------------- 

        private static double[][] WEIGHTS;      //Learned weights   
        private static int counter = 0;         //counter of spectrum analysis
        private static float[] blank;           //array with zeros


        // --------------SECTION: Methods --------------


        //Load the weights from a txt asset
        public static void loadWeights()
        {
            TextAsset textFile = (TextAsset)Resources.Load("weights") as TextAsset;
            string[] lines = textFile.ToString().Split('\n');
            WEIGHTS = new double[lines.Length - 1][];

            for (int i = 0; i < WEIGHTS.Length; i++)
            {
                string[] values = lines[i].Split(' ');
                WEIGHTS[i] = new double[values.Length];
                for (int j = 0; j < values.Length; j++)
                    WEIGHTS[i][j] = double.Parse(values[j]);
            }

            //Initializes the blank array
            blank = new float[16000];
            for (int i = 0; i < blank.Length; i++) blank[i] = 0;
        }


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



        // get the average of the values by snips
        //    sample: values
        //    snipSize: snip size
        public static double[] getAvgSample(double[] sample, int snipSize)
        {
            int steps = sample.Length / snipSize;
            double[] result = new double[steps];

            //Process the sample snip by snip
            for (int index = 0; index < steps; index++)
            {
                //get a snip of the sample
                double total = 0.0;
                for (int i = 0; i < snipSize; i++)
                    total += Math.Abs(sample[(index * snipSize) + i]);

                //Store the results in the buffer
                result[index] = total / snipSize;

            }// End of outter loop (for)

            return (result);

        } //End of getAvgSample(...)



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


        // Remove values under the threshold and shift the frames to the left
        public static double[] shiftBlanks(double[] data, double threshold)
        {
            int k = 0;
            for (int i = 0; i < data.Length; i++)
            {
                double temp = data[i];
                data[i] = 0.0;
                if (temp > threshold)
                {
                    data[k] = temp;
                    k = k + 1;
                }
            }

            return (data);
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
            //Get the spectrum
            double[] spectrum_full = getSpectrum(sample, 384);
            double[] spectrum_voice = new double[4000 - 100];
            Array.Copy(spectrum_full, 100, spectrum_voice, 0, spectrum_voice.Length);
            double[] spectrum_avg = getAvgSample(spectrum_voice, 10);

            //Verify if the spectrum contains a word, increment the counter of analysis
            if (spectrum_avg.Average() > 0.0001) ++counter;
            else counter = 0;

            //Get the information of the sample
            double[] sample_avg = getAvgSample(sample, 100);
            sample_avg = shiftBlanks(sample_avg, 0.01);

            //generate the features
            double max_value = sample_avg.Max();
            if (max_value > 0 && counter > 30) //If it is a valid wave and enough analysis
            {
                counter = 0;
                spectrum_avg = normalization(spectrum_avg, max_value);
                double[] features = new double[sample_avg.Length + spectrum_avg.Length + 1];

                features[0] = 1; //Bias

                for (int i = 0; i < sample_avg.Length; i++)
                    features[i + 1] = sample_avg[i];

                for (int i = 0; i < spectrum_avg.Length; i++)
                    features[sample_avg.Length + i + 1] = spectrum_avg[i];

                return (identifyClass(features));
            }

            return (0);
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

            int command = getCommand(sample);
            if (command > 0)
                audioSource.clip.SetData(blank, 0);

            return (command);
        }

    } //End of Class

} //End of name space




