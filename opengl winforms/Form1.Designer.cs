namespace opengl_winforms
{
    partial class Form1
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
            this.glControl1 = new OpenGL.GlControl();
            this.uiRotationBarZ = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.uiRotationBarY = new System.Windows.Forms.TrackBar();
            this.uiRotationBarX = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.uiRotationBarZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiRotationBarY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiRotationBarX)).BeginInit();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Animation = true;
            this.glControl1.AnimationTimer = false;
            this.glControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.glControl1.ColorBits = ((uint)(24u));
            this.glControl1.DepthBits = ((uint)(0u));
            this.glControl1.Location = new System.Drawing.Point(12, 12);
            this.glControl1.MultisampleBits = ((uint)(0u));
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(441, 426);
            this.glControl1.StencilBits = ((uint)(0u));
            this.glControl1.TabIndex = 0;
            // 
            // uiRotationBarZ
            // 
            this.uiRotationBarZ.LargeChange = 45;
            this.uiRotationBarZ.Location = new System.Drawing.Point(459, 393);
            this.uiRotationBarZ.Maximum = 360;
            this.uiRotationBarZ.Name = "uiRotationBarZ";
            this.uiRotationBarZ.Size = new System.Drawing.Size(329, 45);
            this.uiRotationBarZ.SmallChange = 5;
            this.uiRotationBarZ.TabIndex = 1;
            this.uiRotationBarZ.TickFrequency = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(470, 259);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Rotation";
            // 
            // uiRotationBarY
            // 
            this.uiRotationBarY.LargeChange = 45;
            this.uiRotationBarY.Location = new System.Drawing.Point(459, 342);
            this.uiRotationBarY.Maximum = 360;
            this.uiRotationBarY.Name = "uiRotationBarY";
            this.uiRotationBarY.Size = new System.Drawing.Size(329, 45);
            this.uiRotationBarY.SmallChange = 5;
            this.uiRotationBarY.TabIndex = 3;
            this.uiRotationBarY.TickFrequency = 15;
            // 
            // uiRotationBarX
            // 
            this.uiRotationBarX.LargeChange = 45;
            this.uiRotationBarX.Location = new System.Drawing.Point(459, 291);
            this.uiRotationBarX.Maximum = 360;
            this.uiRotationBarX.Name = "uiRotationBarX";
            this.uiRotationBarX.Size = new System.Drawing.Size(329, 45);
            this.uiRotationBarX.SmallChange = 5;
            this.uiRotationBarX.TabIndex = 4;
            this.uiRotationBarX.TickFrequency = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.uiRotationBarX);
            this.Controls.Add(this.uiRotationBarY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.uiRotationBarZ);
            this.Controls.Add(this.glControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.uiRotationBarZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiRotationBarY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiRotationBarX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenGL.GlControl glControl1;
        private System.Windows.Forms.TrackBar uiRotationBarZ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar uiRotationBarY;
        private System.Windows.Forms.TrackBar uiRotationBarX;
    }
}

