struct Bits
{
    unsigned char bytes[64];
} bits;


void reboot ()
{

// Set bit 3 of byte 5, to 1

bits.bytes[5] |= (1 << 3);

// Set bit 3 of byte 5, to 0

bits.bytes[5] &= ~(1 << 3);

}