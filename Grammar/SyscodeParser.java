// Generated from Syscode.g4 by ANTLR 4.13.2
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue", "this-escape"})
public class SyscodeParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.13.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		COMMENT=1, HEX_LITERAL=2, OCT_LITERAL=3, DEC_LITERAL=4, BIN_LITERAL=5, 
		INTEGER=6, AS=7, BIN16=8, BIN32=9, BIN64=10, BIN8=11, BIN=12, BIT=13, 
		CALL=14, DCL=15, DEC=16, DEF=17, ELIF=18, ELSE=19, ENUM=20, END=21, FOR=22, 
		FOREVER=23, FUNC=24, IF=25, IS=26, LIT=27, PATH=28, PROC=29, RETURN=30, 
		SCOPE=31, STRING=32, STRUCT=33, THEN=34, UBIN16=35, UBIN32=36, UBIN64=37, 
		UBIN8=38, UBIN=39, UDEC=40, UNIT=41, UNTIL=42, WHILE=43, COLON=44, CONC=45, 
		LOGAND=46, LOGOR=47, AND=48, OR=49, NAND=50, NOR=51, XOR=52, XNOR=53, 
		NOT=54, GT=55, LT=56, GTE=57, LTE=58, NGT=59, NLT=60, NE=61, POWER=62, 
		STR_LITERAL=63, PLUS=64, MINUS=65, TIMES=66, DIVIDE=67, PCNT=68, QUOTE=69, 
		REDAND=70, REDOR=71, REDNOR=72, REDXOR=73, REDXNOR=74, REDNAND=75, L_LOG_SHIFT=76, 
		R_LOG_SHIFT=77, R_ART_SHIFT=78, L_ROTATE=79, R_ROTATE=80, EQUALS=81, ASSIGN=82, 
		COMPASSIGN=83, DOT=84, AT=85, SEMICOLON=86, COMMA=87, LPAR=88, RPAR=89, 
		RARROW=90, IDENTIFIER=91, CUSTOM_LITERAL=92, NEWLINE=93, WS=94;
	public static final int
		RULE_preamble = 0, RULE_statementSeparator = 1, RULE_emptyLines = 2, RULE_compilation = 3, 
		RULE_statement = 4, RULE_label = 5, RULE_scope = 6, RULE_blockScope = 7, 
		RULE_procedure = 8, RULE_struct = 9, RULE_enum = 10, RULE_call = 11, RULE_return = 12, 
		RULE_declare = 13, RULE_literal = 14, RULE_if = 15, RULE_exprThenBlock = 16, 
		RULE_thenBlock = 17, RULE_elseBlock = 18, RULE_elifBlock = 19, RULE_assignment = 20, 
		RULE_reference = 21, RULE_basicReference = 22, RULE_argumentsList = 23, 
		RULE_structureQualificationList = 24, RULE_structureQualification = 25, 
		RULE_arguments = 26, RULE_subscriptCommalist = 27, RULE_expression = 28, 
		RULE_primitiveExpression = 29, RULE_strLiteral = 30, RULE_numericLiteral = 31, 
		RULE_hexLiteral = 32, RULE_binLiteral = 33, RULE_octLiteral = 34, RULE_decLiteral = 35, 
		RULE_customLiteral = 36, RULE_parenthesizedExpression = 37, RULE_prefixExpression = 38, 
		RULE_dimensionSuffix = 39, RULE_boundPair = 40, RULE_boundPairCommalist = 41, 
		RULE_lowerBound = 42, RULE_upperBound = 43, RULE_logand = 44, RULE_logor = 45, 
		RULE_concatenate = 46, RULE_power = 47, RULE_shiftRotate = 48, RULE_addSub = 49, 
		RULE_mulDiv = 50, RULE_boolAnd = 51, RULE_boolXor = 52, RULE_boolOr = 53, 
		RULE_comparison = 54, RULE_prefixOperator = 55, RULE_structDefinition = 56, 
		RULE_qualifiedName = 57, RULE_paramList = 58, RULE_constArrayList = 59, 
		RULE_structName = 60, RULE_structMembers = 61, RULE_enumMembers = 62, 
		RULE_structMember = 63, RULE_structField = 64, RULE_structStruct = 65, 
		RULE_enumMember = 66, RULE_identifier = 67, RULE_typename = 68, RULE_unitType = 69, 
		RULE_binaryType = 70, RULE_decimalType = 71, RULE_stringType = 72, RULE_bitstringType = 73, 
		RULE_memberSeparator = 74, RULE_endOfFile = 75, RULE_keyword = 76;
	private static String[] makeRuleNames() {
		return new String[] {
			"preamble", "statementSeparator", "emptyLines", "compilation", "statement", 
			"label", "scope", "blockScope", "procedure", "struct", "enum", "call", 
			"return", "declare", "literal", "if", "exprThenBlock", "thenBlock", "elseBlock", 
			"elifBlock", "assignment", "reference", "basicReference", "argumentsList", 
			"structureQualificationList", "structureQualification", "arguments", 
			"subscriptCommalist", "expression", "primitiveExpression", "strLiteral", 
			"numericLiteral", "hexLiteral", "binLiteral", "octLiteral", "decLiteral", 
			"customLiteral", "parenthesizedExpression", "prefixExpression", "dimensionSuffix", 
			"boundPair", "boundPairCommalist", "lowerBound", "upperBound", "logand", 
			"logor", "concatenate", "power", "shiftRotate", "addSub", "mulDiv", "boolAnd", 
			"boolXor", "boolOr", "comparison", "prefixOperator", "structDefinition", 
			"qualifiedName", "paramList", "constArrayList", "structName", "structMembers", 
			"enumMembers", "structMember", "structField", "structStruct", "enumMember", 
			"identifier", "typename", "unitType", "binaryType", "decimalType", "stringType", 
			"bitstringType", "memberSeparator", "endOfFile", "keyword"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, null, null, null, null, null, "'as'", "'bin16'", "'bin32'", 
			"'bin64'", "'bin8'", "'bin'", "'bit'", "'call'", "'dcl'", "'dec'", "'def'", 
			"'elif'", "'else'", "'enum'", "'end'", "'for'", "'forever'", null, "'if'", 
			"'is'", null, "'path'", null, "'return'", "'scope'", "'string'", "'struct'", 
			"'then'", "'ubin16'", "'ubin32'", "'ubin64'", "'ubin8'", "'ubin'", "'udec'", 
			"'unit'", "'until'", "'while'", "':'", "'++'", "'&&'", "'||'", "'&'", 
			"'|'", "'~&'", "'~|'", null, null, "'~'", "'>'", "'<'", null, null, "'~>'", 
			"'~<'", null, null, null, "'+'", "'-'", "'*'", null, "'%'", "'\"'", "'&('", 
			"'|('", "'~|('", null, null, "'~&('", "'<<'", "'>>'", "'>>>'", null, 
			null, "'='", "'<-'", null, "'.'", "'@'", "';'", "','", "'('", "')'", 
			"'->'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "COMMENT", "HEX_LITERAL", "OCT_LITERAL", "DEC_LITERAL", "BIN_LITERAL", 
			"INTEGER", "AS", "BIN16", "BIN32", "BIN64", "BIN8", "BIN", "BIT", "CALL", 
			"DCL", "DEC", "DEF", "ELIF", "ELSE", "ENUM", "END", "FOR", "FOREVER", 
			"FUNC", "IF", "IS", "LIT", "PATH", "PROC", "RETURN", "SCOPE", "STRING", 
			"STRUCT", "THEN", "UBIN16", "UBIN32", "UBIN64", "UBIN8", "UBIN", "UDEC", 
			"UNIT", "UNTIL", "WHILE", "COLON", "CONC", "LOGAND", "LOGOR", "AND", 
			"OR", "NAND", "NOR", "XOR", "XNOR", "NOT", "GT", "LT", "GTE", "LTE", 
			"NGT", "NLT", "NE", "POWER", "STR_LITERAL", "PLUS", "MINUS", "TIMES", 
			"DIVIDE", "PCNT", "QUOTE", "REDAND", "REDOR", "REDNOR", "REDXOR", "REDXNOR", 
			"REDNAND", "L_LOG_SHIFT", "R_LOG_SHIFT", "R_ART_SHIFT", "L_ROTATE", "R_ROTATE", 
			"EQUALS", "ASSIGN", "COMPASSIGN", "DOT", "AT", "SEMICOLON", "COMMA", 
			"LPAR", "RPAR", "RARROW", "IDENTIFIER", "CUSTOM_LITERAL", "NEWLINE", 
			"WS"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "Syscode.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public SyscodeParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PreambleContext extends ParserRuleContext {
		public List<TerminalNode> NEWLINE() { return getTokens(SyscodeParser.NEWLINE); }
		public TerminalNode NEWLINE(int i) {
			return getToken(SyscodeParser.NEWLINE, i);
		}
		public List<TerminalNode> SEMICOLON() { return getTokens(SyscodeParser.SEMICOLON); }
		public TerminalNode SEMICOLON(int i) {
			return getToken(SyscodeParser.SEMICOLON, i);
		}
		public PreambleContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_preamble; }
	}

	public final PreambleContext preamble() throws RecognitionException {
		PreambleContext _localctx = new PreambleContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_preamble);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(155); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(154);
				_la = _input.LA(1);
				if ( !(_la==SEMICOLON || _la==NEWLINE) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
				}
				setState(157); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==SEMICOLON || _la==NEWLINE );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StatementSeparatorContext extends ParserRuleContext {
		public TerminalNode SEMICOLON() { return getToken(SyscodeParser.SEMICOLON, 0); }
		public TerminalNode NEWLINE() { return getToken(SyscodeParser.NEWLINE, 0); }
		public StatementSeparatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statementSeparator; }
	}

	public final StatementSeparatorContext statementSeparator() throws RecognitionException {
		StatementSeparatorContext _localctx = new StatementSeparatorContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_statementSeparator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(159);
			_la = _input.LA(1);
			if ( !(_la==SEMICOLON || _la==NEWLINE) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EmptyLinesContext extends ParserRuleContext {
		public List<TerminalNode> NEWLINE() { return getTokens(SyscodeParser.NEWLINE); }
		public TerminalNode NEWLINE(int i) {
			return getToken(SyscodeParser.NEWLINE, i);
		}
		public EmptyLinesContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_emptyLines; }
	}

	public final EmptyLinesContext emptyLines() throws RecognitionException {
		EmptyLinesContext _localctx = new EmptyLinesContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_emptyLines);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(162); 
			_errHandler.sync(this);
			_alt = 1;
			do {
				switch (_alt) {
				case 1:
					{
					{
					setState(161);
					match(NEWLINE);
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(164); 
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,1,_ctx);
			} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class CompilationContext extends ParserRuleContext {
		public EndOfFileContext endOfFile() {
			return getRuleContext(EndOfFileContext.class,0);
		}
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public CompilationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_compilation; }
	}

	public final CompilationContext compilation() throws RecognitionException {
		CompilationContext _localctx = new CompilationContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_compilation);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(169);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,2,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(166);
					statement();
					}
					} 
				}
				setState(171);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,2,_ctx);
			}
			setState(172);
			endOfFile();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StatementContext extends ParserRuleContext {
		public AssignmentContext assignment() {
			return getRuleContext(AssignmentContext.class,0);
		}
		public CallContext call() {
			return getRuleContext(CallContext.class,0);
		}
		public ReturnContext return_() {
			return getRuleContext(ReturnContext.class,0);
		}
		public LabelContext label() {
			return getRuleContext(LabelContext.class,0);
		}
		public ScopeContext scope() {
			return getRuleContext(ScopeContext.class,0);
		}
		public EnumContext enum_() {
			return getRuleContext(EnumContext.class,0);
		}
		public StructContext struct() {
			return getRuleContext(StructContext.class,0);
		}
		public IfContext if_() {
			return getRuleContext(IfContext.class,0);
		}
		public DeclareContext declare() {
			return getRuleContext(DeclareContext.class,0);
		}
		public LiteralContext literal() {
			return getRuleContext(LiteralContext.class,0);
		}
		public ProcedureContext procedure() {
			return getRuleContext(ProcedureContext.class,0);
		}
		public PreambleContext preamble() {
			return getRuleContext(PreambleContext.class,0);
		}
		public StatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement; }
	}

	public final StatementContext statement() throws RecognitionException {
		StatementContext _localctx = new StatementContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_statement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(175);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==SEMICOLON || _la==NEWLINE) {
				{
				setState(174);
				preamble();
				}
			}

			setState(188);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,4,_ctx) ) {
			case 1:
				{
				setState(177);
				assignment();
				}
				break;
			case 2:
				{
				setState(178);
				call();
				}
				break;
			case 3:
				{
				setState(179);
				return_();
				}
				break;
			case 4:
				{
				setState(180);
				label();
				}
				break;
			case 5:
				{
				setState(181);
				scope();
				}
				break;
			case 6:
				{
				setState(182);
				enum_();
				}
				break;
			case 7:
				{
				setState(183);
				struct();
				}
				break;
			case 8:
				{
				setState(184);
				if_();
				}
				break;
			case 9:
				{
				setState(185);
				declare();
				}
				break;
			case 10:
				{
				setState(186);
				literal();
				}
				break;
			case 11:
				{
				setState(187);
				procedure();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class LabelContext extends ParserRuleContext {
		public TerminalNode AT() { return getToken(SyscodeParser.AT, 0); }
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public StatementSeparatorContext statementSeparator() {
			return getRuleContext(StatementSeparatorContext.class,0);
		}
		public LabelContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_label; }
	}

	public final LabelContext label() throws RecognitionException {
		LabelContext _localctx = new LabelContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_label);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(190);
			match(AT);
			setState(191);
			identifier();
			setState(192);
			statementSeparator();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ScopeContext extends ParserRuleContext {
		public BlockScopeContext blockScope() {
			return getRuleContext(BlockScopeContext.class,0);
		}
		public ScopeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_scope; }
	}

	public final ScopeContext scope() throws RecognitionException {
		ScopeContext _localctx = new ScopeContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_scope);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(194);
			blockScope();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BlockScopeContext extends ParserRuleContext {
		public QualifiedNameContext Name;
		public TerminalNode SCOPE() { return getToken(SyscodeParser.SCOPE, 0); }
		public TerminalNode END() { return getToken(SyscodeParser.END, 0); }
		public QualifiedNameContext qualifiedName() {
			return getRuleContext(QualifiedNameContext.class,0);
		}
		public List<EmptyLinesContext> emptyLines() {
			return getRuleContexts(EmptyLinesContext.class);
		}
		public EmptyLinesContext emptyLines(int i) {
			return getRuleContext(EmptyLinesContext.class,i);
		}
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public BlockScopeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_blockScope; }
	}

	public final BlockScopeContext blockScope() throws RecognitionException {
		BlockScopeContext _localctx = new BlockScopeContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_blockScope);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(196);
			match(SCOPE);
			setState(198);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(197);
				emptyLines();
				}
			}

			setState(200);
			((BlockScopeContext)_localctx).Name = qualifiedName();
			setState(202);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,6,_ctx) ) {
			case 1:
				{
				setState(201);
				emptyLines();
				}
				break;
			}
			setState(207);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,7,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(204);
					statement();
					}
					} 
				}
				setState(209);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,7,_ctx);
			}
			setState(211);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(210);
				emptyLines();
				}
			}

			setState(213);
			match(END);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ProcedureContext extends ParserRuleContext {
		public IdentifierContext Spelling;
		public TerminalNode PROC() { return getToken(SyscodeParser.PROC, 0); }
		public TerminalNode END() { return getToken(SyscodeParser.END, 0); }
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public List<EmptyLinesContext> emptyLines() {
			return getRuleContexts(EmptyLinesContext.class);
		}
		public EmptyLinesContext emptyLines(int i) {
			return getRuleContext(EmptyLinesContext.class,i);
		}
		public ParamListContext paramList() {
			return getRuleContext(ParamListContext.class,0);
		}
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public ProcedureContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_procedure; }
	}

	public final ProcedureContext procedure() throws RecognitionException {
		ProcedureContext _localctx = new ProcedureContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_procedure);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(215);
			match(PROC);
			setState(217);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(216);
				emptyLines();
				}
			}

			setState(219);
			((ProcedureContext)_localctx).Spelling = identifier();
			setState(221);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LPAR) {
				{
				setState(220);
				paramList();
				}
			}

			setState(226);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,11,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(223);
					statement();
					}
					} 
				}
				setState(228);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,11,_ctx);
			}
			setState(230);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(229);
				emptyLines();
				}
			}

			setState(232);
			match(END);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StructContext extends ParserRuleContext {
		public TerminalNode STRUCT() { return getToken(SyscodeParser.STRUCT, 0); }
		public StructDefinitionContext structDefinition() {
			return getRuleContext(StructDefinitionContext.class,0);
		}
		public StructContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_struct; }
	}

	public final StructContext struct() throws RecognitionException {
		StructContext _localctx = new StructContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_struct);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(234);
			match(STRUCT);
			setState(235);
			structDefinition();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EnumContext extends ParserRuleContext {
		public IdentifierContext Name;
		public EnumMembersContext Members;
		public TerminalNode ENUM() { return getToken(SyscodeParser.ENUM, 0); }
		public MemberSeparatorContext memberSeparator() {
			return getRuleContext(MemberSeparatorContext.class,0);
		}
		public TerminalNode END() { return getToken(SyscodeParser.END, 0); }
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public EnumMembersContext enumMembers() {
			return getRuleContext(EnumMembersContext.class,0);
		}
		public List<EmptyLinesContext> emptyLines() {
			return getRuleContexts(EmptyLinesContext.class);
		}
		public EmptyLinesContext emptyLines(int i) {
			return getRuleContext(EmptyLinesContext.class,i);
		}
		public TypenameContext typename() {
			return getRuleContext(TypenameContext.class,0);
		}
		public EnumContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_enum; }
	}

	public final EnumContext enum_() throws RecognitionException {
		EnumContext _localctx = new EnumContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_enum);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(237);
			match(ENUM);
			setState(239);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(238);
				emptyLines();
				}
			}

			setState(241);
			((EnumContext)_localctx).Name = identifier();
			setState(243);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(242);
				emptyLines();
				}
			}

			setState(246);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 17591982620544L) != 0) || _la==IDENTIFIER) {
				{
				setState(245);
				typename();
				}
			}

			setState(248);
			memberSeparator();
			setState(250);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,16,_ctx) ) {
			case 1:
				{
				setState(249);
				emptyLines();
				}
				break;
			}
			setState(252);
			((EnumContext)_localctx).Members = enumMembers();
			setState(254);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(253);
				emptyLines();
				}
			}

			setState(256);
			match(END);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class CallContext extends ParserRuleContext {
		public TerminalNode CALL() { return getToken(SyscodeParser.CALL, 0); }
		public ReferenceContext reference() {
			return getRuleContext(ReferenceContext.class,0);
		}
		public StatementSeparatorContext statementSeparator() {
			return getRuleContext(StatementSeparatorContext.class,0);
		}
		public EmptyLinesContext emptyLines() {
			return getRuleContext(EmptyLinesContext.class,0);
		}
		public CallContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_call; }
	}

	public final CallContext call() throws RecognitionException {
		CallContext _localctx = new CallContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_call);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(258);
			match(CALL);
			setState(260);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(259);
				emptyLines();
				}
			}

			setState(262);
			reference(0);
			setState(263);
			statementSeparator();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ReturnContext extends ParserRuleContext {
		public TerminalNode RETURN() { return getToken(SyscodeParser.RETURN, 0); }
		public TerminalNode LPAR() { return getToken(SyscodeParser.LPAR, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode RPAR() { return getToken(SyscodeParser.RPAR, 0); }
		public EmptyLinesContext emptyLines() {
			return getRuleContext(EmptyLinesContext.class,0);
		}
		public StatementSeparatorContext statementSeparator() {
			return getRuleContext(StatementSeparatorContext.class,0);
		}
		public ReturnContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_return; }
	}

	public final ReturnContext return_() throws RecognitionException {
		ReturnContext _localctx = new ReturnContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_return);
		int _la;
		try {
			setState(283);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,23,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				{
				setState(265);
				match(RETURN);
				setState(273);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,20,_ctx) ) {
				case 1:
					{
					setState(267);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==NEWLINE) {
						{
						setState(266);
						emptyLines();
						}
					}

					setState(269);
					match(LPAR);
					setState(270);
					expression(0);
					setState(271);
					match(RPAR);
					}
					break;
				}
				}
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				{
				setState(275);
				match(RETURN);
				setState(280);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,22,_ctx) ) {
				case 1:
					{
					setState(277);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==NEWLINE) {
						{
						setState(276);
						emptyLines();
						}
					}

					setState(279);
					expression(0);
					}
					break;
				}
				}
				setState(282);
				statementSeparator();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DeclareContext extends ParserRuleContext {
		public IdentifierContext Spelling;
		public DimensionSuffixContext Bounds;
		public TerminalNode DCL() { return getToken(SyscodeParser.DCL, 0); }
		public TypenameContext typename() {
			return getRuleContext(TypenameContext.class,0);
		}
		public StatementSeparatorContext statementSeparator() {
			return getRuleContext(StatementSeparatorContext.class,0);
		}
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public List<EmptyLinesContext> emptyLines() {
			return getRuleContexts(EmptyLinesContext.class);
		}
		public EmptyLinesContext emptyLines(int i) {
			return getRuleContext(EmptyLinesContext.class,i);
		}
		public DimensionSuffixContext dimensionSuffix() {
			return getRuleContext(DimensionSuffixContext.class,0);
		}
		public DeclareContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_declare; }
	}

	public final DeclareContext declare() throws RecognitionException {
		DeclareContext _localctx = new DeclareContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_declare);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(285);
			match(DCL);
			setState(287);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(286);
				emptyLines();
				}
			}

			setState(289);
			((DeclareContext)_localctx).Spelling = identifier();
			setState(291);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,25,_ctx) ) {
			case 1:
				{
				setState(290);
				emptyLines();
				}
				break;
			}
			setState(294);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LPAR) {
				{
				setState(293);
				((DeclareContext)_localctx).Bounds = dimensionSuffix();
				}
			}

			setState(297);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(296);
				emptyLines();
				}
			}

			setState(299);
			typename();
			setState(300);
			statementSeparator();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class LiteralContext extends ParserRuleContext {
		public TerminalNode LIT() { return getToken(SyscodeParser.LIT, 0); }
		public CustomLiteralContext customLiteral() {
			return getRuleContext(CustomLiteralContext.class,0);
		}
		public TerminalNode AS() { return getToken(SyscodeParser.AS, 0); }
		public DecLiteralContext decLiteral() {
			return getRuleContext(DecLiteralContext.class,0);
		}
		public StatementSeparatorContext statementSeparator() {
			return getRuleContext(StatementSeparatorContext.class,0);
		}
		public LiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_literal; }
	}

	public final LiteralContext literal() throws RecognitionException {
		LiteralContext _localctx = new LiteralContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_literal);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(302);
			match(LIT);
			setState(303);
			customLiteral();
			setState(304);
			match(AS);
			setState(305);
			decLiteral();
			setState(306);
			statementSeparator();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class IfContext extends ParserRuleContext {
		public TerminalNode IF() { return getToken(SyscodeParser.IF, 0); }
		public ExprThenBlockContext exprThenBlock() {
			return getRuleContext(ExprThenBlockContext.class,0);
		}
		public TerminalNode END() { return getToken(SyscodeParser.END, 0); }
		public List<EmptyLinesContext> emptyLines() {
			return getRuleContexts(EmptyLinesContext.class);
		}
		public EmptyLinesContext emptyLines(int i) {
			return getRuleContext(EmptyLinesContext.class,i);
		}
		public ElifBlockContext elifBlock() {
			return getRuleContext(ElifBlockContext.class,0);
		}
		public ElseBlockContext elseBlock() {
			return getRuleContext(ElseBlockContext.class,0);
		}
		public IfContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_if; }
	}

	public final IfContext if_() throws RecognitionException {
		IfContext _localctx = new IfContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_if);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(308);
			match(IF);
			setState(310);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,28,_ctx) ) {
			case 1:
				{
				setState(309);
				emptyLines();
				}
				break;
			}
			setState(312);
			exprThenBlock();
			setState(314);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,29,_ctx) ) {
			case 1:
				{
				setState(313);
				emptyLines();
				}
				break;
			}
			setState(317);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ELIF) {
				{
				setState(316);
				elifBlock();
				}
			}

			setState(320);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,31,_ctx) ) {
			case 1:
				{
				setState(319);
				emptyLines();
				}
				break;
			}
			setState(323);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ELSE) {
				{
				setState(322);
				elseBlock();
				}
			}

			setState(326);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(325);
				emptyLines();
				}
			}

			setState(328);
			match(END);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ExprThenBlockContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode THEN() { return getToken(SyscodeParser.THEN, 0); }
		public ThenBlockContext thenBlock() {
			return getRuleContext(ThenBlockContext.class,0);
		}
		public List<EmptyLinesContext> emptyLines() {
			return getRuleContexts(EmptyLinesContext.class);
		}
		public EmptyLinesContext emptyLines(int i) {
			return getRuleContext(EmptyLinesContext.class,i);
		}
		public ExprThenBlockContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_exprThenBlock; }
	}

	public final ExprThenBlockContext exprThenBlock() throws RecognitionException {
		ExprThenBlockContext _localctx = new ExprThenBlockContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_exprThenBlock);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(331);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(330);
				emptyLines();
				}
			}

			setState(333);
			expression(0);
			setState(335);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(334);
				emptyLines();
				}
			}

			setState(337);
			match(THEN);
			setState(339);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,36,_ctx) ) {
			case 1:
				{
				setState(338);
				emptyLines();
				}
				break;
			}
			setState(341);
			thenBlock();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ThenBlockContext extends ParserRuleContext {
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public ThenBlockContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_thenBlock; }
	}

	public final ThenBlockContext thenBlock() throws RecognitionException {
		ThenBlockContext _localctx = new ThenBlockContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_thenBlock);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(346);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,37,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(343);
					statement();
					}
					} 
				}
				setState(348);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,37,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ElseBlockContext extends ParserRuleContext {
		public TerminalNode ELSE() { return getToken(SyscodeParser.ELSE, 0); }
		public ThenBlockContext thenBlock() {
			return getRuleContext(ThenBlockContext.class,0);
		}
		public EmptyLinesContext emptyLines() {
			return getRuleContext(EmptyLinesContext.class,0);
		}
		public ElseBlockContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_elseBlock; }
	}

	public final ElseBlockContext elseBlock() throws RecognitionException {
		ElseBlockContext _localctx = new ElseBlockContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_elseBlock);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(349);
			match(ELSE);
			setState(351);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,38,_ctx) ) {
			case 1:
				{
				setState(350);
				emptyLines();
				}
				break;
			}
			setState(353);
			thenBlock();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ElifBlockContext extends ParserRuleContext {
		public List<TerminalNode> ELIF() { return getTokens(SyscodeParser.ELIF); }
		public TerminalNode ELIF(int i) {
			return getToken(SyscodeParser.ELIF, i);
		}
		public List<ExprThenBlockContext> exprThenBlock() {
			return getRuleContexts(ExprThenBlockContext.class);
		}
		public ExprThenBlockContext exprThenBlock(int i) {
			return getRuleContext(ExprThenBlockContext.class,i);
		}
		public List<EmptyLinesContext> emptyLines() {
			return getRuleContexts(EmptyLinesContext.class);
		}
		public EmptyLinesContext emptyLines(int i) {
			return getRuleContext(EmptyLinesContext.class,i);
		}
		public ElifBlockContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_elifBlock; }
	}

	public final ElifBlockContext elifBlock() throws RecognitionException {
		ElifBlockContext _localctx = new ElifBlockContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_elifBlock);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(360); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(355);
				match(ELIF);
				setState(357);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,39,_ctx) ) {
				case 1:
					{
					setState(356);
					emptyLines();
					}
					break;
				}
				setState(359);
				exprThenBlock();
				}
				}
				setState(362); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==ELIF );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class AssignmentContext extends ParserRuleContext {
		public ReferenceContext Target;
		public ExpressionContext Source;
		public StatementSeparatorContext statementSeparator() {
			return getRuleContext(StatementSeparatorContext.class,0);
		}
		public ReferenceContext reference() {
			return getRuleContext(ReferenceContext.class,0);
		}
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode EQUALS() { return getToken(SyscodeParser.EQUALS, 0); }
		public AssignmentContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_assignment; }
	}

	public final AssignmentContext assignment() throws RecognitionException {
		AssignmentContext _localctx = new AssignmentContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_assignment);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(364);
			((AssignmentContext)_localctx).Target = reference(0);
			{
			setState(365);
			match(EQUALS);
			}
			setState(366);
			((AssignmentContext)_localctx).Source = expression(0);
			setState(367);
			statementSeparator();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ReferenceContext extends ParserRuleContext {
		public BasicReferenceContext basicReference() {
			return getRuleContext(BasicReferenceContext.class,0);
		}
		public ArgumentsListContext argumentsList() {
			return getRuleContext(ArgumentsListContext.class,0);
		}
		public ReferenceContext reference() {
			return getRuleContext(ReferenceContext.class,0);
		}
		public TerminalNode RARROW() { return getToken(SyscodeParser.RARROW, 0); }
		public ReferenceContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_reference; }
	}

	public final ReferenceContext reference() throws RecognitionException {
		return reference(0);
	}

	private ReferenceContext reference(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ReferenceContext _localctx = new ReferenceContext(_ctx, _parentState);
		ReferenceContext _prevctx = _localctx;
		int _startState = 42;
		enterRecursionRule(_localctx, 42, RULE_reference, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(370);
			basicReference();
			setState(372);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,41,_ctx) ) {
			case 1:
				{
				setState(371);
				argumentsList();
				}
				break;
			}
			}
			_ctx.stop = _input.LT(-1);
			setState(382);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,43,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new ReferenceContext(_parentctx, _parentState);
					pushNewRecursionContext(_localctx, _startState, RULE_reference);
					setState(374);
					if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
					setState(375);
					match(RARROW);
					setState(376);
					basicReference();
					setState(378);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,42,_ctx) ) {
					case 1:
						{
						setState(377);
						argumentsList();
						}
						break;
					}
					}
					} 
				}
				setState(384);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,43,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BasicReferenceContext extends ParserRuleContext {
		public IdentifierContext Spelling;
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public StructureQualificationListContext structureQualificationList() {
			return getRuleContext(StructureQualificationListContext.class,0);
		}
		public BasicReferenceContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_basicReference; }
	}

	public final BasicReferenceContext basicReference() throws RecognitionException {
		BasicReferenceContext _localctx = new BasicReferenceContext(_ctx, getState());
		enterRule(_localctx, 44, RULE_basicReference);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(386);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,44,_ctx) ) {
			case 1:
				{
				setState(385);
				structureQualificationList();
				}
				break;
			}
			setState(388);
			((BasicReferenceContext)_localctx).Spelling = identifier();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ArgumentsListContext extends ParserRuleContext {
		public List<ArgumentsContext> arguments() {
			return getRuleContexts(ArgumentsContext.class);
		}
		public ArgumentsContext arguments(int i) {
			return getRuleContext(ArgumentsContext.class,i);
		}
		public ArgumentsListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_argumentsList; }
	}

	public final ArgumentsListContext argumentsList() throws RecognitionException {
		ArgumentsListContext _localctx = new ArgumentsListContext(_ctx, getState());
		enterRule(_localctx, 46, RULE_argumentsList);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(391); 
			_errHandler.sync(this);
			_alt = 1;
			do {
				switch (_alt) {
				case 1:
					{
					{
					setState(390);
					arguments();
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(393); 
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,45,_ctx);
			} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StructureQualificationListContext extends ParserRuleContext {
		public List<StructureQualificationContext> structureQualification() {
			return getRuleContexts(StructureQualificationContext.class);
		}
		public StructureQualificationContext structureQualification(int i) {
			return getRuleContext(StructureQualificationContext.class,i);
		}
		public StructureQualificationListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_structureQualificationList; }
	}

	public final StructureQualificationListContext structureQualificationList() throws RecognitionException {
		StructureQualificationListContext _localctx = new StructureQualificationListContext(_ctx, getState());
		enterRule(_localctx, 48, RULE_structureQualificationList);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(396); 
			_errHandler.sync(this);
			_alt = 1;
			do {
				switch (_alt) {
				case 1:
					{
					{
					setState(395);
					structureQualification();
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(398); 
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,46,_ctx);
			} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StructureQualificationContext extends ParserRuleContext {
		public IdentifierContext Spelling;
		public TerminalNode DOT() { return getToken(SyscodeParser.DOT, 0); }
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public ArgumentsContext arguments() {
			return getRuleContext(ArgumentsContext.class,0);
		}
		public StructureQualificationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_structureQualification; }
	}

	public final StructureQualificationContext structureQualification() throws RecognitionException {
		StructureQualificationContext _localctx = new StructureQualificationContext(_ctx, getState());
		enterRule(_localctx, 50, RULE_structureQualification);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(400);
			((StructureQualificationContext)_localctx).Spelling = identifier();
			setState(402);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LPAR) {
				{
				setState(401);
				arguments();
				}
			}

			setState(404);
			match(DOT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ArgumentsContext extends ParserRuleContext {
		public TerminalNode LPAR() { return getToken(SyscodeParser.LPAR, 0); }
		public TerminalNode RPAR() { return getToken(SyscodeParser.RPAR, 0); }
		public SubscriptCommalistContext subscriptCommalist() {
			return getRuleContext(SubscriptCommalistContext.class,0);
		}
		public ArgumentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arguments; }
	}

	public final ArgumentsContext arguments() throws RecognitionException {
		ArgumentsContext _localctx = new ArgumentsContext(_ctx, getState());
		enterRule(_localctx, 52, RULE_arguments);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(406);
			match(LPAR);
			setState(408);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & -9205340046362673156L) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & 419434435L) != 0)) {
				{
				setState(407);
				subscriptCommalist();
				}
			}

			setState(410);
			match(RPAR);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SubscriptCommalistContext extends ParserRuleContext {
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(SyscodeParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(SyscodeParser.COMMA, i);
		}
		public SubscriptCommalistContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_subscriptCommalist; }
	}

	public final SubscriptCommalistContext subscriptCommalist() throws RecognitionException {
		SubscriptCommalistContext _localctx = new SubscriptCommalistContext(_ctx, getState());
		enterRule(_localctx, 54, RULE_subscriptCommalist);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(412);
			expression(0);
			setState(417);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(413);
				match(COMMA);
				setState(414);
				expression(0);
				}
				}
				setState(419);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ExpressionContext extends ParserRuleContext {
		public ExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expression; }
	 
		public ExpressionContext() { }
		public void copyFrom(ExpressionContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ExprParenthesizedContext extends ExpressionContext {
		public ParenthesizedExpressionContext parenthesizedExpression() {
			return getRuleContext(ParenthesizedExpressionContext.class,0);
		}
		public ExprParenthesizedContext(ExpressionContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ExprBinaryContext extends ExpressionContext {
		public ExpressionContext Left;
		public ExpressionContext Rite;
		public PowerContext power() {
			return getRuleContext(PowerContext.class,0);
		}
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public List<EmptyLinesContext> emptyLines() {
			return getRuleContexts(EmptyLinesContext.class);
		}
		public EmptyLinesContext emptyLines(int i) {
			return getRuleContext(EmptyLinesContext.class,i);
		}
		public MulDivContext mulDiv() {
			return getRuleContext(MulDivContext.class,0);
		}
		public AddSubContext addSub() {
			return getRuleContext(AddSubContext.class,0);
		}
		public ShiftRotateContext shiftRotate() {
			return getRuleContext(ShiftRotateContext.class,0);
		}
		public ConcatenateContext concatenate() {
			return getRuleContext(ConcatenateContext.class,0);
		}
		public ComparisonContext comparison() {
			return getRuleContext(ComparisonContext.class,0);
		}
		public BoolAndContext boolAnd() {
			return getRuleContext(BoolAndContext.class,0);
		}
		public BoolXorContext boolXor() {
			return getRuleContext(BoolXorContext.class,0);
		}
		public BoolOrContext boolOr() {
			return getRuleContext(BoolOrContext.class,0);
		}
		public LogandContext logand() {
			return getRuleContext(LogandContext.class,0);
		}
		public LogorContext logor() {
			return getRuleContext(LogorContext.class,0);
		}
		public ExprBinaryContext(ExpressionContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ExprPrefixedContext extends ExpressionContext {
		public PrefixExpressionContext prefixExpression() {
			return getRuleContext(PrefixExpressionContext.class,0);
		}
		public ExprPrefixedContext(ExpressionContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ExprPrimitiveContext extends ExpressionContext {
		public PrimitiveExpressionContext primitiveExpression() {
			return getRuleContext(PrimitiveExpressionContext.class,0);
		}
		public ExprPrimitiveContext(ExpressionContext ctx) { copyFrom(ctx); }
	}

	public final ExpressionContext expression() throws RecognitionException {
		return expression(0);
	}

	private ExpressionContext expression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ExpressionContext _localctx = new ExpressionContext(_ctx, _parentState);
		ExpressionContext _prevctx = _localctx;
		int _startState = 56;
		enterRecursionRule(_localctx, 56, RULE_expression, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(424);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case HEX_LITERAL:
			case OCT_LITERAL:
			case DEC_LITERAL:
			case BIN_LITERAL:
			case INTEGER:
			case AS:
			case BIN16:
			case BIN32:
			case BIN64:
			case BIN8:
			case BIN:
			case BIT:
			case CALL:
			case DCL:
			case DEC:
			case DEF:
			case ELIF:
			case ELSE:
			case ENUM:
			case FOR:
			case FOREVER:
			case FUNC:
			case IF:
			case PATH:
			case PROC:
			case RETURN:
			case SCOPE:
			case STRING:
			case STRUCT:
			case THEN:
			case UBIN16:
			case UBIN32:
			case UBIN64:
			case UBIN8:
			case UBIN:
			case UDEC:
			case UNIT:
			case UNTIL:
			case WHILE:
			case STR_LITERAL:
			case IDENTIFIER:
			case CUSTOM_LITERAL:
				{
				_localctx = new ExprPrimitiveContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;

				setState(421);
				primitiveExpression();
				}
				break;
			case REDAND:
			case REDOR:
			case REDNOR:
			case REDXOR:
			case REDXNOR:
			case REDNAND:
			case LPAR:
				{
				_localctx = new ExprParenthesizedContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(422);
				parenthesizedExpression();
				}
				break;
			case NOT:
			case PLUS:
			case MINUS:
				{
				_localctx = new ExprPrefixedContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(423);
				prefixExpression();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			_ctx.stop = _input.LT(-1);
			setState(538);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,74,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(536);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,73,_ctx) ) {
					case 1:
						{
						_localctx = new ExprBinaryContext(new ExpressionContext(_parentctx, _parentState));
						((ExprBinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(426);
						if (!(precpred(_ctx, 11))) throw new FailedPredicateException(this, "precpred(_ctx, 11)");
						setState(428);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(427);
							emptyLines();
							}
						}

						setState(430);
						power();
						setState(432);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(431);
							emptyLines();
							}
						}

						setState(434);
						((ExprBinaryContext)_localctx).Rite = expression(11);
						}
						break;
					case 2:
						{
						_localctx = new ExprBinaryContext(new ExpressionContext(_parentctx, _parentState));
						((ExprBinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(436);
						if (!(precpred(_ctx, 10))) throw new FailedPredicateException(this, "precpred(_ctx, 10)");
						setState(438);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(437);
							emptyLines();
							}
						}

						setState(440);
						mulDiv();
						setState(442);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(441);
							emptyLines();
							}
						}

						setState(444);
						((ExprBinaryContext)_localctx).Rite = expression(11);
						}
						break;
					case 3:
						{
						_localctx = new ExprBinaryContext(new ExpressionContext(_parentctx, _parentState));
						((ExprBinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(446);
						if (!(precpred(_ctx, 9))) throw new FailedPredicateException(this, "precpred(_ctx, 9)");
						setState(448);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(447);
							emptyLines();
							}
						}

						setState(450);
						addSub();
						setState(452);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(451);
							emptyLines();
							}
						}

						setState(454);
						((ExprBinaryContext)_localctx).Rite = expression(10);
						}
						break;
					case 4:
						{
						_localctx = new ExprBinaryContext(new ExpressionContext(_parentctx, _parentState));
						((ExprBinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(456);
						if (!(precpred(_ctx, 8))) throw new FailedPredicateException(this, "precpred(_ctx, 8)");
						setState(458);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(457);
							emptyLines();
							}
						}

						setState(460);
						shiftRotate();
						setState(462);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(461);
							emptyLines();
							}
						}

						setState(464);
						((ExprBinaryContext)_localctx).Rite = expression(9);
						}
						break;
					case 5:
						{
						_localctx = new ExprBinaryContext(new ExpressionContext(_parentctx, _parentState));
						((ExprBinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(466);
						if (!(precpred(_ctx, 7))) throw new FailedPredicateException(this, "precpred(_ctx, 7)");
						setState(468);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(467);
							emptyLines();
							}
						}

						setState(470);
						concatenate();
						setState(472);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(471);
							emptyLines();
							}
						}

						setState(474);
						((ExprBinaryContext)_localctx).Rite = expression(8);
						}
						break;
					case 6:
						{
						_localctx = new ExprBinaryContext(new ExpressionContext(_parentctx, _parentState));
						((ExprBinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(476);
						if (!(precpred(_ctx, 6))) throw new FailedPredicateException(this, "precpred(_ctx, 6)");
						setState(478);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(477);
							emptyLines();
							}
						}

						setState(480);
						comparison();
						setState(482);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(481);
							emptyLines();
							}
						}

						setState(484);
						((ExprBinaryContext)_localctx).Rite = expression(7);
						}
						break;
					case 7:
						{
						_localctx = new ExprBinaryContext(new ExpressionContext(_parentctx, _parentState));
						((ExprBinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(486);
						if (!(precpred(_ctx, 5))) throw new FailedPredicateException(this, "precpred(_ctx, 5)");
						setState(488);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(487);
							emptyLines();
							}
						}

						setState(490);
						boolAnd();
						setState(492);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(491);
							emptyLines();
							}
						}

						setState(494);
						((ExprBinaryContext)_localctx).Rite = expression(6);
						}
						break;
					case 8:
						{
						_localctx = new ExprBinaryContext(new ExpressionContext(_parentctx, _parentState));
						((ExprBinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(496);
						if (!(precpred(_ctx, 4))) throw new FailedPredicateException(this, "precpred(_ctx, 4)");
						setState(498);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(497);
							emptyLines();
							}
						}

						setState(500);
						boolXor();
						setState(502);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(501);
							emptyLines();
							}
						}

						setState(504);
						((ExprBinaryContext)_localctx).Rite = expression(5);
						}
						break;
					case 9:
						{
						_localctx = new ExprBinaryContext(new ExpressionContext(_parentctx, _parentState));
						((ExprBinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(506);
						if (!(precpred(_ctx, 3))) throw new FailedPredicateException(this, "precpred(_ctx, 3)");
						setState(508);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(507);
							emptyLines();
							}
						}

						setState(510);
						boolOr();
						setState(512);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(511);
							emptyLines();
							}
						}

						setState(514);
						((ExprBinaryContext)_localctx).Rite = expression(4);
						}
						break;
					case 10:
						{
						_localctx = new ExprBinaryContext(new ExpressionContext(_parentctx, _parentState));
						((ExprBinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(516);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(518);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(517);
							emptyLines();
							}
						}

						setState(520);
						logand();
						setState(522);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(521);
							emptyLines();
							}
						}

						setState(524);
						((ExprBinaryContext)_localctx).Rite = expression(3);
						}
						break;
					case 11:
						{
						_localctx = new ExprBinaryContext(new ExpressionContext(_parentctx, _parentState));
						((ExprBinaryContext)_localctx).Left = _prevctx;
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(526);
						if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
						setState(528);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(527);
							emptyLines();
							}
						}

						setState(530);
						logor();
						setState(532);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==NEWLINE) {
							{
							setState(531);
							emptyLines();
							}
						}

						setState(534);
						((ExprBinaryContext)_localctx).Rite = expression(2);
						}
						break;
					}
					} 
				}
				setState(540);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,74,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PrimitiveExpressionContext extends ParserRuleContext {
		public NumericLiteralContext numericLiteral() {
			return getRuleContext(NumericLiteralContext.class,0);
		}
		public StrLiteralContext strLiteral() {
			return getRuleContext(StrLiteralContext.class,0);
		}
		public CustomLiteralContext customLiteral() {
			return getRuleContext(CustomLiteralContext.class,0);
		}
		public ReferenceContext reference() {
			return getRuleContext(ReferenceContext.class,0);
		}
		public PrimitiveExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_primitiveExpression; }
	}

	public final PrimitiveExpressionContext primitiveExpression() throws RecognitionException {
		PrimitiveExpressionContext _localctx = new PrimitiveExpressionContext(_ctx, getState());
		enterRule(_localctx, 58, RULE_primitiveExpression);
		try {
			setState(545);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case HEX_LITERAL:
			case OCT_LITERAL:
			case DEC_LITERAL:
			case BIN_LITERAL:
			case INTEGER:
				enterOuterAlt(_localctx, 1);
				{
				setState(541);
				numericLiteral();
				}
				break;
			case STR_LITERAL:
				enterOuterAlt(_localctx, 2);
				{
				setState(542);
				strLiteral();
				}
				break;
			case CUSTOM_LITERAL:
				enterOuterAlt(_localctx, 3);
				{
				setState(543);
				customLiteral();
				}
				break;
			case AS:
			case BIN16:
			case BIN32:
			case BIN64:
			case BIN8:
			case BIN:
			case BIT:
			case CALL:
			case DCL:
			case DEC:
			case DEF:
			case ELIF:
			case ELSE:
			case ENUM:
			case FOR:
			case FOREVER:
			case FUNC:
			case IF:
			case PATH:
			case PROC:
			case RETURN:
			case SCOPE:
			case STRING:
			case STRUCT:
			case THEN:
			case UBIN16:
			case UBIN32:
			case UBIN64:
			case UBIN8:
			case UBIN:
			case UDEC:
			case UNIT:
			case UNTIL:
			case WHILE:
			case IDENTIFIER:
				enterOuterAlt(_localctx, 4);
				{
				setState(544);
				reference(0);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StrLiteralContext extends ParserRuleContext {
		public TerminalNode STR_LITERAL() { return getToken(SyscodeParser.STR_LITERAL, 0); }
		public StrLiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_strLiteral; }
	}

	public final StrLiteralContext strLiteral() throws RecognitionException {
		StrLiteralContext _localctx = new StrLiteralContext(_ctx, getState());
		enterRule(_localctx, 60, RULE_strLiteral);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(547);
			match(STR_LITERAL);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class NumericLiteralContext extends ParserRuleContext {
		public BinLiteralContext binLiteral() {
			return getRuleContext(BinLiteralContext.class,0);
		}
		public OctLiteralContext octLiteral() {
			return getRuleContext(OctLiteralContext.class,0);
		}
		public HexLiteralContext hexLiteral() {
			return getRuleContext(HexLiteralContext.class,0);
		}
		public DecLiteralContext decLiteral() {
			return getRuleContext(DecLiteralContext.class,0);
		}
		public NumericLiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_numericLiteral; }
	}

	public final NumericLiteralContext numericLiteral() throws RecognitionException {
		NumericLiteralContext _localctx = new NumericLiteralContext(_ctx, getState());
		enterRule(_localctx, 62, RULE_numericLiteral);
		try {
			setState(553);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case BIN_LITERAL:
				enterOuterAlt(_localctx, 1);
				{
				setState(549);
				binLiteral();
				}
				break;
			case OCT_LITERAL:
				enterOuterAlt(_localctx, 2);
				{
				setState(550);
				octLiteral();
				}
				break;
			case HEX_LITERAL:
				enterOuterAlt(_localctx, 3);
				{
				setState(551);
				hexLiteral();
				}
				break;
			case DEC_LITERAL:
			case INTEGER:
				enterOuterAlt(_localctx, 4);
				{
				setState(552);
				decLiteral();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class HexLiteralContext extends ParserRuleContext {
		public TerminalNode HEX_LITERAL() { return getToken(SyscodeParser.HEX_LITERAL, 0); }
		public HexLiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_hexLiteral; }
	}

	public final HexLiteralContext hexLiteral() throws RecognitionException {
		HexLiteralContext _localctx = new HexLiteralContext(_ctx, getState());
		enterRule(_localctx, 64, RULE_hexLiteral);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(555);
			match(HEX_LITERAL);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BinLiteralContext extends ParserRuleContext {
		public TerminalNode BIN_LITERAL() { return getToken(SyscodeParser.BIN_LITERAL, 0); }
		public BinLiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_binLiteral; }
	}

	public final BinLiteralContext binLiteral() throws RecognitionException {
		BinLiteralContext _localctx = new BinLiteralContext(_ctx, getState());
		enterRule(_localctx, 66, RULE_binLiteral);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(557);
			match(BIN_LITERAL);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class OctLiteralContext extends ParserRuleContext {
		public TerminalNode OCT_LITERAL() { return getToken(SyscodeParser.OCT_LITERAL, 0); }
		public OctLiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_octLiteral; }
	}

	public final OctLiteralContext octLiteral() throws RecognitionException {
		OctLiteralContext _localctx = new OctLiteralContext(_ctx, getState());
		enterRule(_localctx, 68, RULE_octLiteral);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(559);
			match(OCT_LITERAL);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DecLiteralContext extends ParserRuleContext {
		public TerminalNode INTEGER() { return getToken(SyscodeParser.INTEGER, 0); }
		public TerminalNode DEC_LITERAL() { return getToken(SyscodeParser.DEC_LITERAL, 0); }
		public DecLiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_decLiteral; }
	}

	public final DecLiteralContext decLiteral() throws RecognitionException {
		DecLiteralContext _localctx = new DecLiteralContext(_ctx, getState());
		enterRule(_localctx, 70, RULE_decLiteral);
		try {
			setState(563);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case INTEGER:
				enterOuterAlt(_localctx, 1);
				{
				{
				setState(561);
				match(INTEGER);
				}
				}
				break;
			case DEC_LITERAL:
				enterOuterAlt(_localctx, 2);
				{
				{
				setState(562);
				match(DEC_LITERAL);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class CustomLiteralContext extends ParserRuleContext {
		public TerminalNode CUSTOM_LITERAL() { return getToken(SyscodeParser.CUSTOM_LITERAL, 0); }
		public CustomLiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_customLiteral; }
	}

	public final CustomLiteralContext customLiteral() throws RecognitionException {
		CustomLiteralContext _localctx = new CustomLiteralContext(_ctx, getState());
		enterRule(_localctx, 72, RULE_customLiteral);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(565);
			match(CUSTOM_LITERAL);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ParenthesizedExpressionContext extends ParserRuleContext {
		public TerminalNode LPAR() { return getToken(SyscodeParser.LPAR, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode RPAR() { return getToken(SyscodeParser.RPAR, 0); }
		public TerminalNode REDAND() { return getToken(SyscodeParser.REDAND, 0); }
		public TerminalNode REDOR() { return getToken(SyscodeParser.REDOR, 0); }
		public TerminalNode REDXOR() { return getToken(SyscodeParser.REDXOR, 0); }
		public TerminalNode REDNAND() { return getToken(SyscodeParser.REDNAND, 0); }
		public TerminalNode REDNOR() { return getToken(SyscodeParser.REDNOR, 0); }
		public TerminalNode REDXNOR() { return getToken(SyscodeParser.REDXNOR, 0); }
		public ParenthesizedExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_parenthesizedExpression; }
	}

	public final ParenthesizedExpressionContext parenthesizedExpression() throws RecognitionException {
		ParenthesizedExpressionContext _localctx = new ParenthesizedExpressionContext(_ctx, getState());
		enterRule(_localctx, 74, RULE_parenthesizedExpression);
		try {
			setState(595);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LPAR:
				enterOuterAlt(_localctx, 1);
				{
				setState(567);
				match(LPAR);
				setState(568);
				expression(0);
				setState(569);
				match(RPAR);
				}
				break;
			case REDAND:
				enterOuterAlt(_localctx, 2);
				{
				setState(571);
				match(REDAND);
				setState(572);
				expression(0);
				setState(573);
				match(RPAR);
				}
				break;
			case REDOR:
				enterOuterAlt(_localctx, 3);
				{
				setState(575);
				match(REDOR);
				setState(576);
				expression(0);
				setState(577);
				match(RPAR);
				}
				break;
			case REDXOR:
				enterOuterAlt(_localctx, 4);
				{
				setState(579);
				match(REDXOR);
				setState(580);
				expression(0);
				setState(581);
				match(RPAR);
				}
				break;
			case REDNAND:
				enterOuterAlt(_localctx, 5);
				{
				setState(583);
				match(REDNAND);
				setState(584);
				expression(0);
				setState(585);
				match(RPAR);
				}
				break;
			case REDNOR:
				enterOuterAlt(_localctx, 6);
				{
				setState(587);
				match(REDNOR);
				setState(588);
				expression(0);
				setState(589);
				match(RPAR);
				}
				break;
			case REDXNOR:
				enterOuterAlt(_localctx, 7);
				{
				setState(591);
				match(REDXNOR);
				setState(592);
				expression(0);
				setState(593);
				match(RPAR);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PrefixExpressionContext extends ParserRuleContext {
		public PrefixOperatorContext prefixOperator() {
			return getRuleContext(PrefixOperatorContext.class,0);
		}
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public PrefixExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_prefixExpression; }
	}

	public final PrefixExpressionContext prefixExpression() throws RecognitionException {
		PrefixExpressionContext _localctx = new PrefixExpressionContext(_ctx, getState());
		enterRule(_localctx, 76, RULE_prefixExpression);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(597);
			prefixOperator();
			setState(598);
			expression(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DimensionSuffixContext extends ParserRuleContext {
		public TerminalNode LPAR() { return getToken(SyscodeParser.LPAR, 0); }
		public BoundPairCommalistContext boundPairCommalist() {
			return getRuleContext(BoundPairCommalistContext.class,0);
		}
		public TerminalNode RPAR() { return getToken(SyscodeParser.RPAR, 0); }
		public DimensionSuffixContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_dimensionSuffix; }
	}

	public final DimensionSuffixContext dimensionSuffix() throws RecognitionException {
		DimensionSuffixContext _localctx = new DimensionSuffixContext(_ctx, getState());
		enterRule(_localctx, 78, RULE_dimensionSuffix);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(600);
			match(LPAR);
			setState(601);
			boundPairCommalist();
			setState(602);
			match(RPAR);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BoundPairContext extends ParserRuleContext {
		public UpperBoundContext upperBound() {
			return getRuleContext(UpperBoundContext.class,0);
		}
		public LowerBoundContext lowerBound() {
			return getRuleContext(LowerBoundContext.class,0);
		}
		public TerminalNode COLON() { return getToken(SyscodeParser.COLON, 0); }
		public TerminalNode TIMES() { return getToken(SyscodeParser.TIMES, 0); }
		public BoundPairContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_boundPair; }
	}

	public final BoundPairContext boundPair() throws RecognitionException {
		BoundPairContext _localctx = new BoundPairContext(_ctx, getState());
		enterRule(_localctx, 80, RULE_boundPair);
		try {
			setState(611);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case HEX_LITERAL:
			case OCT_LITERAL:
			case DEC_LITERAL:
			case BIN_LITERAL:
			case INTEGER:
			case AS:
			case BIN16:
			case BIN32:
			case BIN64:
			case BIN8:
			case BIN:
			case BIT:
			case CALL:
			case DCL:
			case DEC:
			case DEF:
			case ELIF:
			case ELSE:
			case ENUM:
			case FOR:
			case FOREVER:
			case FUNC:
			case IF:
			case PATH:
			case PROC:
			case RETURN:
			case SCOPE:
			case STRING:
			case STRUCT:
			case THEN:
			case UBIN16:
			case UBIN32:
			case UBIN64:
			case UBIN8:
			case UBIN:
			case UDEC:
			case UNIT:
			case UNTIL:
			case WHILE:
			case NOT:
			case STR_LITERAL:
			case PLUS:
			case MINUS:
			case REDAND:
			case REDOR:
			case REDNOR:
			case REDXOR:
			case REDXNOR:
			case REDNAND:
			case LPAR:
			case IDENTIFIER:
			case CUSTOM_LITERAL:
				enterOuterAlt(_localctx, 1);
				{
				setState(607);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,79,_ctx) ) {
				case 1:
					{
					setState(604);
					lowerBound();
					setState(605);
					match(COLON);
					}
					break;
				}
				setState(609);
				upperBound();
				}
				break;
			case TIMES:
				enterOuterAlt(_localctx, 2);
				{
				setState(610);
				match(TIMES);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BoundPairCommalistContext extends ParserRuleContext {
		public List<BoundPairContext> boundPair() {
			return getRuleContexts(BoundPairContext.class);
		}
		public BoundPairContext boundPair(int i) {
			return getRuleContext(BoundPairContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(SyscodeParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(SyscodeParser.COMMA, i);
		}
		public BoundPairCommalistContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_boundPairCommalist; }
	}

	public final BoundPairCommalistContext boundPairCommalist() throws RecognitionException {
		BoundPairCommalistContext _localctx = new BoundPairCommalistContext(_ctx, getState());
		enterRule(_localctx, 82, RULE_boundPairCommalist);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(613);
			boundPair();
			setState(618);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(614);
				match(COMMA);
				setState(615);
				boundPair();
				}
				}
				setState(620);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class LowerBoundContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public LowerBoundContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_lowerBound; }
	}

	public final LowerBoundContext lowerBound() throws RecognitionException {
		LowerBoundContext _localctx = new LowerBoundContext(_ctx, getState());
		enterRule(_localctx, 84, RULE_lowerBound);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(621);
			expression(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class UpperBoundContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public UpperBoundContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_upperBound; }
	}

	public final UpperBoundContext upperBound() throws RecognitionException {
		UpperBoundContext _localctx = new UpperBoundContext(_ctx, getState());
		enterRule(_localctx, 86, RULE_upperBound);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(623);
			expression(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class LogandContext extends ParserRuleContext {
		public TerminalNode LOGAND() { return getToken(SyscodeParser.LOGAND, 0); }
		public LogandContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_logand; }
	}

	public final LogandContext logand() throws RecognitionException {
		LogandContext _localctx = new LogandContext(_ctx, getState());
		enterRule(_localctx, 88, RULE_logand);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(625);
			match(LOGAND);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class LogorContext extends ParserRuleContext {
		public TerminalNode LOGOR() { return getToken(SyscodeParser.LOGOR, 0); }
		public LogorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_logor; }
	}

	public final LogorContext logor() throws RecognitionException {
		LogorContext _localctx = new LogorContext(_ctx, getState());
		enterRule(_localctx, 90, RULE_logor);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(627);
			match(LOGOR);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ConcatenateContext extends ParserRuleContext {
		public TerminalNode CONC() { return getToken(SyscodeParser.CONC, 0); }
		public ConcatenateContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_concatenate; }
	}

	public final ConcatenateContext concatenate() throws RecognitionException {
		ConcatenateContext _localctx = new ConcatenateContext(_ctx, getState());
		enterRule(_localctx, 92, RULE_concatenate);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(629);
			match(CONC);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PowerContext extends ParserRuleContext {
		public TerminalNode POWER() { return getToken(SyscodeParser.POWER, 0); }
		public PowerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_power; }
	}

	public final PowerContext power() throws RecognitionException {
		PowerContext _localctx = new PowerContext(_ctx, getState());
		enterRule(_localctx, 94, RULE_power);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(631);
			match(POWER);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ShiftRotateContext extends ParserRuleContext {
		public TerminalNode L_ROTATE() { return getToken(SyscodeParser.L_ROTATE, 0); }
		public TerminalNode R_ROTATE() { return getToken(SyscodeParser.R_ROTATE, 0); }
		public TerminalNode L_LOG_SHIFT() { return getToken(SyscodeParser.L_LOG_SHIFT, 0); }
		public TerminalNode R_LOG_SHIFT() { return getToken(SyscodeParser.R_LOG_SHIFT, 0); }
		public TerminalNode R_ART_SHIFT() { return getToken(SyscodeParser.R_ART_SHIFT, 0); }
		public ShiftRotateContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_shiftRotate; }
	}

	public final ShiftRotateContext shiftRotate() throws RecognitionException {
		ShiftRotateContext _localctx = new ShiftRotateContext(_ctx, getState());
		enterRule(_localctx, 96, RULE_shiftRotate);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(633);
			_la = _input.LA(1);
			if ( !(((((_la - 76)) & ~0x3f) == 0 && ((1L << (_la - 76)) & 31L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class AddSubContext extends ParserRuleContext {
		public TerminalNode PLUS() { return getToken(SyscodeParser.PLUS, 0); }
		public TerminalNode MINUS() { return getToken(SyscodeParser.MINUS, 0); }
		public AddSubContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_addSub; }
	}

	public final AddSubContext addSub() throws RecognitionException {
		AddSubContext _localctx = new AddSubContext(_ctx, getState());
		enterRule(_localctx, 98, RULE_addSub);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(635);
			_la = _input.LA(1);
			if ( !(_la==PLUS || _la==MINUS) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class MulDivContext extends ParserRuleContext {
		public TerminalNode TIMES() { return getToken(SyscodeParser.TIMES, 0); }
		public TerminalNode DIVIDE() { return getToken(SyscodeParser.DIVIDE, 0); }
		public TerminalNode PCNT() { return getToken(SyscodeParser.PCNT, 0); }
		public MulDivContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_mulDiv; }
	}

	public final MulDivContext mulDiv() throws RecognitionException {
		MulDivContext _localctx = new MulDivContext(_ctx, getState());
		enterRule(_localctx, 100, RULE_mulDiv);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(637);
			_la = _input.LA(1);
			if ( !(((((_la - 66)) & ~0x3f) == 0 && ((1L << (_la - 66)) & 7L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BoolAndContext extends ParserRuleContext {
		public TerminalNode AND() { return getToken(SyscodeParser.AND, 0); }
		public TerminalNode NAND() { return getToken(SyscodeParser.NAND, 0); }
		public BoolAndContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_boolAnd; }
	}

	public final BoolAndContext boolAnd() throws RecognitionException {
		BoolAndContext _localctx = new BoolAndContext(_ctx, getState());
		enterRule(_localctx, 102, RULE_boolAnd);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(639);
			_la = _input.LA(1);
			if ( !(_la==AND || _la==NAND) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BoolXorContext extends ParserRuleContext {
		public TerminalNode XOR() { return getToken(SyscodeParser.XOR, 0); }
		public TerminalNode XNOR() { return getToken(SyscodeParser.XNOR, 0); }
		public BoolXorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_boolXor; }
	}

	public final BoolXorContext boolXor() throws RecognitionException {
		BoolXorContext _localctx = new BoolXorContext(_ctx, getState());
		enterRule(_localctx, 104, RULE_boolXor);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(641);
			_la = _input.LA(1);
			if ( !(_la==XOR || _la==XNOR) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BoolOrContext extends ParserRuleContext {
		public TerminalNode OR() { return getToken(SyscodeParser.OR, 0); }
		public TerminalNode NOR() { return getToken(SyscodeParser.NOR, 0); }
		public TerminalNode NOT() { return getToken(SyscodeParser.NOT, 0); }
		public BoolOrContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_boolOr; }
	}

	public final BoolOrContext boolOr() throws RecognitionException {
		BoolOrContext _localctx = new BoolOrContext(_ctx, getState());
		enterRule(_localctx, 106, RULE_boolOr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(643);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 20829148276588544L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ComparisonContext extends ParserRuleContext {
		public TerminalNode GT() { return getToken(SyscodeParser.GT, 0); }
		public TerminalNode GTE() { return getToken(SyscodeParser.GTE, 0); }
		public TerminalNode EQUALS() { return getToken(SyscodeParser.EQUALS, 0); }
		public TerminalNode LT() { return getToken(SyscodeParser.LT, 0); }
		public TerminalNode LTE() { return getToken(SyscodeParser.LTE, 0); }
		public TerminalNode NGT() { return getToken(SyscodeParser.NGT, 0); }
		public TerminalNode NE() { return getToken(SyscodeParser.NE, 0); }
		public TerminalNode NLT() { return getToken(SyscodeParser.NLT, 0); }
		public ComparisonContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_comparison; }
	}

	public final ComparisonContext comparison() throws RecognitionException {
		ComparisonContext _localctx = new ComparisonContext(_ctx, getState());
		enterRule(_localctx, 108, RULE_comparison);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(645);
			_la = _input.LA(1);
			if ( !(((((_la - 55)) & ~0x3f) == 0 && ((1L << (_la - 55)) & 67108991L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PrefixOperatorContext extends ParserRuleContext {
		public TerminalNode PLUS() { return getToken(SyscodeParser.PLUS, 0); }
		public TerminalNode MINUS() { return getToken(SyscodeParser.MINUS, 0); }
		public TerminalNode NOT() { return getToken(SyscodeParser.NOT, 0); }
		public PrefixOperatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_prefixOperator; }
	}

	public final PrefixOperatorContext prefixOperator() throws RecognitionException {
		PrefixOperatorContext _localctx = new PrefixOperatorContext(_ctx, getState());
		enterRule(_localctx, 110, RULE_prefixOperator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(647);
			_la = _input.LA(1);
			if ( !(((((_la - 54)) & ~0x3f) == 0 && ((1L << (_la - 54)) & 3073L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StructDefinitionContext extends ParserRuleContext {
		public StructMembersContext Members;
		public StructNameContext structName() {
			return getRuleContext(StructNameContext.class,0);
		}
		public MemberSeparatorContext memberSeparator() {
			return getRuleContext(MemberSeparatorContext.class,0);
		}
		public TerminalNode END() { return getToken(SyscodeParser.END, 0); }
		public StructMembersContext structMembers() {
			return getRuleContext(StructMembersContext.class,0);
		}
		public List<EmptyLinesContext> emptyLines() {
			return getRuleContexts(EmptyLinesContext.class);
		}
		public EmptyLinesContext emptyLines(int i) {
			return getRuleContext(EmptyLinesContext.class,i);
		}
		public StructDefinitionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_structDefinition; }
	}

	public final StructDefinitionContext structDefinition() throws RecognitionException {
		StructDefinitionContext _localctx = new StructDefinitionContext(_ctx, getState());
		enterRule(_localctx, 112, RULE_structDefinition);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(649);
			structName();
			setState(651);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(650);
				emptyLines();
				}
			}

			setState(653);
			memberSeparator();
			setState(655);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,83,_ctx) ) {
			case 1:
				{
				setState(654);
				emptyLines();
				}
				break;
			}
			setState(657);
			((StructDefinitionContext)_localctx).Members = structMembers();
			setState(659);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(658);
				emptyLines();
				}
			}

			setState(661);
			match(END);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class QualifiedNameContext extends ParserRuleContext {
		public List<IdentifierContext> identifier() {
			return getRuleContexts(IdentifierContext.class);
		}
		public IdentifierContext identifier(int i) {
			return getRuleContext(IdentifierContext.class,i);
		}
		public List<TerminalNode> DOT() { return getTokens(SyscodeParser.DOT); }
		public TerminalNode DOT(int i) {
			return getToken(SyscodeParser.DOT, i);
		}
		public QualifiedNameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_qualifiedName; }
	}

	public final QualifiedNameContext qualifiedName() throws RecognitionException {
		QualifiedNameContext _localctx = new QualifiedNameContext(_ctx, getState());
		enterRule(_localctx, 114, RULE_qualifiedName);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(663);
			identifier();
			setState(668);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==DOT) {
				{
				{
				setState(664);
				match(DOT);
				setState(665);
				identifier();
				}
				}
				setState(670);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ParamListContext extends ParserRuleContext {
		public TerminalNode LPAR() { return getToken(SyscodeParser.LPAR, 0); }
		public List<IdentifierContext> identifier() {
			return getRuleContexts(IdentifierContext.class);
		}
		public IdentifierContext identifier(int i) {
			return getRuleContext(IdentifierContext.class,i);
		}
		public TerminalNode RPAR() { return getToken(SyscodeParser.RPAR, 0); }
		public List<TerminalNode> COMMA() { return getTokens(SyscodeParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(SyscodeParser.COMMA, i);
		}
		public ParamListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_paramList; }
	}

	public final ParamListContext paramList() throws RecognitionException {
		ParamListContext _localctx = new ParamListContext(_ctx, getState());
		enterRule(_localctx, 116, RULE_paramList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(671);
			match(LPAR);
			setState(672);
			identifier();
			setState(677);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(673);
				match(COMMA);
				setState(674);
				identifier();
				}
				}
				setState(679);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(680);
			match(RPAR);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ConstArrayListContext extends ParserRuleContext {
		public TerminalNode LPAR() { return getToken(SyscodeParser.LPAR, 0); }
		public List<TerminalNode> INTEGER() { return getTokens(SyscodeParser.INTEGER); }
		public TerminalNode INTEGER(int i) {
			return getToken(SyscodeParser.INTEGER, i);
		}
		public TerminalNode RPAR() { return getToken(SyscodeParser.RPAR, 0); }
		public List<TerminalNode> COMMA() { return getTokens(SyscodeParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(SyscodeParser.COMMA, i);
		}
		public ConstArrayListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_constArrayList; }
	}

	public final ConstArrayListContext constArrayList() throws RecognitionException {
		ConstArrayListContext _localctx = new ConstArrayListContext(_ctx, getState());
		enterRule(_localctx, 118, RULE_constArrayList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(682);
			match(LPAR);
			setState(683);
			match(INTEGER);
			setState(688);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(684);
				match(COMMA);
				setState(685);
				match(INTEGER);
				}
				}
				setState(690);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(691);
			match(RPAR);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StructNameContext extends ParserRuleContext {
		public IdentifierContext Spelling;
		public DimensionSuffixContext Bounds;
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public DimensionSuffixContext dimensionSuffix() {
			return getRuleContext(DimensionSuffixContext.class,0);
		}
		public StructNameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_structName; }
	}

	public final StructNameContext structName() throws RecognitionException {
		StructNameContext _localctx = new StructNameContext(_ctx, getState());
		enterRule(_localctx, 120, RULE_structName);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(693);
			((StructNameContext)_localctx).Spelling = identifier();
			setState(695);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LPAR) {
				{
				setState(694);
				((StructNameContext)_localctx).Bounds = dimensionSuffix();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StructMembersContext extends ParserRuleContext {
		public List<StructMemberContext> structMember() {
			return getRuleContexts(StructMemberContext.class);
		}
		public StructMemberContext structMember(int i) {
			return getRuleContext(StructMemberContext.class,i);
		}
		public List<EmptyLinesContext> emptyLines() {
			return getRuleContexts(EmptyLinesContext.class);
		}
		public EmptyLinesContext emptyLines(int i) {
			return getRuleContext(EmptyLinesContext.class,i);
		}
		public List<MemberSeparatorContext> memberSeparator() {
			return getRuleContexts(MemberSeparatorContext.class);
		}
		public MemberSeparatorContext memberSeparator(int i) {
			return getRuleContext(MemberSeparatorContext.class,i);
		}
		public StructMembersContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_structMembers; }
	}

	public final StructMembersContext structMembers() throws RecognitionException {
		StructMembersContext _localctx = new StructMembersContext(_ctx, getState());
		enterRule(_localctx, 122, RULE_structMembers);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(698);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(697);
				emptyLines();
				}
			}

			setState(700);
			structMember();
			setState(702);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,90,_ctx) ) {
			case 1:
				{
				setState(701);
				emptyLines();
				}
				break;
			}
			setState(714);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,93,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(704);
					memberSeparator();
					setState(706);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==NEWLINE) {
						{
						setState(705);
						emptyLines();
						}
					}

					setState(708);
					structMember();
					setState(710);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,92,_ctx) ) {
					case 1:
						{
						setState(709);
						emptyLines();
						}
						break;
					}
					}
					} 
				}
				setState(716);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,93,_ctx);
			}
			setState(718);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMA) {
				{
				setState(717);
				memberSeparator();
				}
			}

			setState(721);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,95,_ctx) ) {
			case 1:
				{
				setState(720);
				emptyLines();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EnumMembersContext extends ParserRuleContext {
		public List<EnumMemberContext> enumMember() {
			return getRuleContexts(EnumMemberContext.class);
		}
		public EnumMemberContext enumMember(int i) {
			return getRuleContext(EnumMemberContext.class,i);
		}
		public List<EmptyLinesContext> emptyLines() {
			return getRuleContexts(EmptyLinesContext.class);
		}
		public EmptyLinesContext emptyLines(int i) {
			return getRuleContext(EmptyLinesContext.class,i);
		}
		public List<MemberSeparatorContext> memberSeparator() {
			return getRuleContexts(MemberSeparatorContext.class);
		}
		public MemberSeparatorContext memberSeparator(int i) {
			return getRuleContext(MemberSeparatorContext.class,i);
		}
		public EnumMembersContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_enumMembers; }
	}

	public final EnumMembersContext enumMembers() throws RecognitionException {
		EnumMembersContext _localctx = new EnumMembersContext(_ctx, getState());
		enterRule(_localctx, 124, RULE_enumMembers);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(724);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(723);
				emptyLines();
				}
			}

			setState(726);
			enumMember();
			setState(728);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,97,_ctx) ) {
			case 1:
				{
				setState(727);
				emptyLines();
				}
				break;
			}
			setState(740);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,100,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(730);
					memberSeparator();
					setState(732);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==NEWLINE) {
						{
						setState(731);
						emptyLines();
						}
					}

					setState(734);
					enumMember();
					setState(736);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,99,_ctx) ) {
					case 1:
						{
						setState(735);
						emptyLines();
						}
						break;
					}
					}
					} 
				}
				setState(742);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,100,_ctx);
			}
			setState(744);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMA) {
				{
				setState(743);
				memberSeparator();
				}
			}

			setState(747);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,102,_ctx) ) {
			case 1:
				{
				setState(746);
				emptyLines();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StructMemberContext extends ParserRuleContext {
		public StructFieldContext structField() {
			return getRuleContext(StructFieldContext.class,0);
		}
		public StructDefinitionContext structDefinition() {
			return getRuleContext(StructDefinitionContext.class,0);
		}
		public StructMemberContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_structMember; }
	}

	public final StructMemberContext structMember() throws RecognitionException {
		StructMemberContext _localctx = new StructMemberContext(_ctx, getState());
		enterRule(_localctx, 126, RULE_structMember);
		try {
			setState(751);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,103,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(749);
				structField();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(750);
				structDefinition();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StructFieldContext extends ParserRuleContext {
		public IdentifierContext Spelling;
		public DimensionSuffixContext Bounds;
		public TypenameContext Type;
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public TypenameContext typename() {
			return getRuleContext(TypenameContext.class,0);
		}
		public EmptyLinesContext emptyLines() {
			return getRuleContext(EmptyLinesContext.class,0);
		}
		public DimensionSuffixContext dimensionSuffix() {
			return getRuleContext(DimensionSuffixContext.class,0);
		}
		public StructFieldContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_structField; }
	}

	public final StructFieldContext structField() throws RecognitionException {
		StructFieldContext _localctx = new StructFieldContext(_ctx, getState());
		enterRule(_localctx, 128, RULE_structField);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(753);
			((StructFieldContext)_localctx).Spelling = identifier();
			setState(755);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(754);
				emptyLines();
				}
			}

			setState(758);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LPAR) {
				{
				setState(757);
				((StructFieldContext)_localctx).Bounds = dimensionSuffix();
				}
			}

			setState(760);
			((StructFieldContext)_localctx).Type = typename();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StructStructContext extends ParserRuleContext {
		public StructDefinitionContext structDefinition() {
			return getRuleContext(StructDefinitionContext.class,0);
		}
		public StructStructContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_structStruct; }
	}

	public final StructStructContext structStruct() throws RecognitionException {
		StructStructContext _localctx = new StructStructContext(_ctx, getState());
		enterRule(_localctx, 130, RULE_structStruct);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(762);
			structDefinition();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EnumMemberContext extends ParserRuleContext {
		public IdentifierContext Name;
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public EnumMemberContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_enumMember; }
	}

	public final EnumMemberContext enumMember() throws RecognitionException {
		EnumMemberContext _localctx = new EnumMemberContext(_ctx, getState());
		enterRule(_localctx, 132, RULE_enumMember);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(764);
			((EnumMemberContext)_localctx).Name = identifier();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class IdentifierContext extends ParserRuleContext {
		public KeywordContext keyword() {
			return getRuleContext(KeywordContext.class,0);
		}
		public TerminalNode IDENTIFIER() { return getToken(SyscodeParser.IDENTIFIER, 0); }
		public IdentifierContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_identifier; }
	}

	public final IdentifierContext identifier() throws RecognitionException {
		IdentifierContext _localctx = new IdentifierContext(_ctx, getState());
		enterRule(_localctx, 134, RULE_identifier);
		try {
			setState(768);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case AS:
			case BIN16:
			case BIN32:
			case BIN64:
			case BIN8:
			case BIN:
			case BIT:
			case CALL:
			case DCL:
			case DEC:
			case DEF:
			case ELIF:
			case ELSE:
			case ENUM:
			case FOR:
			case FOREVER:
			case FUNC:
			case IF:
			case PATH:
			case PROC:
			case RETURN:
			case SCOPE:
			case STRING:
			case STRUCT:
			case THEN:
			case UBIN16:
			case UBIN32:
			case UBIN64:
			case UBIN8:
			case UBIN:
			case UDEC:
			case UNIT:
			case UNTIL:
			case WHILE:
				enterOuterAlt(_localctx, 1);
				{
				setState(766);
				keyword();
				}
				break;
			case IDENTIFIER:
				enterOuterAlt(_localctx, 2);
				{
				setState(767);
				match(IDENTIFIER);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class TypenameContext extends ParserRuleContext {
		public BinaryTypeContext binaryType() {
			return getRuleContext(BinaryTypeContext.class,0);
		}
		public DecimalTypeContext decimalType() {
			return getRuleContext(DecimalTypeContext.class,0);
		}
		public StringTypeContext stringType() {
			return getRuleContext(StringTypeContext.class,0);
		}
		public BitstringTypeContext bitstringType() {
			return getRuleContext(BitstringTypeContext.class,0);
		}
		public IdentifierContext identifier() {
			return getRuleContext(IdentifierContext.class,0);
		}
		public UnitTypeContext unitType() {
			return getRuleContext(UnitTypeContext.class,0);
		}
		public TypenameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typename; }
	}

	public final TypenameContext typename() throws RecognitionException {
		TypenameContext _localctx = new TypenameContext(_ctx, getState());
		enterRule(_localctx, 136, RULE_typename);
		try {
			setState(776);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,107,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(770);
				binaryType();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(771);
				decimalType();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(772);
				stringType();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(773);
				bitstringType();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(774);
				identifier();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(775);
				unitType();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class UnitTypeContext extends ParserRuleContext {
		public TerminalNode UNIT() { return getToken(SyscodeParser.UNIT, 0); }
		public UnitTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unitType; }
	}

	public final UnitTypeContext unitType() throws RecognitionException {
		UnitTypeContext _localctx = new UnitTypeContext(_ctx, getState());
		enterRule(_localctx, 138, RULE_unitType);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(778);
			match(UNIT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BinaryTypeContext extends ParserRuleContext {
		public TerminalNode BIN8() { return getToken(SyscodeParser.BIN8, 0); }
		public TerminalNode BIN16() { return getToken(SyscodeParser.BIN16, 0); }
		public TerminalNode BIN32() { return getToken(SyscodeParser.BIN32, 0); }
		public TerminalNode BIN64() { return getToken(SyscodeParser.BIN64, 0); }
		public TerminalNode UBIN8() { return getToken(SyscodeParser.UBIN8, 0); }
		public TerminalNode UBIN16() { return getToken(SyscodeParser.UBIN16, 0); }
		public TerminalNode UBIN32() { return getToken(SyscodeParser.UBIN32, 0); }
		public TerminalNode UBIN64() { return getToken(SyscodeParser.UBIN64, 0); }
		public TerminalNode BIN() { return getToken(SyscodeParser.BIN, 0); }
		public TerminalNode UBIN() { return getToken(SyscodeParser.UBIN, 0); }
		public ArgumentsListContext argumentsList() {
			return getRuleContext(ArgumentsListContext.class,0);
		}
		public BinaryTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_binaryType; }
	}

	public final BinaryTypeContext binaryType() throws RecognitionException {
		BinaryTypeContext _localctx = new BinaryTypeContext(_ctx, getState());
		enterRule(_localctx, 140, RULE_binaryType);
		int _la;
		try {
			setState(792);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case BIN8:
				enterOuterAlt(_localctx, 1);
				{
				setState(780);
				match(BIN8);
				}
				break;
			case BIN16:
				enterOuterAlt(_localctx, 2);
				{
				setState(781);
				match(BIN16);
				}
				break;
			case BIN32:
				enterOuterAlt(_localctx, 3);
				{
				setState(782);
				match(BIN32);
				}
				break;
			case BIN64:
				enterOuterAlt(_localctx, 4);
				{
				setState(783);
				match(BIN64);
				}
				break;
			case UBIN8:
				enterOuterAlt(_localctx, 5);
				{
				setState(784);
				match(UBIN8);
				}
				break;
			case UBIN16:
				enterOuterAlt(_localctx, 6);
				{
				setState(785);
				match(UBIN16);
				}
				break;
			case UBIN32:
				enterOuterAlt(_localctx, 7);
				{
				setState(786);
				match(UBIN32);
				}
				break;
			case UBIN64:
				enterOuterAlt(_localctx, 8);
				{
				setState(787);
				match(UBIN64);
				}
				break;
			case BIN:
			case UBIN:
				enterOuterAlt(_localctx, 9);
				{
				{
				setState(788);
				_la = _input.LA(1);
				if ( !(_la==BIN || _la==UBIN) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(790);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==LPAR) {
					{
					setState(789);
					argumentsList();
					}
				}

				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DecimalTypeContext extends ParserRuleContext {
		public ArgumentsListContext argumentsList() {
			return getRuleContext(ArgumentsListContext.class,0);
		}
		public TerminalNode DEC() { return getToken(SyscodeParser.DEC, 0); }
		public TerminalNode UDEC() { return getToken(SyscodeParser.UDEC, 0); }
		public DecimalTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_decimalType; }
	}

	public final DecimalTypeContext decimalType() throws RecognitionException {
		DecimalTypeContext _localctx = new DecimalTypeContext(_ctx, getState());
		enterRule(_localctx, 142, RULE_decimalType);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(794);
			_la = _input.LA(1);
			if ( !(_la==DEC || _la==UDEC) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			setState(795);
			argumentsList();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StringTypeContext extends ParserRuleContext {
		public TerminalNode STRING() { return getToken(SyscodeParser.STRING, 0); }
		public ArgumentsListContext argumentsList() {
			return getRuleContext(ArgumentsListContext.class,0);
		}
		public StringTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_stringType; }
	}

	public final StringTypeContext stringType() throws RecognitionException {
		StringTypeContext _localctx = new StringTypeContext(_ctx, getState());
		enterRule(_localctx, 144, RULE_stringType);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(797);
			match(STRING);
			setState(799);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LPAR) {
				{
				setState(798);
				argumentsList();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BitstringTypeContext extends ParserRuleContext {
		public TerminalNode BIT() { return getToken(SyscodeParser.BIT, 0); }
		public ArgumentsListContext argumentsList() {
			return getRuleContext(ArgumentsListContext.class,0);
		}
		public BitstringTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_bitstringType; }
	}

	public final BitstringTypeContext bitstringType() throws RecognitionException {
		BitstringTypeContext _localctx = new BitstringTypeContext(_ctx, getState());
		enterRule(_localctx, 146, RULE_bitstringType);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(801);
			match(BIT);
			setState(803);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LPAR) {
				{
				setState(802);
				argumentsList();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class MemberSeparatorContext extends ParserRuleContext {
		public TerminalNode COMMA() { return getToken(SyscodeParser.COMMA, 0); }
		public MemberSeparatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_memberSeparator; }
	}

	public final MemberSeparatorContext memberSeparator() throws RecognitionException {
		MemberSeparatorContext _localctx = new MemberSeparatorContext(_ctx, getState());
		enterRule(_localctx, 148, RULE_memberSeparator);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(805);
			match(COMMA);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EndOfFileContext extends ParserRuleContext {
		public TerminalNode EOF() { return getToken(SyscodeParser.EOF, 0); }
		public EmptyLinesContext emptyLines() {
			return getRuleContext(EmptyLinesContext.class,0);
		}
		public EndOfFileContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_endOfFile; }
	}

	public final EndOfFileContext endOfFile() throws RecognitionException {
		EndOfFileContext _localctx = new EndOfFileContext(_ctx, getState());
		enterRule(_localctx, 150, RULE_endOfFile);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(808);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEWLINE) {
				{
				setState(807);
				emptyLines();
				}
			}

			setState(810);
			match(EOF);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class KeywordContext extends ParserRuleContext {
		public TerminalNode AS() { return getToken(SyscodeParser.AS, 0); }
		public TerminalNode BIN16() { return getToken(SyscodeParser.BIN16, 0); }
		public TerminalNode BIN32() { return getToken(SyscodeParser.BIN32, 0); }
		public TerminalNode BIN64() { return getToken(SyscodeParser.BIN64, 0); }
		public TerminalNode BIN8() { return getToken(SyscodeParser.BIN8, 0); }
		public TerminalNode BIN() { return getToken(SyscodeParser.BIN, 0); }
		public TerminalNode BIT() { return getToken(SyscodeParser.BIT, 0); }
		public TerminalNode CALL() { return getToken(SyscodeParser.CALL, 0); }
		public TerminalNode DCL() { return getToken(SyscodeParser.DCL, 0); }
		public TerminalNode DEC() { return getToken(SyscodeParser.DEC, 0); }
		public TerminalNode DEF() { return getToken(SyscodeParser.DEF, 0); }
		public TerminalNode ELIF() { return getToken(SyscodeParser.ELIF, 0); }
		public TerminalNode ELSE() { return getToken(SyscodeParser.ELSE, 0); }
		public TerminalNode ENUM() { return getToken(SyscodeParser.ENUM, 0); }
		public TerminalNode FOR() { return getToken(SyscodeParser.FOR, 0); }
		public TerminalNode FOREVER() { return getToken(SyscodeParser.FOREVER, 0); }
		public TerminalNode FUNC() { return getToken(SyscodeParser.FUNC, 0); }
		public TerminalNode IF() { return getToken(SyscodeParser.IF, 0); }
		public TerminalNode PATH() { return getToken(SyscodeParser.PATH, 0); }
		public TerminalNode PROC() { return getToken(SyscodeParser.PROC, 0); }
		public TerminalNode RETURN() { return getToken(SyscodeParser.RETURN, 0); }
		public TerminalNode SCOPE() { return getToken(SyscodeParser.SCOPE, 0); }
		public TerminalNode STRING() { return getToken(SyscodeParser.STRING, 0); }
		public TerminalNode STRUCT() { return getToken(SyscodeParser.STRUCT, 0); }
		public TerminalNode THEN() { return getToken(SyscodeParser.THEN, 0); }
		public TerminalNode UBIN16() { return getToken(SyscodeParser.UBIN16, 0); }
		public TerminalNode UBIN32() { return getToken(SyscodeParser.UBIN32, 0); }
		public TerminalNode UBIN64() { return getToken(SyscodeParser.UBIN64, 0); }
		public TerminalNode UBIN8() { return getToken(SyscodeParser.UBIN8, 0); }
		public TerminalNode UBIN() { return getToken(SyscodeParser.UBIN, 0); }
		public TerminalNode UDEC() { return getToken(SyscodeParser.UDEC, 0); }
		public TerminalNode UNIT() { return getToken(SyscodeParser.UNIT, 0); }
		public TerminalNode UNTIL() { return getToken(SyscodeParser.UNTIL, 0); }
		public TerminalNode WHILE() { return getToken(SyscodeParser.WHILE, 0); }
		public KeywordContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_keyword; }
	}

	public final KeywordContext keyword() throws RecognitionException {
		KeywordContext _localctx = new KeywordContext(_ctx, getState());
		enterRule(_localctx, 152, RULE_keyword);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(812);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 17591982620544L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 21:
			return reference_sempred((ReferenceContext)_localctx, predIndex);
		case 28:
			return expression_sempred((ExpressionContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean reference_sempred(ReferenceContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 2);
		}
		return true;
	}
	private boolean expression_sempred(ExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 1:
			return precpred(_ctx, 11);
		case 2:
			return precpred(_ctx, 10);
		case 3:
			return precpred(_ctx, 9);
		case 4:
			return precpred(_ctx, 8);
		case 5:
			return precpred(_ctx, 7);
		case 6:
			return precpred(_ctx, 6);
		case 7:
			return precpred(_ctx, 5);
		case 8:
			return precpred(_ctx, 4);
		case 9:
			return precpred(_ctx, 3);
		case 10:
			return precpred(_ctx, 2);
		case 11:
			return precpred(_ctx, 1);
		}
		return true;
	}

	public static final String _serializedATN =
		"\u0004\u0001^\u032f\u0002\u0000\u0007\u0000\u0002\u0001\u0007\u0001\u0002"+
		"\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004\u0007\u0004\u0002"+
		"\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007\u0007\u0007\u0002"+
		"\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002\u000b\u0007\u000b\u0002"+
		"\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e\u0002\u000f\u0007\u000f"+
		"\u0002\u0010\u0007\u0010\u0002\u0011\u0007\u0011\u0002\u0012\u0007\u0012"+
		"\u0002\u0013\u0007\u0013\u0002\u0014\u0007\u0014\u0002\u0015\u0007\u0015"+
		"\u0002\u0016\u0007\u0016\u0002\u0017\u0007\u0017\u0002\u0018\u0007\u0018"+
		"\u0002\u0019\u0007\u0019\u0002\u001a\u0007\u001a\u0002\u001b\u0007\u001b"+
		"\u0002\u001c\u0007\u001c\u0002\u001d\u0007\u001d\u0002\u001e\u0007\u001e"+
		"\u0002\u001f\u0007\u001f\u0002 \u0007 \u0002!\u0007!\u0002\"\u0007\"\u0002"+
		"#\u0007#\u0002$\u0007$\u0002%\u0007%\u0002&\u0007&\u0002\'\u0007\'\u0002"+
		"(\u0007(\u0002)\u0007)\u0002*\u0007*\u0002+\u0007+\u0002,\u0007,\u0002"+
		"-\u0007-\u0002.\u0007.\u0002/\u0007/\u00020\u00070\u00021\u00071\u0002"+
		"2\u00072\u00023\u00073\u00024\u00074\u00025\u00075\u00026\u00076\u0002"+
		"7\u00077\u00028\u00078\u00029\u00079\u0002:\u0007:\u0002;\u0007;\u0002"+
		"<\u0007<\u0002=\u0007=\u0002>\u0007>\u0002?\u0007?\u0002@\u0007@\u0002"+
		"A\u0007A\u0002B\u0007B\u0002C\u0007C\u0002D\u0007D\u0002E\u0007E\u0002"+
		"F\u0007F\u0002G\u0007G\u0002H\u0007H\u0002I\u0007I\u0002J\u0007J\u0002"+
		"K\u0007K\u0002L\u0007L\u0001\u0000\u0004\u0000\u009c\b\u0000\u000b\u0000"+
		"\f\u0000\u009d\u0001\u0001\u0001\u0001\u0001\u0002\u0004\u0002\u00a3\b"+
		"\u0002\u000b\u0002\f\u0002\u00a4\u0001\u0003\u0005\u0003\u00a8\b\u0003"+
		"\n\u0003\f\u0003\u00ab\t\u0003\u0001\u0003\u0001\u0003\u0001\u0004\u0003"+
		"\u0004\u00b0\b\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0001"+
		"\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0001"+
		"\u0004\u0003\u0004\u00bd\b\u0004\u0001\u0005\u0001\u0005\u0001\u0005\u0001"+
		"\u0005\u0001\u0006\u0001\u0006\u0001\u0007\u0001\u0007\u0003\u0007\u00c7"+
		"\b\u0007\u0001\u0007\u0001\u0007\u0003\u0007\u00cb\b\u0007\u0001\u0007"+
		"\u0005\u0007\u00ce\b\u0007\n\u0007\f\u0007\u00d1\t\u0007\u0001\u0007\u0003"+
		"\u0007\u00d4\b\u0007\u0001\u0007\u0001\u0007\u0001\b\u0001\b\u0003\b\u00da"+
		"\b\b\u0001\b\u0001\b\u0003\b\u00de\b\b\u0001\b\u0005\b\u00e1\b\b\n\b\f"+
		"\b\u00e4\t\b\u0001\b\u0003\b\u00e7\b\b\u0001\b\u0001\b\u0001\t\u0001\t"+
		"\u0001\t\u0001\n\u0001\n\u0003\n\u00f0\b\n\u0001\n\u0001\n\u0003\n\u00f4"+
		"\b\n\u0001\n\u0003\n\u00f7\b\n\u0001\n\u0001\n\u0003\n\u00fb\b\n\u0001"+
		"\n\u0001\n\u0003\n\u00ff\b\n\u0001\n\u0001\n\u0001\u000b\u0001\u000b\u0003"+
		"\u000b\u0105\b\u000b\u0001\u000b\u0001\u000b\u0001\u000b\u0001\f\u0001"+
		"\f\u0003\f\u010c\b\f\u0001\f\u0001\f\u0001\f\u0001\f\u0003\f\u0112\b\f"+
		"\u0001\f\u0001\f\u0003\f\u0116\b\f\u0001\f\u0003\f\u0119\b\f\u0001\f\u0003"+
		"\f\u011c\b\f\u0001\r\u0001\r\u0003\r\u0120\b\r\u0001\r\u0001\r\u0003\r"+
		"\u0124\b\r\u0001\r\u0003\r\u0127\b\r\u0001\r\u0003\r\u012a\b\r\u0001\r"+
		"\u0001\r\u0001\r\u0001\u000e\u0001\u000e\u0001\u000e\u0001\u000e\u0001"+
		"\u000e\u0001\u000e\u0001\u000f\u0001\u000f\u0003\u000f\u0137\b\u000f\u0001"+
		"\u000f\u0001\u000f\u0003\u000f\u013b\b\u000f\u0001\u000f\u0003\u000f\u013e"+
		"\b\u000f\u0001\u000f\u0003\u000f\u0141\b\u000f\u0001\u000f\u0003\u000f"+
		"\u0144\b\u000f\u0001\u000f\u0003\u000f\u0147\b\u000f\u0001\u000f\u0001"+
		"\u000f\u0001\u0010\u0003\u0010\u014c\b\u0010\u0001\u0010\u0001\u0010\u0003"+
		"\u0010\u0150\b\u0010\u0001\u0010\u0001\u0010\u0003\u0010\u0154\b\u0010"+
		"\u0001\u0010\u0001\u0010\u0001\u0011\u0005\u0011\u0159\b\u0011\n\u0011"+
		"\f\u0011\u015c\t\u0011\u0001\u0012\u0001\u0012\u0003\u0012\u0160\b\u0012"+
		"\u0001\u0012\u0001\u0012\u0001\u0013\u0001\u0013\u0003\u0013\u0166\b\u0013"+
		"\u0001\u0013\u0004\u0013\u0169\b\u0013\u000b\u0013\f\u0013\u016a\u0001"+
		"\u0014\u0001\u0014\u0001\u0014\u0001\u0014\u0001\u0014\u0001\u0015\u0001"+
		"\u0015\u0001\u0015\u0003\u0015\u0175\b\u0015\u0001\u0015\u0001\u0015\u0001"+
		"\u0015\u0001\u0015\u0003\u0015\u017b\b\u0015\u0005\u0015\u017d\b\u0015"+
		"\n\u0015\f\u0015\u0180\t\u0015\u0001\u0016\u0003\u0016\u0183\b\u0016\u0001"+
		"\u0016\u0001\u0016\u0001\u0017\u0004\u0017\u0188\b\u0017\u000b\u0017\f"+
		"\u0017\u0189\u0001\u0018\u0004\u0018\u018d\b\u0018\u000b\u0018\f\u0018"+
		"\u018e\u0001\u0019\u0001\u0019\u0003\u0019\u0193\b\u0019\u0001\u0019\u0001"+
		"\u0019\u0001\u001a\u0001\u001a\u0003\u001a\u0199\b\u001a\u0001\u001a\u0001"+
		"\u001a\u0001\u001b\u0001\u001b\u0001\u001b\u0005\u001b\u01a0\b\u001b\n"+
		"\u001b\f\u001b\u01a3\t\u001b\u0001\u001c\u0001\u001c\u0001\u001c\u0001"+
		"\u001c\u0003\u001c\u01a9\b\u001c\u0001\u001c\u0001\u001c\u0003\u001c\u01ad"+
		"\b\u001c\u0001\u001c\u0001\u001c\u0003\u001c\u01b1\b\u001c\u0001\u001c"+
		"\u0001\u001c\u0001\u001c\u0001\u001c\u0003\u001c\u01b7\b\u001c\u0001\u001c"+
		"\u0001\u001c\u0003\u001c\u01bb\b\u001c\u0001\u001c\u0001\u001c\u0001\u001c"+
		"\u0001\u001c\u0003\u001c\u01c1\b\u001c\u0001\u001c\u0001\u001c\u0003\u001c"+
		"\u01c5\b\u001c\u0001\u001c\u0001\u001c\u0001\u001c\u0001\u001c\u0003\u001c"+
		"\u01cb\b\u001c\u0001\u001c\u0001\u001c\u0003\u001c\u01cf\b\u001c\u0001"+
		"\u001c\u0001\u001c\u0001\u001c\u0001\u001c\u0003\u001c\u01d5\b\u001c\u0001"+
		"\u001c\u0001\u001c\u0003\u001c\u01d9\b\u001c\u0001\u001c\u0001\u001c\u0001"+
		"\u001c\u0001\u001c\u0003\u001c\u01df\b\u001c\u0001\u001c\u0001\u001c\u0003"+
		"\u001c\u01e3\b\u001c\u0001\u001c\u0001\u001c\u0001\u001c\u0001\u001c\u0003"+
		"\u001c\u01e9\b\u001c\u0001\u001c\u0001\u001c\u0003\u001c\u01ed\b\u001c"+
		"\u0001\u001c\u0001\u001c\u0001\u001c\u0001\u001c\u0003\u001c\u01f3\b\u001c"+
		"\u0001\u001c\u0001\u001c\u0003\u001c\u01f7\b\u001c\u0001\u001c\u0001\u001c"+
		"\u0001\u001c\u0001\u001c\u0003\u001c\u01fd\b\u001c\u0001\u001c\u0001\u001c"+
		"\u0003\u001c\u0201\b\u001c\u0001\u001c\u0001\u001c\u0001\u001c\u0001\u001c"+
		"\u0003\u001c\u0207\b\u001c\u0001\u001c\u0001\u001c\u0003\u001c\u020b\b"+
		"\u001c\u0001\u001c\u0001\u001c\u0001\u001c\u0001\u001c\u0003\u001c\u0211"+
		"\b\u001c\u0001\u001c\u0001\u001c\u0003\u001c\u0215\b\u001c\u0001\u001c"+
		"\u0001\u001c\u0005\u001c\u0219\b\u001c\n\u001c\f\u001c\u021c\t\u001c\u0001"+
		"\u001d\u0001\u001d\u0001\u001d\u0001\u001d\u0003\u001d\u0222\b\u001d\u0001"+
		"\u001e\u0001\u001e\u0001\u001f\u0001\u001f\u0001\u001f\u0001\u001f\u0003"+
		"\u001f\u022a\b\u001f\u0001 \u0001 \u0001!\u0001!\u0001\"\u0001\"\u0001"+
		"#\u0001#\u0003#\u0234\b#\u0001$\u0001$\u0001%\u0001%\u0001%\u0001%\u0001"+
		"%\u0001%\u0001%\u0001%\u0001%\u0001%\u0001%\u0001%\u0001%\u0001%\u0001"+
		"%\u0001%\u0001%\u0001%\u0001%\u0001%\u0001%\u0001%\u0001%\u0001%\u0001"+
		"%\u0001%\u0001%\u0001%\u0003%\u0254\b%\u0001&\u0001&\u0001&\u0001\'\u0001"+
		"\'\u0001\'\u0001\'\u0001(\u0001(\u0001(\u0003(\u0260\b(\u0001(\u0001("+
		"\u0003(\u0264\b(\u0001)\u0001)\u0001)\u0005)\u0269\b)\n)\f)\u026c\t)\u0001"+
		"*\u0001*\u0001+\u0001+\u0001,\u0001,\u0001-\u0001-\u0001.\u0001.\u0001"+
		"/\u0001/\u00010\u00010\u00011\u00011\u00012\u00012\u00013\u00013\u0001"+
		"4\u00014\u00015\u00015\u00016\u00016\u00017\u00017\u00018\u00018\u0003"+
		"8\u028c\b8\u00018\u00018\u00038\u0290\b8\u00018\u00018\u00038\u0294\b"+
		"8\u00018\u00018\u00019\u00019\u00019\u00059\u029b\b9\n9\f9\u029e\t9\u0001"+
		":\u0001:\u0001:\u0001:\u0005:\u02a4\b:\n:\f:\u02a7\t:\u0001:\u0001:\u0001"+
		";\u0001;\u0001;\u0001;\u0005;\u02af\b;\n;\f;\u02b2\t;\u0001;\u0001;\u0001"+
		"<\u0001<\u0003<\u02b8\b<\u0001=\u0003=\u02bb\b=\u0001=\u0001=\u0003=\u02bf"+
		"\b=\u0001=\u0001=\u0003=\u02c3\b=\u0001=\u0001=\u0003=\u02c7\b=\u0005"+
		"=\u02c9\b=\n=\f=\u02cc\t=\u0001=\u0003=\u02cf\b=\u0001=\u0003=\u02d2\b"+
		"=\u0001>\u0003>\u02d5\b>\u0001>\u0001>\u0003>\u02d9\b>\u0001>\u0001>\u0003"+
		">\u02dd\b>\u0001>\u0001>\u0003>\u02e1\b>\u0005>\u02e3\b>\n>\f>\u02e6\t"+
		">\u0001>\u0003>\u02e9\b>\u0001>\u0003>\u02ec\b>\u0001?\u0001?\u0003?\u02f0"+
		"\b?\u0001@\u0001@\u0003@\u02f4\b@\u0001@\u0003@\u02f7\b@\u0001@\u0001"+
		"@\u0001A\u0001A\u0001B\u0001B\u0001C\u0001C\u0003C\u0301\bC\u0001D\u0001"+
		"D\u0001D\u0001D\u0001D\u0001D\u0003D\u0309\bD\u0001E\u0001E\u0001F\u0001"+
		"F\u0001F\u0001F\u0001F\u0001F\u0001F\u0001F\u0001F\u0001F\u0003F\u0317"+
		"\bF\u0003F\u0319\bF\u0001G\u0001G\u0001G\u0001H\u0001H\u0003H\u0320\b"+
		"H\u0001I\u0001I\u0003I\u0324\bI\u0001J\u0001J\u0001K\u0003K\u0329\bK\u0001"+
		"K\u0001K\u0001L\u0001L\u0001L\u0000\u0002*8M\u0000\u0002\u0004\u0006\b"+
		"\n\f\u000e\u0010\u0012\u0014\u0016\u0018\u001a\u001c\u001e \"$&(*,.02"+
		"468:<>@BDFHJLNPRTVXZ\\^`bdfhjlnprtvxz|~\u0080\u0082\u0084\u0086\u0088"+
		"\u008a\u008c\u008e\u0090\u0092\u0094\u0096\u0098\u0000\f\u0002\u0000V"+
		"V]]\u0001\u0000LP\u0001\u0000@A\u0001\u0000BD\u0002\u00000022\u0001\u0000"+
		"45\u0003\u0000113366\u0002\u00007=QQ\u0002\u000066@A\u0002\u0000\f\f\'"+
		"\'\u0002\u0000\u0010\u0010((\u0003\u0000\u0007\u0014\u0016\u0019\u001c"+
		"+\u0379\u0000\u009b\u0001\u0000\u0000\u0000\u0002\u009f\u0001\u0000\u0000"+
		"\u0000\u0004\u00a2\u0001\u0000\u0000\u0000\u0006\u00a9\u0001\u0000\u0000"+
		"\u0000\b\u00af\u0001\u0000\u0000\u0000\n\u00be\u0001\u0000\u0000\u0000"+
		"\f\u00c2\u0001\u0000\u0000\u0000\u000e\u00c4\u0001\u0000\u0000\u0000\u0010"+
		"\u00d7\u0001\u0000\u0000\u0000\u0012\u00ea\u0001\u0000\u0000\u0000\u0014"+
		"\u00ed\u0001\u0000\u0000\u0000\u0016\u0102\u0001\u0000\u0000\u0000\u0018"+
		"\u011b\u0001\u0000\u0000\u0000\u001a\u011d\u0001\u0000\u0000\u0000\u001c"+
		"\u012e\u0001\u0000\u0000\u0000\u001e\u0134\u0001\u0000\u0000\u0000 \u014b"+
		"\u0001\u0000\u0000\u0000\"\u015a\u0001\u0000\u0000\u0000$\u015d\u0001"+
		"\u0000\u0000\u0000&\u0168\u0001\u0000\u0000\u0000(\u016c\u0001\u0000\u0000"+
		"\u0000*\u0171\u0001\u0000\u0000\u0000,\u0182\u0001\u0000\u0000\u0000."+
		"\u0187\u0001\u0000\u0000\u00000\u018c\u0001\u0000\u0000\u00002\u0190\u0001"+
		"\u0000\u0000\u00004\u0196\u0001\u0000\u0000\u00006\u019c\u0001\u0000\u0000"+
		"\u00008\u01a8\u0001\u0000\u0000\u0000:\u0221\u0001\u0000\u0000\u0000<"+
		"\u0223\u0001\u0000\u0000\u0000>\u0229\u0001\u0000\u0000\u0000@\u022b\u0001"+
		"\u0000\u0000\u0000B\u022d\u0001\u0000\u0000\u0000D\u022f\u0001\u0000\u0000"+
		"\u0000F\u0233\u0001\u0000\u0000\u0000H\u0235\u0001\u0000\u0000\u0000J"+
		"\u0253\u0001\u0000\u0000\u0000L\u0255\u0001\u0000\u0000\u0000N\u0258\u0001"+
		"\u0000\u0000\u0000P\u0263\u0001\u0000\u0000\u0000R\u0265\u0001\u0000\u0000"+
		"\u0000T\u026d\u0001\u0000\u0000\u0000V\u026f\u0001\u0000\u0000\u0000X"+
		"\u0271\u0001\u0000\u0000\u0000Z\u0273\u0001\u0000\u0000\u0000\\\u0275"+
		"\u0001\u0000\u0000\u0000^\u0277\u0001\u0000\u0000\u0000`\u0279\u0001\u0000"+
		"\u0000\u0000b\u027b\u0001\u0000\u0000\u0000d\u027d\u0001\u0000\u0000\u0000"+
		"f\u027f\u0001\u0000\u0000\u0000h\u0281\u0001\u0000\u0000\u0000j\u0283"+
		"\u0001\u0000\u0000\u0000l\u0285\u0001\u0000\u0000\u0000n\u0287\u0001\u0000"+
		"\u0000\u0000p\u0289\u0001\u0000\u0000\u0000r\u0297\u0001\u0000\u0000\u0000"+
		"t\u029f\u0001\u0000\u0000\u0000v\u02aa\u0001\u0000\u0000\u0000x\u02b5"+
		"\u0001\u0000\u0000\u0000z\u02ba\u0001\u0000\u0000\u0000|\u02d4\u0001\u0000"+
		"\u0000\u0000~\u02ef\u0001\u0000\u0000\u0000\u0080\u02f1\u0001\u0000\u0000"+
		"\u0000\u0082\u02fa\u0001\u0000\u0000\u0000\u0084\u02fc\u0001\u0000\u0000"+
		"\u0000\u0086\u0300\u0001\u0000\u0000\u0000\u0088\u0308\u0001\u0000\u0000"+
		"\u0000\u008a\u030a\u0001\u0000\u0000\u0000\u008c\u0318\u0001\u0000\u0000"+
		"\u0000\u008e\u031a\u0001\u0000\u0000\u0000\u0090\u031d\u0001\u0000\u0000"+
		"\u0000\u0092\u0321\u0001\u0000\u0000\u0000\u0094\u0325\u0001\u0000\u0000"+
		"\u0000\u0096\u0328\u0001\u0000\u0000\u0000\u0098\u032c\u0001\u0000\u0000"+
		"\u0000\u009a\u009c\u0007\u0000\u0000\u0000\u009b\u009a\u0001\u0000\u0000"+
		"\u0000\u009c\u009d\u0001\u0000\u0000\u0000\u009d\u009b\u0001\u0000\u0000"+
		"\u0000\u009d\u009e\u0001\u0000\u0000\u0000\u009e\u0001\u0001\u0000\u0000"+
		"\u0000\u009f\u00a0\u0007\u0000\u0000\u0000\u00a0\u0003\u0001\u0000\u0000"+
		"\u0000\u00a1\u00a3\u0005]\u0000\u0000\u00a2\u00a1\u0001\u0000\u0000\u0000"+
		"\u00a3\u00a4\u0001\u0000\u0000\u0000\u00a4\u00a2\u0001\u0000\u0000\u0000"+
		"\u00a4\u00a5\u0001\u0000\u0000\u0000\u00a5\u0005\u0001\u0000\u0000\u0000"+
		"\u00a6\u00a8\u0003\b\u0004\u0000\u00a7\u00a6\u0001\u0000\u0000\u0000\u00a8"+
		"\u00ab\u0001\u0000\u0000\u0000\u00a9\u00a7\u0001\u0000\u0000\u0000\u00a9"+
		"\u00aa\u0001\u0000\u0000\u0000\u00aa\u00ac\u0001\u0000\u0000\u0000\u00ab"+
		"\u00a9\u0001\u0000\u0000\u0000\u00ac\u00ad\u0003\u0096K\u0000\u00ad\u0007"+
		"\u0001\u0000\u0000\u0000\u00ae\u00b0\u0003\u0000\u0000\u0000\u00af\u00ae"+
		"\u0001\u0000\u0000\u0000\u00af\u00b0\u0001\u0000\u0000\u0000\u00b0\u00bc"+
		"\u0001\u0000\u0000\u0000\u00b1\u00bd\u0003(\u0014\u0000\u00b2\u00bd\u0003"+
		"\u0016\u000b\u0000\u00b3\u00bd\u0003\u0018\f\u0000\u00b4\u00bd\u0003\n"+
		"\u0005\u0000\u00b5\u00bd\u0003\f\u0006\u0000\u00b6\u00bd\u0003\u0014\n"+
		"\u0000\u00b7\u00bd\u0003\u0012\t\u0000\u00b8\u00bd\u0003\u001e\u000f\u0000"+
		"\u00b9\u00bd\u0003\u001a\r\u0000\u00ba\u00bd\u0003\u001c\u000e\u0000\u00bb"+
		"\u00bd\u0003\u0010\b\u0000\u00bc\u00b1\u0001\u0000\u0000\u0000\u00bc\u00b2"+
		"\u0001\u0000\u0000\u0000\u00bc\u00b3\u0001\u0000\u0000\u0000\u00bc\u00b4"+
		"\u0001\u0000\u0000\u0000\u00bc\u00b5\u0001\u0000\u0000\u0000\u00bc\u00b6"+
		"\u0001\u0000\u0000\u0000\u00bc\u00b7\u0001\u0000\u0000\u0000\u00bc\u00b8"+
		"\u0001\u0000\u0000\u0000\u00bc\u00b9\u0001\u0000\u0000\u0000\u00bc\u00ba"+
		"\u0001\u0000\u0000\u0000\u00bc\u00bb\u0001\u0000\u0000\u0000\u00bd\t\u0001"+
		"\u0000\u0000\u0000\u00be\u00bf\u0005U\u0000\u0000\u00bf\u00c0\u0003\u0086"+
		"C\u0000\u00c0\u00c1\u0003\u0002\u0001\u0000\u00c1\u000b\u0001\u0000\u0000"+
		"\u0000\u00c2\u00c3\u0003\u000e\u0007\u0000\u00c3\r\u0001\u0000\u0000\u0000"+
		"\u00c4\u00c6\u0005\u001f\u0000\u0000\u00c5\u00c7\u0003\u0004\u0002\u0000"+
		"\u00c6\u00c5\u0001\u0000\u0000\u0000\u00c6\u00c7\u0001\u0000\u0000\u0000"+
		"\u00c7\u00c8\u0001\u0000\u0000\u0000\u00c8\u00ca\u0003r9\u0000\u00c9\u00cb"+
		"\u0003\u0004\u0002\u0000\u00ca\u00c9\u0001\u0000\u0000\u0000\u00ca\u00cb"+
		"\u0001\u0000\u0000\u0000\u00cb\u00cf\u0001\u0000\u0000\u0000\u00cc\u00ce"+
		"\u0003\b\u0004\u0000\u00cd\u00cc\u0001\u0000\u0000\u0000\u00ce\u00d1\u0001"+
		"\u0000\u0000\u0000\u00cf\u00cd\u0001\u0000\u0000\u0000\u00cf\u00d0\u0001"+
		"\u0000\u0000\u0000\u00d0\u00d3\u0001\u0000\u0000\u0000\u00d1\u00cf\u0001"+
		"\u0000\u0000\u0000\u00d2\u00d4\u0003\u0004\u0002\u0000\u00d3\u00d2\u0001"+
		"\u0000\u0000\u0000\u00d3\u00d4\u0001\u0000\u0000\u0000\u00d4\u00d5\u0001"+
		"\u0000\u0000\u0000\u00d5\u00d6\u0005\u0015\u0000\u0000\u00d6\u000f\u0001"+
		"\u0000\u0000\u0000\u00d7\u00d9\u0005\u001d\u0000\u0000\u00d8\u00da\u0003"+
		"\u0004\u0002\u0000\u00d9\u00d8\u0001\u0000\u0000\u0000\u00d9\u00da\u0001"+
		"\u0000\u0000\u0000\u00da\u00db\u0001\u0000\u0000\u0000\u00db\u00dd\u0003"+
		"\u0086C\u0000\u00dc\u00de\u0003t:\u0000\u00dd\u00dc\u0001\u0000\u0000"+
		"\u0000\u00dd\u00de\u0001\u0000\u0000\u0000\u00de\u00e2\u0001\u0000\u0000"+
		"\u0000\u00df\u00e1\u0003\b\u0004\u0000\u00e0\u00df\u0001\u0000\u0000\u0000"+
		"\u00e1\u00e4\u0001\u0000\u0000\u0000\u00e2\u00e0\u0001\u0000\u0000\u0000"+
		"\u00e2\u00e3\u0001\u0000\u0000\u0000\u00e3\u00e6\u0001\u0000\u0000\u0000"+
		"\u00e4\u00e2\u0001\u0000\u0000\u0000\u00e5\u00e7\u0003\u0004\u0002\u0000"+
		"\u00e6\u00e5\u0001\u0000\u0000\u0000\u00e6\u00e7\u0001\u0000\u0000\u0000"+
		"\u00e7\u00e8\u0001\u0000\u0000\u0000\u00e8\u00e9\u0005\u0015\u0000\u0000"+
		"\u00e9\u0011\u0001\u0000\u0000\u0000\u00ea\u00eb\u0005!\u0000\u0000\u00eb"+
		"\u00ec\u0003p8\u0000\u00ec\u0013\u0001\u0000\u0000\u0000\u00ed\u00ef\u0005"+
		"\u0014\u0000\u0000\u00ee\u00f0\u0003\u0004\u0002\u0000\u00ef\u00ee\u0001"+
		"\u0000\u0000\u0000\u00ef\u00f0\u0001\u0000\u0000\u0000\u00f0\u00f1\u0001"+
		"\u0000\u0000\u0000\u00f1\u00f3\u0003\u0086C\u0000\u00f2\u00f4\u0003\u0004"+
		"\u0002\u0000\u00f3\u00f2\u0001\u0000\u0000\u0000\u00f3\u00f4\u0001\u0000"+
		"\u0000\u0000\u00f4\u00f6\u0001\u0000\u0000\u0000\u00f5\u00f7\u0003\u0088"+
		"D\u0000\u00f6\u00f5\u0001\u0000\u0000\u0000\u00f6\u00f7\u0001\u0000\u0000"+
		"\u0000\u00f7\u00f8\u0001\u0000\u0000\u0000\u00f8\u00fa\u0003\u0094J\u0000"+
		"\u00f9\u00fb\u0003\u0004\u0002\u0000\u00fa\u00f9\u0001\u0000\u0000\u0000"+
		"\u00fa\u00fb\u0001\u0000\u0000\u0000\u00fb\u00fc\u0001\u0000\u0000\u0000"+
		"\u00fc\u00fe\u0003|>\u0000\u00fd\u00ff\u0003\u0004\u0002\u0000\u00fe\u00fd"+
		"\u0001\u0000\u0000\u0000\u00fe\u00ff\u0001\u0000\u0000\u0000\u00ff\u0100"+
		"\u0001\u0000\u0000\u0000\u0100\u0101\u0005\u0015\u0000\u0000\u0101\u0015"+
		"\u0001\u0000\u0000\u0000\u0102\u0104\u0005\u000e\u0000\u0000\u0103\u0105"+
		"\u0003\u0004\u0002\u0000\u0104\u0103\u0001\u0000\u0000\u0000\u0104\u0105"+
		"\u0001\u0000\u0000\u0000\u0105\u0106\u0001\u0000\u0000\u0000\u0106\u0107"+
		"\u0003*\u0015\u0000\u0107\u0108\u0003\u0002\u0001\u0000\u0108\u0017\u0001"+
		"\u0000\u0000\u0000\u0109\u0111\u0005\u001e\u0000\u0000\u010a\u010c\u0003"+
		"\u0004\u0002\u0000\u010b\u010a\u0001\u0000\u0000\u0000\u010b\u010c\u0001"+
		"\u0000\u0000\u0000\u010c\u010d\u0001\u0000\u0000\u0000\u010d\u010e\u0005"+
		"X\u0000\u0000\u010e\u010f\u00038\u001c\u0000\u010f\u0110\u0005Y\u0000"+
		"\u0000\u0110\u0112\u0001\u0000\u0000\u0000\u0111\u010b\u0001\u0000\u0000"+
		"\u0000\u0111\u0112\u0001\u0000\u0000\u0000\u0112\u011c\u0001\u0000\u0000"+
		"\u0000\u0113\u0118\u0005\u001e\u0000\u0000\u0114\u0116\u0003\u0004\u0002"+
		"\u0000\u0115\u0114\u0001\u0000\u0000\u0000\u0115\u0116\u0001\u0000\u0000"+
		"\u0000\u0116\u0117\u0001\u0000\u0000\u0000\u0117\u0119\u00038\u001c\u0000"+
		"\u0118\u0115\u0001\u0000\u0000\u0000\u0118\u0119\u0001\u0000\u0000\u0000"+
		"\u0119\u011a\u0001\u0000\u0000\u0000\u011a\u011c\u0003\u0002\u0001\u0000"+
		"\u011b\u0109\u0001\u0000\u0000\u0000\u011b\u0113\u0001\u0000\u0000\u0000"+
		"\u011c\u0019\u0001\u0000\u0000\u0000\u011d\u011f\u0005\u000f\u0000\u0000"+
		"\u011e\u0120\u0003\u0004\u0002\u0000\u011f\u011e\u0001\u0000\u0000\u0000"+
		"\u011f\u0120\u0001\u0000\u0000\u0000\u0120\u0121\u0001\u0000\u0000\u0000"+
		"\u0121\u0123\u0003\u0086C\u0000\u0122\u0124\u0003\u0004\u0002\u0000\u0123"+
		"\u0122\u0001\u0000\u0000\u0000\u0123\u0124\u0001\u0000\u0000\u0000\u0124"+
		"\u0126\u0001\u0000\u0000\u0000\u0125\u0127\u0003N\'\u0000\u0126\u0125"+
		"\u0001\u0000\u0000\u0000\u0126\u0127\u0001\u0000\u0000\u0000\u0127\u0129"+
		"\u0001\u0000\u0000\u0000\u0128\u012a\u0003\u0004\u0002\u0000\u0129\u0128"+
		"\u0001\u0000\u0000\u0000\u0129\u012a\u0001\u0000\u0000\u0000\u012a\u012b"+
		"\u0001\u0000\u0000\u0000\u012b\u012c\u0003\u0088D\u0000\u012c\u012d\u0003"+
		"\u0002\u0001\u0000\u012d\u001b\u0001\u0000\u0000\u0000\u012e\u012f\u0005"+
		"\u001b\u0000\u0000\u012f\u0130\u0003H$\u0000\u0130\u0131\u0005\u0007\u0000"+
		"\u0000\u0131\u0132\u0003F#\u0000\u0132\u0133\u0003\u0002\u0001\u0000\u0133"+
		"\u001d\u0001\u0000\u0000\u0000\u0134\u0136\u0005\u0019\u0000\u0000\u0135"+
		"\u0137\u0003\u0004\u0002\u0000\u0136\u0135\u0001\u0000\u0000\u0000\u0136"+
		"\u0137\u0001\u0000\u0000\u0000\u0137\u0138\u0001\u0000\u0000\u0000\u0138"+
		"\u013a\u0003 \u0010\u0000\u0139\u013b\u0003\u0004\u0002\u0000\u013a\u0139"+
		"\u0001\u0000\u0000\u0000\u013a\u013b\u0001\u0000\u0000\u0000\u013b\u013d"+
		"\u0001\u0000\u0000\u0000\u013c\u013e\u0003&\u0013\u0000\u013d\u013c\u0001"+
		"\u0000\u0000\u0000\u013d\u013e\u0001\u0000\u0000\u0000\u013e\u0140\u0001"+
		"\u0000\u0000\u0000\u013f\u0141\u0003\u0004\u0002\u0000\u0140\u013f\u0001"+
		"\u0000\u0000\u0000\u0140\u0141\u0001\u0000\u0000\u0000\u0141\u0143\u0001"+
		"\u0000\u0000\u0000\u0142\u0144\u0003$\u0012\u0000\u0143\u0142\u0001\u0000"+
		"\u0000\u0000\u0143\u0144\u0001\u0000\u0000\u0000\u0144\u0146\u0001\u0000"+
		"\u0000\u0000\u0145\u0147\u0003\u0004\u0002\u0000\u0146\u0145\u0001\u0000"+
		"\u0000\u0000\u0146\u0147\u0001\u0000\u0000\u0000\u0147\u0148\u0001\u0000"+
		"\u0000\u0000\u0148\u0149\u0005\u0015\u0000\u0000\u0149\u001f\u0001\u0000"+
		"\u0000\u0000\u014a\u014c\u0003\u0004\u0002\u0000\u014b\u014a\u0001\u0000"+
		"\u0000\u0000\u014b\u014c\u0001\u0000\u0000\u0000\u014c\u014d\u0001\u0000"+
		"\u0000\u0000\u014d\u014f\u00038\u001c\u0000\u014e\u0150\u0003\u0004\u0002"+
		"\u0000\u014f\u014e\u0001\u0000\u0000\u0000\u014f\u0150\u0001\u0000\u0000"+
		"\u0000\u0150\u0151\u0001\u0000\u0000\u0000\u0151\u0153\u0005\"\u0000\u0000"+
		"\u0152\u0154\u0003\u0004\u0002\u0000\u0153\u0152\u0001\u0000\u0000\u0000"+
		"\u0153\u0154\u0001\u0000\u0000\u0000\u0154\u0155\u0001\u0000\u0000\u0000"+
		"\u0155\u0156\u0003\"\u0011\u0000\u0156!\u0001\u0000\u0000\u0000\u0157"+
		"\u0159\u0003\b\u0004\u0000\u0158\u0157\u0001\u0000\u0000\u0000\u0159\u015c"+
		"\u0001\u0000\u0000\u0000\u015a\u0158\u0001\u0000\u0000\u0000\u015a\u015b"+
		"\u0001\u0000\u0000\u0000\u015b#\u0001\u0000\u0000\u0000\u015c\u015a\u0001"+
		"\u0000\u0000\u0000\u015d\u015f\u0005\u0013\u0000\u0000\u015e\u0160\u0003"+
		"\u0004\u0002\u0000\u015f\u015e\u0001\u0000\u0000\u0000\u015f\u0160\u0001"+
		"\u0000\u0000\u0000\u0160\u0161\u0001\u0000\u0000\u0000\u0161\u0162\u0003"+
		"\"\u0011\u0000\u0162%\u0001\u0000\u0000\u0000\u0163\u0165\u0005\u0012"+
		"\u0000\u0000\u0164\u0166\u0003\u0004\u0002\u0000\u0165\u0164\u0001\u0000"+
		"\u0000\u0000\u0165\u0166\u0001\u0000\u0000\u0000\u0166\u0167\u0001\u0000"+
		"\u0000\u0000\u0167\u0169\u0003 \u0010\u0000\u0168\u0163\u0001\u0000\u0000"+
		"\u0000\u0169\u016a\u0001\u0000\u0000\u0000\u016a\u0168\u0001\u0000\u0000"+
		"\u0000\u016a\u016b\u0001\u0000\u0000\u0000\u016b\'\u0001\u0000\u0000\u0000"+
		"\u016c\u016d\u0003*\u0015\u0000\u016d\u016e\u0005Q\u0000\u0000\u016e\u016f"+
		"\u00038\u001c\u0000\u016f\u0170\u0003\u0002\u0001\u0000\u0170)\u0001\u0000"+
		"\u0000\u0000\u0171\u0172\u0006\u0015\uffff\uffff\u0000\u0172\u0174\u0003"+
		",\u0016\u0000\u0173\u0175\u0003.\u0017\u0000\u0174\u0173\u0001\u0000\u0000"+
		"\u0000\u0174\u0175\u0001\u0000\u0000\u0000\u0175\u017e\u0001\u0000\u0000"+
		"\u0000\u0176\u0177\n\u0002\u0000\u0000\u0177\u0178\u0005Z\u0000\u0000"+
		"\u0178\u017a\u0003,\u0016\u0000\u0179\u017b\u0003.\u0017\u0000\u017a\u0179"+
		"\u0001\u0000\u0000\u0000\u017a\u017b\u0001\u0000\u0000\u0000\u017b\u017d"+
		"\u0001\u0000\u0000\u0000\u017c\u0176\u0001\u0000\u0000\u0000\u017d\u0180"+
		"\u0001\u0000\u0000\u0000\u017e\u017c\u0001\u0000\u0000\u0000\u017e\u017f"+
		"\u0001\u0000\u0000\u0000\u017f+\u0001\u0000\u0000\u0000\u0180\u017e\u0001"+
		"\u0000\u0000\u0000\u0181\u0183\u00030\u0018\u0000\u0182\u0181\u0001\u0000"+
		"\u0000\u0000\u0182\u0183\u0001\u0000\u0000\u0000\u0183\u0184\u0001\u0000"+
		"\u0000\u0000\u0184\u0185\u0003\u0086C\u0000\u0185-\u0001\u0000\u0000\u0000"+
		"\u0186\u0188\u00034\u001a\u0000\u0187\u0186\u0001\u0000\u0000\u0000\u0188"+
		"\u0189\u0001\u0000\u0000\u0000\u0189\u0187\u0001\u0000\u0000\u0000\u0189"+
		"\u018a\u0001\u0000\u0000\u0000\u018a/\u0001\u0000\u0000\u0000\u018b\u018d"+
		"\u00032\u0019\u0000\u018c\u018b\u0001\u0000\u0000\u0000\u018d\u018e\u0001"+
		"\u0000\u0000\u0000\u018e\u018c\u0001\u0000\u0000\u0000\u018e\u018f\u0001"+
		"\u0000\u0000\u0000\u018f1\u0001\u0000\u0000\u0000\u0190\u0192\u0003\u0086"+
		"C\u0000\u0191\u0193\u00034\u001a\u0000\u0192\u0191\u0001\u0000\u0000\u0000"+
		"\u0192\u0193\u0001\u0000\u0000\u0000\u0193\u0194\u0001\u0000\u0000\u0000"+
		"\u0194\u0195\u0005T\u0000\u0000\u01953\u0001\u0000\u0000\u0000\u0196\u0198"+
		"\u0005X\u0000\u0000\u0197\u0199\u00036\u001b\u0000\u0198\u0197\u0001\u0000"+
		"\u0000\u0000\u0198\u0199\u0001\u0000\u0000\u0000\u0199\u019a\u0001\u0000"+
		"\u0000\u0000\u019a\u019b\u0005Y\u0000\u0000\u019b5\u0001\u0000\u0000\u0000"+
		"\u019c\u01a1\u00038\u001c\u0000\u019d\u019e\u0005W\u0000\u0000\u019e\u01a0"+
		"\u00038\u001c\u0000\u019f\u019d\u0001\u0000\u0000\u0000\u01a0\u01a3\u0001"+
		"\u0000\u0000\u0000\u01a1\u019f\u0001\u0000\u0000\u0000\u01a1\u01a2\u0001"+
		"\u0000\u0000\u0000\u01a27\u0001\u0000\u0000\u0000\u01a3\u01a1\u0001\u0000"+
		"\u0000\u0000\u01a4\u01a5\u0006\u001c\uffff\uffff\u0000\u01a5\u01a9\u0003"+
		":\u001d\u0000\u01a6\u01a9\u0003J%\u0000\u01a7\u01a9\u0003L&\u0000\u01a8"+
		"\u01a4\u0001\u0000\u0000\u0000\u01a8\u01a6\u0001\u0000\u0000\u0000\u01a8"+
		"\u01a7\u0001\u0000\u0000\u0000\u01a9\u021a\u0001\u0000\u0000\u0000\u01aa"+
		"\u01ac\n\u000b\u0000\u0000\u01ab\u01ad\u0003\u0004\u0002\u0000\u01ac\u01ab"+
		"\u0001\u0000\u0000\u0000\u01ac\u01ad\u0001\u0000\u0000\u0000\u01ad\u01ae"+
		"\u0001\u0000\u0000\u0000\u01ae\u01b0\u0003^/\u0000\u01af\u01b1\u0003\u0004"+
		"\u0002\u0000\u01b0\u01af\u0001\u0000\u0000\u0000\u01b0\u01b1\u0001\u0000"+
		"\u0000\u0000\u01b1\u01b2\u0001\u0000\u0000\u0000\u01b2\u01b3\u00038\u001c"+
		"\u000b\u01b3\u0219\u0001\u0000\u0000\u0000\u01b4\u01b6\n\n\u0000\u0000"+
		"\u01b5\u01b7\u0003\u0004\u0002\u0000\u01b6\u01b5\u0001\u0000\u0000\u0000"+
		"\u01b6\u01b7\u0001\u0000\u0000\u0000\u01b7\u01b8\u0001\u0000\u0000\u0000"+
		"\u01b8\u01ba\u0003d2\u0000\u01b9\u01bb\u0003\u0004\u0002\u0000\u01ba\u01b9"+
		"\u0001\u0000\u0000\u0000\u01ba\u01bb\u0001\u0000\u0000\u0000\u01bb\u01bc"+
		"\u0001\u0000\u0000\u0000\u01bc\u01bd\u00038\u001c\u000b\u01bd\u0219\u0001"+
		"\u0000\u0000\u0000\u01be\u01c0\n\t\u0000\u0000\u01bf\u01c1\u0003\u0004"+
		"\u0002\u0000\u01c0\u01bf\u0001\u0000\u0000\u0000\u01c0\u01c1\u0001\u0000"+
		"\u0000\u0000\u01c1\u01c2\u0001\u0000\u0000\u0000\u01c2\u01c4\u0003b1\u0000"+
		"\u01c3\u01c5\u0003\u0004\u0002\u0000\u01c4\u01c3\u0001\u0000\u0000\u0000"+
		"\u01c4\u01c5\u0001\u0000\u0000\u0000\u01c5\u01c6\u0001\u0000\u0000\u0000"+
		"\u01c6\u01c7\u00038\u001c\n\u01c7\u0219\u0001\u0000\u0000\u0000\u01c8"+
		"\u01ca\n\b\u0000\u0000\u01c9\u01cb\u0003\u0004\u0002\u0000\u01ca\u01c9"+
		"\u0001\u0000\u0000\u0000\u01ca\u01cb\u0001\u0000\u0000\u0000\u01cb\u01cc"+
		"\u0001\u0000\u0000\u0000\u01cc\u01ce\u0003`0\u0000\u01cd\u01cf\u0003\u0004"+
		"\u0002\u0000\u01ce\u01cd\u0001\u0000\u0000\u0000\u01ce\u01cf\u0001\u0000"+
		"\u0000\u0000\u01cf\u01d0\u0001\u0000\u0000\u0000\u01d0\u01d1\u00038\u001c"+
		"\t\u01d1\u0219\u0001\u0000\u0000\u0000\u01d2\u01d4\n\u0007\u0000\u0000"+
		"\u01d3\u01d5\u0003\u0004\u0002\u0000\u01d4\u01d3\u0001\u0000\u0000\u0000"+
		"\u01d4\u01d5\u0001\u0000\u0000\u0000\u01d5\u01d6\u0001\u0000\u0000\u0000"+
		"\u01d6\u01d8\u0003\\.\u0000\u01d7\u01d9\u0003\u0004\u0002\u0000\u01d8"+
		"\u01d7\u0001\u0000\u0000\u0000\u01d8\u01d9\u0001\u0000\u0000\u0000\u01d9"+
		"\u01da\u0001\u0000\u0000\u0000\u01da\u01db\u00038\u001c\b\u01db\u0219"+
		"\u0001\u0000\u0000\u0000\u01dc\u01de\n\u0006\u0000\u0000\u01dd\u01df\u0003"+
		"\u0004\u0002\u0000\u01de\u01dd\u0001\u0000\u0000\u0000\u01de\u01df\u0001"+
		"\u0000\u0000\u0000\u01df\u01e0\u0001\u0000\u0000\u0000\u01e0\u01e2\u0003"+
		"l6\u0000\u01e1\u01e3\u0003\u0004\u0002\u0000\u01e2\u01e1\u0001\u0000\u0000"+
		"\u0000\u01e2\u01e3\u0001\u0000\u0000\u0000\u01e3\u01e4\u0001\u0000\u0000"+
		"\u0000\u01e4\u01e5\u00038\u001c\u0007\u01e5\u0219\u0001\u0000\u0000\u0000"+
		"\u01e6\u01e8\n\u0005\u0000\u0000\u01e7\u01e9\u0003\u0004\u0002\u0000\u01e8"+
		"\u01e7\u0001\u0000\u0000\u0000\u01e8\u01e9\u0001\u0000\u0000\u0000\u01e9"+
		"\u01ea\u0001\u0000\u0000\u0000\u01ea\u01ec\u0003f3\u0000\u01eb\u01ed\u0003"+
		"\u0004\u0002\u0000\u01ec\u01eb\u0001\u0000\u0000\u0000\u01ec\u01ed\u0001"+
		"\u0000\u0000\u0000\u01ed\u01ee\u0001\u0000\u0000\u0000\u01ee\u01ef\u0003"+
		"8\u001c\u0006\u01ef\u0219\u0001\u0000\u0000\u0000\u01f0\u01f2\n\u0004"+
		"\u0000\u0000\u01f1\u01f3\u0003\u0004\u0002\u0000\u01f2\u01f1\u0001\u0000"+
		"\u0000\u0000\u01f2\u01f3\u0001\u0000\u0000\u0000\u01f3\u01f4\u0001\u0000"+
		"\u0000\u0000\u01f4\u01f6\u0003h4\u0000\u01f5\u01f7\u0003\u0004\u0002\u0000"+
		"\u01f6\u01f5\u0001\u0000\u0000\u0000\u01f6\u01f7\u0001\u0000\u0000\u0000"+
		"\u01f7\u01f8\u0001\u0000\u0000\u0000\u01f8\u01f9\u00038\u001c\u0005\u01f9"+
		"\u0219\u0001\u0000\u0000\u0000\u01fa\u01fc\n\u0003\u0000\u0000\u01fb\u01fd"+
		"\u0003\u0004\u0002\u0000\u01fc\u01fb\u0001\u0000\u0000\u0000\u01fc\u01fd"+
		"\u0001\u0000\u0000\u0000\u01fd\u01fe\u0001\u0000\u0000\u0000\u01fe\u0200"+
		"\u0003j5\u0000\u01ff\u0201\u0003\u0004\u0002\u0000\u0200\u01ff\u0001\u0000"+
		"\u0000\u0000\u0200\u0201\u0001\u0000\u0000\u0000\u0201\u0202\u0001\u0000"+
		"\u0000\u0000\u0202\u0203\u00038\u001c\u0004\u0203\u0219\u0001\u0000\u0000"+
		"\u0000\u0204\u0206\n\u0002\u0000\u0000\u0205\u0207\u0003\u0004\u0002\u0000"+
		"\u0206\u0205\u0001\u0000\u0000\u0000\u0206\u0207\u0001\u0000\u0000\u0000"+
		"\u0207\u0208\u0001\u0000\u0000\u0000\u0208\u020a\u0003X,\u0000\u0209\u020b"+
		"\u0003\u0004\u0002\u0000\u020a\u0209\u0001\u0000\u0000\u0000\u020a\u020b"+
		"\u0001\u0000\u0000\u0000\u020b\u020c\u0001\u0000\u0000\u0000\u020c\u020d"+
		"\u00038\u001c\u0003\u020d\u0219\u0001\u0000\u0000\u0000\u020e\u0210\n"+
		"\u0001\u0000\u0000\u020f\u0211\u0003\u0004\u0002\u0000\u0210\u020f\u0001"+
		"\u0000\u0000\u0000\u0210\u0211\u0001\u0000\u0000\u0000\u0211\u0212\u0001"+
		"\u0000\u0000\u0000\u0212\u0214\u0003Z-\u0000\u0213\u0215\u0003\u0004\u0002"+
		"\u0000\u0214\u0213\u0001\u0000\u0000\u0000\u0214\u0215\u0001\u0000\u0000"+
		"\u0000\u0215\u0216\u0001\u0000\u0000\u0000\u0216\u0217\u00038\u001c\u0002"+
		"\u0217\u0219\u0001\u0000\u0000\u0000\u0218\u01aa\u0001\u0000\u0000\u0000"+
		"\u0218\u01b4\u0001\u0000\u0000\u0000\u0218\u01be\u0001\u0000\u0000\u0000"+
		"\u0218\u01c8\u0001\u0000\u0000\u0000\u0218\u01d2\u0001\u0000\u0000\u0000"+
		"\u0218\u01dc\u0001\u0000\u0000\u0000\u0218\u01e6\u0001\u0000\u0000\u0000"+
		"\u0218\u01f0\u0001\u0000\u0000\u0000\u0218\u01fa\u0001\u0000\u0000\u0000"+
		"\u0218\u0204\u0001\u0000\u0000\u0000\u0218\u020e\u0001\u0000\u0000\u0000"+
		"\u0219\u021c\u0001\u0000\u0000\u0000\u021a\u0218\u0001\u0000\u0000\u0000"+
		"\u021a\u021b\u0001\u0000\u0000\u0000\u021b9\u0001\u0000\u0000\u0000\u021c"+
		"\u021a\u0001\u0000\u0000\u0000\u021d\u0222\u0003>\u001f\u0000\u021e\u0222"+
		"\u0003<\u001e\u0000\u021f\u0222\u0003H$\u0000\u0220\u0222\u0003*\u0015"+
		"\u0000\u0221\u021d\u0001\u0000\u0000\u0000\u0221\u021e\u0001\u0000\u0000"+
		"\u0000\u0221\u021f\u0001\u0000\u0000\u0000\u0221\u0220\u0001\u0000\u0000"+
		"\u0000\u0222;\u0001\u0000\u0000\u0000\u0223\u0224\u0005?\u0000\u0000\u0224"+
		"=\u0001\u0000\u0000\u0000\u0225\u022a\u0003B!\u0000\u0226\u022a\u0003"+
		"D\"\u0000\u0227\u022a\u0003@ \u0000\u0228\u022a\u0003F#\u0000\u0229\u0225"+
		"\u0001\u0000\u0000\u0000\u0229\u0226\u0001\u0000\u0000\u0000\u0229\u0227"+
		"\u0001\u0000\u0000\u0000\u0229\u0228\u0001\u0000\u0000\u0000\u022a?\u0001"+
		"\u0000\u0000\u0000\u022b\u022c\u0005\u0002\u0000\u0000\u022cA\u0001\u0000"+
		"\u0000\u0000\u022d\u022e\u0005\u0005\u0000\u0000\u022eC\u0001\u0000\u0000"+
		"\u0000\u022f\u0230\u0005\u0003\u0000\u0000\u0230E\u0001\u0000\u0000\u0000"+
		"\u0231\u0234\u0005\u0006\u0000\u0000\u0232\u0234\u0005\u0004\u0000\u0000"+
		"\u0233\u0231\u0001\u0000\u0000\u0000\u0233\u0232\u0001\u0000\u0000\u0000"+
		"\u0234G\u0001\u0000\u0000\u0000\u0235\u0236\u0005\\\u0000\u0000\u0236"+
		"I\u0001\u0000\u0000\u0000\u0237\u0238\u0005X\u0000\u0000\u0238\u0239\u0003"+
		"8\u001c\u0000\u0239\u023a\u0005Y\u0000\u0000\u023a\u0254\u0001\u0000\u0000"+
		"\u0000\u023b\u023c\u0005F\u0000\u0000\u023c\u023d\u00038\u001c\u0000\u023d"+
		"\u023e\u0005Y\u0000\u0000\u023e\u0254\u0001\u0000\u0000\u0000\u023f\u0240"+
		"\u0005G\u0000\u0000\u0240\u0241\u00038\u001c\u0000\u0241\u0242\u0005Y"+
		"\u0000\u0000\u0242\u0254\u0001\u0000\u0000\u0000\u0243\u0244\u0005I\u0000"+
		"\u0000\u0244\u0245\u00038\u001c\u0000\u0245\u0246\u0005Y\u0000\u0000\u0246"+
		"\u0254\u0001\u0000\u0000\u0000\u0247\u0248\u0005K\u0000\u0000\u0248\u0249"+
		"\u00038\u001c\u0000\u0249\u024a\u0005Y\u0000\u0000\u024a\u0254\u0001\u0000"+
		"\u0000\u0000\u024b\u024c\u0005H\u0000\u0000\u024c\u024d\u00038\u001c\u0000"+
		"\u024d\u024e\u0005Y\u0000\u0000\u024e\u0254\u0001\u0000\u0000\u0000\u024f"+
		"\u0250\u0005J\u0000\u0000\u0250\u0251\u00038\u001c\u0000\u0251\u0252\u0005"+
		"Y\u0000\u0000\u0252\u0254\u0001\u0000\u0000\u0000\u0253\u0237\u0001\u0000"+
		"\u0000\u0000\u0253\u023b\u0001\u0000\u0000\u0000\u0253\u023f\u0001\u0000"+
		"\u0000\u0000\u0253\u0243\u0001\u0000\u0000\u0000\u0253\u0247\u0001\u0000"+
		"\u0000\u0000\u0253\u024b\u0001\u0000\u0000\u0000\u0253\u024f\u0001\u0000"+
		"\u0000\u0000\u0254K\u0001\u0000\u0000\u0000\u0255\u0256\u0003n7\u0000"+
		"\u0256\u0257\u00038\u001c\u0000\u0257M\u0001\u0000\u0000\u0000\u0258\u0259"+
		"\u0005X\u0000\u0000\u0259\u025a\u0003R)\u0000\u025a\u025b\u0005Y\u0000"+
		"\u0000\u025bO\u0001\u0000\u0000\u0000\u025c\u025d\u0003T*\u0000\u025d"+
		"\u025e\u0005,\u0000\u0000\u025e\u0260\u0001\u0000\u0000\u0000\u025f\u025c"+
		"\u0001\u0000\u0000\u0000\u025f\u0260\u0001\u0000\u0000\u0000\u0260\u0261"+
		"\u0001\u0000\u0000\u0000\u0261\u0264\u0003V+\u0000\u0262\u0264\u0005B"+
		"\u0000\u0000\u0263\u025f\u0001\u0000\u0000\u0000\u0263\u0262\u0001\u0000"+
		"\u0000\u0000\u0264Q\u0001\u0000\u0000\u0000\u0265\u026a\u0003P(\u0000"+
		"\u0266\u0267\u0005W\u0000\u0000\u0267\u0269\u0003P(\u0000\u0268\u0266"+
		"\u0001\u0000\u0000\u0000\u0269\u026c\u0001\u0000\u0000\u0000\u026a\u0268"+
		"\u0001\u0000\u0000\u0000\u026a\u026b\u0001\u0000\u0000\u0000\u026bS\u0001"+
		"\u0000\u0000\u0000\u026c\u026a\u0001\u0000\u0000\u0000\u026d\u026e\u0003"+
		"8\u001c\u0000\u026eU\u0001\u0000\u0000\u0000\u026f\u0270\u00038\u001c"+
		"\u0000\u0270W\u0001\u0000\u0000\u0000\u0271\u0272\u0005.\u0000\u0000\u0272"+
		"Y\u0001\u0000\u0000\u0000\u0273\u0274\u0005/\u0000\u0000\u0274[\u0001"+
		"\u0000\u0000\u0000\u0275\u0276\u0005-\u0000\u0000\u0276]\u0001\u0000\u0000"+
		"\u0000\u0277\u0278\u0005>\u0000\u0000\u0278_\u0001\u0000\u0000\u0000\u0279"+
		"\u027a\u0007\u0001\u0000\u0000\u027aa\u0001\u0000\u0000\u0000\u027b\u027c"+
		"\u0007\u0002\u0000\u0000\u027cc\u0001\u0000\u0000\u0000\u027d\u027e\u0007"+
		"\u0003\u0000\u0000\u027ee\u0001\u0000\u0000\u0000\u027f\u0280\u0007\u0004"+
		"\u0000\u0000\u0280g\u0001\u0000\u0000\u0000\u0281\u0282\u0007\u0005\u0000"+
		"\u0000\u0282i\u0001\u0000\u0000\u0000\u0283\u0284\u0007\u0006\u0000\u0000"+
		"\u0284k\u0001\u0000\u0000\u0000\u0285\u0286\u0007\u0007\u0000\u0000\u0286"+
		"m\u0001\u0000\u0000\u0000\u0287\u0288\u0007\b\u0000\u0000\u0288o\u0001"+
		"\u0000\u0000\u0000\u0289\u028b\u0003x<\u0000\u028a\u028c\u0003\u0004\u0002"+
		"\u0000\u028b\u028a\u0001\u0000\u0000\u0000\u028b\u028c\u0001\u0000\u0000"+
		"\u0000\u028c\u028d\u0001\u0000\u0000\u0000\u028d\u028f\u0003\u0094J\u0000"+
		"\u028e\u0290\u0003\u0004\u0002\u0000\u028f\u028e\u0001\u0000\u0000\u0000"+
		"\u028f\u0290\u0001\u0000\u0000\u0000\u0290\u0291\u0001\u0000\u0000\u0000"+
		"\u0291\u0293\u0003z=\u0000\u0292\u0294\u0003\u0004\u0002\u0000\u0293\u0292"+
		"\u0001\u0000\u0000\u0000\u0293\u0294\u0001\u0000\u0000\u0000\u0294\u0295"+
		"\u0001\u0000\u0000\u0000\u0295\u0296\u0005\u0015\u0000\u0000\u0296q\u0001"+
		"\u0000\u0000\u0000\u0297\u029c\u0003\u0086C\u0000\u0298\u0299\u0005T\u0000"+
		"\u0000\u0299\u029b\u0003\u0086C\u0000\u029a\u0298\u0001\u0000\u0000\u0000"+
		"\u029b\u029e\u0001\u0000\u0000\u0000\u029c\u029a\u0001\u0000\u0000\u0000"+
		"\u029c\u029d\u0001\u0000\u0000\u0000\u029ds\u0001\u0000\u0000\u0000\u029e"+
		"\u029c\u0001\u0000\u0000\u0000\u029f\u02a0\u0005X\u0000\u0000\u02a0\u02a5"+
		"\u0003\u0086C\u0000\u02a1\u02a2\u0005W\u0000\u0000\u02a2\u02a4\u0003\u0086"+
		"C\u0000\u02a3\u02a1\u0001\u0000\u0000\u0000\u02a4\u02a7\u0001\u0000\u0000"+
		"\u0000\u02a5\u02a3\u0001\u0000\u0000\u0000\u02a5\u02a6\u0001\u0000\u0000"+
		"\u0000\u02a6\u02a8\u0001\u0000\u0000\u0000\u02a7\u02a5\u0001\u0000\u0000"+
		"\u0000\u02a8\u02a9\u0005Y\u0000\u0000\u02a9u\u0001\u0000\u0000\u0000\u02aa"+
		"\u02ab\u0005X\u0000\u0000\u02ab\u02b0\u0005\u0006\u0000\u0000\u02ac\u02ad"+
		"\u0005W\u0000\u0000\u02ad\u02af\u0005\u0006\u0000\u0000\u02ae\u02ac\u0001"+
		"\u0000\u0000\u0000\u02af\u02b2\u0001\u0000\u0000\u0000\u02b0\u02ae\u0001"+
		"\u0000\u0000\u0000\u02b0\u02b1\u0001\u0000\u0000\u0000\u02b1\u02b3\u0001"+
		"\u0000\u0000\u0000\u02b2\u02b0\u0001\u0000\u0000\u0000\u02b3\u02b4\u0005"+
		"Y\u0000\u0000\u02b4w\u0001\u0000\u0000\u0000\u02b5\u02b7\u0003\u0086C"+
		"\u0000\u02b6\u02b8\u0003N\'\u0000\u02b7\u02b6\u0001\u0000\u0000\u0000"+
		"\u02b7\u02b8\u0001\u0000\u0000\u0000\u02b8y\u0001\u0000\u0000\u0000\u02b9"+
		"\u02bb\u0003\u0004\u0002\u0000\u02ba\u02b9\u0001\u0000\u0000\u0000\u02ba"+
		"\u02bb\u0001\u0000\u0000\u0000\u02bb\u02bc\u0001\u0000\u0000\u0000\u02bc"+
		"\u02be\u0003~?\u0000\u02bd\u02bf\u0003\u0004\u0002\u0000\u02be\u02bd\u0001"+
		"\u0000\u0000\u0000\u02be\u02bf\u0001\u0000\u0000\u0000\u02bf\u02ca\u0001"+
		"\u0000\u0000\u0000\u02c0\u02c2\u0003\u0094J\u0000\u02c1\u02c3\u0003\u0004"+
		"\u0002\u0000\u02c2\u02c1\u0001\u0000\u0000\u0000\u02c2\u02c3\u0001\u0000"+
		"\u0000\u0000\u02c3\u02c4\u0001\u0000\u0000\u0000\u02c4\u02c6\u0003~?\u0000"+
		"\u02c5\u02c7\u0003\u0004\u0002\u0000\u02c6\u02c5\u0001\u0000\u0000\u0000"+
		"\u02c6\u02c7\u0001\u0000\u0000\u0000\u02c7\u02c9\u0001\u0000\u0000\u0000"+
		"\u02c8\u02c0\u0001\u0000\u0000\u0000\u02c9\u02cc\u0001\u0000\u0000\u0000"+
		"\u02ca\u02c8\u0001\u0000\u0000\u0000\u02ca\u02cb\u0001\u0000\u0000\u0000"+
		"\u02cb\u02ce\u0001\u0000\u0000\u0000\u02cc\u02ca\u0001\u0000\u0000\u0000"+
		"\u02cd\u02cf\u0003\u0094J\u0000\u02ce\u02cd\u0001\u0000\u0000\u0000\u02ce"+
		"\u02cf\u0001\u0000\u0000\u0000\u02cf\u02d1\u0001\u0000\u0000\u0000\u02d0"+
		"\u02d2\u0003\u0004\u0002\u0000\u02d1\u02d0\u0001\u0000\u0000\u0000\u02d1"+
		"\u02d2\u0001\u0000\u0000\u0000\u02d2{\u0001\u0000\u0000\u0000\u02d3\u02d5"+
		"\u0003\u0004\u0002\u0000\u02d4\u02d3\u0001\u0000\u0000\u0000\u02d4\u02d5"+
		"\u0001\u0000\u0000\u0000\u02d5\u02d6\u0001\u0000\u0000\u0000\u02d6\u02d8"+
		"\u0003\u0084B\u0000\u02d7\u02d9\u0003\u0004\u0002\u0000\u02d8\u02d7\u0001"+
		"\u0000\u0000\u0000\u02d8\u02d9\u0001\u0000\u0000\u0000\u02d9\u02e4\u0001"+
		"\u0000\u0000\u0000\u02da\u02dc\u0003\u0094J\u0000\u02db\u02dd\u0003\u0004"+
		"\u0002\u0000\u02dc\u02db\u0001\u0000\u0000\u0000\u02dc\u02dd\u0001\u0000"+
		"\u0000\u0000\u02dd\u02de\u0001\u0000\u0000\u0000\u02de\u02e0\u0003\u0084"+
		"B\u0000\u02df\u02e1\u0003\u0004\u0002\u0000\u02e0\u02df\u0001\u0000\u0000"+
		"\u0000\u02e0\u02e1\u0001\u0000\u0000\u0000\u02e1\u02e3\u0001\u0000\u0000"+
		"\u0000\u02e2\u02da\u0001\u0000\u0000\u0000\u02e3\u02e6\u0001\u0000\u0000"+
		"\u0000\u02e4\u02e2\u0001\u0000\u0000\u0000\u02e4\u02e5\u0001\u0000\u0000"+
		"\u0000\u02e5\u02e8\u0001\u0000\u0000\u0000\u02e6\u02e4\u0001\u0000\u0000"+
		"\u0000\u02e7\u02e9\u0003\u0094J\u0000\u02e8\u02e7\u0001\u0000\u0000\u0000"+
		"\u02e8\u02e9\u0001\u0000\u0000\u0000\u02e9\u02eb\u0001\u0000\u0000\u0000"+
		"\u02ea\u02ec\u0003\u0004\u0002\u0000\u02eb\u02ea\u0001\u0000\u0000\u0000"+
		"\u02eb\u02ec\u0001\u0000\u0000\u0000\u02ec}\u0001\u0000\u0000\u0000\u02ed"+
		"\u02f0\u0003\u0080@\u0000\u02ee\u02f0\u0003p8\u0000\u02ef\u02ed\u0001"+
		"\u0000\u0000\u0000\u02ef\u02ee\u0001\u0000\u0000\u0000\u02f0\u007f\u0001"+
		"\u0000\u0000\u0000\u02f1\u02f3\u0003\u0086C\u0000\u02f2\u02f4\u0003\u0004"+
		"\u0002\u0000\u02f3\u02f2\u0001\u0000\u0000\u0000\u02f3\u02f4\u0001\u0000"+
		"\u0000\u0000\u02f4\u02f6\u0001\u0000\u0000\u0000\u02f5\u02f7\u0003N\'"+
		"\u0000\u02f6\u02f5\u0001\u0000\u0000\u0000\u02f6\u02f7\u0001\u0000\u0000"+
		"\u0000\u02f7\u02f8\u0001\u0000\u0000\u0000\u02f8\u02f9\u0003\u0088D\u0000"+
		"\u02f9\u0081\u0001\u0000\u0000\u0000\u02fa\u02fb\u0003p8\u0000\u02fb\u0083"+
		"\u0001\u0000\u0000\u0000\u02fc\u02fd\u0003\u0086C\u0000\u02fd\u0085\u0001"+
		"\u0000\u0000\u0000\u02fe\u0301\u0003\u0098L\u0000\u02ff\u0301\u0005[\u0000"+
		"\u0000\u0300\u02fe\u0001\u0000\u0000\u0000\u0300\u02ff\u0001\u0000\u0000"+
		"\u0000\u0301\u0087\u0001\u0000\u0000\u0000\u0302\u0309\u0003\u008cF\u0000"+
		"\u0303\u0309\u0003\u008eG\u0000\u0304\u0309\u0003\u0090H\u0000\u0305\u0309"+
		"\u0003\u0092I\u0000\u0306\u0309\u0003\u0086C\u0000\u0307\u0309\u0003\u008a"+
		"E\u0000\u0308\u0302\u0001\u0000\u0000\u0000\u0308\u0303\u0001\u0000\u0000"+
		"\u0000\u0308\u0304\u0001\u0000\u0000\u0000\u0308\u0305\u0001\u0000\u0000"+
		"\u0000\u0308\u0306\u0001\u0000\u0000\u0000\u0308\u0307\u0001\u0000\u0000"+
		"\u0000\u0309\u0089\u0001\u0000\u0000\u0000\u030a\u030b\u0005)\u0000\u0000"+
		"\u030b\u008b\u0001\u0000\u0000\u0000\u030c\u0319\u0005\u000b\u0000\u0000"+
		"\u030d\u0319\u0005\b\u0000\u0000\u030e\u0319\u0005\t\u0000\u0000\u030f"+
		"\u0319\u0005\n\u0000\u0000\u0310\u0319\u0005&\u0000\u0000\u0311\u0319"+
		"\u0005#\u0000\u0000\u0312\u0319\u0005$\u0000\u0000\u0313\u0319\u0005%"+
		"\u0000\u0000\u0314\u0316\u0007\t\u0000\u0000\u0315\u0317\u0003.\u0017"+
		"\u0000\u0316\u0315\u0001\u0000\u0000\u0000\u0316\u0317\u0001\u0000\u0000"+
		"\u0000\u0317\u0319\u0001\u0000\u0000\u0000\u0318\u030c\u0001\u0000\u0000"+
		"\u0000\u0318\u030d\u0001\u0000\u0000\u0000\u0318\u030e\u0001\u0000\u0000"+
		"\u0000\u0318\u030f\u0001\u0000\u0000\u0000\u0318\u0310\u0001\u0000\u0000"+
		"\u0000\u0318\u0311\u0001\u0000\u0000\u0000\u0318\u0312\u0001\u0000\u0000"+
		"\u0000\u0318\u0313\u0001\u0000\u0000\u0000\u0318\u0314\u0001\u0000\u0000"+
		"\u0000\u0319\u008d\u0001\u0000\u0000\u0000\u031a\u031b\u0007\n\u0000\u0000"+
		"\u031b\u031c\u0003.\u0017\u0000\u031c\u008f\u0001\u0000\u0000\u0000\u031d"+
		"\u031f\u0005 \u0000\u0000\u031e\u0320\u0003.\u0017\u0000\u031f\u031e\u0001"+
		"\u0000\u0000\u0000\u031f\u0320\u0001\u0000\u0000\u0000\u0320\u0091\u0001"+
		"\u0000\u0000\u0000\u0321\u0323\u0005\r\u0000\u0000\u0322\u0324\u0003."+
		"\u0017\u0000\u0323\u0322\u0001\u0000\u0000\u0000\u0323\u0324\u0001\u0000"+
		"\u0000\u0000\u0324\u0093\u0001\u0000\u0000\u0000\u0325\u0326\u0005W\u0000"+
		"\u0000\u0326\u0095\u0001\u0000\u0000\u0000\u0327\u0329\u0003\u0004\u0002"+
		"\u0000\u0328\u0327\u0001\u0000\u0000\u0000\u0328\u0329\u0001\u0000\u0000"+
		"\u0000\u0329\u032a\u0001\u0000\u0000\u0000\u032a\u032b\u0005\u0000\u0000"+
		"\u0001\u032b\u0097\u0001\u0000\u0000\u0000\u032c\u032d\u0007\u000b\u0000"+
		"\u0000\u032d\u0099\u0001\u0000\u0000\u0000q\u009d\u00a4\u00a9\u00af\u00bc"+
		"\u00c6\u00ca\u00cf\u00d3\u00d9\u00dd\u00e2\u00e6\u00ef\u00f3\u00f6\u00fa"+
		"\u00fe\u0104\u010b\u0111\u0115\u0118\u011b\u011f\u0123\u0126\u0129\u0136"+
		"\u013a\u013d\u0140\u0143\u0146\u014b\u014f\u0153\u015a\u015f\u0165\u016a"+
		"\u0174\u017a\u017e\u0182\u0189\u018e\u0192\u0198\u01a1\u01a8\u01ac\u01b0"+
		"\u01b6\u01ba\u01c0\u01c4\u01ca\u01ce\u01d4\u01d8\u01de\u01e2\u01e8\u01ec"+
		"\u01f2\u01f6\u01fc\u0200\u0206\u020a\u0210\u0214\u0218\u021a\u0221\u0229"+
		"\u0233\u0253\u025f\u0263\u026a\u028b\u028f\u0293\u029c\u02a5\u02b0\u02b7"+
		"\u02ba\u02be\u02c2\u02c6\u02ca\u02ce\u02d1\u02d4\u02d8\u02dc\u02e0\u02e4"+
		"\u02e8\u02eb\u02ef\u02f3\u02f6\u0300\u0308\u0316\u0318\u031f\u0323\u0328";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}