using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Threading;

using MediaPortal.MusicPlayer.BASS;
using MediaPortal.Player;

using NLog;

using Timer = System.Timers.Timer;

namespace SpectrumAnalyzer
{
  class SpectrumHandler
  {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private const int minValue = 20;
    private const int maxValue = 5000;

    private Timer SpectrumTimer = null;

    private List<int> _spectrum = null;
    private List<int> _spectrumpeak = null;

    private int _spectrumcount = 16;     // Count of Spectrum lines
    private int _spectrummin = 1;        // MP Default: 1, Min: VU1, Max: VU15
    private int _spectrummax = 15;       // Count of Spectrum VU Images in one line. MP Default: 15, Min: VU1, Max: VU15
    private int _spectrumspacing = 0;    // Space between Spectrum line in Single image
    private int _spectrumupdate = 10;    // Spectrum update time
    private int _spectrumpeakfall = 50;  // Peak falling after 0.05s

    private double _spectrumvulpeak = 0;
    private double _spectrumvurpeak = 0;

    private int _spectrumdbmin = 1;       // MP Default: 1, Min: VU1, Max: VU15
    private int _spectrumdbmax = 15;      // Count of VU Meter Images in one line. MP Default: 15, Min: VU1, Max: VU15
    private int _spectrumdbspacing = 0;   // Space between VU Meter line in Single image
    private int _spectrumdbupdate = 10;   // VU Meter update time
    private int _spectrumdbpeakfall = 50; // VU Meter Peak falling after 0.05s

    private ushort _spectrumtimer = 10;
    private ushort _spectrumtime = 0;
    private ushort _spectrumpeaktime = 0;
    private ushort _spectrumdbtime = 0;
    private ushort _spectrumdbpeaktime = 0;

    private BassAudioEngine Bass = null;
    private bool SpectrumWorking;

    private double minL = Double.MaxValue;
    private double minR = Double.MaxValue;
    private double maxL = Double.MinValue;
    private double maxR = Double.MinValue;

    #region Properties

    public Utils.Provider SpectrumProvider { get; set; }
    public Utils.Spectrum ShowSpectrum { get; set; }

    public bool IsPlaying
    {
      get
      {
        return g_Player.Playing;
      }
    }

    #region Spectrum properties
    public int SpectrumCount
    {
      get
      {
        return _spectrumcount;
      }
      set
      {
        if (value > 2 && value <= 256 && Utils.IsPowerOfTwo(value))
        {
          _spectrumcount = value;
        }
        else
        {
          _spectrumcount = 16;
        }

        InitSpectrumProperties();
      }
    }

    public int SpectrumMin
    {
      get
      {
        return _spectrummin;
      }
      set
      {
        if (value >= 0 && value < 256)
        {
          _spectrummin = value;
        }
        else
        {
          _spectrummin = 1;
        }
      }
    }

    public int SpectrumMax
    {
      get
      {
        return _spectrummax;
      }
      set
      {
        if (value > 0 && value < 256)
        {
          _spectrummax = value;
        }
        else
        {
          _spectrummax = 15;
        }
      }
    }

    public int SpectrumSpacing
    {
      get
      {
        return _spectrumspacing;
      }
      set
      {
        if (value >= 0 && value <= 100)
        {
          _spectrumspacing = value;
        }
        else
        {
          _spectrumspacing = 0;
        }
      }
    }
    
    public int SpectrumUpdate
    {
      get
      {
        return _spectrumupdate;
      }
      set
      {
        if (value >= minValue && value <= maxValue)
        {
          _spectrumupdate = value;
        }
        else
        {
          _spectrumupdate = 20;
        }
      }
    }

    public int SpectrumPeakFall
    {
      get
      {
        return _spectrumpeakfall;
      }
      set
      {
        if (value >= minValue && value <= maxValue)
        {
          _spectrumpeakfall = value;
        }
        else
        {
          _spectrumpeakfall = 50;
        }
      }
    }
    #endregion

    #endregion

    #region VUMeter properties
    public int SpectrumDBMin
    {
      get
      {
        return _spectrumdbmin;
      }
      set
      {
        if (value >= 0 && value < 256)
        {
          _spectrumdbmin = value;
        }
        else
        {
          _spectrumdbmin = 1;
        }
      }
    }

    public int SpectrumDBMax
    {
      get
      {
        return _spectrumdbmax;
      }
      set
      {
        if (value > 0 && value < 256)
        {
          _spectrumdbmax = value;
        }
        else
        {
          _spectrumdbmax = 15;
        }
      }
    }

