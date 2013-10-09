using System;
using System.Collections.Generic;
using System.Linq;

using Münze = System.Int32;
using Cents = System.Int32;
using AnzahlMünzePaar = System.Tuple<System.Int32, System.Int32>;
using Wechselgeld = System.Collections.Generic.IEnumerable<System.Tuple<System.Int32, System.Int32>>;

namespace CoinKata
{
    public class CoinChanger_mutable_ParallelMap
        : ICoinChanger
    {
        public Wechselgeld BerechneWechselgeld(IEnumerable<Münze> verfügbareMünzen, Cents betrag)
        {
            var restBetrag = betrag;
            var münzen = verfügbareMünzen.OrderByDescending(i => i);
            return münzen
                .AsParallel()
                .Select(münze =>
                {
                    var res = Tuple.Create(restBetrag/münze, münze);
                    restBetrag %= münze;
                    return res;
                });
        }
    }
}