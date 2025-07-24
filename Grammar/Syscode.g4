
/*
    GRAMMAR OVERVIEW
    This grammar is based (in part) on the concrete syntax of PL/I defined in ANSI standard X3.74-1987 and Julia and some others.
    This grammar has several important characteristics that reflect language design goals.
    1. There are no reserved words, identfiers can be keywords, new keywords can be added over time with backward compatibility.
    2. Statements are either block or single in nature. Block statements are terminated by 'end' and single by either Newline or Semicolon.
    3. Staments may be preceded by any number of Newlines and or Semicolons which are simply ignored.
    4. One or more newlines may appear inbetween the terms of a statement so that a statement can be split across lines.
    5. A semicolon or newline must appear at the end of single statements and optionally in front of statements (which are ignored like empty statements)
    6. An alternative numeric literal that's an (colon identifier) starting with digits or digits.digits (and optional leading +/-) is supported but must be defined with a true numeric value.
 */

grammar Syscode;

// reference: data alignment = http://bitsavers.informatik.uni-stuttgart.de/pdf/ibm/370/pli/SC26-3114-01_PLI_for_MVS_and_VM_Release_1.1_Language_Reference_199506.pdf

// TODO: We could support this a 'chained' comparison:
// if a < b < c < d < e then...
// but need to fully analyze its meaning and behavior with various operators.
// See: https://langdev.stackexchange.com/a/1866
// See: https://kaya3.github.io/MJr/notes/parser.html#operator-precedence
// Semms its more trouble than its worth, cofuses people and not that common a need.

// Parser rules

preamble: (NEWLINE | SEMICOLON)+;
statementSeparator : (SEMICOLON | NEWLINE | EOF); // EOF lets us end a source file with a line statement without needing a newline/semicolon
emptyLines: NEWLINE+;

compilation: (statement* endOfFile); 

// TODO: avoid 'scope' (aka 'package') for now this is not really a namespace in PL/I and the feature
// needs to be carefully thought out. e.g two source files might each contribute stuff to a package and that 
// complicates how we resolve rerences in package A to stuff in package B...etc

// One way to handle namespace is to name source code as a namespace: system.utils.io.sys wraps all contained items in the namespace system.utils and there could be 
// several source files with that namespace prefix, each of which contributes stuff to the namespace.

statement:  preamble?  (call | return | label | /* scope | */  enum | if | declare | type | /* literal | */ procedure | function | loop | goto | leave | assignment );

//struct: STRUCT structBody ;
structBody: STRUCT Spelling=identifier dimensionSuffix? structAttributes* statementSeparator emptyLines? ((Field=structField|Struct=structBody) emptyLines?)* END ;
structField: Spelling=identifier dimensionSuffix? Type=typeSpecifier attributes* statementSeparator;

label: Name=labelName Subscript=labelSubscript? statementSeparator;
labelName: ATSIGN Spelling=identifier;
labelSubscript: LPAR Literal=decLiteral RPAR;

goto: GOTO Ref=reference statementSeparator;
gotoSubscript: LPAR expression RPAR;

scope:  blockScope; // SEE: https://www.ibm.com/docs/en/epfz/6.2.0?topic=organization-packages
blockScope: (PACKAGE emptyLines? Name=qualifiedName emptyLines? statement* emptyLines? END)  ;
procedure: PROC emptyLines? Spelling=identifier Params=paramList? Options=procOptions? Statements+=statement* emptyLines? END;
function: FUNC emptyLines? Spelling=identifier Params=paramList? Options=procOptions? AS Type=returnDescriptor? Statements+=statement* emptyLines? END;

procOptions: OPTIONS LPAR (Main=MAIN)+ RPAR;

enum: ENUM emptyLines? Name=identifier emptyLines? typeSpecifier? memberSeparator emptyLines? Members=enumMembers emptyLines? END;
call: CALL emptyLines? Ref=reference statementSeparator;
return: (RETURN (emptyLines? Exp=expression)?) statementSeparator ; //| (RETURN (emptyLines? expression)?)) statementSeparator;