    public int SpectrumDBSpacing
    {
      get
      {
        return _spectrumdbspacing;
      }
      set
      {
        if (value >= 0 && value <= 100)
        {
          _spectrumdbspacing = value;
        }
        else
        {
          _spectrumdbspacing = 0;
        }
      }
    }
    
    public int SpectrumDBUpdate
    {
      get
      {
        return _spectrumdbupdate;
      }
      set
      {
        if (value >= minValue && value <= maxValue)
        {
          _spectrumdbupdate = value;
        }
        else
        {
          _spectrumdbupdate = 30;
        }
      }
    }

    public int SpectrumDBPeakFall
    {
      get
      {
        return _spectrumdbpeakfall;
      }
      set
      {
        if (value >= minValue && value <= maxValue)
        {
          _spectrumdbpeakfall = value;
        }
        else
        {
          _spectrumdbpeakfall = 50;
        }
      }
    }
    #endregion

    public SpectrumHandler()
    {
      SpectrumProvider = Utils.Provider.Mediaportal;
      ShowSpectrum = Utils.Spectrum.Single;
      SpectrumWorking = false;
    }

    #region Spectrum methods
    public void InitSpectrumProperties()
    {
      Utils.SetProperty("available", "false");

      Utils.SetProperty("#VUSpectrum", string.Empty);
      for (int i = 1; i <= _spectrumcount; i++)
      {
        Utils.SetProperty("#VUSpectrum" + i, @"VU" + _spectrummin + ".png");
        Utils.SetProperty("#VUSpectrumPeak" + i, @"VU" + _spectrummin + "Peak.png");
      }

      Utils.SetProperty("vumeter.available", Utils.SpectrumVUMeterEnabled ? "true" : "false");

      Utils.SetProperty("vumeterL", @"VU" + _spectrumdbmin + ".png");
      Utils.SetProperty("vumeterR", @"VU" + _spectrumdbmin + ".png");

      Utils.SetProperty("vumeterLPeak", @"VU" + _spectrumdbmin + "Peak.png");
      Utils.SetProperty("vumeterRPeak", @"VU" + _spectrumdbmin + "Peak.png");

      Utils.SetProperty("vumeterLtext", Utils.VUMeterEmptyText);
      Utils.SetProperty("vumeterRtext", Utils.VUMeterEmptyText);

      _spectrumtime = 0;
      _spectrumpeaktime = 0;
      _spectrumdbtime = 0;
      _spectrumdbpeaktime = 0;

      _spectrum = Enumerable.Repeat(_spectrummin, _spectrumcount).ToList();
      _spectrumpeak = Enumerable.Repeat(_spectrummin, _spectrumcount).ToList();

      SpectrumWorking = false;

      if (Utils.SpectrumDebug)
      {
        logger.Debug("Init spectrum properties done.");
      }
    }

    public void DoneSpectrumProperties()
    {
      _spectrumtime = 0;
      _spectrumpeaktime = 0;
      _spectrumdbtime = 0;
      _spectrumdbpeaktime = 0;

      if (_spectrum != null)
      {
        _spectrum.Clear();
        _spectrum = null;
      }

      if (_spectrumpeak != null)
      {
        _spectrumpeak.Clear();
        _spectrumpeak = null;
      }

      Utils.SetProperty("available", "false");

      Utils.SetProperty("#VUSpectrum", string.Empty);
      for (int i = 1; i <= _spectrumcount; i++)
      {
        Utils.SetProperty("#VUSpectrum" + i, string.Empty);
        Utils.SetProperty("#VUSpectrumPeak" + i, string.Empty);
      }

      Utils.SetProperty("vumeter.available", "false");

      Utils.SetProperty("vumeterL", string.Empty);
      Utils.SetProperty("vumeterR", string.Empty);

      Utils.SetProperty("vumeterLPeak", string.Empty);
      Utils.SetProperty("vumeterRPeak", string.Empty);

      Utils.SetProperty("vumeterLtext", string.Empty);
      Utils.SetProperty("vumeterRtext", string.Empty);

      SpectrumWorking = false;

      if (Utils.SpectrumDebug)
      {
        logger.Debug("Clean spectrum properties done.");
      }
    }
    #endregion

