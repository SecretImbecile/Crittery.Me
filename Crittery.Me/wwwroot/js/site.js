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