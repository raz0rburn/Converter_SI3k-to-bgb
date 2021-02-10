using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//подключено дополнительно
using System.IO;
using System.IO.Compression;
namespace Converter
{
    
    public partial class Form1 : Form
    {
        private string exportPath;
        private string fullExportPath;
        private string fullFilePath;
        List<string> CDRfileNames = new List<string>();
        HashSet<string> convertertedFilesList = new HashSet<string>();


        public Form1()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog chooseCDR = new OpenFileDialog();
            chooseCDR.Multiselect = true;
            if (chooseCDR.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string tempFolder = System.IO.Path.GetTempPath();
                foreach (string fileName in chooseCDR.FileNames)
                {
                    if (!CDRfileNames.Any(e => e.EndsWith(fileName))) //если уже выбирали этот файл, то пропустить
                    {
                        CDRfileNames.Add(fileName);
                        //добавить в список
                        listBox1.Items.Add(fileName);
                        //добавить в listbox
                    }


                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog chooseDirectory = new FolderBrowserDialog();
            DialogResult result = chooseDirectory.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(chooseDirectory.SelectedPath))
            {
                exportPath = chooseDirectory.SelectedPath;
                //string[] files = Directory.GetFiles(chooseDirectory.SelectedPath);

                //для теста
                //System.Windows.Forms.MessageBox.Show("Directory path: " + exportPath, "Message");
                textBox1.Text = exportPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int counter = 0;
            foreach (string tempFileName in CDRfileNames)
            {
                

                
                string line;
                
                // Read the file and display it line by line.  
                System.IO.StreamReader file =
                    new System.IO.StreamReader(tempFileName);
                line = file.ReadLine(); //для пропуска первой строки
                while ((line = file.ReadLine()) != null)
                {

                    counter++;


                    LogStr testobj = new LogStr(line);
                    if (testobj.correct==true)
                    {

                    
                        
                        testobj.convert();
                        fullExportPath = exportPath + testobj.exportPath;
                        fullFilePath = fullExportPath + "\\" + testobj.fileName;


                        //добавляем все пути к сконвертированным файлам в hashset
                        convertertedFilesList.Add(fullFilePath);

                   


                        //создание файлов и иерархии папок
                        try
                        {
                            if (!Directory.Exists(fullExportPath))
                            {
                                Directory.CreateDirectory(fullExportPath);
                            }
                            StreamWriter f = new StreamWriter(fullFilePath, true);
                            f.WriteLine(testobj.entry);

                            f.Close();
                        }
                        catch
                        {
                            System.Windows.Forms.MessageBox.Show("Не могу открыть файл для записи, неверный путь: " + fullFilePath, "Message");
                        }

                            //System.Windows.Forms.MessageBox.Show("Сконвертировано, файлы записаны ", "Message");
                    }
                }
                file.Close();

            }













            progressBar1.Minimum = 1;
            progressBar1.Maximum = convertertedFilesList.Count;
            // Set the initial value of the ProgressBar.
            progressBar1.Value = 1;
            progressBar1.Step = 1;
            //архивирование файлов из списка convertertedFilesList
            foreach (string el in convertertedFilesList)    
            {

                progressBar1.PerformStep();
                //MessageBox.Show(el.Substring(el.Length - 5));
                try
                {
                    using (FileStream fs = new FileStream(el + ".zip", FileMode.Create))
                    using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create))
                    {
                        
                        
                        arch.CreateEntryFromFile(el, el.Substring(el.Length - 5));
                    }
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("че-то с архиватором: " + fullFilePath, "Message");
                }

            }
            // Удаление файлов, оставить только zip файлы
            foreach (string filesToDelete in convertertedFilesList)
            {
                File.Delete(filesToDelete);
            }


            System.Windows.Forms.MessageBox.Show("Обработано: " + counter +" строк, файлы записались,: " , "Message");

        }
    }
}
