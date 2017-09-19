namespace SpectrumAnalyzer
{
  partial class SpectrumAnalyzerConfig
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpectrumAnalyzerConfig));
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.groupInfo = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.cbDebug = new System.Windows.Forms.CheckBox();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabSpectrum = new System.Windows.Forms.TabPage();
      this.tabVUMeter = new System.Windows.Forms.TabPage();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.lblVUMeterPeakFallShow = new System.Windows.Forms.Label();
      this.udVUMeterPeakFall = new System.Windows.Forms.NumericUpDown();
      this.udVUMeterMax = new System.Windows.Forms.NumericUpDown();
      this.lblVUMeterPeakFall = new System.Windows.Forms.Label();
      this.lblVUMeterMax = new System.Windows.Forms.Label();
      this.groupSpectrum = new System.Windows.Forms.GroupBox();
      this.cbSpectrumPeakCalculation = new System.Windows.Forms.ComboBox();
      this.lblSpectrumPeakCalculation = new System.Windows.Forms.Label();
      this.lblSpectrumPeakFallShow = new System.Windows.Forms.Label();
      this.udSpectrumPeakFall = new System.Windows.Forms.NumericUpDown();
      this.udSpectrumMax = new System.Windows.Forms.NumericUpDown();
      this.udSpectrumCount = new System.Windows.Forms.NumericUpDown();
      this.lblSpectrumPeakFall = new System.Windows.Forms.Label();
      this.lblSpectrumMax = new System.Windows.Forms.Label();
      this.lblSpectrumCount = new System.Windows.Forms.Label();
      this.groupInfo.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabSpectrum.SuspendLayout();
      this.tabVUMeter.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.udVUMeterPeakFall)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.udVUMeterMax)).BeginInit();
      this.groupSpectrum.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.udSpectrumPeakFall)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.udSpectrumMax)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.udSpectrumCount)).BeginInit();
      this.SuspendLayout();
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(263, 304);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(344, 304);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // groupInfo
      // 
      this.groupInfo.Controls.Add(this.label2);
      this.groupInfo.Controls.Add(this.label1);
      this.groupInfo.Location = new System.Drawing.Point(24, 204);
      this.groupInfo.Name = "groupInfo";
      this.groupInfo.Size = new System.Drawing.Size(395, 89);
      this.groupInfo.TabIndex = 3;
      this.groupInfo.TabStop = false;
      this.groupInfo.Text = "Info";
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(16, 44);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(373, 32);
      this.label2.TabIndex = 2;
      this.label2.Text = "Can be overridden by the skins design.\r\nCheck the SkinSpectrumAnalyzer.xml file i" +
    "n current skin.";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(16, 25);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(86, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Default  settings!";
      // 
      // cbDebug
      // 
      this.cbDebug.AutoSize = true;
      this.cbDebug.Location = new System.Drawing.Point(43, 308);
      this.cbDebug.Name = "cbDebug";
      this.cbDebug.Size = new System.Drawing.Size(78, 17);
      this.cbDebug.TabIndex = 4;
      this.cbDebug.Text = "Debug info";
      this.cbDebug.UseVisualStyleBackColor = true;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabSpectrum);
      this.tabControl1.Controls.Add(this.tabVUMeter);
      this.tabControl1.Location = new System.Drawing.Point(12, 12);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(423, 190);
      this.tabControl1.TabIndex = 5;
      // 
      // tabSpectrum
      // 
      this.tabSpectrum.Controls.Add(this.groupSpectrum);
      this.tabSpectrum.Location = new System.Drawing.Point(4, 22);
      this.tabSpectrum.Name = "tabSpectrum";
      this.tabSpectrum.Padding = new System.Windows.Forms.Padding(3);
      this.tabSpectrum.Size = new System.Drawing.Size(415, 164);
      this.tabSpectrum.TabIndex = 0;
      this.tabSpectrum.Text = "Spectrum";
      this.tabSpectrum.UseVisualStyleBackColor = true;
      // 
      // tabVUMeter
      // 
      this.tabVUMeter.Controls.Add(this.groupBox1);
      this.tabVUMeter.Location = new System.Drawing.Point(4, 22);
      this.tabVUMeter.Name = "tabVUMeter";
      this.tabVUMeter.Padding = new System.Windows.Forms.Padding(3);
      this.tabVUMeter.Size = new System.Drawing.Size(415, 164);
      this.tabVUMeter.TabIndex = 1;
      this.tabVUMeter.Text = "VUMeter";
      this.tabVUMeter.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.lblVUMeterPeakFallShow);
      this.groupBox1.Controls.Add(this.udVUMeterPeakFall);
      this.groupBox1.Controls.Add(this.udVUMeterMax);
      this.groupBox1.Controls.Add(this.lblVUMeterPeakFall);
      this.groupBox1.Controls.Add(this.lblVUMeterMax);
      this.groupBox1.Location = new System.Drawing.Point(9, 4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(395, 153);
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Spectrum";
      // 
      // lblVUMeterPeakFallShow
      // 
      this.lblVUMeterPeakFallShow.AutoSize = true;
      this.lblVUMeterPeakFallShow.Location = new System.Drawing.Point(320, 86);
      this.lblVUMeterPeakFallShow.Name = "lblVUMeterPeakFallShow";
      this.lblVUMeterPeakFallShow.Size = new System.Drawing.Size(27, 13);
      this.lblVUMeterPeakFallShow.TabIndex = 6;
      this.lblVUMeterPeakFallShow.Text = "0.5s";
      // 
      // udVUMeterPeakFall
      // 
      this.udVUMeterPeakFall.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
      this.udVUMeterPeakFall.Location = new System.Drawing.Point(239, 84);
      this.udVUMeterPeakFall.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
      this.udVUMeterPeakFall.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
      this.udVUMeterPeakFall.Name = "udVUMeterPeakFall";
      this.udVUMeterPeakFall.Size = new System.Drawing.Size(75, 20);
      this.udVUMeterPeakFall.TabIndex = 5;
      this.udVUMeterPeakFall.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
      // 
      // udVUMeterMax
      // 
      this.udVUMeterMax.Location = new System.Drawing.Point(239, 55);
      this.udVUMeterMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
      this.udVUMeterMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.udVUMeterMax.Name = "udVUMeterMax";
      this.udVUMeterMax.Size = new System.Drawing.Size(75, 20);
      this.udVUMeterMax.TabIndex = 4;
      this.udVUMeterMax.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
      // 
      // lblVUMeterPeakFall
      // 
      this.lblVUMeterPeakFall.AutoSize = true;
      this.lblVUMeterPeakFall.Location = new System.Drawing.Point(16, 86);
      this.lblVUMeterPeakFall.Name = "lblVUMeterPeakFall";
      this.lblVUMeterPeakFall.Size = new System.Drawing.Size(89, 13);
      this.lblVUMeterPeakFall.TabIndex = 2;
      this.lblVUMeterPeakFall.Text = "Peak falling after:";
      // 
      // lblVUMeterMax
      // 
      this.lblVUMeterMax.AutoSize = true;
      this.lblVUMeterMax.Location = new System.Drawing.Point(16, 57);
      this.lblVUMeterMax.Name = "lblVUMeterMax";
      this.lblVUMeterMax.Size = new System.Drawing.Size(186, 13);
      this.lblVUMeterMax.TabIndex = 1;
      this.lblVUMeterMax.Text = "Count of VU Meter Images in one line:";
      // 
      // groupSpectrum
      // 
      this.groupSpectrum.Controls.Add(this.cbSpectrumPeakCalculation);
      this.groupSpectrum.Controls.Add(this.lblSpectrumPeakCalculation);
      this.groupSpectrum.Controls.Add(this.lblSpectrumPeakFallShow);
      this.groupSpectrum.Controls.Add(this.udSpectrumPeakFall);
      this.groupSpectrum.Controls.Add(this.udSpectrumMax);
      this.groupSpectrum.Controls.Add(this.udSpectrumCount);
      this.groupSpectrum.Controls.Add(this.lblSpectrumPeakFall);
      this.groupSpectrum.Controls.Add(this.lblSpectrumMax);
      this.groupSpectrum.Controls.Add(this.lblSpectrumCount);
      this.groupSpectrum.Location = new System.Drawing.Point(9, 4);
      this.groupSpectrum.Name = "groupSpectrum";
      this.groupSpectrum.Size = new System.Drawing.Size(395, 153);
      this.groupSpectrum.TabIndex = 3;
      this.groupSpectrum.TabStop = false;
      this.groupSpectrum.Text = "Spectrum";
      // 
      // cbSpectrumPeakCalculation
      // 
      this.cbSpectrumPeakCalculation.FormattingEnabled = true;
      this.cbSpectrumPeakCalculation.Location = new System.Drawing.Point(239, 114);
      this.cbSpectrumPeakCalculation.Name = "cbSpectrumPeakCalculation";
      this.cbSpectrumPeakCalculation.Size = new System.Drawing.Size(150, 21);
      this.cbSpectrumPeakCalculation.TabIndex = 8;
      // 
      // lblSpectrumPeakCalculation
      // 
      this.lblSpectrumPeakCalculation.AutoSize = true;
      this.lblSpectrumPeakCalculation.Location = new System.Drawing.Point(16, 117);
      this.lblSpectrumPeakCalculation.Name = "lblSpectrumPeakCalculation";
      this.lblSpectrumPeakCalculation.Size = new System.Drawing.Size(127, 13);
      this.lblSpectrumPeakCalculation.TabIndex = 7;
      this.lblSpectrumPeakCalculation.Text = "Peak calculation method:";
      // 
      // lblSpectrumPeakFallShow
      // 
      this.lblSpectrumPeakFallShow.AutoSize = true;
      this.lblSpectrumPeakFallShow.Location = new System.Drawing.Point(320, 86);
      this.lblSpectrumPeakFallShow.Name = "lblSpectrumPeakFallShow";
      this.lblSpectrumPeakFallShow.Size = new System.Drawing.Size(27, 13);
      this.lblSpectrumPeakFallShow.TabIndex = 6;
      this.lblSpectrumPeakFallShow.Text = "0.5s";
      // 
      // udSpectrumPeakFall
      // 
      this.udSpectrumPeakFall.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
      this.udSpectrumPeakFall.Location = new System.Drawing.Point(239, 84);
      this.udSpectrumPeakFall.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
      this.udSpectrumPeakFall.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
      this.udSpectrumPeakFall.Name = "udSpectrumPeakFall";
      this.udSpectrumPeakFall.Size = new System.Drawing.Size(75, 20);
      this.udSpectrumPeakFall.TabIndex = 5;
      this.udSpectrumPeakFall.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
      // 
      // udSpectrumMax
      // 
      this.udSpectrumMax.Location = new System.Drawing.Point(239, 55);
      this.udSpectrumMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
      this.udSpectrumMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.udSpectrumMax.Name = "udSpectrumMax";
      this.udSpectrumMax.Size = new System.Drawing.Size(75, 20);
      this.udSpectrumMax.TabIndex = 4;
      this.udSpectrumMax.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
      // 
      // udSpectrumCount
      // 
      this.udSpectrumCount.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
      this.udSpectrumCount.Location = new System.Drawing.Point(239, 26);
      this.udSpectrumCount.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
      this.udSpectrumCount.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
      this.udSpectrumCount.Name = "udSpectrumCount";
      this.udSpectrumCount.Size = new System.Drawing.Size(75, 20);
      this.udSpectrumCount.TabIndex = 3;
      this.udSpectrumCount.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
      // 
      // lblSpectrumPeakFall
      // 
      this.lblSpectrumPeakFall.AutoSize = true;
      this.lblSpectrumPeakFall.Location = new System.Drawing.Point(16, 86);
      this.lblSpectrumPeakFall.Name = "lblSpectrumPeakFall";
      this.lblSpectrumPeakFall.Size = new System.Drawing.Size(89, 13);
      this.lblSpectrumPeakFall.TabIndex = 2;
      this.lblSpectrumPeakFall.Text = "Peak falling after:";
      // 
      // lblSpectrumMax
      // 
      this.lblSpectrumMax.AutoSize = true;
      this.lblSpectrumMax.Location = new System.Drawing.Point(16, 57);
      this.lblSpectrumMax.Name = "lblSpectrumMax";
      this.lblSpectrumMax.Size = new System.Drawing.Size(204, 13);
      this.lblSpectrumMax.TabIndex = 1;
      this.lblSpectrumMax.Text = "Count of Spectrum VU Images in one line:";
      // 
      // lblSpectrumCount
      // 
      this.lblSpectrumCount.AutoSize = true;
      this.lblSpectrumCount.Location = new System.Drawing.Point(16, 28);
      this.lblSpectrumCount.Name = "lblSpectrumCount";
      this.lblSpectrumCount.Size = new System.Drawing.Size(122, 13);
      this.lblSpectrumCount.TabIndex = 0;
      this.lblSpectrumCount.Text = "Count of Spectrum lines:";
      // 
      // SpectrumAnalyzerConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(439, 338);
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.cbDebug);
      this.Controls.Add(this.groupInfo);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SpectrumAnalyzerConfig";
      this.Text = "Spectrum Analyzer";
      this.groupInfo.ResumeLayout(false);
      this.groupInfo.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabSpectrum.ResumeLayout(false);
      this.tabVUMeter.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.udVUMeterPeakFall)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.udVUMeterMax)).EndInit();
      this.groupSpectrum.ResumeLayout(false);
      this.groupSpectrum.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.udSpectrumPeakFall)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.udSpectrumMax)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.udSpectrumCount)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.GroupBox groupInfo;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox cbDebug;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabSpectrum;
    private System.Windows.Forms.GroupBox groupSpectrum;
    private System.Windows.Forms.ComboBox cbSpectrumPeakCalculation;
    private System.Windows.Forms.Label lblSpectrumPeakCalculation;
    private System.Windows.Forms.Label lblSpectrumPeakFallShow;
    private System.Windows.Forms.NumericUpDown udSpectrumPeakFall;
    private System.Windows.Forms.NumericUpDown udSpectrumMax;
    private System.Windows.Forms.NumericUpDown udSpectrumCount;
    private System.Windows.Forms.Label lblSpectrumPeakFall;
    private System.Windows.Forms.Label lblSpectrumMax;
    private System.Windows.Forms.Label lblSpectrumCount;
    private System.Windows.Forms.TabPage tabVUMeter;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label lblVUMeterPeakFallShow;
    private System.Windows.Forms.NumericUpDown udVUMeterPeakFall;
    private System.Windows.Forms.NumericUpDown udVUMeterMax;
    private System.Windows.Forms.Label lblVUMeterPeakFall;
    private System.Windows.Forms.Label lblVUMeterMax;
  }
}