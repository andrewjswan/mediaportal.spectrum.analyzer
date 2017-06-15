using System;
using System.Windows.Forms;
using MediaPortal.GUI.Library;
 
namespace SpectrumAnalyzer
{
  public class SpectrumAnalyzer : IPlugin, ISetupForm
  {
 
    public SpectrumAnalyzer()
    {
 
    }

    public void Start()
    {
      throw new NotImplementedException();
    }

    public void Stop()
    {
      throw new NotImplementedException();
    }

    #region ISetupForm Members

    // Returns the name of the plugin which is shown in the plugin menu
    public string PluginName()
    {
      return "Spectrum Handler";
    }
 
    // Returns the description of the plugin is shown in the plugin menu
    public string Description()
    {
      return "Spectrum Handler for Mediaportal";
    }
 
    // Returns the author of the plugin which is shown in the plugin menu
    public string Author()
    {
      return "ajs";
    }
 
    // show the setup dialog
    public void ShowPlugin()
    {
      MessageBox.Show("WBR, ajs");
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
      return false;
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