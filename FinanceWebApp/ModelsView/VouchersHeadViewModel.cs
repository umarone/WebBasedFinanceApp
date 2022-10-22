using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.ModelsView
{
    public class VouchersHeadViewModel
    {
        public long VoucherID { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> BranchId { get; set; }
        public string VoucherCode { get; set; }
        public Nullable<VoucherType> VoucherType { get; set; }
        public Nullable<DateTime> VoucherDate { get; set; }
        public Nullable<PaymentType> PaymentType { get; set; }
        public Nullable<long> PurchaseId { get; set; }
        public Nullable<long> SaleId { get; set; }
        public Nullable<long> SheetNo { get; set; }
        public Nullable<long> FinancialBookYearId { get; set; }
        public Nullable<long> TerminalNo { get; set; }
        public string ChequeNo { get; set; }
        public string EmpCode { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<Guid> TransactionKey { get; set; }
        public Nullable<bool> Posted { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public List<VouchersDetailViewModel> VouchersDetailViewList { get; set; }
    }
    public class VouchersDetailViewModel
    {
        public long VoucherDetailID { get; set; }
        public Nullable<long> VoucherId { get; set; }
        public Nullable<long> AccountId { get; set; }
        public string Narration { get; set; }
        public Nullable<decimal> ClosingBalance { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
    }
}