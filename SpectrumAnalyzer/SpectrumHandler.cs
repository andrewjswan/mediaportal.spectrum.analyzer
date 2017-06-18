using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

using MediaPortal.Player;

using NLog;

using Timer = System.Timers.Timer;
using MediaPortal.MusicPlayer.BASS;

namespace SpectrumAnalyzer
{
  class SpectrumHandler
  {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private Timer SpectrumTimer = null;
    private bool SpectrumTimerWork = false;

    private List<int> _spectrum = null;
    private List<int> _spectrumpeak = null;

    private int _spectrumcount = 16; // Count of Spectrum lines
    private int _spectrummin = 1;    // MP Default: 1, Min: VU1, Max: VU15
    private int _spectrummax = 15;   // Count of Spectrum VU Images in one line. MP Default: 15, Min: VU1, Max: VU15
    private int _spectrumpeakfall = 500; // Peak falling after 0.5s

    private ushort _spectrumtimer = 10;
    private ushort _spectrumpeaktime = 0;

    private BassAudioEngine Bass = null;

    public bool IsPlaying { get; set; }
    public Utils.Provider SpectrumProvider { get; set; }

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

    public int SpectrumPeakFall
    {
      get
      {
        return _spectrumpeakfall;
      }
      set
      {
        if (value >= 500 && value <= 5000)
        {
          _spectrumpeakfall = value;
        }
        else
        {
          _spectrumpeakfall = 500;
        }
      }
    }

    public SpectrumHandler()
    {
      IsPlaying = false;
      SpectrumProvider = Utils.Provider.Mediaportal;
    }

    #region Spectrum properties
    public void InitSpectrumProperties()
    {
      _spectrum = Enumerable.Repeat(_spectrummin, _spectrumcount).ToList();
      _spectrumpeak = Enumerable.Repeat(_spectrummin, _spectrumcount).ToList();

      for (int i = 1; i <= _spectrumcount; i++)
      {
        Utils.SetProperty("#VUSpectrum" + i, @"VU1.png");
        Utils.SetProperty("#VUSpectrumPeak" + i, @"VU1Peak.png");
      }
      _spectrumpeaktime = 0;

      if (Utils.SpectrumDebug)
      {
        logger.Debug("Init spectrum properties done.");
      }
    }

