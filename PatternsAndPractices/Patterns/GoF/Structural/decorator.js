// Create own function that combines all functions to single function
const combine = (...funcs) => (...params) => funcs.forEach(f => f(...params));

const printDB = (a, b) => console.log(`DB: ${a} - ${b}`);
const printAudit = (a, b) => console.log(`Audit: ${a} - ${b}`);
const printBus = (a, b) => console.log(`Bus: ${a} - ${b}`);

const decorated = combine(printDB, printAudit, printBus);

decorated(34, 'Street 10 A');