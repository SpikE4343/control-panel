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
      this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.portText = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.buadText = new System.Windows.Forms.TextBox();
      this.connectionButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // propertyGrid1
      // 
      this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
      this.propertyGrid1.Name = "propertyGrid1";
      this.propertyGrid1.Size = new System.Drawing.Size(361, 283);
      this.propertyGrid1.TabIndex = 0;
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
      this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
      this.splitContainer1.Size = new System.Drawing.Size(547, 283);
      this.splitContainer1.SplitterDistance = 182;
      this.splitContainer1.TabIndex = 2;
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
      this.connectionButton.Location = new System.Drawing.Point(104, 68);
      this.connectionButton.Name = "connectionButton";
      this.connectionButton.Size = new System.Drawing.Size(75, 23);
      this.connectionButton.TabIndex = 4;
      this.connectionButton.Text = "Connect";
      this.connectionButton.UseVisualStyleBackColor = true;
      this.connectionButton.Click += new System.EventHandler(this.connectionButton_Click);
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
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PropertyGrid propertyGrid1;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox portText;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox buadText;
    private System.Windows.Forms.Button connectionButton;

  }
}

