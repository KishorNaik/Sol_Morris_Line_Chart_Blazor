using Sol_Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MorrisLineChart;
using Newtonsoft.Json;

namespace Sol_Demo.Pages.Demo
{
    public partial class LineChartDemo
    {
        #region Private Property

        // Component Referance
        private MorrisLinesChart LineChartM { get; set; }

        private bool IsLoad { get; set; }

        private String LineChartResultSetJson { get; set; }

        private String XKey { get; set; }

        private String YKeysJson { get; set; }

        private String LablesJson { get; set; }

        private dynamic DataMessage { get; set; }

        #endregion Private Property

        #region Private Member

        private Task<List<SalesModel>> GetSalesDataAsync()
        {
            return Task.Run(() =>
            {
                var salesDataList = new List<SalesModel>();
                salesDataList.Add(new SalesModel() { Year = "2015", SalesAmount = 100, ExpenseAmount = 70 });
                salesDataList.Add(new SalesModel() { Year = "2016", SalesAmount = 200, ExpenseAmount = 120 });
                salesDataList.Add(new SalesModel() { Year = "2017", SalesAmount = 500, ExpenseAmount = 300 });
                salesDataList.Add(new SalesModel() { Year = "2018", SalesAmount = 800, ExpenseAmount = 1000 });
                salesDataList.Add(new SalesModel() { Year = "2019", SalesAmount = 1000, ExpenseAmount = 300 });
                salesDataList.Add(new SalesModel() { Year = "2020", SalesAmount = 100, ExpenseAmount = 1000 });

                return salesDataList;
            });
        }

        #endregion Private Member

        #region Ui event Handler

        private async Task OnDisplaySalesChart()
        {
            var salesListData = await this.GetSalesDataAsync();

            this.LineChartResultSetJson = JsonConvert.SerializeObject(salesListData);
            this.XKey = "Year";
            this.YKeysJson = JsonConvert.SerializeObject(new String[] { "SalesAmount", "ExpenseAmount" });
            this.LablesJson = JsonConvert.SerializeObject(new String[] { "Sales", "Expense" });

            base.StateHasChanged();

            await this.LineChartM.SetLineChartAsync();
        }

        private void OnHoverGetData(dynamic data)
        {
            DataMessage = data;
        }

        #endregion Ui event Handler

        #region Protected Member

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                IsLoad = true;
                base.StateHasChanged();
            }
        }

        #endregion Protected Member
    }
}