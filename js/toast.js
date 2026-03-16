function showToast(message){
    
    let toastMessage = document.getElementById("toastMessage");
    toastMessage.innerText = message;

    let toastElement = document.getElementById("appToast");

    let toast = new bootstrap.Toast(toastElement,{
        delay:2000
    });

    toast.show();
}