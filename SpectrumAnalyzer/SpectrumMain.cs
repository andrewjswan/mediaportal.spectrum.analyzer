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

        SH.SpectrumCount = Utils.SpectrumCount;
        SH.SpectrumMax = Utils.SpectrumMax;
        SH.SpectrumPeakFall = Utils.SpectrumPeakFall;
        SH.SpectrumProvider = Utils.SpectrumProvider;

        logger.Info("Spectrum Analyzer: Count: {0}, Max: {1}, PeakFall: {2}, Provider: {3}", SH.SpectrumCount, SH.SpectrumMax, SH.SpectrumPeakFall, SH.SpectrumProvider);

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
