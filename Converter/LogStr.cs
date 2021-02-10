using System;
using System.Collections.Generic;
using System.Text;

namespace Converter
{
    class LogStr
    {
        private DateTime startTime;
        private DateTime endTime;
        private string date;
        private double duration;
        private string numberA;
        private string numberAB;
        private string numberB;
        private string numberBB;
        private string year;
        private string month;
        private string day;
        private string hour;

        public string entry { get; private set; }
        public string exportPath { get; private set; }
        public string fileName { get; private set; }
        public bool correct { get; private set; }
        public LogStr(string LogEntry)
        {
            correct = true;
            string[] words = LogEntry.Split(',');
            numberA = words[0].Substring(1, words[0].Length - 2);
            //str.Substring(startIndex, endIndex);
            numberAB = words[0].Substring(1, words[0].Length - 2);
            numberB = words[1].Substring(1, words[1].Length - 2);
            numberBB = words[1].Substring(1, words[1].Length - 2);
            date = words[2].Substring(1, words[2].Length - 2);
            //парсим строку в дату
            if (!DateTime.TryParse(words[2].Substring(1, words[2].Length - 2), out startTime))
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при парсинге даты ", "Message");
                correct = false;
            }
            if (!DateTime.TryParse(words[3].Substring(1, words[3].Length - 2), out endTime))
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при парсинге даты ", "Message");
                correct = false;
            }

            
            string durationStr = words[4].Substring(1, words[4].Length - 2);
            try
            {
                //вычисляем время в секундах
                duration = TimeSpan.Parse(durationStr).TotalSeconds;
            }
            catch
            {
                correct = false;
            }
         
           

            //вычисляем строку год, месяц, час из timedate
            year = startTime.ToString("yyyy");
            month = startTime.ToString("MM");
            day  = startTime.ToString("dd");
            hour = startTime.ToString("HH");
            //    "\\2020\\01";
            exportPath = "\\" + year + "\\"+ month;
            fileName = day + "_" + hour;
            //для теста
            //System.Windows.Forms.MessageBox.Show("date: " + year, "Message");

            
        }

        public void convert()
        {


           entry = date + "\t" + duration + "\t" + numberA + "\t" + numberAB + "\t" + numberB + "\t" + numberBB + "\t" + "0" + "\t" + "0" + "\t" + "0" + "\t" + duration + "\t" + "0";
        }
    }
}
