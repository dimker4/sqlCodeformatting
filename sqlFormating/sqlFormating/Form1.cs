using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sqlFormating
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sqlText = richTextBox1.Text;

            var f = new Formatting(sqlText);

            richTextBox2.Text = f.MakeFormated();
        }
    }

    public class Formatting
    {
        public string text { get; set; }

        public Formatting()
        {
            text = "empty";
        }

        public Formatting(string sqlText)
        {
            text = sqlText;
        }

        public string MakeFormated()
        {
            var tmpText = "";
            var newLine = "";
            var offset = 0;

            ToUpperRegister();

            using (StringReader reader = new StringReader(text))
            {
                string line;
                // Работаем по одной строке
                while ((line = reader.ReadLine()) != null)
                {
                    newLine = line;
                    var selectPos = line.IndexOf("SELECT");
                    var fromPos = line.IndexOf("FROM");
                    var wherePos = line.IndexOf("WHERE");


                    // проверяем, что бы SELECT и FROM были в одной строке
                    if (selectPos != -1 && fromPos != -1)
                    {
                        // Если зашли, значит в одной строке, добавляем переход на новую
                        newLine = line.Insert(fromPos, "\n");

                        // Если WHERE в этой же строке, то тоже двигаем
                        if (wherePos != -1)
                        {
                            newLine = newLine.Remove(wherePos, 1);
                            newLine = newLine.Insert(wherePos, "\n");
                            
                        }
                    }
                    
                    // Каждый join то же перенесем на новую строку
                    var joinNext = 0;
                    foreach (var word in line.Split(' '))
                    {
                        if (word == "JOIN")
                        {
                            offset = newLine.IndexOf("JOIN", joinNext);
                            newLine = newLine.Insert(offset, "\n");
                            joinNext = newLine.IndexOf("JOIN", offset + 4);
                        }
                    }
                }
            }

            return newLine.ToString();
        }

        // Переведем все служебные слова в верзний регистр
        public void ToUpperRegister()
        {
            string[] serviceWords = {"SELECT", "FROM", "WHERE", "ORDER", "BY", "GROUP", "JOIN", "ON", "AND", "TOP", "ROW_COUNT", "COUNT", "SUM", "OVER", "PARTITION"};

            foreach (var word in text.Split(' '))
            {
                if (serviceWords.Contains(word.ToUpper()))
                {
                    if (word != word.ToUpper())
                    {
                        text = text.Replace(word, word.ToUpper());
                    }
                }
            }
        }
    }
}
