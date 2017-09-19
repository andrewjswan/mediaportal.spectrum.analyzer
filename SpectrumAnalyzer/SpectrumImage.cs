using System;
using System.Collections.Generic;
using System.Drawing;

using MediaPortal.GUI.Library;

using NLog;

namespace SpectrumAnalyzer
{
  class SpectrumImage
  {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private static int MaxSpectrumImages = 100;

    private static Image spectrumImage = null;
    private static Image spectrumPeak = null;

    private static int iSpectrumWidth = 0;
    private static int iSpectrumHeight = 0;

    private static bool bSpectrumVertical = true;

    private static Image vumeterImage = null;
    private static Image vumeterPeak = null;

    private static int iVUMeterWidth = 0;
    private static int iVUMeterHeight = 0;

    private static bool bVUMeterVertical = true;

    // static string PathfortmpFile = Utils.MPThumbsFolder;
    static List<String> SpectrumImages = new List<string>();
    static List<String> VUMeterImages = new List<string>();

    static SpectrumImage() { }

    private SpectrumImage() { }

    #region Images
    private static void AddSpectrumImage(string sTextureName)
    {
      if (SpectrumImages != null)
      {
        lock (SpectrumImages)
        {
          if (SpectrumImages.Count >= MaxSpectrumImages)
          {
            for (int i = 0; i < (SpectrumImages.Count - MaxSpectrumImages) + 1; i++)
            {
              Flush(SpectrumImages[i]);
            }
            SpectrumImages.RemoveRange(0, (SpectrumImages.Count - MaxSpectrumImages));
          }
          SpectrumImages.Add(sTextureName);
        }
      }
      else
      {
        SpectrumImages = new List<string>();
        SpectrumImages.Add(sTextureName);
      }
    }
    
    private static void AddVUMeterImage(string sTextureName)
    {
      if (VUMeterImages != null)
      {
        lock (VUMeterImages)
        {
          if (VUMeterImages.Count >= MaxSpectrumImages)
          {
            for (int i = 0; i < (VUMeterImages.Count - MaxSpectrumImages) + 1; i++)
            {
              Flush(VUMeterImages[i]);
            }
            VUMeterImages.RemoveRange(0, (VUMeterImages.Count - MaxSpectrumImages));
          }
          VUMeterImages.Add(sTextureName);
        }
      }
      else
      {
        VUMeterImages = new List<string>();
        VUMeterImages.Add(sTextureName);
      }
    }

    private static void Flush(string sTextureName)
    {
      if (Utils.SpectrumDebug)
      {
        logger.Debug("Images: Flush {0}", sTextureName);
      }
      GUITextureManager.ReleaseTexture(sTextureName);
    }

    public static void ClearSpectrumImages()
    {
      if (SpectrumImages != null)
      {
        lock (SpectrumImages)
        {
          logger.Debug("ClearSpectrumImages: Flush {0} images...", SpectrumImages.Count);
          foreach (String sTextureName in SpectrumImages)
          {
            Flush(sTextureName);
          }
          SpectrumImages.Clear();
          logger.Debug("ClearSpectrumImages: Flush done.");
        }
      }
      SpectrumImages = null;
      SpectrumImages = new List<string>();

      spectrumImage = null;
      spectrumPeak = null;
    }

    public static void ClearVUMeterImages()
    {
      if (VUMeterImages != null)
      {
        lock (VUMeterImages)
        {
          logger.Debug("ClearVUMeterImages: Flush {0} images...", VUMeterImages.Count);
          foreach (String sTextureName in VUMeterImages)
          {
            Flush(sTextureName);
          }
          VUMeterImages.Clear();
          logger.Debug("ClearVUMeterImages: Flush done.");
        }
      }
      VUMeterImages = null;
      VUMeterImages = new List<string>();

      vumeterImage = null;
      vumeterPeak = null;
    }

    public static void ClearImages()
    {
      ClearSpectrumImages();
      ClearVUMeterImages();
    }
    #endregion

