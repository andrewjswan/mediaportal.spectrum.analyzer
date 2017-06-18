using System;
using System.IO;
using System.Reflection;

using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Profile;
using MediaPortal.Services;

using NLog;
using NLog.Config;
using NLog.Targets;

namespace SpectrumAnalyzer
{
  internal static class Utils
  {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private const string SpectrumAnalyzerPrefix = "#spectrumanalyzer.";

    private const string ConfigFilename = "SpectrumAnalyzer.xml";
    private const string LogFileName = "SpectrumAnalyzer{0}.log";
    private const string OldLogFileName = "SpectrumAnalyzer{0}.bak";

    public static int SpectrumCount;
    public static int SpectrumMax;
    public static int SpectrumPeakFall;
    public static Provider SpectrumProvider;
    public static bool SpectrumDebug;

    public static string GetAllVersionNumber()
    {
      return Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }

    internal static void SetProperty(string property, string value)
    {
      if (string.IsNullOrWhiteSpace(property))
        return;

      if (property.IndexOf('#') == -1)
      {
        property = SpectrumAnalyzerPrefix + property;
      }

      try
      {
        GUIPropertyManager.SetProperty(property, value);
      }
      catch (Exception ex)
      {
        logger.Error("SetProperty: " + ex);
      }
    }

    internal static bool IsPowerOfTwo(int x)
    {
      return (x != 0) && ((x & (x - 1)) == 0);
    }

    public static void InitLogger(bool config = false)
    {
      var loggingConfiguration = LogManager.Configuration ?? new LoggingConfiguration();

      string logFile = string.Format(LogFileName,(config ? "Config" : string.Empty)) ;
      string logOldFile = string.Format(OldLogFileName,(config ? "Config" : string.Empty)) ;

      try
      {
        var fileInfo = new FileInfo(Config.GetFile((Config.Dir)1, logFile));
        if (fileInfo.Exists)
        {
          if (File.Exists(Config.GetFile((Config.Dir)1, logOldFile)))
          {
            File.Delete(Config.GetFile((Config.Dir)1, logOldFile));
          }
          fileInfo.CopyTo(Config.GetFile((Config.Dir)1, logOldFile));
          fileInfo.Delete();
        }
      }
      catch { }

      var fileTarget = new FileTarget()
      {
        FileName = Config.GetFile((Config.Dir)1, logFile),
        Encoding = "utf-8",
        Layout = "${date:format=dd-MMM-yyyy HH\\:mm\\:ss} ${level:fixedLength=true:padding=5} [${logger:fixedLength=true:padding=20:shortName=true}]: ${message} ${exception:format=tostring}"
      };

      loggingConfiguration.AddTarget("spectrum-analyzer", fileTarget);

      var settings = new Settings(Config.GetFile((Config.Dir)10, "MediaPortal.xml"));
      LogLevel minLevel;
      switch ((int)(Level)settings.GetValueAsInt("general", "loglevel", 0))
      {
        case 0:
        minLevel = LogLevel.Error;
        break;
        case 1:
        minLevel = LogLevel.Warn;
        break;
        case 2:
        minLevel = LogLevel.Info;
        break;
        default:
        minLevel = LogLevel.Debug;
        break;
      }
      var loggingRule = new LoggingRule("*", minLevel, fileTarget);
      loggingConfiguration.LoggingRules.Add(loggingRule);
      LogManager.Configuration = loggingConfiguration;
    }

    public static void LoadSettings()
    {
      #region Init variables
      SpectrumCount = 16;
      SpectrumMax = 15;
      SpectrumPeakFall = 500;
      SpectrumProvider = Provider.Mediaportal;
      SpectrumDebug = false;
      #endregion

      try
      {
        logger.Debug("Load settings from: " + ConfigFilename);
        #region Load settings
        using (var settings = new Settings(Config.GetFile((Config.Dir)10, ConfigFilename)))
        {
          Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumCount", SpectrumCount.ToString()), out SpectrumCount);
          Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumMax", SpectrumMax.ToString()), out SpectrumMax);
          Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumPeakFall", SpectrumPeakFall.ToString()), out SpectrumPeakFall);

          string spectrumProvider = settings.GetValueAsString("SpectrumAnalyzer", "SpectrumProvider", SpectrumProvider.ToString());
          if (string.IsNullOrEmpty(spectrumProvider))
          {
            SpectrumProvider = Provider.Mediaportal;
          }
          Provider _spectrumProvider;
          if (!Enum.TryParse(spectrumProvider, out _spectrumProvider))
          {
            SpectrumProvider = Provider.Mediaportal;
          }
          if (!Enum.IsDefined(typeof(Provider), _spectrumProvider))
          {
            SpectrumProvider = Provider.Mediaportal;
          }

          SpectrumDebug = settings.GetValueAsBool("SpectrumAnalyzer", "SpectrumDebug", SpectrumDebug);
        }
        #endregion
        logger.Debug("Load settings from: " + ConfigFilename + " complete.");
      }
      catch (Exception ex)
      {
        logger.Error("LoadSettings: " + ex);
      }
      //
      #region Report Settings
      logger.Debug("Spectrum Analyzer load from Config: Count: {0}, Max: {1}, PeakFall: {2}, Provider: {3}, {4} Debug", SpectrumCount, SpectrumMax, SpectrumPeakFall, SpectrumProvider, Check(SpectrumDebug));
      #endregion
    }

    public static void SaveSettings()
    {
      try
      {
        logger.Debug("Save settings to: " + ConfigFilename);
        #region Save settings
        using (var xmlwriter = new Settings(Config.GetFile((Config.Dir)10, ConfigFilename)))
        {
          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumCount", SpectrumCount);
          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumMax", SpectrumMax);
          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumPeakFall", SpectrumPeakFall);
          
          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumProvider", SpectrumProvider.ToString());

          xmlwriter.SetValueAsBool("SpectrumAnalyzer", "SpectrumDebug", SpectrumDebug);
        }
        #endregion
        logger.Debug("Save settings to: " + ConfigFilename + " complete.");
      }
      catch (Exception ex)
      {
        logger.Error("SaveSettings: " + ex);
      }
    }

    #region Check [x]|[ ] for Log file
    public static string Check(bool Value, bool Box = true)
    {
      return (Box ? "[" : string.Empty) + (Value ? "x" : " ") + (Box ? "]" : string.Empty);
    }
    #endregion

    public enum Provider
    {
      Mediaportal,
      CSCore,
    }
  }
}
