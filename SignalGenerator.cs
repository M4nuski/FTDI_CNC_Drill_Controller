using System;

namespace CNC_Drill_Controller1
{
    static class SignalGenerator
    {
        //Interface
        public static byte OutputByte0, OutputByte1;//, OutputByte2;
        public static byte InputByte0;//, InputByte1;

        //Control signals (v0.0 - v1.0)
        private const byte clock = 0x01;
        private const byte usb_to_outputs = 0x10;
        private const bool usb_to_outputs_default = false;
        private const byte inputs_to_usb = 0x20;
        private const bool inputs_to_usb_default = true;

        //Data signals (v0.0 - v1.0)
        private static byte in_buffer0 = 0x02;
        private const byte out_buffer0 = 0x04;
        private const byte out_buffer1 = 0x08;

        private static int _buffer_index;

        private static byte[] Powers = {0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80};

        //unused private static byte buffer = 0x40; //reserved for v2.0
        //unused private static byte buffer = 0x80; //reserved for v2.0

        public static bool GetBit(byte data, int bit)
        {
            return ((data & Powers[bit]) > 0);
        }

        public static byte SetBit(byte input, byte bitToSet, bool set)
        {
            return (byte)(input | (set ? Powers[bitToSet] : 0));
        }

        public static int Serialize(ref byte[] buffer)
        {
            //reset index
            _buffer_index = 0;

            //strobe inputs_to_usb with clock cycle to load data
            buffer[_buffer_index] = genByte(false, false, false, usb_to_outputs_default, !inputs_to_usb_default);
            buffer[_buffer_index] = genByte(false, false, false, usb_to_outputs_default, !inputs_to_usb_default);

            for (var i = 0; i < 16; i++)
            {
                buffer[_buffer_index] = genByte(false, GetBit(OutputByte0, i/2), GetBit(OutputByte1, i/2), usb_to_outputs_default, inputs_to_usb_default);
            }

            //strobe usb_to_outputs
            buffer[_buffer_index] = genByte(true, false, false, !usb_to_outputs_default, inputs_to_usb_default);
            buffer[_buffer_index] = genByte(true, false, false, usb_to_outputs_default, inputs_to_usb_default);

            return _buffer_index;
        }

        public static void Deserialize(ref byte[] buffer)
        {
            byte result = 0;
            for (byte i = 0; i < 8; i++)
            {
                result = SetBit(result, i, (buffer[3 + (i * 2)] & in_buffer0) > 0);
            }
            in_buffer0 = result;
        }

        private static byte genByte(bool clk_inhibit, bool out0, bool out1, bool out_en, bool in_en)
        {
            byte result = 0;

            result = (byte)(result | ((clk_inhibit | !getClock()) ? 0 : clock));

            result = (byte)(result | ((out0) ? out_buffer0 : 0));
            result = (byte)(result | ((out1) ? out_buffer1 : 0));

            result = (byte)(result | ((in_en) ? inputs_to_usb : 0));
            result = (byte)(result | ((out_en) ? usb_to_outputs : 0));

            _buffer_index++;
            return result;
        }

        private static bool getClock()
        {
            return (_buffer_index % 2 == 1);
        } 
    }
}
