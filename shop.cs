using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ANCAYAN_BAUTISTA
{
    public partial class shop : Form
    {
        private List<Item> allItems = new List<Item>();
        private int sortColumnIndex = -1;
        private SortOrder sortOrder = SortOrder.Ascending;

        public shop()
        {
            InitializeComponent();
            InitializeCategoryButtons();
            AttachItemButtonEvents();
            SetupDataGridView();
            SetupButtonClickEffects(); 
            ShowCategory("All");

            checkoutbtn.Click += btnCheckout_Click;
            clearbtn.Click += btnClear_Click;

            pictureBox33.Parent = pictureBox34;
            pictureBox33.BackColor = Color.Transparent;
        }

        // ==================== DATAGRIDVIEW SETUP ====================
        private void SetupDataGridView()
        {
            if (dgvPOS == null) return;

            dgvPOS.AllowUserToAddRows = false;
            dgvPOS.AllowUserToDeleteRows = false;
            dgvPOS.RowHeadersVisible = false;
            dgvPOS.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPOS.MultiSelect = false;
            dgvPOS.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvPOS.Columns.Clear();

            // Image column
            DataGridViewImageColumn imgColumn = new DataGridViewImageColumn
            {
                Name = "Image",
                HeaderText = "Image",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 60
            };
            dgvPOS.Columns.Add(imgColumn);

            dgvPOS.Columns.Add("Item", "Item");
            dgvPOS.Columns.Add("Qty", "Qty");
            dgvPOS.Columns["Qty"].Width = 50;
            dgvPOS.Columns.Add("Price", "Price");
            dgvPOS.Columns["Price"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Increase button
            DataGridViewButtonColumn increaseBtn = new DataGridViewButtonColumn
            {
                Name = "Increase",
                HeaderText = "",
                Text = "+",
                UseColumnTextForButtonValue = true,
                Width = 40
            };
            dgvPOS.Columns.Add(increaseBtn);

            // Decrease button
            DataGridViewButtonColumn decreaseBtn = new DataGridViewButtonColumn
            {
                Name = "Decrease",
                HeaderText = "",
                Text = "-",
                UseColumnTextForButtonValue = true,
                Width = 40
            };
            dgvPOS.Columns.Add(decreaseBtn);

            dgvPOS.CellClick += DgvPOS_CellClick;
            dgvPOS.ColumnHeaderMouseClick += DgvPOS_ColumnHeaderMouseClick;
            dgvPOS.CellPainting += DgvPOS_CellPainting; // circular ± buttons
        }

        // ==================== CATEGORY BUTTONS ====================
        private void InitializeCategoryButtons()
        {
            if (button1 != null) button1.Click += (s, e) => ShowCategory("All");
            if (btnGearShop != null) btnGearShop.Click += (s, e) => ShowCategory("Gear Shop");
            if (btnSeedShop != null) btnSeedShop.Click += (s, e) => ShowCategory("Seed Shop");
            if (btnPetEggShop != null) btnPetEggShop.Click += (s, e) => ShowCategory("Pet Egg Shop");
        }

        private void ShowCategory(string category)
        {
            if (FlowLayoutPanel == null) return;
            FlowLayoutPanel.SuspendLayout();

            foreach (var panel in FlowLayoutPanel.Controls.OfType<Panel>())
            {
                int panelNumber = GetPanelNumber(panel.Name);
                bool visible = false;
                switch (category)
                {
                    case "All": visible = true; break;
                    case "Gear Shop": visible = panelNumber >= 3 && panelNumber <= 17; break;
                    case "Seed Shop": visible = panelNumber >= 18 && panelNumber <= 31; break;
                    case "Pet Egg Shop": visible = panelNumber >= 32 || panelNumber == 0; break;
                }
                panel.Visible = visible;
            }

            FlowLayoutPanel.ResumeLayout();
            FlowLayoutPanel.Refresh();
        }

        private int GetPanelNumber(string name)
        {
            if (string.IsNullOrEmpty(name)) return 0;
            string digits = new string(name.Where(char.IsDigit).ToArray());
            if (int.TryParse(digits, out int num)) return num;
            return 0;
        }

        // ==================== ITEM BUTTON EVENTS ====================
        private void AttachItemButtonEvents()
        {
            foreach (var btn in GetAllControls(this).OfType<Button>())
            {
                if (btn.Tag != null && btn.Tag.ToString().Contains("|"))
                {
                    btn.Click -= ItemButton_Click;
                    btn.Click += ItemButton_Click;
                }
            }
        }

        private IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                yield return c;
                foreach (var child in GetAllControls(c))
                    yield return child;
            }
        }

        private void ItemButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                string[] parts = btn.Tag.ToString().Split('|');
                if (parts.Length >= 2)
                {
                    string itemName = parts[0].Trim();
                    string priceText = parts[1].Trim().Replace(",", "");
                    if (decimal.TryParse(priceText, out decimal price))
                    {
                        var item = new Item(itemName, "Unknown", null, price);
                        AddToCartGrid(item);
                    }
                }
            }
        }

        // ==================== CART SYSTEM ====================
        private void AddToCartGrid(Item item)
        {
            foreach (DataGridViewRow row in dgvPOS.Rows)
            {
                if (row.Cells["Item"].Value.ToString().Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["Qty"].Value = Convert.ToInt32(row.Cells["Qty"].Value) + 1;
                    UpdateCartTotalGrid();
                    return;
                }
            }

            Image img = null;
            if (imageList1 != null && imageList1.Images.Count > 0)
            {
                for (int i = 0; i < imageList1.Images.Count; i++)
                {
                    if (string.Equals(imageList1.Images.Keys[i], item.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        img = imageList1.Images[i];
                        break;
                    }
                }
            }

            dgvPOS.Rows.Add(img, item.Name, 1, item.Price);
            UpdateCartTotalGrid();
        }

        private void DgvPOS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dgvPOS.Columns["Increase"].Index)
            {
                int qty = Convert.ToInt32(dgvPOS.Rows[e.RowIndex].Cells["Qty"].Value);
                dgvPOS.Rows[e.RowIndex].Cells["Qty"].Value = qty + 1;
            }
            else if (e.ColumnIndex == dgvPOS.Columns["Decrease"].Index)
            {
                int qty = Convert.ToInt32(dgvPOS.Rows[e.RowIndex].Cells["Qty"].Value);
                if (qty > 1)
                    dgvPOS.Rows[e.RowIndex].Cells["Qty"].Value = qty - 1;
                else
                    dgvPOS.Rows.RemoveAt(e.RowIndex);
            }

            UpdateCartTotalGrid();
        }

        private void UpdateCartTotalGrid()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvPOS.Rows)
            {
                decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                int qty = Convert.ToInt32(row.Cells["Qty"].Value);
                total += price * qty;
            }
            lbTotal.Text = $"{total:N0}₵";
        }

        // ==================== SORTABLE COLUMNS ====================
        private void DgvPOS_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var column = dgvPOS.Columns[e.ColumnIndex];
            if (column.Name != "Item" && column.Name != "Qty" && column.Name != "Price") return;

            if (e.ColumnIndex == sortColumnIndex)
                sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            else
            {
                sortColumnIndex = e.ColumnIndex;
                sortOrder = SortOrder.Ascending;
            }

            List<DataGridViewRow> rows = dgvPOS.Rows.Cast<DataGridViewRow>().ToList();

            if (column.Name == "Item")
                rows = sortOrder == SortOrder.Ascending
                    ? rows.OrderBy(r => r.Cells["Item"].Value.ToString()).ToList()
                    : rows.OrderByDescending(r => r.Cells["Item"].Value.ToString()).ToList();
            else if (column.Name == "Qty")
                rows = sortOrder == SortOrder.Ascending
                    ? rows.OrderBy(r => Convert.ToInt32(r.Cells["Qty"].Value)).ToList()
                    : rows.OrderByDescending(r => Convert.ToInt32(r.Cells["Qty"].Value)).ToList();
            else if (column.Name == "Price")
                rows = sortOrder == SortOrder.Ascending
                    ? rows.OrderBy(r => Convert.ToDecimal(r.Cells["Price"].Value)).ToList()
                    : rows.OrderByDescending(r => Convert.ToDecimal(r.Cells["Price"].Value)).ToList();

            dgvPOS.Rows.Clear();
            dgvPOS.Rows.AddRange(rows.ToArray());
            column.HeaderCell.SortGlyphDirection = sortOrder;
        }

        // ==================== REDESIGNED ± BUTTONS ====================
        private void DgvPOS_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dgvPOS.Columns["Increase"].Index || e.ColumnIndex == dgvPOS.Columns["Decrease"].Index)
            {
                e.PaintBackground(e.CellBounds, true);

                int radius = 6; // rounded corner radius
                Rectangle rect = new Rectangle(e.CellBounds.X + 2, e.CellBounds.Y + 2,
                                               e.CellBounds.Width - 4, e.CellBounds.Height - 4);

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);


                    Color btnColor = e.ColumnIndex == dgvPOS.Columns["Increase"].Index ? Color.LightGreen : Color.LightCoral;
                    using (SolidBrush brush = new SolidBrush(btnColor))
                    {
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        e.Graphics.FillPath(brush, path);
                    }

                    string text = e.ColumnIndex == dgvPOS.Columns["Increase"].Index ? "+" : "-";
                    TextRenderer.DrawText(e.Graphics, text, new Font("Segoe UI", 10, FontStyle.Bold),
                                          rect, Color.Black,
                                          TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }

                e.Handled = true;
            }
        }


        // ==================== PANEL BUTTON CLICK EFFECTS ====================
        private void SetupButtonClickEffects()
        {
            void ConfigureButton(Button btn, Color clickColor)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s, e) =>
                {
                    Color original = btn.BackColor;
                    btn.BackColor = clickColor;
                    Timer t = new Timer { Interval = 150 };
                    t.Tick += (s2, e2) =>
                    {
                        btn.BackColor = original;
                        t.Stop();
                        t.Dispose();
                    };
                    t.Start();
                };
            }

            foreach (Button btn in panel1.Controls.OfType<Button>())
                ConfigureButton(btn, Color.SaddleBrown);

            foreach (Button btn in FlowLayoutPanel.Controls.OfType<Button>())
                ConfigureButton(btn, Color.SaddleBrown);

            foreach (Button btn in panel2.Controls.OfType<Button>())
                ConfigureButton(btn, Color.AntiqueWhite);
        }

        // ==================== CHECKOUT BUTTON ====================
        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (dgvPOS.Rows.Count == 0)
            {
                MessageBox.Show("Cart is empty!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to proceed?", "Checkout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ListView tempListView = new ListView();
                foreach (DataGridViewRow row in dgvPOS.Rows)
                {
                    ListViewItem item = new ListViewItem(row.Cells["Item"].Value.ToString());
                    item.SubItems.Add(row.Cells["Qty"].Value.ToString());
                    item.SubItems.Add($"{row.Cells["Price"].Value:N0}₵");
                    tempListView.Items.Add(item);
                }

                decimal total = 0;
                foreach (DataGridViewRow row in dgvPOS.Rows)
                {
                    decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                    int qty = Convert.ToInt32(row.Cells["Qty"].Value);
                    total += price * qty;
                }
                this.Close();
                receipt rForm = new receipt(tempListView.Items, total);
                rForm.ShowDialog();
            }
        }

        // ==================== CLEAR BUTTON ====================
        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvPOS.Rows.Clear();
            lbTotal.Text = "0₵";
        }

        private void pictureBox33_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox33_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }
    }

    // ==================== ITEM CLASS ====================
    public class Item
    {
        public string Name { get; }
        public string Category { get; }
        public Image Image { get; }
        public decimal Price { get; }

        public Item(string name, string category, Image image, decimal price)
        {
            Name = name;
            Category = category;
            Image = image;
            Price = price;
        }
    }
}
