import asyncio

import websockets

from djitellopy import Tello
message = " "
#drone = Tello()
#drone.connect()

def runCMD(cmd):
    pass
    
async def handler(websocket):
    global message
    while True:
        message = await websocket.recv()
        print(message)
        runCMD(message)


async def main():
    async with websockets.serve(handler, "", 8000):
        await asyncio.Future()  # run forever


if __name__ == "__main__":
    asyncio.run(main())
