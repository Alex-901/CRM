var tabNames = { 1: 'divAgencyMain', 2: 'divAgencyContacts', 3: 'divHistory' }

$(document).ready(function () {
    var currentTab = $('#hdnCurrentTab').val();

    changeAgencyTab(currentTab);
});

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

function removeAgency() {
    var clientId = $('#hdnId').val();

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

    $('#hdnCurrentTab').val(tabToShow);
}

function saveAgencyContact() {

    var data = {
        'Forename': $('#NewContact_Forename').val(),
        'Surname': $('#NewContact_Surname').val(),
        'Mobile': $('#NewContact_Mobile').val(),
        'Email': $('#NewContact_Email').val(),
        'ContactDetailId': $('#ContactDetailId').val(),
        'EntityId': $('#Id').val()
    };

    $.ajax({
        type: "POST",
        url: "/Agency/NewContact",
        contentType: 'application/json',
        data: JSON.stringify(data)
    }).success(function (partialView) {
        $('#divAgencyContacts').append(partialView);
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
        url: "/Agency/LoadContacts",
        data: { 'contactId': contactId, 'agencyId': agencyId }
    }).success(function (partialView) {
        $('#contactCont').empty();
        $('#contactCont').append(partialView);
        showModal('NewContact');
    }).error(function (xhr, textStatus, error) {
        console.log(xhr.statusText);
        console.log(textStatus);
        console.log(error);
    });
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