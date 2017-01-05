﻿using System.ComponentModel.DataAnnotations;

namespace RedQ.UnderwritingService.Models.NewYork
{
    public class UnderwritingHOIBestCase
    {
        public double  PurchasePriceAllIn { get; set; }
        public double  TotalInvestment { get; set; }
        public double  CashRequirement { get; set; }
        public double  NetProfit { get; set; }
        public double  ROILoan { get; set; }
    }


}