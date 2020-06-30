﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace EngineLayer.spectralLibrarySearch
{
    public class Spectrum
    {
        public Spectrum()
        {
            //Name = name;
        }
        public string Name { get; set; }
        public double? MW { get; set; }
        public double? rententionTime { get; set; }
        public double? precursorMz { get; set; }
        public int? charge_state { get; set; }
        public PeaksInformationFromSpectrum[] Peaks { get; set; }
        public double totalIonCurrent { get; set; }

        public override string ToString()
        {
            StringBuilder spectrum = new StringBuilder();
            spectrum.Append("Name: " + Name);
            spectrum.Append("precursor: " + precursorMz);
            foreach (PeaksInformationFromSpectrum eachPeak in this.Peaks)
            {

                spectrum.Append(eachPeak.Mz + "\t" + eachPeak.Intensity + "\t");
           
            }

             return spectrum.ToString();

        }
    }
}
