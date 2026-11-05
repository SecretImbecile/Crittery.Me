var clearEvents = document.getElementById("clear-events");
clearEvents?.addEventListener("click", function (e) {
    if (window.confirm("Are you sure you want to clear all past events?")) {
        return true;
    } else {
        e.preventDefault();
        return false;
    }
});

var deleteBios = document.getElementsByClassName("delete-bio");
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

var deleteInterests = document.getElementsByClassName("delete-interest");
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

var eventDateCopy = document.getElementById("event-date-copy");
eventDateCopy?.addEventListener("click", function () {
    const dateStart = document.getElementById("FormData_DateStart");
    const dateEnd = document.getElementById("FormData_DateEnd");

    if (dateStart.value.length > 0) {
        dateEnd.value = dateStart.value;
    }
});


document.addEventListener("DOMContentLoaded", async () => {
    const dashboardBio = document.getElementById("dashboard-bio");
    const dashboardCalendar = document.getElementById("dashboard-calendar");
    const dashboardInterests = document.getElementById("dashboard-interests");
    const dashboardRelationships = document.getElementById("dashboard-relationships");    

    if (dashboardCalendar != null) {
        let request = new Request("CMS?handler=Calendar", {
            method: "GET",
            headers: {
                "Accept": "text/html",
            }
        });

        try {
            let response = await fetch(request);
            dashboardCalendar.outerHTML = await response.text();
        }
        catch (error) {
            console.error("Failed to load calendar events");
        }

    }

    if (dashboardBio != null) {
        let request = new Request("CMS?handler=Bio", {
            method: "GET",
            headers: {
                "Accept": "text/html",
            }
        });

        try {
            let response = await fetch(request);
            dashboardBio.outerHTML = await response.text();
        }
        catch (error) {
            console.error("Failed to load bio items");
        }
    }

    if (dashboardInterests != null) {
        let request = new Request("CMS?handler=Interests", {
            method: "GET",
            headers: {
                "Accept": "text/html",
            }
        });

        try {
            let response = await fetch(request);
            dashboardInterests.outerHTML = await response.text();
        }
        catch (error) {
            console.error("Failed to load interest items");
        }
    }

    if (dashboardRelationships != null) {
        let request = new Request("CMS?handler=Relationships", {
            method: "GET",
            headers: {
                "Accept": "text/html",
            }
        });

        try {
            let response = await fetch(request);
            dashboardRelationships.outerHTML = await response.text();
        }
        catch (error) {
            console.error("Failed to load relationship items");
        }
    }
});