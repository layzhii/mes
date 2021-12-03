using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace expert_system_neylor
{
    public partial class title : Form
    {
        public title()
        {
            InitializeComponent();
        }
        private void title_Load(object sender, EventArgs e)
        {
            // 1000 - 1сек. время, через которое запустится таймер и закроет форму
            timer1.Interval = 5000;
            // через 5 сек форма закроется и появиться главная. не забиваете, что мы вызвали дочернюю форму и через 5 сек её закрыли
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            this.Close();
        }
    }
}
