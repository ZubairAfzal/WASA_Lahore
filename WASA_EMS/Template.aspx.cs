using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;


namespace WASA_EMS
{
    public partial class Template : System.Web.UI.Page
    {
        public string tempName;
        public int pCount;
        public char separator;
        public int countP;
        public int pId;
        //bool flag = false;
        public int idTemp;


        public object MessageBox { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (ViewState["ParameterCount"] != null)
            //    pCount = Convert.ToInt32(ViewState["ParameterCount"]);
            //else
            //    pCount = 0;
            //listbox1.DataTextField = "";
        }

        protected void listbox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Add()
        {

        }

        protected void selectOne_Click(object sender, ImageClickEventArgs e)
        {

            if (listbox1.SelectedIndex == -1)
            {
                listbox2.Items.Add(listbox2.SelectedItem);
                listbox2.Items.Remove(listbox2.SelectedItem);
            }
            else
            {
                listbox2.Items.Add(listbox1.SelectedItem);
                listbox1.Items.Remove(listbox1.SelectedItem);
            }
        }

        protected void deselectOne_Click(object sender, ImageClickEventArgs e)
        {
            if (listbox2.SelectedIndex == -1)
            {
                listbox1.Items.Add(listbox1.SelectedItem);
                listbox1.Items.Remove(listbox1.SelectedItem);
            }
            else
            {
                listbox1.Items.Add(listbox2.SelectedItem);
                listbox2.Items.Remove(listbox2.SelectedItem);
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            if (listbox2.Items.Count <= 1)
            {

            }
            else if (listbox2.SelectedIndex <= 0)
            {

            }
            else
            {
                int index = listbox2.SelectedIndex;
                var item = listbox2.SelectedItem;
                var val = listbox2.SelectedItem.Value;

                listbox2.Items.Insert(index, listbox2.Items[listbox2.SelectedIndex - 1]);
                listbox2.Items.RemoveAt(index + 1);
                listbox2.Items.Insert(index - 1, item);
                listbox2.Items.RemoveAt(index);
            }
        }

        protected void btnmoveDown_Click(object sender, ImageClickEventArgs e)
        {
            if (listbox2.Items.Count <= 1)
            {

            }
            else if (listbox2.SelectedIndex >= listbox2.Items.Count - 1)
            {

            }
            else
            {
                int index = listbox2.SelectedIndex;
                var item = listbox2.SelectedItem;
                var val = listbox2.SelectedItem.Value;

                listbox2.Items.Insert(index, listbox2.Items[listbox2.SelectedIndex + 1]);
                listbox2.Items.RemoveAt(index + 2);
            }
        }

        protected void btnTempSelect_Click(object sender, EventArgs e)
        {
            if (txtTempName.Text == "" || txtSeparator.Text == "" || txtParameterCount.Text == "")
            {

            }
            else
            {
                ViewState["parameters"] = txtParameterCount.Text;
                ViewState["template"] = txtTempName.Text;
                ViewState["separator"] = txtSeparator.Text;
                //pCount = Convert.ToInt32(txtParameterCount.Text);
                divTemp.Visible = false;
                divlist.Visible = true;
            }






            //pCount = Convert.ToInt32(txtParameterCount.Text);
            //separator = Convert.ToChar(txtSeparator.Text);
            //tempName = txtTempName.Text;

            //pCount = Convert.ToInt32(txtParameterCount.Text);
            //separator = Convert.ToChar(txtSeparator.Text);
            //tempName = txtTempName.Text;

            //using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            //{
            //    try
            //    {
            //        string query = "insert into tblTemplate values ('" + tempName + "', '" + pCount + "', '" + separator + "')";
            //        SqlCommand cmd = new SqlCommand(query, conn);
            //        conn.Open();
            //        cmd.ExecuteNonQuery();
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            //public string tempName;
            //public int pCount;
            //public char separator;
            //public int countP;

            //ViewState["parameters"] = txtParameterCount.Text;
            //ViewState["template"] = txtTempName.Text;
            //ViewState["separator"] = txtSeparator.Text;
            tempName = ViewState["template"].ToString();
            pCount = Convert.ToInt32(ViewState["parameters"]);
            separator = Convert.ToChar(ViewState["separator"]);
            txtParameterCount.Text = "";
            txtSeparator.Text = "";
            txtTempName.Text = "";

            if (listbox2.Items.Count != pCount)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select correct number of parameters')", true);
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    try
                    {
                        string query = "insert into tblTemplate (TemplateName, ParameterCount, Separator, CompanyID) values ('" + tempName + "', " + pCount + ", '" + separator + "', " + c_id + ");";
                        query += "SELECT CAST(scope_identity() AS int)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        conn.Open();
                        idTemp = (int)cmd.ExecuteScalar();
                        //cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                }

                for (int i = 0; i < listbox2.Items.Count; i++)
                {
                    string text = listbox2.Items[i].Text.ToString();
                    int val = Convert.ToInt32(listbox2.Items[i].Value);

                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                    {
                        try
                        {
                            //select ParameterID from tblParameter where ParameterOrder = " + val + " "
                            string query = "insert into tblTemplateParameter (TemplateID,ParameterID,ParameterOrder,CompanyID) values(";
                            query += " " + idTemp + ", ";
                            //query += "(select TemplateID from tblTemplate where TemplateName = '"+ tempName +"' ),";
                            query += "(select ParameterID from tblParameter where ParameterOrder = " + val + " and CompanyID = " + c_id + " ), ";
                            query += " " + val + " ,";
                            query += " " + c_id + ")";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {

                        }
                        conn.Close();
                    }

                }
                listbox1.Items.Clear();
                listbox2.Items.Clear();
                Response.Redirect("~/Template/Index");
            }




        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            divlist.Visible = false;
            divTemp.Visible = true;
        }
    }
}