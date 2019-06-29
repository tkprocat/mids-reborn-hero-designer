
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
  [DesignerGenerated]
  public class frmImport_SetAssignments : Form
  {
        Button btnClose;

        Button btnFile;

        Button btnImport;
        OpenFileDialog dlgBrowse;
        Label Label8;
        Label lblDate;
        Label lblFile;
        NumericUpDown udRevision;

    frmBusy bFrm;

    IContainer components;

    string FullFileName;






    public frmImport_SetAssignments()
    {
      this.Load += new EventHandler(this.frmImport_SetAssignments_Load);
      this.FullFileName = "";
      this.InitializeComponent();
    }

    protected void AddSetType(int nIDPower, Enums.eSetType nSetType)
    {
      if (!(nIDPower > -1 & nIDPower < DatabaseAPI.Database.Power.Length))
        return;
      int num = DatabaseAPI.Database.Power[nIDPower].SetTypes.Length - 1;
      for (int index = 0; index <= num; ++index)
      {
        if (DatabaseAPI.Database.Power[nIDPower].SetTypes[index] == nSetType)
          return;
      }
      IPower[] power = DatabaseAPI.Database.Power;
      Enums.eSetType[] eSetTypeArray = (Enums.eSetType[]) Utils.CopyArray((Array) power[nIDPower].SetTypes, (Array) new Enums.eSetType[DatabaseAPI.Database.Power[nIDPower].SetTypes.Length + 1]);
      power[nIDPower].SetTypes = eSetTypeArray;
      DatabaseAPI.Database.Power[nIDPower].SetTypes[DatabaseAPI.Database.Power[nIDPower].SetTypes.Length - 1] = nSetType;
      Array.Sort<Enums.eSetType>(DatabaseAPI.Database.Power[nIDPower].SetTypes);
    }

    void btnClose_Click(object sender, EventArgs e)

    {
      this.Close();
    }

    void btnFile_Click(object sender, EventArgs e)

    {
      this.dlgBrowse.FileName = this.FullFileName;
      if (this.dlgBrowse.ShowDialog((IWin32Window) this) == DialogResult.OK)
        this.FullFileName = this.dlgBrowse.FileName;
      this.BusyHide();
      this.DisplayInfo();
    }

    void btnImport_Click(object sender, EventArgs e)

    {
      this.ParseClasses(this.FullFileName);
      this.BusyHide();
      this.DisplayInfo();
    }

    void BusyHide()

    {
      if (this.bFrm == null)
        return;
      this.bFrm.Close();
      this.bFrm = null;
    }

    void BusyMsg(string sMessage)

    {
      if (this.bFrm == null)
      {
        this.bFrm = new frmBusy();
        this.bFrm.Show((IWin32Window) this);
      }
      this.bFrm.SetMessage(sMessage);
    }

    public void DisplayInfo()
    {
      this.lblFile.Text = FileIO.StripPath(this.FullFileName);
      this.lblDate.Text = "Date: " + Strings.Format((object) DatabaseAPI.Database.IOAssignmentVersion.RevisionDate, "dd/MMM/yy HH:mm:ss");
      this.udRevision.Value = new Decimal(DatabaseAPI.Database.IOAssignmentVersion.Revision);
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

    void frmImport_SetAssignments_Load(object sender, EventArgs e)

    {
      this.FullFileName = DatabaseAPI.Database.IOAssignmentVersion.SourceFile;
      this.DisplayInfo();
    }

    [DebuggerStepThrough]
    void InitializeComponent()

    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (frmImport_SetAssignments));
      this.Label8 = new Label();
      this.lblDate = new Label();
      this.udRevision = new NumericUpDown();
      this.btnClose = new Button();
      this.btnImport = new Button();
      this.lblFile = new Label();
      this.btnFile = new Button();
      this.dlgBrowse = new OpenFileDialog();
      this.udRevision.BeginInit();
      this.SuspendLayout();

      this.Label8.Location = new Point(346, 85);
      this.Label8.Name = "Label8";

      this.Label8.Size = new Size(65, 18);
      this.Label8.TabIndex = 55;
      this.Label8.Text = "Revision:";
      this.Label8.TextAlign = ContentAlignment.TopRight;

      this.lblDate.Location = new Point(9, 85);
      this.lblDate.Name = "lblDate";

      this.lblDate.Size = new Size(175, 18);
      this.lblDate.TabIndex = 54;
      this.lblDate.Text = "Date:";

      this.udRevision.Location = new Point(417, 83);
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

      this.btnClose.Location = new Point(539, 81);
      this.btnClose.Name = "btnClose";

      this.btnClose.Size = new Size(86, 23);
      this.btnClose.TabIndex = 52;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;

      this.btnImport.Location = new Point(539, 38);
      this.btnImport.Name = "btnImport";

      this.btnImport.Size = new Size(86, 23);
      this.btnImport.TabIndex = 50;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.lblFile.BorderStyle = BorderStyle.Fixed3D;

      this.lblFile.Location = new Point(12, 9);
      this.lblFile.Name = "lblFile";

      this.lblFile.Size = new Size(521, 46);
      this.lblFile.TabIndex = 51;
      this.lblFile.TextAlign = ContentAlignment.MiddleLeft;

      this.btnFile.Location = new Point(539, 9);
      this.btnFile.Name = "btnFile";

      this.btnFile.Size = new Size(86, 23);
      this.btnFile.TabIndex = 49;
      this.btnFile.Text = "Browse...";
      this.btnFile.UseVisualStyleBackColor = true;
      this.dlgBrowse.DefaultExt = "csv";
      this.dlgBrowse.Filter = "CSV Spreadsheets (*.csv)|*.csv";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;

      this.ClientSize = new Size(637, 115);
      this.Controls.Add((Control) this.Label8);
      this.Controls.Add((Control) this.lblDate);
      this.Controls.Add((Control) this.udRevision);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.btnImport);
      this.Controls.Add((Control) this.lblFile);
      this.Controls.Add((Control) this.btnFile);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (frmImport_SetAssignments);
      this.ShowInTaskbar = false;
      this.Text = "Invention Set Assignment Import";
      this.udRevision.EndInit();
              //adding events
              if(!System.Diagnostics.Debugger.IsAttached || !this.IsInDesignMode() || !System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLowerInvariant().Contains("devenv"))
              {
                  this.btnClose.Click += btnClose_Click;
                  this.btnFile.Click += btnFile_Click;
                  this.btnImport.Click += btnImport_Click;
              }
              // finished with events
      this.ResumeLayout(false);
    }

    bool ParseClasses(string iFileName)

    {
      int num1 = 0;
      Enums.eSetType eSetType = Enums.eSetType.Untyped;
      StreamReader iStream;
      try
      {
        iStream = new StreamReader(iFileName);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        int num2 = (int) Interaction.MsgBox((object) ex.Message, MsgBoxStyle.Critical, (object) "IO CSV Not Opened");
        bool flag = false;
        ProjectData.ClearProjectError();
        return flag;
      }
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      bool[] flagArray = new bool[Enum.GetValues(eSetType.GetType()).Length - 1 + 1];
      int index1 = -1;
      int num6 = DatabaseAPI.Database.Power.Length - 1;
      for (int index2 = 0; index2 <= num6; ++index2)
        DatabaseAPI.Database.Power[index2].SetTypes = new Enums.eSetType[0];
      try
      {
        string iLine;
        do
        {
          iLine = FileIO.ReadLineUnlimited(iStream, char.MinValue);
          if (iLine != null && !iLine.StartsWith("#"))
          {
            ++num5;
            if (num5 >= 9)
            {
              this.BusyMsg(Strings.Format((object) num3, "###,##0") + " records parsed.");
              num5 = 0;
            }
            string[] array = CSV.ToArray(iLine);
            if (array.Length > 1)
            {
              int num2 = DatabaseAPI.NidFromUidioSet(array[0]);
              if (num2 != index1 & index1 > -1)
                flagArray[(int) DatabaseAPI.Database.EnhancementSets[index1].SetType] = true;
              index1 = num2;
              if (index1 > -1 && !flagArray[(int) DatabaseAPI.Database.EnhancementSets[index1].SetType])
              {
                int nIDPower = DatabaseAPI.NidFromUidPower(array[1]);
                if (nIDPower > -1)
                  this.AddSetType(nIDPower, DatabaseAPI.Database.EnhancementSets[index1].SetType);
              }
              ++num1;
            }
            else
              ++num4;
            ++num3;
          }
        }
        while (iLine != null);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception exception = ex;
        iStream.Close();
        int num2 = (int) Interaction.MsgBox((object) exception.Message, MsgBoxStyle.Critical, (object) "IO CSV Parse Error");
        bool flag = false;
        ProjectData.ClearProjectError();
        return flag;
      }
      iStream.Close();
      DatabaseAPI.Database.IOAssignmentVersion.SourceFile = this.dlgBrowse.FileName;
      DatabaseAPI.Database.IOAssignmentVersion.RevisionDate = DateTime.Now;
      DatabaseAPI.Database.IOAssignmentVersion.Revision = Convert.ToInt32(this.udRevision.Value);
      DatabaseAPI.SaveMainDatabase();
      this.DisplayInfo();
      int num7 = (int) Interaction.MsgBox((object) ("Parse Completed!\r\nTotal Records: " + Conversions.ToString(num3) + "\r\nGood: " + Conversions.ToString(num1) + "\r\nRejected: " + Conversions.ToString(num4)), MsgBoxStyle.Information, (object) "File Parsed");
      return true;
    }
  }
}
