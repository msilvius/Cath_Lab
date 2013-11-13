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
        public static int pnum;

        protected void Page_Load(object sender, EventArgs e)
        {
            //txt.Text = Request.QueryString["partNum"];
            //RadTextBox tb = (RadTextBox)Page.Parent.FindControl("txtPartNum");
            //int.TryParse(tb.Text, out pnum);
            loadLocations();
            loadManufacturers();
            loadProductTypes();
        }

        protected void loadManufacturers()
        {
            using (var context = new cathlabEntities())
            {
                List<Manufacturer> temp = (from man in context.Manufacturers select man).ToList();
                Manufacturer a = new Manufacturer();
                a.ID = 0; a.Name = "<New Manufacturer>";
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
                a.ID = 0; a.Type = "<New Type>";
                temp.Insert(0, a);
                lbxProdType.DataValueField = "ID";
                lbxProdType.DataTextField = "Type";
                lbxProdType.DataSource = temp;
                lbxProdType.DataBind();
            }
        }

        protected void loadLocations()
        {
            using (var context = new cathlabEntities())
            {
                List<Location> temp = (from loc in context.Locations select loc).ToList();
                lbxLocation.DataValueField = "ID";
                lbxLocation.DataTextField = "LocationName";
                lbxLocation.DataSource = temp;
                lbxLocation.DataBind();
            }
        }

        protected void lbxLocation_TextChanged(object sender, EventArgs e)
        {
            
        }

        protected void lbxManufacturer_TextChanged(object sender, EventArgs e)
        {
            pnlNewManu.Visible = true;
            pnlNewPart.Visible = false;
            pnlNewProdType.Visible = false;
        }

        protected void lbxProdType_TextChanged(object sender, EventArgs e)
        {
            pnlNewProdType.Visible = true;
            pnlNewManu.Visible = false;
            pnlNewPart.Visible = false;            
        }

        protected void txtPartNum_TextChanged(object sender, EventArgs e)
        {
            autopopulate();
        }

        protected void autopopulate()
        {
            string part = txtPartNum.Text;
            using (var context = new cathlabEntities())
            {
                var temp = (from pnum in context.PartNumbers
                            where pnum.PartNum == part
                            select new { pnum.Manufacturer.Name, pnum.NameSize, pnum.ProductType.Type }).SingleOrDefault();
                if (temp != null)
                {
                    pnlLotExpLoc.Visible = true;
                    loadLocs();
                    //txtManufacturer.Text = temp.Name;
                    //txtNameSize.Text = temp.NameSize;
                    //txtProdType.Text = temp.Type;
                }
                else
                {
                    pnlLotExpLoc.Visible = false;
                    pnlNewPart.Visible = true;
                }
            }
        }

        protected void loadLocs()
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtPartNum.Text = string.Empty;
            pnlLotExpLoc.Visible = false;
            pnlNewPart.Visible = false;
        }
    }
}