var tabNames = { 1: 'divAgencyMain', 2: 'divAgencyContacts', 3: 'divHistory' }


$(document).ready(function () {
    initDeleteDialog();
});


function initDeleteDialog() {

    //Attach function to element we want to show a confirmation for
    $('.delete-link').click(function (e) {
        //Take the onclick event from the sender and attach it to the confim button 
        $('.delete-link-confirm').attr("onclick", $(this).attr("onclick"));
        $('#delete-dialog').show();
        //Stop the sender from executing
        e.stopImmediatePropagation();
    });

    //Make the confirm button hide the confirmation
    $('.delete-link-confirm').click(function () {
        $('#delete-dialog').hide();
    });

    //Remove and then readd onclick events so the jQuery event gets executed first
    //This ensures the pop up is shown before the event is triggered
    $('.delete-link').each(function () {
        var handler = $(this).prop('onclick');
        $(this).removeProp('onclick');
        $(this).click(handler);
    });
}

function removeAgency(clientId) {

    $.ajax({
        type: "GET",
        url: "/Agency/Delete",
        data: { id: clientId }
    }).done(function () {
        hideModal('deleteConf');
        window.location.reload(true);
    }).error(function () {
        alert(Error.toString());
    });
}

function changeAgencyTab(tabToShow) {
    $('#divAgencyTabCont').children('div').addClass('hideMe');

    $('#' + tabNames[tabToShow]).removeClass('hideMe');

    if (tabToShow == 2) {
        initContactTab();
    }

    if (tabToShow == 3) {
        initHistoryItemTab();
    }
}

function initContactTab() {
    loadAllContacts($('#Id').val());
}

function initHistoryItemTab() {
    loadHistoryItems();
    addHistoryItem();
}

function saveAgencyContact() {

    var data = {
        'Forename': $('#NewContact').find('#Forename').val(),
        'Surname': $('#NewContact').find('#Surname').val(),
        'Mobile': $('#NewContact').find('#Mobile').val(),
        'Email': $('#NewContact').find('#Email').val(),
        'ContactDetailId': $('#NewContact').find('#ContactDetailId').val(),
        'EntityId': $('#Id').val()
    };

    $.ajax({
        type: "POST",
        url: "/Agency/SaveContact",
        contentType: 'application/json',
        data: JSON.stringify(data)
    }).success(function (partialView) {
        $('#agency-contact-container').empty().append(partialView);
        hideModal('NewContact');
    }).error(function (xhr, textStatus, error) {
        console.log(xhr.statusText);
        console.log(textStatus);
        console.log(error);
    });
}

function loadContactDetail(contactId, agencyId) {

    $.ajax({
        type: "POST",
        url: "/Agency/LoadContact",
        data: { 'contactId': contactId, 'agencyId': agencyId }
    }).success(function (partialView) {
        $('#contact-cont').empty().append(partialView);
        showModal('NewContact');
    }).error(function (xhr, textStatus, error) {
        console.log(xhr.statusText);
        console.log(textStatus);
        console.log(error);
    });
}

function loadAllContacts(agencyId) {

    $.ajax({
        type: "POST",
        url: "/Agency/LoadContacts",
        data: { 'agencyId': agencyId },
        beforeSend: function () {
            $('#agency-contact-container').addClass('loading-image');
        }
    }).success(function (partialView) {
        $('#agency-contact-container').empty().append(partialView);
        initDeleteDialog();
        $('#agency-contact-container').removeClass('loading-image');
    }).error(function (xhr, textStatus, error) {
        console.log(xhr.statusText);
        console.log(textStatus);
        console.log(error);
        $('#agency-contact-container').removeClass('loading-image');
    });
}

function deleteContact(contactId, agencyId) {
    $.ajax({
        type: "POST",
        url: "/Agency/DeleteContact",
        data: { 'contactId': contactId, 'agencyId': agencyId }
    }).success(function (partialView) {
        $('#agency-contact-container').empty().append(partialView);
        initDeleteDialog();
    }).error(function (xhr, textStatus, error) {
        console.log(xhr.statusText);
        console.log(textStatus);
        console.log(error);
    });

    event.stopPropagation();
}

function showHideAccordianItem(sender) {

    var nextDiv = $(sender).next('div');

    if (nextDiv.is(':hidden')) {
        nextDiv.fadeIn();
    }
    else {
        nextDiv.fadeOut()
    }
}

function showModal(id, itemId, model) {
    if (itemId > 0) {
        $('#hdnId').val(itemId);
    }

    if (model != null) {

        for (var prop in model) {

            var control = $('#' + id).find('#' + prop);

            if (control != null) {
                control.val(model[prop]);
            }
        }
    }

    $('#' + id).fadeIn();
}

function hideModal(id) {
    $('#' + id).fadeOut();
}

function loadHistoryItems() {

    $.ajax({
        type: "POST",
        url: "/Agency/LoadHistoryItems",
        data: { 'agencyId': $('#Id').val() },
        beforeSend: function () {
            $('#history-container').addClass('loading-image');
        }
    }).success(function (partialView) {
        $('#history-container').empty().append(partialView);
        $('#history-container').removeClass('loading-image');
    }).error(function (xhr, textStatus, error) {
        console.log(xhr.statusText);
        console.log(textStatus);
        console.log(error);
        $('#history-container').removeClass('loading-image');
    });
}

function addHistoryItem() {

    $.ajax({
        type: "POST",
        url: "/Agency/AddHistoryItem",
        data: { 'agencyId': $('#Id').val() }
    }).success(function (partialView) {
        $('#add-history-container').empty().append(partialView);
    }).error(function (xhr, textStatus, error) {
        console.log(xhr.statusText);
        console.log(textStatus);
        console.log(error);
    });
}

function saveHistoryItem() {

    var contact = {
        'ContactDetailId': $('#add-history-container').find('#ContactDetails option:selected').val()
    };

    var data = {
        'Notes': $('#add-history-container').find('#Notes').val(),
        'contact': contact,
        'ParentId': $('#Id').val()
    };

    $.ajax({
        type: "POST",
        url: "/Agency/SaveHistoryItem",
        contentType: 'application/json',
        data: JSON.stringify(data)
    }).success(function (partialView) {
        $('#history-container').empty().append(partialView);
        hideModal('modal-cont');
    }).error(function (xhr, textStatus, error) {
        console.log(xhr.statusText);
        console.log(textStatus);
        console.log(error);
    });
}
