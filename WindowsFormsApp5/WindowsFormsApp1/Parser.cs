
using System;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.goldparser.lalr;
using com.calitha.commons;
using System.Windows.Forms;
using System.Collections.Generic;

namespace com.calitha.goldparser
{

    [Serializable()]
    public class SymbolException : System.Exception
    {
        public SymbolException(string message) : base(message)
        {
        }

        public SymbolException(string message,
            Exception inner) : base(message, inner)
        {
        }

        protected SymbolException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

    }

    [Serializable()]
    public class RuleException : System.Exception
    {

        public RuleException(string message) : base(message)
        {
        }

        public RuleException(string message,
                             Exception inner) : base(message, inner)
        {
        }

        protected RuleException(SerializationInfo info,
                                StreamingContext context) : base(info, context)
        {
        }

    }

    enum SymbolConstants : int
    {
        SYMBOL_EOF              =  0, // (EOF)
        SYMBOL_ERROR            =  1, // (Error)
        SYMBOL_WHITESPACE       =  2, // Whitespace
        SYMBOL_MINUS            =  3, // '-'
        SYMBOL_MINUSMINUS       =  4, // '--'
        SYMBOL_NUMDOT           =  5, // '#.'
        SYMBOL_DOLLAR           =  6, // '$'
        SYMBOL_PERCENT          =  7, // '%'
        SYMBOL_LPAREN           =  8, // '('
        SYMBOL_RPAREN           =  9, // ')'
        SYMBOL_TIMES            = 10, // '*'
        SYMBOL_TIMESTIMES       = 11, // '**'
        SYMBOL_DOT              = 12, // '.'
        SYMBOL_DOTNUM           = 13, // '.#'
        SYMBOL_DOTEQ            = 14, // '.='
        SYMBOL_DIV              = 15, // '/'
        SYMBOL_COLONCOLON       = 16, // '::'
        SYMBOL_LBRACKETRBRACKET = 17, // '[]'
        SYMBOL_LBRACE           = 18, // '{'
        SYMBOL_RBRACE           = 19, // '}'
        SYMBOL_PLUS             = 20, // '+'
        SYMBOL_PLUSPLUS         = 21, // '++'
        SYMBOL_LT               = 22, // '<'
        SYMBOL_LTEQ             = 23, // '<='
        SYMBOL_EQ               = 24, // '='
        SYMBOL_EQEQ             = 25, // '=='
        SYMBOL_GT               = 26, // '>'
        SYMBOL_GTEQ             = 27, // '>='
        SYMBOL_BEGIN            = 28, // begin
        SYMBOL_DATA             = 29, // data
        SYMBOL_DG               = 30, // Dg
        SYMBOL_ELSE             = 31, // else
        SYMBOL_FINISH           = 32, // finish
        SYMBOL_FLOAT            = 33, // float
        SYMBOL_FUN              = 34, // fun
        SYMBOL_ID               = 35, // Id
        SYMBOL_IF               = 36, // if
        SYMBOL_INT              = 37, // int
        SYMBOL_LOOP             = 38, // Loop
        SYMBOL_OPEN             = 39, // open
        SYMBOL_SHUTDOWN         = 40, // shutdown
        SYMBOL_STRING           = 41, // string
        SYMBOL_ASSIGN           = 42, // <assign>
        SYMBOL_CALLMETHOD       = 43, // <callmethod>
        SYMBOL_CODE             = 44, // <code>
        SYMBOL_COMMENT          = 45, // <comment>
        SYMBOL_CON              = 46, // <con>
        SYMBOL_DATATYPE         = 47, // <datatype>
        SYMBOL_DEFINITION       = 48, // <definition>
        SYMBOL_DIGIT            = 49, // <digit>
        SYMBOL_DIV2             = 50, // <div>
        SYMBOL_EXPR             = 51, // <expr>
        SYMBOL_FACT             = 52, // <fact>
        SYMBOL_ID2              = 53, // <id>
        SYMBOL_IF_STM           = 54, // <if_stm>
        SYMBOL_LOOP_STM         = 55, // <loop_stm>
        SYMBOL_METHOD           = 56, // <method>
        SYMBOL_NEG              = 57, // <neg>
        SYMBOL_OP               = 58, // <op>
        SYMBOL_SIMPLE           = 59, // <Simple>
        SYMBOL_STR              = 60, // <str>
        SYMBOL_VALUE            = 61, // <value>
        SYMBOL_VERB             = 62  // <verb>
    };

