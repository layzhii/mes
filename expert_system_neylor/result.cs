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
    public partial class result : Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=pharyngeal_diphtheria1_neylor.accdb;";
        private OleDbConnection myConnection;

        public result(double[,] mass)
        {
            InitializeComponent();
            // создаем экземпляр класса OleDbConnection
            myConnection = new OleDbConnection(connectString);
            // открываем соединение с БД
            myConnection.Open();
            //this.myConnection = myConnection;

            List<string> var_obj = new List<string>();
            List<double> var_p= new List<double>();

            int k = mass.GetLength(0);
            Label[] labels = new Label[k];
            Label[] labels_p = new Label[k];

           for(int i=0; i<k; i++)
           {
                OleDbCommand command = new OleDbCommand("SELECT obj FROM hypothesis WHERE id =" + Convert.ToString(mass[i, 0]), myConnection);

                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //labels[i] = new Label();
                    //labels_p[i] = new Label();

                    //labels[i].Text = Convert.ToString(reader["obj"]);
                    //labels_p[i].Text = Convert.ToString(mass[i,1]);

                    //this.Controls.Add(labels[i]);
                    //this.Controls.Add(labels_p[i]);
                    var_obj.Add(Convert.ToString(reader["obj"]));
                    var_p.Add(Convert.ToDouble(mass[i, 1]));
                }
                reader.Close();
            }

            max(var_p, var_obj);
            label1.Text = Convert.ToString(var_obj[8]);
            label2.Text = Convert.ToString(var_obj[7]);
            label3.Text = Convert.ToString(var_obj[6]);
            label4.Text = Convert.ToString(var_obj[5]);
            label5.Text = Convert.ToString(var_obj[4]);
            label6.Text = Convert.ToString(var_obj[3]);
            label7.Text = Convert.ToString(var_obj[2]);
            label8.Text = Convert.ToString(var_obj[1]);
            label9.Text = Convert.ToString(var_obj[0]);

            label10.Text = Convert.ToString(Math.Round(var_p[8], 2));
            label11.Text = Convert.ToString(Math.Round(var_p[7], 2));
            label12.Text = Convert.ToString(Math.Round(var_p[6], 2));
            label13.Text = Convert.ToString(Math.Round(var_p[5], 2));
            label14.Text = Convert.ToString(Math.Round(var_p[4], 2));
            label15.Text = Convert.ToString(Math.Round(var_p[3], 2));
            label16.Text = Convert.ToString(Math.Round(var_p[2], 2));
            label17.Text = Convert.ToString(Math.Round(var_p[1], 2));
            label18.Text = Convert.ToString(Math.Round(var_p[0], 2));
        }

        private void result_FormClosing(object sender, FormClosingEventArgs e)
        {
            myConnection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void max(List<double> prob, List<string> max_obj)
        {
            double max = prob[0];
            int n = prob.Count();
            double temp;
            string temp1;
            //double [] max_p = new double[prob.Count()];
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (prob[j] > prob[j + 1])
                    {
                        temp = prob[j];
                        temp1 = max_obj[j];
                        prob[j] = prob[j + 1];
                        max_obj[j] = max_obj[j + 1];
                        prob[j + 1] = temp;
                        max_obj[j + 1] = temp1;
                    }
                }
            }
        }
    }
}
