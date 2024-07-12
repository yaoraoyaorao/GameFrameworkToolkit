using System.Text.RegularExpressions;

namespace GameFramework.Toolkit.Runtime
{
    internal class StringArrayArgs : ICommandArgs
    {
        public object ToCommand(string args)
        {
            if (!IsValidFormat(args))
                return null;

            args = args.Substring(1, args.Length - 2);

            string[] pos = args.Split(',');

            return pos;
        }

        private bool IsValidFormat(string input)
        {
            Regex regex = new Regex(@"\[(?:[^,\]]+,)*[^,\]]+\]");
            return regex.IsMatch(input);
        }
    }
}
