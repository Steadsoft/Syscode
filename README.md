# Syscode

## Overview
Syscode is an imperative programming language designed for working on systems oriented software, including operating systems, microcontroller firmware and similar problem domains.

Syscode incorporates native language elements that are usually vendor extensions or non-standard features in languages like C. This helps to improve portability and readability.

A prominent feature of the language is the complete absence of reserved words. This makes it straightforward to introduce new keywords and be confident of backward compatibility.

### Summary of key syntax features
- No reserved words
- Statements can be separated by either newlines or semicolons
- Most statements can be split across multiple lines
- All block type statements are delimited by `keyword` and `end` pairings
- Comment blocks  `/*...*/` may be safely nested
- Numeric literals may contain embedded spaces or underscores

### Language Elements
- Namespaces allow code to be lexically encapsulated
- Every statememnt except assignments, begins with a keyword
- The `elif` keyword can be used within `if` statements
- Loops can be `for` or `while` or `until` and these can be be combined
- Bit rotate operators `<@` and `@>`
- Arbitrary precision integers
- Bit reduce operators `<&` and `<|` and `<^`
- Aligned and unaligned bit strings of arbitrary length
- Nested procedures and functions
- Fixed or varying length strings

## Syntax Highlights
The language syntax borrows a lot from PL/I and takes the positive features of that syntax while removing some of the out dated or idiosyncratic features.

### Declarations
Begin with the `dcl` keyword followed an `identifier` then obligatory and optional attributes, examples
- `dcl counter bin8` an 8 bit signed integer
- `dcl rate bin(44)` a 44 bit signed integer
- `dcl name string(32)` a 32 character fixed length string
- `dcl message string(128) var` 123 character max length varying length string
- `dcl map descriptor unaligned` a variable `map` of `struct` type `descriptor` with all members unaligned for compactness

### Loops
There are three kinds of loops, the iterative `do` loop the `while` loop and the `until` loop. These loops types can be combined, parentheses around expressions are optional, examples (contained statements omitted for brevity)
```
do (index = 1 to 100 by 2)
...
end
```
```
do I = 100 to 40 by -4
...
end
```
```
do (X = A to B by I)
...
end
```
```
do while (limit < 100)
...
end
```
```
do until count > max
...
end
```
#### These can be combined in several ways, giving a rich set of potential loop constructs
```
do J = 1 to 128 while flag = true until storage_used > MAX
...
end
```
There's also the infinite loop, in this loop the loop termination must be from within the loop body itself.
```
do loop
   ...
   if something then
      leave
end
```

The `do` keyword was taken from the PL/I language because we need to reliably distinguish between `while`/`until` *loops* and `while`/`until` as *optional clauses* on loops. 

#### Exiting a loop
Loops can be given an optional name by inserting a label right after the `do` this allows loops to be exited selectively and is especially useful in nested loops:
```
do @outer while space_remains
   ...
   do @inner I = 1 to 100
      ...
      if found then
         leave inner
      elif space = 0 then
         leave outer
      end
   end
   ...
end
```

### Structures
Data structures are defined using either the `dcl` keyword or the `type` keyword, in essence `type` defines a structure of a particular shape in a similar manner to C's typedef. Structures can containa mix of other structures and member fields and these can be nested to an arbitrary level. Note how the `dcl`/`type` keyword is required only at the outermost level. This example defines a struct type named `process_table` that contains another struct named `bitmap`:

```
type struct process_table
     timestamp          ubin(32)
     struct bitmap
          initialized   bit(1)
          init_mode     bit(3)
          reset_count   bit(5)
     end
end
```
The above serves as a kind of template, instances of `process_table` can be declared just as any predefned type like `bin8` or `string`. The alternative syntax using `dcl` like this
```
dcl struct process_table
    timestamp         ubin(32)
    struct bitmap
        initialized   bit(1)
        init_mode     bit(3)
        reset_count   bit(5)
    end
end
```
does not define a structure "template" but rather a runtime created instance of a variable named `process_table` this declaration is instantiated at runtime and can therefore have runtime defined array bounds:
```
dcl struct process_table
    timestamp        ubin(32)
    struct bitmap(x)
       initialized   bit(1)
       init_mode     bit(3)
       reset_count   bit(5)
    end
end
```
Unlike a `type` which must have compile time constants used for array bounds, the declaration form can use expressions like `bitmap(x)` and the entire structure's size is computed at runtime.

### Computed Goto
The language supports the subscripting of label constants with a literal decimal integer, this can then be coupled with a flexible form of the `goto` statement. Labels, because they denote a "place" are defined with the `@` symbol.
```
@state(0)
...
@state(1)
...
...
@state(8)

goto state(x)
```
In addition to subscripted labels Syscode supports label variables:
```
dcl target label
...
target = fsm_start_label
goto target
...
@fsm_start_label
...
```
### Entry Variables
These are akin to C function pointers, variables that can refer to callable procedures and functions.
```
procedure gather (a)
   dcl a bin(24)
...
end
procedure scatter (a)
   dcl a bin(24)
...
end

dcl accessor entry (bin(24))

accessor = gather

call accessor (3219)
``` 
### Numeric Literals
Numeric literals can include a number base specifier and can contain spaces/underscores to aid readability.

```
hex =  01F5 33DA 0F02 11B9:H  // treated as a unsigned ubin64 value 141,075,974,979,719,609
bin = +1001 1101 0010 0011:B  // treated as a signed bin32 value 40,227

sum = hex + bin
```
Literals are assigned a type that matches the magnitude of the value, if it can fit in a `bin8` for example it's treated as a `bin8`. You can use a conversion `builtin` to force a numeric literal to a specific size. In addition if there is no prefix of `+` or `-` then the literal is treated as `unsigned` so just prefix with either a `+` or `-` to have the value treated as a signed number.

