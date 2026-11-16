const artworkFull = document.getElementById("artwork-full");
const photoFull = document.getElementById("photo-full");

/**
 * Opens the artwork inspector.
 */
function inspectOpen() {
    photoFull.style.display = "none";
    artworkFull.style.display = "flex";
}

/**
 * Closes the artwork inspector.
 */
function inspectClose() {
    artworkFull.style.display = "none";
}

/**
 * Opens the photo inspector.
 */
function photoOpen() {
    artworkFull.style.display = "none";
    photoFull.style.display = "flex";
}

/**
 * Closes the photo inspector.
 * @param {boolean} writeHistory if true, push to clear the history state.
 */
function photoClose(writeHistory) {
    photoFull.style.display = "none";
    if (writeHistory == true) {
        window.history.pushState(null, "", "/Photos");
    }
}

const photos = document.getElementById("photos");
if (photos != null) {
    const photoLinks = photos.getElementsByTagName("a");
    for (i = 0; i < photoLinks.length; i++) {
        photoLinks[i].addEventListener("click", async function (e) {
            let stub = e.currentTarget.dataset.photo;
            if (stub != null && stub.length > 0) {
                e.preventDefault();
                photoLoad(stub, true);
            }
        });
    }

    window.addEventListener('popstate', function(event) {
        let priorPhoto = event.state;
        if (priorPhoto != null && priorPhoto.length > 0) {
            photoLoad(priorPhoto, false);
        } else {
            photoClose(false);
        }
    });
}

/**
 * Loads the specified photo in the inspector.
 * @param {string} stub The URL stub of the photo to load.
 * @param {boolean} writeHistory if true, push the loaded photo to the history state.
 */
async function photoLoad(stub, writeHistory) {
    let request = new Request("/Photos?handler=View&partial=true&stub=" + stub, {
        method: "GET",
        headers: {
            "Accept": "text/html",
        }
    });

    try {
        let response = await fetch(request);

        const photoFull = document.getElementById("photo-full");
        photoFull.innerHTML = await response.text();

        registerPhotoNavigation();

        if (writeHistory == true) {
            window.history.pushState(stub, "", "/Photos/" + stub);
        }

        photoOpen();
    }
    catch (error) {
        console.error("Failed to load gallery view for photo " + stub);
    }
}

/**
 * Re-registers the photo navigation buttons after the DOM change.
 */
function registerPhotoNavigation() {
    const photoPrevious = document.getElementById("photo-previous");
    photoPrevious?.addEventListener("click", async function (e) {
        await photoLoad(photoPrevious.dataset.photo, true);
    });

    const photoNext = document.getElementById("photo-next");
    photoNext?.addEventListener("click", async function (e) {
        await photoLoad(photoNext.dataset.photo, true);
    });
}