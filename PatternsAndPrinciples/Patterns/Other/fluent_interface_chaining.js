/**
 * Flunet interface / Method chaining
 */

class Chainable {
  constructor(value) {
    this.value = value;
  }

  add(i) {
    this.value += i;
    return this; // return itself
  }

  double() {
    this.value = this.value * 2;
    return new Chainable(this.value); // return new instance
  }
}

const c = new Chainable(2);
const result = c.add(2).double().value;

console.log({ result });

/** MAP */

Array.prototype.betterMap = (callback) => {
  const newArray = new Array();
  // Do something
  return newArray;
};

const items = [2,3,4,5];
const mapResult = items.map((x) => x * 2).betterMap((x) => x);
console.log({ mapResult });