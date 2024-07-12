﻿using System.Text.RegularExpressions;
using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    internal class Vector2Args : ICommandArgs
    {
        public object ToCommand(string args)
        {
            if (!IsValidFormat(args))
                return null;

            args = args.Substring(1, args.Length - 2);

            string[] pos = args.Split(',');

            float x = float.Parse(pos[0]);
            float y = float.Parse(pos[1]);

            return new Vector2(x, y);
        }

        private bool IsValidFormat(string input)
        {
            Regex regex = new Regex(@"^\((-?\d+(\.\d+)?),(-?\d+(\.\d+)?)\)$");
            return regex.IsMatch(input);
        }
    }
}
