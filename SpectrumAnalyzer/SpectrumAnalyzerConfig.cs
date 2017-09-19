using System;
using System.Windows.Forms;

using NLog;

namespace SpectrumAnalyzer
{
  public partial class SpectrumAnalyzerConfig : Form
  {
    private readonly Logger logger = LogManager.GetCurrentClassLogger();

    public SpectrumAnalyzerConfig()
    {
      InitializeComponent();
      InitConfig();
      InitSettings();
    }

    public void InitSettings()
    {
      udSpectrumCount.Value = Utils.SpectrumCount;
      udSpectrumMax.Value = Utils.SpectrumMax;
      udSpectrumPeakFall.Value = Utils.SpectrumPeakFall;
      // = Utils.SpectrumProvider;
      cbSpectrumPeakCalculation.DataSource = Enum.GetValues(typeof(Utils.Calculation));
      cbSpectrumPeakCalculation.SelectedItem = Utils.SpectrumPeakCalculation;

      udVUMeterMax.Value = Utils.SpectrumDBMax;
      udVUMeterPeakFall.Value = Utils.SpectrumDBPeakFall;

      cbDebug.Checked = Utils.SpectrumDebug;
    }

    private void InitConfig()
    {
      Utils.InitLogger(true);
      //
      logger.Info("Spectrum Analyzer version is " + Utils.GetAllVersionNumber());
      //
      Text = Text + " " + Utils.GetAllVersionNumber();
      //
      Utils.LoadSettings();
      //
      logger.Info("Spectrum Analyzer configuration is starting.");
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      Utils.SpectrumCount = (int)udSpectrumCount.Value;
      Utils.SpectrumMax = (int)udSpectrumMax.Value;
      Utils.SpectrumPeakFall = (int)udSpectrumPeakFall.Value;
      // Utils.SpectrumProvider =
      Utils.SpectrumPeakCalculation = (Utils.Calculation)cbSpectrumPeakCalculation.SelectedItem;

      Utils.SpectrumDBMax = (int)udVUMeterMax.Value;
      Utils.SpectrumDBPeakFall = (int)udVUMeterPeakFall.Value;

      Utils.SpectrumDebug = cbDebug.Checked;

      Utils.SaveSettings();
      Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void upSpectrumPeakFall_ValueChanged(object sender, EventArgs e)
    {
      lblSpectrumPeakFallShow.Text = string.Format("{0:0.00}s", (float)udSpectrumPeakFall.Value / 1000);
    }

  }
}
