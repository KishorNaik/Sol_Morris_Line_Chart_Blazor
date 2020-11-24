/// <reference path="morrisjslib/morris.js" />

export function drawLineChart(divId, itemSourceJson, xKey, yKeys, lablesjson) {
    // Display the LineJson Data
    //console.log(lineJson);

    var data = JSON.parse(itemSourceJson);
    console.log("data:", data);

    console.log("xkey:", xKey);

    var yKeysValue = JSON.parse(yKeys);
    console.log("Ykeys:", yKeys);

    var lablesShow = JSON.parse(lablesjson);
    console.log(lablesShow);

    var counter = 1;

    new Morris.Line({
        // ID of the element in which to draw the chart.
        element: divId,
        // Chart data records -- each entry in this array corresponds to a point on
        // the chart.
        data: data,
        // The name of the data record attribute that contains x-values.
        xkey: xKey,
        // A list of names of data record attributes that contain y-values.
        ykeys: yKeysValue,
        // Labels for the ykeys -- will be displayed when you hover over the
        // chart.
        labels: lablesShow,

        hoverCallback: function (index, options, content, row) {
            options.hideHover = false;
            options.labels = lablesShow;

            console.log(options);

            if (counter == 3) {
                console.log(row);

                var rowJson = JSON.stringify(row);

                DotNet.invokeMethodAsync("MorrisLineChart", "OnHoverJs", rowJson);
                counter = 1;
                return;
            }
            counter++;
        }
    });

    //new Morris.Line({
    //    // ID of the element in which to draw the chart.
    //    element: 'divlineChart',
    //    // Chart data records -- each entry in this array corresponds to a point on
    //    // the chart.
    //    data: [
    //        { year: '2008', value: 20 },
    //        { year: '2009', value: 10 },
    //        { year: '2010', value: 5 },
    //        { year: '2011', value: 5 },
    //        { year: '2012', value: 20 }
    //    ],
    //    // The name of the data record attribute that contains x-values.
    //    xkey: 'year',
    //    // A list of names of data record attributes that contain y-values.
    //    ykeys: ['value'],
    //    // Labels for the ykeys -- will be displayed when you hover over the
    //    // chart.
    //    labels: ['Value']
    //});
}