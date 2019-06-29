
using Base.Data_Classes;
using Base.Display;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Hero_Designer
{
    public class frmEnhData : Form
    {
        Button btnAdd;

        Button btnAddFX;

        Button btnAutoFill;

        Button btnCancel;

        Button btnDown;

        Button btnEdit;

        Button btnEditPowerData;

        Button btnImage;

        Button btnNoImage;

        Button btnOK;

        Button btnRemove;

        Button btnUp;

        ComboBox cbMutEx;

        ComboBox cbRecipe;

        ComboBox cbSched;

        ComboBox cbSet;

        ComboBox cbSubType;

        CheckBox chkSuperior;

        CheckBox chkUnique;
        GroupBox gbBasic;
        GroupBox gbClass;
        GroupBox gbEffects;
        GroupBox gbMod;
        GroupBox gbSet;
        GroupBox gbType;
        OpenFileDialog ImagePicker;
        Label Label1;
        Label Label10;
        Label Label11;
        Label Label2;
        Label Label3;
        Label Label4;
        Label Label5;
        Label Label6;
        Label Label7;
        Label Label8;
        Label Label9;
        Label lblClass;
        Label lblSched;

        ListBox lstAvailable;

        ListBox lstSelected;
        PictureBox pbSet;

        Panel pnlClass;

        Panel pnlClassList;

        RadioButton rbBoth;

        RadioButton rbBuff;

        RadioButton rbDebuff;

        RadioButton rbMod1;

        RadioButton rbMod2;

        RadioButton rbMod3;

        RadioButton rbMod4;

        RadioButton rbModOther;

        TextBox StaticIndex;
        ToolTip tTip;

        TextBox txtDesc;

        TextBox txtInternal;

        TextBox txtModOther;

        TextBox txtNameFull;

        TextBox txtNameShort;

        TextBox txtProb;

        RadioButton typeHO;

        RadioButton typeIO;

        RadioButton typeRegular;

        RadioButton typeSet;

        NumericUpDown udMaxLevel;

        NumericUpDown udMinLevel;

        protected ExtendedBitmap bxClass;
        protected ExtendedBitmap bxClassList;
        protected int ClassSize;
        IContainer components;

        protected int EnhAcross;
        protected int EnhPadding;
        protected bool Loading;
        public IEnhancement myEnh;



















        public frmEnhData(ref IEnhancement iEnh)
        {
            this.Load += new EventHandler(this.frmEnhData_Load);
            this.ClassSize = 15;
            this.EnhPadding = 3;
            this.EnhAcross = 5;
            this.Loading = true;
            this.InitializeComponent();
            this.myEnh = (IEnhancement)new Enhancement(iEnh);
            this.ClassSize = 22;
        }

        void btnAdd_Click(object sender, EventArgs e)

        {
            this.EffectList_Add();
        }

        void btnAddFX_Click(object sender, EventArgs e)

        {
            IEffect iFX = new Effect();
            frmPowerEffect frmPowerEffect = new frmPowerEffect(iFX);
            if (frmPowerEffect.ShowDialog() != DialogResult.OK)
                return;
            IEnhancement enh = this.myEnh;
            Enums.sEffect[] sEffectArray = (Enums.sEffect[])Utils.CopyArray((Array)enh.Effect, (Array)new Enums.sEffect[this.myEnh.Effect.Length + 1]);
            enh.Effect = sEffectArray;
            Enums.sEffect[] effect = this.myEnh.Effect;
            int index = this.myEnh.Effect.Length - 1;
            effect[index].Mode = Enums.eEffMode.FX;
            effect[index].Enhance.ID = -1;
            effect[index].Enhance.SubID = -1;
            effect[index].Multiplier = 1f;
            effect[index].Schedule = Enums.eSchedule.A;
            effect[index].FX = (IEffect)frmPowerEffect.myFX.Clone();
            effect[index].FX.isEnahncementEffect = true;
            this.ListSelectedEffects();
            this.lstSelected.SelectedIndex = this.lstSelected.Items.Count - 1;
        }

        void btnAutoFill_Click(object sender, EventArgs e)

        {
            Enums.eEnhance eEnhance = Enums.eEnhance.None;
            Enums.eEnhanceShort eEnhanceShort = Enums.eEnhanceShort.None;
            Enums.eMez eMez = Enums.eMez.None;
            Enums.eMezShort eMezShort = Enums.eMezShort.None;
            string[] names1 = Enum.GetNames(eEnhance.GetType());
            string[] names2 = Enum.GetNames(eEnhanceShort.GetType());
            string[] names3 = Enum.GetNames(eMez.GetType());
            string[] names4 = Enum.GetNames(eMezShort.GetType());
            this.myEnh.Name = "";
            this.myEnh.ShortName = "";
            names1[4] = "Endurance";
            names1[18] = "Resistance";
            names1[5] = "EndMod";
            names2[18] = "ResDam";
            names3[2] = "Hold";
            names4[2] = "Hold";
            if (this.myEnh.TypeID == Enums.eType.SetO & this.myEnh.nIDSet > -1 & this.myEnh.nIDSet < DatabaseAPI.Database.EnhancementSets.Count - 1)
                this.myEnh.UID = DatabaseAPI.Database.EnhancementSets[this.myEnh.nIDSet].DisplayName.Replace(" ", "_") + "_";
            int num1 = 0;
            int num2 = this.myEnh.Effect.Length - 1;
            for (int index = 0; index <= num2; ++index)
            {
                if (this.myEnh.Effect[index].Mode == Enums.eEffMode.Enhancement)
                {
                    ++num1;
                    Enums.eEnhance id = (Enums.eEnhance)this.myEnh.Effect[index].Enhance.ID;
                    if (id != Enums.eEnhance.Mez)
                    {
                        if (this.myEnh.Name != "")
                            this.myEnh.Name += "/";
                        this.myEnh.Name += names1[(int)id];
                        if (this.myEnh.ShortName != "")
                            this.myEnh.ShortName += "/";
                        this.myEnh.ShortName += names2[(int)id];
                    }
                    else
                    {
                        if (this.myEnh.Name != "")
                            this.myEnh.Name += "/";
                        this.myEnh.Name += names3[this.myEnh.Effect[index].Enhance.SubID];
                        if (this.myEnh.ShortName != "")
                            this.myEnh.ShortName += "/";
                        this.myEnh.ShortName += names4[this.myEnh.Effect[index].Enhance.SubID];
                    }
                }
            }
            float num3 = 1f;
            switch (num1)
            {
                case 2:
                    num3 = 0.625f;
                    break;
                case 3:
                    num3 = 0.5f;
                    break;
                case 4:
                    num3 = 7f / 16f;
                    break;
            }
            int num4 = this.myEnh.Effect.Length - 1;
            for (int index = 0; index <= num4; ++index)
            {
                if (this.myEnh.Effect[index].Mode == Enums.eEffMode.Enhancement)
                    this.myEnh.Effect[index].Multiplier = num3;
            }
            this.DisplayAll();
        }

        void btnCancel_Click(object sender, EventArgs e)

        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        void btnDown_Click(object sender, EventArgs e)

        {
            if (this.lstSelected.SelectedIndices.Count <= 0)
                return;
            int selectedIndex = this.lstSelected.SelectedIndices[0];
            if (selectedIndex < this.lstSelected.Items.Count - 1)
            {
                Enums.sEffect[] sEffectArray = new Enums.sEffect[2];
                sEffectArray[0].Assign(this.myEnh.Effect[selectedIndex]);
                sEffectArray[1].Assign(this.myEnh.Effect[selectedIndex + 1]);
                this.myEnh.Effect[selectedIndex + 1].Assign(sEffectArray[0]);
                this.myEnh.Effect[selectedIndex].Assign(sEffectArray[1]);
                this.FillEffectList();
                this.ListSelectedEffects();
                this.lstSelected.SelectedIndex = selectedIndex + 1;
            }
        }

        void btnEdit_Click(object sender, EventArgs e)

        {
            this.EditClick();
        }

        void btnEditPowerData_Click(object sender, EventArgs e)

        {
            IEnhancement enh = this.myEnh;
            IPower power = enh.Power;
            enh.Power = power;
            frmEditPower frmEditPower = new frmEditPower(ref power);
            if (frmEditPower.ShowDialog() != DialogResult.OK)
                return;
            this.myEnh.Power = (IPower)new Power(frmEditPower.myPower);
            this.myEnh.Power.IsModified = true;
            int num = this.myEnh.Power.Effects.Length - 1;
            for (int index = 0; index <= num; ++index)
                this.myEnh.Power.Effects[index].PowerFullName = this.myEnh.Power.FullName;
        }

        void btnImage_Click(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            this.ImagePicker.InitialDirectory = I9Gfx.GetEnhancementsPath();
            this.ImagePicker.FileName = this.myEnh.Image;
            if (this.ImagePicker.ShowDialog() == DialogResult.OK)
            {
                string str = FileIO.StripPath(this.ImagePicker.FileName);
                if (!File.Exists(FileIO.AddSlash(this.ImagePicker.InitialDirectory) + str))
                {
                    int num = (int)Interaction.MsgBox((object)("You must select an image from the " + I9Gfx.GetEnhancementsPath() + " folder!\r\n\r\nIf you are adding a new image, you should copy it to the folder and then select it."), MsgBoxStyle.Information, (object)"Ah...");
                }
                else
                {
                    this.myEnh.Image = str;
                    this.DisplayIcon();
                    this.SetTypeIcons();
                }
            }
        }

        void btnNoImage_Click(object sender, EventArgs e)

        {
            this.myEnh.Image = "";
            this.SetTypeIcons();
            this.DisplayIcon();
        }

        void btnOK_Click(object sender, EventArgs e)

        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        void btnRemove_Click(object sender, EventArgs e)

        {
            if (this.lstSelected.SelectedIndex <= -1)
                return;
            Enums.sEffect[] sEffectArray = new Enums.sEffect[this.myEnh.Effect.Length - 1 + 1];
            int selectedIndex = this.lstSelected.SelectedIndex;
            int index1 = 0;
            int num1 = this.myEnh.Effect.Length - 1;
            for (int index2 = 0; index2 <= num1; ++index2)
            {
                if (index2 != selectedIndex)
                {
                    sEffectArray[index1].Assign(this.myEnh.Effect[index2]);
                    ++index1;
                }
            }
            this.myEnh.Effect = new Enums.sEffect[this.myEnh.Effect.Length - 2 + 1];
            int num2 = this.myEnh.Effect.Length - 1;
            for (int index2 = 0; index2 <= num2; ++index2)
                this.myEnh.Effect[index2].Assign(sEffectArray[index2]);
            this.FillEffectList();
            this.ListSelectedEffects();
            if (this.lstSelected.Items.Count > selectedIndex)
                this.lstSelected.SelectedIndex = selectedIndex;
            else if (this.lstSelected.Items.Count == selectedIndex)
                this.lstSelected.SelectedIndex = selectedIndex - 1;
        }

        void btnUp_Click(object sender, EventArgs e)

        {
            if (this.lstSelected.SelectedIndices.Count <= 0)
                return;
            int selectedIndex = this.lstSelected.SelectedIndices[0];
            if (selectedIndex >= 1)
            {
                Enums.sEffect[] sEffectArray = new Enums.sEffect[2];
                sEffectArray[0].Assign(this.myEnh.Effect[selectedIndex]);
                sEffectArray[1].Assign(this.myEnh.Effect[selectedIndex - 1]);
                this.myEnh.Effect[selectedIndex - 1].Assign(sEffectArray[0]);
                this.myEnh.Effect[selectedIndex].Assign(sEffectArray[1]);
                this.FillEffectList();
                this.ListSelectedEffects();
                this.lstSelected.SelectedIndex = selectedIndex - 1;
            }
        }

        void cbMutEx_SelectedIndexChanged(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            this.myEnh.MutExID = (Enums.eEnhMutex)this.cbMutEx.SelectedIndex;
        }

        void cbRecipe_SelectedIndexChanged(object sender, EventArgs e)

        {
            if (this.cbRecipe.SelectedIndex > 0)
            {
                this.myEnh.RecipeName = this.cbRecipe.Text;
                this.myEnh.RecipeIDX = this.cbRecipe.SelectedIndex - 1;
            }
            else
            {
                this.myEnh.RecipeName = "";
                this.myEnh.RecipeIDX = -1;
            }
        }

        void cbSched_SelectedIndexChanged(object sender, EventArgs e)

        {
            if (this.lstSelected.SelectedIndex <= -1)
                return;
            int selectedIndex = this.lstSelected.SelectedIndex;
            if (this.myEnh.Effect[selectedIndex].Mode == Enums.eEffMode.Enhancement)
                this.myEnh.Effect[selectedIndex].Schedule = (Enums.eSchedule)this.cbSched.SelectedIndex;
        }

        void cbSet_SelectedIndexChanged(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            this.myEnh.nIDSet = this.cbSet.SelectedIndex - 1;
            this.myEnh.UIDSet = this.myEnh.nIDSet <= -1 ? string.Empty : DatabaseAPI.Database.EnhancementSets[this.myEnh.nIDSet].Uid;
            this.UpdateTitle();
            this.DisplaySetImage();
        }

        void cbSubType_SelectedIndexChanged(object sender, EventArgs e)

        {
            this.myEnh.SubTypeID = (Enums.eSubtype)this.cbSubType.SelectedIndex;
        }

        void chkSuperior_CheckedChanged(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            this.myEnh.Superior = this.chkSuperior.Checked;
            if (this.chkSuperior.Checked)
            {
                this.myEnh.LevelMin = 49;
                this.myEnh.LevelMax = 49;
                this.udMinLevel.Value = new Decimal(50);
                this.udMaxLevel.Value = new Decimal(50);
            }
            this.chkUnique.Checked = true;
        }

        void chkUnique_CheckedChanged(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            this.myEnh.Unique = this.chkUnique.Checked;
        }

        public void DisplayAll()
        {
            this.txtNameFull.Text = this.myEnh.Name;
            this.txtNameShort.Text = this.myEnh.ShortName;
            this.txtDesc.Text = this.myEnh.Desc;
            this.txtProb.Text = Conversions.ToString(this.myEnh.EffectChance);
            this.txtInternal.Text = this.myEnh.UID;
            this.StaticIndex.Text = Conversions.ToString(this.myEnh.StaticIndex);
            this.SetMinLevel(this.myEnh.LevelMin + 1);
            this.SetMaxLevel(this.myEnh.LevelMax + 1);
            this.udMaxLevel.Minimum = this.udMinLevel.Value;
            this.udMinLevel.Maximum = this.udMaxLevel.Value;
            this.chkUnique.Checked = this.myEnh.Unique;
            this.cbMutEx.SelectedIndex = (int)this.myEnh.MutExID;
            this.chkSuperior.Checked = this.myEnh.Superior;
            switch (this.myEnh.TypeID)
            {
                case Enums.eType.Normal:
                    this.typeRegular.Checked = true;
                    this.cbSubType.SelectedIndex = -1;
                    this.cbSubType.Enabled = false;
                    this.cbRecipe.SelectedIndex = 0;
                    this.cbRecipe.Enabled = false;
                    break;
                case Enums.eType.InventO:
                    this.typeIO.Checked = true;
                    this.cbSubType.SelectedIndex = -1;
                    this.cbSubType.Enabled = false;
                    this.cbRecipe.SelectedIndex = this.myEnh.RecipeIDX + 1;
                    this.cbRecipe.Enabled = true;
                    break;
                case Enums.eType.SpecialO:
                    this.typeHO.Checked = true;
                    this.cbSubType.SelectedIndex = (int)this.myEnh.SubTypeID;
                    this.cbSubType.Enabled = true;
                    this.cbRecipe.Enabled = false;
                    this.cbRecipe.SelectedIndex = 0;
                    break;
                case Enums.eType.SetO:
                    this.cbSubType.SelectedIndex = -1;
                    this.cbSubType.Enabled = false;
                    this.typeSet.Checked = true;
                    this.cbRecipe.SelectedIndex = this.myEnh.RecipeIDX + 1;
                    this.cbRecipe.Enabled = true;
                    break;
                default:
                    this.typeRegular.Checked = true;
                    this.cbSubType.SelectedIndex = -1;
                    this.cbSubType.Enabled = false;
                    this.cbRecipe.Enabled = false;
                    break;
            }
            this.DisplaySet();
            this.btnImage.Text = this.myEnh.Image;
            this.DisplayIcon();
            this.DisplaySetImage();
            this.DrawClasses();
            this.ListSelectedEffects();
            this.DisplayEnhanceData();
        }

        public void DisplayEnhanceData()
        {
            if (this.lstSelected.SelectedIndex <= -1)
            {
                this.btnRemove.Enabled = false;
                this.gbMod.Enabled = false;
                this.cbSched.Enabled = false;
                this.btnEdit.Enabled = false;
            }
            else
            {
                this.btnRemove.Enabled = true;
                int selectedIndex = this.lstSelected.SelectedIndex;
                if (this.myEnh.Effect[selectedIndex].Mode != Enums.eEffMode.Enhancement)
                {
                    this.btnEdit.Enabled = true;
                    this.gbMod.Enabled = false;
                    this.cbSched.Enabled = false;
                }
                else
                {
                    if (this.myEnh.Effect[selectedIndex].Enhance.ID == 12)
                        this.btnEdit.Enabled = true;
                    else
                        this.btnEdit.Enabled = false;
                    this.gbMod.Enabled = true;
                    this.cbSched.Enabled = true;
                    switch (this.myEnh.Effect[selectedIndex].Multiplier.ToString())
                    {
                        case "1":
                            this.rbMod1.Checked = true;
                            this.txtModOther.Text = "";
                            this.txtModOther.Enabled = false;
                            break;
                        case "0.625":
                            this.rbMod2.Checked = true;
                            this.txtModOther.Text = "";
                            this.txtModOther.Enabled = false;
                            break;
                        case "0.5":
                            this.rbMod3.Checked = true;
                            this.txtModOther.Text = "";
                            this.txtModOther.Enabled = false;
                            break;
                        default:
                            this.txtModOther.Text = Conversions.ToString(this.myEnh.Effect[selectedIndex].Multiplier);
                            this.rbModOther.Checked = true;
                            this.txtModOther.Enabled = true;
                            break;
                    }
                    switch (this.myEnh.Effect[selectedIndex].BuffMode)
                    {
                        case Enums.eBuffDebuff.BuffOnly:
                            this.rbBuff.Checked = true;
                            break;
                        case Enums.eBuffDebuff.DeBuffOnly:
                            this.rbDebuff.Checked = true;
                            break;
                        default:
                            this.rbBoth.Checked = true;
                            break;
                    }
                    this.cbSched.SelectedIndex = (int)this.myEnh.Effect[selectedIndex].Schedule;
                }
            }
        }

        public void DisplayIcon()
        {
            if (this.myEnh.Image != string.Empty)
            {
                ExtendedBitmap extendedBitmap1 = new ExtendedBitmap(I9Gfx.GetEnhancementsPath() + this.myEnh.Image);
                ExtendedBitmap extendedBitmap2 = new ExtendedBitmap(30, 30);
                extendedBitmap2.Graphics.DrawImage((Image)I9Gfx.Borders.Bitmap, extendedBitmap2.ClipRect, I9Gfx.GetOverlayRect(I9Gfx.ToGfxGrade(this.myEnh.TypeID)), GraphicsUnit.Pixel);
                extendedBitmap2.Graphics.DrawImage((Image)extendedBitmap1.Bitmap, extendedBitmap2.ClipRect, extendedBitmap2.ClipRect, GraphicsUnit.Pixel);
                this.btnImage.Image = (Image)new Bitmap((Image)extendedBitmap2.Bitmap);
                this.btnImage.Text = this.myEnh.Image;
            }
            else
            {
                switch (this.myEnh.TypeID)
                {
                    case Enums.eType.Normal:
                        this.btnImage.Image = this.typeRegular.Image;
                        break;
                    case Enums.eType.InventO:
                        this.btnImage.Image = this.typeIO.Image;
                        break;
                    case Enums.eType.SpecialO:
                        this.btnImage.Image = this.typeHO.Image;
                        break;
                    case Enums.eType.SetO:
                        this.btnImage.Image = this.typeSet.Image;
                        break;
                }
                this.btnImage.Text = "Select Image";
            }
        }

        public void DisplaySet()
        {
            this.gbSet.Enabled = this.myEnh.TypeID == Enums.eType.SetO;
            this.cbSet.SelectedIndex = this.myEnh.nIDSet + 1;
            this.DisplaySetImage();
        }

        public void DisplaySetImage()
        {
            if (this.myEnh.nIDSet > -1)
            {
                this.myEnh.Image = DatabaseAPI.Database.EnhancementSets[this.myEnh.nIDSet].Image;
                this.DisplayIcon();
                this.SetTypeIcons();
                if (DatabaseAPI.Database.EnhancementSets[this.myEnh.nIDSet].Image != "")
                {
                    ExtendedBitmap extendedBitmap1 = new ExtendedBitmap(I9Gfx.GetEnhancementsPath() + DatabaseAPI.Database.EnhancementSets[this.myEnh.nIDSet].Image);
                    ExtendedBitmap extendedBitmap2 = new ExtendedBitmap(30, 30);
                    extendedBitmap2.Graphics.DrawImage((Image)I9Gfx.Borders.Bitmap, extendedBitmap2.ClipRect, I9Gfx.GetOverlayRect(Origin.Grade.SetO), GraphicsUnit.Pixel);
                    extendedBitmap2.Graphics.DrawImage((Image)extendedBitmap1.Bitmap, extendedBitmap2.ClipRect, extendedBitmap2.ClipRect, GraphicsUnit.Pixel);
                    this.pbSet.Image = (Image)new Bitmap((Image)extendedBitmap2.Bitmap);
                }
                else
                {
                    ExtendedBitmap extendedBitmap = new ExtendedBitmap(30, 30);
                    extendedBitmap.Graphics.DrawImage((Image)I9Gfx.Borders.Bitmap, extendedBitmap.ClipRect, I9Gfx.GetOverlayRect(Origin.Grade.SetO), GraphicsUnit.Pixel);
                    this.pbSet.Image = (Image)new Bitmap((Image)extendedBitmap.Bitmap);
                }
            }
            else
                this.pbSet.Image = (Image)new Bitmap(this.pbSet.Width, this.pbSet.Height);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        public void DrawClasses()
        {
            this.bxClass = new ExtendedBitmap(this.pnlClass.Width, this.pnlClass.Height);
            int enhPadding1 = this.EnhPadding;
            int enhPadding2 = this.EnhPadding;
            int num1 = 0;
            this.bxClass.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(0, 0, 0)), this.bxClass.ClipRect);
            int num2 = this.myEnh.ClassID.Length - 1;
            for (int index = 0; index <= num2; ++index)
            {
                Rectangle destRect = new Rectangle(enhPadding2, enhPadding1, this.ClassSize, this.ClassSize);
                this.bxClass.Graphics.DrawImage((Image)I9Gfx.Classes.Bitmap, destRect, I9Gfx.GetImageRect(this.myEnh.ClassID[index]), GraphicsUnit.Pixel);
                enhPadding2 += this.ClassSize + this.EnhPadding;
                ++num1;
                if (num1 == 2)
                {
                    num1 = 0;
                    enhPadding2 = this.EnhPadding;
                    enhPadding1 += this.ClassSize + this.EnhPadding;
                }
            }
            this.pnlClass.CreateGraphics().DrawImageUnscaled((Image)this.bxClass.Bitmap, 0, 0);
        }

        public void DrawClassList()
        {
            this.bxClassList = new ExtendedBitmap(this.pnlClassList.Width, this.pnlClassList.Height);
            int enhPadding1 = this.EnhPadding;
            int enhPadding2 = this.EnhPadding;
            int num1 = 0;
            this.bxClassList.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(0, 0, 0)), this.bxClassList.ClipRect);
            int num2 = DatabaseAPI.Database.EnhancementClasses.Length - 1;
            for (int index = 0; index <= num2; ++index)
            {
                Rectangle destRect = new Rectangle(enhPadding2, enhPadding1, 30, 30);
                this.bxClassList.Graphics.DrawImage((Image)I9Gfx.Classes.Bitmap, destRect, I9Gfx.GetImageRect(index), GraphicsUnit.Pixel);
                enhPadding2 += 30 + this.EnhPadding;
                ++num1;
                if (num1 == this.EnhAcross)
                {
                    num1 = 0;
                    enhPadding2 = this.EnhPadding;
                    enhPadding1 += 30 + this.EnhPadding;
                }
            }
            this.pnlClassList.CreateGraphics().DrawImageUnscaled((Image)this.bxClassList.Bitmap, 0, 0);
        }

        public void EditClick()
        {
            bool flag = true;
            int num1 = -1;
            if (this.lstSelected.SelectedIndex <= -1)
                return;
            int selectedIndex = this.lstSelected.SelectedIndex;
            if (this.myEnh.Effect[selectedIndex].Mode == Enums.eEffMode.Enhancement)
            {
                if (this.myEnh.Effect[selectedIndex].Enhance.ID == 12)
                {
                    int subId = this.myEnh.Effect[selectedIndex].Enhance.SubID;
                    num1 = this.MezPicker(subId);
                    if (num1 == subId)
                        return;
                    int num2 = this.myEnh.Effect.Length - 1;
                    for (int index1 = 0; index1 <= num2; ++index1)
                    {
                        Enums.sEffect[] effect = this.myEnh.Effect;
                        int index2 = index1;
                        if (effect[index2].Mode == Enums.eEffMode.Enhancement & effect[index2].Enhance.SubID == num1)
                            flag = false;
                    }
                }
                if (!flag)
                {
                    int num2 = (int)Interaction.MsgBox((object)"This effect has already been added!", MsgBoxStyle.Information, (object)"There can be only one.");
                    return;
                }
                this.myEnh.Effect[selectedIndex].Enhance.SubID = num1;
            }
            else
            {
                frmPowerEffect frmPowerEffect = new frmPowerEffect(this.myEnh.Effect[selectedIndex].FX);
                if (frmPowerEffect.ShowDialog() == DialogResult.OK)
                {
                    Enums.sEffect[] effect = this.myEnh.Effect;
                    int index = selectedIndex;
                    effect[index].Mode = Enums.eEffMode.FX;
                    effect[index].Enhance.ID = -1;
                    effect[index].Enhance.SubID = -1;
                    effect[index].Multiplier = 1f;
                    effect[index].Schedule = Enums.eSchedule.A;
                    effect[index].FX = (IEffect)frmPowerEffect.myFX.Clone();
                }
            }
            this.ListSelectedEffects();
            this.lstSelected.SelectedIndex = selectedIndex;
        }

        public void EffectList_Add()
        {
            if (this.lstAvailable.SelectedIndex <= -1)
                return;
            Enums.eEnhance eEnhance = Enums.eEnhance.None;
            bool flag = true;
            int tSub = -1;
            Enums.eEnhance integer = (Enums.eEnhance)Conversions.ToInteger(Enum.Parse(eEnhance.GetType(), Conversions.ToString(this.lstAvailable.Items[this.lstAvailable.SelectedIndex])));
            if (integer == Enums.eEnhance.Mez)
            {
                tSub = this.MezPicker(1);
                int num = this.myEnh.Effect.Length - 1;
                for (int index1 = 0; index1 <= num; ++index1)
                {
                    Enums.sEffect[] effect = this.myEnh.Effect;
                    int index2 = index1;
                    if (effect[index2].Mode == Enums.eEffMode.Enhancement & effect[index2].Enhance.SubID == tSub)
                        flag = false;
                }
            }
            if (!flag)
            {
                int num1 = (int)Interaction.MsgBox((object)"This effect has already been added!", MsgBoxStyle.Information, (object)"There can be only one.");
            }
            else
            {
                IEnhancement enh = this.myEnh;
                Enums.sEffect[] sEffectArray = (Enums.sEffect[])Utils.CopyArray((Array)enh.Effect, (Array)new Enums.sEffect[this.myEnh.Effect.Length + 1]);
                enh.Effect = sEffectArray;
                Enums.sEffect[] effect = this.myEnh.Effect;
                int index = this.myEnh.Effect.Length - 1;
                effect[index].Mode = Enums.eEffMode.Enhancement;
                effect[index].Enhance.ID = (int)integer;
                effect[index].Enhance.SubID = tSub;
                effect[index].Multiplier = 1f;
                effect[index].Schedule = Enhancement.GetSchedule(integer, tSub);
                this.FillEffectList();
                this.ListSelectedEffects();
                this.lstSelected.SelectedIndex = this.lstSelected.Items.Count - 1;
            }
        }

        public void FillEffectList()
        {
            Enums.eEnhance eEnhance1 = Enums.eEnhance.None;
            this.lstAvailable.BeginUpdate();
            this.lstAvailable.Items.Clear();
            string[] names = Enum.GetNames(eEnhance1.GetType());
            int num1 = names.Length - 1;
            for (int index1 = 1; index1 <= num1; ++index1)
            {
                Enums.eEnhance eEnhance2 = (Enums.eEnhance)index1;
                bool flag = false;
                int num2 = this.myEnh.Effect.Length - 1;
                for (int index2 = 0; index2 <= num2; ++index2)
                {
                    if (this.myEnh.Effect[index2].Mode == Enums.eEffMode.Enhancement && this.myEnh.Effect[index2].Enhance.ID == index1 & eEnhance2 != Enums.eEnhance.Mez)
                        flag = true;
                }
                if (!flag)
                    this.lstAvailable.Items.Add((object)names[index1]);
            }
            this.btnAdd.Enabled = this.lstAvailable.Items.Count > 0;
            this.lstAvailable.EndUpdate();
        }

        public void FillMutExList()
        {
            string[] names = Enum.GetNames(Enums.eEnhMutex.None.GetType());
            this.cbMutEx.BeginUpdate();
            this.cbMutEx.Items.Clear();
            this.cbMutEx.Items.AddRange((object[])names);
            this.cbMutEx.EndUpdate();
        }

        public void FillRecipeList()
        {
            this.cbRecipe.BeginUpdate();
            this.cbRecipe.Items.Clear();
            this.cbRecipe.Items.Add((object)"None");
            int num = DatabaseAPI.Database.Recipes.Length - 1;
            for (int index = 0; index <= num; ++index)
                this.cbRecipe.Items.Add((object)DatabaseAPI.Database.Recipes[index].InternalName);
            this.cbRecipe.EndUpdate();
        }

        public void FillSchedules()
        {
            this.cbSched.BeginUpdate();
            this.cbSched.Items.Clear();
            string Style = "##0" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "##";
            this.cbSched.Items.Add((object)("A (" + Strings.Format((object)(float)((double)DatabaseAPI.Database.MultSO[0][0] * 100.0), Style) + "%)"));
            this.cbSched.Items.Add((object)("B (" + Strings.Format((object)(float)((double)DatabaseAPI.Database.MultSO[0][1] * 100.0), Style) + "%)"));
            this.cbSched.Items.Add((object)("C (" + Strings.Format((object)(float)((double)DatabaseAPI.Database.MultSO[0][2] * 100.0), Style) + "%)"));
            this.cbSched.Items.Add((object)("D (" + Strings.Format((object)(float)((double)DatabaseAPI.Database.MultSO[0][3] * 100.0), Style) + "%)"));
            this.cbSched.EndUpdate();
        }

        public void FillSetList()
        {
            this.cbSet.BeginUpdate();
            this.cbSet.Items.Clear();
            this.cbSet.Items.Add((object)"None");
            int num = DatabaseAPI.Database.EnhancementSets.Count - 1;
            for (int index = 0; index <= num; ++index)
                this.cbSet.Items.Add((object)DatabaseAPI.Database.EnhancementSets[index].Uid);
            this.cbSet.EndUpdate();
        }

        public void FillSubTypeList()
        {
            string[] names = Enum.GetNames(Enums.eSubtype.None.GetType());
            this.cbSubType.BeginUpdate();
            this.cbSubType.Items.Clear();
            this.cbSubType.Items.AddRange((object[])names);
            this.cbSubType.EndUpdate();
        }

        void frmEnhData_Load(object sender, EventArgs e)

        {
            this.FillSetList();
            this.FillEffectList();
            this.FillMutExList();
            this.FillSubTypeList();
            this.FillRecipeList();
            this.DisplayAll();
            this.SetTypeIcons();
            this.DrawClassList();
            this.FillSchedules();
            this.UpdateTitle();
            this.Loading = false;
        }

        [DebuggerStepThrough]
        void InitializeComponent()

        {
            this.components = (IContainer)new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmEnhData));
            this.gbBasic = new GroupBox();
            this.txtInternal = new TextBox();
            this.Label9 = new Label();
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
            this.gbType = new GroupBox();
            this.cbSubType = new ComboBox();
            this.typeSet = new RadioButton();
            this.typeIO = new RadioButton();
            this.typeRegular = new RadioButton();
            this.typeHO = new RadioButton();
            this.cbSet = new ComboBox();
            this.gbSet = new GroupBox();
            this.chkSuperior = new CheckBox();
            this.pbSet = new PictureBox();
            this.chkUnique = new CheckBox();
            this.gbEffects = new GroupBox();
            this.btnDown = new Button();
            this.btnUp = new Button();
            this.rbBoth = new RadioButton();
            this.rbDebuff = new RadioButton();
            this.rbBuff = new RadioButton();
            this.btnAutoFill = new Button();
            this.Label5 = new Label();
            this.txtProb = new TextBox();
            this.Label1 = new Label();
            this.btnEdit = new Button();
            this.btnAddFX = new Button();
            this.btnRemove = new Button();
            this.btnAdd = new Button();
            this.gbMod = new GroupBox();
            this.rbMod4 = new RadioButton();
            this.txtModOther = new TextBox();
            this.rbModOther = new RadioButton();
            this.rbMod3 = new RadioButton();
            this.rbMod2 = new RadioButton();
            this.rbMod1 = new RadioButton();
            this.lstSelected = new ListBox();
            this.lstAvailable = new ListBox();
            this.cbSched = new ComboBox();
            this.lblSched = new Label();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.gbClass = new GroupBox();
            this.lblClass = new Label();
            this.pnlClassList = new Panel();
            this.pnlClass = new Panel();
            this.ImagePicker = new OpenFileDialog();
            this.btnNoImage = new Button();
            this.tTip = new ToolTip(this.components);
            this.cbMutEx = new ComboBox();
            this.cbRecipe = new ComboBox();
            this.Label8 = new Label();
            this.Label10 = new Label();
            this.btnEditPowerData = new Button();
            this.StaticIndex = new TextBox();
            this.Label11 = new Label();
            this.gbBasic.SuspendLayout();
            this.udMinLevel.BeginInit();
            this.udMaxLevel.BeginInit();
            this.gbType.SuspendLayout();
            this.gbSet.SuspendLayout();
            ((ISupportInitialize)this.pbSet).BeginInit();
            this.gbEffects.SuspendLayout();
            this.gbMod.SuspendLayout();
            this.gbClass.SuspendLayout();
            this.SuspendLayout();
            this.gbBasic.Controls.Add((Control)this.txtInternal);
            this.gbBasic.Controls.Add((Control)this.Label9);
            this.gbBasic.Controls.Add((Control)this.Label7);
            this.gbBasic.Controls.Add((Control)this.Label6);
            this.gbBasic.Controls.Add((Control)this.udMinLevel);
            this.gbBasic.Controls.Add((Control)this.udMaxLevel);
            this.gbBasic.Controls.Add((Control)this.txtDesc);
            this.gbBasic.Controls.Add((Control)this.Label4);
            this.gbBasic.Controls.Add((Control)this.txtNameShort);
            this.gbBasic.Controls.Add((Control)this.Label3);
            this.gbBasic.Controls.Add((Control)this.txtNameFull);
            this.gbBasic.Controls.Add((Control)this.Label2);

            this.gbBasic.Location = new Point(96, 8);
            this.gbBasic.Name = "gbBasic";

            this.gbBasic.Size = new Size(248, 169);
            this.gbBasic.TabIndex = 11;
            this.gbBasic.TabStop = false;
            this.gbBasic.Text = "Basic:";

            this.txtInternal.Location = new Point(84, 68);
            this.txtInternal.Name = "txtInternal";

            this.txtInternal.Size = new Size(156, 20);
            this.txtInternal.TabIndex = 21;

            this.Label9.Location = new Point(8, 68);
            this.Label9.Name = "Label9";

            this.Label9.Size = new Size(72, 20);
            this.Label9.TabIndex = 20;
            this.Label9.Text = "Internal:";
            this.Label9.TextAlign = ContentAlignment.MiddleRight;

            this.Label7.Location = new Point(134, 140);
            this.Label7.Name = "Label7";

            this.Label7.Size = new Size(56, 20);
            this.Label7.TabIndex = 19;
            this.Label7.Text = "to";
            this.Label7.TextAlign = ContentAlignment.MiddleCenter;

            this.Label6.Location = new Point(6, 140);
            this.Label6.Name = "Label6";

            this.Label6.Size = new Size(74, 20);
            this.Label6.TabIndex = 18;
            this.Label6.Text = "Level range:";
            this.Label6.TextAlign = ContentAlignment.MiddleRight;

            this.udMinLevel.Location = new Point(84, 140);
            Decimal num = new Decimal(new int[4]
            {
        53,
        0,
        0,
        0
            });
            this.udMinLevel.Maximum = num;
            num = new Decimal(new int[4] { 1, 0, 0, 0 });
            this.udMinLevel.Minimum = num;
            this.udMinLevel.Name = "udMinLevel";

            this.udMinLevel.Size = new Size(44, 20);
            this.udMinLevel.TabIndex = 17;
            num = new Decimal(new int[4] { 1, 0, 0, 0 });
            this.udMinLevel.Value = num;

            this.udMaxLevel.Location = new Point(196, 140);
            num = new Decimal(new int[4] { 53, 0, 0, 0 });
            this.udMaxLevel.Maximum = num;
            num = new Decimal(new int[4] { 1, 0, 0, 0 });
            this.udMaxLevel.Minimum = num;
            this.udMaxLevel.Name = "udMaxLevel";

            this.udMaxLevel.Size = new Size(44, 20);
            this.udMaxLevel.TabIndex = 16;
            num = new Decimal(new int[4] { 53, 0, 0, 0 });
            this.udMaxLevel.Value = num;

            this.txtDesc.Location = new Point(84, 94);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";

            this.txtDesc.Size = new Size(156, 40);
            this.txtDesc.TabIndex = 15;

            this.Label4.Location = new Point(8, 98);
            this.Label4.Name = "Label4";

            this.Label4.Size = new Size(72, 20);
            this.Label4.TabIndex = 14;
            this.Label4.Text = "Description:";
            this.Label4.TextAlign = ContentAlignment.MiddleRight;

            this.txtNameShort.Location = new Point(84, 42);
            this.txtNameShort.Name = "txtNameShort";

            this.txtNameShort.Size = new Size(156, 20);
            this.txtNameShort.TabIndex = 13;

            this.Label3.Location = new Point(8, 42);
            this.Label3.Name = "Label3";

            this.Label3.Size = new Size(72, 20);
            this.Label3.TabIndex = 12;
            this.Label3.Text = "Short Name:";
            this.Label3.TextAlign = ContentAlignment.MiddleRight;

            this.txtNameFull.Location = new Point(84, 16);
            this.txtNameFull.Name = "txtNameFull";

            this.txtNameFull.Size = new Size(156, 20);
            this.txtNameFull.TabIndex = 11;

            this.Label2.Location = new Point(8, 16);
            this.Label2.Name = "Label2";

            this.Label2.Size = new Size(72, 20);
            this.Label2.TabIndex = 10;
            this.Label2.Text = "Full Name:";
            this.Label2.TextAlign = ContentAlignment.MiddleRight;
            this.btnImage.Image = (Image)componentResourceManager.GetObject("btnImage.Image");
            this.btnImage.ImageAlign = ContentAlignment.TopCenter;

            this.btnImage.Location = new Point(8, 12);
            this.btnImage.Name = "btnImage";

            this.btnImage.Size = new Size(80, 68);
            this.btnImage.TabIndex = 9;
            this.btnImage.Text = "ImageName";
            this.btnImage.TextAlign = ContentAlignment.BottomCenter;
            this.gbType.Controls.Add((Control)this.cbSubType);
            this.gbType.Controls.Add((Control)this.typeSet);
            this.gbType.Controls.Add((Control)this.typeIO);
            this.gbType.Controls.Add((Control)this.typeRegular);
            this.gbType.Controls.Add((Control)this.typeHO);

            this.gbType.Location = new Point(352, 8);
            this.gbType.Name = "gbType";

            this.gbType.Size = new Size(140, 169);
            this.gbType.TabIndex = 2;
            this.gbType.TabStop = false;
            this.gbType.Text = "Enhancement Type:";
            this.cbSubType.DropDownStyle = ComboBoxStyle.DropDownList;

            this.cbSubType.Location = new Point(8, 138);
            this.cbSubType.Name = "cbSubType";

            this.cbSubType.Size = new Size(124, 22);
            this.cbSubType.TabIndex = 54;
            this.tTip.SetToolTip((Control)this.cbSubType, "(Currently only apllicable to Stealth IOs");
            this.typeSet.Appearance = Appearance.Button;
            this.typeSet.CheckAlign = ContentAlignment.TopCenter;
            this.typeSet.Image = (Image)componentResourceManager.GetObject("typeSet.Image");
            this.typeSet.ImageAlign = ContentAlignment.TopCenter;

            this.typeSet.Location = new Point(72, 76);
            this.typeSet.Name = "typeSet";

            this.typeSet.Size = new Size(60, 56);
            this.typeSet.TabIndex = 53;
            this.typeSet.Text = "IO Set";
            this.typeSet.TextAlign = ContentAlignment.BottomCenter;
            this.typeIO.Appearance = Appearance.Button;
            this.typeIO.CheckAlign = ContentAlignment.TopCenter;
            this.typeIO.Image = (Image)componentResourceManager.GetObject("typeIO.Image");
            this.typeIO.ImageAlign = ContentAlignment.TopCenter;

            this.typeIO.Location = new Point(72, 16);
            this.typeIO.Name = "typeIO";

            this.typeIO.Size = new Size(60, 56);
            this.typeIO.TabIndex = 52;
            this.typeIO.Text = "Invention";
            this.typeIO.TextAlign = ContentAlignment.BottomCenter;
            this.typeRegular.Appearance = Appearance.Button;
            this.typeRegular.CheckAlign = ContentAlignment.TopCenter;
            this.typeRegular.Image = (Image)componentResourceManager.GetObject("typeRegular.Image");
            this.typeRegular.ImageAlign = ContentAlignment.TopCenter;

            this.typeRegular.Location = new Point(8, 16);
            this.typeRegular.Name = "typeRegular";

            this.typeRegular.Size = new Size(60, 56);
            this.typeRegular.TabIndex = 50;
            this.typeRegular.Text = "Regular";
            this.typeRegular.TextAlign = ContentAlignment.BottomCenter;
            this.typeHO.Appearance = Appearance.Button;
            this.typeHO.CheckAlign = ContentAlignment.TopCenter;
            this.typeHO.Image = (Image)componentResourceManager.GetObject("typeHO.Image");
            this.typeHO.ImageAlign = ContentAlignment.TopCenter;

            this.typeHO.Location = new Point(8, 76);
            this.typeHO.Name = "typeHO";

            this.typeHO.Size = new Size(60, 56);
            this.typeHO.TabIndex = 51;
            this.typeHO.Text = "Special";
            this.typeHO.TextAlign = ContentAlignment.BottomCenter;
            this.cbSet.DropDownStyle = ComboBoxStyle.DropDownList;

            this.cbSet.Location = new Point(8, 20);
            this.cbSet.Name = "cbSet";

            this.cbSet.Size = new Size(168, 22);
            this.cbSet.TabIndex = 13;
            this.gbSet.Controls.Add((Control)this.chkSuperior);
            this.gbSet.Controls.Add((Control)this.pbSet);
            this.gbSet.Controls.Add((Control)this.cbSet);
            this.gbSet.Controls.Add((Control)this.chkUnique);

            this.gbSet.Location = new Point(496, 8);
            this.gbSet.Name = "gbSet";

            this.gbSet.Size = new Size(184, 119);
            this.gbSet.TabIndex = 14;
            this.gbSet.TabStop = false;
            this.gbSet.Text = "Invention Origin Set:";

            this.chkSuperior.Location = new Point(60, 94);
            this.chkSuperior.Name = "chkSuperior";

            this.chkSuperior.Size = new Size(84, 16);
            this.chkSuperior.TabIndex = 21;
            this.chkSuperior.Text = "Superior";

            this.pbSet.Location = new Point(12, 52);
            this.pbSet.Name = "pbSet";

            this.pbSet.Size = new Size(30, 30);
            this.pbSet.TabIndex = 14;
            this.pbSet.TabStop = false;

            this.chkUnique.Location = new Point(60, 60);
            this.chkUnique.Name = "chkUnique";

            this.chkUnique.Size = new Size(84, 16);
            this.chkUnique.TabIndex = 20;
            this.chkUnique.Text = "Unique";
            this.gbEffects.Controls.Add((Control)this.btnDown);
            this.gbEffects.Controls.Add((Control)this.btnUp);
            this.gbEffects.Controls.Add((Control)this.rbBoth);
            this.gbEffects.Controls.Add((Control)this.rbDebuff);
            this.gbEffects.Controls.Add((Control)this.rbBuff);
            this.gbEffects.Controls.Add((Control)this.btnAutoFill);
            this.gbEffects.Controls.Add((Control)this.Label5);
            this.gbEffects.Controls.Add((Control)this.txtProb);
            this.gbEffects.Controls.Add((Control)this.Label1);
            this.gbEffects.Controls.Add((Control)this.btnEdit);
            this.gbEffects.Controls.Add((Control)this.btnAddFX);
            this.gbEffects.Controls.Add((Control)this.btnRemove);
            this.gbEffects.Controls.Add((Control)this.btnAdd);
            this.gbEffects.Controls.Add((Control)this.gbMod);
            this.gbEffects.Controls.Add((Control)this.lstSelected);
            this.gbEffects.Controls.Add((Control)this.lstAvailable);
            this.gbEffects.Controls.Add((Control)this.cbSched);
            this.gbEffects.Controls.Add((Control)this.lblSched);

            this.gbEffects.Location = new Point(4, 210);
            this.gbEffects.Name = "gbEffects";

            this.gbEffects.Size = new Size(584, 284);
            this.gbEffects.TabIndex = 15;
            this.gbEffects.TabStop = false;
            this.gbEffects.Text = "Effects:";

            this.btnDown.Location = new Point(188, 172);
            this.btnDown.Name = "btnDown";

            this.btnDown.Size = new Size(48, 20);
            this.btnDown.TabIndex = 32;
            this.btnDown.Text = "Down";

            this.btnUp.Location = new Point(188, 148);
            this.btnUp.Name = "btnUp";

            this.btnUp.Size = new Size(48, 20);
            this.btnUp.TabIndex = 31;
            this.btnUp.Text = "Up";
            this.rbBoth.Checked = true;

            this.rbBoth.Location = new Point(428, 228);
            this.rbBoth.Name = "rbBoth";

            this.rbBoth.Size = new Size(148, 16);
            this.rbBoth.TabIndex = 30;
            this.rbBoth.TabStop = true;
            this.rbBoth.Text = "Buff/Debuff Effects";
            this.tTip.SetToolTip((Control)this.rbBoth, "Apply to effects regardles of whether the Mag is positive or negative");

            this.rbDebuff.Location = new Point(428, 212);
            this.rbDebuff.Name = "rbDebuff";

            this.rbDebuff.Size = new Size(148, 16);
            this.rbDebuff.TabIndex = 29;
            this.rbDebuff.Text = "Debuff Effects";
            this.tTip.SetToolTip((Control)this.rbDebuff, "Apply only to effects with a negative Mag");

            this.rbBuff.Location = new Point(428, 196);
            this.rbBuff.Name = "rbBuff";

            this.rbBuff.Size = new Size(148, 16);
            this.rbBuff.TabIndex = 28;
            this.rbBuff.Text = "Buff Effects";
            this.tTip.SetToolTip((Control)this.rbBuff, "Apply only to effects with a positive Mag");

            this.btnAutoFill.Location = new Point(128, 24);
            this.btnAutoFill.Name = "btnAutoFill";

            this.btnAutoFill.Size = new Size(108, 32);
            this.btnAutoFill.TabIndex = 27;
            this.btnAutoFill.Text = "AutoFill Names";

            this.Label5.Location = new Point(196, 244);
            this.Label5.Name = "Label5";

            this.Label5.Size = new Size(28, 20);
            this.Label5.TabIndex = 26;
            this.Label5.Text = "(0-1)";
            this.Label5.TextAlign = ContentAlignment.MiddleLeft;

            this.txtProb.Location = new Point(156, 244);
            this.txtProb.Name = "txtProb";

            this.txtProb.Size = new Size(36, 20);
            this.txtProb.TabIndex = 25;
            this.txtProb.Text = "1";

            this.Label1.Location = new Point(8, 244);
            this.Label1.Name = "Label1";

            this.Label1.Size = new Size(148, 20);
            this.Label1.TabIndex = 24;
            this.Label1.Text = "Special Effect Probability:";
            this.Label1.TextAlign = ContentAlignment.MiddleRight;
            this.btnEdit.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);

            this.btnEdit.Location = new Point(424, 248);
            this.btnEdit.Name = "btnEdit";

            this.btnEdit.Size = new Size(152, 28);
            this.btnEdit.TabIndex = 23;
            this.btnEdit.Text = "Edit Selected...";
            this.btnAddFX.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);

            this.btnAddFX.Location = new Point(8, 208);
            this.btnAddFX.Name = "btnAddFX";

            this.btnAddFX.Size = new Size(228, 28);
            this.btnAddFX.TabIndex = 22;
            this.btnAddFX.Text = "Add Special Effect... ->";
            this.btnRemove.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);

            this.btnRemove.Location = new Point(240, 248);
            this.btnRemove.Name = "btnRemove";

            this.btnRemove.Size = new Size(176, 28);
            this.btnRemove.TabIndex = 21;
            this.btnRemove.Text = "Remove Selected";
            this.btnAdd.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);

            this.btnAdd.Location = new Point(128, 100);
            this.btnAdd.Name = "btnAdd";

            this.btnAdd.Size = new Size(108, 28);
            this.btnAdd.TabIndex = 20;
            this.btnAdd.Text = "Add ->";
            this.gbMod.Controls.Add((Control)this.rbMod4);
            this.gbMod.Controls.Add((Control)this.txtModOther);
            this.gbMod.Controls.Add((Control)this.rbModOther);
            this.gbMod.Controls.Add((Control)this.rbMod3);
            this.gbMod.Controls.Add((Control)this.rbMod2);
            this.gbMod.Controls.Add((Control)this.rbMod1);

            this.gbMod.Location = new Point(424, 44);
            this.gbMod.Name = "gbMod";

            this.gbMod.Size = new Size(152, 148);
            this.gbMod.TabIndex = 19;
            this.gbMod.TabStop = false;
            this.gbMod.Text = "Effect Modifier:";

            this.rbMod4.Location = new Point(12, 80);
            this.rbMod4.Name = "rbMod4";

            this.rbMod4.Size = new Size(128, 20);
            this.rbMod4.TabIndex = 5;
            this.rbMod4.Text = "0.4375 (4-Effect IO)";
            this.txtModOther.Enabled = false;

            this.txtModOther.Location = new Point(28, 120);
            this.txtModOther.Name = "txtModOther";

            this.txtModOther.Size = new Size(112, 20);
            this.txtModOther.TabIndex = 4;

            this.rbModOther.Location = new Point(12, 100);
            this.rbModOther.Name = "rbModOther";

            this.rbModOther.Size = new Size(128, 20);
            this.rbModOther.TabIndex = 3;
            this.rbModOther.Text = "Other";

            this.rbMod3.Location = new Point(12, 60);
            this.rbMod3.Name = "rbMod3";

            this.rbMod3.Size = new Size(128, 20);
            this.rbMod3.TabIndex = 2;
            this.rbMod3.Text = "0.5 (3-Effect IO)";

            this.rbMod2.Location = new Point(12, 40);
            this.rbMod2.Name = "rbMod2";

            this.rbMod2.Size = new Size(128, 20);
            this.rbMod2.TabIndex = 1;
            this.rbMod2.Text = "0.625 (2-Effect IO)";
            this.rbMod1.Checked = true;

            this.rbMod1.Location = new Point(12, 20);
            this.rbMod1.Name = "rbMod1";

            this.rbMod1.Size = new Size(128, 20);
            this.rbMod1.TabIndex = 0;
            this.rbMod1.TabStop = true;
            this.rbMod1.Text = "1.0 (No modifier)";
            this.lstSelected.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.lstSelected.ItemHeight = 14;

            this.lstSelected.Location = new Point(240, 20);
            this.lstSelected.Name = "lstSelected";

            this.lstSelected.Size = new Size(176, 214);
            this.lstSelected.TabIndex = 16;
            this.lstAvailable.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.lstAvailable.ItemHeight = 14;

            this.lstAvailable.Location = new Point(8, 20);
            this.lstAvailable.Name = "lstAvailable";

            this.lstAvailable.Size = new Size(116, 172);
            this.lstAvailable.TabIndex = 15;
            this.cbSched.DropDownStyle = ComboBoxStyle.DropDownList;

            this.cbSched.Location = new Point(488, 20);
            this.cbSched.Name = "cbSched";

            this.cbSched.Size = new Size(88, 22);
            this.cbSched.TabIndex = 14;

            this.lblSched.Location = new Point(424, 20);
            this.lblSched.Name = "lblSched";

            this.lblSched.Size = new Size(64, 20);
            this.lblSched.TabIndex = 3;
            this.lblSched.Text = "Schedule:";
            this.lblSched.TextAlign = ContentAlignment.MiddleRight;
            this.btnOK.DialogResult = DialogResult.OK;

            this.btnOK.Location = new Point(596, 434);
            this.btnOK.Name = "btnOK";

            this.btnOK.Size = new Size(84, 28);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "OK";
            this.btnCancel.DialogResult = DialogResult.Cancel;

            this.btnCancel.Location = new Point(596, 466);
            this.btnCancel.Name = "btnCancel";

            this.btnCancel.Size = new Size(84, 28);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.gbClass.Controls.Add((Control)this.lblClass);
            this.gbClass.Controls.Add((Control)this.pnlClassList);
            this.gbClass.Controls.Add((Control)this.pnlClass);

            this.gbClass.Location = new Point(596, 178);
            this.gbClass.Name = "gbClass";

            this.gbClass.Size = new Size(84, 252);
            this.gbClass.TabIndex = 18;
            this.gbClass.TabStop = false;
            this.gbClass.Text = "Class(es):";

            this.lblClass.Location = new Point(8, 232);
            this.lblClass.Name = "lblClass";

            this.lblClass.Size = new Size(68, 16);
            this.lblClass.TabIndex = 2;
            this.pnlClassList.BackColor = Color.Black;

            this.pnlClassList.Location = new Point(84, 16);
            this.pnlClassList.Name = "pnlClassList";

            this.pnlClassList.Size = new Size(180, 212);
            this.pnlClassList.TabIndex = 1;
            this.pnlClass.BackColor = Color.Black;

            this.pnlClass.Location = new Point(8, 16);
            this.pnlClass.Name = "pnlClass";

            this.pnlClass.Size = new Size(68, 212);
            this.pnlClass.TabIndex = 0;
            this.ImagePicker.Filter = "PNG Images|*.png";
            this.ImagePicker.Title = "Select Image File";

            this.btnNoImage.Location = new Point(8, 84);
            this.btnNoImage.Name = "btnNoImage";

            this.btnNoImage.Size = new Size(80, 20);
            this.btnNoImage.TabIndex = 19;
            this.btnNoImage.Text = "Clear Image";
            this.tTip.AutoPopDelay = 10000;
            this.tTip.InitialDelay = 250;
            this.tTip.ReshowDelay = 100;
            this.cbMutEx.DropDownStyle = ComboBoxStyle.DropDownList;

            this.cbMutEx.Location = new Point(504, 146);
            this.cbMutEx.Name = "cbMutEx";

            this.cbMutEx.Size = new Size(168, 22);
            this.cbMutEx.TabIndex = 21;
            this.tTip.SetToolTip((Control)this.cbMutEx, "(Currently only apllicable to Stealth IOs");
            this.cbRecipe.DropDownStyle = ComboBoxStyle.DropDownList;

            this.cbRecipe.Location = new Point(96, 183);
            this.cbRecipe.Name = "cbRecipe";

            this.cbRecipe.Size = new Size(248, 22);
            this.cbRecipe.TabIndex = 23;
            this.tTip.SetToolTip((Control)this.cbRecipe, "(Currently only apllicable to Stealth IOs");

            this.Label8.Location = new Point(496, 130);
            this.Label8.Name = "Label8";

            this.Label8.Size = new Size(80, 16);
            this.Label8.TabIndex = 22;
            this.Label8.Text = "MutEx Group:";

            this.Label10.Location = new Point(10, 183);
            this.Label10.Name = "Label10";

            this.Label10.Size = new Size(80, 22);
            this.Label10.TabIndex = 24;
            this.Label10.Text = "Recipe:";
            this.Label10.TextAlign = ContentAlignment.MiddleRight;

            this.btnEditPowerData.Location = new Point(352, 183);
            this.btnEditPowerData.Name = "btnEditPowerData";

            this.btnEditPowerData.Size = new Size(236, 22);
            this.btnEditPowerData.TabIndex = 25;
            this.btnEditPowerData.Text = "Edit Power_Mode Data";
            this.btnEditPowerData.UseVisualStyleBackColor = true;

            this.StaticIndex.Location = new Point(8, 146);
            this.StaticIndex.Name = "StaticIndex";

            this.StaticIndex.Size = new Size(82, 20);
            this.StaticIndex.TabIndex = 26;
            this.Label11.AutoSize = true;

            this.Label11.Location = new Point(8, 126);
            this.Label11.Name = "Label11";

            this.Label11.Size = new Size(63, 14);
            this.Label11.TabIndex = 27;
            this.Label11.Text = "Static Index";
            this.AcceptButton = (IButtonControl)this.btnOK;

            this.AutoScaleBaseSize = new Size(5, 13);
            this.CancelButton = (IButtonControl)this.btnCancel;

            this.ClientSize = new Size(686, 501);
            this.Controls.Add((Control)this.Label11);
            this.Controls.Add((Control)this.StaticIndex);
            this.Controls.Add((Control)this.btnEditPowerData);
            this.Controls.Add((Control)this.Label10);
            this.Controls.Add((Control)this.cbRecipe);
            this.Controls.Add((Control)this.Label8);
            this.Controls.Add((Control)this.btnNoImage);
            this.Controls.Add((Control)this.gbClass);
            this.Controls.Add((Control)this.btnCancel);
            this.Controls.Add((Control)this.btnOK);
            this.Controls.Add((Control)this.gbEffects);
            this.Controls.Add((Control)this.gbSet);
            this.Controls.Add((Control)this.gbBasic);
            this.Controls.Add((Control)this.gbType);
            this.Controls.Add((Control)this.btnImage);
            this.Controls.Add((Control)this.cbMutEx);
            this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = nameof(frmEnhData);
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edit [EnhancementName]";
            this.gbBasic.ResumeLayout(false);
            this.gbBasic.PerformLayout();
            this.udMinLevel.EndInit();
            this.udMaxLevel.EndInit();
            this.gbType.ResumeLayout(false);
            this.gbSet.ResumeLayout(false);
            ((ISupportInitialize)this.pbSet).EndInit();
            this.gbEffects.ResumeLayout(false);
            this.gbEffects.PerformLayout();
            this.gbMod.ResumeLayout(false);
            this.gbMod.PerformLayout();
            this.gbClass.ResumeLayout(false);
            this.ResumeLayout(false);
              //adding events
              if(!System.Diagnostics.Debugger.IsAttached || !this.IsInDesignMode() || !System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLowerInvariant().Contains("devenv"))
              {
                  this.StaticIndex.TextChanged += StaticIndex_TextChanged;
                  this.btnAdd.Click += btnAdd_Click;
                  this.btnAddFX.Click += btnAddFX_Click;
                  this.btnAutoFill.Click += btnAutoFill_Click;
                  this.btnCancel.Click += btnCancel_Click;
                  this.btnDown.Click += btnDown_Click;
                  this.btnEdit.Click += btnEdit_Click;
                  this.btnEditPowerData.Click += btnEditPowerData_Click;
                  this.btnImage.Click += btnImage_Click;
                  this.btnNoImage.Click += btnNoImage_Click;
                  this.btnOK.Click += btnOK_Click;
                  this.btnRemove.Click += btnRemove_Click;
                  this.btnUp.Click += btnUp_Click;
                  this.cbMutEx.SelectedIndexChanged += cbMutEx_SelectedIndexChanged;
                  this.cbRecipe.SelectedIndexChanged += cbRecipe_SelectedIndexChanged;
                  this.cbSched.SelectedIndexChanged += cbSched_SelectedIndexChanged;
                  this.cbSet.SelectedIndexChanged += cbSet_SelectedIndexChanged;
                  this.cbSubType.SelectedIndexChanged += cbSubType_SelectedIndexChanged;
                  this.chkSuperior.CheckedChanged += chkSuperior_CheckedChanged;
                  this.chkUnique.CheckedChanged += chkUnique_CheckedChanged;
                  this.lstAvailable.DoubleClick += lstAvailable_DoubleClick;
                  this.lstSelected.SelectedIndexChanged += lstSelected_SelectedIndexChanged;
                  
                  // pnlClass events
                  this.pnlClass.MouseMove += pnlClass_MouseMove;
                  this.pnlClass.Paint += pnlClass_Paint;
                  this.pnlClass.MouseDown += pnlClass_MouseDown;
                  
                  
                  // pnlClassList events
                  this.pnlClassList.MouseMove += pnlClassList_MouseMove;
                  this.pnlClassList.Paint += pnlClassList_Paint;
                  this.pnlClassList.MouseDown += pnlClassList_MouseDown;
                  
                  this.rbBoth.CheckedChanged += rbBuffDebuff_CheckedChanged;
                  this.rbBuff.CheckedChanged += rbBuffDebuff_CheckedChanged;
                  this.rbDebuff.CheckedChanged += rbBuffDebuff_CheckedChanged;
                  this.rbMod1.CheckedChanged += rbMod_CheckedChanged;
                  this.rbMod2.CheckedChanged += rbMod_CheckedChanged;
                  this.rbMod3.CheckedChanged += rbMod_CheckedChanged;
                  this.rbMod4.CheckedChanged += rbMod_CheckedChanged;
                  this.rbModOther.CheckedChanged += rbMod_CheckedChanged;
                  this.txtDesc.TextChanged += txtDesc_TextChanged;
                  this.txtInternal.TextChanged += txtInternal_TextChanged;
                  this.txtModOther.TextChanged += txtModOther_TextChanged;
                  this.txtNameFull.TextChanged += txtNameFull_TextChanged;
                  this.txtNameShort.TextChanged += txtNameShort_TextChanged;
                  
                  // txtProb events
                  this.txtProb.Leave += txtProb_Leave;
                  this.txtProb.TextChanged += txtProb_TextChanged;
                  
                  this.typeHO.CheckedChanged += type_CheckedChanged;
                  this.typeIO.CheckedChanged += type_CheckedChanged;
                  this.typeRegular.CheckedChanged += type_CheckedChanged;
                  this.typeSet.CheckedChanged += type_CheckedChanged;
                  
                  // udMaxLevel events
                  this.udMaxLevel.Leave += udMaxLevel_Leave;
                  this.udMaxLevel.ValueChanged += udMaxLevel_ValueChanged;
                  
                  
                  // udMinLevel events
                  this.udMinLevel.Leave += udMinLevel_Leave;
                  this.udMinLevel.ValueChanged += udMinLevel_ValueChanged;
                  
              }
              // finished with events
            this.PerformLayout();
        }

        public void ListSelectedEffects()
        {
            Enums.eEnhance eEnhance = Enums.eEnhance.None;
            Enums.eMez eMez = Enums.eMez.None;
            string[] names1 = Enum.GetNames(eEnhance.GetType());
            string[] names2 = Enum.GetNames(eMez.GetType());
            this.lstSelected.BeginUpdate();
            this.lstSelected.Items.Clear();
            int num = this.myEnh.Effect.Length - 1;
            for (int index = 0; index <= num; ++index)
            {
                if (this.myEnh.Effect[index].Mode == Enums.eEffMode.Enhancement)
                {
                    string str = names1[this.myEnh.Effect[index].Enhance.ID];
                    if (this.myEnh.Effect[index].Enhance.SubID > -1)
                        str = str + ":" + names2[this.myEnh.Effect[index].Enhance.SubID];
                    this.lstSelected.Items.Add((object)str);
                }
                else
                    this.lstSelected.Items.Add((object)("Special: " + this.myEnh.Effect[index].FX.BuildEffectString(false, "", false, false, false)));
            }
            this.lstSelected.EndUpdate();
        }

        void lstAvailable_DoubleClick(object sender, EventArgs e)

        {
            this.EffectList_Add();
        }

        void lstSelected_SelectedIndexChanged(object sender, EventArgs e)

        {
            this.DisplayEnhanceData();
            this.tTip.SetToolTip((Control)this.lstSelected, Conversions.ToString(this.lstSelected.SelectedItem));
        }

        public int MezPicker(int Index = 1)
        {
            Enums.eMez eMez = Enums.eMez.None;
            frmEnhMiniPick frmEnhMiniPick = new frmEnhMiniPick();
            string[] names = Enum.GetNames(eMez.GetType());
            int num1 = names.Length - 1;
            for (int index = 1; index <= num1; ++index)
                frmEnhMiniPick.lbList.Items.Add((object)names[index]);
            if (Index > -1 & Index < frmEnhMiniPick.lbList.Items.Count)
                frmEnhMiniPick.lbList.SelectedIndex = Index - 1;
            else
                frmEnhMiniPick.lbList.SelectedIndex = 0;
            int num2 = (int)frmEnhMiniPick.ShowDialog();
            return frmEnhMiniPick.lbList.SelectedIndex + 1;
        }

        void PickerExpand()

        {
            if (this.gbClass.Width == 84)
            {
                this.gbClass.Width = 272;
                this.gbClass.Left -= 188;
                this.lblClass.Width = 256;
            }
            else
            {
                this.gbClass.Width = 84;
                this.gbClass.Left = 596;
                this.lblClass.Width = this.pnlClass.Width;
            }
        }

        void pnlClass_MouseDown(object sender, MouseEventArgs e)

        {
            if (e.Button == MouseButtons.Right)
            {
                this.PickerExpand();
            }
            else
            {
                if (this.gbClass.Width <= 84 || this.Loading)
                    return;
                int num1 = -1;
                int num2 = -1;
                int num3 = 0;
                do
                {
                    if (e.X > (this.EnhPadding + this.ClassSize) * num3 & e.X < (this.EnhPadding + this.ClassSize) * (num3 + 1))
                        num1 = num3;
                    ++num3;
                }
                while (num3 <= 1);
                int num4 = 0;
                do
                {
                    if (e.Y > (this.EnhPadding + this.ClassSize) * num4 & e.Y < (this.EnhPadding + this.ClassSize) * (num4 + 1))
                        num2 = num4;
                    ++num4;
                }
                while (num4 <= 10);
                int num5 = num1 + num2 * 2;
                if (num5 < this.myEnh.ClassID.Length & num1 > -1 & num2 > -1)
                {
                    int[] numArray = new int[this.myEnh.ClassID.Length - 1 + 1];
                    int num6 = this.myEnh.ClassID.Length - 1;
                    for (int index = 0; index <= num6; ++index)
                        numArray[index] = this.myEnh.ClassID[index];
                    int index1 = 0;
                    this.myEnh.ClassID = new int[this.myEnh.ClassID.Length - 2 + 1];
                    int num7 = numArray.Length - 1;
                    for (int index2 = 0; index2 <= num7; ++index2)
                    {
                        if (index2 != num5)
                        {
                            this.myEnh.ClassID[index1] = numArray[index2];
                            ++index1;
                        }
                    }
                    Array.Sort<int>(this.myEnh.ClassID);
                    this.DrawClasses();
                }
            }
        }

        void pnlClass_MouseMove(object sender, MouseEventArgs e)

        {
            int num1 = -1;
            int num2 = -1;
            int num3 = 0;
            do
            {
                if (e.X > (this.EnhPadding + this.ClassSize) * num3 & e.X < (this.EnhPadding + this.ClassSize) * (num3 + 1))
                    num1 = num3;
                ++num3;
            }
            while (num3 <= 1);
            int num4 = 0;
            do
            {
                if (e.Y > (this.EnhPadding + this.ClassSize) * num4 & e.Y < (this.EnhPadding + this.ClassSize) * (num4 + 1))
                    num2 = num4;
                ++num4;
            }
            while (num4 <= 10);
            int index = num1 + num2 * 2;
            if (index < this.myEnh.ClassID.Length & num1 > -1 & num2 > -1)
            {
                if (this.gbClass.Width < 100)
                    this.lblClass.Text = DatabaseAPI.Database.EnhancementClasses[this.myEnh.ClassID[index]].ShortName;
                else
                    this.lblClass.Text = DatabaseAPI.Database.EnhancementClasses[this.myEnh.ClassID[index]].Name;
            }
            else
                this.lblClass.Text = "";
        }

        void pnlClass_Paint(object sender, PaintEventArgs e)

        {
            if (this.bxClass == null)
                return;
            e.Graphics.DrawImageUnscaled((Image)this.bxClass.Bitmap, 0, 0);
        }

        void pnlClassList_MouseDown(object sender, MouseEventArgs e)

        {
            if (e.Button == MouseButtons.Right)
            {
                this.PickerExpand();
            }
            else
            {
                if (this.gbClass.Width <= 84 || this.Loading)
                    return;
                int num1 = -1;
                int num2 = -1;
                int num3 = this.EnhAcross - 1;
                for (int index = 0; index <= num3; ++index)
                {
                    if (e.X > (this.EnhPadding + 30) * index & e.X < (this.EnhPadding + 30) * (index + 1))
                        num1 = index;
                }
                int num4 = 0;
                do
                {
                    if (e.Y > (this.EnhPadding + 30) * num4 & e.Y < (this.EnhPadding + 30) * (num4 + 1))
                        num2 = num4;
                    ++num4;
                }
                while (num4 <= 10);
                int num5 = num1 + num2 * this.EnhAcross;
                if (num5 < DatabaseAPI.Database.EnhancementClasses.Length & num1 > -1 & num2 > -1)
                {
                    bool flag = false;
                    int num6 = this.myEnh.ClassID.Length - 1;
                    for (int index = 0; index <= num6; ++index)
                    {
                        if (this.myEnh.ClassID[index] == num5)
                            flag = true;
                    }
                    if (!flag)
                    {
                        IEnhancement enh = this.myEnh;
                        int[] numArray = (int[])Utils.CopyArray((Array)enh.ClassID, (Array)new int[this.myEnh.ClassID.Length + 1]);
                        enh.ClassID = numArray;
                        this.myEnh.ClassID[this.myEnh.ClassID.Length - 1] = num5;
                        Array.Sort<int>(this.myEnh.ClassID);
                        this.DrawClasses();
                    }
                }
            }
        }

        void pnlClassList_MouseMove(object sender, MouseEventArgs e)

        {
            int num1 = -1;
            int num2 = -1;
            int num3 = this.EnhAcross - 1;
            for (int index = 0; index <= num3; ++index)
            {
                if (e.X > (this.EnhPadding + 30) * index & e.X < (this.EnhPadding + 30) * (index + 1))
                    num1 = index;
            }
            int num4 = 0;
            do
            {
                if (e.Y > (this.EnhPadding + 30) * num4 & e.Y < (this.EnhPadding + 30) * (num4 + 1))
                    num2 = num4;
                ++num4;
            }
            while (num4 <= 10);
            int index1 = num1 + num2 * this.EnhAcross;
            if (index1 < DatabaseAPI.Database.EnhancementClasses.Length & num1 > -1 & num2 > -1)
                this.lblClass.Text = DatabaseAPI.Database.EnhancementClasses[index1].Name;
            else
                this.lblClass.Text = string.Empty;
        }

        void pnlClassList_Paint(object sender, PaintEventArgs e)

        {
            if (this.bxClassList == null)
                return;
            e.Graphics.DrawImageUnscaled((Image)this.bxClassList.Bitmap, 0, 0);
        }

        void rbBuffDebuff_CheckedChanged(object sender, EventArgs e)

        {
            if (this.Loading || this.lstSelected.SelectedIndex <= -1)
                return;
            int selectedIndex = this.lstSelected.SelectedIndex;
            if (this.myEnh.Effect[selectedIndex].Mode == Enums.eEffMode.Enhancement)
            {
                if (this.rbBuff.Checked)
                    this.myEnh.Effect[selectedIndex].BuffMode = Enums.eBuffDebuff.BuffOnly;
                else if (this.rbDebuff.Checked)
                    this.myEnh.Effect[selectedIndex].BuffMode = Enums.eBuffDebuff.DeBuffOnly;
                else if (this.rbBoth.Checked)
                    this.myEnh.Effect[selectedIndex].BuffMode = Enums.eBuffDebuff.Any;
            }
        }

        void rbMod_CheckedChanged(object sender, EventArgs e)

        {
            if (this.lstSelected.SelectedIndex <= -1)
                return;
            int selectedIndex = this.lstSelected.SelectedIndex;
            if (this.myEnh.Effect[selectedIndex].Mode == Enums.eEffMode.Enhancement)
            {
                this.txtModOther.Enabled = false;
                if (this.rbModOther.Checked)
                {
                    this.txtModOther.Enabled = true;
                    this.myEnh.Effect[selectedIndex].Multiplier = (float)Conversion.Val(this.txtModOther.Text);
                    this.txtModOther.SelectAll();
                    this.txtModOther.Select();
                }
                else if (this.rbMod1.Checked)
                    this.myEnh.Effect[selectedIndex].Multiplier = 1f;
                else if (this.rbMod2.Checked)
                    this.myEnh.Effect[selectedIndex].Multiplier = 0.625f;
                else if (this.rbMod3.Checked)
                    this.myEnh.Effect[selectedIndex].Multiplier = 0.5f;
                else if (this.rbMod4.Checked)
                    this.myEnh.Effect[selectedIndex].Multiplier = 7f / 16f;
            }
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

        public void SetTypeIcons()
        {
            ExtendedBitmap extendedBitmap1 = new ExtendedBitmap(30, 30);
            ExtendedBitmap extendedBitmap2 = !(this.myEnh.Image != "") ? new ExtendedBitmap(30, 30) : new ExtendedBitmap(I9Gfx.GetEnhancementsPath() + this.myEnh.Image);
            extendedBitmap1.Graphics.Clear(Color.Transparent);
            extendedBitmap1.Graphics.DrawImage((Image)I9Gfx.Borders.Bitmap, extendedBitmap2.ClipRect, I9Gfx.GetOverlayRect(I9Gfx.ToGfxGrade(Enums.eType.Normal)), GraphicsUnit.Pixel);
            extendedBitmap1.Graphics.DrawImage((Image)extendedBitmap2.Bitmap, 0, 0);
            this.typeRegular.Image = (Image)new Bitmap((Image)extendedBitmap1.Bitmap);
            extendedBitmap1.Graphics.Clear(Color.Transparent);
            extendedBitmap1.Graphics.DrawImage((Image)I9Gfx.Borders.Bitmap, extendedBitmap2.ClipRect, I9Gfx.GetOverlayRect(I9Gfx.ToGfxGrade(Enums.eType.InventO)), GraphicsUnit.Pixel);
            extendedBitmap1.Graphics.DrawImage((Image)extendedBitmap2.Bitmap, 0, 0);
            this.typeIO.Image = (Image)new Bitmap((Image)extendedBitmap1.Bitmap);
            extendedBitmap1.Graphics.Clear(Color.Transparent);
            extendedBitmap1.Graphics.DrawImage((Image)I9Gfx.Borders.Bitmap, extendedBitmap2.ClipRect, I9Gfx.GetOverlayRect(I9Gfx.ToGfxGrade(Enums.eType.SpecialO)), GraphicsUnit.Pixel);
            extendedBitmap1.Graphics.DrawImage((Image)extendedBitmap2.Bitmap, 0, 0);
            this.typeHO.Image = (Image)new Bitmap((Image)extendedBitmap1.Bitmap);
            extendedBitmap1.Graphics.Clear(Color.Transparent);
            extendedBitmap1.Graphics.DrawImage((Image)I9Gfx.Borders.Bitmap, extendedBitmap2.ClipRect, I9Gfx.GetOverlayRect(I9Gfx.ToGfxGrade(Enums.eType.SetO)), GraphicsUnit.Pixel);
            extendedBitmap1.Graphics.DrawImage((Image)extendedBitmap2.Bitmap, 0, 0);
            this.typeSet.Image = (Image)new Bitmap((Image)extendedBitmap1.Bitmap);
        }

        void StaticIndex_TextChanged(object sender, EventArgs e)

        {
            this.myEnh.StaticIndex = Conversions.ToInteger(this.StaticIndex.Text);
        }

        void txtDesc_TextChanged(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            this.myEnh.Desc = this.txtDesc.Text;
        }

        void txtInternal_TextChanged(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            this.myEnh.UID = this.txtInternal.Text;
        }

        void txtModOther_TextChanged(object sender, EventArgs e)

        {
            if (this.lstSelected.SelectedIndex <= -1)
                return;
            int selectedIndex = this.lstSelected.SelectedIndex;
            if (this.myEnh.Effect[selectedIndex].Mode == Enums.eEffMode.Enhancement && this.rbModOther.Checked)
                this.myEnh.Effect[selectedIndex].Multiplier = (float)Conversion.Val(this.txtModOther.Text);
        }

        void txtNameFull_TextChanged(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            this.myEnh.Name = this.txtNameFull.Text;
            this.UpdateTitle();
        }

        void txtNameShort_TextChanged(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            this.myEnh.ShortName = this.txtNameShort.Text;
        }

        void txtProb_Leave(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            this.txtProb.Text = Conversions.ToString(this.myEnh.EffectChance);
        }

        void txtProb_TextChanged(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            float num = (float)Conversion.Val(this.txtProb.Text);
            if ((double)num > 1.0)
                num = 1f;
            if ((double)num < 0.0)
                num = 0.0f;
            this.myEnh.EffectChance = num;
        }

        void type_CheckedChanged(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            if (this.typeRegular.Checked)
            {
                this.myEnh.TypeID = Enums.eType.Normal;
                this.chkUnique.Checked = false;
                this.cbSubType.Enabled = false;
                this.cbSubType.SelectedIndex = -1;
                this.cbRecipe.SelectedIndex = -1;
                this.cbRecipe.Enabled = false;
            }
            else if (this.typeIO.Checked)
            {
                this.myEnh.TypeID = Enums.eType.InventO;
                this.chkUnique.Checked = false;
                this.cbSubType.Enabled = false;
                this.cbSubType.SelectedIndex = -1;
                this.cbRecipe.SelectedIndex = 0;
                this.cbRecipe.Enabled = true;
            }
            else if (this.typeHO.Checked)
            {
                this.myEnh.TypeID = Enums.eType.SpecialO;
                this.chkUnique.Checked = false;
                this.cbSubType.Enabled = true;
                this.cbSubType.SelectedIndex = 0;
                this.cbRecipe.SelectedIndex = -1;
                this.cbRecipe.Enabled = false;
            }
            else if (this.typeSet.Checked)
            {
                this.myEnh.TypeID = Enums.eType.SetO;
                this.cbSet.Select();
                this.cbSubType.Enabled = false;
                this.cbSubType.SelectedIndex = -1;
                this.cbRecipe.SelectedIndex = 0;
                this.cbRecipe.Enabled = true;
            }
            this.DisplaySet();
            this.UpdateTitle();
            this.DisplayIcon();
        }

        void udMaxLevel_Leave(object sender, EventArgs e)

        {
            this.SetMaxLevel((int)Math.Round(Conversion.Val(this.udMaxLevel.Text)));
            this.myEnh.LevelMax = Convert.ToInt32(Decimal.Subtract(this.udMaxLevel.Value, new Decimal(1)));
        }

        void udMaxLevel_ValueChanged(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            this.myEnh.LevelMax = Convert.ToInt32(Decimal.Subtract(this.udMaxLevel.Value, new Decimal(1)));
            this.udMinLevel.Maximum = this.udMaxLevel.Value;
        }

        void udMinLevel_Leave(object sender, EventArgs e)

        {
            this.SetMinLevel((int)Math.Round(Conversion.Val(this.udMinLevel.Text)));
            this.myEnh.LevelMin = Convert.ToInt32(Decimal.Subtract(this.udMinLevel.Value, new Decimal(1)));
        }

        void udMinLevel_ValueChanged(object sender, EventArgs e)

        {
            if (this.Loading)
                return;
            this.myEnh.LevelMin = Convert.ToInt32(Decimal.Subtract(this.udMinLevel.Value, new Decimal(1)));
            this.udMaxLevel.Minimum = this.udMinLevel.Value;
        }

        public void UpdateTitle()
        {
            string str1 = "Edit ";
            string str2;
            switch (this.myEnh.TypeID)
            {
                case Enums.eType.InventO:
                    str2 = str1 + "Invention: ";
                    break;
                case Enums.eType.SpecialO:
                    str2 = str1 + "HO: ";
                    break;
                case Enums.eType.SetO:
                    str2 = this.myEnh.nIDSet > -1 ? str1 + DatabaseAPI.Database.EnhancementSets[this.myEnh.nIDSet].DisplayName + ": " : str1 + "Set Invention: ";
                    break;
                default:
                    str2 = str1 + "Enhancement: ";
                    break;
            }
            this.Text = str2 + this.myEnh.Name;
        }
    }
}
