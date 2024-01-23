using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using P2Controller.Class;
using P2Controller.p2Command;

namespace P2Controller;

public class controller : Form
{
	internal string myIP;

	internal BackgroundWorker worker = new BackgroundWorker();

	private IContainer components;

	private Button mainExposure;

	private Button button4;

	private Button button8;

	private Button headExposure;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private GroupBox groupBox3;

	private GroupBox groupBox4;

	private ImageList imageResource;

	private CheckBox ir1;

	private CheckBox ir2;

	private CheckBox checkBox3;

	private CheckBox idx1;

	private CheckBox idx2;

	private CheckBox idx3;

	private CheckBox idx0;

	private NumericUpDown mainExposureUpDown;

	private NumericUpDown headExposureUpDown;

	private GroupBox groupBox5;

	private NumericUpDown xUpDown;

	private NumericUpDown yUpDown;

	private Button Home;

	private Button button2;

	private Label lbF;

	private NumericUpDown fUpDown;

	private Label lbY;

	private Label lbX;

	public controller(string IP)
	{
		InitializeComponent();
		myIP = IP;
		worker.WorkerSupportsCancellation = true;
		worker.DoWork += Worker_DoStreamWork;
		new P2WebAPI();
	}

	private void Worker_DoStreamWork(object sender, DoWorkEventArgs e)
	{
		P2WebAPI api = new P2WebAPI();
		while (!worker.CancellationPending)
		{
			Trace.WriteLine("fetching main camera frame...");
			var sw = Stopwatch.StartNew();
			api.cameraCommand(myIP, int.Parse(e.Argument.ToString()));
			Trace.WriteLine($"took {sw.Elapsed} to fetch/decode/send frame");
		}
		e.Cancel = true;
	}

	private void ir_Icon(CheckBox checkBox)
	{
		if (checkBox.Checked)
		{
			checkBox.ImageKey = "IR_ON";
		}
		else
		{
			checkBox.ImageKey = "IR_OFF";
		}
	}

	private void ir_checkChanged(object sender, EventArgs e)
	{
		ir_Icon((CheckBox)sender);
	}

	private void ir_Command(CheckBox sender)
	{
		p2_irResponse response = new P2WebAPI().irCommand(action: sender.Checked ? "on" : "off", ip: myIP, index: int.Parse(sender.Tag.ToString()));
		ir1.Checked = response.data.ir1 == "on";
		ir2.Checked = response.data.ir2 == "on";
	}

	private void ir2_click(object sender, EventArgs e)
	{
		ir_Command((CheckBox)sender);
	}

	private void ir1_Click(object sender, EventArgs e)
	{
		ir_Command((CheckBox)sender);
	}

	private void light_Icon(CheckBox checkBox)
	{
		if (checkBox.Checked)
		{
			checkBox.ImageKey = "LIGHT_ON";
		}
		else
		{
			checkBox.ImageKey = "LIGHT_OFF";
		}
	}

	private void light0_Command(CheckBox sender)
	{
		p2_lightsResponse response = new P2WebAPI().light0Command(value: sender.Checked ? 255 : 0, ip: myIP, idx: int.Parse(sender.Tag.ToString()));
		idx1.Checked = response.data[0].value == 255;
		idx2.Checked = response.data[1].value == 255;
		idx3.Checked = response.data[2].value == 255;
		idx0.Checked = ((idx1.Checked && idx2.Checked && idx3.Checked) ? true : false);
	}

	private void light_Command(CheckBox sender)
	{
		new P2WebAPI().lightCommand(value: sender.Checked ? 255 : 0, ip: myIP, idx: int.Parse(sender.Tag.ToString()));
		idx0.Checked = ((idx1.Checked && idx2.Checked && idx3.Checked) ? true : false);
	}

	private void idx_Click(object sender, EventArgs e)
	{
		light_Command((CheckBox)sender);
	}

	private void idx0_Click(object sender, EventArgs e)
	{
		light0_Command((CheckBox)sender);
	}

	private void idx0_checkChanged(object sender, EventArgs e)
	{
		light_Icon((CheckBox)sender);
	}

	private void idx1_checkedChanged(object sender, EventArgs e)
	{
		light_Icon((CheckBox)sender);
	}

	private void idx2_checkChanged(object sender, EventArgs e)
	{
		light_Icon((CheckBox)sender);
	}

