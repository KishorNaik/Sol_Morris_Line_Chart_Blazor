using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorrisLineChart
{
    public partial class MorrisLinesChart
    {
        #region Declaration

        private Task<IJSObjectReference> _module = null;

        #endregion Declaration

        #region Public Property

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public String ItemSourceJson { get; set; }

        [Parameter]
        public String Xkey { get; set; }

        [Parameter]
        public String YKeysJson { get; set; }

        [Parameter]
        public String LablesJson { get; set; }

        [Parameter]
        public EventCallback<dynamic> OnHover { get; set; }

        [Parameter]
        public int Height { get; set; }

        #endregion Public Property

        #region Private Property

        private ElementReference DivLineChartElement { get; set; }

        private static Action<Task<dynamic>> ActionJs { get; set; }

        #endregion Private Property

        #region Private Method

        private void LoadJsModules()
        {
            _module = JSRuntime
                            .InvokeAsync<IJSObjectReference>("import", "./_content/MorrisLineChart/LineChart.js")
                            .AsTask();
        }

        private String SetHeightStyle()
        {
            return
                new StringBuilder()
                .Append("height:")
                .Append(Height)
                .Append("px;")
                .ToString();
        }

        private async Task OnLoadLineChartJs()
        {
            await (await _module).InvokeVoidAsync(identifier: "drawLineChart", DivLineChartElement, ItemSourceJson, Xkey, YKeysJson, LablesJson);
        }

        #endregion Private Method

        #region Public & Protected Method

        [JSInvokable]
        public static Task OnHoverJs(string datajson)
        {
            return Task.Run(() =>
            {
                var data = JsonConvert.DeserializeObject<dynamic>(datajson);

                ActionJs.Invoke(Task.FromResult<dynamic>(data));
            });
        }

        public async Task SetLineChartAsync()
        {
            LoadJsModules();

            await this.OnLoadLineChartJs();

            base.StateHasChanged();
        }

        public async ValueTask DisposeAsync()
        {
            if (_module != null)
            {
                await (await _module).DisposeAsync();
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                ActionJs = async (data) =>
                  {
                      await base.InvokeAsync(async () =>
                      {
                          await this.OnHover.InvokeAsync(await data);
                          this.StateHasChanged();
                      });
                  };
            }
        }

        #endregion Public & Protected Method
    }
}