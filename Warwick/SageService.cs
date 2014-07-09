using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  
using Warwick.SageServiceReference;

namespace Warwick
{
    public class SageService : IDisposable
    {
        private WebServiceSoapPortClient sage;
        public SessionHeader sessionobj;
        public string centerno;
        public int nextid;
        public int cusid = 0;
        public int contactid;
        public DateTime cusupdate;

        public int productid;
        public int productfamily;
        public int productuom;

        public SageService(string username, string password)
        {
            sage = new WebServiceSoapPortClient();
            logonresult logonrs = sage.logon(username, password);
            sessionobj = new SessionHeader();
            sessionobj.sessionId = logonrs.sessionid;
        }

        public SageService()
        {

        }

        public string SageQueryCustomID()
        {
            string strNextId;

            queryrecordresult CRMQueryRecordResult = sage.queryrecord(sessionobj, "ctid_center ,ctid_nextid ,ctid_customidid, ctid_updateddate", "ctid_entity ='Order'", "Customid", "");
            crmrecord[] EntityNameList = CRMQueryRecordResult.records;
            if (EntityNameList.Length > 0)
            {
                for (int intCount = 0; intCount < EntityNameList.Length; intCount++)
                {
                    recordfield[] CRMFieldList = EntityNameList[intCount].records;

                    recordfield ctCenter = (recordfield)CRMFieldList[0];
                    recordfield ctNextId = (recordfield)CRMFieldList[1];
                    recordfield ctCusId = (recordfield)CRMFieldList[2];
                    recordfield ctUpdate = (recordfield)CRMFieldList[3];

                    centerno = SageQueryCenterCaptCode(ctCenter.value.ToString());
                    nextid = Convert.ToInt32(ctNextId.value.ToString());
                    cusid = Convert.ToInt32(ctCusId.value.ToString());
                    cusupdate = Convert.ToDateTime(ctUpdate.value.ToString());

                }
            }

            DateTime dtNow = DateTime.Now;
            if (dtNow.Year > cusupdate.Year)
                nextid = 0;

            if(nextid.ToString().Length > 4)
                strNextId = nextid.ToString();
            else
                strNextId = nextid.ToString("D4");

            return String.Format("{0}{1}{2}", centerno, dtNow.Year.ToString(), strNextId);  
            
        }

        private string SageQueryCenterCaptCode(string code)
        {
            string returnString = code;
            if(code == "siam")
            {
                returnString = "40";
            }
            return returnString;
        }

        public void SageInsertOrder(
            string Tx, 
            string Ref1, 
            string Ref2, 
            string Amount, 
            string Time, 
            DateTime KbankDate,
            string orde_customorderid
            )
        {

            //query Person from ref2
            if (SagePersonQuery(Ref2))
            {
                //query product from ref1
                if (SageProductQuery(Ref1))
                {
                    Console.WriteLine(productid);
                    string[] aTime = Time.Split(':');

                    DateTime opened = new DateTime(
                        KbankDate.Year, 
                        KbankDate.Month, 
                        KbankDate.Day, 
                        Convert.ToInt32(aTime[0]), 
                        Convert.ToInt32(aTime[1]), 
                        Convert.ToInt32(aTime[2])
                        );
                    ewarebase[] list = new ewarebase[1];
                    orders ord = new orders();


                    //ord.grossamt = double.Parse(Amount);
                    //ord.grossamt = Convert.ToDouble(Amount);
                    //ord.grossamt = 7.9;
                    // ord.grossamt = (double)Convert.ToDecimal(Amount);
                    //ord.grossamtSpecified = true;

                    ord.discountamt = 10.50;
                    ord.discountamtSpecified = true;
                    ord.discounttype = "PC";
                    ord.expiredelivery = DateTime.Now;

                    //ord.nettamt = double.Parse(Amount, System.Globalization.CultureInfo.InvariantCulture);
                    //ord.nettamt = Convert.ToDouble(Amount);
                    //ord.nettamtSpecified = true;
                    // ord.opened = opened;
                    // ord.openedSpecified = true;
                    ord.billaddress = "";
                    ord.currency = "5";

                    Console.WriteLine(ord.grossamt);

                    #region orderitems
                    ord.orderitems = new ewarebaselist();
                    ord.orderitems.records = new ewarebase[1];
                    ord.orderitems.entityname = "OrderItems";

                    orderitems neworderitems = new orderitems();
                    if (productid != null)
                    {
                        neworderitems.productid = productid;
                        neworderitems.productidSpecified = true;
                        ord.orderitems.records[0] = neworderitems;
                    }
                    #endregion

                    list[0] = ord;
                    addresult result = sage.add(sessionobj, "orders", list);
                    crmid newRecID = (crmid)result.records[0];

                    //SageInsertOrderItem();

                    //return newRecID.crmid1;
                }
                
            }



            

            //if already person check product

            //if already product add new order and orderitem

            //add new receipt
            //orders ord = new orders();
            //ord.grossamt = double.Parse(Amount, System.Globalization.CultureInfo.InvariantCulture);
            //ord.grossamtSpecified = true;

            //ewarebase[] list = new ewarebase[1];
            //list[0] = ord;
            //addresult result = sage.add(sessionobj, "orders", list);
            //crmid newRecID = (crmid)result.records[0];
            //return newRecID.crmid1;

        }
        public void SageInsertOrderItem()
        {
            orderitems ort = new orderitems();
            ewarebase[] listort = new ewarebase[1];
            ort.productid = productid;
            ort.productidSpecified = true;
            //ort.orderquoteid = newRecID.crmid1;
            //ort.orderquoteidSpecified = true;
            listort[0] = ort;
            addresult result1 = sage.add(sessionobj, "orderitems", listort);
        }

