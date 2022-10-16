import json
from typing import Callable
import urllib.request


"""
Encapsulation & Abstraction: Hiding details

Encapsulation
 - Hiding the internal details or mechanics of how an object does something.
Abstraction
  - Focus on what the object does / external shape, not how it does it.

> Abstraction can be achieved by using encapsulation.
> Encapsulating behavior by using abstractions.
"""


class ExampleA:
    def execute_logic(self, id):
        url = 'https://dummy-sensors.azurewebsites.net/api/sensor'
        sensor_response = urllib.request.urlopen(f'{url}/{id}')
        payload = sensor_response.read()
        sensor =  json.loads(payload.decode('utf-8'))
        print(f'{sensor["id"]}: {sensor["data"]}')


ExampleA().execute_logic('iddqd')

class SensorStore:  # <-- abstaction
    URL = 'https://dummy-sensors.azurewebsites.net/api/sensor'  # <-- encalpsulation   
    def get_sensor(self, id):  # <-- abstaction
        sensor_response = urllib.request.urlopen(f'{self.URL}/{id}')  # <-- encalpsulation
        payload = sensor_response.read()
        return json.loads(payload.decode('utf-8'))


class ExampleB:
    def execute_logic(self, id):
        sensor = SensorStore().get_sensor(id)  # <-- using abstaction
        print(f'{sensor["id"]}: {sensor["data"]}')


ExampleB().execute_logic('idkfa')


class SensorStoreDummy:  
    def get_sensor(self, id):  
        dummy = { 'id': f'dummy-sensor-{id}', 'data': 40 }
        return dummy


class ExampleC:
    def __init__(self, store):
        self.store = store # <-- using abstaction

    def execute_logic(self, id):
        sensor = self.store.get_sensor(id)  # <-- doesn't matter where sensor data is coming from
        print(f'{sensor["id"]}: {sensor["data"]}')


ExampleC(SensorStore()).execute_logic('acdc1')
ExampleC(SensorStoreDummy()).execute_logic('doom')


# How to encapsulate without classes?
# Functions encapsulate also
# Pass functions as parameters and use wrapper functions (composition)

def execute_logic_A(id: str, get_sensor_func: Callable[[str], dict]):
    sensor = get_sensor_func(id) 
    print(f'{sensor["id"]}: {sensor["data"]}')


def get_sensor(id: str) -> dict:  
    URL = 'https://dummy-sensors.azurewebsites.net/api/sensor'    
    sensor_response = urllib.request.urlopen(f'{URL}/{id}')
    payload = sensor_response.read()
    return json.loads(payload.decode('utf-8'))


def get_sensor_dummy(id: str) -> dict:  
    dummy = { 'id': f'dummy-sensor-{id}', 'data': 40 }
    return dummy


execute_logic_A('iddqd', get_sensor)

execute_logic_func = lambda x: execute_logic_A(x, get_sensor)
# Above is same as: ExampleC(SensorStore())
# execute_logic_func = lambda x: execute_logic_A(x, get_sensor_dummy)
# Above is same as: ExampleC(SensorStoreDummy())

execute_logic_func('acdc1')