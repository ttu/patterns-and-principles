import json
import urllib.request

"""
Inversion of Control (IoC)

In normal control sequence 
  * the object/function finds the objects it depends on by itself 
  * then calls them

With IoC, this is reversed (inverted)
  * The dependencies are handed to the object when it's created
  * Or functioncality to handle is passed to the function 
"""


class SensorStore: 
    URL = 'https://dummy-sensors.azurewebsites.net/api/sensor' 
    def get_sensor(self, id): 
        sensor_response = urllib.request.urlopen(f'{self.URL}/{id}') # <-- Not Inversion of control
        # SensorStore
        payload = sensor_response.read()
        return json.loads(payload.decode('utf-8'))


class Example:
    def __init__(self, store):
        self.store = store  # <-- Inversion of control / Dependency injenction

    def execute_logic(self, id, handle_data):
        sensor = self.store.get_sensor(id)
        if sensor:
            handle_data(sensor)  # <-- Inversion of control / callback
            # Using e.g. event bus would also be IoC
            # self.event_bus.publish({ type: 'new_data', payload: sensor })


def callback(sensor):  
    id = sensor["id"]
    message = "hot" if sensor["data"] > 25 else "cold"
    print(f'{id}: {message}')


store= SensorStore()
example = Example(store)  
example.execute_logic('iddqd', callback)