        private bool SagePersonQuery(string Ref2)
        {
            string queryString = String.Format("pers_studentID = '{0}'", Ref2);
            queryrecordresult CRMQueryRecordResult = sage.queryrecord(sessionobj, "Pers_PersonId", queryString, "Person", "");
            crmrecord[] EntityNameList = CRMQueryRecordResult.records;
            if (EntityNameList != null)
            {
                if (EntityNameList.Length > 0)
                {
                    for (int intCount = 0; intCount < EntityNameList.Length; intCount++)
                    {
                        recordfield[] CRMFieldList = EntityNameList[intCount].records;

                        recordfield personId = (recordfield)CRMFieldList[0];

                        contactid = Convert.ToInt32(personId.value.ToString());
                    }
                    return true;
                }
                else
                {
                    contactid = 0;
                    return false;
                }
            }
            else
            {
                contactid = 0;
                return false;
            }
        }

        private bool SageProductQuery(string Ref1)
        {
            string queryString = String.Format("prod_code = '{0}'", Ref1);
            queryrecordresult CRMQueryRecordResult = sage.queryrecord(sessionobj, "Prod_ProductID, prod_productfamilyid, prod_UOMCategory", queryString, "NewProduct", "");
            crmrecord[] EntityNameList = CRMQueryRecordResult.records;
            if (EntityNameList != null)
            {
                if (EntityNameList.Length > 0)
                {
                    for (int intCount = 0; intCount < EntityNameList.Length; intCount++)
                    {
                        recordfield[] CRMFieldList = EntityNameList[intCount].records;

                        recordfield productId = (recordfield)CRMFieldList[0];
                        recordfield productFamilyId = (recordfield)CRMFieldList[1];
                        recordfield productUOMCategory = (recordfield)CRMFieldList[0];
                        
                        productid = Convert.ToInt32(productId.value.ToString());
                        productfamily = Convert.ToInt32(productFamilyId.value.ToString());
                        productuom = Convert.ToInt32(productUOMCategory.value.ToString());
                        Console.WriteLine(productid);
                    }
                    return true;
                }
                else
                {
                    productid = 0;
                    productfamily = 0;
                    productuom = 0;
                    return false;
                }
            }
            else
            {
                productid = 0;
                productfamily = 0;
                productuom = 0;
                return false;
            }
        }

        public void SageUpdateCustomId()
        {
            DateTime dtNow = DateTime.Now;
            int NextId;
            ewarebase[] CRMBase;
            updateresult CRMUpdateResult;
            customid cusId = new customid();
            try
            {
                if (dtNow.Year > cusupdate.Year)
                    NextId = 0;
                else
                    NextId = nextid + 1;
 
                CRMBase = new ewarebase[1];
                cusId.customidid = cusid;
                cusId.customididSpecified = true;
                cusId.entity = "Order";
                cusId.nextid = NextId;
                cusId.nextidSpecified = true;
                CRMBase[0] = cusId;
                CRMUpdateResult = sage.update(sessionobj, "customid", CRMBase);
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            sage.logoff(sessionobj, sessionobj.sessionId);
        }

        #endregion

    }
}
