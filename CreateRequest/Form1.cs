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
            var test = info.Where(x => x.Split(new char[] { ':',' '}).Count() > 0)
                .Select(x =>
                {
                    if (x.Contains("HTTP/1.1")) return null;

                    var split = x.Split(new char[] { ':','\t'});

                    if (split.First().Length == 0)return null;

                    if (split.First().ToLower() == "cookie" && !checkBox1.Checked)return null;
                    if(split.First().ToLower() == "user-agent")
                        return $"{textBox1.Text}.UserAgent = {string.Join(":", split.Skip(1)).Trim()};";
                    if(split.First().ToLower() == "connection" && split[1] == "keep-alive")
                        return $"{textBox1.Text}.KeepAlive = true;";

                    return $"{textBox1.Text}.AddHeader(\"{split[0]}\",\"{string.Join(":", split.Skip(1)).Trim()}\");";
                }).ToList();


            richTextBox2.Lines = test.Where(x=> x != null).ToArray();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox2.Text);
        }
    }
}
