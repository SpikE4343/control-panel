namespace tester
{
  partial class VesselWindow
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
      this.panelGrid = new System.Windows.Forms.PropertyGrid();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.button1 = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.portText = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.buadText = new System.Windows.Forms.TextBox();
      this.connectionButton = new System.Windows.Forms.Button();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.vesselGrid = new System.Windows.Forms.PropertyGrid();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.SuspendLayout();
      // 
      // panelGrid
      // 
      this.panelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelGrid.Location = new System.Drawing.Point(3, 3);
      this.panelGrid.Name = "panelGrid";
      this.panelGrid.Size = new System.Drawing.Size(347, 251);
      this.panelGrid.TabIndex = 0;
      // 
      // statusStrip1
      // 
      this.statusStrip1.Location = new System.Drawing.Point(0, 283);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(547, 22);
      this.statusStrip1.TabIndex = 1;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.button1);
      this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
      this.splitContainer1.Size = new System.Drawing.Size(547, 283);
      this.splitContainer1.SplitterDistance = 182;
      this.splitContainer1.TabIndex = 2;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(8, 252);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 6;
      this.button1.Text = "Save";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.saveButtonClicked);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.portText);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.buadText);
      this.groupBox1.Controls.Add(this.connectionButton);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(182, 100);
      this.groupBox1.TabIndex = 5;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Connection";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(5, 21);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(26, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Port";
      // 
      // portText
      // 
      this.portText.Location = new System.Drawing.Point(46, 18);
      this.portText.Name = "portText";
      this.portText.Size = new System.Drawing.Size(134, 20);
      this.portText.TabIndex = 1;
      this.portText.Text = "COM4";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(5, 44);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Baud";
      // 
      // buadText
      // 
      this.buadText.Location = new System.Drawing.Point(46, 44);
      this.buadText.Name = "buadText";
      this.buadText.Size = new System.Drawing.Size(134, 20);
      this.buadText.TabIndex = 3;
      this.buadText.Text = "9600";
      // 
      // connectionButton
      // 
      this.connectionButton.Location = new System.Drawing.Point(8, 70);
      this.connectionButton.Name = "connectionButton";
      this.connectionButton.Size = new System.Drawing.Size(75, 23);
      this.connectionButton.TabIndex = 4;
      this.connectionButton.Text = "Connect";
      this.connectionButton.UseVisualStyleBackColor = true;
      this.connectionButton.Click += new System.EventHandler(this.connectionButton_Click);
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl1.Location = new System.Drawing.Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(361, 283);
      this.tabControl1.TabIndex = 1;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.panelGrid);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(353, 257);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Panel";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.vesselGrid);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(353, 257);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Vessel";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // vesselGrid
      // 
      this.vesselGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.vesselGrid.Location = new System.Drawing.Point(3, 3);
      this.vesselGrid.Name = "vesselGrid";
      this.vesselGrid.Size = new System.Drawing.Size(347, 251);
      this.vesselGrid.TabIndex = 1;
      // 
      // VesselWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(547, 305);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.statusStrip1);
      this.Name = "VesselWindow";
      this.Text = "Control Panel Tester";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VesselWindow_FormClosed);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PropertyGrid panelGrid;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox portText;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox buadText;
    private System.Windows.Forms.Button connectionButton;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.PropertyGrid vesselGrid;
    private System.Windows.Forms.Button button1;

  }
}

