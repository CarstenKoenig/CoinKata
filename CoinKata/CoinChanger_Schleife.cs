using System;
using System.Collections.Generic;
using System.Linq;

using Münze = System.Int32;
using Cents = System.Int32;
using AnzahlMünzePaar = System.Tuple<System.Int32, System.Int32>;
using Wechselgeld = System.Collections.Generic.IEnumerable<System.Tuple<System.Int32, System.Int32>>;


namespace CoinKata
{
    public class CoinChanger_Schleife
        : ICoinChanger
    {
        public Wechselgeld BerechneWechselgeld(IEnumerable<Münze> verfügbareMünzen, Cents betrag)
        {
            var münzen = verfügbareMünzen.OrderByDescending(i => i).ToArray();
            var ausgabe = new AnzahlMünzePaar[münzen.Length];

            var restBetrag = betrag;
            for (var i = 0; i < münzen.Length; i++)
            {
                var münze = münzen[i];
                var anzahl = Math.DivRem(restBetrag, münze, out restBetrag);
                ausgabe[i] = Tuple.Create(anzahl, münze);
            }

            return ausgabe;
        }
    }
}