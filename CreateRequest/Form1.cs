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

namespace CreateRequest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Текстовый документ *.txt|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
                richTextBox1.Lines = File.ReadAllLines(ofd.FileName);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Текстовый документ *.txt|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
                File.WriteAllText(sfd.FileName, richTextBox2.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Проверьте переменную", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var info = richTextBox1.Lines.ToList();
            var test = info.Where(x => x.Contains(": "))
                .Select(x =>
                {
                    var split = x.Split(':');

                    if (split.First() == "Cookie" && !checkBox1.Checked) {
                        return null;
                    }

                    return $"{textBox1.Text}.AddHeader(\"{split[0]}\",\"{string.Join(":", split.Skip(1)).TrimStart()}\")";
                }).ToList();


            richTextBox2.Lines = test.ToArray();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox2.Text);
        }
    }
}