    #region Spectrum Timer
    public void InitSpectrumTimer()
    {
      if (SpectrumTimer == null)
      {
        SpectrumTimer = new Timer();
        SpectrumTimer.Interval = Convert.ToDouble(_spectrumtimer);
        SpectrumTimer.Elapsed += new ElapsedEventHandler(OnSpectrumTimerTickEvent);
        SpectrumTimer.Start();

        if (Utils.SpectrumDebug)
        {
          logger.Debug("Spectrum timer started.");
          minL = Double.MaxValue;
          minR = Double.MaxValue;
          maxL = Double.MinValue;
          maxR = Double.MinValue;
        }
      }
    }
 
    /// <summary>
    /// The Spectrum Timer has elapsed.
    /// Set the Spectrum Properties
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnSpectrumTimerTickEvent(object sender, ElapsedEventArgs e)
    {
      if (Utils.SpectrumDebug)
      {
        logger.Debug("Spectrum timer fired...");
      }
      if (!SpectrumWorking)
      {
        SpectrumWorking = true;
        try
        {
          if (Utils.SpectrumEnabled)
          {
            if (_spectrumtime == 0 || _spectrumtime >= _spectrumupdate)
            {
              _spectrumtime = 0;
              Utils.SetProperty("available", Spectrum() ? "true" : "false");
            }
            unchecked { _spectrumtime = (ushort)(_spectrumtime + _spectrumtimer); };
          }

          if (Utils.SpectrumVUMeterEnabled)
          {
            if (_spectrumdbtime == 0 || _spectrumdbtime >= _spectrumdbupdate)
            {
              _spectrumdbtime = 0;
              RMSLevel();
            }
            unchecked { _spectrumdbtime = (ushort)(_spectrumdbtime + _spectrumtimer); };
          }
        }
        catch (Exception ex)
        {
          logger.Error("OnSpectrumTimerTickEvent: " + ex);
        }
        finally
        {
          SpectrumWorking = false;
        }
      }
    }

    public void DoneSpectrumTimer()
    {
      if (SpectrumTimer != null)
      {
        SpectrumTimer.Stop();
        SpectrumTimer = null;

        if (Utils.SpectrumDebug)
        {
          logger.Debug("Spectrum timer done.");
          logger.Debug("VU Meter Min L:{0} R:{1} Max L{2} R{3}", minL, minR, maxL, maxR);
        }
      }
    }
    #endregion

    #region Player
    public void InitPlayer(bool force = false)
    {
      if (SpectrumProvider == Utils.Provider.Mediaportal && BassMusicPlayer.IsDefaultMusicPlayer && (Bass == null || force))
      {
        Bass = BassMusicPlayer.Player;
        if (Utils.SpectrumDebug)
        {
          logger.Debug("Bass Player init.");
        }
      }
    }

    public void DonePlayer()
    {
      if (SpectrumProvider == Utils.Provider.Mediaportal && Bass != null)
      {
        Bass = null;
        if (Utils.SpectrumDebug)
        {
          logger.Debug("Bass Player done.");
        }
      }
    }
    #endregion

    #region Spectrum analyzer
    public bool Spectrum()
    {
      bool _spectrumpresent = false;

      if (!IsPlaying)
      {
        return _spectrumpresent; 
      }

      _spectrumpresent = GetSpectrum();

      if (_spectrumpresent)
      {
        for (int i = 1; i <= _spectrumcount; i++)
        {
          if (!IsPlaying)
          {
            return false;
          }

          if (ShowSpectrum == Utils.Spectrum.Both || ShowSpectrum == Utils.Spectrum.Multi)
          {
            Utils.SetProperty("#VUSpectrum" + i, @"VU" + _spectrum[i - 1] + ".png");
          }
          if (Utils.SpectrumPeakEnabled)
          {
            if (_spectrumpeaktime >= _spectrumpeakfall)
            {
              _spectrumpeak[i - 1] = _spectrum[i - 1];
              _spectrumpeaktime = 0;
            }
            else
            {
              _spectrumpeak[i - 1] = Utils.CalcPeak(_spectrum[i - 1], _spectrumpeak[i - 1]);
            }
            if (ShowSpectrum == Utils.Spectrum.Both || ShowSpectrum == Utils.Spectrum.Multi)
            {
              Utils.SetProperty("#VUSpectrumPeak" + i, @"VU" + _spectrumpeak[i - 1] + "Peak.png");
            }
          }
        }

        if (IsPlaying)
        {
          if (ShowSpectrum == Utils.Spectrum.Both || ShowSpectrum == Utils.Spectrum.Single)
          {
            Utils.SetProperty("#VUSpectrum", SpectrumImage.BuildConcatSpectrumImage(_spectrum, _spectrumpeak, _spectrumcount, _spectrumspacing, _spectrummin, _spectrummax));
          }
        }

        if (Utils.SpectrumPeakEnabled)
        {
          unchecked { _spectrumpeaktime = (ushort)(_spectrumpeaktime + _spectrumtimer); };
        }
      }
      else
      {
        InitSpectrumProperties();
      }

      return _spectrumpresent;
    }

