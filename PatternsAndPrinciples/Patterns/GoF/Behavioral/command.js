var fetch = amount => console.log(`Fetch ${amount} users`);

const fetchHundred = () => fetch(100);
const fetchSome = () => fetch(12345);

fetchHundred();

class Device {
  turnOn() {
    console.log("on");
  }
  turnOff() {
    console.log("off");
  }
}

class DeviceBinary {
  turnOn() {
    console.log("1");
  }
  turnOff() {
    console.log("0");
  }
}

class NotGood {

}

const dev = new Device();
const devBin = new DeviceBinary();

const turnOnCommand = () => dev.turnOn();
const turnOffCommand = () => dev.turnOff();

const callTurnOnCommand = device => device.turnOn();
const myTurnOn = () => callTurnOnCommand(dev);
const myTurnOn2 = () => callTurnOnCommand(devBin);

const commands = [turnOnCommand, turnOffCommand, myTurnOn, myTurnOn2];

for (const cmd of commands) cmd();
