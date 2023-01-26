using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddressBook
{
    public partial class Contacts : Form
    {
        public Contacts()
        {
            InitializeComponent();
        }

        private AddressExample.AddressBookEntities dbcontext = null;

        private void Contacts_Load(object sender, EventArgs e)
        {
            Load_Contacts();
        }

        private void browseAllButton_Click(object sender, EventArgs e)
        {
            Load_Contacts();
        }

        private void Load_Contacts()
        {
            if (dbcontext != null)
            {
                dbcontext.Dispose();
            }
            dbcontext = new AddressExample.AddressBookEntities();

            dbcontext.Addresses.OrderBy(address => address.LastName).ThenBy(address => address.FirstName).Load();
            addressBindingSource.DataSource = dbcontext.Addresses.Local;
            addressBindingSource.MoveFirst();
            findTextBox.Clear();
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            var lastNameQuery = from address in dbcontext.Addresses
                                where address.LastName.StartsWith(findTextBox.Text)
                                orderby address.LastName, address.FirstName
                                select address;

            addressBindingSource.DataSource = lastNameQuery.ToList();
            addressBindingSource.MoveFirst();
        }

        private void addressBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            addressBindingSource.EndEdit();

            try
            {
                dbcontext.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                MessageBox.Show("Columns cannot be empty", "Entity Validation Exception");
            }
            Load_Contacts();
        }

        private void findAreaButton_Click(object sender, EventArgs e)
        {
            var areaQuery = from address in dbcontext.Addresses
                                where address.PhoneNumber.StartsWith(findTextBox.Text)
                                orderby address.LastName, address.FirstName
                                select address;

            addressBindingSource.DataSource = areaQuery.ToList();
            addressBindingSource.MoveFirst();
        }
    }
}
