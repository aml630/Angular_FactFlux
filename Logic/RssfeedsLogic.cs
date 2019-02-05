using FactFluxV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactFluxV3.Logic
{
    public class RssfeedsLogic
    {
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

            var wordList = db.Words.Where(x => x.Word == word || x.Word.StartsWith(beginning) || x.Word.EndsWith(end) || x.Word.Contains(middle)).OrderByDescending(x=>x.Word.Length).ToList();

            if (wordList.Count == 0)
            {
                var newWordLogic = new WordLogic();

                newWordLogic.CreateWord(word);
            }
            else
            {
                foreach(var phrase in wordList)
                {
                    if(newArticle.ArticleTitle.Contains(phrase.Word))
                    {
                        IncrementWordCount(phrase);

                        db.SaveChanges();

                        var newWordLogLogic = new WordLogLogic();

                        newWordLogLogic.CreateWordLog(phrase.WordId, newArticle.ArticleId);

                        break;
                    }
                }
            }
        }

        private void IncrementWordCount(Words isDupe)
        {
            if (isDupe.DateIncremented == null)
            {
                isDupe.DateIncremented = DateTime.UtcNow;
            }

            if (isDupe.DateIncremented.Value < DateTime.UtcNow.Date)
            {
                isDupe.Daily = 0;
            }

            if (isDupe.DateIncremented.Value.Month != DateTime.UtcNow.Month)
            {
                isDupe.Monthly = 0;
            }

            isDupe.Daily++;

            isDupe.Monthly++;

            isDupe.Yearly++;
        }
    }
}
