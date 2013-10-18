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
 - [x] naiv/Schleife in C#
 - [x] mit LINQ/mutable in C# (?)
 - [x] mit LINQ/immutable in C# (?)
 - [ ] in Javascript (?)
 - [ ] in Elexir (?)

## wann geht das Schief
Der hier vorgestellte Algorithmus funktioniert korrekt - d.h. die Münzen entsprechen den Betrag - und liefert für die üblichen Münzen auch ein optimales Ergebnis.
Die Aufgabenstellung schreibt aber keine festen Münzen vor. Und in dieser allgemeinen Aufgabenstellung stößt ein greedy-Algorithmus an seine Grenzen.

### Gegenbeispiel 
Die Aufgabe soll es jetzt sein, mit 25ct, 10ct und 1ct Münzen 30ct zu wechseln.
Die Lösung ist natürlich 3x10ct.
Der Greedy-Algorithmus wird aber zu erst ein 25ct Stück wählen und hat dann nur noch die Möglichkeit mit fünf 1ct Münzen aufzufüllen - liefert also 6 anstelle der optimalen 3 Münzen.
In der Beispielsolutionen findet sich ein entsprechender Testfall und wie erwartet fallen alle bisher behandelten Algorithmen durch.

## Brute-Force suche
Der Algorithmus muss also in jeden Fall geändert werden - die Frage ist nur wie.
Eine sichere Lösung ist immer der sogenannte Brute-Force Algorithmus, bei dem einfach alle möglichen Lösungen (oder gar alle möglichen Kombinationen) versucht werden.
Die einzige Schwierigkeit besteht jetzt darin, alle möglichen korrekten (nicht unbedingt optimalen) Lösungen per Program aufzulisten.

In dieser Situation bietet sich immer die teile-und-herrsche Strategie an, die ein rekursiver Algorithmus mit sich bringt: Wenn bekannt ist, wie die Lösung für n Münzen und einen Betrag b berechnet wird,
ist es einfach die Lösung für n+1 Münzen zu finden: für jede mögliche Anzahl a der hinzugekommenen Münze m berechnen wird rekursiv die Lösung für die anderen Münzen, allerdings mit einem verringerten Betrag von b - a*n.

Der entsprechende Code in C# sieht dann so aus:

``` CSharp
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
```

und wie erwartet löst der Algorithmus den neuen Test von oben optimal.

## Brute-Force stößt an seine Grenze
Allerdings habe ich jetzt einen kleinen Kniff in die Tests eingebaut: für den Brute-Force-Algorithmus wird der geforderte Betrag deutlich eingeschränkt (unter 50ct), denn schon der Betrag von 1eur
mit unseren üblichen EUR-Münzen braucht auf meinem System [todo einfügen] Sekunden ... versuchen Sie mal 432.22eur wechseln zu lassen - kleiner Hinweis: Sie können sich einen Kaffee holen.

## TODO: Kurzeinführung dynamische Programmierung
## TOOD: Umsetzung dieser in Beliebiger Programmiersprache
## TODO: schöne Umsetzung dieser in Haskell (demonstration lazy)

Was meinst Du - in Ordnung so?

[CmuKata]: http://www.craftsmanship.sv.cmu.edu/exercises/coin-change-kata "Coin-Change-Kata der CMU"