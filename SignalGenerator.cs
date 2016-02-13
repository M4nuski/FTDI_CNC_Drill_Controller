namespace CNC_Drill_Controller1
{
    static class SignalGenerator
    {
        //Interface
        public static byte OutputByte0, OutputByte1, OutputByte2;
        public static byte InputByte0, InputByte1;

        //Control signals (v2.0 - v2v2)
        private const int clock_bit = 7;

        private const int usb_to_outputs_bit = 3;
        private const bool usb_to_outputs_default = false;

        private const int inputs_to_usb_bit = 4;
        private const bool inputs_to_usb_default = true;

        //Data signals (v2.0 - v2.2)
        private const int in_buffer0_data_bit = 0;
        private const int in_buffer1_data_bit = 2;

        private const int out_buffer0_data_bit = 6;
        private const int out_buffer1_data_bit = 5;
        private const int out_buffer2_data_bit = 1;

        private static int _buffer_index;

        private static byte[] Powers = {0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80};

        public static bool GetBit(byte data, int bit)
        {
            return ((data & Powers[bit]) > 0);
        }

        public static byte SetBit(byte input, int bitToSet, bool set)
        {
            return (byte)(input | (set ? Powers[bitToSet] : 0));
        }

        public static int Serialize(ref byte[] buffer)
        {
            //reset index
            _buffer_index = 0;

            //strobe inputs_to_usb with clock cycle to load data
            buffer[_buffer_index] = genByte(false, false, false, false, usb_to_outputs_default, !inputs_to_usb_default);
            buffer[_buffer_index] = genByte(false, false, false, false, usb_to_outputs_default, !inputs_to_usb_default);

            for (var i = 0; i < 16; i++)
            {
                buffer[_buffer_index] = genByte(false, GetBit(OutputByte0, (i / 2)), GetBit(OutputByte1, (i / 2)), GetBit(OutputByte2, (i / 2)), usb_to_outputs_default, inputs_to_usb_default);
            }

            //strobe usb_to_outputs
            buffer[_buffer_index] = genByte(true, false, false, false, !usb_to_outputs_default, inputs_to_usb_default);
            buffer[_buffer_index] = genByte(true, false, false, false, usb_to_outputs_default, inputs_to_usb_default);

            return _buffer_index;
        }

        public static void Deserialize(byte[] buffer)
        {
            byte result0 = 0;
            byte result1 = 0;

            for (byte i = 0; i < 8; i++)
            {
                result0 = SetBit(result0, i, GetBit(buffer[17 - (i * 2)], in_buffer0_data_bit));
                result1 = SetBit(result1, i, GetBit(buffer[17 - (i * 2)], in_buffer1_data_bit));
            }

            InputByte0 = result0;
            InputByte1 = result1;
        }

        private static byte genByte(bool clk_inhibit, bool out0, bool out1, bool out2, bool out_en, bool in_en)
        {
            byte result = 0;

            result = SetBit(result, clock_bit, (!clk_inhibit & getClock()));

            result = SetBit(result, out_buffer0_data_bit, out0);
            result = SetBit(result, out_buffer1_data_bit, out1);
            result = SetBit(result, out_buffer2_data_bit, out2);

            result = SetBit(result, inputs_to_usb_bit, in_en);
            result = SetBit(result, usb_to_outputs_bit, out_en);

            _buffer_index++;

            return result;
        }

        private static bool getClock()
        {
            return (_buffer_index % 2 == 1);
        } 
    }
}
