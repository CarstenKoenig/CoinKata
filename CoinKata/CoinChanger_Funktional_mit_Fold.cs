using System;
using System.Collections.Generic;
using System.Linq;

using Münze = System.Int32;
using Cents = System.Int32;
using AnzahlMünzePaar = System.Tuple<System.Int32, System.Int32>;
using Wechselgeld = System.Collections.Generic.IEnumerable<System.Tuple<System.Int32, System.Int32>>;

namespace CoinKata
{
    public class CoinChanger_Funktional_mit_Fold
        : ICoinChanger
    {
        public Wechselgeld BerechneWechselgeld(IEnumerable<Münze> verfügbareMünzen, Cents betrag)
        {
            var münzen = verfügbareMünzen.OrderByDescending(i => i);
            var initial = Tuple.Create((Wechselgeld) new AnzahlMünzePaar[] {}, betrag);
            var gewählteMünzen = münzen.Aggregate(initial, AusgabeUndRestbetragFürMünze);
            return gewählteMünzen.Item1;
        }

        private Tuple<Wechselgeld, Cents> AusgabeUndRestbetragFürMünze(Tuple<Wechselgeld, Cents> bisherUndRestbetrag, Münze aktuelleMünze)
        {
            var restbetrag = bisherUndRestbetrag.Item2;
            int neuerRest;
            var anzahl = Math.DivRem(restbetrag, aktuelleMünze, out neuerRest);
            var neuerWechsel = (Wechselgeld) new List<AnzahlMünzePaar>(bisherUndRestbetrag.Item1) {Tuple.Create(anzahl, aktuelleMünze)};
            return Tuple.Create(neuerWechsel, neuerRest);
        }
    }
}