    enum RuleConstants : int
    {
        RULE_SIMPLE_OPEN_LBRACE_RBRACE_SHUTDOWN                              =  0, // <Simple> ::= open '{' <code> '}' shutdown
        RULE_CODE                                                            =  1, // <code> ::= <comment>
        RULE_CODE2                                                           =  2, // <code> ::= <definition>
        RULE_CODE3                                                           =  3, // <code> ::= <definition> <code>
        RULE_CODE4                                                           =  4, // <code> ::= <comment> <definition> <code>
        RULE_COMMENT_DOTNUM_NUMDOT                                           =  5, // <comment> ::= '.#' <id> '#.'
        RULE_ID_ID                                                           =  6, // <id> ::= Id
        RULE_DEFINITION                                                      =  7, // <definition> ::= <method>
        RULE_DEFINITION2                                                     =  8, // <definition> ::= <callmethod>
        RULE_DEFINITION3                                                     =  9, // <definition> ::= <assign>
        RULE_DEFINITION4                                                     = 10, // <definition> ::= <if_stm>
        RULE_DEFINITION5                                                     = 11, // <definition> ::= <loop_stm>
        RULE_METHOD_FUN_LBRACKETRBRACKET_COLONCOLON_BEGIN_FINISH_DOT         = 12, // <method> ::= fun <id> '[]' '::' begin <definition> finish '.'
        RULE_CALLMETHOD_FUN_LBRACKETRBRACKET_DOT                             = 13, // <callmethod> ::= fun <id> '[]' '.'
        RULE_ASSIGN_DATA_LPAREN_RPAREN_EQ_DOT                                = 14, // <assign> ::= data '(' <id> ')' <datatype> '=' <expr> '.'
        RULE_DATATYPE_INT                                                    = 15, // <datatype> ::= int
        RULE_DATATYPE_FLOAT                                                  = 16, // <datatype> ::= float
        RULE_DATATYPE_STRING                                                 = 17, // <datatype> ::= string
        RULE_EXPR_PLUS                                                       = 18, // <expr> ::= <expr> '+' <div>
        RULE_EXPR_MINUS                                                      = 19, // <expr> ::= <expr> '-' <div>
        RULE_EXPR                                                            = 20, // <expr> ::= <div>
        RULE_DIV_TIMES                                                       = 21, // <div> ::= <div> '*' <fact>
        RULE_DIV_DIV                                                         = 22, // <div> ::= <div> '/' <fact>
        RULE_DIV_PERCENT                                                     = 23, // <div> ::= <div> '%' <fact>
        RULE_DIV                                                             = 24, // <div> ::= <fact>
        RULE_FACT_TIMESTIMES                                                 = 25, // <fact> ::= <fact> '**' <neg>
        RULE_FACT                                                            = 26, // <fact> ::= <neg>
        RULE_NEG_MINUS                                                       = 27, // <neg> ::= '-' <value>
        RULE_NEG                                                             = 28, // <neg> ::= <value>
        RULE_VALUE_LPAREN_RPAREN                                             = 29, // <value> ::= '(' <value> ')'
        RULE_VALUE                                                           = 30, // <value> ::= <id>
        RULE_VALUE2                                                          = 31, // <value> ::= <digit>
        RULE_VALUE3                                                          = 32, // <value> ::= <str>
        RULE_STR_DOLLAR                                                      = 33, // <str> ::= '$' <id>
        RULE_STR_DOLLAR2                                                     = 34, // <str> ::= <digit> '$'
        RULE_DIGIT_DG                                                        = 35, // <digit> ::= Dg
        RULE_IF_STM_IF_LT_GT_COLONCOLON_BEGIN_FINISH_DOT                     = 36, // <if_stm> ::= if '<' <con> '>' '::' begin <definition> finish '.'
        RULE_IF_STM_IF_LT_GT_COLONCOLON_BEGIN_FINISH_DOT_ELSE_COLONCOLON_DOT = 37, // <if_stm> ::= if '<' <con> '>' '::' begin <definition> finish '.' else '::' <definition> '.'
        RULE_CON                                                             = 38, // <con> ::= <expr> <op> <expr>
        RULE_OP_LT                                                           = 39, // <op> ::= '<'
        RULE_OP_DOTEQ                                                        = 40, // <op> ::= '.='
        RULE_OP_EQEQ                                                         = 41, // <op> ::= '=='
        RULE_OP_GT                                                           = 42, // <op> ::= '>'
        RULE_OP_LTEQ                                                         = 43, // <op> ::= '<='
        RULE_OP_GTEQ                                                         = 44, // <op> ::= '>='
        RULE_LOOP_STM_LOOP_LT_GT_BEGIN_FINISH_DOT                            = 45, // <loop_stm> ::= <assign> Loop '<' <con> '>' begin <definition> <verb> finish '.'
        RULE_VERB_MINUSMINUS                                                 = 46, // <verb> ::= '--' <id>
        RULE_VERB_MINUSMINUS2                                                = 47, // <verb> ::= <id> '--'
        RULE_VERB_PLUSPLUS                                                   = 48, // <verb> ::= '++' <id>
        RULE_VERB_PLUSPLUS2                                                  = 49, // <verb> ::= <id> '++'
        RULE_VERB                                                            = 50  // <verb> ::= <assign>
    };

