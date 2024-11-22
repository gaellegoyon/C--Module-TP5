namespace RadioheadSalesDashboard
{
    partial class Form1
    {
        private LiveChartsCore.SkiaSharpView.WinForms.CartesianChart salesChart;
        private LiveChartsCore.SkiaSharpView.WinForms.CartesianChart stockChart;
        private System.Windows.Forms.Timer updateTimer;
        private System.ComponentModel.IContainer components = null;
        private Label recentSalesLabel;
        private Label totalRevenueLabel;
        private System.Windows.Forms.Button reloadStockButton;

        private void ReloadStockButton_MouseEnter(object sender, EventArgs e)
        {
            this.reloadStockButton.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void ReloadStockButton_MouseLeave(object sender, EventArgs e)
        {
            this.reloadStockButton.BackColor = System.Drawing.Color.CornflowerBlue;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.salesChart = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            this.stockChart = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.recentSalesLabel = new System.Windows.Forms.Label();
            this.totalRevenueLabel = new System.Windows.Forms.Label();
            this.reloadStockButton = new System.Windows.Forms.Button();

            // 
            // reloadStockButton
            // 
            this.reloadStockButton.Text = "Reload Stock";
            this.reloadStockButton.Location = new System.Drawing.Point(10, 490);
            this.reloadStockButton.Size = new System.Drawing.Size(120, 40);
            this.reloadStockButton.BackColor = System.Drawing.Color.CornflowerBlue;
            this.reloadStockButton.ForeColor = System.Drawing.Color.White;
            this.reloadStockButton.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.reloadStockButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reloadStockButton.FlatAppearance.BorderSize = 2;
            this.reloadStockButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.reloadStockButton.Cursor = Cursors.Hand;
            this.reloadStockButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.reloadStockButton.Padding = new Padding(4);
            this.reloadStockButton.MouseEnter += new EventHandler(this.ReloadStockButton_MouseEnter);
            this.reloadStockButton.MouseLeave += new EventHandler(this.ReloadStockButton_MouseLeave);

            this.reloadStockButton.Click += new System.EventHandler(this.ReloadStockButton_Click);
            this.Controls.Add(this.reloadStockButton);

            // 
            // salesChart
            // 
            this.salesChart.Name = "salesChart";
            this.salesChart.Size = new System.Drawing.Size(900, 300);
            this.salesChart.TabIndex = 0;
            this.salesChart.Left = 12;
            this.salesChart.Top = 12;
            this.salesChart.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);

            // 
            // stockChart
            // 
            this.stockChart.Name = "stockChart";
            this.stockChart.Size = new System.Drawing.Size(900, 300);
            this.stockChart.TabIndex = 1;
            this.stockChart.Left = 12;
            this.stockChart.Top = 320;
            this.stockChart.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);

            // 
            // updateTimer
            // 
            this.updateTimer.Interval = 5000;
            this.updateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);

            // 
            // recentSalesLabel
            // 
            this.recentSalesLabel.AutoSize = true;
            this.recentSalesLabel.Location = new System.Drawing.Point(12, 630);
            this.recentSalesLabel.Name = "recentSalesLabel";
            this.recentSalesLabel.Size = new System.Drawing.Size(100, 20);
            this.recentSalesLabel.TabIndex = 2;
            this.recentSalesLabel.Text = "Ventes recentes: 0";
            this.recentSalesLabel.ForeColor = System.Drawing.Color.White;

            // 
            // totalRevenueLabel
            // 
            this.totalRevenueLabel.AutoSize = true;
            this.totalRevenueLabel.Location = new System.Drawing.Point(12, 660);
            this.totalRevenueLabel.Name = "totalRevenueLabel";
            this.totalRevenueLabel.Size = new System.Drawing.Size(120, 20);
            this.totalRevenueLabel.TabIndex = 3;
            this.totalRevenueLabel.Text = "Revenu total : 0 €";
            this.totalRevenueLabel.ForeColor = System.Drawing.Color.White;

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 900);
            this.Controls.Add(this.salesChart);
            this.Controls.Add(this.stockChart);
            this.Controls.Add(this.recentSalesLabel);
            this.Controls.Add(this.totalRevenueLabel);
            this.Name = "Form1";
            this.Text = "Radiohead Sales Dashboard";
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
