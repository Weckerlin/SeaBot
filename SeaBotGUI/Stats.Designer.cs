namespace SeaBotGUI
{
    partial class Stats
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Stats));
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.radio_res = new System.Windows.Forms.RadioButton();
            this.btn_zoomreset = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radio_days = new System.Windows.Forms.RadioButton();
            this.radio_hours = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cartesianChart1
            // 
            resources.ApplyResources(this.cartesianChart1, "cartesianChart1");
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.ChildChanged += new System.EventHandler<System.Windows.Forms.Integration.ChildChangedEventArgs>(this.CartesianChart1_ChildChanged);
            // 
            // radio_res
            // 
            resources.ApplyResources(this.radio_res, "radio_res");
            this.radio_res.Checked = true;
            this.radio_res.Name = "radio_res";
            this.radio_res.TabStop = true;
            this.radio_res.UseVisualStyleBackColor = true;
            this.radio_res.CheckedChanged += new System.EventHandler(this.Radio_res_CheckedChanged);
            // 
            // btn_zoomreset
            // 
            resources.ApplyResources(this.btn_zoomreset, "btn_zoomreset");
            this.btn_zoomreset.Name = "btn_zoomreset";
            this.btn_zoomreset.UseVisualStyleBackColor = true;
            this.btn_zoomreset.Click += new System.EventHandler(this.Btn_zoomreset_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radio_res);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.radio_days);
            this.groupBox2.Controls.Add(this.radio_hours);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // radio_days
            // 
            resources.ApplyResources(this.radio_days, "radio_days");
            this.radio_days.Name = "radio_days";
            this.radio_days.UseVisualStyleBackColor = true;
            this.radio_days.CheckedChanged += new System.EventHandler(this.Radio_days_CheckedChanged);
            // 
            // radio_hours
            // 
            resources.ApplyResources(this.radio_hours, "radio_hours");
            this.radio_hours.Checked = true;
            this.radio_hours.Name = "radio_hours";
            this.radio_hours.TabStop = true;
            this.radio_hours.UseVisualStyleBackColor = true;
            this.radio_hours.CheckedChanged += new System.EventHandler(this.Radio_hours_CheckedChanged);
            // 
            // Stats
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_zoomreset);
            this.Controls.Add(this.cartesianChart1);
            this.Name = "Stats";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private System.Windows.Forms.RadioButton radio_res;
        private System.Windows.Forms.Button btn_zoomreset;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radio_days;
        private System.Windows.Forms.RadioButton radio_hours;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}