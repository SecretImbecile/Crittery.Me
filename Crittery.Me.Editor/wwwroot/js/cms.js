const clearEvents = document.getElementById("clear-events");
clearEvents?.addEventListener("click", function (e) {
    if (window.confirm("Are you sure you want to clear all past events?")) {
        return true;
    } else {
        e.preventDefault();
        return false;
    }
});

const deleteBios = document.getElementsByClassName("delete-bio");
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

const deleteInterests = document.getElementsByClassName("delete-interest");
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

const eventDateCopy = document.getElementById("event-date-copy");
eventDateCopy?.addEventListener("click", function () {
    const dateStart = document.getElementById("FormData_DateStart");
    const dateEnd = document.getElementById("FormData_DateEnd");

    if (dateStart.value.length > 0) {
        dateEnd.value = dateStart.value;
    }
});

const iconSourcePng = document.getElementById("FormData_Icon_Files_0__Source");
iconSourcePng?.addEventListener("change", function () {
    const imageSourcePng = document.getElementById("preview-icon-png");
    imageSourcePng.src = iconSourcePng.value;
});

const iconSourceWebp = document.getElementById("FormData_Icon_Files_1__Source");
iconSourceWebp?.addEventListener("change", function () {
    const imageSourceWebp = document.getElementById("preview-icon-webp");
    imageSourceWebp.src = iconSourceWebp.value;
});


document.addEventListener("DOMContentLoaded", async () => {
    const dashboardBio = document.getElementById("dashboard-bio");
    const dashboardCalendar = document.getElementById("dashboard-calendar");
    const dashboardInterests = document.getElementById("dashboard-interests");
    const dashboardLinks = document.getElementById("dashboard-links");
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

    if (dashboardLinks != null) {
        let request = new Request("CMS?handler=Links", {
            method: "GET",
            headers: {
                "Accept": "text/html",
            }
        });

        try {
            let response = await fetch(request);
            dashboardLinks.outerHTML = await response.text();
        }
        catch (error) {
            console.error("Failed to load links");
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

const photoSourceJpeg = document.getElementById("FormData_ImageFullSize_Files_0__Source");
photoSourceJpeg?.addEventListener("change", async function () {
    const photoPreviewJpeg = document.getElementById("preview-fullsize-jpeg");
    photoPreviewJpeg.src = photoSourceJpeg.value;

    let fileUrl = photoSourceJpeg.value;
    fileUrl = encodeURIComponent(fileUrl);
    if (fileUrl.length > 0) {
        let request = new Request("?handler=Metadata&src=" + fileUrl, {
            method: "GET",
            headers: {
                "Accept": "text/html",
            }
        });

        try {
            let response = await fetch(request);

            if (response.status == 200) {
                let html = await response.text();
                document.getElementById("photo-metadata").innerHTML = html;

                const buttonPhotoAutofill = document.getElementById("button-photo-autofill");
                buttonPhotoAutofill?.addEventListener("click", function () {
                    photoAutoFill(buttonPhotoAutofill);
                });
            } else {
                console.log("No metadata for '" + fileUrl + "'");
            }
        }
        catch (error) {
            console.error("Failed to fetch image metadata");
        }
    }
});

const photoSourceWebp = document.getElementById("FormData_ImageFullSize_Files_1__Source");
photoSourceWebp?.addEventListener("change", function () {
    const photoPreviewWebp = document.getElementById("preview-fullsize-webp");
    photoPreviewWebp.src = photoSourceWebp.value;
});

const thumbnailSourceJpeg = document.getElementById("FormData_ImageThumbnail_Files_0__Source");
thumbnailSourceJpeg?.addEventListener("change", function () {
    const thumbnailPreviewJpeg = document.getElementById("preview-thumbnail-jpeg");
    thumbnailPreviewJpeg.src = thumbnailSourceJpeg.value;
});

const thumbnailSourceWebp = document.getElementById("FormData_ImageThumbnail_Files_1__Source");
thumbnailSourceWebp?.addEventListener("change", function () {
    const thumbnailPreviewWebp = document.getElementById("preview-thumbnail-webp");
    thumbnailPreviewWebp.src = thumbnailSourceWebp.value;
});

const buttonExposureFraction = document.getElementById("button-exposure-fraction");
buttonExposureFraction?.addEventListener("click", function () {
    const exposureTime = document.getElementById("FormData_Metadata_ExposureTime");
    let exposure = exposureTime.value;
    if (exposure != null && exposure > 0.0) {
        exposureTime.value = 1 / exposure;
    }
});

/**
 * Button click action for the photo metadata autofill.
 * @param {HTMLButtonElement} button The button that triggered the event.
 */
function photoAutoFill(button) {
    document.getElementById("FormData_CapturedOn").value = button.dataset.datetime;
    document.getElementById("FormData_Metadata_ImageDescription").value = button.dataset.description;
    document.getElementById("FormData_Metadata_Copyright").value = button.dataset.copyright;
    document.getElementById("FormData_Metadata_CopyrightUrl").value = button.dataset.copyrighturl;
    document.getElementById("FormData_Metadata_CameraModel").value = button.dataset.cameramodel;
    document.getElementById("FormData_Metadata_LensModel").value = button.dataset.lensmodel;
    document.getElementById("FormData_Metadata_FocalLength").value = button.dataset.focallength;
    document.getElementById("FormData_Metadata_ApertureValue").value = button.dataset.aperture;
    document.getElementById("FormData_Metadata_ExposureTime").value = button.dataset.exposure;
    document.getElementById("FormData_Metadata_IsoSpeedRating").value = button.dataset.iso;

    const flashYes = document.getElementById("FlashYes");
    const flashNo = document.getElementById("FlashNo");
    const flashUnknown = document.getElementById("FlashUnknown");

    let flash = button.dataset.flash;
    if (flash == "true") {
        flashYes.checked = true;
        flashNo.checked = false;
        flashUnknown.checked = false;
    } else if (flash == "false") {
        flashYes.checked = false;
        flashNo.checked = true;
        flashUnknown.checked = false;
    } else {
        flashYes.checked = false;
        flashNo.checked = false;
        flashUnknown.checked = true;
    }
}