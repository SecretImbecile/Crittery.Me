var deleteBios = document.getElementsByClassName("delete-bio");
var deleteInterests = document.getElementsByClassName("delete-interest");

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