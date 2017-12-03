using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChartJsTest2.Views.Chart
{
    public class LineChartModel : PageModel
    {

        public string Message { get; set; }

        public void OnGet()
        {
            Message = "This is a line chart.";
        }
    }
}
