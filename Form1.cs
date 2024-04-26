﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

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
            string tool = Path.GetFullPath(@"HZDCoreTools\HZDCoreTools.exe"); // получаем полный путь до HZDCoreTools.exe
            string path = Path.GetFullPath(@"HZDCoreTools"); // получаем полный путь до папки HZDCoreTools
            string subpath = @"core_staging\prefetch"; // переменная для создания подпапок

            if (!File.Exists(tool) || !Directory.Exists(path))
            {
                MessageBox.Show(
                    "HZDCoreTools missing",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                Directory.CreateDirectory($"{path}/{subpath}");
                
                string outputFile = Path.GetFullPath(@"HZDCoreTools\core_staging\prefetch");
               // string currientToolCommand = tool + " " + "--horizonzerodawn ";
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Title = "Pick any game archive from Packed_DX12 folder",
                    Multiselect = false,
                    Filter = "HZD binary archives|*.bin"
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // string _path = '\u0022' + Path.GetDirectoryName(ofd.FileName) + '\u0022'; // получаем путь к папке игры из выбора и заключаем в кавычки
                    string _path2 = Path.GetDirectoryName(ofd.FileName); // получаем путь к папке игры из выбора
                    string _target = @"\*.bin";
                    string _targetFile = '\u0022' + _path2 + _target + '\u0022'; // заключаем путь до целевого файла в кавычки
                    string _output = @"\fullgame.prefetch.core";

                                                                                // Первая стадия - создание fullgame.prefetch.core
                    Process process = new Process();
                    {
                        process.StartInfo.FileName = tool; //путь к приложению, которое будем запускать
                        process.StartInfo.WorkingDirectory = Path.GetFullPath(@"HZDCoreTools\"); //путь к рабочей директории приложения
                        process.StartInfo.Arguments = "--horizonzerodawn " + "rebuildprefetch " + "--input " + _targetFile + " " + "--output " + outputFile + _output + " " + "--patchesonly " + "--verbose"; //аргументы командной строки (параметры)
                        process.Start();
                        process.WaitForExit();
                    };
                                                                                   // Вторая стадия - создяние бинарного архива
                    string outBinary = @"\Patch_zzzzPrefetch.bin";
                    string coreFile = Path.GetFullPath(@"HZDCoreTools\core_staging\prefetch\fullgame.prefetch.core");
                    string binaryFile = '\u0022' + _path2 + outBinary + '\u0022';
                    Process process1 = new Process();
                    {
                        process1.StartInfo.FileName = tool; //путь к приложению, которое будем запускать
                        process1.StartInfo.WorkingDirectory = Path.GetFullPath(@"HZDCoreTools"); //путь к рабочей директории приложения
                        process1.StartInfo.Arguments = "--horizonzerodawn " + "pack " + "--input " + coreFile + " " + "--output " + binaryFile + " " + "--verbose"; //аргументы командной строки (параметры)
                        process1.Start();
                        process1.WaitForExit();
                        MessageBox.Show(
                            "Rebuild complete!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                       
                        Directory.Delete(outputFile, true);
                    };
                }
                else
                {

                };
            }
        }

    }
}
