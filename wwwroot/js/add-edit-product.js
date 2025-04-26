const form = document.getElementById("product-form");

function handleRequest(api,id){

const isAddMode = id === null;

form.addEventListener("submit", function (event) {
    event.preventDefault();

    const productName = document.getElementById("product-name").value.trim().toLowerCase();
    const category = document.getElementById("category").value.trim().toLowerCase();
    const price = document.getElementById("price").value.trim();
    const quantity = document.getElementById("quantity").value.trim();
    const description = document.getElementById("description").value.trim();


    if(isOnlyNumbers(productName)){
    	showAlert("Product Name is not valid!","danger");
    	return;
    }

    if(isOnlyNumbers(description)){
    	showAlert("Description is not valid!","danger");
    	return;
    }

    const productData = {
        productName: productName,
        category: category,
        price: parseFloat(price),
        quantity: parseInt(quantity),
        description: description
    };

    const finalUrl = isAddMode ? api : `${api}/${id}`;

    fetch(finalUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(productData)
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            sessionStorage.setItem("successMessage",data.message);
            window.location.href = data.redirectUrl;
        } else {
            showAlert(data.message, "danger");
        }
    })
    .catch(error => {
        console.error("Error:", error);
        showAlert("Something went wrong.", "danger");
    });
});
}


window.onload = () => {
	form.reset();
};

function isOnlyNumbers(str) {
    return /^[0-9]+$/.test(str);
}

function cancelOperation(url){
	window.location.href = url;
}
