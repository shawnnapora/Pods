using System;
using System.Linq;
using System.Text;

namespace OddsGenerator
{
    internal class Output
    {
        public void AppendLine(string toAppend)
        {
            Tab();
            _builder.Append(toAppend);
            AppendLine();
        }

        public void IndentAppendLine(string toAppend)
        {
            Indent();
            AppendLine(toAppend);
        }

        public void DedentAppendLine(string toAppend)
        {
            Indent(-1);
            AppendLine(toAppend);
        }
        
        public void AppendLine() => _builder.Append("\n");

        public void Append(string toAppend) => _builder.Append(toAppend);
        
        public void Indent(int toIndent = 1)
        {
            _indent += toIndent;
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

        public void Tab()
        {
            if (_indent == 0)
            {
                return;
            }
            
            _builder.Append(String.Concat(Enumerable.Repeat("    ", _indent)));
        }

        private readonly StringBuilder _builder = new StringBuilder();
        private int _indent = 0;
    }
}