const decorate = (...funcs) => (...params) => funcs.forEach(f => f(...params));

const printDB = (a, b) => console.log(`DB: ${a} - ${b}`);
const printAudit = (a, b) => console.log(`Audit: ${a} - ${b}`);
const printBus = (a, b) => console.log(`Bus: ${a} - ${b}`);

const saveToDb = decorate(printDB, printAudit, printBus);

saveToDb(34, 'Street 10 A');