    #region Spectrum
    public static void InitSpectrumImages()
    {
      if (spectrumImage == null && Utils.SpectrumEnabled && Utils.SpectrumImageFound)
      {
        string spectrumImageFile = GUIGraphicsContext.GetThemedSkinFile(Utils.SpectrumImage);
        spectrumImage = Utils.LoadImageFastFromFile(spectrumImageFile);
        bSpectrumVertical = spectrumImage.Height > spectrumImage.Width;
      }
      if (spectrumPeak == null && Utils.SpectrumPeakEnabled && Utils.SpectrumPeakImageFound)
      {
        string spectrumPeakImageFile = GUIGraphicsContext.GetThemedSkinFile(Utils.SpectrumPeakImage);
        spectrumPeak = Utils.LoadImageFastFromFile(spectrumPeakImageFile);
      }
    }

    public static string BuildConcatSpectrumImage(List<int> spectrum, List<int> spectrumpeak, int bars, int spacing, int min, int max)
    {
      if (spectrum == null)
      {
        return string.Empty;
      }

      if (!Utils.SpectrumImageFound)
      {
        return string.Empty;
      }

      try
      {
        if (spectrum.Count > 0)
        {
          string spectrumLine = string.Empty;
          string spectrumPeakLine = string.Empty;

          bool skeepPeak = !Utils.SpectrumPeakEnabled || !Utils.SpectrumPeakImageFound || (spectrum.Count != spectrumpeak.Count);

          for (int i = 0; i < spectrum.Count; i++)
          {
            spectrumLine += spectrum[i].ToString("000");
            if (!skeepPeak)
            {
              spectrumPeakLine += spectrumpeak[i].ToString("000");
            }
          }
          // string sFile = @"skin\s" + spectrumLine + (!skeepPeak ? "p" + spectrumPeakLine : string.Empty);
          string sFile = @"skin\s" + spectrumLine.GetHashCode().ToString("x") + (!skeepPeak ? "p" + spectrumPeakLine.GetHashCode().ToString("x") : string.Empty);
          sFile = "[" + (Utils.SpectrumDebug ? "" : "NoLog:") + "SpectrumImage:" + sFile + "]";
          if (SpectrumImages.Contains(sFile) && GUITextureManager.LoadFromMemory(null, sFile, 0, 0, 0) > 0) // Name already exists in MP cache
          {
            return sFile;
          }
          else
          {
            return BuildSpectrumImage(spectrum, spectrumpeak, bars, spacing, min, max, sFile, bSpectrumVertical);
          }
        }
      }
      catch (Exception ex)
      {
        logger.Error("BuildConcatImage: The Logo Building Engine generated an error: " + ex.Message);
      }
      return string.Empty;
    }

