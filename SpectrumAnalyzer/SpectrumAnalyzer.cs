using System;

using MediaPortal.Configuration;
using MediaPortal.GUI.Library;

namespace SpectrumAnalyzer
{
  [PluginIcons("SpectrumAnalyzer.Resources.SpectrumAnalyzer_Icon.png", "SpectrumAnalyzer.Resources.SpectrumAnalyzer_Icon_Disabled.png")]

  public class SpectrumAnalyzer : IPlugin, ISetupForm
  {
    private SpectrumMain SM;
    private SpectrumAnalyzerConfig SC;

    public SpectrumAnalyzer()
    {
      SM = new SpectrumMain();
    }

    public void Start()
    {
      SM.Start();
    }

    public void Stop()
    {
      SM.Stop();
    }

    #region ISetupForm Members

    // Returns the name of the plugin which is shown in the plugin menu
    public string PluginName()
    {
      return "Spectrum Analyzer";
    }
 
    // Returns the description of the plugin is shown in the plugin menu
    public string Description()
    {
      return "Spectrum Analyzer for Mediaportal";
    }
 
    // Returns the author of the plugin which is shown in the plugin menu
    public string Author()
    {
      return "ajs";
    }
 
    // show the setup dialog
    public void ShowPlugin()
    {
      if (SC == null)
        SC = new SpectrumAnalyzerConfig();
      SC.InitSettings();
      SC.ShowDialog();
    }
 
    // Indicates whether plugin can be enabled/disabled
    public bool CanEnable()
    {
      return true;
    }
 
    // Get Windows-ID
    public int GetWindowId()
    {
      // WindowID of windowplugin belonging to this setup
      // enter your own unique code
      return 55055;
    }
 
    // Indicates if plugin is enabled by default;
    public bool DefaultEnabled()
    {
      return true;
    }
 
    // indicates if a plugin has it's own setup screen
    public bool HasSetup()
    {
      return true;
    }
 
    /// <summary>
    /// If the plugin should have it's own button on the main menu of MediaPortal then it
    /// should return true to this method, otherwise if it should not be on home
    /// it should return false
    /// </summary>
    /// <param name="strButtonText">text the button should have</param>
    /// <param name="strButtonImage">image for the button, or empty for default</param>
    /// <param name="strButtonImageFocus">image for the button, or empty for default</param>
    /// <param name="strPictureImage">subpicture for the button or empty for none</param>
    /// <returns>true : plugin needs it's own button on home
    /// false : plugin does not need it's own button on home</returns>
 
    public bool GetHome(out string strButtonText, out string strButtonImage,
      out string strButtonImageFocus, out string strPictureImage)
    {
      strButtonText = String.Empty;
      strButtonImage = String.Empty;
      strButtonImageFocus = String.Empty;
      strPictureImage = String.Empty;
      return false;
    }

    #endregion
 
  }
}