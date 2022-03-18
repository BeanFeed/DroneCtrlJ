import websockets
import asyncio
from djitellopy import Tello
message = ""

#drone = Tello()
#drone.connect()

async def handler(websocket, path):
    global message
    data = await websocket.recv()
    message = data
    print(message)
    #reply = f"Data recieved as:  {data}!"
    #drone.send_control_command(message)
    #await websocket.send(reply)

 

start_server = websockets.serve(handler, "localhost", 8000)

 

asyncio.get_event_loop().run_until_complete(start_server)

asyncio.get_event_loop().run_forever()