﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CathLab.UserControls
{
    public partial class NewPartNumber : System.Web.UI.UserControl
    {
        public static string pnum;

        protected void Page_Load(object sender, EventArgs e)
        { }

        protected void btnRestart_Click(object sender, EventArgs e)
        {
            tbPartNum.Text = string.Empty;
            pnum = null;
            lblStatus.Visible = false;
            pnlScan.Visible = true;
            pnlNewProduct.Visible = false;
            pnlNewPartNumber.Visible = false;
            pnlNewManufacturer.Visible = false;
            pnlNewProdType.Visible = false;
            lblMissing.Visible = false;
        }

        #region NewProduct
        protected void tbPartNum_TextChanged(object sender, EventArgs e)
        {
            if (tbPartNum.Text != string.Empty)
            {
                pnum = tbPartNum.Text;
                tbEPartNum.Text = pnum;
                pnlScan.Visible = false;
                autopopulate();
            }
            else
            {
                lblStatus.Text = "ERROR! Please enter PartNumber again.";
                lblStatus.Visible = true;
            }
        }

        protected void autopopulate()
        {
            string part = tbPartNum.Text;
            using (var context = new cathlabEntities())
            {
                var temp = (from pnum in context.PartNumbers
                            where pnum.PartNum == part
                            select new { pnum.Manufacturer.Name, pnum.NameSize, pnum.ProductType.Type }).SingleOrDefault();
                if (temp != null)
                {
                    pnlNewProduct.Visible = true;
                    loadELocations();
                    tbEManufacturer.Text = temp.Name;
                    tbENameSize.Text = temp.NameSize;
                    tbEProdType.Text = temp.Type;
                }
                else
                {
                    pnlNewPartNumber.Visible = true;
                    pnlNewProduct.Visible = false;                    
                    loadManufacturers();
                    loadProductTypes();
                }
            }
        }

        protected void loadELocations()
        {
            using (var context = new cathlabEntities())
            {
                List<Location> temp = (from loc in context.Locations select loc).ToList();
                lbxELocation.DataValueField = "ID";
                lbxELocation.DataTextField = "LocationName";
                lbxELocation.DataSource = temp;
                lbxELocation.DataBind();
            }
        }

        protected void btnInsertProduct_Click(object sender, EventArgs e)
        {
            using (var context = new cathlabEntities())
            {
                int lotNum, locID;
                int.TryParse(lbxELocation.SelectedValue, out locID);
                int.TryParse(tbLotNumber.Text, out lotNum);

                Product p = new Product();
                p.PartNumber = pnum;
                //p.SerialNumber = ;
                p.ExpirationDate = rdpExpiration.SelectedDate.Value.Date;
                p.LocationID = locID;
                p.LotNumber = lotNum;
                p.StatusID = 4;
                context.Products.Add(p);
                context.SaveChanges();
                pnlScan.Visible = true;                
                pnlNewProduct.Visible = false;
                lblStatus.Visible = true;
                lblStatus.Text = string.Format("Product '{0}' has been successfully entered.", pnum);
                tbPartNum.Text = string.Empty;
            }
        }
        #endregion ExistingPartNum

        #region NewPartNum
        protected void btnPNSubmit_Click(object sender, EventArgs e)
        {
            if (lbxManufacturer.SelectedItem == null || lbxProdType.SelectedItem == null || tbNNameSize.Text == string.Empty)
            {
                lblMissing.Visible = true;
            }
            else
            {
                using (var context = new cathlabEntities())
                {
                    int cost, par;
                    int.TryParse(tbNCost.Text, out cost);
                    int.TryParse(tbNPar.Text, out par);
                    PartNumber pn = new PartNumber();
                    pn.PartNum = pnum;
                    pn.NameSize = tbNNameSize.Text;
                    pn.ManufacturerID = int.Parse(lbxManufacturer.SelectedValue);   //
                    pn.ProductTypeID = int.Parse(lbxProdType.SelectedValue);        //                
                    pn.Cost = cost;
                    pn.Par = par;
                    context.PartNumbers.Add(pn);
                    context.SaveChanges();
                    pnlNewPartNumber.Visible = true;
                    pnlNewPartNumber.Visible = false;
                    lblMissing.Visible = false;
                    autopopulate();
                }
            }
        }

        protected void loadManufacturers()
        {
            using (var context = new cathlabEntities())
            {
                List<Manufacturer> temp = (from man in context.Manufacturers select man).ToList();
                Manufacturer a = new Manufacturer();
                a.ID = 0; a.Name = "New Manufacturer";
                temp.Insert(0, a);
                lbxManufacturer.DataValueField = "ID";
                lbxManufacturer.DataTextField = "Name";
                lbxManufacturer.DataSource = temp;
                lbxManufacturer.DataBind();
            }
        }

        protected void loadProductTypes()
        {
            using (var context = new cathlabEntities())
            {
                List<ProductType> temp = (from prodtype in context.ProductTypes select prodtype).ToList();
                ProductType a = new ProductType();
                a.ID = 0; a.Type = "New Type";
                temp.Insert(0, a);
                lbxProdType.DataValueField = "ID";
                lbxProdType.DataTextField = "Type";
                lbxProdType.DataSource = temp;
                lbxProdType.DataBind();
            }
        }
        #endregion NewPartNum

        #region NewManufacturer
        protected void lbxManufacturer_TextChanged(object sender, EventArgs e)
        {
            int manID = -1;
            int.TryParse(lbxManufacturer.SelectedValue, out manID);
            if (manID == 0)
            {
                pnlNewManufacturer.Visible = true;
                pnlNewPartNumber.Visible = false;
                pnlNewProdType.Visible = false;
            }
            //else
            //{ }
        }

        protected void btnManSubmit_Click(object sender, EventArgs e)
        {
            using (var context = new cathlabEntities())
            {
                string manName = tbManufacturerName.Text;
                Manufacturer man = new Manufacturer();
                man.Name = tbManufacturerName.Text;
                man.Email = tbxEmail.Text;
                man.PhoneNumber = tbxPhoneNumber.Text;
                man.Address = tbxAddress.Text;
                context.Manufacturers.Add(man);
                context.SaveChanges();
                int mmm = (int)(from mann in context.Manufacturers where mann.Name == manName select mann.ID).First();
                loadManufacturers();
                pnlNewPartNumber.Visible = true;
                pnlNewManufacturer.Visible = false;
                lbxManufacturer.SelectedValue = mmm.ToString();
            }
        }

        protected void btnManCancel_Click(object sender, EventArgs e)
        {
            pnlNewManufacturer.Visible = false;
            pnlNewProdType.Visible = false;
            pnlNewPartNumber.Visible = true;
        }
        #endregion NewManufacturer

        #region ProdType
        protected void lbxProdType_TextChanged(object sender, EventArgs e)
        {
            int prodTypeID = -1;
            int.TryParse(lbxProdType.SelectedValue, out prodTypeID);
            if (prodTypeID == 0)
            {
                pnlNewProdType.Visible = true;
                pnlNewManufacturer.Visible = false;
                pnlNewPartNumber.Visible = false;
            }
            //else
            //{ }
        }

        protected void btnPTSubmit_Click(object sender, EventArgs e)
        {
            using (var context = new cathlabEntities())
            {
                // Check for existing Prod Type?
                string ptType = tbNProdType.Text;
                ProductType pt = new ProductType();
                pt.Type = tbNProdType.Text;
                context.ProductTypes.Add(pt);
                context.SaveChanges();
                int ptID = (int)(from prodType in context.ProductTypes where prodType.Type == ptType select prodType.ID).First();
                loadProductTypes();
                pnlNewPartNumber.Visible = true;
                pnlNewProdType.Visible = false;
                loadProductTypes();
                lbxProdType.SelectedValue = ptID.ToString();
            }
        }

        protected void btnPTCancel_Click(object sender, EventArgs e)
        {
            pnlNewPartNumber.Visible = true;
            pnlNewProdType.Visible = false;
        }
        #endregion ProdType

        protected void btnFinished_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Reports/Reports.aspx?ReportName=Scanned", true);
        }
    }
}