    static string BuildSpectrumImage(List<int> spectrum, List<int> spectrumpeak, int bars, int spacing, int min, int max, string sFileName, bool Vertical)
    {
      // step zero: prepare images
      int cropSteps = (max - min) + 1;
      if (cropSteps <= 0)
      {
        return string.Empty;
      }

      InitSpectrumImages();
      if (spectrumImage == null)
      {
        return string.Empty;
      }

      // step one: get result sizes
      if (Vertical)
      {
        iSpectrumWidth = spectrumImage.Width * bars + spacing * (bars - 1); 
        iSpectrumHeight = spectrumImage.Height; 
      }
      else
      {
        iSpectrumWidth = spectrumImage.Width; 
        iSpectrumHeight = spectrumImage.Height * bars + spacing * (bars - 1); 
      }

      // step one: get all images
      bool skeepPeak = !Utils.SpectrumPeakEnabled || !Utils.SpectrumPeakImageFound || (spectrum.Count != spectrumpeak.Count) || (spectrumPeak == null);

      List<Image> imgs = new List<Image>();
      List<Image> imgspeak = new List<Image>();
      Image single = null;
      for (int i = 0; i < spectrum.Count; i++)
      {
        try
        {
          int cropWidth = Vertical ? 0 : (spectrumImage.Width / cropSteps) * spectrum[i];
          int cropHeight = Vertical ? (spectrumImage.Height / cropSteps) * spectrum[i] : 0;
          single = CropImage(spectrumImage, cropWidth, cropHeight, bSpectrumVertical)  ;
          if (single == null)
          {
            continue;
          }
        }
        catch (Exception)
        {
          logger.Error("Skip: Could not crop Image for  " + spectrum[i]);
          continue;
        }
        imgs.Add(single);

        if (!skeepPeak)
        {
          try
          {
            int barWidth = Vertical ? spectrumImage.Width : (spectrumImage.Width / cropSteps) * spectrumpeak[i];
            int barHeight = Vertical ? (spectrumImage.Height / cropSteps) * spectrumpeak[i] : spectrumImage.Height;
            single = MakePeakImage(spectrumPeak, barWidth, barHeight, bSpectrumVertical)  ;
            if (single == null)
            {
              continue;
            }
          }
          catch (Exception)
          {
            logger.Error("Skip: Could not makePeak Image for  " + spectrum[i] + " - " + spectrumpeak[i]);
            continue;
          }
          imgspeak.Add(single);
        }
      }

      // step two: finally draw them
      skeepPeak = imgs.Count != imgspeak.Count;
      Bitmap b = new Bitmap(iSpectrumWidth, iSpectrumHeight);
      Image img = b;
      Graphics g = Graphics.FromImage(img);
      try
      {
        int x_pos = 0;
        int y_pos = 0;
        for (int i = 0; i < imgs.Count; i++)
        {
          if (Vertical)
          {
            g.DrawImage(imgs[i], x_pos, iSpectrumHeight - imgs[i].Height, imgs[i].Width, imgs[i].Height);
            if (!skeepPeak)
            {
              g.DrawImage(imgspeak[i], x_pos, iSpectrumHeight - imgspeak[i].Height, imgspeak[i].Width, imgspeak[i].Height);
            }
            x_pos += imgs[i].Width + spacing;
          }
          else
          {
            g.DrawImage(imgs[i], iSpectrumWidth - imgs[i].Width, y_pos, imgs[i].Width, imgs[i].Height);
            if (!skeepPeak)
            {
              g.DrawImage(imgspeak[i], iSpectrumWidth - imgspeak[i].Width, y_pos, imgspeak[i].Width, imgspeak[i].Height);
            }
            y_pos += imgs[i].Height + spacing;
          }
        }
      }
      finally
      {
        g.Dispose();
      }

      // step three: build image in memory
      string name = sFileName;
      try
      {                
        // we don't have to try first, if name already exists mp will not do anything with the image
        GUITextureManager.LoadFromMemory(b, name, 0, iSpectrumWidth, iSpectrumHeight);

        if (!string.IsNullOrEmpty(name) && !SpectrumImages.Contains(name))
        {
          AddSpectrumImage(name);
        }
      }
      catch (Exception)
      {
        logger.Error("BuildImages: Unable to add to MP's Graphics memory: " + name);
        return string.Empty;
      }
      return name;
    }
    #endregion
    
    #region VU Meter
    public static void InitVUMeterImages()
    {
      if (vumeterImage == null && Utils.SpectrumVUMeterEnabled && Utils.VUMeterImageFound)
      {
        string vumeterImageFile = GUIGraphicsContext.GetThemedSkinFile(Utils.VUMeterImage);
        vumeterImage = Utils.LoadImageFastFromFile(vumeterImageFile);
        bVUMeterVertical = vumeterImage.Height > vumeterImage.Width;
      }
      if (vumeterPeak == null && Utils.SpectrumVUMeterEnabled && Utils.VUMeterPeakImageFound)
      {
        string vumeterPeakImageFile = GUIGraphicsContext.GetThemedSkinFile(Utils.VUMeterPeakImage);
        vumeterPeak = Utils.LoadImageFastFromFile(vumeterPeakImageFile);
      }
    }

