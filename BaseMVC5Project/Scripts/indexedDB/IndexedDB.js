let offlineDB;

function openDatabase() {
    const customerData = [
        { ssn: "444-44-4444", name: "Bill", age: 35, email: "bill@company.com" },
        { ssn: "555-55-5555", name: "Donna", age: 32, email: "donna@home.org" }
    ];


    const dbName = "my_offline_db";
    // Let us open our database
    // this return a IDBOpenDBRequest
    const request = window.indexedDB.open(dbName, 3);

    request.onerror = (event) => {
        // Do something with request.errorCode!
        console.log(event);
    };
    request.onsuccess = (event) => {
        // Do something with request.result!
        console.log(event);
        offlineDB = event.target.result;
    };

    request.onupgradeneeded = (event) => {
        const db = event.target.result;

        // Create an objectStore to hold information about our customers. We're
        // going to use "ssn" as our key path because it's guaranteed to be
        // unique - or at least that's what I was told during the kickoff meeting.
        const objectStore = db.createObjectStore("customers", { keyPath: "ssn" });

        // Create an index to search customers by name. We may have duplicates
        // so we can't use a unique index.
        objectStore.createIndex("name", "name", { unique: false });

        // Create an index to search customers by email. We want to ensure that
        // no two customers have the same email, so use a unique index.
        objectStore.createIndex("email", "email", { unique: true });

        // Use transaction oncomplete to make sure the objectStore creation is
        // finished before adding data into it.
        objectStore.transaction.oncomplete = (event) => {
            // Store values in the newly created objectStore.
            const customerObjectStore = db.transaction("customers", "readwrite").objectStore("customers");
            customerData.forEach((customer) => {
                customerObjectStore.add(customer);
            });
        };
    };

}

function saveToLocalStorage() {

}


