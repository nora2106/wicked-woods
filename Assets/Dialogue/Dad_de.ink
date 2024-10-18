VAR story_name = "dad"
VAR dad_awake = false
VAR talked_to_dad = false
VAR door_inspected = false

//{ dad_awake:
    //-> talk_to_dad
//}
-> talk_to_dad

== talk_to_dad ==
Hatschi! Mishkaaa, meinn Kindd! Was m..machst duu denn hiier?
    + [Iva ist weg! Ich muss rausgehen und sie finden.]
        Nein, nein, nein! Das ist viel zu gef..gefäährlich. Du…du kannst gar nicht rausgehen. -> dad_about_door
    + [Warum war mein Zimmer abgesperrt, Papa?]   
        Ich versuche, euch zu beschützen! -> dad_about_door
    * {door_inspected }
        Papa, warum ist die Haustür verbarrikadiert?
        -> dad_about_door
    
== dad_about_door ==
       Ich habe aaalle Türen zu.. zugesperrt und gesichert, damit euch nichts passieren kann.
    + [So werden wir Iva niemals finden!]
        Niemand geht raus, bevor es nicht hell und sicher ist! Ich verliere nicht noch jemanden nach meiner süßen * hicks * Iva…
        + + [Schlaf weiter, Papa.]
            ~ dad_awake = false
            -> DONE
        + + [Wo ist der Schlüssel, Papa?]
            -> dad_about_key
    + [Wo ist der Schlüssel? Ich muss sie finden!]
    -> dad_about_key
    
== dad_about_key ==
Den wirst du niiiemals finden, ich habe ihn so * hicks * gut versteckt…  Und, und, es ist so ein gutes Versteck…ich musste mir sogar die Hände dreckig machen, aber ist ok… * gähn * Zzzzzz…
~ dad_awake = false
-> DONE
    