    public class MyParser
    {
        private LALRParser parser;
        ListBox lst;
        ListBox l2;
        public MyParser(string filename,ListBox lst,ListBox l2)
        {
            FileStream stream = new FileStream(filename,
                                               FileMode.Open, 
                                               FileAccess.Read, 
                                               FileShare.Read);
            this.lst = lst;
            this.l2 = l2;
            Init(stream);
            stream.Close();
        }

        public MyParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public MyParser(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            CGTReader reader = new CGTReader(stream);
            parser = reader.CreateNewParser();
            parser.TrimReductions = false;
            parser.StoreTokens = LALRParser.StoreTokensMode.NoUserObject;

            parser.OnTokenError += new LALRParser.TokenErrorHandler(TokenErrorEvent);
            parser.OnParseError += new LALRParser.ParseErrorHandler(ParseErrorEvent);
            parser.OnTokenRead += new LALRParser.TokenReadHandler(TokenReadEvent);
        }

        public void Parse(string source)
        {
            NonterminalToken token = parser.Parse(source);
            if (token != null)
            {
                Object obj = CreateObject(token);
                //todo: Use your object any way you like
            }
        }

        private Object CreateObject(Token token)
        {
            if (token is TerminalToken)
                return CreateObjectFromTerminal((TerminalToken)token);
            else
                return CreateObjectFromNonterminal((NonterminalToken)token);
        }

        private Object CreateObjectFromTerminal(TerminalToken token)
        {
            switch (token.Symbol.Id)
            {
                case (int)SymbolConstants.SYMBOL_EOF :
                //(EOF)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ERROR :
                //(Error)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //Whitespace
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MINUS :
                //'-'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MINUSMINUS :
                //'--'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NUMDOT :
                //'#.'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DOLLAR :
                //'$'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PERCENT :
                //'%'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LPAREN :
                //'('
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RPAREN :
                //')'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TIMES :
                //'*'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TIMESTIMES :
                //'**'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DOT :
                //'.'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DOTNUM :
                //'.#'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DOTEQ :
                //'.='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIV :
                //'/'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COLONCOLON :
                //'::'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LBRACKETRBRACKET :
                //'[]'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LBRACE :
                //'{'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RBRACE :
                //'}'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PLUS :
                //'+'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PLUSPLUS :
                //'++'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LT :
                //'<'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LTEQ :
                //'<='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQ :
                //'='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQEQ :
                //'=='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GT :
                //'>'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GTEQ :
                //'>='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BEGIN :
                //begin
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DATA :
                //data
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DG :
                //Dg
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ELSE :
                //else
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FINISH :
                //finish
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FLOAT :
                //float
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FUN :
                //fun
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ID :
                //Id
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IF :
                //if
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INT :
                //int
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LOOP :
                //Loop
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_OPEN :
                //open
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SHUTDOWN :
                //shutdown
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STRING :
                //string
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ASSIGN :
                //<assign>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CALLMETHOD :
                //<callmethod>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CODE :
                //<code>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COMMENT :
                //<comment>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CON :
                //<con>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DATATYPE :
                //<datatype>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DEFINITION :
                //<definition>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIGIT :
                //<digit>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIV2 :
                //<div>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPR :
                //<expr>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FACT :
                //<fact>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ID2 :
                //<id>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IF_STM :
                //<if_stm>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LOOP_STM :
                //<loop_stm>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_METHOD :
                //<method>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NEG :
                //<neg>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_OP :
                //<op>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SIMPLE :
                //<Simple>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STR :
                //<str>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VALUE :
                //<value>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VERB :
                //<verb>
                //todo: Create a new object that corresponds to the symbol
                return null;

            }
            throw new SymbolException("Unknown symbol");
        }

