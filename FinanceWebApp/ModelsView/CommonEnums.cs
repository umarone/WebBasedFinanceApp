using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class CommonEnums
    {
        public enum SaleInvoiceTypeEnum
        {
            SaleInvoice = 1,
            NetSaleInvoice=2
        }
        public enum VoucherType
        {
            CPV = 1,
            CRV = 2,
            BPV = 3,
            BRV = 4,
            JV = 5,
        }
        public enum PaymentType
        {
            [Display(Name = "Purchase Order")]
            PurchaseOrder = 1,
            [Display(Name = "Sale Order")]
            SaleOrder = 2,
            [Display(Name = "Other")]
            Othere = 3,
           
        }
        public enum AccountDefaultType
        {
            [Display(Name = "Purchases")]
            Purchases = 1,
            [Display(Name = "Sales")]
            Sales = 2,
            [Display(Name = "Other")]
            Othere = 3,

        }
        public enum SaleOrderCustomerType
        {
           
            [Display(Name = "Customer Statement")]
            CustomerStatement = 1,
            [Display(Name = "Distributor")]
            Distributor = 2,
        }
        public enum PaymentStatus
        {
            UnPaid = 0,            
            Paid = 1,
        }
        public enum DepartmentType
        {
            HR = 1,
            Sale = 2,
            Purchase = 3,
            Accounts = 4,
            IT = 5,
        }
        public enum TransactionType
        {
            Cash = 1,
            Credit = 2,
            [Display(Name = "HP Credit")]
            HPCredit = 3         
        }
        public enum PuchaseOrderType
        {
            Local = 1,
            Import = 2,
            Other = 3
        }
        public enum PaymentAging
        {
            Local = 1,
            Import = 2,
            Other = 3,

        }
        public enum OwnershipType
        {
            Owner = 1,
            Rented = 2,
          
        }
        public enum StockType
        {
            InStock = 1,
            OutStock = 2,
        }
        public enum DashboardReportType
        {
            Today = 1,
            Weekly = 2,
            Monthly = 3,
        }
        public enum NotificationType
        {
            General = 1,
            Specific = 2          
        }
        public enum NotificationPriority
        {
            Low = 1,
            Medium = 2,
            High = 3
        }
        public enum NotificationGeneralCode
        {
            SalesAccount = 1,
            PurchasesAccount = 2,
            FinancialYear = 3,
            Currency=4,
            GeneralStockMinQty = 5,
            LowStock = 6,
            Others = 7,
        }
    }
}