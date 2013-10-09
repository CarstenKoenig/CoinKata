using System;
using System.Linq;

using Münze = System.Int32;
using Cents = System.Int32;
using AnzahlMünzePaar = System.Tuple<System.Int32, System.Int32>;
using Wechselgeld = System.Collections.Generic.IEnumerable<System.Tuple<System.Int32, System.Int32>>;


namespace CoinKata
{
    public static class TypExtensions
    {
        public static int Anzahl(this AnzahlMünzePaar paar)
        {
            return paar.Item1;
        }

        public static Münze Münze(this AnzahlMünzePaar paar)
        {
            return paar.Item2;
        }

        public static Cents Wert(this AnzahlMünzePaar paar)
        {
            return paar.Anzahl()*paar.Münze();
        }

        public static Cents Wechselbetrag(this Wechselgeld wechselgeld)
        {
            return wechselgeld.Sum(paar => Wert(paar));
        }
    }
}