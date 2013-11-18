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
        {
            //btnRestart_Click(sender, e);
            loadManufacturers();
            loadProductTypes();
        }

        protected void btnRestart_Click(object sender, EventArgs e)
        {
            tbPartNum.Text = string.Empty;
            lblError.Visible = false;
            pnlScan.Visible = true;
            pnlExisting.Visible = false;
            pnlNewPart.Visible = false;
            pnlNewManu.Visible = false;
            pnlNewProdType.Visible = false;
        }

        #region ExistingPartNum
        protected void tbPartNum_TextChanged(object sender, EventArgs e)
        {
            if (tbPartNum.Text != string.Empty)
            {
                tbEPartNum.Text = tbPartNum.Text;
                pnlScan.Visible = false;
                autopopulate();
            }
            else
            {
                lblError.Visible = true;
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
                    pnlExisting.Visible = true;
                    loadELocations();
                    tbEManufacturer.Text = temp.Name;
                    tbENameSize.Text = temp.NameSize;
                    tbEProdType.Text = temp.Type;
                }
                else
                {
                    pnlExisting.Visible = false;
                    pnlNewPart.Visible = true;
                }
            }
        }

        protected void loadELocations()
        {
            using (var context = new cathlabEntities())
            {
                List<Location> temp = (from loc in context.Locations select loc).ToList();
                lbxLoc.DataValueField = "ID";
                lbxLoc.DataTextField = "LocationName";
                lbxLoc.DataSource = temp;
                lbxLoc.DataBind();
            }
        }
        #endregion ExistingPartNum

        #region NewPartNum        
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

        protected void lbxManufacturer_TextChanged(object sender, EventArgs e)
        {
            if (lbxManufacturer.SelectedValue.Equals(0))
            {
                pnlNewManu.Visible = true;
                pnlNewPart.Visible = false;
                pnlNewProdType.Visible = false;
            }
            //else
            //{
            //    RadListBox item = (RadListBox)sender;
            //    lbxManufacturer.SelectedItems.Add((RadListBoxItem)item.SelectedItem);
            //}
        }

        protected void lbxProdType_TextChanged(object sender, EventArgs e)
        {
            if (lbxProdType.SelectedValue.Equals(0))
            {
                pnlNewProdType.Visible = true;
                pnlNewManu.Visible = false;
                pnlNewPart.Visible = false;
            }
        }

        protected void btnPTSubmit_Click(object sender, EventArgs e)
        {
            using (var context = new cathlabEntities())
            {
                // Check for existing Prod Type?
                ProductType pt = new ProductType();
                pt.Type = tbType.Text;
                context.ProductTypes.Add(pt);
                context.SaveChanges();
                loadProductTypes();
            }
        }

        protected void btnPTCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnNext_Click(object sender, EventArgs e)
        {

        }
        #endregion New PartNum

        // Why commeted out??
        //protected void loadLocations()
        //{
        //    using (var context = new cathlabEntities())
        //    {
        //        List<Location> temp = (from loc in context.Locations select loc).ToList();
        //        lbxLocation.DataValueField = "ID";
        //        lbxLocation.DataTextField = "LocationName";
        //        lbxLocation.DataSource = temp;
        //        lbxLocation.DataBind();
        //    }
        //}

        protected void lbxLocation_TextChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnManSubmit_Click(object sender, EventArgs e)
        {
            using (var context = new cathlabEntities())
            {
                Manufacturer man = new Manufacturer();
                man.Name = tbManufacturerName.Text;
                man.Email = tbxEmail.Text;
                man.PhoneNumber = tbxPhoneNumber.Text;
                man.Address = tbxAddress.Text;
                context.Manufacturers.Add(man);
                context.SaveChanges();
                loadManufacturers();
            }
        }

        protected void btnManCancel_Click(object sender, EventArgs e)
        {
            pnlNewManu.Visible = false;
            pnlNewProdType.Visible = false;
            pnlNewPart.Visible = true;
        }
    }
}