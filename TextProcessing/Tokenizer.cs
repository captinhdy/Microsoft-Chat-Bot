using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TextProcessing
{
    [Serializable]
    public class Tokenizer
    {
        
        public List<string> Keywords;
        List<string> tokens;
        string[] stopWords;
        
        public Tokenizer()
        {
            tokens = new List<string>();
            Keywords = new List<string>();
            stopWords = GetStopWords();
        }

        public List<string> TokenizeString(string TokenString)
        {
            tokens.Clear();

            string[] tokenString = TokenString.Split(' ');

            foreach(string token in tokenString)
            {
                if (ValidWord(token))
                {
                    
                    tokens.Add(token.ToLower());
                }
            }


            return tokens;
        }

        public string StemToken(string Token)
        {
            return Token;
        }

        public int ConvertTokenToInteger(string Token)
        {
            byte[] array = Encoding.UTF8.GetBytes(Token);

            if(array.Length < 4)
            {
                byte[] temp = new byte[4];
                array.CopyTo(temp, 0);
                array = temp;
            }
            int integer = BitConverter.ToInt32(array, 0);
            return integer;

        }

        private bool ValidWord(string Token)
        {
            foreach (string stopWord in stopWords)
            {
                if(Token == stopWord)
                    return false;
            }

            return true;
        }

        private string[] GetStopWords()
        {
            string line = "";
            List<string> words = new List<string>();
            FileProcessing fileProcessing = new FileProcessing();

            words = fileProcessing.ReadTextFileLines("stopwords.txt").ToList();

            return words.ToArray();
        }
    }
}
