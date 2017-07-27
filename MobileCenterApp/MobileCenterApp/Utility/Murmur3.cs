using System;

namespace MobileCenterApp
{
    /// <summary>
    /// Murmur hash.
    /// 
    /// Creates an evenly destributed uint hash from a byte array.
    /// Very fast and fairly unique
    /// </summary>
    public static class Murmur3
    {
        /// <summary>
        /// x86 32 bit implementation of Murmur Hash 3 initializer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        static public uint MurmurHash3(byte[] data, uint length, uint seed)
        {
            uint nblocks = length >> 2;

            uint h1 = seed;

            const uint c1 = 0xcc9e2d51;
            const uint c2 = 0x1b873593;

            //----------
            // body

            int i = 0;

            for (uint j = nblocks; j > 0; --j)
            {
                uint k1l = BitConverter.ToUInt32(data, i);

                k1l *= c1;
                k1l = rotl(k1l, 15);
                k1l *= c2;

                h1 ^= k1l;
                h1 = rotl(h1, 13);
                h1 = h1 * 5 + 0xe6546b64;

                i += 4;
            }

            //----------
            // tail

            nblocks <<= 2;

            uint k1 = 0;

            uint tailLength = length & 3;

            if (tailLength == 3)
                k1 ^= (uint)data[2 + nblocks] << 16;
            if (tailLength >= 2)
                k1 ^= (uint)data[1 + nblocks] << 8;
            if (tailLength >= 1)
            {
                k1 ^= data[nblocks];
                k1 *= c1; k1 = rotl(k1, 15); k1 *= c2; h1 ^= k1;
            }

            //----------
            // finalization

            h1 ^= length;

            h1 = fmix(h1);

            return h1;
        }

        static uint fmix(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;

            return h;
        }

        static uint rotl(uint x, byte r)
        {
            return (x << r) | (x >> (32 - r));
        }
    }
}
