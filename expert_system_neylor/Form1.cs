using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace expert_system_neylor
{
    public partial class Form1 : Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=pharyngeal_diphtheria1_neylor.accdb;";
        // поле - ссылка на экземпляр класса OleDbConnection для соединения с БД
        private OleDbConnection myConnection;
        public Form1()
        {
            InitializeComponent();
            // создаем экземпляр класса OleDbConnection
            myConnection = new OleDbConnection(connectString);
            // открываем соединение с БД
            myConnection.Open();
        }

        //обработчик второй формы
        private void Form1_Load(object sender, EventArgs e)
        {
            //запускаем вторую форму (в вашем случае это будет главна форма. На самом деле она есть дочерняя первой формы, просто вызываеться она главное формой и поэтому будет на первом плане)
            title frm2 = new title();
            frm2.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //скрываем главную форму
            Hide();
            diagnostic newform = new diagnostic();
            newform.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myConnection.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand("UPDATE p_evidence SET P_correct = -1", myConnection);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Успешно!", "Отмена изменений", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
