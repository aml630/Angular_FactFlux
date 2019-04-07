using FactFluxV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactFluxV3.Logic
{
    public class WordLogLogic
    {
        public WordLogs CreateWordLog(int wordId, int articleId)
        {
            using (var db = new FactFluxV3Context())
            {
                WordLogs newWord = new WordLogs()
                {
                    WordId = wordId,
                    ArticleId = articleId,
                    DateAdded = DateTime.UtcNow
                };

                db.WordLogs.Add(newWord);

                db.SaveChanges();

                return newWord;
            }
        }

        public void LogWordsUsed(Article newArticle)
        {
            if (newArticle.ArticleTitle.StartsWith("Dupe--"))
            {
                return;
            }

            var punctuation = newArticle.ArticleTitle.Where(Char.IsPunctuation).Distinct().ToArray();

            var words = newArticle.ArticleTitle.Split().Select(x => x.Trim(punctuation));

            using (FactFluxV3Context db = new FactFluxV3Context())
            {
                foreach (var word in words)
                {
                    CreateWordOrWordLog(newArticle, db, word);
                }
            }
        }

        private void CreateWordOrWordLog(Article newArticle, FactFluxV3Context db, string word)
        {
            string beginning = word + " ";
            string end = " " + word;
            string middle = " " + word + " ";

            var wordList = db.Words.Where(x => x.Word == word || x.Word.StartsWith(beginning) || x.Word.EndsWith(end) || x.Word.Contains(middle)).OrderByDescending(x => x.Word.Length).ToList();

            if (wordList.Count == 0)
            {
                var newWordLogic = new WordLogic();
              
                newWordLogic.CreateWord(word);
            }
            else
            {
                foreach (var wordOrPhrase in wordList)
                {
                    if (newArticle.ArticleTitle.Contains(wordOrPhrase.Word))
                    {
                        IncrementWordCount(wordOrPhrase);

                        db.SaveChanges();
                    }
                }
            }
        }

        private void IncrementWordCount(Words wordToInc)
        {
            if (wordToInc.DateIncremented == null)
            {
                wordToInc.DateIncremented = DateTime.UtcNow;
            }

            if (wordToInc.DateIncremented.Value < DateTime.UtcNow.AddDays(1).Date)
            {
                wordToInc.Daily = 0;
            }

            if (wordToInc.DateIncremented.Value.Month != DateTime.UtcNow.Month)
            {
                wordToInc.Monthly = 0;
            }

            wordToInc.DateIncremented = DateTime.UtcNow;

            wordToInc.Daily++;

            wordToInc.Monthly++;

            wordToInc.Yearly++;
        }
    }
}
