using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.VtipBaze.Data;

namespace Altairis.VtipBaze.Import {
    class Program {
        static void Main(string[] args) {
            using (var dc = new VtipBazeContext()) {
                // Načíst celý obsah souboru do stringu (můžeme si to dovolit, 
                // protože víme, že je malý, jinak bychom ho načítali po částech)
                var s = File.ReadAllText("vtipy.txt", Encoding.GetEncoding("windows-1250"));

                // Rozdělit podle oddělovače, kterým je řádek obsahující jenom -----
                var items = s.Split(new string[] { "\r\n-----\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                // Projet postupně všechny položky
                for (int i = 0; i < items.Length; i++) {
                    // Načíst konkrétní vtip a normalizovat ho
                    var item = NormalizeItem(items[i]);
                    Console.WriteLine(item);

                    // Přidat do databáze
                    dc.Jokes.Add(new Joke {
                        Text = item,
                        DateCreated = DateTime.Now,
                        Approved = true, 
                    });
                    Console.WriteLine("-----");
                }

                dc.SaveChanges();
            }
        }

        private static string NormalizeItem(string item) {
            // Nahradit "-" na začátku řádku hvězdičkou (WikiPlex markup - dialog)
            item = "\r\n" + item;
            item = item.Replace("\r\n-", "\r\n* ");

            // Odstranit whitespace na začátku a konci
            item = item.Trim();

            // Odstranit nadbytečné mezery
            while (item.Contains("  ")) {
                item = item.Replace("  ", " ");
            }

            // Odstranit nadbytečné prázdné řádky
            //while (item.Contains("\r\n\r\n")) {
            //    item = item.Replace("\r\n\r\n", "\r\n");
            //}
            return item;
        }
    }

}
