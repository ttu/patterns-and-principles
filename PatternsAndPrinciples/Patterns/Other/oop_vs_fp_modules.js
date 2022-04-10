/** 
 * "Modern" OO avoids inheritance and objects end up looking like functions/modules, 
 * where constructors are partial application.
*/

// connection is passed as parameter
const send = (connection, data) => {
    data.env = "send";
    connection.post(data);
};

// Partail application of send function
// Partial application are language feature of functional languages, but in e.g.
// JavaScript, they must be implemented as functions
const createPartialApplication = (connection) => (data) => send(connection, data);

// createService is a function factory
// connection argument is captured to returned object
// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Closures
const createService = (connection) => ({
  send: (data) => {
    data.env = "createService";
    connection.post(data);
  },
});

// Service class with send function
// connection is passed to service on constructor
class Service {
  constructor(connection) {
    this.connection = connection;
  }

  send(data) {
    data.env = "Service";
    this.connection.post(data);
  }
}

// Sample connection, will print "sent" data
const myAPIConnection = {
  post: (data) => console.log(data),
};

const data = { payload: "info" };

send(myAPIConnection, data);

const sendPartial = createPartialApplication(myAPIConnection);
sendPartial(data);

const service = createService(myAPIConnection);
service.send(data);

const s = new Service(myAPIConnection);
s.send(data);