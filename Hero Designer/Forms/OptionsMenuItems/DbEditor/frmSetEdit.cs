
using Base.Data_Classes;
using Base.Display;
using Base.Master_Classes;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Hero_Designer
{
  public class frmSetEdit : Form
  {
        Button btnCancel;

        Button btnImage;

        Button btnNoImage;

        Button btnOK;

        Button btnPaste;

        ComboBox cbSetType;

        ComboBox cbSlotCount;
        ColumnHeader ColumnHeader1;
        ColumnHeader ColumnHeader2;
        ColumnHeader ColumnHeader3;
        ColumnHeader ColumnHeader4;
        GroupBox gbBasic;
        GroupBox GroupBox2;
        GroupBox GroupBox3;
        ImageList ilEnh;
        OpenFileDialog ImagePicker;
        Label Label1;
        Label Label16;
        Label Label2;
        Label Label27;
        Label Label3;
        Label Label4;
        Label Label5;
        Label Label6;
        Label Label7;

        ListBox lstBonus;

    [AccessedThroughProperty("lvBonusList")]
    ListView _lvBonusList;
        ListView lvEnh;
        RadioButton rbIfAny;
        RadioButton rbIfCritter;
        RadioButton rbIfPlayer;
        RichTextBox rtbBonus;

        TextBox txtAlternate;

        TextBox txtDesc;

        TextBox txtInternal;

        TextBox txtNameFull;

        TextBox txtNameShort;

    [AccessedThroughProperty("udMaxLevel")]
    NumericUpDown _udMaxLevel;

    [AccessedThroughProperty("udMinLevel")]
    NumericUpDown _udMinLevel;

    IContainer components;

    protected bool Loading;
    public EnhancementSet mySet;
    protected int[] SetBonusList;
    protected int[] SetBonusListPVP;

















    ListView lvBonusList
    {
      get
      {
        return this._lvBonusList;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] set
      {
        EventHandler eventHandler1 = new EventHandler(this.lvBonusList_SelectedIndexChanged);
        EventHandler eventHandler2 = new EventHandler(this.lvBonusList_DoubleClick);
        if (this._lvBonusList != null)
        {
          this._lvBonusList.SelectedIndexChanged -= eventHandler1;
          this._lvBonusList.DoubleClick -= eventHandler2;
        }
        this._lvBonusList = value;
        if (this._lvBonusList == null)
          return;
        this._lvBonusList.SelectedIndexChanged += eventHandler1;
        this._lvBonusList.DoubleClick += eventHandler2;
      }
    }





    NumericUpDown udMaxLevel
    {
      get
      {
        return this._udMaxLevel;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] set
      {
        EventHandler eventHandler1 = new EventHandler(this.udMaxLevel_Leave);
        EventHandler eventHandler2 = new EventHandler(this.udMaxLevel_ValueChanged);
        if (this._udMaxLevel != null)
        {
          this._udMaxLevel.Leave -= eventHandler1;
          this._udMaxLevel.ValueChanged -= eventHandler2;
        }
        this._udMaxLevel = value;
        if (this._udMaxLevel == null)
          return;
        this._udMaxLevel.Leave += eventHandler1;
        this._udMaxLevel.ValueChanged += eventHandler2;
      }
    }

    NumericUpDown udMinLevel
    {
      get
      {
        return this._udMinLevel;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] set
      {
        EventHandler eventHandler1 = new EventHandler(this.udMinLevel_Leave);
        EventHandler eventHandler2 = new EventHandler(this.udMinLevel_ValueChanged);
        if (this._udMinLevel != null)
        {
          this._udMinLevel.Leave -= eventHandler1;
          this._udMinLevel.ValueChanged -= eventHandler2;
        }
        this._udMinLevel = value;
        if (this._udMinLevel == null)
          return;
        this._udMinLevel.Leave += eventHandler1;
        this._udMinLevel.ValueChanged += eventHandler2;
      }
    }

    public frmSetEdit(ref EnhancementSet iSet)
    {
      this.Load += new EventHandler(this.frmSetEdit_Load);
      this.SetBonusList = new int[0];
      this.SetBonusListPVP = new int[0];
      this.Loading = true;
      this.InitializeComponent();
      this.mySet = new EnhancementSet(iSet);
    }

    public int BonusID()
    {
      return this.cbSlotCount.SelectedIndex;
    }

    void btnCancel_Click(object sender, EventArgs e)

    {
      this.DialogResult = DialogResult.Cancel;
      this.Hide();
    }

    void btnImage_Click(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.ImagePicker.InitialDirectory = I9Gfx.GetEnhancementsPath();
      this.ImagePicker.FileName = this.mySet.Image;
      if (this.ImagePicker.ShowDialog() == DialogResult.OK)
      {
        string str = FileIO.StripPath(this.ImagePicker.FileName);
        if (!File.Exists(FileIO.AddSlash(this.ImagePicker.InitialDirectory) + str))
        {
          int num = (int) Interaction.MsgBox((object) ("You must select an image from the " + I9Gfx.GetEnhancementsPath() + " folder!\r\n\r\nIf you are adding a new image, you should copy it to the folder and then select it."), MsgBoxStyle.Information, (object) "Ah...");
        }
        else
        {
          this.mySet.Image = str;
          this.DisplayIcon();
        }
      }
    }

    void btnNoImage_Click(object sender, EventArgs e)

    {
      this.mySet.Image = "";
      this.DisplayIcon();
    }

    void btnOK_Click(object sender, EventArgs e)

    {
      this.mySet.LevelMin = Convert.ToInt32(Decimal.Subtract(this.udMinLevel.Value, new Decimal(1)));
      this.mySet.LevelMax = Convert.ToInt32(Decimal.Subtract(this.udMaxLevel.Value, new Decimal(1)));
      this.DialogResult = DialogResult.OK;
      this.Hide();
    }

    void btnPaste_Click(object sender, EventArgs e)

    {
      string str = Conversions.ToString(Clipboard.GetData("System.String"));
      char[] chArray = new char[1]{ '^' };
      string[] strArray1 = str.Replace("\r\n", Conversions.ToString(chArray[0])).Split(chArray);
      chArray[0] = '\t';
      this.mySet.InitBonus();
      int num1 = strArray1.Length - 1;
      for (int index1 = 0; index1 <= num1; ++index1)
      {
        string[] strArray2 = strArray1[index1].Split(chArray);
        if (strArray2.Length > 3)
        {
          int num2 = (int) Math.Round(Conversion.Val(strArray2[0]));
          int index2 = DatabaseAPI.NidFromUidPower(strArray2[3]);
          int num3 = num2 - 2;
          if (num3 > -1 & index2 > -1)
          {
            EnhancementSet.BonusItem[] bonus = this.mySet.Bonus;
            int index3 = num3;
            bonus[index3].Name = (string[]) Utils.CopyArray((Array) bonus[index3].Name, (Array) new string[bonus[index3].Name.Length + 1]);
            bonus[index3].Index = (int[]) Utils.CopyArray((Array) bonus[index3].Index, (Array) new int[bonus[index3].Index.Length + 1]);
            bonus[index3].Index[bonus[index3].Index.Length - 1] = index2;
            bonus[index3].Name[bonus[index3].Name.Length - 1] = DatabaseAPI.Database.Power[index2].FullName;
          }
        }
      }
      this.DisplayBonus();
      this.DisplayBonusText();
    }

    void cbSetType_SelectedIndexChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.mySet.SetType = (Enums.eSetType) this.cbSetType.SelectedIndex;
    }

    void cbSlotX_SelectedIndexChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.DisplayBonus();
      this.DisplayBonusText();
    }

    public void DisplayBonus()
    {
      try
      {
        this.lstBonus.BeginUpdate();
        this.lstBonus.Items.Clear();
        if (this.isBonus())
        {
          int index1 = this.BonusID();
          int num = this.mySet.Bonus[index1].Index.Length - 1;
          for (int index2 = 0; index2 <= num; ++index2)
            this.lstBonus.Items.Add((object) DatabaseAPI.Database.Power[this.mySet.Bonus[index1].Index[index2]].PowerName);
          this.txtAlternate.Text = this.mySet.Bonus[index1].AltString;
        }
        else if (this.isSpecial())
        {
          int index1 = this.SpecialID();
          int num = this.mySet.SpecialBonus[index1].Index.Length - 1;
          for (int index2 = 0; index2 <= num; ++index2)
            this.lstBonus.Items.Add((object) DatabaseAPI.Database.Power[this.mySet.SpecialBonus[index1].Index[index2]].PowerName);
          this.txtAlternate.Text = this.mySet.SpecialBonus[index1].AltString;
        }
        this.lstBonus.EndUpdate();
        this.cbSlotCount.Enabled = this.mySet.Enhancements.Length > 1;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    public void DisplayBonusText()
    {
      string str1 = RTF.StartRTF();
      int num1 = this.mySet.Bonus.Length - 1;
      for (int index1 = 0; index1 <= num1; ++index1)
      {
        if (this.mySet.Bonus[index1].Index.Length > 0)
          str1 = str1 + RTF.Color(RTF.ElementID.Black) + RTF.Bold(Conversions.ToString(this.mySet.Bonus[index1].Slotted) + " Enhancements: ");
        int num2 = this.mySet.Bonus[index1].Index.Length - 1;
        for (int index2 = 0; index2 <= num2; ++index2)
        {
          if (this.mySet.Bonus[index1].Index[index2] > -1)
          {
            if (index2 > 0)
              str1 += ", ";
            str1 = str1 + RTF.Color(RTF.ElementID.InentionInvert) + DatabaseAPI.Database.Power[this.mySet.Bonus[index1].Index[index2]].PowerName;
          }
        }
        if (this.mySet.Bonus[index1].Index.Length > 0)
          str1 = str1 + RTF.Crlf() + "   " + RTF.Italic(this.mySet.GetEffectString(index1, false, false));
        if (this.mySet.Bonus[index1].PvMode == Enums.ePvX.PvP)
          str1 += "(PvP)";
        if (this.mySet.Bonus[index1].Index.Length > 0)
          str1 += RTF.Crlf();
      }
      int num3 = this.mySet.SpecialBonus.Length - 1;
      for (int index1 = 0; index1 <= num3; ++index1)
      {
        if (this.mySet.SpecialBonus[index1].Special > -1)
        {
          string str2 = str1 + RTF.Color(RTF.ElementID.Black) + RTF.Bold("Special Case Enhancement: ") + RTF.Color(RTF.ElementID.InentionInvert);
          if (this.mySet.Enhancements[this.mySet.SpecialBonus[index1].Special] > -1)
            str2 += DatabaseAPI.Database.Enhancements[this.mySet.Enhancements[this.mySet.SpecialBonus[index1].Special]].Name;
          string str3 = str2 + RTF.Crlf();
          int num2 = this.mySet.SpecialBonus[index1].Index.Length - 1;
          for (int index2 = 0; index2 <= num2; ++index2)
          {
            if (this.mySet.SpecialBonus[index1].Index[index2] > -1)
            {
              if (index2 > 0)
                str3 += ", ";
              str3 = str3 + RTF.Color(RTF.ElementID.InentionInvert) + DatabaseAPI.Database.Power[this.mySet.SpecialBonus[index1].Index[index2]].PowerName;
            }
          }
          str1 = str3 + RTF.Crlf() + "   " + RTF.Italic(this.mySet.GetEffectString(index1, true, false)) + RTF.Crlf();
        }
        if (this.mySet.SpecialBonus[index1].Index.Length > 0)
          str1 += RTF.Crlf();
      }
      this.rtbBonus.Rtf = str1 + RTF.EndRTF();
    }

    void DisplayIcon()

    {
      if (this.mySet.Image != "")
      {
        ExtendedBitmap extendedBitmap1 = new ExtendedBitmap(I9Gfx.GetEnhancementsPath() + this.mySet.Image);
        ExtendedBitmap extendedBitmap2 = new ExtendedBitmap(30, 30);
        extendedBitmap2.Graphics.DrawImage((Image) I9Gfx.Borders.Bitmap, extendedBitmap2.ClipRect, I9Gfx.GetOverlayRect(Origin.Grade.SetO), GraphicsUnit.Pixel);
        extendedBitmap2.Graphics.DrawImage((Image) extendedBitmap1.Bitmap, extendedBitmap2.ClipRect, extendedBitmap2.ClipRect, GraphicsUnit.Pixel);
        this.btnImage.Image = (Image) new Bitmap((Image) extendedBitmap2.Bitmap);
        this.btnImage.Text = this.mySet.Image;
      }
      else
      {
        ExtendedBitmap extendedBitmap = new ExtendedBitmap(30, 30);
        extendedBitmap.Graphics.DrawImage((Image) I9Gfx.Borders.Bitmap, extendedBitmap.ClipRect, I9Gfx.GetOverlayRect(Origin.Grade.SetO), GraphicsUnit.Pixel);
        this.btnImage.Image = (Image) new Bitmap((Image) extendedBitmap.Bitmap);
        this.btnImage.Text = "Select Image";
      }
    }

    public void DisplaySetData()
    {
      this.DisplaySetIcons();
      this.DisplayIcon();
      this.txtNameFull.Text = this.mySet.DisplayName;
      this.txtNameShort.Text = this.mySet.ShortName;
      this.txtDesc.Text = this.mySet.Desc;
      this.txtInternal.Text = this.mySet.Uid;
      this.SetMinLevel(this.mySet.LevelMin + 1);
      this.SetMaxLevel(this.mySet.LevelMax + 1);
      this.udMaxLevel.Minimum = this.udMinLevel.Value;
      this.udMinLevel.Maximum = this.udMaxLevel.Value;
      this.cbSetType.SelectedIndex = (int) this.mySet.SetType;
      this.btnImage.Text = this.mySet.Image;
      this.DisplayBonusText();
      this.DisplayBonus();
    }

    public void DisplaySetIcons()
    {
      this.FillImageList();
      string[] items = new string[2];
      this.lvEnh.BeginUpdate();
      this.lvEnh.Items.Clear();
      this.FillImageList();
      int num1 = this.mySet.Enhancements.Length - 1;
      for (int imageIndex = 0; imageIndex <= num1; ++imageIndex)
      {
        IEnhancement enhancement = DatabaseAPI.Database.Enhancements[this.mySet.Enhancements[imageIndex]];
        items[0] = enhancement.Name + " (" + enhancement.ShortName + ")";
        items[1] = "";
        int num2 = enhancement.ClassID.Length - 1;
        for (int index1 = 0; index1 <= num2; ++index1)
        {
          if (items[1] != "")
          {
            string[] strArray1 = items;
            int num3 = 1;
            string[] strArray2;
            IntPtr index2;
            (strArray2 = strArray1)[(int) (index2 = (IntPtr) num3)] = strArray2[(int)index2] + ",";
          }
          string[] strArray3 = items;
          int num4 = 1;
          string[] strArray4;
          IntPtr index3;
          (strArray4 = strArray3)[(int) (index3 = (IntPtr) num4)] = strArray4[(int)index3] + DatabaseAPI.Database.EnhancementClasses[enhancement.ClassID[index1]].ShortName;
        }
        this.lvEnh.Items.Add(new ListViewItem(items, imageIndex));
      }
      this.lvEnh.EndUpdate();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public void FillBonusCombos()
    {
      this.cbSlotCount.BeginUpdate();
      this.cbSlotCount.Items.Clear();
      int num1 = this.mySet.Enhancements.Length - 2;
      for (int index = 0; index <= num1; ++index)
        this.cbSlotCount.Items.Add((object) (Conversions.ToString(index + 2) + " Enhancements"));
      int num2 = this.mySet.Enhancements.Length - 1;
      for (int index = 0; index <= num2; ++index)
        this.cbSlotCount.Items.Add((object) DatabaseAPI.Database.Enhancements[this.mySet.Enhancements[index]].Name);
      if (this.cbSlotCount.Items.Count > 0)
        this.cbSlotCount.SelectedIndex = 0;
      this.cbSlotCount.EndUpdate();
    }

    public void FillBonusList()
    {
      this.lvBonusList.BeginUpdate();
      this.lvBonusList.Items.Clear();
      string[] items = new string[2];
      int num1 = this.SetBonusList.Length - 1;
      for (int index = 0; index <= num1; ++index)
      {
        items[1] = "";
        if (DatabaseAPI.Database.Power[this.SetBonusList[index]].Effects.Length > 0)
          items[1] = DatabaseAPI.Database.Power[this.SetBonusList[index]].Effects[0].BuildEffectStringShort(false, true, false);
        items[0] = DatabaseAPI.Database.Power[this.SetBonusList[index]].PowerName;
        this.lvBonusList.Items.Add(new ListViewItem(items)
        {
          Tag = (object) this.SetBonusList[index]
        });
      }
      int num2 = this.SetBonusListPVP.Length - 1;
      for (int index = 0; index <= num2; ++index)
      {
        items[1] = "";
        if (DatabaseAPI.Database.Power[this.SetBonusListPVP[index]].Effects.Length > 0)
          items[1] = DatabaseAPI.Database.Power[this.SetBonusListPVP[index]].Effects[0].BuildEffectStringShort(false, true, false);
        items[0] = DatabaseAPI.Database.Power[this.SetBonusListPVP[index]].PowerName + " (PVP Only)";
        this.lvBonusList.Items.Add(new ListViewItem(items)
        {
          Tag = (object) this.SetBonusListPVP[index]
        });
      }
      this.lvBonusList.Sort();
      this.lvBonusList.EndUpdate();
    }

    public void FillComboBoxes()
    {
      string[] names = Enum.GetNames(Enums.eSetType.Untyped.GetType());
      this.cbSetType.BeginUpdate();
      this.cbSetType.Items.Clear();
      this.cbSetType.Items.AddRange((object[]) names);
      this.cbSetType.EndUpdate();
    }

    public void FillImageList()
    {
      Size imageSize1 = this.ilEnh.ImageSize;
      int width1 = imageSize1.Width;
      imageSize1 = this.ilEnh.ImageSize;
      int height1 = imageSize1.Height;
      ExtendedBitmap extendedBitmap = new ExtendedBitmap(width1, height1);
      this.ilEnh.Images.Clear();
      int num = this.mySet.Enhancements.Length - 1;
      for (int index = 0; index <= num; ++index)
      {
        IEnhancement enhancement = DatabaseAPI.Database.Enhancements[this.mySet.Enhancements[index]];
        if (enhancement.ImageIdx > -1)
        {
          Origin.Grade gfxGrade = I9Gfx.ToGfxGrade(enhancement.TypeID);
          extendedBitmap.Graphics.Clear(Color.White);
          Graphics graphics = extendedBitmap.Graphics;
          I9Gfx.DrawEnhancement(ref graphics, DatabaseAPI.Database.Enhancements[this.mySet.Enhancements[index]].ImageIdx, gfxGrade);
          this.ilEnh.Images.Add((Image) extendedBitmap.Bitmap);
        }
        else
        {
          ImageList.ImageCollection images = this.ilEnh.Images;
          Size imageSize2 = this.ilEnh.ImageSize;
          int width2 = imageSize2.Width;
          imageSize2 = this.ilEnh.ImageSize;
          int height2 = imageSize2.Height;
          Bitmap bitmap = new Bitmap(width2, height2);
          images.Add((Image) bitmap);
        }
      }
    }

    void frmSetEdit_Load(object sender, EventArgs e)

    {
      if (MidsContext.Config.MasterMode)
        this.btnPaste.Visible = true;
      this.SetBonusList = DatabaseAPI.NidPowers("set_bonus.set_bonus", "");
      this.SetBonusListPVP = DatabaseAPI.NidPowers("set_bonus.pvp_set_bonus", "");
      if (this.mySet.Bonus.Length < 1)
        this.mySet.InitBonus();
      this.FillComboBoxes();
      this.FillBonusCombos();
      this.FillBonusList();
      this.DisplaySetData();
      this.Loading = false;
      this.DisplayBonus();
    }

    [DebuggerStepThrough]
    void InitializeComponent()

    {
      this.components = (IContainer)new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (frmSetEdit));
      this.btnNoImage = new Button();
      this.gbBasic = new GroupBox();
      this.txtInternal = new TextBox();
      this.Label5 = new Label();
      this.cbSetType = new ComboBox();
      this.Label1 = new Label();
      this.Label7 = new Label();
      this.Label6 = new Label();
      this.udMinLevel = new NumericUpDown();
      this.udMaxLevel = new NumericUpDown();
      this.txtDesc = new TextBox();
      this.Label4 = new Label();
      this.txtNameShort = new TextBox();
      this.Label3 = new Label();
      this.txtNameFull = new TextBox();
      this.Label2 = new Label();
      this.btnImage = new Button();
      this.GroupBox2 = new GroupBox();
      this.lvEnh = new ListView();
      this.ColumnHeader1 = new ColumnHeader();
      this.ColumnHeader2 = new ColumnHeader();
      this.ilEnh = new ImageList(this.components);
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.ImagePicker = new OpenFileDialog();
      this.GroupBox3 = new GroupBox();
      this.btnPaste = new Button();
      this.txtAlternate = new TextBox();
      this.lvBonusList = new ListView();
      this.ColumnHeader3 = new ColumnHeader();
      this.ColumnHeader4 = new ColumnHeader();
      this.lstBonus = new ListBox();
      this.Label16 = new Label();
      this.cbSlotCount = new ComboBox();
      this.rtbBonus = new RichTextBox();
      this.Label27 = new Label();
      this.rbIfPlayer = new RadioButton();
      this.rbIfCritter = new RadioButton();
      this.rbIfAny = new RadioButton();
      this.gbBasic.SuspendLayout();
      this.udMinLevel.BeginInit();
      this.udMaxLevel.BeginInit();
      this.GroupBox2.SuspendLayout();
      this.GroupBox3.SuspendLayout();
      this.SuspendLayout();

      this.btnNoImage.Location = new Point(8, 84);
      this.btnNoImage.Name = "btnNoImage";

      this.btnNoImage.Size = new Size(80, 20);
      this.btnNoImage.TabIndex = 2;
      this.btnNoImage.Text = "Clear Image";
      this.gbBasic.Controls.Add((Control) this.txtInternal);
      this.gbBasic.Controls.Add((Control) this.Label5);
      this.gbBasic.Controls.Add((Control) this.cbSetType);
      this.gbBasic.Controls.Add((Control) this.Label1);
      this.gbBasic.Controls.Add((Control) this.Label7);
      this.gbBasic.Controls.Add((Control) this.Label6);
      this.gbBasic.Controls.Add((Control) this.udMinLevel);
      this.gbBasic.Controls.Add((Control) this.udMaxLevel);
      this.gbBasic.Controls.Add((Control) this.txtDesc);
      this.gbBasic.Controls.Add((Control) this.Label4);
      this.gbBasic.Controls.Add((Control) this.txtNameShort);
      this.gbBasic.Controls.Add((Control) this.Label3);
      this.gbBasic.Controls.Add((Control) this.txtNameFull);
      this.gbBasic.Controls.Add((Control) this.Label2);

      this.gbBasic.Location = new Point(96, 8);
      this.gbBasic.Name = "gbBasic";

      this.gbBasic.Size = new Size(248, 202);
      this.gbBasic.TabIndex = 0;
      this.gbBasic.TabStop = false;
      this.gbBasic.Text = "Basic:";

      this.txtInternal.Location = new Point(84, 66);
      this.txtInternal.Name = "txtInternal";

      this.txtInternal.Size = new Size(156, 20);
      this.txtInternal.TabIndex = 21;

      this.Label5.Location = new Point(8, 66);
      this.Label5.Name = "Label5";

      this.Label5.Size = new Size(72, 20);
      this.Label5.TabIndex = 22;
      this.Label5.Text = "Internal:";
      this.Label5.TextAlign = ContentAlignment.MiddleRight;
      this.cbSetType.DropDownStyle = ComboBoxStyle.DropDownList;

      this.cbSetType.Location = new Point(84, 174);
      this.cbSetType.Name = "cbSetType";

      this.cbSetType.Size = new Size(156, 22);
      this.cbSetType.TabIndex = 3;

      this.Label1.Location = new Point(8, 174);
      this.Label1.Name = "Label1";

      this.Label1.Size = new Size(72, 20);
      this.Label1.TabIndex = 20;
      this.Label1.Text = "SetType:";
      this.Label1.TextAlign = ContentAlignment.MiddleRight;

      this.Label7.Location = new Point(8, 150);
      this.Label7.Name = "Label7";

      this.Label7.Size = new Size(52, 20);
      this.Label7.TabIndex = 19;
      this.Label7.Text = "MaxLev:";
      this.Label7.TextAlign = ContentAlignment.MiddleRight;

      this.Label6.Location = new Point(8, 126);
      this.Label6.Name = "Label6";

      this.Label6.Size = new Size(52, 20);
      this.Label6.TabIndex = 18;
      this.Label6.Text = "MinLev:";
      this.Label6.TextAlign = ContentAlignment.MiddleRight;

      this.udMinLevel.Location = new Point(64, 126);
      Decimal num = new Decimal(new int[4]
      {
        53,
        0,
        0,
        0
      });
      this.udMinLevel.Maximum = num;
      num = new Decimal(new int[4]{ 1, 0, 0, 0 });
      this.udMinLevel.Minimum = num;
      this.udMinLevel.Name = "udMinLevel";

      this.udMinLevel.Size = new Size(36, 20);
      this.udMinLevel.TabIndex = 4;
      num = new Decimal(new int[4]{ 1, 0, 0, 0 });
      this.udMinLevel.Value = num;

      this.udMaxLevel.Location = new Point(64, 150);
      num = new Decimal(new int[4]{ 53, 0, 0, 0 });
      this.udMaxLevel.Maximum = num;
      num = new Decimal(new int[4]{ 1, 0, 0, 0 });
      this.udMaxLevel.Minimum = num;
      this.udMaxLevel.Name = "udMaxLevel";

      this.udMaxLevel.Size = new Size(36, 20);
      this.udMaxLevel.TabIndex = 5;
      num = new Decimal(new int[4]{ 53, 0, 0, 0 });
      this.udMaxLevel.Value = num;

      this.txtDesc.Location = new Point(100, 102);
      this.txtDesc.Multiline = true;
      this.txtDesc.Name = "txtDesc";

      this.txtDesc.Size = new Size(140, 68);
      this.txtDesc.TabIndex = 2;

      this.Label4.Location = new Point(8, 102);
      this.Label4.Name = "Label4";

      this.Label4.Size = new Size(88, 20);
      this.Label4.TabIndex = 14;
      this.Label4.Text = "Description:";
      this.Label4.TextAlign = ContentAlignment.MiddleRight;

      this.txtNameShort.Location = new Point(84, 40);
      this.txtNameShort.Name = "txtNameShort";

      this.txtNameShort.Size = new Size(156, 20);
      this.txtNameShort.TabIndex = 1;

      this.Label3.Location = new Point(8, 40);
      this.Label3.Name = "Label3";

      this.Label3.Size = new Size(72, 20);
      this.Label3.TabIndex = 12;
      this.Label3.Text = "Short Name:";
      this.Label3.TextAlign = ContentAlignment.MiddleRight;

      this.txtNameFull.Location = new Point(84, 16);
      this.txtNameFull.Name = "txtNameFull";

      this.txtNameFull.Size = new Size(156, 20);
      this.txtNameFull.TabIndex = 0;

      this.Label2.Location = new Point(8, 16);
      this.Label2.Name = "Label2";

      this.Label2.Size = new Size(72, 20);
      this.Label2.TabIndex = 10;
      this.Label2.Text = "Full Name:";
      this.Label2.TextAlign = ContentAlignment.MiddleRight;
      this.btnImage.Image = (Image) componentResourceManager.GetObject("btnImage.Image");
      this.btnImage.ImageAlign = ContentAlignment.TopCenter;

      this.btnImage.Location = new Point(8, 12);
      this.btnImage.Name = "btnImage";

      this.btnImage.Size = new Size(80, 68);
      this.btnImage.TabIndex = 1;
      this.btnImage.Text = "ImageName";
      this.btnImage.TextAlign = ContentAlignment.BottomCenter;
      this.GroupBox2.Controls.Add((Control) this.lvEnh);

      this.GroupBox2.Location = new Point(6, 216);
      this.GroupBox2.Name = "GroupBox2";

      this.GroupBox2.Size = new Size(336, 224);
      this.GroupBox2.TabIndex = 24;
      this.GroupBox2.TabStop = false;
      this.GroupBox2.Text = "Enhancements belonging to this set:";
      this.lvEnh.Columns.AddRange(new ColumnHeader[2]
      {
        this.ColumnHeader1,
        this.ColumnHeader2
      });
      this.lvEnh.FullRowSelect = true;
      this.lvEnh.LargeImageList = this.ilEnh;

      this.lvEnh.Location = new Point(8, 20);
      this.lvEnh.Name = "lvEnh";

      this.lvEnh.Size = new Size(320, 196);
      this.lvEnh.SmallImageList = this.ilEnh;
      this.lvEnh.TabIndex = 0;
      this.lvEnh.UseCompatibleStateImageBehavior = false;
      this.lvEnh.View = View.Details;
      this.ColumnHeader1.Text = "Name";
      this.ColumnHeader1.Width = 203;
      this.ColumnHeader2.Text = "Classes";
      this.ColumnHeader2.Width = 91;
      this.ilEnh.ColorDepth = ColorDepth.Depth32Bit;

      this.ilEnh.ImageSize = new Size(30, 30);
      this.ilEnh.TransparentColor = Color.Transparent;
      this.btnCancel.DialogResult = DialogResult.Cancel;

      this.btnCancel.Location = new Point(160, 446);
      this.btnCancel.Name = "btnCancel";

      this.btnCancel.Size = new Size(84, 28);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnOK.DialogResult = DialogResult.OK;

      this.btnOK.Location = new Point(252, 446);
      this.btnOK.Name = "btnOK";

      this.btnOK.Size = new Size(84, 28);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.ImagePicker.Filter = "PNG Images|*.png";
      this.ImagePicker.Title = "Select Image File";
      this.GroupBox3.Controls.Add((Control) this.btnPaste);
      this.GroupBox3.Controls.Add((Control) this.txtAlternate);
      this.GroupBox3.Controls.Add((Control) this.lvBonusList);
      this.GroupBox3.Controls.Add((Control) this.lstBonus);
      this.GroupBox3.Controls.Add((Control) this.Label16);
      this.GroupBox3.Controls.Add((Control) this.cbSlotCount);
      this.GroupBox3.Controls.Add((Control) this.rtbBonus);

      this.GroupBox3.Location = new Point(348, 8);
      this.GroupBox3.Name = "GroupBox3";

      this.GroupBox3.Size = new Size(629, 432);
      this.GroupBox3.TabIndex = 25;
      this.GroupBox3.TabStop = false;
      this.GroupBox3.Text = "Set Bonuses:";

      this.btnPaste.Location = new Point(8, 280);
      this.btnPaste.Name = "btnPaste";

      this.btnPaste.Size = new Size(96, 23);
      this.btnPaste.TabIndex = 19;
      this.btnPaste.Text = "Fill by Paste";
      this.btnPaste.Visible = false;

      this.txtAlternate.Location = new Point(8, 406);
      this.txtAlternate.Name = "txtAlternate";

      this.txtAlternate.Size = new Size(270, 20);
      this.txtAlternate.TabIndex = 18;
      this.lvBonusList.Columns.AddRange(new ColumnHeader[2]
      {
        this.ColumnHeader3,
        this.ColumnHeader4
      });
      this.lvBonusList.FullRowSelect = true;
      this.lvBonusList.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.lvBonusList.HideSelection = false;

      this.lvBonusList.Location = new Point(284, 19);
      this.lvBonusList.MultiSelect = false;
      this.lvBonusList.Name = "lvBonusList";

      this.lvBonusList.Size = new Size(339, 407);
      this.lvBonusList.TabIndex = 17;
      this.lvBonusList.UseCompatibleStateImageBehavior = false;
      this.lvBonusList.View = View.Details;
      this.ColumnHeader3.Text = "Bonus";
      this.ColumnHeader3.Width = 185;
      this.ColumnHeader4.Text = "Effect";
      this.ColumnHeader4.Width = 110;
      this.lstBonus.ItemHeight = 14;

      this.lstBonus.Location = new Point(8, 309);
      this.lstBonus.Name = "lstBonus";

      this.lstBonus.Size = new Size(271, 88);
      this.lstBonus.TabIndex = 16;

      this.Label16.Location = new Point(8, 281);
      this.Label16.Name = "Label16";

      this.Label16.Size = new Size(96, 20);
      this.Label16.TabIndex = 15;
      this.Label16.Text = "Bonus for slotting:";
      this.Label16.TextAlign = ContentAlignment.MiddleRight;
      this.cbSlotCount.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbSlotCount.Items.AddRange(new object[5]
      {
        (object) "2",
        (object) "3",
        (object) "4",
        (object) "5",
        (object) "6"
      });

      this.cbSlotCount.Location = new Point(110, 281);
      this.cbSlotCount.Name = "cbSlotCount";

      this.cbSlotCount.Size = new Size(170, 22);
      this.cbSlotCount.TabIndex = 3;
      this.rtbBonus.BackColor = Color.White;
      this.rtbBonus.ForeColor = Color.Black;

      this.rtbBonus.Location = new Point(6, 19);
      this.rtbBonus.Name = "rtbBonus";
      this.rtbBonus.ReadOnly = true;
      this.rtbBonus.ScrollBars = RichTextBoxScrollBars.Vertical;

      this.rtbBonus.Size = new Size(272, 256);
      this.rtbBonus.TabIndex = 1;
      this.rtbBonus.Text = "";

      this.Label27.Location = new Point(351, 450);
      this.Label27.Name = "Label27";

      this.Label27.Size = new Size(76, 20);
      this.Label27.TabIndex = 106;
      this.Label27.Text = "If Target =";
      this.Label27.TextAlign = ContentAlignment.MiddleRight;

      this.rbIfPlayer.Location = new Point(563, 451);
      this.rbIfPlayer.Name = "rbIfPlayer";

      this.rbIfPlayer.Size = new Size(76, 20);
      this.rbIfPlayer.TabIndex = 105;
      this.rbIfPlayer.Text = "Players";

      this.rbIfCritter.Location = new Point(486, 451);
      this.rbIfCritter.Name = "rbIfCritter";

      this.rbIfCritter.Size = new Size(71, 20);
      this.rbIfCritter.TabIndex = 104;
      this.rbIfCritter.Text = "Critters";
      this.rbIfAny.Checked = true;

      this.rbIfAny.Location = new Point(433, 451);
      this.rbIfAny.Name = "rbIfAny";

      this.rbIfAny.Size = new Size(57, 20);
      this.rbIfAny.TabIndex = 103;
      this.rbIfAny.TabStop = true;
      this.rbIfAny.Text = "Any";
      this.AcceptButton = (IButtonControl) this.btnOK;

      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;

      this.ClientSize = new Size(989, 482);
      this.Controls.Add((Control) this.Label27);
      this.Controls.Add((Control) this.rbIfPlayer);
      this.Controls.Add((Control) this.rbIfCritter);
      this.Controls.Add((Control) this.rbIfAny);
      this.Controls.Add((Control) this.GroupBox3);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.GroupBox2);
      this.Controls.Add((Control) this.btnNoImage);
      this.Controls.Add((Control) this.gbBasic);
      this.Controls.Add((Control) this.btnImage);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (frmSetEdit);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Invention Set Editor";
      this.gbBasic.ResumeLayout(false);
      this.gbBasic.PerformLayout();
      this.udMinLevel.EndInit();
      this.udMaxLevel.EndInit();
      this.GroupBox2.ResumeLayout(false);
      this.GroupBox3.ResumeLayout(false);
      this.GroupBox3.PerformLayout();
              //adding events
              if(!System.Diagnostics.Debugger.IsAttached || !this.IsInDesignMode() || !System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLowerInvariant().Contains("devenv"))
              {
                  this.btnCancel.Click += btnCancel_Click;
                  this.btnImage.Click += btnImage_Click;
                  this.btnNoImage.Click += btnNoImage_Click;
                  this.btnOK.Click += btnOK_Click;
                  this.btnPaste.Click += btnPaste_Click;
                  this.cbSetType.SelectedIndexChanged += cbSetType_SelectedIndexChanged;
                  this.cbSlotCount.SelectedIndexChanged += cbSlotX_SelectedIndexChanged;
                  this.lstBonus.DoubleClick += lstBonus_DoubleClick;
                  this.txtAlternate.TextChanged += txtAlternate_TextChanged;
                  this.txtDesc.TextChanged += txtDesc_TextChanged;
                  this.txtInternal.TextChanged += txtInternal_TextChanged;
                  this.txtNameFull.TextChanged += txtNameFull_TextChanged;
                  this.txtNameShort.TextChanged += txtNameShort_TextChanged;
              }
              // finished with events
      this.ResumeLayout(false);
    }

    public bool isBonus()
    {
      return this.cbSlotCount.SelectedIndex > -1 & this.cbSlotCount.SelectedIndex < this.mySet.Enhancements.Length - 1;
    }

    public bool isSpecial()
    {
      return this.cbSlotCount.SelectedIndex >= this.mySet.Enhancements.Length - 1 & this.cbSlotCount.SelectedIndex < this.mySet.Enhancements.Length + this.mySet.Enhancements.Length - 1;
    }

    void lstBonus_DoubleClick(object sender, EventArgs e)

    {
      if (this.lstBonus.SelectedIndex < 0)
        return;
      int selectedIndex = this.lstBonus.SelectedIndex;
      int[] numArray1 = new int[0];
      string[] strArray1 = new string[0];
      int index1 = 0;
      if (this.isBonus())
      {
        int[] numArray2 = new int[this.mySet.Bonus[this.BonusID()].Index.Length - 2 + 1];
        string[] strArray2 = new string[this.mySet.Bonus[this.BonusID()].Name.Length - 2 + 1];
        int num1 = this.mySet.Bonus[this.BonusID()].Index.Length - 1;
        for (int index2 = 0; index2 <= num1; ++index2)
        {
          if (index2 != selectedIndex)
          {
            numArray2[index1] = this.mySet.Bonus[this.BonusID()].Index[index2];
            strArray2[index1] = this.mySet.Bonus[this.BonusID()].Name[index2];
            ++index1;
          }
        }
        this.mySet.Bonus[this.BonusID()].Name = new string[numArray2.Length - 1 + 1];
        this.mySet.Bonus[this.BonusID()].Index = new int[strArray2.Length - 1 + 1];
        int num2 = numArray2.Length - 1;
        for (int index2 = 0; index2 <= num2; ++index2)
        {
          this.mySet.Bonus[this.BonusID()].Index[index2] = numArray2[index2];
          this.mySet.Bonus[this.BonusID()].Name[index2] = strArray2[index2];
        }
      }
      else if (this.isSpecial())
      {
        int[] numArray2 = new int[this.mySet.SpecialBonus[this.SpecialID()].Index.Length - 2 + 1];
        string[] strArray2 = new string[this.mySet.SpecialBonus[this.SpecialID()].Name.Length - 2 + 1];
        int num1 = this.mySet.SpecialBonus[this.SpecialID()].Index.Length - 1;
        for (int index2 = 0; index2 <= num1; ++index2)
        {
          if (index2 != selectedIndex)
          {
            numArray2[index1] = this.mySet.SpecialBonus[this.SpecialID()].Index[index2];
            strArray2[index1] = this.mySet.SpecialBonus[this.SpecialID()].Name[index2];
            ++index1;
          }
        }
        this.mySet.SpecialBonus[this.SpecialID()].Name = new string[numArray2.Length - 1 + 1];
        this.mySet.SpecialBonus[this.SpecialID()].Index = new int[strArray2.Length - 1 + 1];
        int num2 = numArray2.Length - 1;
        for (int index2 = 0; index2 <= num2; ++index2)
        {
          this.mySet.SpecialBonus[this.SpecialID()].Index[index2] = numArray2[index2];
          this.mySet.SpecialBonus[this.SpecialID()].Name[index2] = strArray2[index2];
        }
        if (this.mySet.SpecialBonus[this.SpecialID()].Index.Length < 1)
          this.mySet.SpecialBonus[this.SpecialID()].Special = -1;
      }
      this.DisplayBonus();
      this.DisplayBonusText();
    }

    void lvBonusList_DoubleClick(object sender, EventArgs e)

    {
      if (this.lvBonusList.SelectedIndices.Count < 1)
        return;
      int index = (int) Math.Round(Conversion.Val(RuntimeHelpers.GetObjectValue(this.lvBonusList.SelectedItems[0].Tag)));
      if (index < 0)
      {
        int num = (int) Interaction.MsgBox((object) "Tag was < 0!", MsgBoxStyle.OkOnly, null);
      }
      else
      {
        if (this.isBonus())
        {
          this.mySet.Bonus[this.BonusID()].Name = (string[]) Utils.CopyArray((Array) this.mySet.Bonus[this.BonusID()].Name, (Array) new string[this.mySet.Bonus[this.BonusID()].Name.Length + 1]);
          this.mySet.Bonus[this.BonusID()].Index = (int[]) Utils.CopyArray((Array) this.mySet.Bonus[this.BonusID()].Index, (Array) new int[this.mySet.Bonus[this.BonusID()].Index.Length + 1]);
          this.mySet.Bonus[this.BonusID()].Name[this.mySet.Bonus[this.BonusID()].Name.Length - 1] = DatabaseAPI.Database.Power[index].FullName;
          this.mySet.Bonus[this.BonusID()].Index[this.mySet.Bonus[this.BonusID()].Index.Length - 1] = index;
        }
        else if (this.isSpecial())
        {
          this.mySet.SpecialBonus[this.SpecialID()].Special = this.SpecialID();
          this.mySet.SpecialBonus[this.SpecialID()].Name = (string[]) Utils.CopyArray((Array) this.mySet.SpecialBonus[this.SpecialID()].Name, (Array) new string[this.mySet.SpecialBonus[this.SpecialID()].Name.Length + 1]);
          this.mySet.SpecialBonus[this.SpecialID()].Index = (int[]) Utils.CopyArray((Array) this.mySet.SpecialBonus[this.SpecialID()].Index, (Array) new int[this.mySet.SpecialBonus[this.SpecialID()].Index.Length + 1]);
          this.mySet.SpecialBonus[this.SpecialID()].Name[this.mySet.SpecialBonus[this.SpecialID()].Name.Length - 1] = DatabaseAPI.Database.Power[index].FullName;
          this.mySet.SpecialBonus[this.SpecialID()].Index[this.mySet.SpecialBonus[this.SpecialID()].Index.Length - 1] = index;
        }
        this.DisplayBonus();
        this.DisplayBonusText();
      }
    }

    void lvBonusList_SelectedIndexChanged(object sender, EventArgs e)

    {
    }

    public void SetMaxLevel(int iValue)
    {
      if (Decimal.Compare(new Decimal(iValue), this.udMaxLevel.Minimum) < 0)
        iValue = Convert.ToInt32(this.udMaxLevel.Minimum);
      if (Decimal.Compare(new Decimal(iValue), this.udMaxLevel.Maximum) > 0)
        iValue = Convert.ToInt32(this.udMaxLevel.Maximum);
      this.udMaxLevel.Value = new Decimal(iValue);
    }

    public void SetMinLevel(int iValue)
    {
      if (Decimal.Compare(new Decimal(iValue), this.udMinLevel.Minimum) < 0)
        iValue = Convert.ToInt32(this.udMinLevel.Minimum);
      if (Decimal.Compare(new Decimal(iValue), this.udMinLevel.Maximum) > 0)
        iValue = Convert.ToInt32(this.udMinLevel.Maximum);
      this.udMinLevel.Value = new Decimal(iValue);
    }

    public int SpecialID()
    {
      return this.cbSlotCount.SelectedIndex - (this.mySet.Enhancements.Length - 1);
    }

    void txtAlternate_TextChanged(object sender, EventArgs e)

    {
      if (this.isBonus())
        this.mySet.Bonus[this.BonusID()].AltString = this.txtAlternate.Text;
      else if (this.isSpecial())
        this.mySet.SpecialBonus[this.SpecialID()].AltString = this.txtAlternate.Text;
      this.DisplayBonusText();
    }

    void txtDesc_TextChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.mySet.Desc = this.txtDesc.Text;
    }

    void txtInternal_TextChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.mySet.Uid = this.txtInternal.Text;
    }

    void txtNameFull_TextChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.mySet.DisplayName = this.txtNameFull.Text;
    }

    void txtNameShort_TextChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.mySet.ShortName = this.txtNameShort.Text;
    }

    void udMaxLevel_Leave(object sender, EventArgs e)

    {
      this.SetMaxLevel((int) Math.Round(Conversion.Val(this.udMaxLevel.Text)));
      this.mySet.LevelMax = Convert.ToInt32(Decimal.Subtract(this.udMaxLevel.Value, new Decimal(1)));
    }

    void udMaxLevel_ValueChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.mySet.LevelMax = Convert.ToInt32(Decimal.Subtract(this.udMaxLevel.Value, new Decimal(1)));
      this.udMinLevel.Maximum = this.udMaxLevel.Value;
    }

    void udMinLevel_Leave(object sender, EventArgs e)

    {
      this.SetMinLevel((int) Math.Round(Conversion.Val(this.udMinLevel.Text)));
      this.mySet.LevelMin = Convert.ToInt32(Decimal.Subtract(this.udMinLevel.Value, new Decimal(1)));
    }

    void udMinLevel_ValueChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.mySet.LevelMin = Convert.ToInt32(Decimal.Subtract(this.udMinLevel.Value, new Decimal(1)));
      this.udMaxLevel.Minimum = this.udMinLevel.Value;
    }
  }
}
