﻿using System;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CathLab
{
    public partial class Types : System.Web.UI.Page
    {
        public static string pnum;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { }
        }

        #region Data Load
        protected void rgManufacturers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            using (var context = new cathlabEntities())
                rgManufacturers.DataSource = (from man in context.Manufacturers select new { man.ID, man.Name, man.PhoneNumber, man.Email }).ToList();
        }

        protected void rgProdType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            using (var context = new cathlabEntities())
                rgProdType.DataSource = (from types in context.ProductTypes select new { types.ID, types.Type }).ToList();
        }

        protected void rgPartNumbers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            using (var context = new cathlabEntities())
                rgPartNumbers.DataSource = (from pnum in context.PartNumbers select new { pnum.PartNum, pnum.NameSize, pnum.ProductType.Type, pnum.Cost, pnum.Manufacturer.Name, pnum.Par }).ToList();
        }
        #endregion Data Load

        protected void rgManufacturers_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            using (var context = new cathlabEntities())
            {
                int pnum = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                ProductType pt = context.ProductTypes.Find(ID);
            }
            
        }

        protected void rgPartNumbers_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                using (var context = new cathlabEntities())
                {
                    int PNum = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PartNum"].ToString());
                    PartNumber pn = context.PartNumbers.Find(PNum);
                    pn.NameSize = (e.Item.FindControl("tbNameSize") as RadTextBox).Text;
                    pn.Cost = int.Parse((e.Item.FindControl("tbCost") as RadTextBox).Text);
                    pn.Par = int.Parse((e.Item.FindControl("tbPar") as RadTextBox).Text);
                    context.SaveChanges();
                }
            }
        }

        protected void rgProdType_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            using (var context = new cathlabEntities())
            {
                int ID = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                ProductType pt = context.ProductTypes.Find(ID);
                pt.Type = (e.Item.FindControl("tbType") as RadTextBox).Text;
                context.SaveChanges();
            }
        }

        #region Delete
        protected void rgPartNumbers_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = (GridDataItem)e.Item;
            string PartNum = item.GetDataKeyValue("PartNum").ToString();
            using (var context = new cathlabEntities())
            {
                PartNumber pn = context.PartNumbers.Find(PartNum);
                try
                {
                    context.PartNumbers.Remove(pn);
                    context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    rnLabel.Text = "ERROR! Cannot delete, there are products<br/> in the database of that part number.";
                    RadNotification.Show();
                }
            }
        }

        protected void rgManufacturers_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = (GridDataItem)e.Item;
            int ID = (int)item.GetDataKeyValue("ID");
            using (var context = new cathlabEntities())
            {
                Manufacturer m = context.Manufacturers.Find(ID);
                try
                {
                    context.Manufacturers.Remove(m);
                    context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    rnLabel.Text = "ERROR! Can't delete manufacturer. Products in database rely on this entry.";
                    RadNotification.Show();
                }
            }
        }

        protected void rgProdType_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = (GridDataItem)e.Item;
            int ID = (int)item.GetDataKeyValue("ID");
            using (var context = new cathlabEntities())
            {
                ProductType pt = context.ProductTypes.Find(ID);
                var temp = (from prod in context.Products
                           where prod.PartNumber1.ProductTypeID == pt.ID
                           select prod).FirstOrDefault();

                if(temp ==null)
                {
                    context.ProductTypes.Remove(pt);
                    context.SaveChanges();
                    rgProdType.Rebind();
                }
                else
                {
                    rnLabel.Text = "ERROR! Can't delete product type. Products in database rely on this entry.";
                    RadNotification.Show();
                }

                //try
                //{
                //    context.ProductTypes.Remove(pt);
                //    context.SaveChanges();
                //}
                //catch (Exception)
                //{
                //    rnLabel.Text = "ERROR! Can't delete product type. Products in database rely on this entry.";
                //    RadNotification.Show();
                //}
            }
        }
        #endregion Delete       

        protected void tbPNSearch_TextChanged(object sender, EventArgs e)
        {
            if (tbPNSearch.Text != string.Empty)
            {
                using (var context = new cathlabEntities())
                    rgPartNumbers.DataSource = (from pnum in context.PartNumbers select new { pnum.PartNum, pnum.NameSize, pnum.ProductType.Type, pnum.Cost, pnum.Manufacturer.Name, pnum.Par })
                        .Where(a => a.PartNum.Contains(tbPNSearch.Text)).ToList();
            }
            else
                rgPartNumbers.Rebind();
        }

        protected void btnNewPartNum_Click(object sender, EventArgs e)
        {
            //pnlNewPartNumber.Visible = true;
            //pnlNewManufacturer.Visible = false;
            //pnlNewProdType.Visible = false;
        }

        protected void btnManufacturer_Click(object sender, EventArgs e)
        {
            //pnlNewPartNumber.Visible = false;
            //pnlNewManufacturer.Visible = true;
            //pnlNewProdType.Visible = false;
        }

        protected void btnProductType_Click(object sender, EventArgs e)
        {
            //pnlNewPartNumber.Visible = false;
            //pnlNewManufacturer.Visible = false;
            //pnlNewProdType.Visible = true;
        }
    }
}