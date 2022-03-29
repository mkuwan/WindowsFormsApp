using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp.Infrastructure.DBConnection;

namespace WindowsFormsApp.AppForm
{
    public partial class MainForm : Form
    {
        private readonly IStart _start;
        private readonly WinFormDbConnectionStringConfig _dbConnectionString;
        private static MDIParent _mdiParent;
        private static DataGridForm _dataGridForm;

        public MainForm(IStart start, WinFormDbConnectionStringConfig dbConnectionString)
        {
            _start = start;
            _dbConnectionString = dbConnectionString;
            InitializeComponent();
        }

        private void btnShowMessage_Click(object sender, EventArgs e)
        {
            var msg = _start.Get();
            MessageBox.Show(msg + _dbConnectionString.SqlServerConnectionString);
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
                _dataGridForm = new DataGridForm();

            _dataGridForm.Show();
            _dataGridForm.Activate();
        }
    }
}
