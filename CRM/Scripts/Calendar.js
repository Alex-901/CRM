var month = new Array();
month[0] = "January";
month[1] = "February";
month[2] = "March";
month[3] = "April";
month[4] = "May";
month[5] = "June";
month[6] = "July";
month[7] = "August";
month[8] = "September";
month[9] = "October";
month[10] = "November";
month[11] = "December";

(function ($) {
    $.fn.createCalendar = function (options) {

        var settings = $.extend({
            date: new Date(),
            format: "month"
        }, options);

        var currentDay = settings.date.getDate();
        var currentMonth = settings.date.getMonth();
        var currentYear = settings.date.getFullYear();

        var calendarActualStart = new Date(currentYear, currentMonth, 1);
        var calendarActualEnd = new Date(currentYear, currentMonth + 1, 0);

        var data = getHistoryData(settings.date);

        //Get the first of the selected month then find the previous monday to get the calendar start.
        var firstDayOfCalendar = getMonday(calendarActualStart);

        var maxRows = settings.format == "month" ? 5 : 1;
        var maxCols = settings.format == "month" ? 6 : 6;

        var currentRow = 0;
        var day = new Date(firstDayOfCalendar);
        var calendar =
              "<div class='calendar-header tableDiv'>"
            + "<div class='tableDivRow'>"
            + "<div class='tableDivCell'>"
            + "<i onclick=\"navCal('" + currentMonth + "', '" + currentYear + "', 'left')\" class='fa fa-arrow-circle-left fa-fw' aria-hidden='true'></i>"
            + "</div>"
            + "<div class='tableDivCell' onclick=\"showModal('date-selection')\">"
            + month[currentMonth] + " - " + currentYear
            + "</div>"
            + "<div class='tableDivCell'>"
            + "<i onclick=\"navCal('" + currentMonth + "', '" + currentYear + "', 'right')\" class='fa fa-arrow-circle-right fa-fw' aria-hidden='true'></i>"
            + "</div>"
            + "</div>"
            + "</div><table>";

        while (currentRow <= maxRows) {

            var currentColumn = 0;

            calendar += currentRow != 0 ? "<tr>" : "<tr>"
            + "<th>MON</th>"
            + "<th>TUE</th>"
            + "<th>WED</th>"
            + "<th>THU</th>"
            + "<th>FRI</th>"
            + "<th>SAT</th>"
            + "<th>SUN</th>"
            + "</tr>";
            + "<tr>";

            while (currentColumn <= maxCols) {

                calendar += day >= calendarActualStart && day <= calendarActualEnd ? "<td>" : "<td class='outside-month'>";
                calendar += "<div id='date-cont'>" + day.getDate().toString() + "</div>";

                for (var i = 0; i < data.length; i++) {
                    var dateCheck = new Date(parseInt(data[i].CreationDate.replace("/Date(", "").replace(")/", ""), 10));

                    if (dateCheck.setHours(0, 0, 0, 0) == day.setHours(0, 0, 0, 0)) {
                        calendar += "<div id='cal-inner-cont' onclick='showCalendarPopup(" + data[i].Id.toString() + ")'>" + data[i].Contact.Forename.toString() + " - " + data[i].Notes.toString() + "</div>";
                    }
                }

                calendar += "</td>";

                day.setDate(day.getDate() + 1);
                currentColumn++;
            }

            calendar += "</tr>";
            currentRow++;
        }

        calendar += "</table>";

        this.empty();
        this.append(calendar);

        return this;
    };
}(jQuery));


function getHistoryData(date) {
    var retVal = {};

    var data = {
        'date': date,
        'agencyId': 0
    };

    $.ajax({
        type: "POST",
        url: "/Agency/LoadCalendarHistoryItems",
        contentType: 'application/json',
        data: JSON.stringify(data),
        async: false
    }).success(function (items) {
        retVal = items;
        $('#month-data-container').val(JSON.stringify(items));
    }).error(function (xhr, textStatus, error) {
        console.log(xhr.statusText);
        console.log(textStatus);
        console.log(error);
    });

    return retVal;
}

function getMonday(d) {
    d = new Date(d);
    var day = d.getDay(),
        diff = d.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday
    return new Date(d.setDate(diff));
}

function convertCSDate(input) {
    return new Date(parseInt(input.replace("/Date(", "").replace(")/", ""), 10));
}

function getDateString(input) {
    return input.getDate() + '/' + input.getMonth() + '/' + input.getFullYear() + ' - ' + input.getHours() + ':' + input.getMinutes();
}

function showCalendarPopup(id) {
    var monthData = JSON.parse($('#month-data-container').val());

    for (var i = 0; i < monthData.length; i++) {
        if (monthData[i].Id == id) {
            $('#calendar-detail').find('#date').html(getDateString(convertCSDate(monthData[i].CreationDate)));
            $('#calendar-detail').find('#contact').html(monthData[i].Contact.Forename + ' ' + monthData[i].Contact.Surname);
            $('#calendar-detail').find('#note').html(monthData[i].Notes);
        }
    }

    showModal('calendar-detail');
}

function navCal(currentMonth, currentYear, direction) {

    var nextDate;

    if (currentMonth == 12 && direction == "right") {
        nextDate = new Date(currentYear + 1, 0, 1);
    }
    else if (currentMonth == 0 && direction == "left") {
        nextDate = new Date(currentYear - 1, 11, 1);
    }
    else {
        nextDate = new Date(currentYear, Number(currentMonth) + (direction == "left" ? -1 : 1), 1);
    }

    $('.calendar-container').createCalendar({ date: nextDate });
}
