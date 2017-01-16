﻿using InternalLogicEngineLayer;
using NUnit.Framework;
using System.Collections.Generic;
using OldInternalLogic;
using MassSpectrometry;
using Spectra;
using System;
using System.Collections;
using InternalLogicTaskLayer;
using Chemistry;

namespace Test
{
	[TestFixture]
	public class ClassicSearchEngineTest
	{

		[Test]
		public void TestClassicSearchEngine()
		{
			var myMsDataFile = new TestDataFile();
			var variableModifications = new List<MorpheusModification>();
			var fixedModifications = new List<MorpheusModification>();
			var proteinList = new List<Protein> { new Protein("MNNNKQQQ", null, null, new Dictionary<int, List<MorpheusModification>>(), new int[0], new int[0], new string[0], null, null, 0, false) };

			var productMassTolerance = new Tolerance(ToleranceUnit.Absolute, 0.01);
			var searchModes = new List<SearchMode> { new SinglePpmAroundZeroSearchMode("", 5) };
			var protease = new Protease("Custom Protease", new List<string> { "K" }, new List<string>(), Terminus.C, CleavageSpecificity.Full, null, null, null);
			var engine = new ClassicSearchEngine(myMsDataFile, 0, variableModifications, fixedModifications, proteinList, productMassTolerance, protease, searchModes);
			var searchResults = (ClassicSearchResults)engine.Run();

			// Single search mode
			Assert.AreEqual(1, searchResults.outerPsms.Length);

			// Two scans, even including the MS1 scans
			Assert.AreEqual(2, searchResults.outerPsms[0].Length);

			Assert.IsTrue(searchResults.outerPsms[0][1].Score > 1);
			Assert.AreEqual(2, searchResults.outerPsms[0][1].scanNumber);
			Assert.AreEqual("QQQ", searchResults.outerPsms[0][1].ps.BaseSequence);

		}

		class TestDataFile : IMsDataFile<DefaultMzSpectrum>
		{
			public TestDataFile()
			{
				var mz1 = new double []{ 50, 60, 70, 80, 90, 100 };
				var intensities1 = new double []{ 1, 1, 1, 1, 1, 1 };

				var mz2 = new double []{ 50, 60, 70, 147.0764, 257.1244, 275.1350 };
				var intensities2 = new double[]{ 1, 1, 1, 1, 1, 1 };

				var MassSpectrum1 = new DefaultMzSpectrum(mz1, intensities1, false);
				var MassSpectrum2 = new DefaultMzSpectrum(mz2, intensities2, false);

				Scans = new List<TestScan> { new TestScan(1, 1, MassSpectrum1), new TestScan(2, 2, MassSpectrum2, 402.18629720155.ToMassToChargeRatio(2), 2, 1) };
			}
			public string FilePath
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public string Name
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public int NumSpectra
			{
				get
				{
					return Scans.Count;
				}
			}

			public void Close()
			{
				throw new NotImplementedException();
			}

			readonly List<TestScan> Scans;

			public IEnumerator<IMsDataScan<IMzSpectrum<MzPeak>>> GetEnumerator()
			{
				return Scans.GetEnumerator();
			}

			public IMsDataScan<IMzSpectrum<MzPeak>> GetOneBasedScan(int oneBasedScanNumber)
			{
				return Scans[oneBasedScanNumber - 1];
			}

			public void LoadAllScansInMemory()
			{
				throw new NotImplementedException();
			}

			public void Open()
			{
				throw new NotImplementedException();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			IMsDataScan<DefaultMzSpectrum> IMsDataFile<DefaultMzSpectrum>.GetOneBasedScan(int oneBasedScanNumber)
			{
				throw new NotImplementedException();
			}

			IEnumerator<IMsDataScan<DefaultMzSpectrum>> IEnumerable<IMsDataScan<DefaultMzSpectrum>>.GetEnumerator()
			{
				return Scans.GetEnumerator();
			}
		}

		class TestScan : IMsDataScan<DefaultMzSpectrum>
		{
			double selectedIonGuessMonoisotopicIntensity;

