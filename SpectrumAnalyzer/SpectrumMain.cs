using System;

using NLog;

namespace SpectrumAnalyzer
{
  class SpectrumMain
  {
    private readonly Logger logger = LogManager.GetCurrentClassLogger();

    private SpectrumHandler SH;

    public SpectrumMain()
    {
      SH = new SpectrumHandler();
    }

    public void Start()
    {
      try
      {
        Utils.InitLogger();
        logger.Info("Spectrum Analyzer version is " + Utils.GetAllVersionNumber());

        Utils.LoadSettings();
        Utils.LoadSkinSettings();

        SH.SpectrumCount = Utils.SpectrumCount;
        SH.SpectrumMin = Utils.SpectrumMin;
        SH.SpectrumMax = Utils.SpectrumMax;
        SH.SpectrumSpacing = Utils.SpectrumSpacing;
        SH.SpectrumUpdate = Utils.SpectrumUpdate;
        SH.SpectrumPeakFall = Utils.SpectrumPeakFall;

        SH.SpectrumDBMin = Utils.SpectrumDBMin;
        SH.SpectrumDBMax = Utils.SpectrumDBMax;
        SH.SpectrumDBSpacing = Utils.SpectrumDBSpacing;
        SH.SpectrumDBUpdate = Utils.SpectrumDBUpdate;
        SH.SpectrumDBPeakFall = Utils.SpectrumDBPeakFall;

        SH.SpectrumProvider = Utils.SpectrumProvider;
        SH.ShowSpectrum = Utils.ShowSpectrum;

        logger.Info("Spectrum Analyzer: {0} Spectrum, {1} Peak, {2} VU Meter, Show: {3}", 
                                        Utils.Check(Utils.SpectrumEnabled), Utils.Check(Utils.SpectrumPeakEnabled), Utils.Check(Utils.SpectrumVUMeterEnabled), SH.ShowSpectrum);
        logger.Info("Spectrum Analyzer: Count: {0}, Max: {1}, Update {2}, PeakFall: {3}, Provider: {4}", 
                                        SH.SpectrumCount, SH.SpectrumMax, SH.SpectrumUpdate, SH.SpectrumPeakFall, SH.SpectrumProvider);
        logger.Info("Spectrum Analyzer: Image: {0} {1}, PeakImage: {2} {3}, Spacing: {4}", 
                                        Utils.Check(Utils.SpectrumImageFound), Utils.SpectrumImage,
                                        Utils.Check(Utils.SpectrumPeakImageFound), Utils.SpectrumPeakImage, 
                                        SH.SpectrumSpacing);
       logger.Debug("Spectrum Analyzer: VU Meter: Max: {0}, Update: {1}, PeakFall: {2}", 
                                        SH.SpectrumDBMax, SH.SpectrumDBUpdate, SH.SpectrumDBPeakFall);

        SH.StartHandler();

        logger.Info("Spectrum Analyzer is starting.");
      }
      catch (Exception ex)
      {
        logger.Error("Start: " + ex);
      }
    }

    public void Stop()
    {
      try
      {
        SH.StopHandler();
        logger.Info("Spectrum Analyzer is stopped.");
      }
      catch (Exception ex)
      {
        logger.Error("Stop: " + ex);
      }
    }

  }
}
