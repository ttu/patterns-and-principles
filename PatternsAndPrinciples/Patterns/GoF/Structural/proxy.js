/*
 * Proxy with functions
 * Check also JavaScript Proxy object
 * https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Proxy
 */

const fetch = require("node-fetch");

const URL = 'http://dummy-sensors.azurewebsites.net/api/sensor/iddqd';

const cache = {};

const getResponseJson = async url => {
  const result = await fetch(url);
  console.log(`Fetch status:${result.status}`);
  return result.json();
};

const cacheProxyWrapper = async (url, func) => {
  if (!cache[url]) {
    cache[url] = await func(url);
  }
  return cache[url];
};

const getCached = async url => await cacheProxyWrapper(url, getResponseJson);

const getData = async (url, getFunc) => {
  const data = await getFunc(url);
  console.log(data);
};

(async () => {
  await getData(URL, getCached);
  await getData(URL, getCached);
  await getData(URL, getResponseJson);
})();
