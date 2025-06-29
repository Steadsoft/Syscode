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

## Synatx Highlights
The language syntax is based on PL/I and takes the positive features of that syntax while removing some of the out dated or idiosyncratic features.

### Declarations
Begin with the `dcl` keyword followed an `identifier` and obligator and optional attributes, examples
- `dcl counter bin8` an 8 bit signed integer
- `dcl rate bin(44)` a 44 bit signed integer
- `dcl name string(32)` a 32 character fixed lenght string
- `dcl message string(128) var` 123 character max length varying length string
- `dcl map descriptor unaligned` a variable `map` of `struct` type `descriptor` with all members unaligned for compactness

### Loops
There are three kinds of loops, the `for` loop the `while` loop and the `until` loop. These loops types can be combined, examples (contained statements omitted for brevity)
```
for (index = 1 to 100 by 2)
end
```
```
for I = 100 to 40 by -4
end
```
```
for (X = A to B by I) 
end
```
```
while (limit < 100)
end
```
```
until count > max
end
```

