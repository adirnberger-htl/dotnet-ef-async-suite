# Asnychrone Programmierung: URL-Antwortzeit bestimmen
## Einleitung
In dieser Aufgabe wenden Sie den asynchronen Programmierstil in C# an, um eine gegebene Liste von URLs abzufragen und die Antwortzeit zu bestimmen. 

Das Programm `UrlResponseImgCount` erhält eine Liste von URLs über die Kommandozeile. Für jede dieser URLs bestimmt es, 
1. wie lange es dauert, bis die Antwort vom Server kommt, und
2. wie viele IMG-Tags in der Antwort enthalten sind.

## Verhalten des Programms
`UrlresponseImgCount` gibt die Daten auf auf der Konsole aus, wie im folgenden Beispiel zu sehen ist (ohne die Markierungen (1), (2), ...):
``` sh
(1) dotnet run http://nowhere.org https://www.startpage.com https://www.govt.nz/ https://www.htl-leonding.at/ https://www.orkney.com/ https://aka.ms/new-console-template https://tangodemayohotel.com-hotel.com/ 
Hello, World!
(2).Getting http://nowhere.org...
Getting https://www.startpage.com...
Getting https://www.htl-leonding.at/...
Getting https://www.govt.nz/...
Getting https://aka.ms/new-console-template...
Getting https://tangodemayohotel.com-hotel.com/...
Getting https://www.orkney.com/...
(3)..........
(4)Program took 2739 ms to run.
Sequentially, this would have taken 4696 ms.
(5)UrlResponseTimeImageCount { Url = https://tangodemayohotel.com-hotel.com/, ResponseTimeMs = 143, ImageCount = 0 }
UrlResponseTimeImageCount { Url = http://nowhere.org, ResponseTimeMs = 192, ImageCount = 1 }
UrlResponseTimeImageCount { Url = https://www.htl-leonding.at/, ResponseTimeMs = 256, ImageCount = 20 }
UrlResponseTimeImageCount { Url = https://www.orkney.com/, ResponseTimeMs = 275, ImageCount = 15 }
UrlResponseTimeImageCount { Url = https://aka.ms/new-console-template, ResponseTimeMs = 502, ImageCount = 3 }
UrlResponseTimeImageCount { Url = https://www.startpage.com, ResponseTimeMs = 599, ImageCount = 5 }
UrlResponseTimeImageCount { Url = https://www.govt.nz/, ResponseTimeMs = 2729, ImageCount = 15 }
```

<b> (1) Kommandozeile</b>

Die URLs werde über die Kommandozeile an das Programm gegeben.

<b>(2) Abfragen der URs </b>

Jede URL wird abgefragt. Wenn eine Antwort erhalten wurde, wird die Anzahl an `img` Tags im HTML ermittelt.

<b>(3) Spinner während der Abfrage </b>

Während das Programm auf die Antwort der Server wartet, wird alle 250ms ein Punkt auf die Konsole geschrieben.

<b>(4) Ausgabe der Gesamtdauer der Abfragen </b>

Wenn von allen Servern eine Antwort erhalten und ausgewertet wurde, wird die bis jetzt verstrichene Realzeit in ms ausgegeben.

<b>(5) Ausgabe der Ergebnisse der Auswertungen </b>

Am Ende werden die Daten der Auswertung mittels einfachem `ToString()` ausgegeben.
