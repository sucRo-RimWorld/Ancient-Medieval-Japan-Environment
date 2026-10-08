Feature: AMJE downloaded Steam Workshop regression

  Scenario: Downloaded production Mod and assembly are the actual selected Steam root
    Then the installed Environment Mod and assembly come from the exact Steam folder

  Scenario: Four authored plant Defs have unique Workshop ownership
    Then all four Environment plants are uniquely owned by the downloaded Mod

  Scenario: Japan-oriented biome plant exclusion is loaded
    Then rejected Vanilla vegetation is absent from the selected Japanese biomes

  Scenario: Replacement native plants retain their tuned commonality
    Then the replacement structural plants preserve their balanced commonalities

  Scenario: Native wood harvest Defs match Vanilla or Medieval Overhaul
    Then the harvested wood Def contracts remain correct with or without Medieval Overhaul
