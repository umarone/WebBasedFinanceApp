using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;

namespace MudasirRehmanAlp.AppDAL
{
    public class AutoGenerateCodeDAL
    {
        private AppEntities db = new AppEntities();

        public OrganizationDefinition GetOrganizationDefinition(long ID)
        {
            OrganizationDefinition organization = new OrganizationDefinition();
            organization = db.OrganizationDefinitions.Where(a => a.Id == ID && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();
            return organization;
        }
        public BranchDefinition GetBranchDefinition(long Id)
        {
            BranchDefinition branch = new BranchDefinition();
            branch = db.BranchDefinitions.Where(a => a.Id == Id && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();
            return branch;
        }
        //---------- Country Code-------------
        public string AutoGenerateCountryCode()
        {
            string Code = "";
            var list = (from max in db.CountryDefinitions
                        where max.CountryCode != null
                        select max.CountryCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Province Code-------------
        public string AutoGenerateProvinceCodeCode()
        {
            string Code = "";
            var list = (from max in db.ProvinceDefinitions
                        where max.ProvinceCode != null
                        select max.ProvinceCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Region Code-------------
        public string AutoGenerateRegionCode()
        {
            string Code = "";
            var list = (from max in db.RegionDefinitions
                        where max.RegionCode != null
                        select max.RegionCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- City Code-------------
        public string AutoGenerateCityCode()
        {
            string Code = "";
            var list = (from max in db.CityDefinitions
                        where max.CityCode != null
                        select max.CityCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Area Code-------------
        public string AutoGenerateAreaCode()
        {
            string Code = "";
            var list = (from max in db.AreaDefinitions
                        where max.AreaCode != null
                        select max.AreaCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }

        //---------- Province Code-------------
        public string AutoGenerateOrganizationCode()
        {
            string Code = "";
            var list = (from max in db.OrganizationDefinitions
                        where max.OrganizationUnitCode != null
                        select max.OrganizationUnitCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Manufacture Code-------------
        public string AutoGenerateManufactureDefinitions(long IdOrganization/*,long IdBranch*/)
        {
            string Code = "";
            var list = (from max in db.ManufactureDefinitions
                        where max.ManufactureCode != null && max.OrganizationID == IdOrganization /*&& max.BranchId == IdBranch*/
                        select max.ManufactureCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }

        //---------- Category Code-------------
        public string AutoGenerateCategoryCode(long IdOrganization/*,long IdBranche*/)
        {
            string Code = "";
            var list = (from max in db.CategoryDefinitions
                        where max.CategoryCode != null && max.OrganizationID == IdOrganization /*&& max.BranchId == IdBranche*/
                        select max.CategoryCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- PaymentAgings Code-------------
        public string AutoPaymentAgingsCode(long IdOrganization/*, long IdBranche*/)
        {
            string Code = "";
            var list = (from max in db.PaymentAgings
                        where max.Code != null && max.OrganizationId == IdOrganization /*&& max.BranchId == IdBranche*/
                        select max.Code).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Colours Code-------------
        public string AutoColoursCode(long IdOrganization/*, long IdBranche*/)
        {
            string Code = "";
            var list = (from max in db.Colours
                        where max.Code != null && max.OrganizationId == IdOrganization /*&& max.BranchId == IdBranche*/
                        select max.Code).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- SizeDefinitions Code-------------
        public string AutoSizeDefinitionsCode(long IdOrganization/*, long IdBranche*/)
        {
            string Code = "";
            var list = (from max in db.SizeDefinitions
                        where max.Code != null && max.OrganizationId == IdOrganization /*&& max.BranchId == IdBranche*/
                        select max.Code).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- ProductModels Code-------------
        public string AutoProductModelsCode(long IdOrganization/*, long IdBranche*/)
        {
            string Code = "";
            var list = (from max in db.ProductModels
                        where max.Code != null && max.OrganizationId == IdOrganization /*&& max.BranchId == IdBranche*/
                        select max.Code).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Supplier Code-------------
        public string AutoGenerateSupplierCode(long IdOrganization/*,long IdBranch*/)
        {
            string Code = "";
            var list = (from max in db.SupplierDefinitions
                        where max.SupplierCode != null && max.OrganizationID == IdOrganization /*&& max.BranchId==IdBranch*/
                        select max.SupplierCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Distributor Code-------------
        public string AutoGenerateDistributorCode(long IdOrganization/*,long IdBranch*/)
        {
            string Code = "";
            var list = (from max in db.DistributorDefinitions
                        where max.DistributorCode != null && max.OrganizationID == IdOrganization /*&& max.BranchId == IdBranch*/
                        select max.DistributorCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Product Code-------------
        public string AutoGenerateProductCode(long IdOrganization/*,long IdBranch*/)
        {
            string Code = "";
            var list = (from max in db.ProductDefinitions
                        where max.ProductCode != null && max.OrganizationID == IdOrganization/* && max.BranchId == IdBranch*/
                        select max.ProductCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Currency Code-------------
        public string AutoGenerateCurrencyCode(long IdOrganization/*, long IdBranch*/)
        {
            string Code = "";
            var list = (from max in db.CurrencyDefinitions
                        where max.CurrencyCode != null && max.OrganizationID == IdOrganization /*&& max.BranchId == IdBranch*/
                        select max.CurrencyCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Purchase Order Code-------------
        public string AutoGeneratePurchaseOrderCode(long IdOrganization,long IdBranch)
        {
            string Code = "";
            var list = (from max in db.PurchaseOrders
                        where max.PurchaseOrderNO != null && max.OrganizationID == IdOrganization && max.BranchId == IdBranch
                        select max.PurchaseOrderNO).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Good Receiveds Code-------------
        public string AutoGenerateGoodReceivedsCode(long IdOrganization,long IdBranch)
        {
            string Code = "";
            var list = (from max in db.GoodReceiveds
                        where max.GRNNO != null && max.OrganizationID == IdOrganization && max.BranchId == IdBranch
                        select max.GRNNO).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Good Employee Code-------------
        public string AutoGenerateEmployeeCode(long IdOrganization)
        {
            string Code = "";

            var maxValue = db.EmployeePersonalDetails.OrderByDescending(i => i.EmployeeId).Where(a => a.OrganizationID == IdOrganization).FirstOrDefault();
            if (maxValue == null)
            {
                Code = "EMP-" + 1;
            }
            else
            {
                var maxCode = maxValue.EmployeeCode;
                maxCode = maxCode.Replace("EMP-", "");
                long newCode = Convert.ToInt64(maxCode) + 1;
                Code = "EMP-" + newCode;

            }
            return Code;
        }
        //---------- Good Receiveds Return Code-------------
        public string AutoGenerateGoodReceivedsReturnCode(long IdOrganization,long IdBranch)
        {
            string Code = "";
            var list = (from max in db.GoodReceivedReturns
                        where max.GRReturnNO != null && max.OrganizationID == IdOrganization && max.BranchId == IdBranch
                        select max.GRReturnNO).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Purchase Return Code-------------
        public string AutoGeneratePurchaseReturnCode(long IdOrganization,long IdBranch)
        {
            string Code = "";
            var list = (from max in db.PurchaseOrderReturns
                        where max.PurchaseOrderReturnNO != null && max.OrganizationID == IdOrganization && max.BranchId == IdBranch
                        select max.PurchaseOrderReturnNO).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Adjustment Note Code-------------
        public string AutoGenerateAdjustmentNoteCode(long IdOrganization,long IdBranch)
        {
            string Code = "";
            var list = (from max in db.AdjustmentNotes
                        where max.AdjustmentNoteNO != null && max.OrganizationID == IdOrganization && max.BranchId== IdBranch
                        select max.AdjustmentNoteNO).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Distributor  Code-------------
        public string AutoGenerateDistributorsCode(long IdOrganization)
        {
            string Code = "";
            var list = (from max in db.DistributorDefinitions
                        where max.DistributorCode != null && max.OrganizationID == IdOrganization
                        select max.DistributorCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Sale Order  Code-------------
        public string AutoGenerateSaleOrderCode(long IdOrganization,long IdBranch)
        {

            string Code = "";

            var maxValue = db.SaleOrders.OrderByDescending(i => i.SaleOrdeID).Where(a => a.OrganizationID == IdOrganization && a.BranchId==IdBranch).FirstOrDefault();
            if (maxValue == null)
            {
                Code = "SO-" + 1;
            }
            else
            {
                var maxCode = maxValue.SaleOrderNo;
                maxCode = maxCode.Replace("SO-", "");
                long newCode = Convert.ToInt64(maxCode) + 1;
                Code = "SO-" + newCode;

            }
            return Code;
        }
        //---------- Sale Order  Code-------------
        public string AutoGenerateStoreTransferNotesCode(long IdOrganization, long IdBranch)
        {

            string Code = "";

            var maxValue = db.StoreTransferNotes.OrderByDescending(i => i.Id).Where(a => a.OrganizationID == IdOrganization && a.FromBranchId == IdBranch).FirstOrDefault();
            if (maxValue == null)
            {
                Code = "STN-" + 1;
            }
            else
            {
                var maxCode = maxValue.Code;
                maxCode = maxCode.Replace("STN-", "");
                long newCode = Convert.ToInt64(maxCode) + 1;
                Code = "STN-" + newCode;

            }
            return Code;
        }
        //---------- Sale Invoice  Code-------------
        public string AutoGenerateSaleInvoiceCode(long IdOrganization)
        {
            string Code = "";
            var maxValue = db.SaleInvoices.OrderByDescending(i => i.SaleInvoiceID).Where(a => a.OrganizationID == IdOrganization && a.SaleInvoiceType == CommonEnums.SaleInvoiceTypeEnum.SaleInvoice).FirstOrDefault();
            if (maxValue == null)
            {
                Code = "SI-" + 1;
            }
            else
            {
                var maxCode = maxValue.SaleInvoiceNo;
                maxCode = maxCode.Replace("SI-", "");
                long newCode = Convert.ToInt64(maxCode) + 1;
                Code = "SI-" + newCode;

            }
            return Code;
        }
        //---------- Sale Invoice Net  Code-------------
        public string AutoGenerateSaleInvoiceNetCode(long IdOrganization)
        {
            string Code = "";

            var maxValue = db.SaleInvoices.OrderByDescending(i => i.SaleInvoiceID).Where(a => a.OrganizationID == IdOrganization && a.SaleInvoiceType == CommonEnums.SaleInvoiceTypeEnum.NetSaleInvoice).FirstOrDefault();
            if (maxValue == null)
            {
                Code = "SI-" + 1;
            }
            else
            {
                var maxCode = maxValue.SaleInvoiceNo;
                maxCode = maxCode.Replace("SI-", "");
                long newCode = Convert.ToInt64(maxCode) + 1;
                Code = "SI-" + newCode;

            }
            return Code;
        }
        //---------- Sale Return  Code-------------
        public string AutoGenerateSaleReturnCode(long IdOrganization)
        {
            string Code = "";
            var list = (from max in db.SaleReturnNotes
                        where max.SaleReturnCode != null && max.OrganizationID == IdOrganization
                        select max.SaleReturnCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Customers  Code-------------
        public string AutoGenerateCustomerCode(long IdOrganization)
        {
            string Code = "";
            var list = (from max in db.Customers
                        where max.CustomerCode != null && max.OrganizationID == IdOrganization
                        select max.CustomerCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- All Financial Book Years  Code-------------
        public string AutoGenerateFinancialBookYearsCode(long IdOrganization,long IdBranch)
        {
            string Code = "";

            var list = (from max in db.FinancialBookYears
                        where max.YearCode != null && max.OrganizationID == IdOrganization && max.BranchId==IdBranch
                        select max.YearCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- All Mail Setting Code-------------
        public string AutoGenerateMailSettingCode(long IdOrganization/*,long IdBranch*/)
        {
            string Code = "";

            var list = (from max in db.MailSettings
                        where max.MailCode != null && max.OrganizationID == IdOrganization /*&& max.BranchId == IdBranch*/
                        select max.MailCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- All Vouchers Generate  Code-------------
        public string AutoGenerateVouchersCPVCode(long IdOrganization, long IdBranch)
        {
            string Code = "";

            var list = (from max in db.VouchersHeads
                        where max.VoucherCode != null && max.VoucherType == CommonEnums.VoucherType.CPV && max.OrganizationID == IdOrganization && max.BranchId == IdBranch
                        select max.VoucherCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        public string AutoGenerateVouchersCRVCode(long IdOrganization,long IdBranch)
        {
            string Code = "";

            var list = (from max in db.VouchersHeads
                        where max.VoucherCode != null && max.VoucherType == CommonEnums.VoucherType.CRV && max.OrganizationID == IdOrganization && max.BranchId == IdBranch
                        select max.VoucherCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        public string AutoGenerateVouchersBPVCode(long IdOrganization, long IdBranch)
        {
            string Code = "";

            var list = (from max in db.VouchersHeads
                        where max.VoucherCode != null && max.VoucherType == CommonEnums.VoucherType.BPV && max.OrganizationID == IdOrganization && max.BranchId == IdBranch
                        select max.VoucherCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        public string AutoGenerateVouchersBRVCode(long IdOrganization, long IdBranch)
        {
            string Code = "";

            var list = (from max in db.VouchersHeads
                        where max.VoucherCode != null && max.VoucherType == CommonEnums.VoucherType.BRV && max.OrganizationID == IdOrganization && max.BranchId== IdBranch
                        select max.VoucherCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        public string AutoGenerateVouchersJVCode(long IdOrganization, long IdBranch)
        {
            string Code = "";

            var list = (from max in db.VouchersHeads
                        where max.VoucherCode != null && max.VoucherType == CommonEnums.VoucherType.JV && max.OrganizationID == IdOrganization && max.BranchId == IdBranch
                        select max.VoucherCode).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //----------------Auto Generate Customer Statement Code

        public string AutoGenerateCustomerStatementCode(long IdOrganization,long IdBranch)
        {
            string Code = "";

            var maxValue = db.CustomerStatements.OrderByDescending(i => i.Id).Where(a => a.OrganizationID == IdOrganization && a.BranchId == IdBranch).FirstOrDefault();
            if (maxValue == null)
            {
                Code = "CS-" + 1;
            }
            else
            {
                var maxCode = maxValue.Code;
                maxCode = maxCode.Replace("CS-", "");
                long newCode = Convert.ToInt64(maxCode) + 1;
                Code = "CS-" + newCode;

            }
            return Code;
        }
        public string AutoGenerateCustomerStatementCAFNo(long IdOrganization, long IdBranch)
        {
            string Code = "";

            var list = (from max in db.CustomerStatements
                        where max.CAFNo != null && max.OrganizationID == IdOrganization && max.BranchId == IdBranch
                        select max.CAFNo).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //----------------Auto Generate Customer Statement Code

        public string AutoGenerateGuarantorDefinitionsCode(long IdOrganization,long IdBranch)
        {
            string Code = "";

            var maxValue = db.GuarantorDefinitions.OrderByDescending(i => i.Id).Where(a => a.OrganizationID == IdOrganization && a.BranchId==IdBranch).FirstOrDefault();
            if (maxValue == null)
            {
                Code = "GT-" + 1;
            }
            else
            {
                var maxCode = maxValue.Code;
                maxCode = maxCode.Replace("GT-", "");
                long newCode = Convert.ToInt64(maxCode) + 1;
                Code = "GT-" + newCode;

            }
            return Code;
        }
        //---------- DepartmentDefinitions Code-------------
        public string AutoDepartmentDefinitionsCode(long IdOrganization)
        {
            string Code = "";
            var list = (from max in db.DepartmentDefinitions
                        where max.Code != null && max.OrganizationID == IdOrganization
                        select max.Code).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- DesignationDefinitions Code-------------
        public string AutoDesignationDefinitionsCode(long IdOrganization)
        {
            string Code = "";
            var list = (from max in db.DesignationDefinitions
                        where max.Code != null && max.OrganizationID == IdOrganization
                        select max.Code).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- EducationInstitutes Code-------------
        public string AutoEducationInstitutesCode(long IdOrganization)
        {
            string Code = "";
            var list = (from max in db.EducationInstitutes
                        where max.Code != null && max.OrganizationID == IdOrganization
                        select max.Code).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- EducationDegrees Code-------------
        public string AutoEducationDegreesCode(long IdOrganization)
        {
            string Code = "";
            var list = (from max in db.EducationDegrees
                        where max.Code != null && max.OrganizationID == IdOrganization
                        select max.Code).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
        //---------- Category Code-------------
        public string AutoGenerateBranchCode(long IdOrganization)
        {
            string Code = "";
            var list = (from max in db.BranchDefinitions
                        where max.Code != null && max.OrganizationId == IdOrganization
                        select max.Code).ToList();
            long maxvalue = 0;
            if (list.Count() != 0)
                maxvalue = list.Select(long.Parse).ToList().Max();
            if (maxvalue == 0)
            {
                Code = "1";
            }
            else
            {
                long newItemCode = maxvalue + 1;
                Code = newItemCode.ToString();
            }
            return Code;
        }
    }
}