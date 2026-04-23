$("#loginForm").submit(function(e){
     
    e.preventDefault();

    let email = $("#email").val();
    let password = $("#password").val();

    if(email === "admin@upgrad.com" && password === "12345"){

        sessionStorage.setItem("adminLoggedIn", "true");

        window.location.href = "events.html";

    }
    else{

        showToast("Invalid login credentials");

    }

});