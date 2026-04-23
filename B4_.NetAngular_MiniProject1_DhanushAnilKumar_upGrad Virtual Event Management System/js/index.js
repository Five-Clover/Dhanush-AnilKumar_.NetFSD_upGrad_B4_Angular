function loadHomeEvents(){

    console.log("Loading home events");
    
    $("#eventContainer").html("");

    getAllEvents(function(events){

        events.forEach(function(event){

            let card = `
            <div class="col-md-4 mb-3">
            
                <div class="card p-3">
                    <h5>${event.name}</h5>
                    
                    <p><strong>Category:</strong> ${event.category}</p>
                    <p><strong>Date:</strong> ${event.date}</p>
                    <p><strong>Time:</strong> ${event.time}</p>
                    
                    <a href="register.html?id=${event.id}" class="btn btn-primary">
                    Register
                    </a>
                
                </div>
                
            </div>`;

            $("#eventContainer").append(card);

        });

    });
}