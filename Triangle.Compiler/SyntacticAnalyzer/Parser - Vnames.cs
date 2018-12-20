using Triangle.Compiler.SyntaxTrees.Terminals;
using Triangle.Compiler.SyntaxTrees.Vnames;

namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {

        // /////////////////////////////////////////////////////////////////////////////
        //
        // VALUE-OR-VARIABLE NAMES
        //
        // /////////////////////////////////////////////////////////////////////////////

        /**
         * Parses the v-name, and constructs an AST to represent its phrase structure.
         * 
         * @return a {@link triangle.compiler.syntax.trees.vnames.Vname}
         * 
         * @throws SyntaxError
         *           a syntactic error
         * 
         */
        SimpleVname ParseVname()
        {
            var start = _currentToken.Start;
            var identifier = ParseIdentifier();
            var vNamePosition = new SourcePosition(start, _currentToken.Finish);
            return new SimpleVname(identifier, vNamePosition);
        }

        Vname ParseRestOfVname(Identifier firstIdentifier)
        {
            var startLocation = firstIdentifier.Start;
            Vname vname = new SimpleVname(firstIdentifier, firstIdentifier.Position);
            return vname;
        }

    }
}