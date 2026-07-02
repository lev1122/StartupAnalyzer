using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StartupAnalyzer
{
    public partial class Form1 : Form
    {
        private SystemScanner scanner;
        private StartupManager manager;
        private SimpleLogger logger;
        private List<StartupItem> currentItems;

        public Form1()
        {
            InitializeComponent();

            scanner = new SystemScanner();
            manager = new StartupManager();
            logger = new SimpleLogger();

            gridItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridItems.MultiSelect = false;
            gridItems.ReadOnly = true;
            gridItems.AllowUserToAddRows = false;

            logger.LogAction("Запуск приложения", "Form1 загружена");
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            currentItems = scanner.GetStartupItems();

            gridItems.DataSource = null;
            gridItems.DataSource = currentItems;

            gridItems.Columns["Name"].HeaderText = "Название программы / Службы";
            gridItems.Columns["Path"].HeaderText = "Путь к файлу";
            gridItems.Columns["Source"].HeaderText = "Где найдено";
            gridItems.Columns["IsActive"].HeaderText = "Включена";

            gridItems.Columns["Path"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            logger.LogAction("Сканирование", $"Найдено записей: {currentItems.Count}");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridItems.SelectedRows.Count > 0)
            {
                var selectedItem = (StartupItem)gridItems.SelectedRows[0].DataBoundItem;

                var confirmResult = MessageBox.Show(
                    $"Вы уверены, что хотите удалить программу '{selectedItem.Name}' из автозагрузки?\n\nВнимание: Для некоторых записей могут потребоваться права администратора.",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    bool success = manager.RemoveItem(selectedItem);
                    if (success)
                    {
                        logger.LogAction("Удаление (Успех)", selectedItem.Name);
                        MessageBox.Show("Программа успешно удалена из автозагрузки!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnScan_Click(null, null);
                    }
                    else
                    {
                        logger.LogAction("Удаление (Ошибка)", selectedItem.Name);
                        MessageBox.Show("Не удалось удалить запись. Возможно, не хватает прав администратора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, сначала выберите программу в таблице.", "Внимание");
            }
        }
    }
}