	private void idx3_checkChanged(object sender, EventArgs e)
	{
		light_Icon((CheckBox)sender);
	}

	private void camera_Command(Button sender)
	{
		new P2WebAPI().cameraCommand(myIP, int.Parse(sender.Tag.ToString()));
	}

	private void camera_Click(object sender, EventArgs e)
	{
		camera_Command((Button)sender);
	}

	private void mainCamera_State(object sender, EventArgs e)
	{
		if (((CheckBox)sender).Checked)
		{
			if (!worker.IsBusy)
			{
				worker.RunWorkerAsync(int.Parse(((CheckBox)sender).Tag.ToString()));
			}
		}
		else if (worker.IsBusy)
		{
			worker.CancelAsync();
		}
	}

	private void exposure_Click(object sender, EventArgs e)
	{
		Button bt = (Button)sender;
		P2WebAPI p2WebAPI = new P2WebAPI();
		if (bt.Tag.ToString() == "0")
		{
			_ = (int)mainExposureUpDown.Value;
		}
		else
		{
			_ = (int)headExposureUpDown.Value;
		}
		p2WebAPI.exposureCommand(myIP, int.Parse(bt.Tag.ToString()), 23);
	}

	private void move_Click(object sender, EventArgs e)
	{
		new P2WebAPI().moveCommand(myIP, (int)xUpDown.Value, (int)yUpDown.Value, (int)fUpDown.Value);
	}

