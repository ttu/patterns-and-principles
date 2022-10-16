import json
import urllib.request
import smtplib
from email.message import EmailMessage


# We have a class with a simple business logic
# Decide from sensor data if we should send an alert or an ok-message

"""
High Coupling
  - Class has dependency to data fetching and email sending besides that actual business logic
Low Cohesion
 - Class has multiple functionalities; fetches data, alert decision logic and email sending
"""
class ExampleStart:
    URL = 'https://dummy-sensors.azurewebsites.net/api/sensor'
    def execute_logic(self, id):
        sensor_response = urllib.request.urlopen(f'{self.URL}/{id}')  # <-- dependency to urllib
        payload = sensor_response.read()
        sensor =  json.loads(payload.decode('utf-8'))

        message  = 'alert' if sensor['data'] > 25 else 'ok'  # Actual business logic of the class

        msg = EmailMessage()   # <-- dependency to EmailMessage
        msg.set_content(message)
        msg['Subject'] = f'Message from sensor: {sensor["id"]}'
        msg['From'] = 'alert@me.com'
        msg['To'] = 'receiver@you.com'

        # ConnectionRefusedError: [Errno 111] Connection refused
        # s = smtplib.SMTP('localhost')  # <-- dependency to smtplib
        # s.send_message(msg)
        # s.quit()
        print(f'Sent: {msg}')


class SensorStore:
    URL = 'https://dummy-sensors.azurewebsites.net/api/sensor'
    def get_sensor(self, id):
        sensor_response = urllib.request.urlopen(f'{self.URL}/{id}')
        payload = sensor_response.read()
        return json.loads(payload.decode('utf-8'))


class EmailSender:
    def send_message(self, sensor_id, message):
        msg = EmailMessage()
        msg.set_content(message)
        msg['Subject'] = f'Message from sensor: {sensor_id}'
        msg['From'] = 'alert@me.com'
        msg['To'] = 'receiver@you.com'

        # ConnectionRefusedError: [Errno 111] Connection refused
        # s = smtplib.SMTP('localhost')
        # s.send_message(msg)
        # s.quit()
        print(f'Sent: {msg}')


"""
High Coupling
  - Class has dependency to SensorStore, which encapsulates sensor data fetching, and EmailSender, encapsulatin email sending
  - Coupling is "better" as logic is in dependent classes, but instances are still created inside the example-class
High Cohesion
 - Class does a single thing; decide the message type we should send
 - Data fetching and Email sending are in own classes
"""
class ExampleMiddle:
    def execute_logic(self, id):
        store = SensorStore()   # <-- dependency to SensorStore
        sensor = store.get_sensor(id)

        message  = 'alert' if sensor['data'] > 25 else 'ok'  # Actual business logic of the class

        message_sender = EmailSender()   # <-- dependency to EmailSender
        message_sender.send_message(sensor['id'], message)


class SensorStoreDB:
    def get_sensor(self, id):
        return self.connection.execute('SELECT * FROM sensorts WHERE id = %s LIMIT 1', [id])

"""
Low Coupling
  - SensorStore and EmailSender are passed to class in constructor
    - store can be SensorStore or SensorStoreDB or something else
    - message_sender can be EmailSender or something else
High Cohesion
 - Class does a single thing, decide the message type we should send
 - Data fetching and Email sending are in own classes
"""
class ExampleEnd:
    def __init__(self, store, message_sender):
        self.store = store
        self.message_sender = message_sender
        
    def execute_logic(self, id):
        sensor = self.store.get_sensor(id)

        message  = 'alert' if sensor['data'] > 25 else 'ok'  # Actual business logic of the class

        self.message_sender.send_message(sensor['id'], message)


ExampleStart().execute_logic('acdc1')
ExampleMiddle().execute_logic('acdc1')
ExampleEnd(SensorStore(), EmailSender()).execute_logic('abba5')


# Functional bonus examples

# Functional version of execute logic
def execute_logic(id, get_sensor, send_message):
    sensor = get_sensor(id)
    message  = 'alert' if sensor['data'] > 25 else 'ok'  # Actual business logic of the class
    send_message(sensor['id'], message)

execute_logic('idkfa', SensorStore().get_sensor, EmailSender().send_message)

# Create a wrapper function that encapsulates whole logic and dependencies
execute_get_from_api_send_email = lambda x: execute_logic(x, SensorStore().get_sensor, EmailSender().send_message)
# Now this functiion can be passed to other functions/modules
execute_get_from_api_send_email('iddqd')