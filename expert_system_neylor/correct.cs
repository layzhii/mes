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
    public partial class correct : Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=pharyngeal_diphtheria1_neylor.accdb;";
        private OleDbConnection myConnection;
        double response;
        int h;
        int c;
        int schet = 0;
        double prob;
        List<int> hypot = new List<int>();
        //OleDbConnection myConnection;
        public correct(int evid)
        {
            InitializeComponent();
            // создаем экземпляр класса OleDbConnection
            myConnection = new OleDbConnection(connectString);
            // открываем соединение с БД
            myConnection.Open();
            //this.myConnection = myConnection;
            c =evid;

            OleDbCommand command_question = new OleDbCommand("SELECT question_add FROM add_question", myConnection);
            OleDbDataReader reader_q = command_question.ExecuteReader();
            while (reader_q.Read())
            {
                laQuest_add.Text = Convert.ToString(reader_q["question_add"]);
            }
            reader_q.Close();

            OleDbCommand command = new OleDbCommand("SELECT num_h FROM p_evidence WHERE num_c =" + Convert.ToString(c), myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                hypot.Add(Convert.ToInt32(reader["num_h"]));
            }
            reader.Close();
            ActivQ(schet);
        }

        public void ActivQ( int i)
        {
            OleDbCommand command = new OleDbCommand("SELECT obj FROM hypothesis WHERE id =" + Convert.ToString(hypot[i]), myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            bool buffer = false;
            while (reader.Read())
            {
                la_h.Text = Convert.ToString(reader["obj"]);
                h = hypot[i];
                buffer = true;
            }
            reader.Close();
            
            if (!buffer) end_();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (response >= 0) prob = 0.1 + 0.9 * (response / 5);
            else prob = 0.1 + 0.1 * (response / 5);
            
            OleDbCommand cmd = new OleDbCommand("UPDATE p_evidence SET P_correct = @atr1 WHERE num_h = @atr2 AND num_c = @atr3", myConnection);
            cmd.Parameters.AddWithValue("@atr1", prob);
            cmd.Parameters.AddWithValue("@atr2", h);
            cmd.Parameters.AddWithValue("@atr3", c);
            cmd.ExecuteNonQuery();

            OleDbCommand cmd_ = new OleDbCommand("UPDATE p_evidence SET comment = @atr1 WHERE num_h = @atr2 AND num_c = @atr3", myConnection);
            cmd_.Parameters.AddWithValue("@atr1", richTextBox1.Text);
            cmd_.Parameters.AddWithValue("@atr2", h);
            cmd_.Parameters.AddWithValue("@atr3", c);
            cmd_.ExecuteNonQuery();
            richTextBox1.Clear();

            schet += 1;
            if (schet != hypot.Count())
            {
                if (hypot[schet] != 0)
                {
                    //вопрос
                    ActivQ(schet);
                }
            }
            else
            {
                this.Close();
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            // приводим отправителя к элементу типа RadioButton
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
                switch (radioButton.Text)
                {
                    case "-5":
                        {
                            response = Convert.ToInt32(radioButton.Text);
                            break;
                        }
                    case "-4":
                        {
                            response = Convert.ToInt32(radioButton.Text);
                            break;
                        }
                    case "-3":
                        {
                            response = Convert.ToInt32(radioButton.Text);
                            break;
                        }
                    case "-2":
                        {
                            response = Convert.ToInt32(radioButton.Text);
                            break;
                        }
                    case "-1":
                        {
                            response = Convert.ToInt32(radioButton.Text);
                            break;
                        }
                    case "0":
                        {
                            response = Convert.ToInt32(radioButton.Text);
                            break;
                        }
                    case "1":
                        {
                            response = Convert.ToInt32(radioButton.Text);
                            break;
                        }
                    case "2":
                        {
                            response = Convert.ToInt32(radioButton.Text);
                            break;
                        }
                    case "3":
                        {
                            response = Convert.ToInt32(radioButton.Text);
                            break;
                        }
                    case "4":
                        {
                            response = Convert.ToInt32(radioButton.Text);
                            break;
                        }
                    case "5":
                        {
                            response = Convert.ToInt32(radioButton.Text);
                            break;
                        }
                }
            {
                
            }
        }

        public void end_()
        {
            Hide();
        }

        private void correct_FormClosing(object sender, FormClosingEventArgs e)
        {
            myConnection.Close();
        }
    }
}
