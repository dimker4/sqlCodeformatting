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

            richTextBox2.Text = f.makeFormated();
        }
    }

    public class Formatting
    {
        public string text { get; }

        public Formatting()
        {
            text = "empty";
        }

        public Formatting(string sqlText)
        {
            text = sqlText;
        }

        public string makeFormated()
        {
            var tmpText = "";
            var newLine = "";
            using (StringReader reader = new StringReader(text))
            {
                string line;
                // Работаем по одной строке
                while ((line = reader.ReadLine()) != null)
                {
                    newLine = line;
                    var selectPos = line.IndexOf("select", StringComparison.CurrentCultureIgnoreCase);
                    var fromPos = line.IndexOf("from", StringComparison.CurrentCultureIgnoreCase);
                    var wherePos = line.IndexOf("where", StringComparison.CurrentCultureIgnoreCase);

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

                }
            }

            return newLine.ToString();

        }
    }
}
