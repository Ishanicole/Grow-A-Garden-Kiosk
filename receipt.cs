using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ANCAYAN_BAUTISTA
{
    public partial class receipt : Form
    {
        private ListView.ListViewItemCollection items;
        private decimal total;
    

        public receipt(ListView.ListViewItemCollection items, decimal total)
        {
            // Remove InitializeComponent() — we build controls manually
            this.items = items;
            this.total = total;

            InitializeReceiptUI();  // custom setup method
            GenerateReceipt(items, total);
        }

        private void InitializeReceiptUI()
        {
            // === Window Settings ===
            this.Text = "Receipt";
            this.Size = new Size(420, 550);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // === RichTextBox ===
            rtbReceipt = new RichTextBox
            {
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Consolas", 10),
                Dock = DockStyle.Fill,
                TabStop = false,
                Cursor = Cursors.Default,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            // === Close Button ===
            btnClose = new Button
            {
                Text = "Close",
                Dock = DockStyle.Bottom,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Sienna,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += btnClose_Click;

            // === Add Controls ===
            this.Controls.Add(rtbReceipt);
            this.Controls.Add(btnClose);
        }

        private void GenerateReceipt(ListView.ListViewItemCollection items, decimal total)
        {
            StringBuilder receiptText = new StringBuilder();
            string shopName = "🌿 GROW A GARDEN SHOPIFIED 🌿";
            int totalWidth = 58;

            // --- HEADER ---
            receiptText.AppendLine(shopName.PadLeft((totalWidth + shopName.Length) / 2));
            receiptText.AppendLine();
            receiptText.AppendLine($"Order # {new Random().Next(1000, 9999)}");
            receiptText.AppendLine($"Date: {DateTime.Now:MMMM dd, yyyy}");
            receiptText.AppendLine("---------------------------------------------------------");
            receiptText.AppendLine(string.Format("{0,-8}{1,-32}{2,12}", "QTY", "ITEM", "PRICE"));
            receiptText.AppendLine("---------------------------------------------------------");

            int totalItems = 0;

            // --- BODY ---
            foreach (ListViewItem item in items)
            {
                string itemName = item.SubItems[0].Text;
                string qtyText = item.SubItems[1].Text;
                string priceText = item.SubItems[2].Text;

                if (int.TryParse(qtyText, out int qty))
                    totalItems += qty;

                receiptText.AppendLine(string.Format("{0,-8}{1,-32}{2,12}", qtyText, itemName, priceText));
            }

            // --- FOOTER ---
            receiptText.AppendLine("---------------------------------------------------------");
            receiptText.AppendLine(string.Format("{0,-20}{1,35}", "Item count:", totalItems));
            receiptText.AppendLine(string.Format("{0,-20}{1,35}", "Total:", $"{total:0.##}¢"));
            receiptText.AppendLine("---------------------------------------------------------");
            receiptText.AppendLine();
            receiptText.AppendLine("THANK YOU FOR PURCHASING!".PadLeft((totalWidth + 25) / 2));
            receiptText.AppendLine("PLEASE BUY AGAIN 🌸".PadLeft((totalWidth + 10) / 2));

            rtbReceipt.Text = receiptText.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 mainForm = new Form1();
            mainForm.Show();
        }

        private void rtbReceipt_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
