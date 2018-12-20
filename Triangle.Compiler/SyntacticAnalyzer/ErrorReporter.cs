/**
 * @Author: Petyo Nachev <petyo>
 * @Date:   6-Nov-2017
 * @Filename: ErrorReporter.cs
 * @Last modified by:   petyo
 * @Last modified time: 6-Nov-2017
 */

using System;

namespace Triangle.Compiler.SyntacticAnalyzer
{
    public class ErrorReporter
    {
        //variable to count the errors
        int errorCount;

        public ErrorReporter()
        {
            errorCount = 0;
        }
        //Report an error method
        //Prints an error and increments the errorCount
        public void ReportError(String message, Token token)
        {
            Console.WriteLine(message + " at line " + token.Location.Start.Line + " index " + token.Location.Start.Column);
            errorCount++;
        }
        //Checks if there are any errors
        Boolean HasErrors()
        {
            return errorCount == 0;
        }
        //Returns the number of errors
        int ErrorCount()
        {
            return errorCount;
        }
    }
}
