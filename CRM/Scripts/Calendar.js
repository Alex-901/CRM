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

        //TODO Convert this to a view
        var calendar =
          "<div class='calendar-header tableDiv'>"
            + "<div class='tableDivRow'>"
                + "<div class='tableDivCell'>"
                + "</div>"
                + "<div class='tableDivCell'>"
                    + "<div class='calendar-header-inner tableDiv'>"
                        + "<div class='tableDivRow'>"
                            + "<div class='tableDivCell clickable'>"
                                + "<i onclick=\"navCal('" + currentMonth + "', '" + currentYear + "', 'left')\" class='fa fa-arrow-circle-left fa-fw' aria-hidden='true'></i>"
                            + "</div>"
                            + "<div class='tableDivCell clickable' onclick=\"showModal('date-selection')\">"
                                + month[currentMonth] + " - " + currentYear
                            + "</div>"
                            + "<div class='tableDivCell clickable'>"
                                + "<i onclick=\"navCal('" + currentMonth + "', '" + currentYear + "', 'right')\" class='fa fa-arrow-circle-right fa-fw' aria-hidden='true'></i>"
                            + "</div>"
                        + "</div>"
                    + "</div>"
                + "</div>"
                + "<div id='cal-drag-cont' class='tableDivCell' title='Drag to a date to create new item'>"
                    + "<i class='fa fa-plus-circle draggable clickable' aria-hidden='true'></i>"
                + "</div>"

            + "</div>"
        + "</div>"
        + "<table>";

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

                calendar += day >= calendarActualStart && day <= calendarActualEnd ? "<td class='dropable'>" : "<td class='outside-month'>";
                calendar += "<div id='divActualDate' class='hideMe'>" + day.toString() + "</div>";
                calendar += "<div id='date-cont'>" + day.getDate().toString() + "</div>";

                for (var i = 0; i < data.length; i++) {
                    var dateCheck = new Date(parseInt(data[i].CreationDate.replace("/Date(", "").replace(")/", ""), 10));

                    if (dateCheck.setHours(0, 0, 0, 0) == day.setHours(0, 0, 0, 0)) {
                        calendar += "<div id='cal-inner-cont' onclick='showCalendarPopup(" + data[i].Id.toString() + ")'>" + data[i].Agency.Name.toString() + " - " + data[i].Contact.Forename.toString() + " - " + data[i].Notes.toString() + "</div>";
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

function showCalendarPopup(id) {
    var monthData = JSON.parse($('#month-data-container').val());

    for (var i = 0; i < monthData.length; i++) {
        if (monthData[i].Id == id) {
            $('#calendar-detail').find('#agency').html(monthData[i].Agency.Name);
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
    registerDragAndDrop();
}

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

function addHistoryItemCalendar() {

    $.ajax({
        type: "POST",
        url: "/Agency/AddHistoryItem",
        data: { 'agencyId': 0 },
        async: false,
        beforeSend: function () {
            $('#add-history-container').addClass('loading-image');
        }
    }).success(function (partialView) {
        $('#add-history-container').empty().append(partialView);
        $('#add-history-container').find('#divAgencySelect').removeClass('hideMe');
        $('#add-history-container').removeClass('loading-image');
    }).error(function (xhr, textStatus, error) {
        console.log(xhr.statusText);
        console.log(textStatus);
        console.log(error);
    });
}

function loadContacts() {

    $.ajax({
        type: "POST",
        url: "/Agency/AddHistoryItem",
        data: { 'agencyId': 0 },
        async: false
    }).success(function (partialView) {
        $('#add-history-container').empty().append(partialView);
    }).error(function (xhr, textStatus, error) {
        console.log(xhr.statusText);
        console.log(textStatus);
        console.log(error);
    });
}

function agencySelectChanged() {

    var select = $('#add-history-container').find('#Agencies option:selected');
    var agencyId = select.val();
    if (!agencyId > 0)
    { return }

    $.ajax({
        type: "POST",
        url: "/Agency/LoadContactsByAgency",
        data: { 'agencyId': agencyId },
        async: false
    }).success(function (data) {

        $.each(data, function (i) {

            var optionhtml = '<option value="' +
            data[i].ContactDetailId + '">' + data[i].Forename + '</option>';

            var target = $('#add-history-container').find('#ContactDetails').empty().append(optionhtml);
        });

    }).error(function (xhr, textStatus, error) {
        console.log(xhr.statusText);
        console.log(textStatus);
        console.log(error);
    });

}

function registerDragAndDrop() {

    $(".draggable").data({
        'originalLeft': $(".draggable").css('left'),
        'origionalTop': $(".draggable").css('top')
    }).draggable(
        {
            containment: 'document',
            stop: function (event, ui) {
                $(this).css({
                    'left': $(".draggable").data('originalLeft'),
                    'top': $(".draggable").data('origionalTop')
                });
            }
        });

    $(".dropable").droppable({
        hoverClass: 'drop-hover',
        drop: function (event, ui) {
            showModal('cal-new-hist-modal');
            addHistoryItemCalendar();
            $('#cal-new-hist-modal').find('#CreationDate').val(getDateString(new Date($(this).find('#divActualDate').html().toString())));
        }
    });
}

