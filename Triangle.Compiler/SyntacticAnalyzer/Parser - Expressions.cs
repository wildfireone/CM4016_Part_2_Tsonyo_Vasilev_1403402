
using Triangle.Compiler.SyntaxTrees.Expressions;

namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {
        ///////////////////////////////////////////////////////////////////////////////
        //
        // EXPRESSIONS
        //
        ///////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Parses the expression, and constructs an AST to represent its phrase
        /// structure.
        /// </summary>
        /// <returns>
        /// an <link>Triangle.SyntaxTrees.Expressions.Expression</link>
        /// </returns> 
        /// <throws type="SyntaxError">
        /// a syntactic error
        /// </throws>
        Expression ParseExpression()
        {

            var startLocation = _currentToken.Start;

            switch (_currentToken.Kind)
            {

                case TokenKind.Let:
                    {
                        AcceptIt();
                        var declaration = ParseDeclaration();
                        Accept(TokenKind.In);
                        var expression = ParseExpression();
                        var expressionPos = new SourcePosition(startLocation, _currentToken.Finish);
                        return new LetExpression(declaration, expression, expressionPos);
                    }

                case TokenKind.If:
                    {
                        AcceptIt();
                        var expression = ParseExpression();
                        Accept(TokenKind.Then);
                        var expression2 = ParseExpression();
                        Accept(TokenKind.Else);
                        var expression3 = ParseExpression();
                        var expressionPos = new SourcePosition(startLocation, _currentToken.Finish);
                        return new IfExpression(expression, expression2, expression3, expressionPos);
                    }

                default:
                    {                       
                        return ParseSecondaryExpression(); ;
                    }
            }
        }

        /// <summary>
        // Parses the secondary expression, and constructs an AST to represent its
        /// phrase structure.
        /// </summary>
        /// <returns>
        /// an <link>Triangle.SyntaxTrees.Expressions.Expression</link>
        /// </returns>
        /// <throws type="SyntaxError">
        /// a syntactic error
        /// </throws>
        Expression ParseSecondaryExpression()
        {

            var startLocation = _currentToken.Start;
            var primaryExpression = ParsePrimaryExpression();
            while (_currentToken.Kind == TokenKind.Operator)
            {
                var op = ParseOperator();
                var primaryExpression2 = ParsePrimaryExpression();
                var expressionPos = new SourcePosition(startLocation, _currentToken.Finish);
                primaryExpression = new BinaryExpression(primaryExpression, op, primaryExpression2, expressionPos);
            }
            return primaryExpression;
        }

        /// <summary>
        /// Parses the primary expression, and constructs an AST to represent its
        /// phrase structure.
        /// </summary>
        /// <returns>
        /// an <link>Triangle.SyntaxTrees.Expressions.Expression</link>
        /// </returns>
        /// <throws type="SyntaxError">
        /// a syntactic error
        /// </throws>
        Expression ParsePrimaryExpression()
        {

            var startlocation = _currentToken.Start;
            switch (_currentToken.Kind)
            {

                case TokenKind.IntLiteral:
                    {
                        var integer = ParseIntegerLiteral();
                        var expressionPos = new SourcePosition(startlocation, _currentToken.Finish);
                        return new IntegerExpression(integer, expressionPos);
                    }

                case TokenKind.CharLiteral:
                    {
                        var character = ParseCharacterLiteral();
                        var expressionPos = new SourcePosition(startlocation, _currentToken.Finish);
                        return new CharacterExpression(character, expressionPos);
                    }


                case TokenKind.Identifier:
                    {
                        var identifier = ParseIdentifier();
                        if (_currentToken.Kind == TokenKind.LeftParen)
                        {
                            AcceptIt();
                            var actualParameterSequence = ParseActualParameterSequence();
                            Accept(TokenKind.RightParen);
                            var expressionPos = new SourcePosition(startlocation, _currentToken.Finish);
                            return new CallExpression(identifier, actualParameterSequence, expressionPos);
                        }
                        else
                        {
                            var vname = ParseRestOfVname(identifier);
                            var expressionPos = new SourcePosition(startlocation, _currentToken.Finish);
                            return new VnameExpression(vname, expressionPos);
                        }
                    }

                case TokenKind.Operator:
                    {
                        var op = ParseOperator();
                        var primaryExpression = ParsePrimaryExpression();
                        var expressionPos = new SourcePosition(startlocation, _currentToken.Finish);
                        return new UnaryExpression(op, primaryExpression, expressionPos);
                    }

                case TokenKind.LeftParen:
                    {
                        AcceptIt();
                        var expression = ParseExpression();
                        Accept(TokenKind.RightParen);
                        var expressionPos = new SourcePosition(startlocation, _currentToken.Finish);
                        return expression;
                    }

                default:
                    {
                        RaiseSyntacticError("\"%\" cannot start an expression", _currentToken);
                        return null;
                    }
            }
        }

    }
}