    public static string BuildConcatVUMeterImage(List<int> vumeter, List<int> vumeterpeak, int spacing, int min, int max)
    {
      if (!Utils.VUMeterImageFound)
      {
        return string.Empty;
      }

      try
      {
        if (vumeter.Count > 0)
        {
          string vumeterLine = string.Empty;
          string vumeterPeakLine = string.Empty;

          bool skeepPeak = !Utils.VUMeterPeakImageFound || (vumeter.Count != vumeterpeak.Count);

          for (int i = 0; i < vumeter.Count; i++)
          {
            vumeterLine += vumeter[i].ToString("000");
            if (!skeepPeak)
            {
              vumeterPeakLine += vumeterpeak[i].ToString("000");
            }
          }
          string sFile = @"skin\s" + vumeterLine.GetHashCode().ToString("x") + (!skeepPeak ? "p" + vumeterPeakLine.GetHashCode().ToString("x") : string.Empty);
          sFile = "[" + (Utils.SpectrumDebug ? "" : "NoLog:") + "VUMeterImage:" + sFile + "]";
          if (VUMeterImages.Contains(sFile) && GUITextureManager.LoadFromMemory(null, sFile, 0, 0, 0) > 0) // Name already exists in MP cache
          {
            return sFile;
          }
          else
          {
            return BuildVUMeterImage(vumeter, vumeterpeak, 2, spacing, min, max, sFile, bVUMeterVertical);
          }
        }
      }
      catch (Exception ex)
      {
        logger.Error("BuildConcatImage: The Logo Building Engine generated an error: " + ex.Message);
      }
      return string.Empty;
    }

