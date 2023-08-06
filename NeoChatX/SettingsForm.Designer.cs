namespace EverChat
{
    partial class SettingsForm
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
            invisibleBox = new CheckBox();
            notifyBox = new CheckBox();
            apply = new Button();
            SuspendLayout();
            // 
            // invisibleBox
            // 
            invisibleBox.AutoSize = true;
            invisibleBox.Location = new Point(12, 28);
            invisibleBox.Name = "invisibleBox";
            invisibleBox.Size = new Size(144, 24);
            invisibleBox.TabIndex = 1;
            invisibleBox.Text = "Login as invisible";
            invisibleBox.UseVisualStyleBackColor = true;
            // 
            // notifyBox
            // 
            notifyBox.AutoSize = true;
            notifyBox.Location = new Point(12, 58);
            notifyBox.Name = "notifyBox";
            notifyBox.Size = new Size(341, 24);
            notifyBox.TabIndex = 2;
            notifyBox.Text = "Flash taskbar on new message when unfocused";
            notifyBox.UseVisualStyleBackColor = true;
            // 
            // apply
            // 
            apply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            apply.ImageAlign = ContentAlignment.BottomRight;
            apply.Location = new Point(297, 151);
            apply.Name = "apply";
            apply.Size = new Size(144, 29);
            apply.TabIndex = 3;
            apply.Text = "Apply and close";
            apply.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(453, 192);
            ControlBox = false;
            Controls.Add(apply);
            Controls.Add(notifyBox);
            Controls.Add(invisibleBox);
            Name = "SettingsForm";
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox invisibleBox;
        private CheckBox notifyBox;
        private Button apply;
    }
}