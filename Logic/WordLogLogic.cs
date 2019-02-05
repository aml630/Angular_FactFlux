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
    }
}
