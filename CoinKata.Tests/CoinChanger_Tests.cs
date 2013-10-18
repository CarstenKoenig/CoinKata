using System;
using FsCheck.Xunit;
using Xunit;
using FluentAssertions;

namespace CoinKata.Tests
{
    public abstract class CoinChanger_Tests<tImplementierung>
        where tImplementierung : ICoinChanger, new ()
    {
        private readonly int _maxBetrag;

        protected CoinChanger_Tests(int maxBetrag = 50000)
        {
            _maxBetrag = maxBetrag;
        }


        [Property(MaxTest = 500)]
        public bool BerechneWechselgeld_liefert_den_geforderten_Betrag(FsCheck.NonNegativeInt betrag)
        {
            var betragCent = betrag.Get%_maxBetrag; // zugroße Beträge umschlagen

            var münzen = new[] { 1, 2, 5, 10, 20, 50, 100, 200 };
            var implementierung = new tImplementierung();

            return implementierung.BerechneWechselgeld(münzen, betragCent).Wechselbetrag() == betragCent;
        }

        [Fact]
        public void Wechselgeld_für_2EUR47_sollte_1x2eur_2x20ct_1x5ct_und_1x2ct_sein()
        {
            var münzen = new[] { 1, 2, 5, 10, 20, 50, 100, 200 };
            const int betrag = 247;
            var erwartet = 
                new[] {1.mal(2.Euro()), 2.mal(20.Cent()), 1.mal(5.Cent()), 1.mal(2.Cent())}
                .ErgänzeMitNullen(münzen);

            var implementierung = new tImplementierung();
            var wechselgeld = implementierung.BerechneWechselgeld(münzen, betrag);

            wechselgeld.Should().BeEquivalentTo(erwartet);
        }

        /// <summary>
        /// dieser Test ist so angelegt, dass ein Greedy-Algorithmus versagen muss
        /// </summary>
        [Fact]
        public void Wechselgeld_für_30ct_mit_25ct_10ct_und_1ct_Münzen_sollte_3x10ct_sein()
        {
            var münzen = new[] { 25, 10, 1 };
            const int betrag = 30;
            var erwartet =
                new[] {3.mal(10.Cent())}
                .ErgänzeMitNullen(münzen);

            var implementierung = new tImplementierung();
            var wechselgeld = implementierung.BerechneWechselgeld(münzen, betrag);

            wechselgeld.Should().BeEquivalentTo(erwartet);
        }
    }
}