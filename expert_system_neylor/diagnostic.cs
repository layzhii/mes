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
    public partial class diagnostic : Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=pharyngeal_diphtheria1_neylor.accdb;";
        // поле - ссылка на экземпляр класса OleDbConnection для соединения с БД
        private OleDbConnection myConnection;
        //OleDbConnection myConnection;
        int num_quest = 1;
        int num_atr = 1;
        Dictionary<int, bool> num_c = new Dictionary<int, bool>();
        //int[] num_c = new int[4];
        int counter = 0;
        int[] evidence = new int[4];
        string[,] value;
        int quest_count;
        double[,] probability;
        int probab_count;

        Dictionary<int, string> answers_user;
        public diagnostic()
        {
            InitializeComponent();
            // создаем экземпляр класса OleDbConnection
            myConnection = new OleDbConnection(connectString);
            // открываем соединение с БД
            myConnection.Open();
           // this.myConnection = myConnection;
            answers_user = new Dictionary<int, string>();

            List<string> var_h = new List<string>();
            List<string> var_q = new List<string>();

            OleDbCommand command = new OleDbCommand("SELECT obj FROM hypothesis", myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var_h.Add(Convert.ToString(reader["obj"]));
            }
            reader.Close();
            probab_count = var_h.Count();
            probability = new double[probab_count, 2];


            for (int i = 0; i < probab_count; i++)
            {
                probability[i, 1] = 0.1;
                probability[i, 0] = i + 1;
            }

            //вопрос
            ActivQuest(num_quest);
            //варианты ответов
            ActivAnswer(num_quest);
        }

        public void ActivQuest(int n)
        {
            // создаем объект для выполнения запроса к БД
            OleDbCommand command = new OleDbCommand("SELECT question, id_atr FROM questions WHERE id_question=" + Convert.ToString(n), myConnection);

            // получаем объект для чтения результата запроса 
            OleDbDataReader reader = command.ExecuteReader();
            // в цикле построчно читаем ответ от БД
            bool buffer = false;
            while (reader.Read())
            {
                laQuest.Text = Convert.ToString(reader["question"]);
                num_atr = Convert.ToInt32(reader["id_atr"]);
                buffer = true;
            }
            reader.Close();
            if (!buffer) end__();
        }
        public void ActivAnswer(int n)
        {
            List<string> var_answer = new List<string>();
            List<int> var_evidence = new List<int>();

            OleDbCommand command = new OleDbCommand("SELECT answer, num_c FROM answer WHERE id_question=" + Convert.ToString(n), myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var_answer.Add(Convert.ToString(reader["answer"]));
                var_evidence.Add(Convert.ToInt32(reader["num_c"]));
            }
            reader.Close();

            switch (var_answer.Count)
            {
                case 2:
                    checkBox1.Visible = true;
                    checkBox2.Visible = true;
                    checkBox3.Visible = false;
                    checkBox4.Visible = false;
                    checkBox1.Text = var_answer[0];
                    checkBox2.Text = var_answer[1];
                    evidence[0] = var_evidence[0];
                    evidence[1] = var_evidence[1];
                    num_c.Add(evidence[0], false);
                    num_c.Add(evidence[1], false);
                    break;
                case 3:
                    checkBox1.Visible = true;
                    checkBox2.Visible = true;
                    checkBox3.Visible = true;
                    checkBox4.Visible = false;
                    checkBox1.Text = var_answer[0];
                    checkBox2.Text = var_answer[1];
                    checkBox3.Text = var_answer[2];
                    evidence[0] = var_evidence[0];
                    evidence[1] = var_evidence[1];
                    evidence[2] = var_evidence[2];
                    num_c.Add(evidence[0], false);
                    num_c.Add(evidence[1], false);
                    num_c.Add(evidence[2], false);
                    break;
                case 4:
                    checkBox1.Visible = true;
                    checkBox2.Visible = true;
                    checkBox3.Visible = true;
                    checkBox4.Visible = true;
                    checkBox1.Text = var_answer[0];
                    checkBox2.Text = var_answer[1];
                    checkBox3.Text = var_answer[2];
                    checkBox4.Text = var_answer[3];
                    evidence[0] = var_evidence[0];
                    evidence[1] = var_evidence[1];
                    evidence[2] = var_evidence[2];
                    evidence[3] = var_evidence[3];
                    num_c.Add(evidence[0], false);
                    num_c.Add(evidence[1], false);
                    num_c.Add(evidence[2], false);
                    num_c.Add(evidence[3], false);
                    break;
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = false;
            //проверяем какое значение выбрано
            if (checkBox1.Checked)
            {
                num_c[evidence[0]] = true;
                counter++;
                checkBox1.Checked = false;
                flag = true;
                
            }
            if (checkBox2.Checked)
            {
                num_c[evidence[1]] = true;
                counter++;
                checkBox2.Checked = false;
                flag = true;
            }
            if (checkBox3.Checked)
            {
                num_c[evidence[2]] = true;
                counter++;
                checkBox3.Checked = false;
                flag = true;
            }
            if (checkBox4.Checked)
            {
                num_c[evidence[3]] = true;
                counter++;
                checkBox4.Checked = false;
                flag = true;
            }
            if(flag)
            {
                f_Bayes();
                //номер след вопроса
                num_quest += 1;
                num_c.Clear();
                if (num_quest != 0)
                {
                    //вопрос
                    ActivQuest(num_quest);
                    //варианты ответов
                    ActivAnswer(num_quest);
                }
                else
                {
                    Close();
                }
            }
            else MessageBox.Show("Выберите симптом", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        public void f_Bayes()
        {
            List<string> var_answer = new List<string>();
            List<string> var_evidence = new List<string>();

            int h;
            double p1, p2, p_correct;
            double var_h;

            foreach (KeyValuePair<int, bool> kvp in num_c)
            {
                OleDbCommand command = new OleDbCommand("SELECT num_h, P1, P2, P_correct FROM p_evidence WHERE num_c =" + Convert.ToString(kvp.Key), myConnection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    h = (Convert.ToInt32(reader["num_h"]));
                    p1 = (Convert.ToDouble(reader["P1"]));
                    p2 = (Convert.ToDouble(reader["P2"]));
                    p_correct = (Convert.ToDouble(reader["P_correct"]));

                    if ( kvp.Value )
                    {
                        if(p_correct!=-1)
                        {
                            var_h = p_correct * probability[h - 1, 1] / (p_correct * probability[h - 1, 1] + p2 * (1 - probability[h - 1, 1]));
                            probability[h - 1, 1] = var_h;
                        }
                        else
                        {
                            var_h = p1 * probability[h - 1, 1] / (p1 * probability[h - 1, 1] + p2 * (1 - probability[h - 1, 1]));
                            probability[h - 1, 1] = var_h;
                        }
                        
                    }
                    if (kvp.Value == false)
                    {
                        if(p_correct!=-1)
                        {
                            var_h = (1 - p_correct) * probability[h - 1, 1] / ((1 - p_correct) * probability[h - 1, 1] + (1 - p2) * (1 - probability[h - 1, 1]));
                            probability[h - 1, 1] = var_h;
                        }
                        else
                        {
                            var_h = (1 - p1) * probability[h - 1, 1] / ((1 - p1) * probability[h - 1, 1] + (1 - p2) * (1 - probability[h - 1, 1]));
                            probability[h - 1, 1] = var_h;
                        }
                        
                    }

                }
                reader.Close();
            }
           
        }

        public void end__()
        {
            Hide();
            result newform = new result(probability);
            newform.Show();
        }

        //public void end_()
        //{

        //    OleDbParameter atr_1;
        //    OleDbParameter atr_2;
        //    OleDbParameter value_1;
        //    OleDbParameter value_2;


        //    atr_1 = new OleDbParameter("@atr_1", value[0, 0]);
        //    value_1 = new OleDbParameter("@value_1", value[0, 1]);
        //    atr_2 = new OleDbParameter("@atr_2", value[1, 0]);
        //    value_2 = new OleDbParameter("@value_2", value[1, 1]);

        //    string end_st = "";
        //    int i = 0, quety_ch = 0;
        //    string b_drule = "";
        //    string queryend = "";
        //    List<string> r_rule = new List<string>();
        //    string par1, par2, par3, par4;
        //    par1 = value[i, 0];
        //    par2 = value[i, 1];
        //    par3 = value[i + 1, 0];
        //    par4 = value[i + 1, 1];
                       
        //    while (i != 10)
        //    {
        //        i++;
        //        atr_1 = new OleDbParameter("@atr_1", par1);
        //        value_1 = new OleDbParameter("@value_1", par2);
        //        atr_2 = new OleDbParameter("@atr_2", par3);
        //        value_2 = new OleDbParameter("@value_2", par4);
        //        if (quety_ch == 0)
        //            queryend = "SELECT then_value, drule FROM rules_complex WHERE(if1_atr = @atr_1) AND (if1_value = @value_1) AND (if2_atr = @atr_2) AND (if2_value = @value_2) ";
        //        if (quety_ch == 1)
        //            queryend = "SELECT then_value, drule FROM rules_complex WHERE(if1_atr = @atr_1) AND  (if2_atr = @atr_2) AND (if2_value = @value_2) ";
        //        if (quety_ch == 2)
        //            queryend = "SELECT then_value, drule FROM rules_complex WHERE(if1_atr = @atr_1)  AND (if2_atr = @atr_2) ";



        //        OleDbCommand commandend = new OleDbCommand(queryend, myConnection);
        //        commandend.Parameters.Add(atr_1);
        //        if (quety_ch == 0)
        //        {
        //            commandend.Parameters.Add(value_1);
        //            commandend.Parameters.Add(atr_2);
        //            commandend.Parameters.Add(value_2);
        //        }
        //        if (quety_ch == 1)
        //        {
        //            commandend.Parameters.Add(atr_2);
        //            commandend.Parameters.Add(value_2);
        //        }
        //        if (quety_ch == 2)
        //        {
        //            commandend.Parameters.Add(atr_2);
        //        }
        //        OleDbDataReader readerend = commandend.ExecuteReader();
        //        while (readerend.Read())
        //        {
        //            end_st = readerend[0].ToString();
        //            b_drule = readerend[1].ToString();
        //            r_rule.Add(b_drule);
        //            if ((end_st == "") && (b_drule != ""))
        //            {
        //                //if (r_rule.Count == 2)
        //                //{
        //                //    par1 = r_rule[0];
        //                //    par3 = r_rule[1];
        //                //    r_rule.Clear();
        //                //    quety_ch = 2;
        //                //    break;
        //                //}
        //                //if ((10 - i) < 3)
        //                //else
        //                //{
        //                par1 = r_rule[r_rule.Count - 1];
        //                par3 = value[i + 1, 0];
        //                par4 = value[i + 1, 1];
        //                quety_ch = 1;

        //                //}
        //                //else
        //                //{
        //                //    par1 = value[i + 1, 0];
        //                //    par2 = value[i + 1,1];
        //                //    par3 = value[i + 2,0];
        //                //    par4 = value[i + 2,1];
        //                //    quety_ch = 0;
        //                //}
        //            }
        //        }

        //        readerend.Close();
        //    }

        //    MessageBox.Show(
        //                 end_st,
        //                "Диагноз",
        //                MessageBoxButtons.OK,
        //                MessageBoxIcon.Information,
        //                MessageBoxDefaultButton.Button1,
        //                MessageBoxOptions.DefaultDesktopOnly);


        //}
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // закрываем соединение с БД
            myConnection.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int c = 0;
            int flag = 0;
            if (checkBox1.Checked) { c = evidence[0]; flag++; }
            if (checkBox2.Checked) { c = evidence[1]; flag++; }
            if (checkBox3.Checked) { c = evidence[2]; flag++; }
            if (checkBox4.Checked) { c = evidence[3]; flag++; }
            if(flag==0 || flag>1)
            {
                MessageBox.Show("Выберите один элемент");
            }
            else
            {
                if (c == -1)
                {
                    MessageBox.Show("Выбранный элемент не является свидетельством");
                }
                else
                {
                    flag = 0;
                    Hide();
                    correct newform = new correct(c);
                    newform.Show();
                    newform.FormClosed += new FormClosedEventHandler(correctFormClosed);
                    this.Hide();
                }
                
            }
            
        }

        private void correctFormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void diagnostic_FormClosing(object sender, FormClosingEventArgs e)
        {
            myConnection.Close();
        }
    }
}
