using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DBUsers
{
    public class User
    {
        public int id { get; set; }
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public string data_urodzenia { get; set; }
        public string administrator { get; set; }
    }
    public class UserDataAccessLayer
    {
        public static List<User> GetUsers(int pageIndex, int pageSize, string sortExpression, string sortDirection, out int totalRows)
        {
            List<User> listusers = new List<User>();
            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spGetUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter paramStartIndex = new SqlParameter(); paramStartIndex.ParameterName = "@PageIndex";
                paramStartIndex.Value = pageIndex;
                cmd.Parameters.Add(paramStartIndex);

                

                

                SqlParameter paramMaximumRows = new SqlParameter();
                paramMaximumRows.ParameterName = "@PageSize"; paramMaximumRows.Value = pageSize;
                cmd.Parameters.Add(paramMaximumRows);

                SqlParameter paramSortExpression = new SqlParameter();
                paramSortExpression.ParameterName = "@SortExpression";
                paramSortExpression.Value = sortExpression;
                cmd.Parameters.Add(paramSortExpression);

                SqlParameter paramSortDirection = new SqlParameter();
                paramSortDirection.ParameterName = "@SortDirection";
                paramSortDirection.Value = sortDirection;
                cmd.Parameters.Add(paramSortDirection);
                SqlParameter paramOutputTotalRows = new SqlParameter();
                paramOutputTotalRows.ParameterName = "@TotalRows";
                paramOutputTotalRows.Direction = ParameterDirection.Output;
                paramOutputTotalRows.SqlDbType = SqlDbType.Int;
                cmd.Parameters.Add(paramOutputTotalRows);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    User user = new User();
                    user.id = Convert.ToInt32(rdr["id"]);
                    user.imie = rdr["Imie"].ToString();
                    user.nazwisko = rdr["Nazwisko"].ToString();
                    user.data_urodzenia = rdr["data_urodzenia"].ToString().Substring(0, rdr["data_urodzenia"].ToString().IndexOf(' '));
                    user.administrator = rdr["administrator"].ToString();
                    listusers.Add(user);
                }
                rdr.Close(); totalRows = (int)cmd.Parameters["@TotalRows"].Value;
            }
            return listusers;
        }
    }

    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                int totalRows = 0;
                GridView1.DataSource = UserDataAccessLayer.GetUsers(0, GridView1.PageSize,
                    GridView1.Attributes["CustomSortField"], GridView1.Attributes["CustomSortDirection"],  out totalRows);
                GridView1.DataBind();

                DataBindRepeater(0, GridView1.PageSize, totalRows);

            }




        }

        private void DataBindRepeater(int pageIndex, int pageSize, int totalRows)
        {

            int totalPages = totalRows / pageSize;
            if ((totalRows % pageSize) != 0)
            {
                totalPages++;
            }

            List<ListItem> pages = new List<ListItem>();

            if (totalPages > 1)
            {
                for (int i = 1; i <= totalPages; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != (pageIndex + 1)));
                }

            }
            repeaterpaging.DataSource = pages;
            repeaterpaging.DataBind();


        }

        protected void linkButton_Click(object sender, EventArgs e)
        {
            int totalRows = 0;
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            pageIndex -= 1;
            GridView1.PageIndex = pageIndex;
            GridView1.DataSource = UserDataAccessLayer.GetUsers(pageIndex, GridView1.PageSize, 
                GridView1.Attributes["CustomSortField"], GridView1.Attributes["CustomSortDirection"], out totalRows);
            GridView1.DataBind();
            DataBindRepeater(pageIndex, GridView1.PageSize, totalRows);

        }

        private void SortGridview(GridView gridView, GridViewSortEventArgs e, out SortDirection sortDirection, out string sortField)
        {
            sortField = e.SortExpression;
            sortDirection = e.SortDirection;

            if(gridView.Attributes["CustomSortField"] != null && gridView.Attributes["CustomSortDirection"] != null)
            {
                if(sortField == gridView.Attributes["CustomSortField"])
                {
                    if (gridView.Attributes["CustomSortDirection"] == "ASC")
                    {
                        sortDirection = SortDirection.Descending;

                    }
                    else
                    {
                        sortDirection = SortDirection.Ascending;
                    }

                }
                gridView.Attributes["CustomSortField"] = sortField;
                gridView.Attributes["CustomSortDirection"] = (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");


            }
        }
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string sortField = string.Empty;

            SortGridview(GridView1, e, out sortDirection, out sortField);
            string strSortDirection = sortDirection == SortDirection.Ascending ? "ASC" : "DESC";

            int totalRows = 0;
            GridView1.DataSource = UserDataAccessLayer.GetUsers(GridView1.PageIndex, GridView1.PageSize, e.SortExpression, strSortDirection, out totalRows);
            GridView1.DataBind();
            DataBindRepeater(GridView1.PageIndex, GridView1.PageSize, totalRows);
        }

        protected void dodaj_user(object sender, EventArgs e)
        {
            string im = imie.Text;
            string nazw = nazwisko.Text;
            string date = ur1.Text;
            int adm = int.Parse(administrator.Text);

            string CS = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(CS))
            {
                string sql = "INSERT INTO Users (Imie, Nazwisko, Data_urodzenia, Administrator) " +
                    "values (@imie, @nazwisko, @data_urodzenia, @administrator)";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@imie", SqlDbType.VarChar);
                cmd.Parameters.Add("@nazwisko", SqlDbType.VarChar);
                cmd.Parameters.Add("@data_urodzenia", SqlDbType.VarChar);
                cmd.Parameters.Add("@administrator", SqlDbType.Bit);
                cmd.Parameters["@imie"].Value = im;
                cmd.Parameters["@nazwisko"].Value = nazw;
                cmd.Parameters["@data_urodzenia"].Value = date;
                cmd.Parameters["@administrator"].Value = adm;
                cmd.ExecuteNonQuery();
            }

        }

    }
}
