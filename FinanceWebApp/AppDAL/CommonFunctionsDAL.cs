using MudasirRehmanAlp.ModelsView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.AppDAL
{
    public class CommonFunctionsDAL
    {
        public int GetValueOfEnumByName(string EnumName,string EnumValue)
        {
            int value = 0;
            if (EnumName== "DepartmentType" && !String.IsNullOrEmpty(EnumValue))
            {
                value = CommonFunctions.GetValueOfEnumByName<CommonEnums.DepartmentType>(EnumValue);
            }
            else if (EnumName == "SaleOrderCustomerType" && !String.IsNullOrEmpty(EnumValue))
            {
                value = CommonFunctions.GetValueOfEnumByName<CommonEnums.SaleOrderCustomerType>(EnumValue);
            }
            else if (EnumName == "AccountDefaultType" && !String.IsNullOrEmpty(EnumValue))
            {
                value = CommonFunctions.GetValueOfEnumByName<CommonEnums.AccountDefaultType>(EnumValue);
            }
            else if (EnumName == "OwnershipType" && !String.IsNullOrEmpty(EnumValue))
            {
                value = CommonFunctions.GetValueOfEnumByName<CommonEnums.OwnershipType>(EnumValue);
            }
            else if (EnumName == "NotificationType" && !String.IsNullOrEmpty(EnumValue))
            {
                value = CommonFunctions.GetValueOfEnumByName<CommonEnums.NotificationType>(EnumValue);
            }
            else if (EnumName == "NotificationPriority" && !String.IsNullOrEmpty(EnumValue))
            {
                value = CommonFunctions.GetValueOfEnumByName<CommonEnums.NotificationPriority>(EnumValue);
            }
            return value;
        }
    }
}