declare
    : DCL Struct=structBody
    | DCL emptyLines? Spelling=identifier emptyLines? Bounds=dimensionSuffix? emptyLines? Type=typeSpecifier attributes* statementSeparator 
    ;

type: TYPE Body=structBody ;    

leave: LEAVE Ref=reference statementSeparator;

// literal: LIT customLiteral AS decLiteral statementSeparator ;
loop: Loop=loopLoop | For=forLoop | While=whileLoop | Until=untilLoop ;
forLoop : DO Name=labelName? For=reference EQUALS From=expression TO To=expression (BY By=expression)? emptyLines? (While=whileCondition emptyLines? Until=untilCondition? | Until=untilCondition emptyLines? While=whileCondition? | While=whileCondition | Until=untilCondition)?  statement* emptyLines? END ;
whileLoop: DO Name=labelName? While=whileCondition Until=untilCondition?  statement* emptyLines? END;
untilLoop: DO Name=labelName? Until=untilCondition While=whileCondition?  statement* emptyLines? END;
loopLoop: DO Name=labelName? LOOP statement* emptyLines? END;

whileCondition: WHILE Exp=expression ;
untilCondition: UNTIL Exp=expression ;



if:             IF Name=labelName? emptyLines? ExprThen=exprThenBlock emptyLines? Elif=elifBlock? emptyLines? Else=elseBlock? emptyLines? END;
exprThenBlock:  emptyLines? Exp=expression emptyLines? THEN emptyLines? Then=thenBlock;
thenBlock :     statement*;
elseBlock :     (ELSE emptyLines? Then=thenBlock);
elifBlock :     (ELIF emptyLines? ExprThen+=exprThenBlock)+;

// typeSpecifier 
//     : Code=typeCode Args=arguments? varying?
//     | AS As=identifier
//     ;

typeSpecifier
    : Fix=fixedType
    | Bit=bitType
    | Str=stringType
    | Ent=entryType
    | Lab=labelType
    | Ptr=pointerType
    | As=asType
    | Bytes=bytepadType
    | Builtin=builtinType
    ;

asType: AS Typename=identifier ;    

fixedType: (Typename=(BIN8 | BIN16 | BIN32 | BIN64 | UBIN8 | UBIN16 | UBIN32 | UBIN64)) | (Typename=(BIN | UBIN | DEC | UDEC) Args=arguments? );

bitType: Typename=BIT LPAR Len=decLiteral RPAR;

builtinType: Typename=BUILTIN;

bytepadType: BYTEPAD LPAR Len=decLiteral RPAR;

stringType
    : Typename=STRING LPAR Len=decLiteral RPAR Var=VARIABLE? ;

entryType: Typename=ENTRY 
    (
    | Args=entryArgTypes? Ret=returnDescriptor? Var=VARIABLE?
    | Args=entryArgTypes? Var=VARIABLE? Ret=returnDescriptor? 
    | Ret=returnDescriptor? Args=entryArgTypes? Var=VARIABLE?
    | Ret=returnDescriptor? Var=VARIABLE? Args=entryArgTypes? 
    | Var=VARIABLE? Ret=returnDescriptor? Args=entryArgTypes?
    | var=VARIABLE? Args=entryArgTypes? Ret=returnDescriptor? 
    )
    ;

labelType: Typename=LABEL ;    

pointerType: Typename=POINTER ;    

typeCode: BIN8 | BIN16 | BIN32 | BIN64 | UBIN8 | UBIN16 | UBIN32 | UBIN64 | BIN | UBIN | DEC | UDEC | STRING | BIT | LABEL | (ENTRY entryArgTypes? returnDescriptor?) | POINTER ;

// Consider also <- or == as an assignment opeator, which implicitly does an atomic assignment...