    public void DoneSpectrumProperties()
    {
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

      for (int i = 1; i <= _spectrumcount; i++)
      {
        Utils.SetProperty("#VUSpectrum" + i, string.Empty);
        Utils.SetProperty("#VUSpectrumPeak" + i, string.Empty);
      }
      _spectrumpeaktime = 0;

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
        SpectrumTimerWork = false;
        SpectrumTimer = new Timer();
        SpectrumTimer.Interval = Convert.ToDouble(_spectrumtimer);
        SpectrumTimer.Elapsed += new ElapsedEventHandler(OnSpectrumTimerTickEvent);
        SpectrumTimer.Start();

        if (Utils.SpectrumDebug)
        {
          logger.Debug("Spectrum timer started.");
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
      if (SpectrumTimerWork)
      {
        return;
      }
      SpectrumTimerWork = true;

      if (Utils.SpectrumDebug)
      {
        logger.Debug("Spectrum timer fired.");
      }

      Spectrum();

      SpectrumTimerWork = false;
    }

    public void DoneSpectrumTimer()
    {
      if (SpectrumTimer != null)
      {
        SpectrumTimer.Stop();
        SpectrumTimer = null;
        SpectrumTimerWork = false;

        if (Utils.SpectrumDebug)
        {
          logger.Debug("Spectrum timer done.");
        }
      }
    }
    #endregion

    #region Player
    public void InitPlayer()
    {
      if (SpectrumProvider == Utils.Provider.Mediaportal && BassMusicPlayer.IsDefaultMusicPlayer && Bass == null)
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

      unchecked { _spectrumpeaktime = (ushort)(_spectrumpeaktime + _spectrumtimer); };

      _spectrumpresent = GetSpectrum();

      if (_spectrumpresent)
      {
        for (int i = 1; i <= _spectrumcount; i++)
        {
          if (_spectrumpeaktime >= _spectrumpeakfall)
          {
            _spectrumpeak[i-1] = _spectrum[i-1];
            _spectrumpeaktime = 0;
          }
          else
          {
            // find the max as the peak value in that frequency band.
            int d = _spectrum[i-1];
            int b = _spectrumpeak[i-1];
            _spectrumpeak[i-1] = ( d > b ) ? d : b;
          }

          Utils.SetProperty("#VUSpectrum" + i, @"VU" + _spectrum[i-1] + ".png");
          Utils.SetProperty("#VUSpectrumPeak" + i, @"VU" + _spectrumpeak[i-1] + "Peak.png");
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
      if (SpectrumProvider == Utils.Provider.Mediaportal && Bass != null)
      {
        result = Bass.GetSpectrum(ref _spectrum, (byte)_spectrumcount, _spectrummin, _spectrummax);
      }
      result = result && (_spectrum != null && _spectrum.Count > 0 && _spectrum.Count == _spectrumcount);
      return result;
    }

    #endregion

    public void StartHandler()
    {
      InitSpectrumProperties() ;

      g_Player.PlayBackStarted += new g_Player.StartedHandler(OnPlayBackStarted);
      g_Player.PlayBackStopped += new g_Player.StoppedHandler(OnPlayBackStopped);
      g_Player.PlayBackEnded += new g_Player.EndedHandler(OnPlayBackEnded);
    }

    public void StopHandler()
    {
      g_Player.PlayBackStarted -= new g_Player.StartedHandler(OnPlayBackStarted);
      g_Player.PlayBackStopped -= new g_Player.StoppedHandler(OnPlayBackStopped);
      g_Player.PlayBackEnded -= new g_Player.EndedHandler(OnPlayBackEnded);

      DoneSpectrumTimer();
    }

    internal void OnPlayBackStarted(g_Player.MediaType type, string filename)
    {
      try
      {
        IsPlaying = true;
        InitSpectrumProperties();

        if (type == g_Player.MediaType.Music || type == g_Player.MediaType.Radio || MediaPortal.Util.Utils.IsLastFMStream(filename))
        {
          if (Utils.SpectrumDebug)
          {
            logger.Debug("OnPlayBackStarted: MediaType: " + type.ToString() + " LastFM: " + MediaPortal.Util.Utils.IsLastFMStream(filename).ToString() + " - " + filename);
          }
          InitPlayer();
          InitSpectrumTimer();
        }
        else if (Utils.SpectrumDebug)
        {
          logger.Debug("OnPlayBackStarted: Skiped due MediaType: " + type.ToString() + " LastFM: " + MediaPortal.Util.Utils.IsLastFMStream(filename).ToString() + " - " + filename);
        }
      }
      catch (Exception ex)
      {
        logger.Error("OnPlayBackStarted: " + ex.ToString());
      }
    }

    private void OnPlayBackStopped(g_Player.MediaType type, int stoptime, string filename)
    {
      try
      {
        IsPlaying = false;
        DoneSpectrumTimer();
        DoneSpectrumProperties();
        DonePlayer();

        if (Utils.SpectrumDebug)
        {
          logger.Debug("OnPlayBackStopped: MediaType: " + type.ToString() + " LastFM: " + MediaPortal.Util.Utils.IsLastFMStream(filename).ToString() + " - " + filename);
        }
      }
      catch (Exception ex)
      {
        logger.Error("OnPlayBackStopped: " + ex.ToString());
      }
    }

    internal void OnPlayBackEnded(g_Player.MediaType type, string filename)
    {
      try
      {
        IsPlaying = false;
        DoneSpectrumTimer();
        DoneSpectrumProperties();
        DonePlayer();

        if (Utils.SpectrumDebug)
        {
          logger.Debug("OnPlayBackEnded: MediaType: " + type.ToString() + " LastFM: " + MediaPortal.Util.Utils.IsLastFMStream(filename).ToString() + " - " + filename);
        }
      }
      catch (Exception ex)
      {
        logger.Error("OnPlayBackEnded: " + ex.ToString());
      }
    }

  }
}
