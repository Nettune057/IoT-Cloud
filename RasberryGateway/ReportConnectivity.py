from http import client
from pydoc import cli
import time
from azure.iot.device import IoTHubModuleClient
from janus import T
import serial
import json
import enum

#CONNECTION STRING CỦA THIẾT BỊ => CODE CHẠY TRÊN RAS BERRY
CONNECTION_STRING = "HostName=nha-kinh-iot-hub.azure-devices.net;DeviceId=demorasberry;SharedAccessKey=X0c5Bggoi4jZ8E+TxIeF2uPghFZeDLEaDPfg6th4fzA="
ser = serial.Serial('COM4', 9600)

class DataStateFlag(enum.Enum):
    IDLE = 0
    SendingReportedData = 1
    ReceiveDesiredData = 2

def get_sensor_data(reported_data):
    data = json.loads(reported_data)
    arduino_id = data["ArduinoId"] 
    humid = data["Humidity"]
    temp = data["Temperature"]
    light = data["LuminousIntensity"]
    dict ={ 
        'SensorParameters' : {
            arduino_id : {
                'Humidity' : humid,
                'Temperature' : temp,
                'LuminousIntensity' : light    
            }
        }
    }
    return dict

def get_desired_data(client, arduino_id):
    twin_patch = client.receive_twin_desired_properties_patch()
    myData = twin_patch['DesiredParameters'][arduino_id]
    humid = myData['Humidity']
    temp = myData['Temperature']
    desired_data = {
        "ArduinoId" : arduino_id,
        "Humidity" : humid,
        "Temperature" : temp
    }
    return desired_data

#Hàm để tạo client
def create_client():
    try:
        client = IoTHubModuleClient.create_from_connection_string(CONNECTION_STRING)
        client.connect()
    except:
        client.shutdown()
    return client

def main():
    print("Starting the py Iot Hub Device Twin device sample...")
    client = create_client()
    print("IoTHubModuleClient waiting for commands")
    try:
        twin_patch = client.on_twin_desired_properties_patch_received
        print(twin_patch)
        time.sleep(1)
        
        # reported = """{"ArduinoId":"AR01","Humidity":90,"Temperature": 27,"LuminousIntensity":102}"""
        # reported_patch = get_sensor_data(reported)
        # print(reported_patch)
        # client.patch_twin_reported_properties(reported_patch)
        # print("Reported properties updated")  
        # time.sleep(1)
        
    except KeyboardInterrupt:
        print("Iot Hub Device Twin device sample stopped")
    finally:
        print("Shutting down IoT Hub client")
        ser.close()
        client.shutdown()

if __name__ == '__main__':
    main()