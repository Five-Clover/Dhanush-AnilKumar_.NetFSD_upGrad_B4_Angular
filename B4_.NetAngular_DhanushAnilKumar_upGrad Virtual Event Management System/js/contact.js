$("#contactForm").submit(function(e){

    e.preventDefault();

    showToast("Your query has been submitted successfully!");

    $("#contactForm")[0].reset();

});