			public TestScan(int OneBasedScanNumber, double RetentionTime, DefaultMzSpectrum MassSpectrum, double selectedIonGuessMonoisotopicMZ, int selectedIonGuessChargeStateGuess, double selectedIonGuessMonoisotopicIntensity)
			{
				MsnOrder = 2;
				this.OneBasedScanNumber = OneBasedScanNumber;
				this.RetentionTime = RetentionTime;
				this.MassSpectrum = MassSpectrum;

				this.selectedIonGuessMonoisotopicMZ = selectedIonGuessMonoisotopicMZ;
				this.selectedIonGuessChargeStateGuess = selectedIonGuessChargeStateGuess;
				this.selectedIonGuessMonoisotopicIntensity = selectedIonGuessMonoisotopicIntensity;
			}
			public TestScan(int OneBasedScanNumber, double RetentionTime, DefaultMzSpectrum MassSpectrum)
			{
				MsnOrder = 1;
				this.OneBasedScanNumber = OneBasedScanNumber;
				this.RetentionTime = RetentionTime;
				this.MassSpectrum = MassSpectrum;
			}
			public string id
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public double InjectionTime
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public bool isCentroid
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public DefaultMzSpectrum MassSpectrum { get; private set; }

			public int MsnOrder { get; private set; }

			public MZAnalyzerType MzAnalyzer
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public int OneBasedScanNumber { get; private set; }

			public Polarity Polarity
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public double RetentionTime { get; private set; }

			public string ScanFilter
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public MzRange ScanWindowRange
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public double TotalIonCurrent
			{
				get
				{
					return MassSpectrum.SumOfAllY;
				}
			}

			public void tranformByApplyingFunctionsToSpectraAndReplacingPrecursorMZs(Func<MzPeak, double> convertorForSpectrum, double newPrecursorMZ, double selectedIonGuessMonoisotopicMZ)
			{
				throw new NotImplementedException();
			}

			public bool TryGetDissociationType(out DissociationType DissociationType)
			{
				throw new NotImplementedException();
			}

			public bool TryGetIsolationMZ(out double IsolationMZ)
			{
				throw new NotImplementedException();
			}

			public bool TryGetIsolationRange(out MzRange IsolationRange)
			{
				throw new NotImplementedException();
			}

			public bool TryGetIsolationWidth(out double IsolationWidth)
			{
				throw new NotImplementedException();
			}

			public bool TryGetPrecursorID(out string PrecursorID)
			{
				throw new NotImplementedException();
			}

			public bool TryGetPrecursorOneBasedScanNumber(out int precursorOneBasedScanNumber)
			{
				throw new NotImplementedException();
			}

			int selectedIonGuessChargeStateGuess;

			public bool TryGetSelectedIonGuessChargeStateGuess(out int SelectedIonGuessChargeStateGuess)
			{
				if (MsnOrder == 2)
				{
					SelectedIonGuessChargeStateGuess = selectedIonGuessChargeStateGuess;
					return true;
				}
				SelectedIonGuessChargeStateGuess = 0;
				return false;
			}

			public bool TryGetSelectedIonGuessIntensity(out double SelectedIonGuessIntensity)
			{
				throw new NotImplementedException();
			}

			public bool TryGetSelectedIonGuessMonoisotopicIntensity(out double SelectedIonGuessMonoisotopicIntensity)
			{
				if (MsnOrder == 2)
				{
					SelectedIonGuessMonoisotopicIntensity = selectedIonGuessMonoisotopicIntensity;
					return true;
				}
				SelectedIonGuessMonoisotopicIntensity = double.NaN;
				return false;
			}

			double selectedIonGuessMonoisotopicMZ;

			public bool TryGetSelectedIonGuessMonoisotopicMZ(out double SelectedIonGuessMonoisotopicMZ)
			{
				if (MsnOrder == 2)
				{
					SelectedIonGuessMonoisotopicMZ = selectedIonGuessMonoisotopicMZ;
					return true;
				}
				SelectedIonGuessMonoisotopicMZ = double.NaN;
				return false;
			}

			public bool TryGetSelectedIonGuessMZ(out double SelectedIonGuessMZ)
			{
				throw new NotImplementedException();
			}
		}
	}

}