assignment 
    : Ref=reference comparer Exp=expression statementSeparator
    // | LPAR reference COMMA reference RPAR comparer expression statementSeparator; this is too 'out there' for an initial language design. 
    ;

comparer: EQUALS | COMPASSIGN;

reference
  : Pointer=reference RARROW Basic=basicReference ArgsList=argumentsList?  
  | Basic=basicReference ArgsList=argumentsList?                    
  ;

basicReference
  : structureQualificationList? Spelling=identifier
  ;

argumentsList
  : ArgsSet+=arguments+;

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

expression
  : primitiveExpression                         # ExprPrimitive
  | parenthesizedExpression                     # ExprParenthesized
  | prefixExpression                            # ExprPrefixed

  | <assoc=right> 
    Left=expression emptyLines? power emptyLines? Rite=expression       # ExprBinary
  | Left=expression emptyLines? mulDiv emptyLines? Rite=expression      # ExprBinary
  | Left=expression emptyLines? addSub emptyLines? Rite=expression      # ExprBinary
  | Left=expression emptyLines? shiftRotate emptyLines? Rite=expression # ExprBinary
  | Left=expression emptyLines? concatenate emptyLines? Rite=expression # ExprBinary
  | Left=expression emptyLines? comparison emptyLines? Rite=expression  # ExprBinary
  | Left=expression emptyLines? boolAnd emptyLines? Rite=expression     # ExprBinary
  | Left=expression emptyLines? boolXor emptyLines? Rite=expression     # ExprBinary
  | Left=expression emptyLines? boolOr emptyLines? Rite=expression      # ExprBinary
  | Left=expression emptyLines? logand emptyLines?  Rite=expression     # ExprBinary
  | Left=expression emptyLines? logor emptyLines? Rite=expression       # ExprBinary
  ;

primitiveExpression
  : numericLiteral
  | stringLiteral
  //| customLiteral
  | reference
  ;

stringLiteral
  : Text=STR_LITERAL
  ;

numericLiteral
  : Signed=(PLUS | MINUS)? Bin=binLiteral
  | Signed=(PLUS | MINUS)? Oct=octLiteral
  | Signed=(PLUS | MINUS)? Hex=hexLiteral
  | Signed=(PLUS | MINUS)? Dec=decLiteral
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
  
// customLiteral // Used to allow stuff like 23.5MHz to represent 23.5 (or whatever, defined by the code somewhere)
//     : (CUSTOM_LITERAL)
//     ;

parenthesizedExpression
  : LPAR expression RPAR
  ;

prefixExpression
  : prefixOperator expression
  ;

dimensionSuffix
  : LPAR Pair=boundPairCommalist RPAR
  ;

boundPair
  : (Lower=expression COLON)? Upper=expression
  | TIMES
  ;

boundPairCommalist
  : BoundPairs+=boundPair (COMMA BoundPairs+=boundPair)*
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
  : (PLUS  | MINUS)
  | (OPLUS | OMINUS)
  | (SPLUS | SMINUS)
  ;

mulDiv
  : (OTIMES | STIMES | TIMES | DIVIDE | PCNT)
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
  | REDAND
  | REDOR
  | REDXOR
  ;
 
qualifiedName: identifier (DOT identifier)*;
paramList: LPAR Params+=identifier (COMMA Params+=identifier)* RPAR;
constArrayList: (LPAR INTEGER (COMMA INTEGER)* RPAR);
enumMembers: emptyLines? enumMember emptyLines? (memberSeparator emptyLines? enumMember emptyLines?)* memberSeparator? emptyLines?;

enumMember: (Name=identifier);
identifier: Key=keyword | IDENTIFIER;

varying: VARIABLE ;


//entryType: ENTRY (entryList);

//entryList: LPAR typename (COMMA typename)* RPAR ;

structAttributes 
    : ALIGNED 
    | PACKED 
    | basedAttribute
    | AUTO
    | atAttribute
    | orderAttribute
    ;

