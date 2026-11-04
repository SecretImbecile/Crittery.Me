var clearEvents = document.getElementById("clear-events");
var deleteBios = document.getElementsByClassName("delete-bio");
var deleteInterests = document.getElementsByClassName("delete-interest");
var eventDateCopy = document.getElementById("event-date-copy");

clearEvents?.addEventListener("click", function (e) {
    if (window.confirm("Are you sure you want to clear all past events?")) {
        return true;
    } else {
        e.preventDefault();
        return false;
    }
});

for (i = 0; i < deleteBios.length; i++) {
    deleteBios[i].addEventListener("click", function (e) {
        if (window.confirm("Are you sure you want to delete this bio item?")) {
            return true;
        } else {
            e.preventDefault();
            return false;
        }
    });
}    

for (i = 0; i < deleteInterests.length; i++) {
    deleteInterests[i].addEventListener("click", function (e) {
        if (window.confirm("Are you sure you want to delete this interest?")) {
            return true;
        } else {
            e.preventDefault();
            return false;
        }
    });
}

eventDateCopy?.addEventListener("click", function () {
    const dateStart = document.getElementById("FormData_DateStart");
    const dateEnd = document.getElementById("FormData_DateEnd");

    if (dateStart.value.length > 0) {
        dateEnd.value = dateStart.value;
    }
});