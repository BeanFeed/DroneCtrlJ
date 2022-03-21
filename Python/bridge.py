import asyncio

import websockets

from djitellopy import Tello
message = " "
drone = Tello()
drone.connect()

def runCMD(cmd):
    if(type(cmd) is str):
        if(cmd.startswith("rc")):
            cmd = cmd.split()
            a1 = int(cmd[1])
            a2 = int(cmd[2])
            a3 = int(cmd[3])
            a4 = int(cmd[4])
            drone.send_rc_control(a1,a2,a3,a4)
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
