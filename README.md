# FTDI_CNC_Drill_Controller
USB controller for custom CNC drilling machine.

Interface is a FTDI UM245R module with custom breakout board to expose 3 output bytes and 2 input bytes.
Stepper motor control is real-time over the interface, with only high current motor drivers betwwen board and steppers.

Software allow for basic cnc configuration and controls, loading of svg and vdx files to find holes positions.
Path algorithm tries a few differents patterns and select shortest.

PCB of breakout board is included in project.

Pictures of the CNC drilling machine construction and tests (also some early prototypes)
* https://goo.gl/photos/cUezNY92o8xAcwc56
* https://goo.gl/photos/mY4PiU6PA8UAoFRF8
* https://goo.gl/photos/yG3t6QUkUywif1z76
* https://goo.gl/photos/jmXB7fFSiwjEQvk7A
* https://goo.gl/photos/BcCgAcpxxzAGTXC69
* https://goo.gl/photos/S6WXo8gRHk6RUXXL8

Videos:
* http://youtu.be/yBXQCClp4Iw
* http://youtu.be/GDIg3tSUmlU
* http://youtu.be/zTymWBI7UiY
* http://youtu.be/2H-7o8vJd5Q
* http://youtu.be/W3UiSMm1FBU
* http://youtu.be/XLfDvXdqlG8
* http://youtu.be/9ViaTz70iDg
* http://youtu.be/PQLuftB3uqs

Latest : Test run with new torque assist and multi-threading
* http://youtu.be/hOwpW07IG70
