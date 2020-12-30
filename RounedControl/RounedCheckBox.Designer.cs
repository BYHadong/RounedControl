﻿namespace RounedControl
{
    public partial class RounedCheckBox
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkLabel = new RounedControl.RounedLabel();
            this.checkBtn = new RounedControl.RounedButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.48598F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.51402F));
            this.tableLayoutPanel1.Controls.Add(this.checkLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBtn, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(248, 74);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // checkLabel
            // 
            this.checkLabel.AutoSize = true;
            this.checkLabel.BackColor = System.Drawing.Color.Transparent;
            this.checkLabel.BorderColor = System.Drawing.Color.Black;
            this.checkLabel.BorderSize = 5;
            this.checkLabel.BorderVisible = true;
            this.checkLabel.BoxColor = System.Drawing.Color.White;
            this.checkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkLabel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.checkLabel.Location = new System.Drawing.Point(3, 3);
            this.checkLabel.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.checkLabel.Name = "checkLabel";
            this.checkLabel.Radius = 15;
            this.checkLabel.Size = new System.Drawing.Size(156, 68);
            this.checkLabel.TabIndex = 0;
            this.checkLabel.Text = "checkLabel";
            this.checkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkLabel.Click += new System.EventHandler(this.checkLabel_Click);
            // 
            // checkBtn
            // 
            this.checkBtn.BackColor = System.Drawing.Color.LightGray;
            this.checkBtn.BorderColor = System.Drawing.Color.Black;
            this.checkBtn.BorderSize = 0;
            this.checkBtn.BorderVisible = false;
            this.checkBtn.Clicked = false;
            this.checkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBtn.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.checkBtn.ForeColor = System.Drawing.Color.Black;
            this.checkBtn.Location = new System.Drawing.Point(160, 3);
            this.checkBtn.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.checkBtn.Name = "checkBtn";
            this.checkBtn.Radius = 15;
            this.checkBtn.SelectButtonStyle = RounedControl.RounedButton.ButtonStyle.CheckBox;
            this.checkBtn.Size = new System.Drawing.Size(85, 68);
            this.checkBtn.TabIndex = 1;
            this.checkBtn.TextAlignFormat = System.Drawing.StringAlignment.Center;
            this.checkBtn.UseVisualStyleBackColor = false;
            // 
            // RounedCheckBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RounedCheckBox";
            this.Size = new System.Drawing.Size(248, 74);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RounedLabel checkLabel;
        private RounedButton checkBtn;
    }
}