        public Object CreateObjectFromNonterminal(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_SIMPLE_OPEN_LBRACE_RBRACE_SHUTDOWN :
                //<Simple> ::= open '{' <code> '}' shutdown
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CODE :
                //<code> ::= <comment>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CODE2 :
                //<code> ::= <definition>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CODE3 :
                //<code> ::= <definition> <code>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CODE4 :
                //<code> ::= <comment> <definition> <code>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_COMMENT_DOTNUM_NUMDOT :
                //<comment> ::= '.#' <id> '#.'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ID_ID :
                //<id> ::= Id
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DEFINITION :
                //<definition> ::= <method>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DEFINITION2 :
                //<definition> ::= <callmethod>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DEFINITION3 :
                //<definition> ::= <assign>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DEFINITION4 :
                //<definition> ::= <if_stm>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DEFINITION5 :
                //<definition> ::= <loop_stm>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_METHOD_FUN_LBRACKETRBRACKET_COLONCOLON_BEGIN_FINISH_DOT :
                //<method> ::= fun <id> '[]' '::' begin <definition> finish '.'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CALLMETHOD_FUN_LBRACKETRBRACKET_DOT :
                //<callmethod> ::= fun <id> '[]' '.'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGN_DATA_LPAREN_RPAREN_EQ_DOT :
                //<assign> ::= data '(' <id> ')' <datatype> '=' <expr> '.'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DATATYPE_INT :
                //<datatype> ::= int
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DATATYPE_FLOAT :
                //<datatype> ::= float
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DATATYPE_STRING :
                //<datatype> ::= string
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_PLUS :
                //<expr> ::= <expr> '+' <div>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_MINUS :
                //<expr> ::= <expr> '-' <div>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR :
                //<expr> ::= <div>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DIV_TIMES :
                //<div> ::= <div> '*' <fact>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DIV_DIV :
                //<div> ::= <div> '/' <fact>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DIV_PERCENT :
                //<div> ::= <div> '%' <fact>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DIV :
                //<div> ::= <fact>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FACT_TIMESTIMES :
                //<fact> ::= <fact> '**' <neg>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FACT :
                //<fact> ::= <neg>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_NEG_MINUS :
                //<neg> ::= '-' <value>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_NEG :
                //<neg> ::= <value>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VALUE_LPAREN_RPAREN :
                //<value> ::= '(' <value> ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VALUE :
                //<value> ::= <id>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VALUE2 :
                //<value> ::= <digit>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VALUE3 :
                //<value> ::= <str>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STR_DOLLAR :
                //<str> ::= '$' <id>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STR_DOLLAR2 :
                //<str> ::= <digit> '$'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DIGIT_DG :
                //<digit> ::= Dg
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IF_STM_IF_LT_GT_COLONCOLON_BEGIN_FINISH_DOT :
                //<if_stm> ::= if '<' <con> '>' '::' begin <definition> finish '.'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IF_STM_IF_LT_GT_COLONCOLON_BEGIN_FINISH_DOT_ELSE_COLONCOLON_DOT :
                //<if_stm> ::= if '<' <con> '>' '::' begin <definition> finish '.' else '::' <definition> '.'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CON :
                //<con> ::= <expr> <op> <expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_LT :
                //<op> ::= '<'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_DOTEQ :
                //<op> ::= '.='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_EQEQ :
                //<op> ::= '=='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_GT :
                //<op> ::= '>'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_LTEQ :
                //<op> ::= '<='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_GTEQ :
                //<op> ::= '>='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LOOP_STM_LOOP_LT_GT_BEGIN_FINISH_DOT :
                //<loop_stm> ::= <assign> Loop '<' <con> '>' begin <definition> <verb> finish '.'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VERB_MINUSMINUS :
                //<verb> ::= '--' <id>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VERB_MINUSMINUS2 :
                //<verb> ::= <id> '--'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VERB_PLUSPLUS :
                //<verb> ::= '++' <id>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VERB_PLUSPLUS2 :
                //<verb> ::= <id> '++'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VERB :
                //<verb> ::= <assign>
                //todo: Create a new object using the stored tokens.
                return null;

            }
            throw new RuleException("Unknown rule");
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            string message = "Token error with input: '"+args.Token.ToString()+"'";
            //todo: Report message to UI?
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {
            string message = "Parse error caused by token: '" + args.UnexpectedToken.ToString() + " in line" +
               args.UnexpectedToken.Location.LineNr;
            lst.Items.Add(message);
            string m2 = "expected token = " + args.ExpectedTokens.ToString();
            lst.Items.Add(m2);
            //todo: Report message to UI?
        }

        private void TokenReadEvent(LALRParser p, TokenReadEventArgs a)
        {
            string info = " +"+a.Token.Text + " \t \t" +(SymbolConstants)a.Token.Symbol.Id;
            l2.Items.Add(info);

        }

    }
}
