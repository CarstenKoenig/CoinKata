## Beschreibung der Kata
Ziel dieser Kata ist es ein Programm zu schreiben, dass Wechselgeld berechnet.
Dazu wird dem Programm eine Liste mit Münz-Werte (zum Beispiel `[1, 2, 5, 10, 20, 50, 100, 200]` für unsere üblichen EUR-Münzen) und der zu wechselnde Betrag übergeben.
Das Programm soll darauf hin eine Liste von Anzahl/Münzwert-Paaren ausgeben, so dass der Betrag korrekt gewechselt wird und die Gesamtzahl der herausgegebenen Münzen minimiert wird.

### Beispiel
Es soll 2,53 EUR gewechselt werden.
Sowohl "1x2EUR, 1x50ct und 1x2ct, 1x1ct" als auch "2x1EUR, 2x20ct, 1x10ct, 3x1ct" würden den Betrag korrekt wechseln, aber die erste Möglichkeit gibt 4 Münzen zurück, während die Alternative 8 Münzen ausgeben würde. Das Programm soll in diesem Fall die erste Alternative zurückgeben (da diese hier die Optimallösung darstellt).

## sei "greedy"...
Wir wollen hier zunächst den naheliegensten Algorithmus verwenden, denn man beispielsweise auch an der Supermarktkasse beobachten kann und dessen Idee man prägnant so formulieren kann:
"für den noch zu wechselnden Restbetrag wähle die größte Münze die den Betrag nicht überschreitet und füge dieser der Rückgabe hinzu".
Sie werden es schon gemerkt haben: es handelt sich hierbei um einen sogenannten "gierigen" (engl. greedy) Algorithmus.

Übrigens: für unsere im Altag vertrauten Münzen liefert dieser Algorithmus immer das optimale Ergebnis. Der "Beweis" dieser Aussage würde aber den Artikel sprengen.

## TODO: es folgen die "greedy" Implementierungen
TODO:
 - [ ] naiv/Schleife in C#
 - [ ] mit LINQ/mutable in C# (?)
 - [ ] mit LINQ/immutable in C# (?)
 - [ ] in Javascript (?)
 - [ ] in Elexir (?)

## TODO: wann geht das Schief
## TODO: Brute-Force suche (in C# oder vielleicht F#?)
## TODO: wo geht das schief (schnell: wegen schlechter Laufzeit)
## TODO: Kurzeinführung dynamische Programmierung
## TOOD: Umsetzung dieser in Beliebiger Programmiersprache
## TODO: schöne Umsetzung dieser in Haskell (demonstration lazy)

Was meinst Du - in Ordnung so?

[CmuKata]: http://www.craftsmanship.sv.cmu.edu/exercises/coin-change-kata "Coin-Change-Kata der CMU"