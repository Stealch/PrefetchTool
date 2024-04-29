using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PrefetchTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void BtnRebuild_Click(object sender, EventArgs e)
        {
            BtnRebuild.Enabled = false; // Отключение кнопки после нажатия
            string tool = Path.GetFullPath(@"HZDCoreTools\HZDCoreTools.exe")?.ToString(); // получаем полный путь до HZDCoreTools.exe
            string path = Path.GetFullPath(@"HZDCoreTools")?.ToString(); // получаем полный путь до папки HZDCoreTools
            string subpath = @"core_staging\prefetch"; // переменная для создания подпапок
            string _tempFolder = Path.GetFullPath(@"HZDCoreTools\core_staging")?.ToString(); // Получаем путь к временной папке, которая могла остаться от предыдущего запуска или предыдущего релиза
            if (Directory.Exists(_tempFolder)) // Проверка наличия не нужной временной папки
            {
                Directory.Delete(_tempFolder, true); // Удаление временной папки если она по какой-то причине осталась от предыдущего запуска или предыдущего релиза
            };
                        

            if (!File.Exists(tool) || !Directory.Exists(path))
            {
                MessageBox.Show(
                    "HZDCoreTools missing",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                BtnRebuild.Enabled = true; // Включение кнопки
                return;
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Title = "Pick any game archive from Packed_DX12 folder",
                    Multiselect = false,
                    Filter = "HZD binary archives|*.bin"
                };

                if (ofd.ShowDialog() == DialogResult.OK) // Выбор файла: Нажата кнопка ОК и защита от дурака не сработала
                {

                    Directory.CreateDirectory($"{path}/{subpath}"); // Создание временной папки и подпапки
                    string _path2 = Path.GetDirectoryName(ofd.FileName)?.ToString(); // Получаем путь к папке игры из выбора
                    string _Initial = _path2 + (@"\Initial.bin"); // Защита от дурака
                    string outputFile = Path.GetFullPath(@"HZDCoreTools\core_staging\prefetch")?.ToString();
                    string _target = @"\*.bin";
                    string _targetFile = '\u0022' + _path2 + _target + '\u0022'; // заключаем путь до целевого файла в кавычки
                    string _output = @"\fullgame.prefetch.core";
                    string _prefetch = @"\Patch_zzzzPrefetch.bin"; // Конечный файл
                    string mustBeDeleted = _path2 + _prefetch;
                    string _remove = Path.GetFullPath(@"HZDCoreTools\core_staging")?.ToString();
                    string workDir = Path.GetFullPath(@"HZDCoreTools\")?.ToString(); //путь к рабочей директории приложения
                    if (File.Exists(_Initial) && _path2 != null && outputFile != null) // Защита от дурака не сработала
                    {

                        if (File.Exists(mustBeDeleted)) // Проверка наличия Patch_zzzzPrefetch.bin
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
                                File.Delete(mustBeDeleted); // Удаление Patch_zzzzPrefetch.bin если есть.
                            }
                            else
                            {
                                if (_remove != null)
                                {
                                    Directory.Delete(_remove, true); // Удаление временной папки в случае отказа
                                    BtnRebuild.Enabled = true; // Включение кнопки
                                    return;
                                };
                                BtnRebuild.Enabled = true; // Включение кнопки
                                return;
                            };
                        };

                        // Первая стадия - создание fullgame.prefetch.core

                        BtnRebuild.Text = "Rebuilding!"; // Меняем текст на кнопке
                        Process process = new Process();
                        {
                            process.StartInfo.FileName = tool; //путь к приложению, которое будем запускать
                            process.StartInfo.WorkingDirectory = workDir; //путь к рабочей директории приложения
                            process.StartInfo.Arguments = "--horizonzerodawn " + "rebuildprefetch " + "--input " + _targetFile + " " + "--output " + outputFile + _output + " " + "--patchesonly " + "--verbose"; //аргументы командной строки (параметры)
                            process.Start();
                            process.WaitForExit();
                        };
                        // Вторая стадия - создание бинарного архива
                        string _prefetchTMP = Path.GetFullPath(@"HZDCoreTools\core_staging\prefetch\fullgame.prefetch.core")?.ToString();
                        if (!File.Exists(_prefetchTMP)) // Проверяем наличие сгенерированного fullgame.prefetch.core
                        {
                            MessageBox.Show(
                                "fullgame.prefetch.core missing or have incorrect path",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                            Directory.Delete(_remove, true); // Удаление временной папки при ошибке
                            BtnRebuild.Text = "Rebuild!"; // Возвращаем текст на кнопке по умолчанию
                            BtnRebuild.Enabled = true; // Включение кнопки
                            return;
                        }
                        else
                        {
                            string coreFile = Path.GetFullPath(@"HZDCoreTools\core_staging\prefetch\fullgame.prefetch.core")?.ToString(); // Получаем полный путь к fullgame.prefetch.core
                            string _coreFile = '\u0022' + coreFile + '\u0022'; // Заключаем путь к fullgame.prefetch.core в кавычки для обработки возможных экзотичных путей
                            string binaryFile = '\u0022' + _path2 + _prefetch + '\u0022';
                            Process process1 = new Process();
                            {
                                process1.StartInfo.FileName = tool; //путь к приложению, которое будем запускать
                                process1.StartInfo.WorkingDirectory = workDir; //путь к рабочей директории приложения
                                process1.StartInfo.Arguments = "--horizonzerodawn " + "pack " + "--input " + _coreFile + " " + "--output " + binaryFile + " " + "--verbose"; //аргументы командной строки (параметры)
                                process1.Start();
                                process1.WaitForExit();
                                MessageBox.Show(
                                    "Rebuild complete!",
                                    "Success",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1,
                                    MessageBoxOptions.DefaultDesktopOnly);

                                Directory.Delete(_remove, true); // Удаление временной папки по завершению
                                BtnRebuild.Enabled = true; // Включение кнопки
                                BtnRebuild.Text = "Rebuild!"; // Возвращаем текст на кнопке по умолчанию
                                Application.Exit();
                            };
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
                            Directory.Delete(_remove, true); // Удаление временной папки при ошибке
                            BtnRebuild.Text = "Rebuild!"; // Возвращаем текст на кнопке по умолчанию
                            BtnRebuild.Enabled = true; // Включение кнопки
                            return;
                    };
                }   
                else // Выбор файла: Нажата кнопка отмены или сработала защита от дурака
                {
                    BtnRebuild.Enabled = true; // Включение кнопки
                    return;
                };
            }
        }

    }
}
