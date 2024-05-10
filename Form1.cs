using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PrefetchTool
{
    
    public partial class Form1 : Form
    {
        /// <summary>
        /// Главное окно.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// БОЛЬШАЯ КНОПКА
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRebuild_Click(object sender, EventArgs e)
        {
            BtnRebuild.Enabled = false; // Отключение кнопки после нажатия
            string tool = Path.GetFullPath(@"HZDCoreTools\HZDCoreTools.exe")?.ToString(); // получаем полный путь до HZDCoreTools.exe
            string tempName = Guid.NewGuid().ToString(); // Создание уникального имени
            string path = Path.Combine(Path.GetTempPath(), tempName); // Путь к временной папке с уникальным именем
            string subpath = @"core_staging\prefetch"; // переменная для создания подпапок
            Directory.CreateDirectory($"{path}/{subpath}"); // Создание временной папки и подпапки
            
            if (File.Exists(tool))
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Title = "Pick any game archive from Packed_DX12 folder",
                    Multiselect = false,
                    Filter = "HZD binary archives|*.bin"
                };

                if (ofd.ShowDialog() == DialogResult.OK) // Выбор файла: Нажата кнопка ОК и защита от дурака не сработала
                {
                    string _path2 = Path.GetDirectoryName(ofd.FileName); // Получаем путь к папке игры из выбора
                    string _Initial = Path.Combine(_path2, "Initial.bin"); // Защита от дурака
                    string outputPath = Path.Combine(path, subpath); // Путь к временной папке
                    string _target = Path.Combine(_path2, "*.bin");
                    string targetFile = '\u0022' + _target + '\u0022'; // аргумент --input
                    string _output = Path.Combine(outputPath, "fullgame.prefetch.core");
                    string output = '\u0022' + _output + '\u0022'; // аргумент --output
                    string _binaryFile = Path.Combine(_path2, "Patch_zzzzPrefetch.bin"); // Конечный файл (абсолютный путь)
                    string workDir = Path.Combine(path, @"\"); //путь к временной рабочей директории приложения
                    string rebuilded = Path.Combine(path, @"core_staging\*.*");
                    if (File.Exists(_Initial) && Directory.Exists(_path2) && Directory.Exists(outputPath)) // Защита от дурака не сработала
                    {

                        if (File.Exists(_binaryFile)) // Проверка наличия Patch_zzzzPrefetch.bin
                        {
                            DialogResult result = MessageBox.Show(
                            "Existing Patch_zzzzPrefetch.bin will be permanently deleted! Do you wish to continue?",
                            "Patch_zzzzPrefetch.bin already exist!",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                            if (result == DialogResult.OK)
                            {
                                File.Delete(_binaryFile); // Удаление Patch_zzzzPrefetch.bin если есть.
                            }
                            else
                            {
                                Directory.Delete(path, true);
                                BtnRebuild.Enabled = true; // Включение кнопки
                                return;
                            };
                        };

                        // Первая стадия - создание fullgame.prefetch.core

                        BtnRebuild.Text = "Rebuilding..."; // Меняем текст на кнопке
                        string _args = $"--horizonzerodawn rebuildprefetch --input {targetFile} --output {output} --patchesonly --verbose"; //аргументы командной строки (параметры)
                        Execute ex = new Execute();
                        ex.Start(tool, workDir, _args);

                        // Вторая стадия - создание бинарного архива
                        
                        if (File.Exists(_output)) // Проверяем наличие сгенерированного fullgame.prefetch.core
                        {
                            string coreFile = '\u0022' + rebuilded + '\u0022'; // Аргуиент --input второй стадии (для запаковки fullgame.prefetch.core)
                            string binaryFile = '\u0022' + _binaryFile + '\u0022'; // Аргумент --output второй стадии (Patch_zzzzPrefetch.bin)
                            string args = $"--horizonzerodawn pack --input {coreFile} --output {binaryFile} --verbose"; //аргументы командной строки (параметры)
                            Execute _ex = new Execute();
                            _ex.Start(tool, workDir, args);
                            BtnRebuild.Text = "Done!"; // Готово
                            MessageBox.Show(
                            $"{_binaryFile} successfully created.",
                            $"Done!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                            Directory.Delete(path, true); // Удаление временной папки по завершению
                            Application.Exit();
                        }
                        else
                        {
                            MessageBox.Show(
                                "fullgame.prefetch.core missing or have incorrect path",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                            Directory.Delete(path, true); // Удаление временной папки при ошибке
                            BtnRebuild.Text = "Rebuild!"; // Возвращаем текст на кнопке по умолчанию
                            BtnRebuild.Enabled = true; // Включение кнопки
                            return;
                        }
                    }
                    else // Защита от дурака сработала
                    {
                        MessageBox.Show(
                            "Wrong path or non-HZD archive!",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        Directory.Delete(path, true); // Удаление временной папки при ошибке
                        BtnRebuild.Text = "Rebuild!"; // Возвращаем текст на кнопке по умолчанию
                        BtnRebuild.Enabled = true; // Включение кнопки
                        return;
                    };
                }
                else // Выбор файла: Нажата кнопка отмены или сработала защита от дурака
                {
                    Directory.Delete(path, true); // Удаление временной папки
                    BtnRebuild.Enabled = true; // Включение кнопки
                    return;
                };
            }
            else
            {
                MessageBox.Show(
                    "HZDCoreTools missing",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                Directory.Delete(path, true);
                Application.Exit();
            }
        }

    }

    public class Execute
    {
        /// <summary>
        /// Точка входа запуска консольного приложения.
        /// </summary>
        /// <param name="tool"></param>
        /// <param name="workDir"></param>
        /// <param name="args"></param>
        public void Start(string tool, string workDir, string args)
        {
            try
            {
                Process process = new Process();
                {
                    process.StartInfo.FileName = tool; //путь к приложению, которое будем запускать
                    process.StartInfo.WorkingDirectory = workDir; //путь к рабочей директории приложения
                    process.StartInfo.Arguments = args;
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception e)
            {
 //               string errlog = $"errlog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                MessageBox.Show(
                    $"Exception in Execute function! Reason: {e.Message}",
                    "Oops... Something wrong...",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                Environment.Exit(0);
            }
        }
    }
}
