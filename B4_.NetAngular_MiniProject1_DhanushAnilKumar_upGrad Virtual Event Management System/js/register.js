function getEventId(){

    let params = new URLSearchParams(window.location.search);

    return params.get("id");
}

function loadEventDetails(){

    let id = getEventId();

    getAllEvents(function(events){

        let event = events.find(e => e.id == id);

        if(event){

            $("#eventId").text(event.id);
            $("#eventName").text(event.name);
            $("#eventCategory").text(event.category);
            $("#eventDate").text(event.date);
            $("#eventTime").text(event.time);

        }

    });

}

$("#registrationForm").submit(function(e){

    e.preventDefault();

    showToast("Successfully registered for the event!");

    $("#registrationForm")[0].reset();

});
