const artworkFull = document.getElementById("artwork-full");
const photoFull = document.getElementById("photo-full");

/**
 * Opens the artwork inspector
 */
function inspectOpen() {
    photoFull.style.display = "none";
    artworkFull.style.display = "flex";
}

/**
 * Closes the artwork inspector
 */
function inspectClose() {
    artworkFull.style.display = "none";
}

/**
 * Opens the photo inspector
 */
function photoOpen() {
    artworkFull.style.display = "none";
    photoFull.style.display = "flex";
}

/**
 * Closes the photo inspector
 */
function photoClose() {
    photoFull.style.display = "none";
}

const photos = document.getElementById("photos");
if (photos != null) {
    const photoLinks = photos.getElementsByTagName("a");
    for (i = 0; i < photoLinks.length; i++) {
        photoLinks[i].addEventListener("click", async function (e) {
            let stub = e.currentTarget.dataset.photo;
            if (stub != null && stub.length > 0) {
                // Make an ajax call instead of reloading the full page
                e.preventDefault();

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

                    window.history.replaceState(stub, "", "/Photos/" + stub);

                    photoOpen();
                }
                catch (error) {
                    console.error("Failed to load gallery view for photo " + stub);
                }
            }
        });
    }
}