attributes
    : constAttribute         #AttribConst
    | alignedAttribute       #AttribAligned
    | offsetAttribute        #AttribOffset
    | packedAttribute        #AttribPacked
    | externalAttribute      #AttribExternal
    | internalAttribute      #AttribInternal
    | staticAttribute        #AttribStatic
    | basedAttribute         #AttribBased
    | stackAttribute         #AttribStack
    | initAttribute          #AttribInit
    | PAD                    #AttrPad
    ;

atAttribute: AT (LPAR Address=expression RPAR);
orderAttribute: ORDER LPAR (ASC | DESC) RPAR;
constAttribute: CONST;
alignedAttribute: ALIGNED (LPAR Alignment=expression RPAR)?;
offsetAttribute: OFFSET (LPAR Offset=expression RPAR);
packedAttribute: PACKED;    
externalAttribute: EXTERNAL;
internalAttribute: INTERNAL;
staticAttribute: STATIC;
basedAttribute: BASED (LPAR Base=expression RPAR)? ;
stackAttribute: STACK;
initAttribute: INIT LPAR Value=expression RPAR;
unitType: UNIT;

entryArgTypes: LPAR typeSpecifier (COMMA typeSpecifier)* RPAR;
returnDescriptor: AS LPAR typeSpecifier RPAR;

// binaryCode: BIN8 | BIN16 | BIN32 | BIN64 | UBIN8 | UBIN16 | UBIN32 | UBIN64 | BIN | UBIN arguments?) ;
// decimalType:  ((DEC | UDEC) arguments) ;

// stringType: STRING argumentsList? ; //'(' INTEGER ')';
// bitstringType: BIT argumentsList? ;//'(' INTEGER ')';

// Punctuation rules
memberSeparator : COMMA;

// Utility rules
endOfFile: emptyLines? EOF;

keyword
    : ALIGNED
    | AS
    | ATSIGN
    | AUTO
    | BASED
    | BIN16
    | BIN32
    | BIN64
    | BIN8
    | BIN
    | BIT
    | BUILTIN
    | BY
    | BYTEPAD
    | CALL
    | CONST
    | DCL
    | DEC
    | DEF
    | DO
    | ELIF
    | ELSE
    | END
    | ENTRY
    | ENUM
    | EXTERNAL
    | FOR
    | FOREVER
    | FUNC
    | GOTO
    | IF
    | INIT
    | INTERNAL
    | IS
    | LABEL
    | LEAVE
    | LIT
    | LOOP
    | MAIN
    | OFFSET
    | OPTIONS
    | PACKAGE
    | PACKED
    | PAD
    | PATH
    | POINTER
    | PROC
    | RETURN
    | STACK
    | STATIC
    | STRING
    | STRUCT
    | THEN
    | TO
    | TYPE
    | UBIN16
    | UBIN32
    | UBIN64
    | UBIN8
    | UBIN
    | UDEC
    | UNIT
    | UNTIL
    | VARIABLE
    | WHILE 
    ;


// Allow comment blocks slash/star TEXT star/slash to be nested 
COMMENT: (BCOM (COMMENT | .)*? ECOM) -> skip; //channel(HIDDEN);
LINECOM: (LCOM ~[\r\n]*) -> skip;
HYPERCOMMENT: ('/#' (.)*? '#/') -> skip;
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

HEX_LITERAL:  ((HEX    (' '+ HEX)*)+    | (HEX ('_'+ HEX)*)+) FRAC_H? BASE_H;
OCT_LITERAL:  ((OCT    (' '+ OCT)*)+    | (OCT ('_'+ OCT)*)+) FRAC_O? BASE_O;
DEC_LITERAL:  (DECIMAL (' '+ DECIMAL)*)+ FRAC_D? BASE_D?;
BIN_LITERAL:  ((BINARY (' '+ BINARY)*)+ | (BINARY ('_'+ BINARY)*)+) FRAC_B? BASE_B;
INTEGER:      ([1-9] [0-9]*);

// Keyword Tokens

