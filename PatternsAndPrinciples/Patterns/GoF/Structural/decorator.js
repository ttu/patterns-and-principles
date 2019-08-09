// Create own function that combines all functions to single function
const combine = (...funcs) => (...params) => funcs.forEach(f => f(...params));

// With reduce we can pass prev functions return value to next if needed
//const combine = (...funcs) => (...params) => funcs.reduce((prev, curr, idx) => curr(...params), 0);


const printDB = (a, b) => console.log(`DB: ${a} - ${b}`);
const printAudit = (a, b) => console.log(`Audit: ${a} - ${b}`);
const printBus = (a, b) => console.log(`Bus: ${a} - ${b}`);

const decorated = combine(printDB, printAudit, printBus);

decorated(34, 'Street 10 A');