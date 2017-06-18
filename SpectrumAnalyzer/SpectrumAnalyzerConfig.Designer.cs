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
      this.groupSpectrum = new System.Windows.Forms.GroupBox();
      this.lblSpectrumPeakFallShow = new System.Windows.Forms.Label();
      this.udSpectrumPeakFall = new System.Windows.Forms.NumericUpDown();
      this.udSpectrumMax = new System.Windows.Forms.NumericUpDown();
      this.udSpectrumCount = new System.Windows.Forms.NumericUpDown();
      this.lblSpectrumPeakFall = new System.Windows.Forms.Label();
      this.lblSpectrumMax = new System.Windows.Forms.Label();
      this.lblSpectrumCount = new System.Windows.Forms.Label();
      this.groupSpectrum.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.udSpectrumPeakFall)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.udSpectrumMax)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.udSpectrumCount)).BeginInit();
      this.SuspendLayout();
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(251, 137);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(332, 137);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // groupSpectrum
      // 
      this.groupSpectrum.Controls.Add(this.lblSpectrumPeakFallShow);
      this.groupSpectrum.Controls.Add(this.udSpectrumPeakFall);
      this.groupSpectrum.Controls.Add(this.udSpectrumMax);
      this.groupSpectrum.Controls.Add(this.udSpectrumCount);
      this.groupSpectrum.Controls.Add(this.lblSpectrumPeakFall);
      this.groupSpectrum.Controls.Add(this.lblSpectrumMax);
      this.groupSpectrum.Controls.Add(this.lblSpectrumCount);
      this.groupSpectrum.Location = new System.Drawing.Point(12, 12);
      this.groupSpectrum.Name = "groupSpectrum";
      this.groupSpectrum.Size = new System.Drawing.Size(395, 115);
      this.groupSpectrum.TabIndex = 2;
      this.groupSpectrum.TabStop = false;
      this.groupSpectrum.Text = "Spectrum";
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
      this.udSpectrumPeakFall.ValueChanged += new System.EventHandler(this.upSpectrumPeakFall_ValueChanged);
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
      this.ClientSize = new System.Drawing.Size(419, 168);
      this.Controls.Add(this.groupSpectrum);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SpectrumAnalyzerConfig";
      this.Text = "Spectrum Analyzer";
      this.groupSpectrum.ResumeLayout(false);
      this.groupSpectrum.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.udSpectrumPeakFall)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.udSpectrumMax)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.udSpectrumCount)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.GroupBox groupSpectrum;
    private System.Windows.Forms.Label lblSpectrumPeakFallShow;
    private System.Windows.Forms.NumericUpDown udSpectrumPeakFall;
    private System.Windows.Forms.NumericUpDown udSpectrumMax;
    private System.Windows.Forms.NumericUpDown udSpectrumCount;
    private System.Windows.Forms.Label lblSpectrumPeakFall;
    private System.Windows.Forms.Label lblSpectrumMax;
    private System.Windows.Forms.Label lblSpectrumCount;
  }
}