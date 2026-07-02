using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StartupAnalyzer
{
    public partial class MainForm : Form
    {
        private Scanner scanner;
        private AutorunController manager;
        private Logger logger;
        private List<Model> currentItems;

        public MainForm()
        {
            InitializeComponent();

            scanner = new Scanner();
            manager = new AutorunController();
            logger = new Logger();

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

            if (gridItems.Columns["Pid"] != null)
                gridItems.Columns["Pid"].Visible = false;

            if (gridItems.Columns["MemoryUsage"] != null)
                gridItems.Columns["MemoryUsage"].Visible = false;

            if (gridItems.Columns["CpuUsage"] != null)
                gridItems.Columns["CpuUsage"].Visible = false;

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
                var selectedItem = (Model)gridItems.SelectedRows[0].DataBoundItem;

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

        private void btnClear_Click(object sender, EventArgs e)
        {
            gridItems.DataSource = null;

            logger.LogAction("Очистка экрана", "Пользователь очистил таблицу вывода");
        }

        private void btnOpenLogs_Click(object sender, EventArgs e)
        {
            string logFile = "audit_logs.txt";

            if (System.IO.File.Exists(logFile))
            {
                System.Diagnostics.Process.Start("notepad.exe", logFile);
            }
            else
            {
                MessageBox.Show("Журнал действий пока пуст. Выполните сканирование или удаление, чтобы создать первую запись.",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gridItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var item = (Model)gridItems.Rows[e.RowIndex].DataBoundItem;

                string ramInfo = "Не запущен";
                string cpuPerc = "0%";
                int pid = 0;
                string location = "Нет данных";

                try
                {
                    string cleanPath = item.Path.Replace("\"", "").Trim();
                    if (cleanPath.Contains(" -")) cleanPath = cleanPath.Substring(0, cleanPath.IndexOf(" -"));

                    location = System.IO.Path.GetDirectoryName(cleanPath);
                    string procName = System.IO.Path.GetFileNameWithoutExtension(cleanPath);
                    var procs = System.Diagnostics.Process.GetProcessesByName(procName);

                    if (procs.Length > 0)
                    {
                        var p = procs[0];
                        pid = p.Id;
                        ramInfo = (p.WorkingSet64 / 1024 / 1024).ToString() + " МБ";

                        var t1 = p.TotalProcessorTime;
                        System.Threading.Thread.Sleep(500);
                        var t2 = p.TotalProcessorTime;
                        double cpu = (t2 - t1).TotalMilliseconds / 100.0 / Environment.ProcessorCount * 100;
                        cpuPerc = cpu.ToString("F1") + "%";
                    }
                }
                catch { }

                string info = $"ПРОЦЕСС: {item.Name}\n" +
                              $"РАСПОЛОЖЕНИЕ: {location}\n\n" +
                              $"ДОПОЛНИТЕЛЬНО:\n" +
                              $"PID процесса: {pid}\n" +
                              $"Потребление RAM: {ramInfo}\n" +
                              $"Нагрузка CPU: {cpuPerc}";

                MessageBox.Show(info, "Подробная информация");
            }
        }
    }
}