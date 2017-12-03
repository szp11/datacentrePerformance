using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChartJsTest2.Models;
using ChartJSCore.Models;

namespace ChartJsTest2.Controllers
{
    public class ChartController : Controller
    {
        [Route("~/Chart/ChartData")]
        [HttpGet]
        public int[] ChartData()
        {
            int[] data = { 59, 80, 81, 56, 55, 40, 72 };

            return data;
        }

        public List<string> ChartLabels()
        {
            List<string> lables = new List<string>() { "2", "3", "4", "5", "6", "7", "8" };

            return lables;
        }

        public IActionResult LineChart()
        {
            var chart = new Chart();
            chart.Type = "line";

            var data = new Data();
            data.Labels = new List<string>() { "January", "February", "March", "April", "May", "June", "July" };

            LineDataset dataset = new LineDataset()
            {
                Label = "Line Chart",
                Data = new List<double>() { 65, 59, 80, 81, 56, 55, 40 },
                Fill = false,
                LineTension = 0.1,
                BackgroundColor = "rgba(75, 192, 192, 0.4)",
                BorderColor = "rgba(75,192,192,1)",
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<string>() { "rgba(75,192,192,1)" },
                PointBackgroundColor = new List<string>() { "#fff" },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<string>() { "rgba(75,192,192,1)" },
                PointHoverBorderColor = new List<string>() { "rgba(220,220,220,1)" },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);
            chart.Data = data;
            ViewData["chart"] = chart;

            return View();
        }

        
    }
}