    public bool GetSpectrum()
    {
      bool result = false;

      if (!IsPlaying)
      {
        return result;
      }

      InitPlayer();

      if (SpectrumProvider == Utils.Provider.Mediaportal && Bass != null)
      {
        result = Bass.GetSpectrum(ref _spectrum, (byte)_spectrumcount, _spectrummin, _spectrummax);
      }
      result = result && (_spectrum != null && _spectrum.Count > 0 && _spectrum.Count == _spectrumcount);
      return result;
    }
    #endregion

    #region VE Meter
    public void RMSLevel()
    {
      double dBlevelL = 0.0;
      double dBlevelR = 0.0;

      if (!IsPlaying)
      {
        Utils.SetProperty("vumeterLtext", Utils.VUMeterEmptyText);
        Utils.SetProperty("vumeterRtext", Utils.VUMeterEmptyText);
        return;
      }

      GetRMSLevel(out dBlevelL, out dBlevelR);

      if (!Double.IsInfinity(dBlevelL))
      {
        Utils.SetProperty("vumeterLtext", string.Format(Utils.VUMeterText, dBlevelL));
        if (Utils.SpectrumDebug)
        {
          minL = (minL > dBlevelL) ? dBlevelL : minL;
          maxL = (maxL < dBlevelL) ? dBlevelL : maxL;
        }
      }
      else
      {
        Utils.SetProperty("vumeterLtext", Utils.VUMeterEmptyText);
      }

      if (!Double.IsInfinity(dBlevelR))
      {
        Utils.SetProperty("vumeterRtext", string.Format(Utils.VUMeterText, dBlevelR));
        if (Utils.SpectrumDebug)
        {
          minR = (minR > dBlevelR) ? dBlevelR : minR;
          maxR = (maxR < dBlevelR) ? dBlevelR : maxR;
        }
      }
      else
      {
        Utils.SetProperty("vumeterRtext", Utils.VUMeterEmptyText);
      }

      if (_spectrumdbpeaktime >= _spectrumdbpeakfall)
      {
        _spectrumvulpeak = dBlevelL;
        _spectrumvurpeak = dBlevelR;
        _spectrumdbpeaktime = 0;
      }
      else
      {
        _spectrumvulpeak = Utils.CalcPeak(dBlevelL, _spectrumvulpeak);
        _spectrumvurpeak = Utils.CalcPeak(dBlevelR, _spectrumvurpeak);
      }

      int iLeft = Utils.CalcdB(dBlevelL, _spectrummin, _spectrummax);
      int iLeftPeak = Utils.CalcdB(_spectrumvulpeak, _spectrummin, _spectrummax);
      int iRight = Utils.CalcdB(dBlevelR, _spectrummin, _spectrummax);
      int iRightPeak = Utils.CalcdB(_spectrumvurpeak, _spectrummin, _spectrummax);

      if (ShowSpectrum == Utils.Spectrum.Both || ShowSpectrum == Utils.Spectrum.Multi)
      {
        Utils.SetProperty("vumeterL", "VU" + iLeft + ".png");
        Utils.SetProperty("vumeterLPeak", "VU" + Utils.CalcdB(_spectrumvulpeak, _spectrummin, _spectrummax).ToString() + "Peak.png");
        Utils.SetProperty("vumeterR", "VU" + Utils.CalcdB(dBlevelR, _spectrummin, _spectrummax).ToString() + ".png");
        Utils.SetProperty("vumeterRPeak", "VU" + Utils.CalcdB(_spectrumvurpeak, _spectrummin, _spectrummax).ToString() + "Peak.png");
      }
      if (ShowSpectrum == Utils.Spectrum.Both || ShowSpectrum == Utils.Spectrum.Single)
      {
        List<int> vumeter = new List<int>() {iLeft, iRight};
        List<int> vumeterpeak = new List<int>() {iLeft, iRight};
        Utils.SetProperty("vumeter", SpectrumImage.BuildConcatVUMeterImage(vumeter, vumeterpeak, _spectrumdbspacing, _spectrumdbmin, _spectrumdbmax));
      }

      unchecked { _spectrumdbpeaktime = (ushort)(_spectrumdbpeaktime + _spectrumtimer); };
    }

