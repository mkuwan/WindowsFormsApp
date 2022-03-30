using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp.Applications.CustomerService;
using WindowsFormsApp.Domain.DapperRepository;

namespace WindowsFormsApp.AppForm
{
    public partial class DataGridForm : Form
    {
        private readonly ICustomerService _customerService;


        public DataGridForm(ICustomerService customerService) : base()
        {
            _customerService = customerService;

            InitializeComponent();
        }

        private async void buttonReadData_Click(object sender, EventArgs e)
        {
            var customers = await _customerService.GetCustomersAsync();

            // DataSourceを設定してからHeaderTextを設定します(逆はエラーになりますよ)
            this.dataGridViewCustomer.DataSource = customers;
            this.dataGridViewCustomer.Columns[0].HeaderText = @"ID";
            this.dataGridViewCustomer.Columns[1].HeaderText = @"名前";

        }
    }
}
