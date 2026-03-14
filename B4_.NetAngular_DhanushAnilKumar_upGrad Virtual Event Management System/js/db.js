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