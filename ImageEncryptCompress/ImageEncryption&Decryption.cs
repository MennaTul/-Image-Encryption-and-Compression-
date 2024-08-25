using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageEncryptCompress
{
    public class LFSREncryption
    {
        public long Seed;
        public byte TapPosition;
        public byte NumberOfBits;
        public string debugText; // Variable to store debug text
        public LFSREncryption(long seed, byte tapPostition, byte NumOfBits)
        {
            Seed = seed;
            TapPosition = tapPostition;
            NumberOfBits = NumOfBits;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte EncryptByte(byte originalByte)
        {
            byte encryption_byte = 0;
            for (int i = 0; i < 8; i++)
            {
                int TapPostitionBit = (int)(Seed >> TapPosition) & 1;
                int FinalBit = (int)(Seed >> NumberOfBits - 1) & 1;
                int ResXor = FinalBit ^ TapPostitionBit;
                encryption_byte |= (byte)(ResXor << (7 - i));
                Seed = Seed << 1;
                Seed |= ResXor;
            }


            originalByte ^= encryption_byte;
            return originalByte;
        }




    }
}
