using System;
using System.Collections.Generic;
using Cents = System.Int32;
using Münze = System.Int32;
using Wechselgeld = System.Collections.Generic.IEnumerable<System.Tuple<System.Int32, System.Int32>>;


namespace CoinKata
{
    public interface ICoinChanger
    {
        Wechselgeld BerechneWechselgeld(IEnumerable<Münze> verfügbareMünzen, Cents betrag);
    }
}