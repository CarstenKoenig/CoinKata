using System;
using System.Collections.Generic;
using System.Linq;

using Münze = System.Int32;
using Cents = System.Int32;
using AnzahlMünzePaar = System.Tuple<System.Int32, System.Int32>;
using Wechselgeld = System.Collections.Generic.IEnumerable<System.Tuple<System.Int32, System.Int32>>;

namespace CoinKata
{
    public class CoinChanger_BruteForce
        : ICoinChanger
    {
        public Wechselgeld BerechneWechselgeld(IEnumerable<Münze> verfügbareMünzen, Cents betrag)
        {
            var münzen = verfügbareMünzen.OrderByDescending(i => i).ToArray();
            var lösung = LöseRekursiv(münzen, 0, betrag);
            if (lösung == null) throw new Exception("keine Lösung gefunden");
            return lösung.Item2;
        }

        /// <summary>
        /// berechnet die Lösung rekursiv
        /// </summary>
        /// <remarks>
        /// eine Rückgabe von null bedeutet, dass es KEINE Lösung für das Teilproblem gibt!
        /// </remarks>
        private Tuple<int, Wechselgeld> LöseRekursiv(Münze[] münzen, int münzIndex, Cents restBetrag)
        {
            // keine weiteren Münzen vorhanden?
            if (münzIndex >= münzen.Length)
            {
                // Restbetrag = 0?
                if (restBetrag == 0) return Tuple.Create<int, Wechselgeld>(0, new AnzahlMünzePaar[] { });
                // sonst geht das sicher schief
                return null;
            }

            // es sind also noch Münzen vorhanden
            var aktuelleMünze = münzen[münzIndex];
            // wieviele Münzen könenn wir von der aktuellen maximal nehmen?
            var maxAnzahl = restBetrag / aktuelleMünze;

            // von 0 bis maxAnzahl müssen wir jede Möglichkeit testen und das Minimum suchen
            var minimaleAnzahl = Int32.MaxValue;
            Wechselgeld minimaleLösung = null;
            for (var anzahl = maxAnzahl; anzahl >= 0; anzahl--)
            {
                // mit der aktuellen Wahl haben wir schon herausgegeben:
                var betrag = anzahl * aktuelleMünze;
                // berechne rekursiv die Lösung des Restproblems:
                var restLösung = LöseRekursiv(münzen, münzIndex+1, restBetrag - betrag);
                // falls keine Lösung gefunden, weitermachen
                if (restLösung == null) continue;

                // falls die Lösung nicht verbessert wurde
                if (restLösung.Item1 + anzahl >= minimaleAnzahl) continue;

                minimaleAnzahl = restLösung.Item1 + anzahl;
                var gesamtLösung = new List<AnzahlMünzePaar> { Tuple.Create(anzahl, aktuelleMünze) };
                gesamtLösung.AddRange(restLösung.Item2);
                minimaleLösung = gesamtLösung;
            }

            if (minimaleLösung == null) return null;
            return Tuple.Create(minimaleAnzahl, minimaleLösung);
        }
    }
}
