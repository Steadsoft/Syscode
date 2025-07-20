# Data Layout

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
An array can be declared with a specific alignment, this determines the alignment only of the item's address, all array elements however to the default for their type. 

## Structure Alignment
Syscode allows you to have complete control over the physical layout of storage when using structures. You can declare and use structures without regard to alignment when the structure is used merely as an aggerate of disparate data elements. 

The following attributes influence the layout of data within a `structure`:

### Packed
The `packed` attribute on a `structure` causes every member field to be aligned on the next available byte boundary. No padding is inserted between the members of the structure. 

### Align
The `align` attribute on a declared structure or field, causes that item to be given a runtime address that has the specified alignment. 

### Offset
The `offset` attribute on a declared structure or field, causes that item to be given a runtime address that is offset by the specified number of bytes, from the start of the structure.

### Auto
The `auto` attribute causes the compiler reorder structure members if it can use less space, the physical order of members might not be the same as the written order.

For maximum  density in a structure simply give it the `packed` attribute, this might lead to members being misaligned and therefore incurr a runtime cost overhead (or in some cases a hardware misalignment exception).

| Attribute | Field Effect                                                 | Struct Effect                                                              | Auto Padded? |  Remarks                              |
|-------------|--------------------------------------------------------------|----------------------------------------------------------------------------|--------------|---------------------------------------|
| Packed      | ILLEGAL                                                      | Byte aligned struct and members, no padding and possible misaligned fields | No           |                                       |
| Align(n)    | Field gets address with specified alignment                  | Struct gets address with specified alignment                               | Yes          |                                       |
| Offset(n)   | Field gets address offset `n` bytes from parent struct start | Struct gets address offset `n` bytes from parent struct start              | Yes          | Cannot be used on outermost structure |
| Pad(n,byte) | Insert a `pad` field `n` bytes long.                         |  ILLEGAL                                                                   |              | Creates a distinct field              |
| Pad(n,bit)  | Insert a `pad` field `n` bits long.                          |  ILLEGAL                                                                   |              | Creates a distinct field              |
| Auto        | ILLEGAL                                                      | All fields aligned naturally and possibly reordered from what is written   | Yes          |                                       |

The attributes `offset` and `pad` include a unit specifier of either `bit` or `byte` like `offset(3, bit)` or `pad(11,byte)` the `bit` forms only being permitted when the preceding field is a bit field. 

Only bit fields can ever have a bit offset, non bit fields are always aligned on at least a 1-byte boundary. 

