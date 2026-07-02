namespace StartupAnalyzer
{
    partial class MainForm
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
            this.gridItems = new System.Windows.Forms.DataGridView();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnOpenLogs = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridItems)).BeginInit();
            this.SuspendLayout();
            // 
            // gridItems
            // 
            this.gridItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridItems.Location = new System.Drawing.Point(92, 48);
            this.gridItems.Name = "gridItems";
            this.gridItems.Size = new System.Drawing.Size(1010, 286);
            this.gridItems.TabIndex = 0;
            this.gridItems.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridItems_CellDoubleClick);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(92, 412);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(179, 58);
            this.btnScan.TabIndex = 1;
            this.btnScan.Text = "Пуск";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(914, 412);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(188, 58);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Удалить выбранное\r\n";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnOpenLogs
            // 
            this.btnOpenLogs.Location = new System.Drawing.Point(583, 18);
            this.btnOpenLogs.Name = "btnOpenLogs";
            this.btnOpenLogs.Size = new System.Drawing.Size(145, 23);
            this.btnOpenLogs.TabIndex = 5;
            this.btnOpenLogs.Text = "Журнал действий";
            this.btnOpenLogs.UseVisualStyleBackColor = true;
            this.btnOpenLogs.Click += new System.EventHandler(this.btnOpenLogs_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(484, 18);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "Очистить";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1202, 516);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnOpenLogs);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.gridItems);
            this.Name = "MainForm";
            this.Text = "Анализатор автозагрузки";
            ((System.ComponentModel.ISupportInitialize)(this.gridItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridItems;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnOpenLogs;
        private System.Windows.Forms.Button btnClear;
    }
}

