using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using WindowsFormsApp.Infrastructure.DBConnection;

namespace WindowsFormsApp.AppForm
{
    public partial class MainForm : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WinFormDbConnectionStringConfig _dbConnectionString;
        private static MDIParent _mdiParent;
        private static DataGridForm _dataGridForm;

        public MainForm(WinFormDbConnectionStringConfig dbConnectionString, IServiceProvider serviceProvider)
        {

            _dbConnectionString = dbConnectionString;
            _serviceProvider = serviceProvider;
            InitializeComponent();
        }

        private void btnShowMessage_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_dbConnectionString.SqlServerConnectionString);
        }

        private void btmShowMDI_Click(object sender, EventArgs e)
        {
          
            if(_mdiParent == null || _mdiParent.IsDisposed)
                _mdiParent = new MDIParent();
            _mdiParent.Show();
            _mdiParent.Activate();
        }

        private void btnShowDataGrid_Click(object sender, EventArgs e)
        {
            if (_dataGridForm == null || _dataGridForm.IsDisposed)
                _dataGridForm = _serviceProvider.GetRequiredService<DataGridForm>();

            _dataGridForm.Show();
            _dataGridForm.Activate();
        }
    }
}