ALIGNED:        'aligned';
AS:             'as';
ASC:            'asc' | 'ascending';
AT:             'at';
AUTO:           'auto';
BASED:          'based';
BIN16:          'bin16';
BIN32:          'bin32';
BIN64:          'bin64';
BIN8:           'bin8';
BIN:            'bin';
BIT:            'bit';
BUILTIN:        'builtin';
BY:             'by';
BYTEPAD:        'bytepad';
CALL:           'call';
CONST:          'const';
DCL:            'dcl' ;
DEC:            'dec';
DEF:            'def';
DESC:           'desc' | 'descending';
DO:             'do';         
ELIF:           'elif';
ELSE:           'else';
END:            'end';
ENTRY:          'entry';
ENUM:           'enum';
EXTERNAL:       'ext' | 'external';
FOR:            'for';
FOREVER:        'forever';
FUNC:           'func' | 'function';
GOTO:           'goto';
IF:             'if';
INIT:           'init';
INTERNAL:       'internal';
IS:             'is';
LABEL:          'label';
LEAVE:          'leave';
LIT:            'lit' | 'literal';
LOOP:           'loop';
MAIN:           'main';
OFFSET:         'offset';
OPTIONS:        'options';
ORDER:          'order';
PACKAGE:        'package';
PACKED:         'packed';
PAD:            'pad';
PATH:           'path';
POINTER:        'ptr' | 'pointer';
PROC:           'proc' | 'procedure';
RETURN:         'return';
SCOPE:          'scope';
STACK:          'stack';
STATIC:         'static';
STRING:         'string';
STRUCT:         'struct' | 'structure';
THEN:           'then';
TO:             'to';
TYPE:           'type';
UBIN16:         'ubin16';
UBIN32:         'ubin32';
UBIN64:         'ubin64';
UBIN8:          'ubin8';
UBIN:           'ubin';
UDEC:           'udec';
UNIT:           'unit';
UNTIL:          'until';
VARIABLE:       'var' | 'variable';
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
OPLUS:          '[+]';
SPLUS:          '(+)';
MINUS:          '-';
OMINUS:         '[-]';
SMINUS:         '(-)';
TIMES:          '*';
OTIMES:         '[*]';
STIMES:         '(*)';
LCOM:           '//';
DIVIDE:         '/' | '÷';    // U+00F7
PCNT:           '%';
QUOTE:          '"';
REDAND:         '<&';
REDOR:          '<|';
REDXOR:         '<^';   // U+2295
L_LOG_SHIFT:    '<<';         // logical: left bit lost rite bit becomes zero
R_LOG_SHIFT:    '>>';         // logical: rite bit lost left bit becomes zero
R_ART_SHIFT:    '>>>';        // arithmetic: rite bit lost left bit is copy of sign bit
L_ROTATE:       '<@'|'⧀';    // U+29C0 rotate: left bit rotated out rite bit becomes that rotated left bit
R_ROTATE:       '@>'|'⧁';    // U+29C1 rotate: rite bit rotated out left bit becomes that rotated rite bit
EQUALS:         '=' ;
ASSIGN:         '<-';

// comppund assignment

COMPASSIGN:     '+='|'-='|'*='|'/='|'%='|'&='|'|='|'^='|'<<='|'>>='|'<@='|'@>=';
DOT:            '.';
ATSIGN:         '@';
SEMICOLON:      ';'; 
COMMA:          ',';
LPAR:           '(';
RPAR:           ')';
LBRACK:         '[';
RBRACK:         ']';
RARROW:         '->';





IDENTIFIER:     ([a-zA-Z_] [a-zA-Z0-9_]*);
//CUSTOM_LITERAL: ('-' | '+')? ((DECIMAL (' ' DECIMAL)*)+ FRAC_D?) COLON IDENTIFIER;

NEWLINE:        ('\r' '\n'); 
WS:             [ \t]+ -> skip;
//COMMENT: '/*' .*? '*/' -> channel(HIDDEN);
