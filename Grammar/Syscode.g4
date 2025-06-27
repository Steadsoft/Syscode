
/*
    GRAMMAR OVERVIEW
    This grammar is based (in part) on the concrete syntax of PL/I defined in ANSI standard X3.74-1987 and Julia and some others.
    This grammar has several important characteristics that reflect language design goals.
    1. There are no reserved words, identfiers can be keywords, new keywords can be added over time with backward compatibility.
    2. Statements are either block or single in nature. Block statements are terminated by 'end' and single by either Newline or Semicolon.
    3. Staments may be preceded by any number of Newlines and or Semicolons which are simply ignored.
    4. One or more newlines may appear inbetween the terms of a statement so that a statement can be split across lines.
    5. Semicolons must appear at the end of single statements and optionally in front of statements (which are ignored like empty statements)
    6. An alternative numeric literal that's an (colon identifier) starting with digits or digits.digits (and optional leading +/-) is supported but must be defined with a true numeric value.
 */

grammar Syscode;

// Parser rules

preamble: (NEWLINE | SEMICOLON)+;
statementSeparator : (SEMICOLON | NEWLINE);
emptyLines: NEWLINE+;


compilation: (statement* endOfFile); 
statement:  preamble?  (assignment | call | return | label | scope | enum | struct | if | declare | literal | procedure | forLoop | whileLoop | untilLoop);
//realStatement : (assignment | label | scope | enum | struct | if | declare | literal | procedure );

//statements: (statement)*;
label: AT identifier statementSeparator;
scope:  blockScope;
//lineScope:  (SCOPE emptyLines? Name=qualifiedName emptyLines? statementSeparator);
blockScope: (SCOPE emptyLines? Name=qualifiedName emptyLines? statement* emptyLines? END)  ;
procedure: PROC emptyLines? Spelling=identifier paramList? statement* emptyLines? END;
struct: STRUCT structDefinition ;
enum: ENUM emptyLines? Name=identifier emptyLines? typename? memberSeparator emptyLines? Members=enumMembers emptyLines? END;
call: CALL emptyLines? reference statementSeparator;
return: (RETURN (emptyLines? LPAR expression RPAR)?) | (RETURN (emptyLines? expression)?) statementSeparator;
declare: DCL emptyLines? Spelling=identifier emptyLines?  Bounds=dimensionSuffix? emptyLines? typename statementSeparator ;
literal: LIT customLiteral AS decLiteral statementSeparator ;
loop: forLoop | whileLoop | untilLoop ;
forLoop : FOR reference EQUALS expression TO expression (BY expression)? emptyLines? (whileCondition emptyLines? untilCondition? | untilCondition emptyLines? whileCondition? | whileCondition | untilCondition)? statement* emptyLines? END ;
whileLoop: whileCondition untilCondition? statement* emptyLines? END;
untilLoop: untilCondition whileCondition? statement* emptyLines? END;

whileCondition: WHILE expression ;
untilCondition: UNTIL expression ;

if:             IF emptyLines? exprThenBlock emptyLines? elifBlock? emptyLines? elseBlock? emptyLines? END;
exprThenBlock:  emptyLines? expression emptyLines? THEN emptyLines? thenBlock;
thenBlock :     statement*;
elseBlock :     (ELSE emptyLines? thenBlock);
elifBlock :     (ELIF emptyLines? exprThenBlock)+;

assignment : Target=reference (EQUALS) Source=expression statementSeparator;


reference
  : reference RARROW basicReference argumentsList?  
  | basicReference argumentsList?                    
  ;

basicReference
  : structureQualificationList? Spelling=identifier
  ;

argumentsList
  : arguments+;

structureQualificationList
  : structureQualification+
  ;

structureQualification
  : Spelling=identifier arguments? DOT
  ;

arguments
  : LPAR subscriptCommalist? RPAR
  ;

subscriptCommalist
  : expression (COMMA expression)*
  ;

// subscript
//   : expression
//   ;

expression
  : primitiveExpression                         # ExprPrimitive
  | parenthesizedExpression                     # ExprParenthesized
  | prefixExpression                            # ExprPrefixed

  | <assoc=right> 
    Left=expression emptyLines? power emptyLines? Rite=expression       # ExprBinary
  | Left=expression emptyLines? mulDiv emptyLines? Rite=expression      # ExprBinary
  | Left=expression emptyLines? addSub emptyLines? Rite=expression      # ExprBinary
  | Left=expression emptyLines? shiftRotate emptyLines? Rite=expression   # ExprBinary
  | Left=expression emptyLines? concatenate emptyLines? Rite=expression # ExprBinary
  | Left=expression emptyLines? comparison emptyLines? Rite=expression  # ExprBinary
  | Left=expression emptyLines? boolAnd emptyLines? Rite=expression     # ExprBinary
  | Left=expression emptyLines? boolXor emptyLines? Rite=expression     # ExprBinary
  | Left=expression emptyLines? boolOr emptyLines? Rite=expression      # ExprBinary
  | Left=expression emptyLines? logand emptyLines?  Rite=expression      # ExprBinary
  | Left=expression emptyLines? logor emptyLines? Rite=expression       # ExprBinary
  ;

