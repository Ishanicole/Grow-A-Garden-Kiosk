using System;

namespace ANCAYAN_BAUTISTA
{
    partial class receipt
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
            this.rtbReceipt = new System.Windows.Forms.RichTextBox();
            this.lblTotalReceipt = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbReceipt
            // 
            this.rtbReceipt.BackColor = System.Drawing.Color.White;
            this.rtbReceipt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbReceipt.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbReceipt.Font = new System.Drawing.Font("Consolas", 10F);
            this.rtbReceipt.ForeColor = System.Drawing.Color.Black;
            this.rtbReceipt.Location = new System.Drawing.Point(26, 27);
            this.rtbReceipt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rtbReceipt.Name = "rtbReceipt";
            this.rtbReceipt.ReadOnly = true;
            this.rtbReceipt.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbReceipt.Size = new System.Drawing.Size(489, 560);
            this.rtbReceipt.TabIndex = 0;
            this.rtbReceipt.TabStop = false;
            this.rtbReceipt.Text = "";
            this.rtbReceipt.TextChanged += new System.EventHandler(this.rtbReceipt_TextChanged);
            // 
            // lblTotalReceipt
            // 
            this.lblTotalReceipt.AutoSize = true;
            this.lblTotalReceipt.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalReceipt.ForeColor = System.Drawing.Color.Sienna;
            this.lblTotalReceipt.Location = new System.Drawing.Point(26, 600);
            this.lblTotalReceipt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalReceipt.Name = "lblTotalReceipt";
            this.lblTotalReceipt.Size = new System.Drawing.Size(64, 28);
            this.lblTotalReceipt.TabIndex = 1;
            this.lblTotalReceipt.Text = "Total:";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Sienna;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(193, 640);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(154, 53);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // receipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(540, 733);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblTotalReceipt);
            this.Controls.Add(this.rtbReceipt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "receipt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Receipt";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.RichTextBox rtbReceipt;
        private System.Windows.Forms.Label lblTotalReceipt;
        private System.Windows.Forms.Button btnClose;
    }
}