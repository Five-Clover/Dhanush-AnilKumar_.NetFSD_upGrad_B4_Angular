let db;

let request = indexedDB.open("EventDB", 1);

request.onupgradeneeded = function (event) {

    db = event.target.result;

    if (!db.objectStoreNames.contains("events")) {

        let store = db.createObjectStore("events", { keyPath: "id" });

        store.createIndex("name", "name", { unique: false });
        store.createIndex("category", "category", { unique: false });

    }
};

request.onsuccess = function (event) {
    
    db = event.target.result;

    addDefaultEvents();

    console.log("Database ready");

    if(typeof loadHomeEvents === "function"){
        loadHomeEvents();
    }

    if(typeof loadEvents === "function"){
        loadEvents();
    }

    if(typeof loadEventDetails === "function"){
        loadEventDetails();
    }
};

request.onerror = function () {
    console.log("Database failed to open");
};

function addEvent(eventData) {

    let transaction = db.transaction(["events"], "readwrite");

    let store = transaction.objectStore("events");

    let request = store.add(eventData);

    request.onsuccess = function(){

        showToast("Event added successfully!");

    };

    request.onerror = function(){

        showToast("Event ID already exists! Please use a different ID.");
    
    };

}

function getAllEvents(callback) {

    let transaction = db.transaction(["events"], "readonly");

    let store = transaction.objectStore("events");

    let request = store.getAll();

    request.onsuccess = function () {

        callback(request.result);

    };

}

function deleteEvent(id) {

    let transaction = db.transaction(["events"], "readwrite");

    let store = transaction.objectStore("events");

    store.delete(id);

}

function searchEvents(keyword, callback) {

    getAllEvents(function(events){

        let results = events.filter(event =>

            event.id.includes(keyword) ||
            event.name.toLowerCase().includes(keyword.toLowerCase()) ||
            event.category.toLowerCase().includes(keyword.toLowerCase())

        );

        callback(results);

    });

}

function addDefaultEvents() {

    getAllEvents(function(events){

        if(events.length === 0){

            let defaultEvents = [

                {
                   id:"101",
                   name:"Dev Tech",
                   category:"Tech & Innovations",
                   date:"2026-03-04",
                   time:"15:15",
                   url:"https://example.com"
                },

                {
                   id:"102",
                   name:"AI Summit",
                   category:"Artificial Intelligence",
                   date:"2026-03-05",
                   time:"14:45",
                   url:"https://example.com"
                },

                {
                   id:"103",
                   name:"Client Summit",
                   category:"Industrial Event",
                   date:"2026-03-17",
                   time:"15:00",
                   url:"https://example.com"
                },
            ];

            defaultEvents.forEach(event => addEvent(event));

            if(typeof loadHomeEvents === "function"){
                loadHomeEvents();
            }

            if(typeof loadEvents === "function"){
                loadEvents();
            }


        }

    });

}