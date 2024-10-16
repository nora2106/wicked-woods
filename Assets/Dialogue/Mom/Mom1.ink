-> intro

=== intro ===
Hallo, mein Kind.
    + [Mama, warum weinst du?]
        Deine Schwester... sie ist verschwunden.
        + + [Was ist passiert?]
            -> ask_for_iva
        ++ [Ich werde sie finden.]
            Pass auf... -> talk_about_dad
    + [Wo ist Iva?]
        -> ask_for_iva

=== ask_for_iva ===
Sie ist gestern nicht mehr vom Spielen zurückgekommen... Niemand weiß, wo sie ist!
+ [Was soll ich tun?]
    Bitte hilf mir, sie zu finden. Aber pass auf.. -> talk_about_dad
    -> DONE
+ [Ich werde sie finden. ]
    Pass auf... -> talk_about_dad

=== talk_about_dad ===
Deinem Vater geht es nicht gut.
-> DONE

{talk_about_dad} -> standard
=== standard ===
Bitte hilf mir, Iva zu finden.
-> DONE