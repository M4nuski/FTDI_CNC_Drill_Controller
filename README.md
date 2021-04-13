# FTDI_CNC_Drill_Controller
USB controller for custom CNC drilling machine.

Interface is a FTDI UM245R module with custom breakout board to expose 3 output bytes and 2 input bytes.
Stepper motor control is real-time over the interface, with only high current motor drivers between board and steppers.

Software allow for basic CNC configuration and controls (jog, steps/units, backlash, moveTo, moveBy), loading of svg and vdx files to find holes positions. Loading of gerber drill text files.
Path algorithm tries a few differents patterns and select shortest.

PCB of breakout board is included in project.

Pictures of the CNC drilling machine construction and tests (also some early prototypes)
* https://goo.gl/photos/cUezNY92o8xAcwc56
* https://goo.gl/photos/mY4PiU6PA8UAoFRF8
* https://goo.gl/photos/yG3t6QUkUywif1z76
* https://goo.gl/photos/jmXB7fFSiwjEQvk7A
* https://goo.gl/photos/BcCgAcpxxzAGTXC69
* https://goo.gl/photos/S6WXo8gRHk6RUXXL8

Videos of first version using unkown steppers and screw drive:
* http://youtu.be/yBXQCClp4Iw
* http://youtu.be/GDIg3tSUmlU
* http://youtu.be/zTymWBI7UiY
* http://youtu.be/2H-7o8vJd5Q
* http://youtu.be/W3UiSMm1FBU
* http://youtu.be/XLfDvXdqlG8
* http://youtu.be/9ViaTz70iDg
* http://youtu.be/PQLuftB3uqs

Test run with new torque assist and multi-threading
* http://youtu.be/hOwpW07IG70

Videos of testing version 2.0 with proper steppers and belt drive:
* https://youtu.be/Uma1yXQDaL8
* https://youtu.be/PN_97VGhWDc
* https://youtu.be/VXQnqaPJm00

Test run for an actual PCB:
* https://youtu.be/fNJGF84zmqw