    static string BuildVUMeterImage(List<int> vumeter, List<int> vumeterpeak, int bars, int spacing, int min, int max, string sFileName, bool Vertical)
    {
      // step zero: prepare images
      int cropSteps = (max - min) + 1;
      if (cropSteps <= 0)
      {
        return string.Empty;
      }

      InitVUMeterImages();
      if (vumeterImage == null)
      {
        return string.Empty;
      }

      // step one: get result sizes
      if (Vertical)
      {
        iVUMeterWidth = vumeterImage.Width * bars + spacing * (bars - 1); 
        iVUMeterHeight = vumeterImage.Height; 
      }
      else
      {
        iVUMeterWidth = vumeterImage.Width; 
        iVUMeterHeight = vumeterImage.Height * bars + spacing * (bars - 1); 
      }

      // step one: get all images
      bool skeepPeak = !Utils.VUMeterPeakImageFound || (vumeter.Count != vumeterpeak.Count) || (vumeterPeak == null);

      List<Image> imgs = new List<Image>();
      List<Image> imgspeak = new List<Image>();
      Image single = null;
      for (int i = 0; i < vumeter.Count; i++)
      {
        try
        {
          int cropWidth = Vertical ? 0 : (vumeterImage.Width / cropSteps) * vumeter[i];
          int cropHeight = Vertical ? (vumeterImage.Height / cropSteps) * vumeter[i] : 0;
          single = CropImage(vumeterImage, cropWidth, cropHeight, bVUMeterVertical)  ;
          if (single == null)
          {
            continue;
          }
        }
        catch (Exception)
        {
          logger.Error("Skip: Could not crop Image for  " + vumeter[i]);
          continue;
        }
        imgs.Add(single);

        if (!skeepPeak)
        {
          try
          {
            int barWidth = Vertical ? vumeterImage.Width : (vumeterImage.Width / cropSteps) * vumeterpeak[i];
            int barHeight = Vertical ? (vumeterImage.Height / cropSteps) * vumeterpeak[i] : vumeterImage.Height;
            single = MakePeakImage(vumeterPeak, barWidth, barHeight, bVUMeterVertical)  ;
            if (single == null)
            {
              continue;
            }
          }
          catch (Exception)
          {
            logger.Error("Skip: Could not makePeak Image for  " + vumeter[i] + " - " + vumeterpeak[i]);
            continue;
          }
          imgspeak.Add(single);
        }
      }

      // step two: finally draw them
      skeepPeak = imgs.Count != imgspeak.Count;
      Bitmap b = new Bitmap(iVUMeterWidth, iVUMeterHeight);
      Image img = b;
      Graphics g = Graphics.FromImage(img);
      try
      {
        int x_pos = 0;
        int y_pos = 0;
        for (int i = 0; i < imgs.Count; i++)
        {
          if (Vertical)
          {
            g.DrawImage(imgs[i], x_pos, iVUMeterHeight - imgs[i].Height, imgs[i].Width, imgs[i].Height);
            if (!skeepPeak)
            {
              g.DrawImage(imgspeak[i], x_pos, iVUMeterHeight - imgspeak[i].Height, imgspeak[i].Width, imgspeak[i].Height);
            }
            x_pos += imgs[i].Width + spacing;
          }
          else
          {
            g.DrawImage(imgs[i], iVUMeterWidth - imgs[i].Width, y_pos, imgs[i].Width, imgs[i].Height);
            if (!skeepPeak)
            {
              g.DrawImage(imgspeak[i], iVUMeterWidth - imgspeak[i].Width, y_pos, imgspeak[i].Width, imgspeak[i].Height);
            }
            y_pos += imgs[i].Height + spacing;
          }
        }
      }
      finally
      {
        g.Dispose();
      }

      // step three: build image in memory
      string name = sFileName;
      try
      {                
        // we don't have to try first, if name already exists mp will not do anything with the image
        GUITextureManager.LoadFromMemory(b, name, 0, iVUMeterWidth, iVUMeterHeight);

        if (!string.IsNullOrEmpty(name) && !VUMeterImages.Contains(name))
        {
          AddVUMeterImage(name);
        }
      }
      catch (Exception)
      {
        logger.Error("BuildImages: Unable to add to MP's Graphics memory: " + name);
        return string.Empty;
      }
      return name;
    }
    #endregion

    static Image CropImage(Image single, int iCropWidth, int iCropHeight, bool Vertical)
    {
      Rectangle cropRect;
      if (Vertical)
      {
        cropRect = new Rectangle(0, Math.Max(0, single.Height-iCropHeight), single.Width, Math.Min(single.Height, iCropHeight));
      }
      else
      {
        cropRect = new Rectangle(Math.Max(0, single.Width-iCropWidth), 0, Math.Min(single.Width, iCropWidth), single.Height);
      }
      Rectangle targRect = new Rectangle(0, 0, cropRect.Width, cropRect.Height);

      Bitmap src = single as Bitmap;
      Bitmap target = new Bitmap(targRect.Width, targRect.Height);

      target.SetResolution(single.HorizontalResolution, single.VerticalResolution);

      using(Graphics g = Graphics.FromImage(target))
      {
        /*
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        */
        g.DrawImage(src, targRect, cropRect, GraphicsUnit.Pixel);
      }
      return target;
    }

    static Image MakePeakImage(Image single, int barWidth, int barHeight, bool Vertical)
    {
      Rectangle srcRect = new Rectangle(0, 0, single.Width, single.Height);
      Rectangle trgRect = new Rectangle(0, 0, Vertical ? barWidth : single.Width, Vertical ? single.Height : barHeight);

      Bitmap src = single as Bitmap;

      Bitmap target = new Bitmap(barWidth, barHeight);
      target.SetResolution(single.HorizontalResolution, single.VerticalResolution);

      using(Graphics g = Graphics.FromImage(target))
      {
        /*
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        */
        g.DrawImage(src, trgRect, srcRect, GraphicsUnit.Pixel);
      }
      return target;
    }
  }
}