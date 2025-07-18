# Data Alignment

## Default Alignment
Syscode is designed for working with unusual or challenging memory layouts. Every data element has a default alignment based on it's declared type. A `bin8` variable for example is given a default alignment of 1 byte, that is, its address at runtime is unrestricted and can be any address the system chooses for it. By contrast a `bin32` is given a default alignment of 4 bytes, that is, its address at runtime will always be a multiple of 4.

A `bin(X)` variable is given the alignment that corresponds to X bits rounded up to the nearest power of two, bytes, so for example a `bin(35)` is aligned as if it were a `bin(64)`.

A `structure` too has a default alignemnt that's based on the default alignment of the structure member which has the greatest alignment, so a `structure` that has a member of type `ubin64` for example will be given a default alignment of 8 bytes if every other member has the same or lesser alignment. 

A bit data type too has a default alignemnt of 1 byte. If you declare a variable to be `bit(3)` or `bit(47)` the variable will be aligned on a byte boundary, the `0th` bit being aligned with bit 0 of the byte. 

You can specify the keyword `aligned` on its own, in a declaration to emphasize that the variable is being explicitly given its default alignment.

## Specified Alignment
The `aligned` keyword can optionally include an alignment value which represents the byte alignment to be given to the declared data elemeent. 

```c++
dcl count_rate bin(12) aligned(4) // the datum is aligned on a 4 byte boundary despite it's default alignment being 2 bytes.
```
The alignment value is always understood to represent a number of bytes. Bit field alignment is always either 1 byte (`aligned`) or the next "free" bit (`unaligned`), there's no support for aligning bit fields explicitly on some given bit-offset and so their alignment is always on a byte boundary unless it is a member of a structure.

## Array Alignment
Every element of an array always has the same aligment. That can be either the default or some specific chosen value:

```c++
dcl entries (32) bin16 aligned (4) // the alignment of elements is 4 depsite their default alignment being 2. 
```
if you declare an array with an alignment less than the element size, the compiler will report an error.

## Structure Alignment
Syscode allows you to have complete control over the physical layout of storage when using structures. You can declare and use structures without regard to alignment when the structure is used merely as an aggerate of disparate data elements. 
