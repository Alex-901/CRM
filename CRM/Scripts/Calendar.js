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
            date: new Date()
        }, options);

        var currentDay = settings.date.getDate();
        var currentMonth = settings.date.getMonth();
        var currentYear = settings.date.getFullYear();

        var data = getHistoryData(settings.date);

        //Get the first of the selected month then find the previous monday to get the calendar start.
        var firstDayOfCalendar = getMonday(new Date(currentYear, currentMonth, 1));

        var currentRow = 0;
        var currentDay = new Date(firstDayOfCalendar);
        var calendar = "<h2>" + month[currentDay.getMonth() + 1] + " - " + currentDay.getFullYear() + "</h2><input id='calender-date-pick' type='date' class='form-control' /><table>";

        while (currentRow <= 5) {

            var currentColumn = 0;

            calendar += currentRow != 0 ? "<tr>" : "<tr>"
            + "<th>Mon</th>"
            + "<th>Tue</th>"
            + "<th>Wed</th>"
            + "<th>Thu</th>"
            + "<th>Fri</th>"
            + "<th>Sat</th>"
            + "<th>Sun</th>"
            + "</tr>";
            + "<tr>";

            while (currentColumn <= 6) {

                calendar += "<td>";
                calendar += "<div id='date-cont'>" + currentDay.getDate().toString() + "</div>";

                for (var i = 0; i < data.length; i++) {
                    var dateCheck = new Date(parseInt(data[i].CreationDate.replace("/Date(", "").replace(")/", ""), 10));

                    if (dateCheck.setHours(0, 0, 0, 0) == currentDay.setHours(0, 0, 0, 0)) {
                        calendar += "<div id='cal-inner-cont' onclick='showCalendarPopup(" + data[i].Id.toString() + ")'>" + data[i].Notes.toString() + "</div>";
                    }
                }

                calendar += "</td>";

                currentDay.setDate(currentDay.getDate() + 1);
                currentColumn++;
            }

            calendar += "</tr>";
            currentRow++;
        }

        calendar += "</table>";

        this.empty();
        this.append(calendar);

        $('#calender-date-pick').change(function (e) {
            $('.calendar-container').createCalendar({ date: new Date($(this).val()) });
        });

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
    return input.getDate() + '/' + input.getMonth() + '/' + input.getFullYear() + ':' + input.getTime();
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