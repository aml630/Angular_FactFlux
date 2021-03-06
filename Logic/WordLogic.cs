﻿using FactFluxV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactFluxV3.Logic
{
    public class WordLogic
    {
        public Words CreateWord(string word)
        {
            using (var db = new DB_A41BC9_aml630Context())
            {
                Words newWord = new Words()
                {
                    Word = word,
                    Banned = false,
                    DateCreated = DateTime.UtcNow,
                    Weekly = 1,
                    Monthly = 1,
                    Yearly = 1,
                    Main = false,
                    DateIncremented = DateTime.UtcNow,
                };

                db.Words.Add(newWord);

                db.SaveChanges();

                return newWord;
            }
        }

        public Words GetWordByString(string word)
        {
            using (var db = new DB_A41BC9_aml630Context())
            {
                var wordRetrieved = db.Words.Where(x => x.Word == word).FirstOrDefault();

                return wordRetrieved;
            }
        }

    }
}
