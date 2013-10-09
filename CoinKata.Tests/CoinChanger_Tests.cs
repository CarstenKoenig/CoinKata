using System;
using FsCheck.Xunit;

namespace CoinKata.Tests
{
    public abstract class CoinChanger_Tests<tImplementierung>
        where tImplementierung : ICoinChanger, new ()
    {

        [Property(MaxTest = 500)]
        public bool BerechneWechselgeld_liefert_den_geforderten_Betrag(FsCheck.NonNegativeInt betrag)
        {
            var münzen = new[] { 1, 2, 5, 10, 20, 50, 100, 200 };
            var implementierung = new tImplementierung();

            return implementierung.BerechneWechselgeld(münzen, betrag.Get).Wechselbetrag() == betrag.Get;
        }
    }

}