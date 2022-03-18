using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DirectInput;
using WebSocketSharp;

namespace DroneJoystickCtrl
{
    class Program
    {
        

        static void Main(string[] args)
        {
            WebSocket ws = new WebSocket("ws://localhost:8000");
            

            Console.ReadKey();
            int x = 0;
            int y = 0;

            var directInput = new DirectInput();

            var joystickGuid = Guid.Empty;

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad,
                DeviceEnumerationFlags.AllDevices))
                joystickGuid = deviceInstance.InstanceGuid;

            // If Gamepad not found, look for a Joystick
            if (joystickGuid == Guid.Empty)
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick,
                        DeviceEnumerationFlags.AllDevices))
                    joystickGuid = deviceInstance.InstanceGuid;

            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                Console.WriteLine("No joystick/Gamepad found.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // Instantiate the joystick
            var joystick = new Joystick(directInput, joystickGuid);

            Console.WriteLine("Found Joystick/Gamepad with GUID: {0}", joystickGuid);

            // Query all suported ForceFeedback effects
            var allEffects = joystick.GetEffects();
            foreach (var effectInfo in allEffects)
                Console.WriteLine("Effect available {0}", effectInfo.Name);

            // Set BufferSize in order to use buffered data.
            joystick.Properties.BufferSize = 128;

            // Acquire the joystick
            joystick.Acquire();
            
            while (true)
            {
                joystick.Poll();
                var datas = joystick.GetBufferedData();
                
                
                foreach (var state in datas)
                {
                    
                    if (Convert.ToString(state.Offset) == "X")
                    {
                        x = Convert.ToInt32((state.Value - 32511) / 325.11);
                        if (x > 100) x = 100;
                    }
                    else if (Convert.ToString(state.Offset) == "Y")
                    {
                        y = Convert.ToInt32((state.Value - 32511) / 325.11);
                        if (y > 100) y = 100;
                    }
                    Console.WriteLine(state);
                    string package = "";
                    ws.Send(package);
                    
                }
                //Console.WriteLine("X: " + Convert.ToString(x) + "Y: "+ Convert.ToString(y));
            }
            
            
            
  
        }
        

    }
}