primitiveExpression
  : numericLiteral
  | strLiteral
  | customLiteral
  | reference
  ;

strLiteral
  : STR_LITERAL
  ;

numericLiteral
  : binLiteral
  | octLiteral
  | hexLiteral
  | decLiteral
  ;

hexLiteral
  : (HEX_LITERAL)
  ;

binLiteral
  : (BIN_LITERAL)
  ;

octLiteral
  : (OCT_LITERAL)
  ;

decLiteral
  : (INTEGER)
  | (DEC_LITERAL)
  ;
  
customLiteral // Used to allow stuff like 23.5MHz to represent 23.5 (or whatever, defined by the code somewhere)
    : (CUSTOM_LITERAL)
    ;

parenthesizedExpression
  : LPAR expression RPAR
  | REDAND expression RPAR
  | REDOR expression RPAR
  | REDXOR expression RPAR
  | REDNAND expression RPAR
  | REDNOR expression RPAR
  | REDXNOR expression RPAR
  ;

prefixExpression
  : prefixOperator expression
  ;

dimensionSuffix
  : LPAR boundPairCommalist RPAR
  ;

boundPair
  : (lowerBound COLON)? upperBound
  | TIMES
  ;

boundPairCommalist
  : boundPair (COMMA boundPair)*
  ;

// See page 208 PL/I Subset G standard. Lower bound must be <= upper
// (but this is not a grammar issue, just a note)

lowerBound
  : expression
  ;

upperBound
  : expression
  ;

logand: LOGAND;
logor: LOGOR;
concatenate: CONC;
power: POWER;
shiftRotate
  : (L_ROTATE | R_ROTATE | L_LOG_SHIFT | R_LOG_SHIFT | R_ART_SHIFT)
  ;

addSub
  : (PLUS | MINUS)
  ;

mulDiv
  : (TIMES | DIVIDE | PCNT)
  ;

boolAnd
  : (AND | NAND)
  ;

boolXor
  : (XOR | XNOR)
  ;

boolOr
  : (OR | NOR | NOT)
  ;

comparison
  : GT
  | GTE
  | EQUALS
  | LT
  | LTE
  | NGT
  | NE 
  | NLT
  ;

prefixOperator
  : PLUS
  | MINUS
  | NOT
  ;

structDefinition: structName emptyLines? memberSeparator emptyLines? Members=structMembers emptyLines? END;
 
qualifiedName: identifier (DOT identifier)*;
paramList: LPAR identifier (COMMA identifier)* RPAR;
constArrayList: (LPAR INTEGER (COMMA INTEGER)* RPAR);
//numericConstant: INTEGER;
// struct
//structMemberList: structMember+ ;
structName: Spelling=identifier Bounds=dimensionSuffix?;
structMembers
    :  emptyLines? structMember emptyLines? (memberSeparator emptyLines? structMember emptyLines?)*  memberSeparator? emptyLines?;
enumMembers: emptyLines? enumMember emptyLines? (memberSeparator emptyLines? enumMember emptyLines?)* memberSeparator? emptyLines?;
structMember
    : structField
    | structDefinition;

structField:   (Spelling=identifier emptyLines? Bounds=dimensionSuffix? Type=typename );
structStruct:  structDefinition; 

enumMember: (Name=identifier);
identifier: keyword | IDENTIFIER;
typename 
    : binaryType
    | decimalType
    | stringType 
    | bitstringType 
    | identifier
    | unitType
    ;

unitType: UNIT;

binaryType: BIN8 | BIN16 | BIN32 | BIN64 | UBIN8 | UBIN16 | UBIN32 | UBIN64 | ((BIN | UBIN) argumentsList?) ;
decimalType:  ((DEC | UDEC) argumentsList) ;

stringType: STRING argumentsList? ; //'(' INTEGER ')';
bitstringType: BIT argumentsList? ;//'(' INTEGER ')';

// Punctuation rules
memberSeparator : COMMA;

// Utility rules
endOfFile: emptyLines? EOF;

keyword: AS|BIN16|BIN32|BIN64|BIN8|BIN|BIT|BY|CALL|DCL|DEC|DEF|ELIF|ELSE|ENUM|FOR|FOREVER|FUNC|IF|PATH|PROC|RETURN|SCOPE|STRING|STRUCT|THEN|TO|UBIN16|UBIN32|UBIN64|UBIN8|UBIN|UDEC|UNIT|UNTIL|WHILE ;


// Allow comment blocks slash/star TEXT star/slash to be nested 
COMMENT: (BCOM (COMMENT | .)*? ECOM) -> skip; //channel(HIDDEN);

