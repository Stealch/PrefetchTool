
namespace PrefetchTool
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.BtnRebuild = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnRebuild
            // 
            this.BtnRebuild.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnRebuild.Location = new System.Drawing.Point(36, 32);
            this.BtnRebuild.Name = "BtnRebuild";
            this.BtnRebuild.Size = new System.Drawing.Size(209, 109);
            this.BtnRebuild.TabIndex = 0;
            this.BtnRebuild.Text = "Rebuild!";
            this.BtnRebuild.UseVisualStyleBackColor = true;
            this.BtnRebuild.Click += new System.EventHandler(this.BtnRebuild_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 171);
            this.Controls.Add(this.BtnRebuild);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Prefetch Rebuilding Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnRebuild;
    }
}

