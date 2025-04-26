function handleDeleteRequest(api) {
    if (!confirm("Are you sure you want to delete this product?")) return;

    fetch(api, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(res => res.json())
    .then(data => {
        if (data.success) {
            sessionStorage.setItem("successMessage",data.message);
            window.location.href = data.redirectUrl;
        } 
        else {
            showAlert(data.message || "Delete failed!","danger");
        }
    })
    .catch(error => {
        console.error("Error:", error);
        showAlert("Something went wrong!","danger");
    });
}

function deleteAll(api){
    if (!confirm("Are you sure you want to delete all products?")) return;

    fetch(api, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(res => res.json())
    .then(data => {
        if (data.success) {
            sessionStorage.setItem("successMessage",data.message);
            window.location.href = data.redirectUrl;
        } 
        else {
            showAlert(data.message || "Delete failed!","danger");
        }
    })
    .catch(error => {
        console.error("Error:", error);
        showAlert("Something went wrong!","danger");
    });
}

