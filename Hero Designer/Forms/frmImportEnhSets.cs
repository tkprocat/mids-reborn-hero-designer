
using Import;
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
  [DesignerGenerated]
  public class frmImportEnhSets : Form
  {
    frmBusy _bFrm;

        Button btnCheckAll;

        Button btnClose;

        Button btnFile;

        Button btnImport;

        Button btnUncheckAll;
        ColumnHeader ColumnHeader1;
        ColumnHeader ColumnHeader2;
        ColumnHeader ColumnHeader4;
        ColumnHeader ColumnHeader5;
        ColumnHeader ColumnHeader6;

    readonly List<ListViewItem> _currentItems;
        OpenFileDialog dlgBrowse;

    string _fullFileName;

        Button HideUnchanged;

    List<EnhSetData> _importBuffer;
        Label Label6;
        Label Label8;
        Label lblDate;
        Label lblFile;
        ListView lstImport;

    bool _showUnchanged;
        NumericUpDown udRevision;

    IContainer components;












    public frmImportEnhSets()
    {
      this.Load += new EventHandler(this.frmImportEnhSets_Load);
      this._fullFileName = "";
      this._showUnchanged = true;
      this.InitializeComponent();
      this._importBuffer = new List<EnhSetData>();
      this._currentItems = new List<ListViewItem>();
    }

    void btnCheckAll_Click(object sender, EventArgs e)

    {
      this.lstImport.BeginUpdate();
      int num = this.lstImport.Items.Count - 1;
      for (int index = 0; index <= num; ++index)
        this.lstImport.Items[index].Checked = true;
      this.lstImport.EndUpdate();
    }

    void btnClose_Click(object sender, EventArgs e)

    {
      this.Close();
    }

    void btnFile_Click(object sender, EventArgs e)

    {
      this.dlgBrowse.FileName = this._fullFileName;
      if (this.dlgBrowse.ShowDialog((IWin32Window) this) == DialogResult.OK)
      {
        this._fullFileName = this.dlgBrowse.FileName;
        this.Enabled = false;
        if (this.ParseClasses(this._fullFileName))
          this.FillListView();
        this.Enabled = true;
      }
      this.BusyHide();
      this.DisplayInfo();
    }

    void btnImport_Click(object sender, EventArgs e)

    {
      this.ProcessImport();
    }

    void btnUncheckAll_Click(object sender, EventArgs e)

    {
      this.lstImport.BeginUpdate();
      int num = this.lstImport.Items.Count - 1;
      for (int index = 0; index <= num; ++index)
        this.lstImport.Items[index].Checked = false;
      this.lstImport.EndUpdate();
    }

    void BusyHide()

    {
      if (this._bFrm == null)
        return;
      this._bFrm.Close();
      this._bFrm = null;
    }

    void BusyMsg(string sMessage)

    {
      if (this._bFrm == null)
      {
        this._bFrm = new frmBusy();
        this._bFrm.Show((IWin32Window) this);
      }
      this._bFrm.SetMessage(sMessage);
    }

    void DisplayInfo()

    {
      this.lblFile.Text = FileIO.StripPath(this._fullFileName);
    }

    [DebuggerNonUserCode]
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing || this.components == null)
          return;
        this.components.Dispose();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    void FillListView()

    {
      string[] items = new string[6];
      this.lstImport.BeginUpdate();
      this.lstImport.Items.Clear();
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = this._importBuffer.Count - 1;
      for (int index = 0; index <= num4; ++index)
      {
        ++num1;
        if (num1 >= 100)
        {
          this.BusyMsg(Strings.Format((object) index, "###,##0") + " records checked.");
          Application.DoEvents();
          num1 = 0;
        }
        if (this._importBuffer[index].IsValid)
        {
          items[0] = this._importBuffer[index].Data.DisplayName;
          items[1] = Enum.GetName(this._importBuffer[index].Data.SetType.GetType(), (object) this._importBuffer[index].Data.SetType);
          bool flag = false;
          if (this._importBuffer[index].IsNew)
          {
            items[2] = "Yes";
            ++num2;
          }
          else
          {
            items[2] = "No";
            flag = this._importBuffer[index].CheckDifference(out items[4]);
          }
          if (flag)
          {
            items[3] = "Yes";
            ++num3;
          }
          else
            items[3] = "No";
          ListViewItem listViewItem = new ListViewItem(items)
          {
            Checked = flag | this._importBuffer[index].IsNew,
            Tag = (object) index
          };
          this._currentItems.Add(listViewItem);
          this.lstImport.Items.Add(listViewItem);
        }
      }
      if (this.lstImport.Items.Count > 0)
        this.lstImport.Items[0].EnsureVisible();
      this.lstImport.EndUpdate();
      this.HideUnchanged.Text = "Hide Unchanged";
      int num5 = (int) Interaction.MsgBox((object) ("New: " + Conversions.ToString(num2) + "\r\nModified: " + Conversions.ToString(num3)), MsgBoxStyle.OkOnly, null);
    }

    void frmImportEnhSets_Load(object sender, EventArgs e)

    {
      this._fullFileName = "boostsets.csv";
      this.DisplayInfo();
    }

    void HideUnchanged_Click(object sender, EventArgs e)

    {
      this._showUnchanged = !this._showUnchanged;
      this.lstImport.BeginUpdate();
      this.lstImport.Items.Clear();
      int num = this._currentItems.Count - 1;
      for (int index = 0; index <= num; ++index)
      {
        if (this._showUnchanged | this._currentItems[index].SubItems[2].Text == "Yes" | this._currentItems[index].SubItems[3].Text == "Yes")
          this.lstImport.Items.Add(this._currentItems[index]);
      }
      this.lstImport.EndUpdate();
    }

    [DebuggerStepThrough]
    void InitializeComponent()

    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (frmImportEnhSets));
      this.btnCheckAll = new Button();
      this.btnClose = new Button();
      this.dlgBrowse = new OpenFileDialog();
      this.btnUncheckAll = new Button();
      this.btnImport = new Button();
      this.lstImport = new ListView();
      this.ColumnHeader1 = new ColumnHeader();
      this.ColumnHeader2 = new ColumnHeader();
      this.ColumnHeader4 = new ColumnHeader();
      this.ColumnHeader5 = new ColumnHeader();
      this.ColumnHeader6 = new ColumnHeader();
      this.Label8 = new Label();
      this.lblDate = new Label();
      this.udRevision = new NumericUpDown();
      this.Label6 = new Label();
      this.lblFile = new Label();
      this.btnFile = new Button();
      this.HideUnchanged = new Button();
      this.udRevision.BeginInit();
      this.SuspendLayout();

      this.btnCheckAll.Location = new Point(12, 545);
      this.btnCheckAll.Name = "btnCheckAll";

      this.btnCheckAll.Size = new Size(75, 23);
      this.btnCheckAll.TabIndex = 60;
      this.btnCheckAll.Text = "Check All";
      this.btnCheckAll.UseVisualStyleBackColor = true;

      this.btnClose.Location = new Point(904, 516);
      this.btnClose.Name = "btnClose";

      this.btnClose.Size = new Size(86, 23);
      this.btnClose.TabIndex = 59;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.dlgBrowse.DefaultExt = "csv";
      this.dlgBrowse.Filter = "CSV Spreadsheets (*.csv)|*.csv";

      this.btnUncheckAll.Location = new Point(93, 545);
      this.btnUncheckAll.Name = "btnUncheckAll";

      this.btnUncheckAll.Size = new Size(75, 23);
      this.btnUncheckAll.TabIndex = 61;
      this.btnUncheckAll.Text = "Uncheck All";
      this.btnUncheckAll.UseVisualStyleBackColor = true;

      this.btnImport.Location = new Point(904, 77);
      this.btnImport.Name = "btnImport";

      this.btnImport.Size = new Size(86, 22);
      this.btnImport.TabIndex = 58;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.lstImport.CheckBoxes = true;
      this.lstImport.Columns.AddRange(new ColumnHeader[5]
      {
        this.ColumnHeader1,
        this.ColumnHeader2,
        this.ColumnHeader4,
        this.ColumnHeader5,
        this.ColumnHeader6
      });
      this.lstImport.HeaderStyle = ColumnHeaderStyle.Nonclickable;

      this.lstImport.Location = new Point(12, 77);
      this.lstImport.Name = "lstImport";

      this.lstImport.Size = new Size(886, 462);
      this.lstImport.TabIndex = 57;
      this.lstImport.UseCompatibleStateImageBehavior = false;
      this.lstImport.View = View.Details;
      this.ColumnHeader1.Text = "Set Name";
      this.ColumnHeader1.Width = 293;
      this.ColumnHeader2.Text = "Set Type";
      this.ColumnHeader2.Width = 87;
      this.ColumnHeader4.Text = "New";
      this.ColumnHeader4.Width = 53;
      this.ColumnHeader5.Text = "Modified";
      this.ColumnHeader6.Text = "Change";
      this.ColumnHeader6.Width = 310;

      this.Label8.Location = new Point(632, 53);
      this.Label8.Name = "Label8";

      this.Label8.Size = new Size(65, 18);
      this.Label8.TabIndex = 56;
      this.Label8.Text = "Revision:";
      this.Label8.TextAlign = ContentAlignment.TopRight;

      this.lblDate.Location = new Point(9, 53);
      this.lblDate.Name = "lblDate";

      this.lblDate.Size = new Size(175, 18);
      this.lblDate.TabIndex = 54;
      this.lblDate.Text = "Date:";

      this.udRevision.Location = new Point(703, 51);
      this.udRevision.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.udRevision.Name = "udRevision";

      this.udRevision.Size = new Size(116, 20);
      this.udRevision.TabIndex = 53;

      this.Label6.Location = new Point(12, 9);
      this.Label6.Name = "Label6";

      this.Label6.Size = new Size(150, 14);
      this.Label6.TabIndex = 52;
      this.Label6.Text = "Effect Definition File:";
      this.lblFile.BorderStyle = BorderStyle.Fixed3D;

      this.lblFile.Location = new Point(12, 25);
      this.lblFile.Name = "lblFile";

      this.lblFile.Size = new Size(807, 23);
      this.lblFile.TabIndex = 51;
      this.lblFile.TextAlign = ContentAlignment.MiddleLeft;

      this.btnFile.Location = new Point(825, 25);
      this.btnFile.Name = "btnFile";

      this.btnFile.Size = new Size(165, 23);
      this.btnFile.TabIndex = 50;
      this.btnFile.Text = "Load / Re-Load";
      this.btnFile.UseVisualStyleBackColor = true;

      this.HideUnchanged.Location = new Point(174, 545);
      this.HideUnchanged.Name = "HideUnchanged";

      this.HideUnchanged.Size = new Size(99, 23);
      this.HideUnchanged.TabIndex = 64;
      this.HideUnchanged.Text = "Hide Unchanged";
      this.HideUnchanged.UseVisualStyleBackColor = true;
      this.AutoScaleMode = AutoScaleMode.None;

      this.ClientSize = new Size(1002, 573);
      this.Controls.Add((Control) this.HideUnchanged);
      this.Controls.Add((Control) this.btnCheckAll);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.btnUncheckAll);
      this.Controls.Add((Control) this.btnImport);
      this.Controls.Add((Control) this.lstImport);
      this.Controls.Add((Control) this.Label8);
      this.Controls.Add((Control) this.lblDate);
      this.Controls.Add((Control) this.udRevision);
      this.Controls.Add((Control) this.Label6);
      this.Controls.Add((Control) this.lblFile);
      this.Controls.Add((Control) this.btnFile);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (frmImportEnhSets);
      this.ShowInTaskbar = false;
      this.Text = "Import Enhancement Sets";
      this.udRevision.EndInit();
              //adding events
              if(!System.Diagnostics.Debugger.IsAttached || !this.IsInDesignMode() || !System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLowerInvariant().Contains("devenv"))
              {
                  this.HideUnchanged.Click += HideUnchanged_Click;
                  this.btnCheckAll.Click += btnCheckAll_Click;
                  this.btnClose.Click += btnClose_Click;
                  this.btnFile.Click += btnFile_Click;
                  this.btnImport.Click += btnImport_Click;
                  this.btnUncheckAll.Click += btnUncheckAll_Click;
              }
              // finished with events
      this.ResumeLayout(false);
    }

    bool ParseClasses(string iFileName)

    {
      int num1 = 0;
      StreamReader iStream;
      try
      {
        iStream = new StreamReader(iFileName);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        int num2 = (int) Interaction.MsgBox((object) ex.Message, MsgBoxStyle.Critical, (object) "Power CSV Not Opened");
        ProjectData.ClearProjectError();
        return false;
      }
      int num3 = 0;
      int num4 = 0;
      this._importBuffer = new List<EnhSetData>();
      int num5 = 0;
      string iString;
      do
      {
        iString = FileIO.ReadLineUnlimited(iStream, char.MinValue);
        if (iString != null && !iString.StartsWith("#"))
        {
          ++num5;
          if (num5 >= 100)
          {
            this.BusyMsg(Strings.Format((object) num3, "###,##0") + " records parsed.");
            num5 = 0;
          }
          this._importBuffer.Add(new EnhSetData(iString));
          ++num3;
          if (this._importBuffer[this._importBuffer.Count - 1].IsValid)
            ++num1;
          else
            ++num4;
        }
      }
      while (iString != null);
      iStream.Close();
      int num6 = (int) Interaction.MsgBox((object) ("Parse Completed!\r\nTotal Records: " + Conversions.ToString(num3) + "\r\nGood: " + Conversions.ToString(num1) + "\r\nRejected: " + Conversions.ToString(num4)), MsgBoxStyle.Information, (object) "File Parsed");
      return true;
    }

    bool ProcessImport()

    {
      bool flag = false;
      int num1 = 0;
      int num2 = 0;
      this.BusyMsg("Applying...");
      this.Enabled = false;
      int num3 = this.lstImport.Items.Count - 1;
      for (int index = 0; index <= num3; ++index)
      {
        if (this.lstImport.Items[index].Checked)
        {
          this._importBuffer[Conversions.ToInteger(this.lstImport.Items[index].Tag)].Apply();
          ++num1;
          ++num2;
          if (num2 >= 9)
          {
            this.BusyMsg("Applying: " + Conversions.ToString(index) + " records done.");
            Application.DoEvents();
            num2 = 0;
          }
        }
      }
      this.Enabled = true;
      this.BusyMsg("Saving...");
      DatabaseAPI.SaveMainDatabase();
      this.BusyHide();
      int num4 = (int) Interaction.MsgBox((object) ("Import of " + Conversions.ToString(num1) + " records completed!"), MsgBoxStyle.Information, (object) "Done");
      this.DisplayInfo();
      return flag;
    }
  }
}