	private void home_Click(object sender, EventArgs e)
	{
		new P2WebAPI().moveCommand(myIP, 0, 0, (int)fUpDown.Value);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(P2Controller.controller));
		this.mainExposure = new System.Windows.Forms.Button();
		this.imageResource = new System.Windows.Forms.ImageList(this.components);
		this.button4 = new System.Windows.Forms.Button();
		this.button8 = new System.Windows.Forms.Button();
		this.headExposure = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.ir2 = new System.Windows.Forms.CheckBox();
		this.ir1 = new System.Windows.Forms.CheckBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.idx0 = new System.Windows.Forms.CheckBox();
		this.idx3 = new System.Windows.Forms.CheckBox();
		this.idx1 = new System.Windows.Forms.CheckBox();
		this.idx2 = new System.Windows.Forms.CheckBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.checkBox3 = new System.Windows.Forms.CheckBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.headExposureUpDown = new System.Windows.Forms.NumericUpDown();
		this.mainExposureUpDown = new System.Windows.Forms.NumericUpDown();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.lbF = new System.Windows.Forms.Label();
		this.fUpDown = new System.Windows.Forms.NumericUpDown();
		this.lbY = new System.Windows.Forms.Label();
		this.xUpDown = new System.Windows.Forms.NumericUpDown();
		this.lbX = new System.Windows.Forms.Label();
		this.Home = new System.Windows.Forms.Button();
		this.yUpDown = new System.Windows.Forms.NumericUpDown();
		this.button2 = new System.Windows.Forms.Button();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.headExposureUpDown).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.mainExposureUpDown).BeginInit();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fUpDown).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xUpDown).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.yUpDown).BeginInit();
		base.SuspendLayout();
		this.mainExposure.Location = new System.Drawing.Point(17, 19);
		this.mainExposure.Name = "mainExposure";
		this.mainExposure.Size = new System.Drawing.Size(130, 38);
		this.mainExposure.TabIndex = 0;
		this.mainExposure.Tag = "0";
		this.mainExposure.Text = "Main Exposure";
		this.mainExposure.UseVisualStyleBackColor = true;
		this.mainExposure.Click += new System.EventHandler(exposure_Click);
		this.imageResource.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageResource.ImageStream");
		this.imageResource.TransparentColor = System.Drawing.Color.Transparent;
		this.imageResource.Images.SetKeyName(0, "LIGHT_OFF");
		this.imageResource.Images.SetKeyName(1, "LIGHT_ON");
		this.imageResource.Images.SetKeyName(2, "Camera");
		this.imageResource.Images.SetKeyName(3, "Video");
		this.imageResource.Images.SetKeyName(4, "IR_ON");
		this.imageResource.Images.SetKeyName(5, "IR_OFF");
		this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.button4.ImageKey = "Camera";
		this.button4.ImageList = this.imageResource;
		this.button4.Location = new System.Drawing.Point(17, 19);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(130, 38);
		this.button4.TabIndex = 3;
		this.button4.Tag = "0";
		this.button4.Text = "Main Camera";
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(camera_Click);
		this.button8.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.button8.ImageKey = "Camera";
		this.button8.ImageList = this.imageResource;
		this.button8.Location = new System.Drawing.Point(154, 19);
		this.button8.Name = "button8";
		this.button8.Size = new System.Drawing.Size(130, 38);
		this.button8.TabIndex = 7;
		this.button8.Tag = "1";
		this.button8.Text = "Head Camera";
		this.button8.UseVisualStyleBackColor = true;
		this.button8.Click += new System.EventHandler(camera_Click);
		this.headExposure.Location = new System.Drawing.Point(154, 19);
		this.headExposure.Name = "headExposure";
		this.headExposure.Size = new System.Drawing.Size(129, 38);
		this.headExposure.TabIndex = 8;
		this.headExposure.Tag = "1";
		this.headExposure.Text = "Head Exposure";
		this.headExposure.UseVisualStyleBackColor = true;
		this.headExposure.Click += new System.EventHandler(exposure_Click);
		this.groupBox1.Controls.Add(this.ir2);
		this.groupBox1.Controls.Add(this.ir1);
		this.groupBox1.Location = new System.Drawing.Point(12, 256);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(295, 76);
		this.groupBox1.TabIndex = 9;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "IRs";
		this.ir2.Appearance = System.Windows.Forms.Appearance.Button;
		this.ir2.Cursor = System.Windows.Forms.Cursors.Default;
		this.ir2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.ir2.ImageKey = "IR_OFF";
		this.ir2.ImageList = this.imageResource;
		this.ir2.Location = new System.Drawing.Point(17, 19);
		this.ir2.Name = "ir2";
		this.ir2.Size = new System.Drawing.Size(130, 38);
		this.ir2.TabIndex = 14;
		this.ir2.Tag = "2";
		this.ir2.Text = "Main IR";
		this.ir2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.ir2.UseVisualStyleBackColor = true;
		this.ir2.CheckedChanged += new System.EventHandler(ir_checkChanged);
		this.ir2.Click += new System.EventHandler(ir2_click);
		this.ir1.Appearance = System.Windows.Forms.Appearance.Button;
		this.ir1.Cursor = System.Windows.Forms.Cursors.Default;
		this.ir1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.ir1.ImageKey = "IR_OFF";
		this.ir1.ImageList = this.imageResource;
		this.ir1.Location = new System.Drawing.Point(154, 19);
		this.ir1.Name = "ir1";
		this.ir1.Size = new System.Drawing.Size(130, 38);
		this.ir1.TabIndex = 13;
		this.ir1.Tag = "1";
		this.ir1.Text = "Camera IR";
		this.ir1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.ir1.UseVisualStyleBackColor = true;
		this.ir1.CheckedChanged += new System.EventHandler(ir_checkChanged);
		this.ir1.Click += new System.EventHandler(ir1_Click);
		this.groupBox2.Controls.Add(this.idx0);
		this.groupBox2.Controls.Add(this.idx3);
		this.groupBox2.Controls.Add(this.idx1);
		this.groupBox2.Controls.Add(this.idx2);
		this.groupBox2.Location = new System.Drawing.Point(12, 133);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(295, 117);
		this.groupBox2.TabIndex = 10;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Lights";
		this.idx0.Appearance = System.Windows.Forms.Appearance.Button;
		this.idx0.Cursor = System.Windows.Forms.Cursors.Default;
		this.idx0.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.idx0.ImageKey = "LIGHT_OFF";
		this.idx0.ImageList = this.imageResource;
		this.idx0.Location = new System.Drawing.Point(17, 19);
		this.idx0.Name = "idx0";
		this.idx0.Size = new System.Drawing.Size(130, 38);
		this.idx0.TabIndex = 19;
		this.idx0.Tag = "0";
		this.idx0.Text = "All Lights";
		this.idx0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.idx0.UseVisualStyleBackColor = true;
		this.idx0.CheckedChanged += new System.EventHandler(idx0_checkChanged);
		this.idx0.Click += new System.EventHandler(idx0_Click);
		this.idx3.Appearance = System.Windows.Forms.Appearance.Button;
		this.idx3.Cursor = System.Windows.Forms.Cursors.Default;
		this.idx3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.idx3.ImageKey = "LIGHT_OFF";
		this.idx3.ImageList = this.imageResource;
		this.idx3.Location = new System.Drawing.Point(153, 63);
		this.idx3.Name = "idx3";
		this.idx3.Size = new System.Drawing.Size(130, 38);
		this.idx3.TabIndex = 18;
		this.idx3.Tag = "3";
		this.idx3.Text = "Main Light L";
		this.idx3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.idx3.UseVisualStyleBackColor = true;
		this.idx3.CheckedChanged += new System.EventHandler(idx3_checkChanged);
		this.idx3.Click += new System.EventHandler(idx_Click);
		this.idx1.Appearance = System.Windows.Forms.Appearance.Button;
		this.idx1.Cursor = System.Windows.Forms.Cursors.Default;
		this.idx1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.idx1.ImageKey = "LIGHT_OFF";
		this.idx1.ImageList = this.imageResource;
		this.idx1.Location = new System.Drawing.Point(153, 19);
		this.idx1.Name = "idx1";
		this.idx1.Size = new System.Drawing.Size(130, 38);
		this.idx1.TabIndex = 16;
		this.idx1.Tag = "1";
		this.idx1.Text = "Head Light";
		this.idx1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.idx1.UseVisualStyleBackColor = true;
		this.idx1.CheckedChanged += new System.EventHandler(idx1_checkedChanged);
		this.idx1.Click += new System.EventHandler(idx_Click);
		this.idx2.Appearance = System.Windows.Forms.Appearance.Button;
		this.idx2.Cursor = System.Windows.Forms.Cursors.Default;
		this.idx2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.idx2.ImageKey = "LIGHT_OFF";
		this.idx2.ImageList = this.imageResource;
		this.idx2.Location = new System.Drawing.Point(17, 63);
		this.idx2.Name = "idx2";
		this.idx2.Size = new System.Drawing.Size(130, 38);
		this.idx2.TabIndex = 17;
		this.idx2.Tag = "2";
		this.idx2.Text = "Main Light R";
		this.idx2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.idx2.UseVisualStyleBackColor = true;
		this.idx2.CheckedChanged += new System.EventHandler(idx2_checkChanged);
		this.idx2.Click += new System.EventHandler(idx_Click);
		this.groupBox3.Controls.Add(this.button8);
		this.groupBox3.Controls.Add(this.checkBox3);
		this.groupBox3.Controls.Add(this.button4);
		this.groupBox3.Location = new System.Drawing.Point(12, 12);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(295, 115);
		this.groupBox3.TabIndex = 11;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Cameras";
		this.checkBox3.Appearance = System.Windows.Forms.Appearance.Button;
		this.checkBox3.Cursor = System.Windows.Forms.Cursors.Default;
		this.checkBox3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.checkBox3.ImageKey = "Video";
		this.checkBox3.ImageList = this.imageResource;
		this.checkBox3.Location = new System.Drawing.Point(17, 63);
		this.checkBox3.Name = "checkBox3";
		this.checkBox3.Size = new System.Drawing.Size(130, 38);
		this.checkBox3.TabIndex = 15;
		this.checkBox3.Tag = "0";
		this.checkBox3.Text = "Main Stream";
		this.checkBox3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.checkBox3.UseVisualStyleBackColor = true;
		this.checkBox3.CheckedChanged += new System.EventHandler(mainCamera_State);
		this.groupBox4.Controls.Add(this.headExposureUpDown);
		this.groupBox4.Controls.Add(this.mainExposureUpDown);
		this.groupBox4.Controls.Add(this.mainExposure);
		this.groupBox4.Controls.Add(this.headExposure);
		this.groupBox4.Location = new System.Drawing.Point(12, 338);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(295, 101);
		this.groupBox4.TabIndex = 12;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Control";
		this.headExposureUpDown.Location = new System.Drawing.Point(154, 63);
		this.headExposureUpDown.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.headExposureUpDown.Name = "headExposureUpDown";
		this.headExposureUpDown.Size = new System.Drawing.Size(129, 20);
		this.headExposureUpDown.TabIndex = 14;
		this.headExposureUpDown.Value = new decimal(new int[4] { 150, 0, 0, 0 });
		this.mainExposureUpDown.Location = new System.Drawing.Point(17, 63);
		this.mainExposureUpDown.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.mainExposureUpDown.Name = "mainExposureUpDown";
		this.mainExposureUpDown.Size = new System.Drawing.Size(130, 20);
		this.mainExposureUpDown.TabIndex = 13;
		this.mainExposureUpDown.Value = new decimal(new int[4] { 80, 0, 0, 0 });
		this.groupBox5.Controls.Add(this.lbF);
		this.groupBox5.Controls.Add(this.fUpDown);
		this.groupBox5.Controls.Add(this.lbY);
		this.groupBox5.Controls.Add(this.xUpDown);
		this.groupBox5.Controls.Add(this.lbX);
		this.groupBox5.Controls.Add(this.Home);
		this.groupBox5.Controls.Add(this.yUpDown);
		this.groupBox5.Controls.Add(this.button2);
		this.groupBox5.Location = new System.Drawing.Point(12, 445);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(295, 148);
		this.groupBox5.TabIndex = 15;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Head";
		this.lbF.AutoSize = true;
		this.lbF.Location = new System.Drawing.Point(155, 117);
		this.lbF.Name = "lbF";
		this.lbF.Size = new System.Drawing.Size(10, 13);
		this.lbF.TabIndex = 18;
		this.lbF.Text = "f";
		this.fUpDown.Location = new System.Drawing.Point(175, 115);
		this.fUpDown.Maximum = new decimal(new int[4] { 9600, 0, 0, 0 });
		this.fUpDown.Name = "fUpDown";
		this.fUpDown.Size = new System.Drawing.Size(110, 20);
		this.fUpDown.TabIndex = 15;
		this.fUpDown.Value = new decimal(new int[4] { 9600, 0, 0, 0 });
		this.lbY.AutoSize = true;
		this.lbY.Location = new System.Drawing.Point(155, 89);
		this.lbY.Name = "lbY";
		this.lbY.Size = new System.Drawing.Size(14, 13);
		this.lbY.TabIndex = 17;
		this.lbY.Text = "Y";
		this.xUpDown.Location = new System.Drawing.Point(175, 63);
		this.xUpDown.Maximum = new decimal(new int[4] { 9600, 0, 0, 0 });
		this.xUpDown.Name = "xUpDown";
		this.xUpDown.Size = new System.Drawing.Size(108, 20);
		this.xUpDown.TabIndex = 14;
		this.lbX.AutoSize = true;
		this.lbX.Location = new System.Drawing.Point(155, 65);
		this.lbX.Name = "lbX";
		this.lbX.Size = new System.Drawing.Size(14, 13);
		this.lbX.TabIndex = 16;
		this.lbX.Text = "X";
		this.Home.Location = new System.Drawing.Point(17, 19);
		this.Home.Name = "Home";
		this.Home.Size = new System.Drawing.Size(130, 38);
		this.Home.TabIndex = 0;
		this.Home.Tag = "0";
		this.Home.Text = "Home";
		this.Home.UseVisualStyleBackColor = true;
		this.Home.Click += new System.EventHandler(home_Click);
		this.yUpDown.Location = new System.Drawing.Point(175, 89);
		this.yUpDown.Maximum = new decimal(new int[4] { 9600, 0, 0, 0 });
		this.yUpDown.Name = "yUpDown";
		this.yUpDown.Size = new System.Drawing.Size(110, 20);
		this.yUpDown.TabIndex = 13;
		this.button2.Location = new System.Drawing.Point(155, 19);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(129, 38);
		this.button2.TabIndex = 8;
		this.button2.Tag = "1";
		this.button2.Text = "Move";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(move_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(326, 605);
		base.Controls.Add(this.groupBox5);
		base.Controls.Add(this.groupBox4);
		base.Controls.Add(this.groupBox3);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.groupBox1);
		base.Name = "controller";
		this.Text = "P2 Controller";
		this.groupBox1.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.headExposureUpDown).EndInit();
		((System.ComponentModel.ISupportInitialize)this.mainExposureUpDown).EndInit();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.fUpDown).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xUpDown).EndInit();
		((System.ComponentModel.ISupportInitialize)this.yUpDown).EndInit();
		base.ResumeLayout(false);
	}
}
