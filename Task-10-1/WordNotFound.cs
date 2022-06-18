using System;

namespace Translator
{
    class WordNotFound : Exception
    {
        private string _word;

        public WordNotFound(string word) : base("A word not found in the vocabulary") // the string for default usage
        {
            _word = word;
        }

        public string GetWord()
        {
            return _word;
        }

    }
}