fragment BINARY:  [0-1];
fragment OCT:     [0-7];
fragment DECIMAL: [0-9];
fragment HEX:     [0-9a-fA-F];
fragment BCOM:    ('/*');
fragment ECOM:    ('*/');
fragment FRAC_H:  ('.' [0-9a-fA-F]+);
fragment BASE_H:  (':h' | ':H');
fragment FRAC_D:  ('.' [0-9]+);
fragment BASE_D:  (':d' | ':D');
fragment FRAC_O:  ('.' [0-7]+);
fragment BASE_O:  (':o' | ':O');
fragment FRAC_B:  ('.' [0-1]+);
fragment BASE_B:  (':b' | ':B');

HEX_LITERAL:  ((HEX (' ' HEX)*)+ | (HEX ('_' HEX)*)+) FRAC_H? BASE_H;
OCT_LITERAL:  ((OCT (' ' OCT)*)+ | (OCT ('_' OCT)*)+) FRAC_O? BASE_O;
DEC_LITERAL:  (DECIMAL (' ' DECIMAL)*)+ FRAC_D? BASE_D?;
BIN_LITERAL:  ((BINARY (' ' BINARY)*)+ | (BINARY ('_' BINARY)*)+) FRAC_B? BASE_B;
INTEGER:      ([1-9] [0-9]*);

// Keyword Tokens

AS:             'as';
BIN16:          'bin16';
BIN32:          'bin32';
BIN64:          'bin64';
BIN8:           'bin8';
BIN:            'bin';
BIT:            'bit';
BY:             'by';
CALL:           'call';
DCL:            'dcl' ;
DEC:            'dec';
DEF:            'def';
ELIF:           'elif';
ELSE:           'else';
ENUM:           'enum';
END:            'end';
FOR:            'for';
FOREVER:        'forever';
FUNC:           'func' | 'function';
IF:             'if';
IS:             'is';
LIT:            'lit' | 'literal';
PATH:           'path';
PROC:           'proc' | 'procedure';
RETURN:         'return';
SCOPE:          'scope';
STRING:         'string';
STRUCT:         'struct';
THEN:           'then';
TO:             'to';
UBIN16:         'ubin16';
UBIN32:         'ubin32';
UBIN64:         'ubin64';
UBIN8:          'ubin8';
UBIN:           'ubin';
UDEC:           'udec';
UNIT:           'unit';
UNTIL:          'until';
WHILE:          'while';

// Symbol tokens

// NOTE the lexer function GetLiteralText will return null for LPAR: ('('); but works for LPAR: '(';

COLON:          ':';
CONC:           '++';         // concatenate character strings or bit strings
LOGAND:         '&&';         // short-circuit, logical AND
LOGOR:          '||';         // short-circuit, logical OR
AND:            '&';
OR:             '|';
NAND:           '~&';
NOR:            '~|';
XOR:            '^'|'⊕';      // U+2295 excluisve bitwise OR
XNOR:           '~^'|'~⊕';    // U+2295
NOT:            '~';
GT:             '>';
LT:             '<';
GTE:            '>='|'≥';
LTE:            '<='|'≤';
NGT:            '~>';
NLT:            '~<';
NE:             '~='|'≠';
POWER:          '**' | '🠕';   // U+1F815
STR_LITERAL:    (QUOTE (.)*? QUOTE);
PLUS:           '+';
MINUS:          '-';
TIMES:          '*';
DIVIDE:         '/' | '÷';    // U+00F7
PCNT:           '%';
QUOTE:          '"';
REDAND:         '&(';
REDOR:          '|(';
REDNOR:         '~|(';
REDXOR:         '^('|'⊕(';   // U+2295
REDXNOR:        '~^('|'~⊕('; // U+2295
REDNAND:        '~&(';
L_LOG_SHIFT:    '<<';         // logical: left bit lost rite bit becomes zero
R_LOG_SHIFT:    '>>';         // logical: rite bit lost left bit becomes zero
R_ART_SHIFT:    '>>>';        // arithmetic: rite bit lost left bit is copy of sign bit
L_ROTATE:       '<@'|'⧀';    // U+29C0 rotate: left bit rotated out rite bit becomes that rotated left bit
R_ROTATE:       '@>'|'⧁';    // U+29C1 rotate: rite bit rotated out left bit becomes that rotated rite bit
EQUALS:         '=' ;
ASSIGN:         '<-';
COMPASSIGN:     '+=' | '-=' | '*=' | '/=';
DOT:            '.';
AT:             '@';
SEMICOLON:      ';'; 
COMMA:          ',';
LPAR:           '(';
RPAR:           ')';
RARROW:         '->';

IDENTIFIER:     ([a-zA-Z_] [a-zA-Z0-9_]*);
CUSTOM_LITERAL: ('-' | '+')? ((DECIMAL (' ' DECIMAL)*)+ FRAC_D?) COLON IDENTIFIER;

NEWLINE:        ('\r' '\n'); 
WS:             [ \t]+ -> skip;
//COMMENT: '/*' .*? '*/' -> channel(HIDDEN);