function showAlert(message, type = "success") {
    const alertBox = document.getElementById("alert-message");
    alertBox.className = `alert alert-${type}`;
    alertBox.innerHTML = message;
    alertBox.classList.remove("d-none");
    setTimeout(function(){
    	alertBox.style.display = "none";
    },2000);
    alertBox.style.display = "block";
}

function displaySuccessMessage(){
    const message = sessionStorage.getItem("successMessage");

    if (message) {
        showAlert(message, "success");
        sessionStorage.removeItem("successMessage");
    }
}