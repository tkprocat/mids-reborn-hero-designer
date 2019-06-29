
using Base.Display;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Hero_Designer
{
  public class frmEditPowerset : Form
  {
        Button btnCancel;

        Button btnClearIcon;

        Button btnClose;

        Button btnIcon;

        ComboBox cbAT;

        ComboBox cbLinkGroup;

        ComboBox cbLinkSet;

        ComboBox cbMutexGroup;

    [AccessedThroughProperty("cbNameGroup")]
    ComboBox _cbNameGroup;

        ComboBox cbSetType;

        ComboBox cbTrunkGroup;

        ComboBox cbTrunkSet;

        CheckBox chkNoLink;

        CheckBox chkNoTrunk;
        ColumnHeader ColumnHeader1;
        ColumnHeader ColumnHeader2;
        ColumnHeader ColumnHeader3;
        GroupBox gbLink;
        GroupBox GroupBox1;
        GroupBox GroupBox2;
        GroupBox GroupBox3;
        GroupBox GroupBox4;
        GroupBox GroupBox5;
        OpenFileDialog ImagePicker;
        Label Label1;
        Label Label2;
        Label Label22;
        Label Label3;
        Label Label31;
        Label Label33;
        Label Label4;
        Label Label5;
        Label Label6;
        Label Label7;
        Label Label8;
        Label lblNameFull;
        Label lblNameUnique;

        ListBox lvMutexSets;
        ListView lvPowers;
        PictureBox picIcon;

        TextBox txtDesc;

        TextBox txtName;

    [AccessedThroughProperty("txtNameSet")]
    TextBox _txtNameSet;

    IContainer components;

    protected bool Loading;
    public IPowerset myPS;
    ComboBox cbNameGroup
    {
      get
      {
        return this._cbNameGroup;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] set
      {
        EventHandler eventHandler1 = new EventHandler(this.cbNameGroup_TextChanged);
        EventHandler eventHandler2 = new EventHandler(this.cbNameGroup_SelectedIndexChanged);
        EventHandler eventHandler3 = new EventHandler(this.cbNameGroup_Leave);
        if (this._cbNameGroup != null)
        {
          this._cbNameGroup.TextChanged -= eventHandler1;
          this._cbNameGroup.SelectedIndexChanged -= eventHandler2;
          this._cbNameGroup.Leave -= eventHandler3;
        }
        this._cbNameGroup = value;
        if (this._cbNameGroup == null)
          return;
        this._cbNameGroup.TextChanged += eventHandler1;
        this._cbNameGroup.SelectedIndexChanged += eventHandler2;
        this._cbNameGroup.Leave += eventHandler3;
      }
    }























    TextBox txtNameSet
    {
      get
      {
        return this._txtNameSet;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] set
      {
        EventHandler eventHandler1 = new EventHandler(this.txtNameSet_TextChanged);
        EventHandler eventHandler2 = new EventHandler(this.txtNameSet_Leave);
        if (this._txtNameSet != null)
        {
          this._txtNameSet.TextChanged -= eventHandler1;
          this._txtNameSet.Leave -= eventHandler2;
        }
        this._txtNameSet = value;
        if (this._txtNameSet == null)
          return;
        this._txtNameSet.TextChanged += eventHandler1;
        this._txtNameSet.Leave += eventHandler2;
      }
    }

    public frmEditPowerset(ref IPowerset iSet)
    {
      this.Load += new EventHandler(this.frmEditPowerset_Load);
      this.Loading = true;
      this.InitializeComponent();
      this.myPS = (IPowerset) new Powerset(iSet);
    }

    public void AddListItem(int Index)
    {
      this.lvPowers.Items.Add(new ListViewItem(new string[3]
      {
        Conversions.ToString(DatabaseAPI.Database.Power[this.myPS.Power[Index]].Level),
        DatabaseAPI.Database.Power[this.myPS.Power[Index]].DisplayName,
        DatabaseAPI.Database.Power[this.myPS.Power[Index]].DescShort
      }));
      this.lvPowers.Items[Index].Selected = true;
      this.lvPowers.Items[Index].EnsureVisible();
    }

    void btnCancel_Click(object sender, EventArgs e)

    {
      this.DialogResult = DialogResult.Cancel;
      this.Hide();
    }

    void btnClearIcon_Click(object sender, EventArgs e)

    {
      this.myPS.ImageName = "";
      this.DisplayIcon();
    }

    void btnClose_Click(object sender, EventArgs e)

    {
      IPowerset ps = this.myPS;
      this.lblNameFull.Text = ps.GroupName + "." + ps.SetName;
      if (ps.GroupName == "" | ps.SetName == "")
      {
        int num1 = (int) Interaction.MsgBox((object) ("Powerset name '" + ps.FullName + " is invalid."), MsgBoxStyle.Exclamation, (object) "No Can Do");
      }
      else if (!frmEditPowerset.PowersetFullNameIsUnique(Conversions.ToString(ps.nID), -1))
      {
        int num2 = (int) Interaction.MsgBox((object) ("Powerset name '" + ps.FullName + " already exists, please enter a unique name."), MsgBoxStyle.Exclamation, (object) "No Can Do");
      }
      else
      {
        this.myPS.IsModified = true;
        this.DialogResult = DialogResult.OK;
        this.Hide();
      }
    }

    void btnIcon_Click(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.ImagePicker.InitialDirectory = I9Gfx.GetPowersetsPath();
      this.ImagePicker.FileName = this.myPS.ImageName;
      if (this.ImagePicker.ShowDialog() == DialogResult.OK)
      {
        string str = FileIO.StripPath(this.ImagePicker.FileName);
        if (!File.Exists(FileIO.AddSlash(this.ImagePicker.InitialDirectory) + str))
        {
          int num = (int) Interaction.MsgBox((object) ("You must select an image from the " + I9Gfx.GetPowersetsPath() + " folder!\r\n\r\nIf you are adding a new image, you should copy it to the folder and then select it."), MsgBoxStyle.Information, (object) "Ah...");
        }
        else
        {
          this.myPS.ImageName = str;
          this.DisplayIcon();
        }
      }
    }

    string BuildFullName()

    {
      string str = this.cbNameGroup.Text + "." + this.txtNameSet.Text;
      this.lblNameFull.Text = str;
      this.myPS.FullName = str;
      this.myPS.SetName = this.txtNameSet.Text;
      return str;
    }

    void cbAT_SelectedIndexChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      if (this.cbAT.SelectedIndex > -1)
      {
        this.myPS.nArchetype = this.cbAT.SelectedIndex - 1;
        this.myPS.ATClass = DatabaseAPI.UidFromNidClass(this.cbAT.SelectedIndex - 1);
      }
      else
        this.myPS.nArchetype = -1;
    }

    void cbLinkGroup_SelectedIndexChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.FillLinkSetCombo();
    }

    void cbLinkSet_SelectedIndexChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      if (this.chkNoLink.Checked)
      {
        this.myPS.UIDLinkSecondary = "";
        this.myPS.nIDLinkSecondary = -1;
      }
      else if (this.cbLinkSet.SelectedIndex > -1)
      {
        string uidPowerset = this.cbLinkGroup.Text + "." + this.cbLinkSet.Text;
        int num = DatabaseAPI.NidFromUidPowerset(uidPowerset);
        this.myPS.UIDLinkSecondary = uidPowerset;
        this.myPS.nIDLinkSecondary = num;
      }
    }

    void cbMutexGroup_SelectionChangeCommitted(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.ListMutexSets();
    }

    void cbNameGroup_Leave(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.DisplayNameData();
    }

    void cbNameGroup_SelectedIndexChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.BuildFullName();
    }

    void cbNameGroup_TextChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.BuildFullName();
    }

    void cbSetType_SelectedIndexChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      if (this.cbSetType.SelectedIndex > -1)
        this.myPS.SetType = (Enums.ePowerSetType) this.cbSetType.SelectedIndex;
      if (this.myPS.SetType == Enums.ePowerSetType.Primary)
      {
        this.gbLink.Enabled = true;
      }
      else
      {
        this.gbLink.Enabled = false;
        this.cbLinkSet.SelectedIndex = -1;
        this.cbLinkGroup.SelectedIndex = -1;
        this.chkNoLink.Checked = true;
      }
    }

    void cbTrunkGroup_SelectedIndexChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.FillTrunkSetCombo();
    }

    void cbTrunkSet_SelectedIndexChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      if (this.chkNoTrunk.Checked)
      {
        this.myPS.UIDTrunkSet = "";
        this.myPS.nIDTrunkSet = -1;
      }
      else if (this.cbTrunkSet.SelectedIndex > -1)
      {
        string uidPowerset = this.cbTrunkGroup.Text + "." + this.cbTrunkSet.Text;
        int num = DatabaseAPI.NidFromUidPowerset(uidPowerset);
        this.myPS.UIDTrunkSet = uidPowerset;
        this.myPS.nIDTrunkSet = num;
      }
    }

    void chkNoLink_CheckedChanged(object sender, EventArgs e)

    {
      this.cbLinkSet_SelectedIndexChanged((object) this, new EventArgs());
    }

    void chkNoTrunk_CheckedChanged(object sender, EventArgs e)

    {
      this.cbTrunkSet_SelectedIndexChanged((object) this, new EventArgs());
    }

    public void DisplayIcon()
    {
      if (this.myPS.ImageName != "")
      {
        this.picIcon.Image = (Image) new Bitmap((Image) new ExtendedBitmap(I9Gfx.GetPowersetsPath() + this.myPS.ImageName).Bitmap);
        this.btnIcon.Text = this.myPS.ImageName;
      }
      else
      {
        this.picIcon.Image = (Image) new Bitmap((Image) new ExtendedBitmap(30, 30).Bitmap);
        this.btnIcon.Text = "Select Icon";
      }
    }

    void DisplayNameData()

    {
      IPowerset ps = this.myPS;
      this.lblNameFull.Text = this.BuildFullName();
      if (ps.GroupName == "" | ps.SetName == "")
        this.lblNameUnique.Text = "This name is invalid.";
      else if (frmEditPowerset.PowersetFullNameIsUnique(Conversions.ToString(ps.nID), -1))
        this.lblNameUnique.Text = "This name is unique.";
      else
        this.lblNameUnique.Text = "This name is NOT unique.";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    void FillLinkGroupCombo()

    {
      this.cbLinkGroup.BeginUpdate();
      this.cbLinkGroup.Items.Clear();
      foreach (object key in (IEnumerable<string>) DatabaseAPI.Database.PowersetGroups.Keys)
        this.cbLinkGroup.Items.Add(key);
      this.cbLinkGroup.EndUpdate();
      if (!(this.myPS.UIDLinkSecondary != ""))
        return;
      int index = DatabaseAPI.NidFromUidPowerset(this.myPS.UIDLinkSecondary);
      if (index > -1)
        this.cbLinkGroup.SelectedValue = (object) DatabaseAPI.Database.Powersets[index].GroupName;
    }

    void FillLinkSetCombo()

    {
      this.cbLinkSet.BeginUpdate();
      this.cbLinkSet.Items.Clear();
      if (this.cbLinkGroup.SelectedIndex > -1)
      {
        int index1 = DatabaseAPI.NidFromUidPowerset(this.myPS.UIDLinkSecondary);
        int[] indexesByGroupName = DatabaseAPI.GetPowersetIndexesByGroupName(this.cbLinkGroup.SelectedText);
        int num = indexesByGroupName.Length - 1;
        for (int index2 = 0; index2 <= num; ++index2)
        {
          this.cbLinkSet.Items.Add((object) DatabaseAPI.Database.Powersets[indexesByGroupName[index2]].SetName);
          if (index1 > -1 && DatabaseAPI.Database.Powersets[indexesByGroupName[index2]].SetName == DatabaseAPI.Database.Powersets[index1].SetName)
            index1 = index2;
        }
        this.cbLinkSet.SelectedIndex = index1;
      }
      this.cbLinkSet.EndUpdate();
    }

    void FillTrunkGroupCombo()

    {
      this.cbTrunkGroup.BeginUpdate();
      this.cbTrunkGroup.Items.Clear();
      foreach (object key in (IEnumerable<string>) DatabaseAPI.Database.PowersetGroups.Keys)
        this.cbTrunkGroup.Items.Add(key);
      this.cbTrunkGroup.EndUpdate();
      if (!(this.myPS.UIDTrunkSet != ""))
        return;
      int index = DatabaseAPI.NidFromUidPowerset(this.myPS.UIDTrunkSet);
      if (index > -1)
        this.cbTrunkGroup.SelectedValue = (object) DatabaseAPI.Database.Powersets[index].GroupName;
    }

    void FillTrunkSetCombo()

    {
      this.cbTrunkSet.BeginUpdate();
      this.cbTrunkSet.Items.Clear();
      if (this.cbTrunkGroup.SelectedIndex > -1)
      {
        int index1 = DatabaseAPI.NidFromUidPowerset(this.myPS.UIDTrunkSet);
        int[] indexesByGroupName = DatabaseAPI.GetPowersetIndexesByGroupName(this.cbTrunkGroup.SelectedText);
        int num = indexesByGroupName.Length - 1;
        for (int index2 = 0; index2 <= num; ++index2)
        {
          this.cbTrunkSet.Items.Add((object) DatabaseAPI.Database.Powersets[indexesByGroupName[index2]].SetName);
          if (index1 > -1 && DatabaseAPI.Database.Powersets[indexesByGroupName[index2]].SetName == DatabaseAPI.Database.Powersets[index1].SetName)
            index1 = index2;
        }
        this.cbTrunkSet.SelectedIndex = index1;
      }
      this.cbTrunkSet.EndUpdate();
    }

    void frmEditPowerset_Load(object sender, EventArgs e)

    {
      Enums.ePowerSetType ePowerSetType = Enums.ePowerSetType.None;
      this.ListPowers();
      this.txtName.Text = this.myPS.DisplayName;
      this.cbNameGroup.BeginUpdate();
      this.cbNameGroup.Items.Clear();
      foreach (object key in (IEnumerable<string>) DatabaseAPI.Database.PowersetGroups.Keys)
        this.cbNameGroup.Items.Add(key);
      this.cbNameGroup.EndUpdate();
      this.cbNameGroup.Text = this.myPS.GroupName;
      this.txtNameSet.Text = this.myPS.SetName;
      this.txtDesc.Text = this.myPS.Description;
      this.FillTrunkGroupCombo();
      this.FillTrunkSetCombo();
      this.chkNoTrunk.Checked = this.myPS.UIDTrunkSet == "";
      this.FillLinkGroupCombo();
      this.FillLinkSetCombo();
      this.chkNoLink.Checked = this.myPS.UIDLinkSecondary == "";
      if (this.myPS.SetType == Enums.ePowerSetType.Primary)
      {
        this.gbLink.Enabled = true;
      }
      else
      {
        this.gbLink.Enabled = false;
        this.cbLinkSet.SelectedIndex = -1;
        this.cbLinkGroup.SelectedIndex = -1;
        this.chkNoLink.Checked = true;
      }
      this.DisplayIcon();
      this.cbAT.BeginUpdate();
      this.cbAT.Items.Clear();
      this.cbAT.Items.Add((object) "All / None");
      int num = DatabaseAPI.Database.Classes.Length - 1;
      for (int index = 0; index <= num; ++index)
        this.cbAT.Items.Add((object) DatabaseAPI.Database.Classes[index].DisplayName);
      this.cbAT.EndUpdate();
      this.cbAT.SelectedIndex = this.myPS.nArchetype + 1;
      this.cbSetType.BeginUpdate();
      this.cbSetType.Items.Clear();
      this.cbSetType.Items.AddRange((object[]) Enum.GetNames(ePowerSetType.GetType()));
      this.cbSetType.EndUpdate();
      this.cbSetType.SelectedIndex = (int) this.myPS.SetType;
      this.ListMutexGroups();
      this.ListMutexSets();
      this.Loading = false;
      this.DisplayNameData();
    }

    [DebuggerStepThrough]
    void InitializeComponent()

    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (frmEditPowerset));
      this.txtName = new TextBox();
      this.Label1 = new Label();
      this.cbAT = new ComboBox();
      this.Label2 = new Label();
      this.Label3 = new Label();
      this.cbSetType = new ComboBox();
      this.btnIcon = new Button();
      this.picIcon = new PictureBox();
      this.lvPowers = new ListView();
      this.ColumnHeader3 = new ColumnHeader();
      this.ColumnHeader1 = new ColumnHeader();
      this.ColumnHeader2 = new ColumnHeader();
      this.Label4 = new Label();
      this.btnClose = new Button();
      this.btnClearIcon = new Button();
      this.ImagePicker = new OpenFileDialog();
      this.lblNameUnique = new Label();
      this.lblNameFull = new Label();
      this.cbNameGroup = new ComboBox();
      this.Label22 = new Label();
      this.txtNameSet = new TextBox();
      this.Label33 = new Label();
      this.GroupBox1 = new GroupBox();
      this.GroupBox2 = new GroupBox();
      this.btnCancel = new Button();
      this.GroupBox3 = new GroupBox();
      this.txtDesc = new TextBox();
      this.GroupBox4 = new GroupBox();
      this.chkNoTrunk = new CheckBox();
      this.cbTrunkSet = new ComboBox();
      this.cbTrunkGroup = new ComboBox();
      this.Label5 = new Label();
      this.Label31 = new Label();
      this.gbLink = new GroupBox();
      this.chkNoLink = new CheckBox();
      this.cbLinkSet = new ComboBox();
      this.cbLinkGroup = new ComboBox();
      this.Label6 = new Label();
      this.Label7 = new Label();
      this.GroupBox5 = new GroupBox();
      this.lvMutexSets = new ListBox();
      this.Label8 = new Label();
      this.cbMutexGroup = new ComboBox();
      ((ISupportInitialize) this.picIcon).BeginInit();
      this.GroupBox1.SuspendLayout();
      this.GroupBox2.SuspendLayout();
      this.GroupBox3.SuspendLayout();
      this.GroupBox4.SuspendLayout();
      this.gbLink.SuspendLayout();
      this.GroupBox5.SuspendLayout();
      this.SuspendLayout();

      this.txtName.Location = new Point(110, 16);
      this.txtName.Name = "txtName";

      this.txtName.Size = new Size(196, 20);
      this.txtName.TabIndex = 0;
      this.txtName.Text = "TextBox1";

      this.Label1.Location = new Point(6, 16);
      this.Label1.Name = "Label1";

      this.Label1.Size = new Size(100, 20);
      this.Label1.TabIndex = 1;
      this.Label1.Text = "Display Name:";
      this.Label1.TextAlign = ContentAlignment.MiddleRight;
      this.cbAT.DropDownStyle = ComboBoxStyle.DropDownList;

      this.cbAT.Location = new Point(403, 122);
      this.cbAT.Name = "cbAT";

      this.cbAT.Size = new Size(124, 22);
      this.cbAT.TabIndex = 2;

      this.Label2.Location = new Point(336, 122);
      this.Label2.Name = "Label2";

      this.Label2.Size = new Size(63, 20);
      this.Label2.TabIndex = 3;
      this.Label2.Text = "Archetype:";
      this.Label2.TextAlign = ContentAlignment.MiddleRight;

      this.Label3.Location = new Point(336, 150);
      this.Label3.Name = "Label3";

      this.Label3.Size = new Size(63, 20);
      this.Label3.TabIndex = 5;
      this.Label3.Text = "Set Type:";
      this.Label3.TextAlign = ContentAlignment.MiddleRight;
      this.cbSetType.DropDownStyle = ComboBoxStyle.DropDownList;

      this.cbSetType.Location = new Point(403, 150);
      this.cbSetType.Name = "cbSetType";

      this.cbSetType.Size = new Size(124, 22);
      this.cbSetType.TabIndex = 4;

      this.btnIcon.Location = new Point(6, 52);
      this.btnIcon.Name = "btnIcon";

      this.btnIcon.Size = new Size(179, 20);
      this.btnIcon.TabIndex = 6;
      this.btnIcon.Text = "Select Icon";
      this.picIcon.BorderStyle = BorderStyle.FixedSingle;

      this.picIcon.Location = new Point(85, 22);
      this.picIcon.Name = "picIcon";

      this.picIcon.Size = new Size(20, 20);
      this.picIcon.TabIndex = 7;
      this.picIcon.TabStop = false;
      this.lvPowers.Columns.AddRange(new ColumnHeader[3]
      {
        this.ColumnHeader3,
        this.ColumnHeader1,
        this.ColumnHeader2
      });
      this.lvPowers.FullRowSelect = true;
      this.lvPowers.HideSelection = false;

      this.lvPowers.Location = new Point(12, 448);
      this.lvPowers.MultiSelect = false;
      this.lvPowers.Name = "lvPowers";

      this.lvPowers.Size = new Size(515, 121);
      this.lvPowers.TabIndex = 8;
      this.lvPowers.UseCompatibleStateImageBehavior = false;
      this.lvPowers.View = View.Details;
      this.ColumnHeader3.Text = "Level";
      this.ColumnHeader3.Width = 47;
      this.ColumnHeader1.Text = "Power";
      this.ColumnHeader1.Width = 124;
      this.ColumnHeader2.Text = "Short Description";
      this.ColumnHeader2.Width = 313;

      this.Label4.Location = new Point(9, 425);
      this.Label4.Name = "Label4";

      this.Label4.Size = new Size(100, 20);
      this.Label4.TabIndex = 9;
      this.Label4.Text = "Powers:";
      this.Label4.TextAlign = ContentAlignment.MiddleLeft;
      this.btnClose.DialogResult = DialogResult.OK;

      this.btnClose.Location = new Point(452, 575);
      this.btnClose.Name = "btnClose";

      this.btnClose.Size = new Size(75, 36);
      this.btnClose.TabIndex = 15;
      this.btnClose.Text = "OK";

      this.btnClearIcon.Location = new Point(6, 76);
      this.btnClearIcon.Name = "btnClearIcon";

      this.btnClearIcon.Size = new Size(179, 20);
      this.btnClearIcon.TabIndex = 16;
      this.btnClearIcon.Text = "Clear Icon";
      this.ImagePicker.Filter = "PNG Images|*.png";
      this.ImagePicker.Title = "Select Image File";

      this.lblNameUnique.Location = new Point(10, 131);
      this.lblNameUnique.Name = "lblNameUnique";

      this.lblNameUnique.Size = new Size(296, 20);
      this.lblNameUnique.TabIndex = 25;
      this.lblNameUnique.Text = "This name is unique.";
      this.lblNameUnique.TextAlign = ContentAlignment.MiddleCenter;
      this.lblNameFull.BorderStyle = BorderStyle.FixedSingle;

      this.lblNameFull.Location = new Point(13, 95);
      this.lblNameFull.Name = "lblNameFull";

      this.lblNameFull.Size = new Size(293, 32);
      this.lblNameFull.TabIndex = 24;
      this.lblNameFull.Text = "Group_Name.Powerset_Name";
      this.lblNameFull.TextAlign = ContentAlignment.MiddleLeft;
      this.cbNameGroup.FormattingEnabled = true;

      this.cbNameGroup.Location = new Point(110, 44);
      this.cbNameGroup.Name = "cbNameGroup";

      this.cbNameGroup.Size = new Size(196, 22);
      this.cbNameGroup.TabIndex = 20;

      this.Label22.Location = new Point(10, 44);
      this.Label22.Name = "Label22";

      this.Label22.Size = new Size(96, 20);
      this.Label22.TabIndex = 22;
      this.Label22.Text = "Group:";
      this.Label22.TextAlign = ContentAlignment.MiddleRight;

      this.txtNameSet.Location = new Point(110, 72);
      this.txtNameSet.Name = "txtNameSet";

      this.txtNameSet.Size = new Size(196, 20);
      this.txtNameSet.TabIndex = 21;
      this.txtNameSet.Text = "PowerName";

      this.Label33.Location = new Point(3, 72);
      this.Label33.Name = "Label33";

      this.Label33.Size = new Size(103, 20);
      this.Label33.TabIndex = 23;
      this.Label33.Text = "Powerset Name:";
      this.Label33.TextAlign = ContentAlignment.MiddleRight;
      this.GroupBox1.Controls.Add((Control) this.lblNameUnique);
      this.GroupBox1.Controls.Add((Control) this.lblNameFull);
      this.GroupBox1.Controls.Add((Control) this.cbNameGroup);
      this.GroupBox1.Controls.Add((Control) this.Label22);
      this.GroupBox1.Controls.Add((Control) this.txtNameSet);
      this.GroupBox1.Controls.Add((Control) this.Label33);
      this.GroupBox1.Controls.Add((Control) this.Label1);
      this.GroupBox1.Controls.Add((Control) this.txtName);

      this.GroupBox1.Location = new Point(12, 12);
      this.GroupBox1.Name = "GroupBox1";

      this.GroupBox1.Size = new Size(318, 160);
      this.GroupBox1.TabIndex = 26;
      this.GroupBox1.TabStop = false;
      this.GroupBox1.Text = "Powerset Name";
      this.GroupBox2.Controls.Add((Control) this.btnClearIcon);
      this.GroupBox2.Controls.Add((Control) this.picIcon);
      this.GroupBox2.Controls.Add((Control) this.btnIcon);

      this.GroupBox2.Location = new Point(336, 12);
      this.GroupBox2.Name = "GroupBox2";

      this.GroupBox2.Size = new Size(191, 102);
      this.GroupBox2.TabIndex = 27;
      this.GroupBox2.TabStop = false;
      this.GroupBox2.Text = "Icon";
      this.btnCancel.DialogResult = DialogResult.OK;

      this.btnCancel.Location = new Point(371, 575);
      this.btnCancel.Name = "btnCancel";

      this.btnCancel.Size = new Size(75, 36);
      this.btnCancel.TabIndex = 28;
      this.btnCancel.Text = "Cancel";
      this.GroupBox3.Controls.Add((Control) this.txtDesc);

      this.GroupBox3.Location = new Point(12, 178);
      this.GroupBox3.Name = "GroupBox3";

      this.GroupBox3.Size = new Size(515, 80);
      this.GroupBox3.TabIndex = 29;
      this.GroupBox3.TabStop = false;
      this.GroupBox3.Text = "Description";

      this.txtDesc.Location = new Point(6, 19);
      this.txtDesc.Multiline = true;
      this.txtDesc.Name = "txtDesc";
      this.txtDesc.ScrollBars = ScrollBars.Vertical;

      this.txtDesc.Size = new Size(503, 55);
      this.txtDesc.TabIndex = 1;
      this.txtDesc.Text = "TextBox1";
      this.GroupBox4.Controls.Add((Control) this.chkNoTrunk);
      this.GroupBox4.Controls.Add((Control) this.cbTrunkSet);
      this.GroupBox4.Controls.Add((Control) this.cbTrunkGroup);
      this.GroupBox4.Controls.Add((Control) this.Label5);
      this.GroupBox4.Controls.Add((Control) this.Label31);

      this.GroupBox4.Location = new Point(12, 264);
      this.GroupBox4.Name = "GroupBox4";

      this.GroupBox4.Size = new Size(515, 75);
      this.GroupBox4.TabIndex = 30;
      this.GroupBox4.TabStop = false;
      this.GroupBox4.Text = "Trunk Set:";

      this.chkNoTrunk.Location = new Point(279, 16);
      this.chkNoTrunk.Name = "chkNoTrunk";

      this.chkNoTrunk.Size = new Size(210, 50);
      this.chkNoTrunk.TabIndex = 17;
      this.chkNoTrunk.Text = "This power has no Trunk set";
      this.chkNoTrunk.UseVisualStyleBackColor = true;
      this.cbTrunkSet.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbTrunkSet.FormattingEnabled = true;

      this.cbTrunkSet.Location = new Point(68, 44);
      this.cbTrunkSet.Name = "cbTrunkSet";

      this.cbTrunkSet.Size = new Size(196, 22);
      this.cbTrunkSet.TabIndex = 14;
      this.cbTrunkGroup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbTrunkGroup.FormattingEnabled = true;

      this.cbTrunkGroup.Location = new Point(68, 16);
      this.cbTrunkGroup.Name = "cbTrunkGroup";

      this.cbTrunkGroup.Size = new Size(196, 22);
      this.cbTrunkGroup.TabIndex = 13;

      this.Label5.Location = new Point(10, 16);
      this.Label5.Name = "Label5";

      this.Label5.Size = new Size(54, 20);
      this.Label5.TabIndex = 15;
      this.Label5.Text = "Group:";
      this.Label5.TextAlign = ContentAlignment.MiddleRight;

      this.Label31.Location = new Point(13, 44);
      this.Label31.Name = "Label31";

      this.Label31.Size = new Size(49, 20);
      this.Label31.TabIndex = 16;
      this.Label31.Text = "Set:";
      this.Label31.TextAlign = ContentAlignment.MiddleRight;
      this.gbLink.Controls.Add((Control) this.chkNoLink);
      this.gbLink.Controls.Add((Control) this.cbLinkSet);
      this.gbLink.Controls.Add((Control) this.cbLinkGroup);
      this.gbLink.Controls.Add((Control) this.Label6);
      this.gbLink.Controls.Add((Control) this.Label7);

      this.gbLink.Location = new Point(12, 345);
      this.gbLink.Name = "gbLink";

      this.gbLink.Size = new Size(515, 75);
      this.gbLink.TabIndex = 31;
      this.gbLink.TabStop = false;
      this.gbLink.Text = "Linked Secondary";

      this.chkNoLink.Location = new Point(279, 16);
      this.chkNoLink.Name = "chkNoLink";

      this.chkNoLink.Size = new Size(210, 50);
      this.chkNoLink.TabIndex = 17;
      this.chkNoLink.Text = "No link";
      this.chkNoLink.UseVisualStyleBackColor = true;
      this.cbLinkSet.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbLinkSet.FormattingEnabled = true;

      this.cbLinkSet.Location = new Point(68, 44);
      this.cbLinkSet.Name = "cbLinkSet";

      this.cbLinkSet.Size = new Size(196, 22);
      this.cbLinkSet.TabIndex = 14;
      this.cbLinkGroup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbLinkGroup.FormattingEnabled = true;

      this.cbLinkGroup.Location = new Point(68, 16);
      this.cbLinkGroup.Name = "cbLinkGroup";

      this.cbLinkGroup.Size = new Size(196, 22);
      this.cbLinkGroup.TabIndex = 13;

      this.Label6.Location = new Point(10, 16);
      this.Label6.Name = "Label6";

      this.Label6.Size = new Size(54, 20);
      this.Label6.TabIndex = 15;
      this.Label6.Text = "Group:";
      this.Label6.TextAlign = ContentAlignment.MiddleRight;

      this.Label7.Location = new Point(13, 44);
      this.Label7.Name = "Label7";

      this.Label7.Size = new Size(49, 20);
      this.Label7.TabIndex = 16;
      this.Label7.Text = "Set:";
      this.Label7.TextAlign = ContentAlignment.MiddleRight;
      this.GroupBox5.Controls.Add((Control) this.lvMutexSets);
      this.GroupBox5.Controls.Add((Control) this.Label8);
      this.GroupBox5.Controls.Add((Control) this.cbMutexGroup);

      this.GroupBox5.Location = new Point(533, 12);
      this.GroupBox5.Name = "GroupBox5";

      this.GroupBox5.Size = new Size(253, 327);
      this.GroupBox5.TabIndex = 32;
      this.GroupBox5.TabStop = false;
      this.GroupBox5.Text = "Mutually Exclusive Sets";
      this.lvMutexSets.ItemHeight = 14;

      this.lvMutexSets.Location = new Point(9, 72);
      this.lvMutexSets.Name = "lvMutexSets";
      this.lvMutexSets.SelectionMode = SelectionMode.MultiSimple;

      this.lvMutexSets.Size = new Size(238, 242);
      this.lvMutexSets.TabIndex = 111;

      this.Label8.Location = new Point(6, 16);
      this.Label8.Name = "Label8";

      this.Label8.Size = new Size(100, 20);
      this.Label8.TabIndex = 22;
      this.Label8.Text = "Group (Only one):";
      this.Label8.TextAlign = ContentAlignment.MiddleRight;
      this.cbMutexGroup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbMutexGroup.FormattingEnabled = true;

      this.cbMutexGroup.Location = new Point(9, 44);
      this.cbMutexGroup.Name = "cbMutexGroup";

      this.cbMutexGroup.Size = new Size(238, 22);
      this.cbMutexGroup.TabIndex = 21;

      this.AutoScaleBaseSize = new Size(5, 13);

      this.ClientSize = new Size(798, 621);
      this.Controls.Add((Control) this.GroupBox5);
      this.Controls.Add((Control) this.gbLink);
      this.Controls.Add((Control) this.GroupBox4);
      this.Controls.Add((Control) this.GroupBox3);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.GroupBox2);
      this.Controls.Add((Control) this.GroupBox1);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.Label4);
      this.Controls.Add((Control) this.lvPowers);
      this.Controls.Add((Control) this.Label3);
      this.Controls.Add((Control) this.cbSetType);
      this.Controls.Add((Control) this.Label2);
      this.Controls.Add((Control) this.cbAT);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (frmEditPowerset);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Edit Powerset (Group_Name.Set_Name)";
      ((ISupportInitialize) this.picIcon).EndInit();
      this.GroupBox1.ResumeLayout(false);
      this.GroupBox1.PerformLayout();
      this.GroupBox2.ResumeLayout(false);
      this.GroupBox3.ResumeLayout(false);
      this.GroupBox3.PerformLayout();
      this.GroupBox4.ResumeLayout(false);
      this.gbLink.ResumeLayout(false);
      this.GroupBox5.ResumeLayout(false);
              //adding events
              if(!System.Diagnostics.Debugger.IsAttached || !this.IsInDesignMode() || !System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLowerInvariant().Contains("devenv"))
              {
                  this.btnCancel.Click += btnCancel_Click;
                  this.btnClearIcon.Click += btnClearIcon_Click;
                  this.btnClose.Click += btnClose_Click;
                  this.btnIcon.Click += btnIcon_Click;
                  this.cbAT.SelectedIndexChanged += cbAT_SelectedIndexChanged;
                  this.cbLinkGroup.SelectedIndexChanged += cbLinkGroup_SelectedIndexChanged;
                  this.cbLinkSet.SelectedIndexChanged += cbLinkSet_SelectedIndexChanged;
                  this.cbMutexGroup.SelectionChangeCommitted += cbMutexGroup_SelectionChangeCommitted;
                  this.cbSetType.SelectedIndexChanged += cbSetType_SelectedIndexChanged;
                  this.cbTrunkGroup.SelectedIndexChanged += cbTrunkGroup_SelectedIndexChanged;
                  this.cbTrunkSet.SelectedIndexChanged += cbTrunkSet_SelectedIndexChanged;
                  this.chkNoLink.CheckedChanged += chkNoLink_CheckedChanged;
                  this.chkNoTrunk.CheckedChanged += chkNoTrunk_CheckedChanged;
                  this.lvMutexSets.SelectedIndexChanged += lvMutexSets_SelectedIndexChanged;
                  this.txtDesc.TextChanged += txtDesc_TextChanged;
                  this.txtName.TextChanged += txtName_TextChanged;
              }
              // finished with events
      this.ResumeLayout(false);
    }

    void ListMutexGroups()

    {
      this.cbMutexGroup.BeginUpdate();
      this.cbMutexGroup.Items.Clear();
      foreach (object key in (IEnumerable<string>) DatabaseAPI.Database.PowersetGroups.Keys)
        this.cbMutexGroup.Items.Add(key);
      this.cbMutexGroup.EndUpdate();
      if (this.myPS.nIDMutexSets.Length <= 0)
        return;
      int index = DatabaseAPI.NidFromUidPowerset(this.myPS.UIDMutexSets[0]);
      if (index > -1)
        this.cbMutexGroup.SelectedValue = (object) DatabaseAPI.Database.Powersets[index].GroupName;
    }

    void ListMutexSets()

    {
      this.lvMutexSets.BeginUpdate();
      this.lvMutexSets.Items.Clear();
      if (this.cbMutexGroup.SelectedIndex > -1)
      {
        int[] numArray = DatabaseAPI.NidSets(this.cbMutexGroup.SelectedText, Conversions.ToString(-1), Enums.ePowerSetType.None);
        int num1 = numArray.Length - 1;
        for (int index1 = 0; index1 <= num1; ++index1)
        {
          this.lvMutexSets.Items.Add((object) DatabaseAPI.Database.Powersets[numArray[index1]].FullName);
          int num2 = this.myPS.nIDMutexSets.Length - 1;
          for (int index2 = 0; index2 <= num2; ++index2)
          {
            if (numArray[index1] == this.myPS.nIDMutexSets[index2])
            {
              this.lvMutexSets.SetSelected(index1, true);
              break;
            }
          }
        }
      }
      this.lvMutexSets.EndUpdate();
    }

    public void ListPowers()
    {
      this.lvPowers.BeginUpdate();
      this.lvPowers.Items.Clear();
      int num = this.myPS.Power.Length - 1;
      for (int Index = 0; Index <= num; ++Index)
        this.AddListItem(Index);
      if (this.lvPowers.Items.Count > 0)
      {
        this.lvPowers.Items[0].Selected = true;
        this.lvPowers.Items[0].EnsureVisible();
      }
      this.lvPowers.EndUpdate();
    }

    void lvMutexSets_SelectedIndexChanged(object sender, EventArgs e)

    {
      if (this.Loading || this.cbMutexGroup.SelectedIndex < 0)
        return;
      IPowerset ps = this.myPS;
      ps.UIDMutexSets = new string[this.lvMutexSets.SelectedIndices.Count - 1 + 1];
      ps.nIDMutexSets = new int[this.lvMutexSets.SelectedIndices.Count - 1 + 1];
      int[] numArray = DatabaseAPI.NidSets(this.cbMutexGroup.SelectedText, Conversions.ToString(-1), Enums.ePowerSetType.None);
      int num = this.lvMutexSets.SelectedIndices.Count - 1;
      for (int index = 0; index <= num; ++index)
      {
        ps.UIDMutexSets[index] = DatabaseAPI.Database.Powersets[numArray[this.lvMutexSets.SelectedIndices[index]]].FullName;
        ps.nIDMutexSets[index] = DatabaseAPI.NidFromUidPowerset(ps.UIDMutexSets[index]);
      }
    }

    static bool PowersetFullNameIsUnique(string iFullName, int skipId = -1)

    {
      if (!string.IsNullOrEmpty(iFullName))
      {
        int num = DatabaseAPI.Database.Powersets.Length - 1;
        for (int index = 0; index <= num; ++index)
        {
          if (index != skipId && string.Equals(DatabaseAPI.Database.Powersets[index].FullName, iFullName, StringComparison.OrdinalIgnoreCase))
            return false;
        }
      }
      return true;
    }

    void txtDesc_TextChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.myPS.Description = this.txtDesc.Text;
    }

    void txtName_TextChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.myPS.DisplayName = this.txtName.Text;
    }

    void txtNameSet_Leave(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.DisplayNameData();
    }

    void txtNameSet_TextChanged(object sender, EventArgs e)

    {
      if (this.Loading)
        return;
      this.BuildFullName();
    }
  }
}
