/* 
 * Author   : GitHub@Guila767
 * Modifier : GitHub@KevinZonda
 * Source   : https://github.com/Guila767/GoogleTranslateApi/blob/GoogleTranslateApi_v2/GoogleTranslateApi/GoogleTranslator.cs
 * SRC LIC. : MIT license
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenFreeAPI.Google.Translate.Models
{
    internal struct Block
    {
        public object[] Data { get; }
        public int Elements { get => Data.Length; }
        public int Blocks
        {
            get
            {
                int c = 0;
                foreach (object _data in Data)
                    if (_data is Block)
                        c++;
                return c;
            }
        }
        public Block this[int x]
        {
            get
            {
                if (x > Blocks - 1)
                    throw new IndexOutOfRangeException("Index out of range");
                List<Block> _blocks = new List<Block>();
                foreach (object _data in Data)
                {
                    if (_data is Block block)
                    {
                        //Block block = _data as Block? ?? throw new Exception("Cannot Parse the block at the given index");
                        _blocks.Add(block);
                    }
                }
                return _blocks[x];
            }
        }

        /// <summary>
        /// Creates a Block class 
        /// </summary>
        /// <param name="data">The data string to be parsed</param>
        /// <exception cref="ArgumentException"></exception>
        public Block(string data)
        {
            if (!IsValidData(data))
                throw new ArgumentException("Invalid data string", "data");
            List<object> ldata = new List<object>() { null };
            Queue<char> vs = new Queue<char>(data.ToCharArray());

            while (vs.Count > 0)
            {
                char @char = vs.Dequeue();
                switch (@char)
                {
                    case '[':
                        string nblock = new string(vs.ToArray());
                        int end = 0;
                        /* FIXED - find the correct end*/
                        for (int n = 1; n != 0; end++)
                        {
                            if (nblock[end] == '[')
                                n++;
                            else if (nblock[end] == ']')
                                n--;
                        }
                        vs = new Queue<char>(nblock.Substring(end));
                        ldata[ldata.Count - 1] = new Block(nblock.Substring(0, end - 1));
                        break;
                    case ',':
                        ldata.Add(null);
                        continue;
                    case '"':
                        do
                        {
                            @char = vs.Peek();
                            switch (@char)
                            {
                                case '"':
                                    continue;
                                case '\\':
                                    vs.Dequeue();
                                    goto default;
                                default:
                                    @char = vs.Dequeue();
                                    break;
                            }
                            ldata[ldata.Count - 1] = string.Concat(ldata[ldata.Count - 1], @char);
                        }
                        while (vs.Peek() != '"');
                        vs.Dequeue();
                        break;
                    default:
                        ldata[ldata.Count - 1] = string.Concat(ldata[ldata.Count - 1], @char);
                        break;
                }

            }
            Data = ldata.ToArray();
        }

        private static bool IsValidData(string data)
        {
            if ((data.Count(c => c == '[') + data.Count(c => c == ']')) % 2 != 0)
                return false;
            if (data.Count(c => c == '"') % 2 != 0)
                return false;
            return true;
        }
    }
}
