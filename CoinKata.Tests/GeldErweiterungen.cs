using System;
using System.Collections.Generic;
using System.Linq;

using Münze = System.Int32;
using Cents = System.Int32;
using AnzahlMünzePaar = System.Tuple<System.Int32, System.Int32>;
using Wechselgeld = System.Collections.Generic.IEnumerable<System.Tuple<System.Int32, System.Int32>>;


namespace CoinKata.Tests
{
    public static class GeldErweiterungen
    {
        public static AnzahlMünzePaar mal(this int anzahl, Münze münze)
        {
            return Tuple.Create(anzahl, münze);
        }

        public static Cents Euro(this int betragInEuro)
        {
            return betragInEuro*100;
        }

        public static Cents Cent(this int betragInCents)
        {
            return betragInCents;
        }

        public static Wechselgeld ErgänzeMitNullen(this Wechselgeld wechsel, IEnumerable<Münze> münzen)
        {
            var dictionary = wechsel.ToDictionary(w => w.Item2, w => w.Item1);
            foreach (var m in münzen.Where(m => !dictionary.ContainsKey(m)))
                dictionary.Add(m, 0);
            return dictionary.Select(kvp => Tuple.Create(kvp.Value, kvp.Key));
        }
    }
}