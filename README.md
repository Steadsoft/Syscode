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
