using System;
using System.IO;
using System.Text;
using Mnk.Library.Common.Base.Log;
using Mnk.TBox.Core.PluginsShared.LongPaths;
using Mnk.TBox.Plugins.Searcher.Code.Finders.Scanner;
using Mnk.TBox.Plugins.Searcher.Code.Settings;

namespace Mnk.TBox.Plugins.Searcher.Code.Finders.Parsers
{
    sealed class Parser : IParser
    {
        private ILog log = LogManager.GetLogger<Parser>();
        private readonly IAdder adder;
        private readonly IndexSettings settings;
        public char[] Whitespaces = { ' ', '\t', '\r', '\n' };

        private static bool IsSpace(char ch)
        {
            return (ch == ' ') || (ch == '\t') || (ch == '\n') || (ch == '\r');
        }

        private static void GoFirstNoSpace(string file, ref int i)
        {
            while (i < file.Length && IsSpace(file[i])) { i++; }
        }

        private static bool SkipComments(string file, ref int i)
        {
            var ch = file[i];
            if (ch != '/') return false;
            if (i + 1 >= file.Length) return false;
            if (file[i + 1] == '/')
            {
                i += 2;
                while (i < file.Length)
                {
                    ch = file[i];
                    if (ch == '\r' || ch == '\n')
                        break;
                    i++;
                }
                return true;
            }
            if (file[i + 1] != '*') return false;
            i += 2;
            while (i < file.Length)
            {
                ch = file[i];
                if (ch == '*')
                {
                    if (i + 1 < file.Length && file[i + 1] == '/')
                    {
                        i++;
                        break;
                    }
                }
                i++;
            }
            return true;
        }

        private static bool IsString(char ch)
        {
            return ch == '\'' || ch == '"';
        }

        private void DecodeString(string word, int begin, int fileId)
        {
            var id = 0;
            do
            {
                var prevId = id;
                id = word.IndexOfAny(Whitespaces, id);
                if (id < 0)
                    id = word.Length;
                if (prevId == id)
                    break;
                adder.AddWord(word.Substring(prevId, id - prevId), fileId);
                GoFirstNoSpace(word, ref id);
            } while (true);
        }

        private bool ExtractString(string file, int fileId, ref int i)
        {
            var strId = file[i];
            if (!IsString(file[i])) return false;
            i++;
            var begin = i;
            while (i < file.Length && (strId != file[i])) { i++; }
            if (i != begin)
            {
                DecodeString(file.Substring(begin, i - begin), begin, fileId);
                i++;
                return true;
            }
            i++;
            return false;
        }

        private static bool IsWord(char ch)
        {
            return (ch >= 'a' && ch <= 'z') ||
                    (ch >= 'A' && ch <= 'Z') ||
                    (ch >= 'а' && ch <= 'я') || ch == 'ё' ||
                    (ch >= 'А' && ch <= 'Я') || ch == 'Ё' ||
                    (ch >= '0' && ch <= '9') ||
                    (ch == '_');
        }

        private bool ExtractWord(string file, int fileId, ref int i)
        {
            if (i >= file.Length || !IsWord(file[i])) return false;
            var begin = i;
            i++;
            while (i < file.Length && IsWord(file[i])) { i++; }
            adder.AddWord(file.Substring(begin, i - begin), fileId);
            return true;
        }

        public Parser(IAdder adder, IndexSettings settings)
        {
            this.adder = adder;
            this.settings = settings;
        }

        public bool Parse(AddInfo info)
        {
            try
            {
                using (var f = LongPathExtensions.OpenRead(info.Path))
                {
                    using (var s = new StreamReader(f, Encoding.UTF8))
                    {
                        while (!s.EndOfStream)
                        {
                            ParseFileData(s.ReadLine(), info.Id);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Write(ex, "Can't parse: " + info.Path);
                return false;
            }
            return true;
        }

        public void ParseFileData(string data, int fileId)
        {
            var i = 0;
            while (i < data.Length)
            {
                GoFirstNoSpace(data, ref i);
                if (i >= data.Length) continue;
                if (settings.SkipComments && SkipComments(data, ref i)) continue;
                if (settings.DecodeStrings && ExtractString(data, fileId, ref i))
                    continue;
                if (ExtractWord(data, fileId, ref i))
                    continue;
                i++;
            }
        }
    }
}