    public void GetRMSLevel(out double dBlevelL, out double dBlevelR)
    {
      dBlevelL = 0.0;
      dBlevelR = 0.0;

      if (!IsPlaying)
      {
        return;
      }

      InitPlayer();

      if (SpectrumProvider == Utils.Provider.Mediaportal && Bass != null)
      {
        Bass.RMS(out dBlevelL, out dBlevelR);
      }
    }
    #endregion

    #region Playback
    internal void PlaybackInit(bool force = false)
    {
      InitSpectrumProperties();
      InitPlayer(force);
      InitSpectrumTimer();
      SpectrumImage.InitSpectrumImages();

      if (Utils.SpectrumDebug)
      {
        logger.Debug("Playback init.");
      }
    }

    internal void PlaybackDone()
    {
      DoneSpectrumTimer();
      DonePlayer();
      DoneSpectrumProperties();
      SpectrumImage.ClearImages();

      if (Utils.SpectrumDebug)
      {
        logger.Debug("Playback done.");
      }
    }
    #endregion

    #region Handler events
    public void StartHandler()
    {
      InitSpectrumProperties() ;

      g_Player.PlayBackStarted += new g_Player.StartedHandler(OnPlayBackStarted);
      g_Player.PlayBackChanged += new g_Player.ChangedHandler(OnPlayBackChanged);
      g_Player.PlayBackStopped += new g_Player.StoppedHandler(OnPlayBackStopped);
      g_Player.PlayBackEnded += new g_Player.EndedHandler(OnPlayBackEnded);
    }

    public void StopHandler()
    {
      g_Player.PlayBackStarted -= new g_Player.StartedHandler(OnPlayBackStarted);
      g_Player.PlayBackChanged -= new g_Player.ChangedHandler(OnPlayBackChanged);
      g_Player.PlayBackStopped -= new g_Player.StoppedHandler(OnPlayBackStopped);
      g_Player.PlayBackEnded -= new g_Player.EndedHandler(OnPlayBackEnded);

      DoneSpectrumTimer();
    }

    internal void OnPlayBackStarted(g_Player.MediaType type, string filename)
    {
      try
      {
        if ((Utils.SpectrumEnabled || Utils.SpectrumVUMeterEnabled) &&
            (type == g_Player.MediaType.Music || type == g_Player.MediaType.Radio || MediaPortal.Util.Utils.IsLastFMStream(filename)))
        {
          logger.Debug("OnPlayBackStarted: MediaType: " + type.ToString() + " LastFM: " + MediaPortal.Util.Utils.IsLastFMStream(filename).ToString() + " - " + filename);
          PlaybackInit(true);
        }
        else // if (Utils.SpectrumDebug)
        {
          logger.Debug("OnPlayBackStarted: Skiped due " + Utils.Check(Utils.SpectrumEnabled) + " Spectrum, " + Utils.Check(Utils.SpectrumVUMeterEnabled) + "VUMeter, MediaType: " + type.ToString() + " LastFM: " + MediaPortal.Util.Utils.IsLastFMStream(filename).ToString() + " - " + filename);
        }
      }
      catch (Exception ex)
      {
        logger.Error("OnPlayBackStarted: " + ex.ToString());
      }
    }

    private void OnPlayBackChanged(g_Player.MediaType type, int stoptime, string filename)
    {
      logger.Debug("OnPlayBackChanged: MediaType: " + type.ToString() + " LastFM: " + MediaPortal.Util.Utils.IsLastFMStream(filename).ToString() + " - " + filename);
      PlaybackDone();
    }

    private void OnPlayBackStopped(g_Player.MediaType type, int stoptime, string filename)
    {
      logger.Debug("OnPlayBackStopped: MediaType: " + type.ToString() + " LastFM: " + MediaPortal.Util.Utils.IsLastFMStream(filename).ToString() + " - " + filename);
      PlaybackDone();
    }

    internal void OnPlayBackEnded(g_Player.MediaType type, string filename)
    {
      logger.Debug("OnPlayBackEnded: MediaType: " + type.ToString() + " LastFM: " + MediaPortal.Util.Utils.IsLastFMStream(filename).ToString() + " - " + filename);
      PlaybackDone();
    }
    #endregion

  }
}
