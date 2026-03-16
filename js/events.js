let deleteEventId = null;

if(!sessionStorage.getItem("adminLoggedIn")){

    showToast("Please login first");

    window.location.href = "login.html";
}


$("#eventForm").submit(function(e){

    e.preventDefault();

    let event = {

        id: $("#eventId").val(),
        name: $("#eventName").val(),
        category: $("#eventCategory").val(),
        date: $("#eventDate").val(),
        time: $("#eventTime").val(),
        url: $("#eventURL").val()
    };

    addEvent(event);

    $("#eventForm")[0].reset();

    loadEvents();

});
    
function loadEvents(){

    console.log("Loading events...");

    $("#eventsContainer").html("");

    getAllEvents(function(events){

        displayEvents(events);

    });

}

function removeEvent(id){

    deleteEventId = id;

    let modal = new bootstrap.Modal(document.getElementById("deleteModal"));
    modal.show();

}

$(document).ready(function(){

    loadEvents();

});

function logout(){

    sessionStorage.removeItem("adminLoggedIn");

    window.location.href = "login.html";

}

$("#searchInput").on("keyup", function(){

    let keyword = $(this).val().toLowerCase();

    getAllEvents(function(events){

        let filtered = events.filter(event =>

            event.id.toLowerCase().includes(keyword) ||
            event.name.toLowerCase().includes(keyword) ||
            event.category.toLowerCase().includes(keyword)

        );

        displayEvents(filtered);

    });

});

function displayEvents(events){

    $("#eventsContainer").html("");

    events.forEach(function(event){

        let card = `
            <div class="col-md-4 mb-3">
                <div class="card p-3">
                    <h5>${event.name}</h5>
                    <p>Category: ${event.category}</p>
                    <p>Date: ${event.date}</p>
                    <p>Time: ${event.time}</p>

                    <a href="${event.url}" target="_blank" class="btn btn-success mb-2">
                    Join Event
                    </a>
                    
                    <button class="btn btn-danger" onclick="removeEvent('${event.id}')">
                    Delete
                    </button>

                </div>

            </div>
            `;
        $("#eventsContainer").append(card);

    });
}

function triggerSearch(){
    $("#searchInput").trigger("keyup");
}

document.getElementById("confirmDeleteBtn").addEventListener("click",function(){

    if(deleteEventId){

        deleteEventId(deleteEventId);

        loadEvents();

        showToast("Event deleted successfully");

        deleteEventId = null;

    }

    let modalElement = document.getElementById("deleteModal");
    let modal = bootstrap.Modal.getInstance(modalElement);
    modal.hide();

});