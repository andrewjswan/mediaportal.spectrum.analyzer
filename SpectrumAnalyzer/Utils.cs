using System;
using System.Drawing;
using System.IO;
using System.Reflection;

using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Profile;
using MediaPortal.Services;
using MediaPortal.Util;

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
    private const string SkinConfigFilename = "SkinSpectrumAnalyzer.xml";
    private const string LogFileName = "SpectrumAnalyzer{0}.log";
    private const string OldLogFileName = "SpectrumAnalyzer{0}.bak";

    public const string VUMeterDBText = "dB";
    // public const string VUMeterText = "{0:00.#}dB";
    public const string VUMeterText = "{0:00}" + VUMeterDBText;
    public const string VUMeterEmptyText = "--" + VUMeterDBText;

    public static bool SpectrumEnabled;
    public static bool SpectrumPeakEnabled;
    public static bool SpectrumVUMeterEnabled;
    public static Provider SpectrumProvider;

    public static int SpectrumCount;
    public static int SpectrumMin;
    public static int SpectrumMax;
    public static int SpectrumUpdate;
    public static int SpectrumPeakFall;

    public static int SpectrumDBMin;
    public static int SpectrumDBMax;
    public static int SpectrumDBUpdate;
    public static int SpectrumDBPeakFall;

    public static string SpectrumImage;
    public static bool SpectrumImageFound;
    public static string SpectrumPeakImage;
    public static bool SpectrumPeakImageFound;
    public static int SpectrumSpacing;

    public static string VUMeterImage;
    public static bool VUMeterImageFound;
    public static string VUMeterPeakImage;
    public static bool VUMeterPeakImageFound;
    public static int SpectrumDBSpacing;

    public static Calculation SpectrumPeakCalculation;
    public static Spectrum ShowSpectrum;
    public static Spectrum ShowVUMeter;

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

    /// <summary>
    /// Loads an Image from a File by invoking GDI Plus instead of using build-in .NET methods, or falls back to Image.FromFile
    /// Can perform up to 10x faster
    /// </summary>
    /// <param name="filename">The filename to load</param>
    /// <returns>A .NET Image object</returns>
    public static Image LoadImageFastFromFile(string filename)
    {
      filename = Path.GetFullPath(filename);
      if (!File.Exists(filename))
      {
        return null;
      }

      Image imageFile = null;
      try
      {
        try
        {
          imageFile = ImageFast.FromFile(filename);
        }
        catch (Exception)
        {
          Log.Debug("GUIImageAllocator: Reverting to slow ImageLoading for: " + filename);
          imageFile = Image.FromFile(filename);
        }
      }
      catch (FileNotFoundException fe)
      {
        Log.Debug("GUIImageAllocator: Image does not exist: " + filename + " - " + fe.Message);
        return null;
      }
      catch (Exception e)
      {
        // this probably means the image is bad
        Log.Debug("GUIImageAllocator: Unable to load Imagefile (corrupt?): " + filename + " - " + e.Message);
        return null;
      }
      return imageFile;
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

      LogLevel logLevel = LogLevel.Debug;
      int intLogLevel = 3;

      using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.MPSettings())
      {
        intLogLevel = xmlreader.GetValueAsInt("general", "loglevel", intLogLevel);
      }

      switch (intLogLevel)
      {
        case 0:
          logLevel = LogLevel.Error;
          break;
        case 1:
          logLevel = LogLevel.Warn;
          break;
        case 2:
          logLevel = LogLevel.Info;
          break;
        default:
          logLevel = LogLevel.Debug;
          break;
      }
      #if DEBUG
      logLevel = LogLevel.Debug;
      #endif

      var loggingRule = new LoggingRule("*", logLevel, fileTarget);
      loggingConfiguration.LoggingRules.Add(loggingRule);
      LogManager.Configuration = loggingConfiguration;
    }

    public static void LoadSettings()
    {
      #region Init variables
      SpectrumEnabled = true;
      SpectrumPeakEnabled = true;
      SpectrumVUMeterEnabled = true;

      SpectrumCount = 16;
      SpectrumMin = 1;
      SpectrumMax = 15;
      SpectrumUpdate = 20;
      SpectrumPeakFall = 500;
      SpectrumPeakCalculation = Calculation.Average;
      SpectrumProvider = Provider.Mediaportal;

      SpectrumDBMin = SpectrumMin;
      SpectrumDBMax = SpectrumMax;
      SpectrumDBUpdate = 20;
      SpectrumDBPeakFall = 500;

      ShowSpectrum = Spectrum.Both;
      ShowVUMeter = Spectrum.Both;
      SpectrumDebug = false;
      #endregion

      try
      {
        logger.Debug("Load settings from: " + ConfigFilename);
        #region Load settings
        using (var settings = new Settings(Config.GetFile((Config.Dir)10, ConfigFilename)))
        {
          SpectrumEnabled = settings.GetValueAsBool("Enabled", "Spectrum", SpectrumEnabled);
          SpectrumPeakEnabled = settings.GetValueAsBool("Enabled", "SpectrumPeak", SpectrumPeakEnabled);
          SpectrumVUMeterEnabled = settings.GetValueAsBool("Enabled", "SpectrumVUMeter", SpectrumVUMeterEnabled);

          Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumCount", SpectrumCount.ToString()), out SpectrumCount);
          Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumMax", SpectrumMax.ToString()), out SpectrumMax);
          Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumUpdate", SpectrumUpdate.ToString()), out SpectrumUpdate);
          Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumPeakFall", SpectrumPeakFall.ToString()), out SpectrumPeakFall);

          Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumVUMeterMax", SpectrumDBMax.ToString()), out SpectrumDBMax);
          Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumVUMeterUpdate", SpectrumDBUpdate.ToString()), out SpectrumDBUpdate);
          Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumVUMeterPeakFall", SpectrumDBPeakFall.ToString()), out SpectrumDBPeakFall);

          string spectrumProvider = settings.GetValueAsString("SpectrumAnalyzer", "SpectrumProvider", SpectrumProvider.ToString());
          Provider _spectrumProvider;
          if (string.IsNullOrEmpty(spectrumProvider))
          {
            _spectrumProvider = SpectrumProvider;
          }
          else
          {
            if (!Enum.TryParse(spectrumProvider, out _spectrumProvider))
            {
              _spectrumProvider = SpectrumProvider;
            }
            if (!Enum.IsDefined(typeof(Provider), _spectrumProvider))
            {
              _spectrumProvider = SpectrumProvider;
            }
          }
          SpectrumProvider = _spectrumProvider;

          string spectrumShow = settings.GetValueAsString("SpectrumAnalyzer", "SpectrumShow", ShowSpectrum.ToString());
          Spectrum _spectrumShow;
          if (string.IsNullOrEmpty(spectrumShow))
          {
            _spectrumShow = ShowSpectrum;
          }
          else
          {
            if (!Enum.TryParse(spectrumShow, out _spectrumShow))
            {
              _spectrumShow = ShowSpectrum;
            }
            if (!Enum.IsDefined(typeof(Spectrum), _spectrumShow))
            {
              _spectrumShow = ShowSpectrum;
            }
          }
          ShowSpectrum = _spectrumShow;

          string spectrumPeakCalculation = settings.GetValueAsString("SpectrumAnalyzer", "SpectrumPeakCalculation", SpectrumPeakCalculation.ToString());
          Calculation _spectrumPeakCalculation;
          if (string.IsNullOrEmpty(spectrumPeakCalculation))
          {
            _spectrumPeakCalculation = SpectrumPeakCalculation;
          }
          else
          {
            if (!Enum.TryParse(spectrumPeakCalculation, out _spectrumPeakCalculation))
            {
              _spectrumPeakCalculation = SpectrumPeakCalculation;
            }
            if (!Enum.IsDefined(typeof(Calculation), _spectrumPeakCalculation))
            {
              _spectrumPeakCalculation = SpectrumPeakCalculation;
            }
          }
          SpectrumPeakCalculation = _spectrumPeakCalculation;

          string vumeterShow = settings.GetValueAsString("SpectrumAnalyzer", "VUMeterShow", ShowVUMeter.ToString());
          Spectrum _vumeterShow;
          if (string.IsNullOrEmpty(vumeterShow))
          {
            _vumeterShow = ShowVUMeter;
          }
          else
          {
            if (!Enum.TryParse(vumeterShow, out _vumeterShow))
            {
              _vumeterShow = ShowVUMeter;
            }
            if (!Enum.IsDefined(typeof(Spectrum), _vumeterShow))
            {
              _vumeterShow = ShowVUMeter;
            }
          }
          ShowVUMeter = _vumeterShow;

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
      logger.Debug("Spectrum Analyzer Config Settings: {0} Spectrum, {1} Peak, {2} VU Meter, Show: {3}, VUMeter: {4}", 
                                                       Check(Utils.SpectrumEnabled), Check(SpectrumPeakEnabled), Check(Utils.SpectrumVUMeterEnabled), ShowSpectrum, ShowVUMeter);
      logger.Debug("Spectrum Analyzer Config Settings: Count: {0}, Max: {1}, Update: {2}, PeakFall: {3}, Peak: {4}, Provider: {5}, {6} Debug", 
                                                       SpectrumCount, SpectrumMax, SpectrumUpdate, SpectrumPeakFall, SpectrumPeakCalculation, SpectrumProvider, 
                                                       Check(SpectrumDebug));
      logger.Debug("Spectrum Analyzer Config Settings: VU Meter: Max: {0}, Update: {1}, PeakFall: {2}, Peak: {3}", 
                                                       SpectrumDBMax, SpectrumDBUpdate, SpectrumDBPeakFall, SpectrumPeakCalculation);
                                                       
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
          xmlwriter.SetValueAsBool("Enabled", "Spectrum", SpectrumEnabled);
          xmlwriter.SetValueAsBool("Enabled", "SpectrumPeak", SpectrumPeakEnabled);
          xmlwriter.SetValueAsBool("Enabled", "SpectrumVUMeter", SpectrumVUMeterEnabled);

          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumCount", SpectrumCount);
          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumMax", SpectrumMax);
          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumUpdate", SpectrumUpdate);
          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumPeakFall", SpectrumPeakFall);
          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumPeakCalculation", SpectrumPeakCalculation);

          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumVUMeterMax", SpectrumDBMax);
          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumVUMeterUpdate", SpectrumDBUpdate);
          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumVUMeterPeakFall", SpectrumDBPeakFall);

          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumProvider", SpectrumProvider.ToString());
          xmlwriter.SetValue("SpectrumAnalyzer", "SpectrumShow", ShowSpectrum.ToString());
          xmlwriter.SetValue("SpectrumAnalyzer", "VUMeterShow", ShowVUMeter.ToString());

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

    public static void LoadSkinSettings()
    {
      #region Init variables
      SpectrumImage = @"Spectrum\Spectrum.png";
      SpectrumPeakImage = @"Spectrum\SpectrumPeak.png";
      SpectrumSpacing = 2;
      SpectrumImageFound = false;
      SpectrumPeakImageFound = false;

      VUMeterImage = @"Spectrum\VUMeter.png";
      VUMeterPeakImage = @"Spectrum\VUMeterPeak.png";
      SpectrumDBSpacing = 5;
      VUMeterImageFound = false;
      VUMeterPeakImageFound = false;
      #endregion

      string filename = GUIGraphicsContext.GetThemedSkinFile(@"\" + SkinConfigFilename);
      if (File.Exists(filename))
      {
        try
        {
          logger.Debug("Load Skin settings from: " + filename);
          #region Load settings
          using (var settings = new Settings(filename))
          {
            SpectrumEnabled = settings.GetValueAsBool("Enabled", "Spectrum", SpectrumEnabled);
            SpectrumPeakEnabled = settings.GetValueAsBool("Enabled", "SpectrumPeak", SpectrumPeakEnabled);
            SpectrumVUMeterEnabled = settings.GetValueAsBool("Enabled", "SpectrumVUMeter", SpectrumVUMeterEnabled);

            SpectrumImage = settings.GetValueAsString("SpectrumAnalyzer", "SpectrumImage", SpectrumImage);
            SpectrumPeakImage = settings.GetValueAsString("SpectrumAnalyzer", "SpectrumPeakImage", SpectrumPeakImage);

            Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumSpacing", SpectrumSpacing.ToString()), out SpectrumSpacing);

            Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumCount", SpectrumCount.ToString()), out SpectrumCount);
            Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumMax", SpectrumMax.ToString()), out SpectrumMax);

            VUMeterImage = settings.GetValueAsString("SpectrumAnalyzer", "SpectrumVUMeterImage", VUMeterImage);
            VUMeterPeakImage = settings.GetValueAsString("SpectrumAnalyzer", "SpectrumVUMeterPeakImage", VUMeterPeakImage);

            Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumVUMeterSpacing", SpectrumDBSpacing.ToString()), out SpectrumDBSpacing);
            Int32.TryParse(settings.GetValueAsString("SpectrumAnalyzer", "SpectrumVUMeterMax", SpectrumDBMax.ToString()), out SpectrumDBMax);

            string spectrumShow = settings.GetValueAsString("SpectrumAnalyzer", "SpectrumShow", ShowSpectrum.ToString());
            Spectrum _spectrumShow;
            if (string.IsNullOrEmpty(spectrumShow))
            {
              _spectrumShow = ShowSpectrum;
            }
            else
            {
              if (!Enum.TryParse(spectrumShow, out _spectrumShow))
              {
                _spectrumShow = ShowSpectrum;
              }
              if (!Enum.IsDefined(typeof(Spectrum), _spectrumShow))
              {
                _spectrumShow = ShowSpectrum;
              }
            }
            ShowSpectrum = _spectrumShow;

            string vumeterShow = settings.GetValueAsString("SpectrumAnalyzer", "VUMeterShow", ShowVUMeter.ToString());
            Spectrum _vumeterShow;
            if (string.IsNullOrEmpty(vumeterShow))
            {
              _vumeterShow = ShowVUMeter;
            }
            else
            {
              if (!Enum.TryParse(vumeterShow, out _vumeterShow))
              {
                _vumeterShow = ShowVUMeter;
              }
              if (!Enum.IsDefined(typeof(Spectrum), _vumeterShow))
              {
                _vumeterShow = ShowVUMeter;
              }
            }
            ShowVUMeter = _vumeterShow;
          }
          #endregion
          logger.Debug("Load Skin settings from: " + ConfigFilename + " complete.");
        }
        catch (Exception ex)
        {
          logger.Error("LoadSkinSettings: " + ex);
        }
      }
      else
      {
        logger.Debug("Load Skin settings from: " + filename + " failed. File not found!");
      }
      //
      #region Report Settings
      SpectrumImage = @"\Media\" + SpectrumImage;
      SpectrumPeakImage = @"\Media\" + SpectrumPeakImage;

      string spectrumImage = GUIGraphicsContext.GetThemedSkinFile(SpectrumImage);
      string spectrumPeakImage = GUIGraphicsContext.GetThemedSkinFile(SpectrumPeakImage);

      SpectrumImageFound = File.Exists(spectrumImage); 
      SpectrumPeakImageFound = File.Exists(spectrumPeakImage);

      VUMeterImage = @"\Media\" + SpectrumImage;
      VUMeterPeakImage = @"\Media\" + SpectrumPeakImage;

      string vumeterImage = GUIGraphicsContext.GetThemedSkinFile(VUMeterImage);
      string vumeterPeakImage = GUIGraphicsContext.GetThemedSkinFile(VUMeterPeakImage);
      
      VUMeterImageFound = File.Exists(vumeterImage); 
      VUMeterPeakImageFound = File.Exists(vumeterPeakImage);

      logger.Debug("Spectrum Analyzer Skin Settings: {0} Spectrum, {1} Peak, {2} VU Meter, Show: {3}, VU Meter: {4}", 
                                                     Check(Utils.SpectrumEnabled), Check(SpectrumPeakEnabled), Check(Utils.SpectrumVUMeterEnabled), ShowSpectrum, ShowVUMeter);
      logger.Debug("Spectrum Analyzer Skin Settings: Spectrum Image: {0} {1}, PeakImage: {2} {3}, Spacing: {4}", 
                                                     Check(SpectrumImageFound), SpectrumImage, Check(SpectrumPeakImageFound), SpectrumPeakImage, SpectrumSpacing);
      logger.Debug("Spectrum Analyzer Skin Settings: VU Meter Image: {0} {1}, PeakImage: {2} {3}, Spacing: {4}", 
                                                     Check(VUMeterImageFound), VUMeterImage, Check(VUMeterPeakImageFound), VUMeterPeakImage, SpectrumDBSpacing);
      logger.Debug("Spectrum Analyzer Skin Settings: Count: {0}, Max: {1}, VU Meter Max: {2}", SpectrumCount, SpectrumMax, SpectrumDBMax);
      #endregion
    }

    #region Check [x]|[ ] for Log file
    public static string Check(bool Value, bool Box = true)
    {
      return (Box ? "[" : string.Empty) + (Value ? "x" : " ") + (Box ? "]" : string.Empty);
    }
    #endregion

    public static int CalcPeak(int spectrum, int peak)
    {
      if (SpectrumPeakCalculation == Calculation.Maximum)
      {
        // find the max as the peak value in that frequency band.
        return (spectrum > peak) ? spectrum : peak;
      }
      else if (SpectrumPeakCalculation == Calculation.Average)
      {
        if (spectrum > peak)
        {
          return spectrum; 
        }
        int average = (spectrum + peak) / 2;
        return Math.Min(peak, spectrum + average);
      }
      else
      {
        return spectrum;
      }
    }

    public static double CalcPeak(double level, double peak)
    {
      if (SpectrumPeakCalculation == Calculation.Maximum)
      {
        // find the max as the peak value in that frequency band.
        return (level > peak) ? level : peak;
      }
      else if (SpectrumPeakCalculation == Calculation.Average)
      {
        if (level > peak)
        {
          return level;
        }
        double average = (level + peak) / 2;
        return Math.Min(peak, level + average);
      }
      else
      {
        return level;
      }
    }

    public static int CalcdB(double dB, int min, int max)
    {
      if (Double.IsInfinity(dB))
      {
        return min;
      }

      int minMP = -96;
      int maxMP = 0;

      return (int)((((dB - minMP) * (max - min)) / (maxMP - minMP)) + min);
    }

    public enum Provider
    {
      Mediaportal,
      CSCore,
    }

    public enum Spectrum
    {
      Single,
      Multi,
      Both,
    }

    public enum Calculation
    {
      Maximum,
      Average,
    }
  }
}
