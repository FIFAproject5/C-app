using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace ProjectFifaV2
{
    public partial class frmAdmin : Form
    {
        private DatabaseHandler dbh;
        private OpenFileDialog opfd;

        DataTable table;

        public frmAdmin()
        {
            dbh = new DatabaseHandler();
            table = new DataTable();
            this.ControlBox = false;
            InitializeComponent();
        }

        private void btnAdminLogOut_Click(object sender, EventArgs e)
        {
            txtQuery.Text = null;
            txtPath = null;
            dgvAdminData.DataSource = null;
            Hide();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (txtQuery.TextLength > 0)
            {
                ExecuteSQL(txtQuery.Text);
            }
        }

        private void ExecuteSQL(string selectCommandText)
        {
            dbh.TestConnection();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommandText, dbh.GetCon());
            dataAdapter.Fill(table);
            dgvAdminData.DataSource = table;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            txtPath.Text = null;
            
            string path = GetFilePath();

            if (CheckExtension(path, "csv"))
            {
                txtPath.Text = path;
            }
            else
            {
                MessageHandler.ShowMessage("The wrong filetype is selected.");
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            if (!(txtPath.Text == ""))
            {
                dbh.OpenConnectionToDB();

                MessageHandler.ShowMessage(txtPath.Text);

                    List<string> listA = new List<string>();
                    List<string> listB = new List<string>();
                    List<string> listC = new List<string>();
                    List<string> listD = new List<string>();
                    List<string> listE = new List<string>();
                    List<string> listF = new List<string>();


                using (var reader = new StreamReader(txtPath.Text))
                {

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if(RB_load_matches.Checked)
                        {
                        listA.Add(values[0]);
                        listB.Add(values[1]);
                        listC.Add(values[2]);
                        listD.Add(values[3]);
                        listE.Add(values[4]);
                        listF.Add(values[5]);
                        }
                        else if (RB_load_teams.Checked)
                        {
                            listA.Add(values[0]);
                            listB.Add(values[1]);
                            listC.Add(values[2]);
                            listD.Add(values[3]);
                            listE.Add(values[4]);
                            string check = "";
                            foreach (string item in listA)
                            {
                                
                                if( check != item)
                                {
                                    check = item;
                                    foreach(string itemB in listC)
                                    {
                                        
                                        string B = item.Replace("\"", "");
                                        MessageHandler.ShowMessage(item);
                                        MessageHandler.ShowMessage(itemB);

                                        int A = Convert.ToInt32(B);
                                        string query = "insert into Tblteams (team_id, teamname) values ('" + A + "','" + itemB + "')";
                                        dbh.FillDT(query);
                                    }
                                    
                                    
                                }
                                

                            }
                        }
                        else
                        {
                            MessageHandler.ShowMessage("select an button");
                        }
                    }
                }

                

                dbh.CloseConnectionToDB();
            }
            else
            {
                MessageHandler.ShowMessage("No filename selected.");
            }
        }
        
        private string GetFilePath()
        {
            string filePath = "";
            opfd = new OpenFileDialog();

            opfd.Multiselect = false;

            if (opfd.ShowDialog() == DialogResult.OK)
            {
                filePath = opfd.FileName;
            }

            return filePath;
        }

        private bool CheckExtension(string fileString, string extension)
        {
            int extensionLength = extension.Length;
            int strLength = fileString.Length;

            string ext = fileString.Substring(strLength - extensionLength, extensionLength);

            if (ext == extension)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
