# Data Alignment

Syscode is designed for working with unusual or challenging memory layouts. Every data element has a default alignment based on it's declared type. A `bin8` variable for example is given a default alignment of 1 byte, that is, its address at runtime is unrestricted and can be any address the system chooses for it. By contrast a `bin32` is given a default alignment of 4 bytes, that is, its address at runtime will always be a multiple of 4.

A `structure` too has a default alignemnt that's based on the default alignment of the structure member which has the greatest alignment, so a `structure` that has a member of type `ubin64` for example will be given a default alignment of 8 bytes.



